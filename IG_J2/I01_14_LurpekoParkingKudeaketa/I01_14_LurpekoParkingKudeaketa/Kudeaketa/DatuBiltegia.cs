using I01_14_LurpekoParkingKudeaketa.Ereduak;

namespace I01_14_LurpekoParkingKudeaketa.Kudeaketa;

/// <summary>
/// Bi pantailak probatzeko memoria barruko adibide-datuak prestatzen ditu.
/// Datuak ez dira oraindik diskoan gordetzen.
/// </summary>
public class DatuBiltegia
{
    public IReadOnlyList<Bezeroa> Bezeroak { get; }
    public IReadOnlyList<Ibilgailua> Ibilgailuak { get; }
    public IReadOnlyList<Plaza> Plazak { get; }
    public IReadOnlyList<Aparkatzea> Aparkatzeak { get; }

    /// <summary>
    /// Elkarren artean koherenteak diren bezero, ibilgailu, plaza eta erregistroak sortzen ditu.
    /// </summary>
    public DatuBiltegia()
    {
        var ane = new Bezeroa("00000000T", "Ane", "Etxeberria",
            "600000001", "ane@example.com");
        var jon = new Bezeroa("00000001R", "Jon", "Aranburu",
            "600000002", "jon@example.com");
        var maite = new Bezeroa("00000002W", "Maite", "Zubizarreta",
            "600000003", "maite@example.com");
        Bezeroak = new List<Bezeroa> { ane, jon, maite };

        var autoa = new Ibilgailua("1234ABC", "SEAT", "León",
            "Gorria", ane, IbilgailuMota.Autoa, false);
        var tesla = new Ibilgailua("5678DEF", "Tesla", "Model 3",
            "Zuria", jon, IbilgailuMota.Autoa, true);
        var motorra = new Ibilgailua("2222GHI", "Honda", "CB500",
            "Beltza", ane, IbilgailuMota.Motorra, false);
        var zoe = new Ibilgailua("4444JKL", "Renault", "Zoe",
            "Urdina", maite, IbilgailuMota.Autoa, true);
        Ibilgailuak = new List<Ibilgailua> { autoa, tesla, motorra, zoe };

        var p01 = new Plaza("P01", -1, PlazaMota.Autoa, PlazaEgoera.Okupatuta, autoa);
        var p02 = new Plaza("P02", -1, PlazaMota.Autoa, PlazaEgoera.Libre);
        var p03 = new Plaza("P03", -1, PlazaMota.Elektrikoa, PlazaEgoera.Okupatuta, tesla);
        var p04 = new Plaza("P04", -1, PlazaMota.Motorra, PlazaEgoera.Libre);
        var p05 = new Plaza("P05", -1, PlazaMota.MugikortasunUrrikoa, PlazaEgoera.Libre);
        var p06 = new Plaza("P06", -1, PlazaMota.Autoa, PlazaEgoera.MantentzeLanetan);
        var p07 = new Plaza("P07", -2, PlazaMota.Autoa, PlazaEgoera.Erreserbatuta);
        var p08 = new Plaza("P08", -2, PlazaMota.Elektrikoa, PlazaEgoera.Libre);
        var p09 = new Plaza("P09", -2, PlazaMota.Motorra, PlazaEgoera.Okupatuta, motorra);
        var p10 = new Plaza("P10", -2, PlazaMota.Autoa, PlazaEgoera.Libre);
        var p11 = new Plaza("P11", -2, PlazaMota.Autoa, PlazaEgoera.Libre);
        var p12 = new Plaza("P12", -2, PlazaMota.Elektrikoa, PlazaEgoera.Libre);
        Plazak = new List<Plaza>
        {
            p01, p02, p03, p04, p05, p06, p07, p08, p09, p10, p11, p12
        };

        var orain = DateTime.Now;
        var iragana = DateTime.Today.AddDays(-8);
        Aparkatzeak = new List<Aparkatzea>
        {
            new(autoa, p01, orain.AddHours(-2)),
            new(tesla, p03, orain.AddHours(-1)),
            new(motorra, p09, orain.AddMinutes(-40)),
            new(autoa, p10, iragana.AddHours(9), iragana.AddHours(12), 5.40m),
            new(tesla, p08, iragana.AddDays(2).AddHours(10),
                iragana.AddDays(2).AddHours(11), 2.00m),
            new(zoe, p08, iragana.AddDays(4).AddHours(14),
                iragana.AddDays(4).AddHours(16), 3.60m)
        };
    }
}
