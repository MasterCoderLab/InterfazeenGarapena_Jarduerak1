using I01_14_LurpekoParkingKudeaketa.Ereduak;
using Microsoft.Maui.Storage;
using SQLite;

namespace I01_14_LurpekoParkingKudeaketa.Kudeaketa;

/// <summary>
/// Bezeroak, ibilgailuak, plazak eta aparkatzeak SQLite fitxategian gordetzen ditu.
/// Ereduak eta datu-baseko erregistroak bereizita mantentzen ditu.
/// </summary>
public class ParkingDatuBasea
{
    private readonly SQLiteConnection _db;

    /// <summary>
    /// Aplikazioaren SQLite konexioa ireki, taulak eta indizeak sortu eta hasierako datuak prestatzen ditu.
    /// </summary>
    public ParkingDatuBasea()
    {
        string bidea = Path.Combine(FileSystem.AppDataDirectory, "parking.db3");
        _db = new SQLiteConnection(bidea);
        _db.CreateTable<BezeroErregistroa>();
        _db.CreateTable<IbilgailuErregistroa>();
        _db.CreateTable<PlazaErregistroa>();
        _db.CreateTable<AparkatzeErregistroa>();
        _db.Execute("CREATE UNIQUE INDEX IF NOT EXISTS idx_ibilgailu_aktiboa " +
            "ON Aparkatzeak (Matrikula) WHERE IrteeraData IS NULL");
        _db.Execute("CREATE UNIQUE INDEX IF NOT EXISTS idx_plaza_aktiboa " +
            "ON Aparkatzeak (PlazaZenbakia) WHERE IrteeraData IS NULL");
        HasierakoDatuakGehitu();
    }

    /// <summary>
    /// Bezeroen taula hutsik dagoenean soilik kargatzen ditu probako datu koherenteak, transakzio bakarrean.
    /// </summary>
    private void HasierakoDatuakGehitu()
    {
        if (_db.Table<BezeroErregistroa>().Count() > 0)
        {
            return;
        }

        var adibideak = new DatuBiltegia();
        _db.RunInTransaction(() =>
        {
            foreach (var b in adibideak.Bezeroak)
            {
                _db.Insert(new BezeroErregistroa
                {
                    Nan = b.Nan, Izena = b.Izena, Abizenak = b.Abizenak,
                    Telefonoa = b.Telefonoa, Emaila = b.Emaila
                });
            }

            foreach (var i in adibideak.Ibilgailuak)
            {
                _db.Insert(new IbilgailuErregistroa
                {
                    Matrikula = i.Matrikula, Marka = i.Marka,
                    Modeloa = i.Modeloa, Kolorea = i.Kolorea,
                    JabeNan = i.Jabea.Nan, Mota = (int)i.Mota,
                    ElektrikoaDa = i.ElektrikoaDa
                });
            }

            foreach (var p in adibideak.Plazak)
            {
                _db.Insert(new PlazaErregistroa
                {
                    Zenbakia = p.Zenbakia, Solairua = p.Solairua,
                    Mota = (int)p.Mota, Egoera = (int)p.Egoera
                });
            }

            foreach (var a in adibideak.Aparkatzeak)
            {
                _db.Insert(new AparkatzeErregistroa
                {
                    Matrikula = a.Ibilgailua.Matrikula,
                    PlazaZenbakia = a.Plaza.Zenbakia,
                    SarreraData = a.SarreraData,
                    IrteeraData = a.IrteeraData,
                    OrdaindutakoZentimoak = a.OrdaindutakoZenbatekoa.HasValue
                        ? (int)Math.Round(a.OrdaindutakoZenbatekoa.Value * 100m)
                        : null
                });
            }
        });
    }

    /// <summary>
    /// Bezeroak irakurri eta izenaren arabera ordenatzen ditu.
    /// </summary>
    /// <returns>Bezeroen zerrenda.</returns>
    public List<Bezeroa> BezeroakLortu() => _db.Table<BezeroErregistroa>()
        .ToList()
        .OrderBy(b => b.Izena)
        .Select(b => new Bezeroa(b.Nan, b.Izena, b.Abizenak, b.Telefonoa, b.Emaila))
        .ToList();

    /// <summary>
    /// Ibilgailuak irakurri eta bakoitza dagokion bezeroarekin lotzen du.
    /// </summary>
    /// <returns>Matrikularen arabera ordenatutako ibilgailuak.</returns>
    public List<Ibilgailua> IbilgailuakLortu()
    {
        var bezeroak = BezeroakLortu().ToDictionary(b => b.Nan);
        return _db.Table<IbilgailuErregistroa>().ToList()
            .OrderBy(i => i.Matrikula)
            .Select(i => new Ibilgailua(i.Matrikula, i.Marka, i.Modeloa,
                i.Kolorea, bezeroak[i.JabeNan], (IbilgailuMota)i.Mota, i.ElektrikoaDa))
            .ToList();
    }

    /// <summary>
    /// Plazak irakurri eta aparkatze aktiboetako ibilgailuekin lotzen ditu.
    /// </summary>
    /// <param name="ezabatuakEre">Egia bada, logikoki ezabatutako plazak ere itzultzen ditu historikorako.</param>
    /// <returns>Zenbakiaren arabera ordenatutako plazak.</returns>
    public List<Plaza> PlazakLortu(bool ezabatuakEre = false)
    {
        var ibilgailuak = IbilgailuakLortu().ToDictionary(i => i.Matrikula);
        var okupazioak = _db.Table<AparkatzeErregistroa>().ToList()
            .Where(a => a.IrteeraData is null)
            .ToDictionary(a => a.PlazaZenbakia, a => a.Matrikula);

        return _db.Table<PlazaErregistroa>().ToList()
            .Where(p => ezabatuakEre || !p.Ezabatuta)
            .OrderBy(p => p.Zenbakia)
            .Select(p => new Plaza(p.Zenbakia, p.Solairua,
                (PlazaMota)p.Mota, (PlazaEgoera)p.Egoera,
                okupazioak.TryGetValue(p.Zenbakia, out var matrikula)
                    ? ibilgailuak[matrikula] : null))
            .ToList();
    }

    /// <summary>
    /// Aparkatzeak irakurri eta ibilgailu zein plaza objektuekin lotzen ditu, ezabatutako plazen erreferentziak mantenduz.
    /// </summary>
    /// <returns>Aparkatze aktiboen eta amaitutakoen zerrenda.</returns>
    public List<Aparkatzea> AparkatzeakLortu()
    {
        var ibilgailuak = IbilgailuakLortu().ToDictionary(i => i.Matrikula);
        var plazak = PlazakLortu(true).ToDictionary(p => p.Zenbakia);
        return _db.Table<AparkatzeErregistroa>().ToList()
            .Select(a => new Aparkatzea(ibilgailuak[a.Matrikula],
                plazak[a.PlazaZenbakia], a.SarreraData, a.IrteeraData,
                a.OrdaindutakoZentimoak.HasValue
                    ? a.OrdaindutakoZentimoak.Value / 100m : null))
            .ToList();
    }

    /// <summary>
    /// Matrikula bati lotutako irteera-datarik gabeko aparkatzea bilatzen du.
    /// </summary>
    /// <param name="matrikula">Kontsultatu nahi den ibilgailuaren matrikula.</param>
    /// <returns>Aparkatze aktiboa, edo null ez badago.</returns>
    public AparkatzeErregistroa? AparkatzeAktiboaLortu(string matrikula) =>
        _db.Table<AparkatzeErregistroa>().ToList()
            .FirstOrDefault(a => a.Matrikula == matrikula && a.IrteeraData is null);

    /// <summary>
    /// Sarrera eta plazaren okupazioa SQLite transakzio berean gordetzen ditu.
    /// </summary>
    /// <param name="matrikula">Sartzen den ibilgailuaren matrikula.</param>
    /// <param name="plazaZenbakia">Esleitutako plaza librearen identifikatzailea.</param>
    /// <param name="data">Sarrera-data eta ordua.</param>
    /// <exception cref="InvalidOperationException">Ibilgailuak aparkatze aktiboa badu edo plaza ez badago libre.</exception>
    public void SarreraGorde(string matrikula, string plazaZenbakia, DateTime data)
    {
        _db.RunInTransaction(() =>
        {
            if (AparkatzeAktiboaLortu(matrikula) is not null)
                throw new InvalidOperationException("Ibilgailuak dagoeneko aparkatze aktiboa dauka.");

            var plaza = _db.Find<PlazaErregistroa>(plazaZenbakia);
            if (plaza is null || plaza.Ezabatuta || plaza.Egoera != (int)PlazaEgoera.Libre)
                throw new InvalidOperationException("Proposatutako plaza ez dago libre.");

            _db.Insert(new AparkatzeErregistroa
            {
                Matrikula = matrikula,
                PlazaZenbakia = plazaZenbakia,
                SarreraData = data
            });
            plaza.Egoera = (int)PlazaEgoera.Okupatuta;
            _db.Update(plaza);
        });
    }

    /// <summary>
    /// Irteera-data, ordainketa eta plazaren askatzea transakzio berean gordetzen ditu.
    /// </summary>
    /// <param name="matrikula">Irteten den ibilgailuaren matrikula.</param>
    /// <param name="data">Irteera-data eta ordua.</param>
    /// <param name="zenbatekoa">Ordaindutako euroak; datu-basean zentimotan gordetzen dira.</param>
    /// <exception cref="InvalidOperationException">Aparkatze aktiborik ez badago, data baliogabea bada edo plaza aurkitzen ez bada.</exception>
    public void IrteeraGorde(string matrikula, DateTime data, decimal zenbatekoa)
    {
        _db.RunInTransaction(() =>
        {
            var aktiboa = AparkatzeAktiboaLortu(matrikula)
                ?? throw new InvalidOperationException("Ibilgailuak ez dauka aparkatze aktiborik.");
            if (data < aktiboa.SarreraData)
                throw new InvalidOperationException("Irteera-data ezin da sarrera baino lehenagokoa izan.");

            aktiboa.IrteeraData = data;
            aktiboa.OrdaindutakoZentimoak = (int)Math.Round(zenbatekoa * 100m);
            _db.Update(aktiboa);

            var plaza = _db.Find<PlazaErregistroa>(aktiboa.PlazaZenbakia)
                ?? throw new InvalidOperationException("Plaza ez da aurkitu.");
            plaza.Egoera = (int)PlazaEgoera.Libre;
            _db.Update(plaza);
        });
    }

    /// <summary>
    /// Plaza berri bat SQLite datu-basean gordetzen du.
    /// </summary>
    /// <param name="zenbakia">Plazaren identifikatzaile bakarra.</param>
    /// <param name="solairua">Plazaren solairua.</param>
    /// <param name="mota">Plazaren mota.</param>
    /// <param name="egoera">Plazaren hasierako egoera.</param>
    /// <exception cref="InvalidOperationException">Plaza-zenbakia dagoeneko existitzen bada.</exception>
    public void PlazaGehitu(string zenbakia, int solairua, PlazaMota mota, PlazaEgoera egoera)
    {
        if (_db.Find<PlazaErregistroa>(zenbakia) is not null)
            throw new InvalidOperationException("Plaza-zenbakia dagoeneko existitzen da.");
        _db.Insert(new PlazaErregistroa
        {
            Zenbakia = zenbakia, Solairua = solairua,
            Mota = (int)mota, Egoera = (int)egoera
        });
    }

    /// <summary>
    /// Plaza baten datuak eguneratzen ditu, egoera eta mota aldatzeko okupazio-mugak egiaztatuz.
    /// </summary>
    /// <param name="zenbakia">Aldatu beharreko plazaren identifikatzailea.</param>
    /// <param name="solairua">Solairu berria.</param>
    /// <param name="mota">Plaza mota berria.</param>
    /// <param name="egoera">Egoera berria.</param>
    /// <exception cref="InvalidOperationException">Plaza ez badago, ezabatuta badago edo okupazio-arauak urratzen badira.</exception>
    public void PlazaAldatu(string zenbakia, int solairua, PlazaMota mota, PlazaEgoera egoera)
    {
        var plaza = _db.Find<PlazaErregistroa>(zenbakia)
            ?? throw new InvalidOperationException("Plaza ez da aurkitu.");
        if (plaza.Ezabatuta)
            throw new InvalidOperationException("Ezabatutako plaza ezin da aldatu.");
        if (plaza.Egoera == (int)PlazaEgoera.Okupatuta
            && (egoera != PlazaEgoera.Okupatuta || mota != (PlazaMota)plaza.Mota))
            throw new InvalidOperationException("Okupatutako plazaren egoera eta mota ezin dira aldatu.");
        if (plaza.Egoera != (int)PlazaEgoera.Okupatuta && egoera == PlazaEgoera.Okupatuta)
            throw new InvalidOperationException("Okupazioa sarrera bidez erregistratu behar da.");

        plaza.Solairua = solairua;
        plaza.Mota = (int)mota;
        plaza.Egoera = (int)egoera;
        _db.Update(plaza);
    }

    /// <summary>
    /// Plaza logikoki ezabatzen du; historikoa gordetzeko errenkada fisikoa mantentzen da.
    /// </summary>
    /// <param name="zenbakia">Ezabatu beharreko plazaren identifikatzailea.</param>
    /// <exception cref="InvalidOperationException">Plaza ez badago edo okupatuta badago.</exception>
    public void PlazaEzabatu(string zenbakia)
    {
        var plaza = _db.Find<PlazaErregistroa>(zenbakia)
            ?? throw new InvalidOperationException("Plaza ez da aurkitu.");
        if (plaza.Egoera == (int)PlazaEgoera.Okupatuta)
            throw new InvalidOperationException("Okupatutako plaza ezin da ezabatu.");
        plaza.Ezabatuta = true;
        _db.Update(plaza);
    }
}

/// <summary>
/// Bezeroen SQLite taulako erregistroa. NANa da gako nagusia.
/// </summary>
[Table("Bezeroak")]
public class BezeroErregistroa
{
    [PrimaryKey] public string Nan { get; set; } = "";
    public string Izena { get; set; } = "";
    public string Abizenak { get; set; } = "";
    public string Telefonoa { get; set; } = "";
    public string Emaila { get; set; } = "";
}

/// <summary>
/// Ibilgailuen SQLite taulako erregistroa. JabeNan eremuak bezeroaren NANa gordetzen du.
/// </summary>
[Table("Ibilgailuak")]
public class IbilgailuErregistroa
{
    [PrimaryKey] public string Matrikula { get; set; } = "";
    public string Marka { get; set; } = "";
    public string Modeloa { get; set; } = "";
    public string Kolorea { get; set; } = "";
    public string JabeNan { get; set; } = "";
    public int Mota { get; set; }
    public bool ElektrikoaDa { get; set; }
}

/// <summary>
/// Plazen SQLite taulako erregistroa. Ezabatuta eremuak baja logikoa adierazten du.
/// </summary>
[Table("Plazak")]
public class PlazaErregistroa
{
    [PrimaryKey] public string Zenbakia { get; set; } = "";
    public int Solairua { get; set; }
    public int Mota { get; set; }
    public int Egoera { get; set; }
    public bool Ezabatuta { get; set; }
}

/// <summary>
/// Aparkatzeen SQLite taulako erregistroa. IrteeraData hutsik dagoenean aparkatzea aktiboa da.
/// </summary>
[Table("Aparkatzeak")]
public class AparkatzeErregistroa
{
    [PrimaryKey, AutoIncrement] public int Id { get; set; }
    public string Matrikula { get; set; } = "";
    public string PlazaZenbakia { get; set; } = "";
    public DateTime SarreraData { get; set; }
    public DateTime? IrteeraData { get; set; }
    /// <summary>Ordaindutako zenbatekoa zentimotan; null irteerarik ez bada.</summary>
    public int? OrdaindutakoZentimoak { get; set; }
}
