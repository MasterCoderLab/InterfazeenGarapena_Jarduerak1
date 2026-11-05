using I01_14_LurpekoParkingKudeaketa.Ereduak;

namespace I01_14_LurpekoParkingKudeaketa.Kudeaketa;

/// <summary>
/// Bi pantailen kontsultak kudeatzen ditu, aurkezpen-kodea eta datuak bereiziz.
/// Sarrera, irteera eta administrazio-eragiketak hurrengo faseetarako geratzen dira.
/// </summary>
public class ParkingKudeatzailea
{
    private readonly DatuBiltegia _datuak;

    public Ibilgailua? HautatutakoIbilgailua { get; set; }

    /// <summary>
    /// Datu-biltegi partekatuarekin kudeatzailea sortzen du.
    /// </summary>
    /// <param name="datuak">Kontsultatu beharreko datu-biltegia.</param>
    public ParkingKudeatzailea(DatuBiltegia datuak)
    {
        _datuak = datuak;
    }

    /// <summary>
    /// Plazak solairuaren, egoeraren eta motaren arabera iragazten ditu.
    /// Libreko aukera aktibatuta dagoenean, egoera-iragazkiari lehentasuna kentzen dio.
    /// Ez ditu jatorrizko datuak aldatzen.
    /// </summary>
    /// <param name="solairua">Aukeratutako solairua edo null denak ikusteko.</param>
    /// <param name="egoera">Aukeratutako egoera edo null denak ikusteko.</param>
    /// <param name="mota">Aukeratutako mota edo null denak ikusteko.</param>
    /// <param name="libreakBakarrik">Plaza libreak soilik erakutsi behar diren.</param>
    /// <returns>Iragazkiak betetzen dituzten plazen zerrenda berria.</returns>
    public IReadOnlyList<Plaza> PlazakIragazi(int? solairua,
        PlazaEgoera? egoera, PlazaMota? mota, bool libreakBakarrik)
    {
        return _datuak.Plazak
            .Where(p => !solairua.HasValue || p.Solairua == solairua.Value)
            .Where(p => !mota.HasValue || p.Mota == mota.Value)
            .Where(p => libreakBakarrik
                ? p.Egoera == PlazaEgoera.Libre
                : !egoera.HasValue || p.Egoera == egoera.Value)
            .ToList();
    }

    /// <summary>
    /// Matrikula, marka edo jabearen izenean testua bilatzen du.
    /// Maiuskulak eta minuskulak berdin tratatzen ditu.
    /// </summary>
    /// <param name="testua">Bilatu beharreko testua; hutsik bada, denak erakusten dira.</param>
    /// <returns>Bilaketarekin bat datozen ibilgailuak.</returns>
    public IReadOnlyList<Ibilgailua> IbilgailuakBilatu(string? testua)
    {
        var bilaketa = testua?.Trim();
        if (string.IsNullOrEmpty(bilaketa))
        {
            return _datuak.Ibilgailuak.ToList();
        }

        return _datuak.Ibilgailuak
            .Where(i => i.Matrikula.Contains(bilaketa, StringComparison.OrdinalIgnoreCase)
                || i.Marka.Contains(bilaketa, StringComparison.OrdinalIgnoreCase)
                || i.Jabea.IzenOsoa.Contains(bilaketa, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    /// <summary>
    /// Ibilgailu bati dagozkion aparkatzeak berrienetik zaharrenera ordenatzen ditu.
    /// </summary>
    /// <param name="ibilgailua">Kontsultatu beharreko ibilgailua.</param>
    /// <returns>Ibilgailuaren aparkatze-erregistroak.</returns>
    public IReadOnlyList<Aparkatzea> HistoriaBilatu(Ibilgailua ibilgailua)
    {
        return _datuak.Aparkatzeak
            .Where(a => ReferenceEquals(a.Ibilgailua, ibilgailua))
            .OrderByDescending(a => a.SarreraData)
            .ToList();
    }
}
