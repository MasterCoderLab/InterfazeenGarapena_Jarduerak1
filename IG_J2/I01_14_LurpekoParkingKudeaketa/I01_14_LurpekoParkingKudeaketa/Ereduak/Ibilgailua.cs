namespace I01_14_LurpekoParkingKudeaketa.Ereduak;

/// <summary>
/// Ibilgailuaren datuak eta haren jabea gordetzen ditu.
/// </summary>
public class Ibilgailua
{
    public string Matrikula { get; }
    public string Marka { get; }
    public string Modeloa { get; }
    public string Kolorea { get; }
    public Bezeroa Jabea { get; }
    public IbilgailuMota Mota { get; }
    public bool ElektrikoaDa { get; }

    /// <summary>
    /// Ibilgailu bat sortzen du; matrikula normalizatzen du bilaketak errazteko.
    /// </summary>
    /// <param name="matrikula">Ibilgailuaren matrikula.</param>
    /// <param name="marka">Ibilgailuaren marka.</param>
    /// <param name="modeloa">Ibilgailuaren modeloa.</param>
    /// <param name="kolorea">Ibilgailuaren kolorea.</param>
    /// <param name="jabea">Ibilgailuaren jabea.</param>
    /// <param name="mota">Autoa edo motorra.</param>
    /// <param name="elektrikoaDa">Ibilgailua elektrikoa den adierazten du.</param>
    public Ibilgailua(string matrikula, string marka, string modeloa,
        string kolorea, Bezeroa jabea, IbilgailuMota mota, bool elektrikoaDa)
    {
        Matrikula = matrikula.Trim().ToUpperInvariant();
        Marka = marka;
        Modeloa = modeloa;
        Kolorea = kolorea;
        Jabea = jabea;
        Mota = mota;
        ElektrikoaDa = elektrikoaDa;
    }
}
