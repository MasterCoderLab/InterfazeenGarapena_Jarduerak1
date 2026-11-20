using I01_14_LurpekoParkingKudeaketa.Ereduak;

namespace I01_14_LurpekoParkingKudeaketa.Kudeaketa;

/// <summary>
/// Aparkalekuaren kontsultak eta sarrera-, irteera- eta administrazio-arauak kudeatzen ditu.
/// </summary>
public class ParkingKudeatzailea
{
    private readonly ParkingDatuBasea _datuak;
    /// <summary>Adibideko tarifa: 1,80 euro orduko. Enuntziatuak ez du preziorik zehazten.</summary>
    private const decimal OrdukoTarifa = 1.80m;

    /// <summary>Pantailen artean aukeratutako ibilgailua partekatzen du.</summary>
    public Ibilgailua? HautatutakoIbilgailua { get; set; }

    /// <summary>
    /// Parkingaren kudeatzailea datu-base partekatuarekin sortzen du.
    /// </summary>
    /// <param name="datuak">Aparkalekuko datuak irakurri eta gordetzeko zerbitzua.</param>
    public ParkingKudeatzailea(ParkingDatuBasea datuak)
    {
        _datuak = datuak;
    }

    /// <summary>
    /// Plazak solairu, egoera eta motaren arabera iragazten ditu. Libreak soilik aukeratuz gero, egoera-iragazkia baztertzen du.
    /// </summary>
    /// <param name="solairua">Aukeratutako solairua; null balioak guztiak hartzen ditu.</param>
    /// <param name="egoera">Aukeratutako egoera; null balioak egoera guztiak hartzen ditu.</param>
    /// <param name="mota">Aukeratutako plaza mota; null balioak mota guztiak hartzen ditu.</param>
    /// <param name="libreakBakarrik">Egia bada, plaza libreak soilik erakusten ditu.</param>
    /// <returns>Iragazkiak betetzen dituzten plazen zerrenda.</returns>
    public IReadOnlyList<Plaza> PlazakIragazi(int? solairua,
        PlazaEgoera? egoera, PlazaMota? mota, bool libreakBakarrik)
    {
        return _datuak.PlazakLortu()
            .Where(p => !solairua.HasValue || p.Solairua == solairua.Value)
            .Where(p => !mota.HasValue || p.Mota == mota.Value)
            .Where(p => libreakBakarrik
                ? p.Egoera == PlazaEgoera.Libre
                : !egoera.HasValue || p.Egoera == egoera.Value)
            .ToList();
    }

    /// <summary>
    /// Matrikula, marka edo jabearen izenaren arabera bilatzen ditu ibilgailuak.
    /// </summary>
    /// <param name="testua">Bilatzeko testua; null edo hutsik bada, ibilgailu guztiak itzultzen ditu.</param>
    /// <returns>Bilaketarekin bat datozen ibilgailuak.</returns>
    public IReadOnlyList<Ibilgailua> IbilgailuakBilatu(string? testua)
    {
        string bilaketa = testua?.Trim() ?? "";
        return _datuak.IbilgailuakLortu()
            .Where(i => i.Matrikula.Contains(bilaketa, StringComparison.OrdinalIgnoreCase)
                || i.Marka.Contains(bilaketa, StringComparison.OrdinalIgnoreCase)
                || i.Jabea.IzenOsoa.Contains(bilaketa, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    /// <summary>
    /// Erregistratutako bezero guztiak eskuratzen ditu.
    /// </summary>
    /// <returns>Bezeroen zerrenda.</returns>
    public IReadOnlyList<Bezeroa> BezeroakLortu() => _datuak.BezeroakLortu();

    /// <summary>
    /// Bezero bati lotutako ibilgailuak eskuratzen ditu.
    /// </summary>
    /// <param name="bezeroa">Ibilgailuen jabea.</param>
    /// <returns>Bezero horren ibilgailuak.</returns>
    public IReadOnlyList<Ibilgailua> BezeroarenIbilgailuakLortu(Bezeroa bezeroa) =>
        _datuak.IbilgailuakLortu()
            .Where(i => i.Jabea.Nan == bezeroa.Nan).ToList();

    /// <summary>
    /// Ibilgailuaren aparkatzeak berrienetik zaharrenera antolatzen ditu.
    /// </summary>
    /// <param name="ibilgailua">Historikoa kontsultatu nahi den ibilgailua.</param>
    /// <returns>Aparkatzeen zerrenda kronologiko beherakorra.</returns>
    public IReadOnlyList<Aparkatzea> HistoriaBilatu(Ibilgailua ibilgailua) =>
        _datuak.AparkatzeakLortu()
            .Where(a => a.Ibilgailua.Matrikula == ibilgailua.Matrikula)
            .OrderByDescending(a => a.SarreraData).ToList();

    /// <summary>
    /// Matrikula normalizatu eta erregistratutako ibilgailua aurkitzen du.
    /// </summary>
    /// <param name="matrikula">Bilatu beharreko matrikula, maiuskulak eta zuriuneak normalizatuta.</param>
    /// <returns>Matrikulari dagokion ibilgailua.</returns>
    /// <exception cref="InvalidOperationException">Matrikula erregistratuta ez dagoenean.</exception>
    private Ibilgailua IbilgailuaAurkitu(string? matrikula)
    {
        string balioa = matrikula?.Trim().ToUpperInvariant() ?? "";
        return _datuak.IbilgailuakLortu()
            .FirstOrDefault(i => i.Matrikula == balioa)
            ?? throw new InvalidOperationException("Matrikula ez dago erregistratuta.");
    }

    /// <summary>
    /// Plaza mota ibilgailuarekin eta irisgarritasun-eskaerarekin bateragarria den zehazten du.
    /// </summary>
    /// <param name="ibilgailua">Sartu nahi den ibilgailua.</param>
    /// <param name="mota">Aztertu beharreko plaza mota.</param>
    /// <param name="irisgarria">Mugikortasun urriko plaza eskatu den.</param>
    /// <returns>Egia, plaza egokia bada; bestela, gezurra.</returns>
    private static bool Bateragarria(Ibilgailua ibilgailua, PlazaMota mota, bool irisgarria)
    {
        if (ibilgailua.Mota == IbilgailuMota.Motorra)
            return mota == PlazaMota.Motorra && !irisgarria;
        if (irisgarria)
            return mota == PlazaMota.MugikortasunUrrikoa;
        return mota == PlazaMota.Autoa || (ibilgailua.ElektrikoaDa && mota == PlazaMota.Elektrikoa);
    }

    /// <summary>
    /// Aparkatze aktiborik gabeko ibilgailuarentzat plaza libre bateragarria proposatzen du; plaza elektrikoak lehenesten ditu egokia denean.
    /// </summary>
    /// <param name="matrikula">Sartu nahi den ibilgailuaren matrikula.</param>
    /// <param name="irisgarria">Mugikortasun urriko plaza behar den.</param>
    /// <returns>Proposatutako plaza librea.</returns>
    /// <exception cref="InvalidOperationException">Matrikula ezezaguna denean, aparkatze aktiboa dagoenean edo plaza egokirik ez dagoenean.</exception>
    public Plaza PlazaProposatu(string? matrikula, bool irisgarria)
    {
        var ibilgailua = IbilgailuaAurkitu(matrikula);
        if (_datuak.AparkatzeAktiboaLortu(ibilgailua.Matrikula) is not null)
            throw new InvalidOperationException("Ibilgailuak dagoeneko aparkatze aktiboa dauka.");

        var aukerak = _datuak.PlazakLortu()
            .Where(p => p.Egoera == PlazaEgoera.Libre && Bateragarria(ibilgailua, p.Mota, irisgarria));

        var plaza = aukerak
            .OrderBy(p => ibilgailua.ElektrikoaDa && !irisgarria && p.Mota == PlazaMota.Elektrikoa ? 0 : 1)
            .ThenBy(p => p.Zenbakia)
            .FirstOrDefault();
        return plaza ?? throw new InvalidOperationException("Ez dago mota horretako plaza librerik.");
    }

    /// <summary>
    /// Proposatutako plaza egiaztatu eta ibilgailuaren sarrera gordetzen du.
    /// </summary>
    /// <param name="matrikula">Sartzen den ibilgailuaren matrikula.</param>
    /// <param name="plazaZenbakia">Esleitutako plazaren identifikatzailea.</param>
    /// <param name="irisgarria">Mugikortasun urriko plaza eskatu den.</param>
    /// <exception cref="InvalidOperationException">Ibilgailua edo plaza ez bada baliozkoa, edo aparkatze aktiboa badago.</exception>
    public void SarreraErregistratu(string? matrikula, string plazaZenbakia, bool irisgarria)
    {
        var ibilgailua = IbilgailuaAurkitu(matrikula);
        var plaza = _datuak.PlazakLortu().FirstOrDefault(p => p.Zenbakia == plazaZenbakia);
        if (plaza is null || plaza.Egoera != PlazaEgoera.Libre
            || !Bateragarria(ibilgailua, plaza.Mota, irisgarria))
            throw new InvalidOperationException("Plaza ez dago libre edo ez da bateragarria.");

        _datuak.SarreraGorde(ibilgailua.Matrikula, plaza.Zenbakia, DateTime.Now);
    }

    /// <summary>
    /// Egonaldiaren iraupena eta ordainketa kalkulatzen ditu, gutxienez minutu bat fakturatuz eta zentimora biribilduz.
    /// </summary>
    /// <param name="sarrera">Aparkatzearen hasierako data eta ordua.</param>
    /// <param name="irteera">Aparkatzearen amaierako data eta ordua.</param>
    /// <returns>Egonaldiaren iraupena eta eurotan kalkulatutako zenbatekoa.</returns>
    /// <exception cref="InvalidOperationException">Irteera sarrera baino lehenagokoa denean.</exception>
    private static (TimeSpan Iraupena, decimal Zenbatekoa) Kalkulatu(DateTime sarrera, DateTime irteera)
    {
        TimeSpan iraupena = irteera - sarrera;
        if (iraupena.TotalSeconds < 0)
            throw new InvalidOperationException("Irteera-data ezin da sarrera baino lehenagokoa izan.");
        decimal minutuak = Math.Max(1m, (decimal)iraupena.TotalMinutes);
        decimal zenbatekoa = Math.Round(minutuak * OrdukoTarifa / 60m,
            2, MidpointRounding.AwayFromZero);
        return (iraupena, zenbatekoa);
    }

    /// <summary>
    /// Une honetan irtengo balitz, egonaldiaren iraupena eta ordaindu beharrekoa kalkulatzen ditu, datu-basea aldatu gabe.
    /// </summary>
    /// <param name="matrikula">Irten nahi duen ibilgailuaren matrikula.</param>
    /// <returns>Egonaldiaren iraupena, zenbatekoa eurotan eta erabilitako plaza.</returns>
    /// <exception cref="InvalidOperationException">Ibilgailua ezezaguna denean edo aparkatze aktiborik ez duenean.</exception>
    public (TimeSpan Iraupena, decimal Zenbatekoa, string Plaza) IrteeraAurreikusi(string? matrikula)
    {
        var ibilgailua = IbilgailuaAurkitu(matrikula);
        var aktiboa = _datuak.AparkatzeAktiboaLortu(ibilgailua.Matrikula)
            ?? throw new InvalidOperationException("Ibilgailuak ez dauka aparkatze aktiborik.");
        var emaitza = Kalkulatu(aktiboa.SarreraData, DateTime.Now);
        return (emaitza.Iraupena, emaitza.Zenbatekoa, aktiboa.PlazaZenbakia);
    }

    /// <summary>
    /// Ibilgailuaren irteera eta ordainketa gordetzen ditu, plaza libre utziz.
    /// </summary>
    /// <param name="matrikula">Irteten den ibilgailuaren matrikula.</param>
    /// <returns>Gordetako ordainketaren zenbatekoa eurotan.</returns>
    /// <exception cref="InvalidOperationException">Ibilgailua ezezaguna denean, aparkatze aktiborik ez duenean edo data baliogabea denean.</exception>
    public decimal IrteeraErregistratu(string? matrikula)
    {
        var ibilgailua = IbilgailuaAurkitu(matrikula);
        var aktiboa = _datuak.AparkatzeAktiboaLortu(ibilgailua.Matrikula)
            ?? throw new InvalidOperationException("Ibilgailuak ez dauka aparkatze aktiborik.");
        DateTime orain = DateTime.Now;
        var emaitza = Kalkulatu(aktiboa.SarreraData, orain);
        _datuak.IrteeraGorde(ibilgailua.Matrikula, orain, emaitza.Zenbatekoa);
        return emaitza.Zenbatekoa;
    }

    /// <summary>
    /// Plaza berri baten datuak balioztatu eta gordetzen ditu.
    /// </summary>
    /// <param name="zenbakia">Plazaren identifikatzaile bakarra.</param>
    /// <param name="solairua">Plaza dagoen solairua.</param>
    /// <param name="mota">Plazaren mota.</param>
    /// <param name="egoera">Hasierako egoera; ezin da Okupatuta izan.</param>
    /// <exception cref="InvalidOperationException">Zenbakia hutsa, errepikatua edo hasierako egoera baliogabea denean.</exception>
    public void PlazaGehitu(string zenbakia, int solairua, PlazaMota mota, PlazaEgoera egoera)
    {
        if (string.IsNullOrWhiteSpace(zenbakia))
            throw new InvalidOperationException("Plaza-zenbakia sartu behar da.");
        if (egoera == PlazaEgoera.Okupatuta)
            throw new InvalidOperationException("Plaza berri bat ezin da zuzenean okupatuta sortu.");
        _datuak.PlazaGehitu(zenbakia.Trim().ToUpperInvariant(), solairua, mota, egoera);
    }

    /// <summary>
    /// Dagoen plaza baten kokapena, mota eta egoera eguneratzen ditu, okupazio-arauak errespetatuz.
    /// </summary>
    /// <param name="zenbakia">Aldatu beharreko plazaren identifikatzailea.</param>
    /// <param name="solairua">Solairu berria.</param>
    /// <param name="mota">Plaza mota berria.</param>
    /// <param name="egoera">Plazaren egoera berria.</param>
    /// <exception cref="InvalidOperationException">Plaza aurkitzen ez bada, ezabatuta badago edo aldaketak okupazio-arauak urratzen baditu.</exception>
    public void PlazaAldatu(string zenbakia, int solairua, PlazaMota mota, PlazaEgoera egoera)
    {
        _datuak.PlazaAldatu(zenbakia, solairua, mota, egoera);
    }

    /// <summary>
    /// Libre edo bestelako egoera egokian dagoen plaza mantentze-lanetan jartzen du.
    /// </summary>
    /// <param name="zenbakia">Mantentze-lanetan jarri beharreko plazaren identifikatzailea.</param>
    /// <exception cref="InvalidOperationException">Plaza ez badago edo okupatuta badago.</exception>
    public void MantentzeraJarri(string zenbakia)
    {
        var plaza = _datuak.PlazakLortu().FirstOrDefault(p => p.Zenbakia == zenbakia)
            ?? throw new InvalidOperationException("Plaza ez da aurkitu.");
        PlazaAldatu(zenbakia, plaza.Solairua, plaza.Mota, PlazaEgoera.MantentzeLanetan);
    }

    /// <summary>
    /// Plaza ezabatuta markatzen du, historikoa mantenduta; okupatutako plaza ezabatzea eragozten du datu-baseak.
    /// </summary>
    /// <param name="zenbakia">Ezabatu nahi den plazaren identifikatzailea.</param>
    /// <exception cref="InvalidOperationException">Plaza ez bada existitzen edo okupatuta badago.</exception>
    public void PlazaEzabatu(string zenbakia) => _datuak.PlazaEzabatu(zenbakia);
}
