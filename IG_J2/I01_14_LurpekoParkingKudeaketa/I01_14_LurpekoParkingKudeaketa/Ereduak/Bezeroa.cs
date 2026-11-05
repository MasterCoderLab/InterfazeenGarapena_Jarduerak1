namespace I01_14_LurpekoParkingKudeaketa.Ereduak;

/// <summary>
/// Bezeroaren identifikazio- eta harreman-datuak gordetzen ditu.
/// </summary>
public class Bezeroa
{
    public string Nan { get; }
    public string Izena { get; }
    public string Abizenak { get; }
    public string Telefonoa { get; }
    public string Emaila { get; }

    public string IzenOsoa => $"{Izena} {Abizenak}";

    /// <summary>
    /// Bezero bat sortzen du emandako datuekin.
    /// </summary>
    /// <param name="nan">Bezeroaren NANa.</param>
    /// <param name="izena">Bezeroaren izena.</param>
    /// <param name="abizenak">Bezeroaren abizenak.</param>
    /// <param name="telefonoa">Harremanetarako telefonoa.</param>
    /// <param name="emaila">Helbide elektronikoa.</param>
    public Bezeroa(string nan, string izena, string abizenak,
        string telefonoa, string emaila)
    {
        Nan = nan;
        Izena = izena;
        Abizenak = abizenak;
        Telefonoa = telefonoa;
        Emaila = emaila;
    }
}
