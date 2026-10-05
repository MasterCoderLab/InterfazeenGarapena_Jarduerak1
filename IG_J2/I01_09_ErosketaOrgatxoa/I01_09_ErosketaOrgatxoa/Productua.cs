namespace I01_09_ErosketaOrgatxoa;

/// <summary>
/// Erosketa-orgatxoko produktu baten datuak gordetzen dituen klasea.
/// Produktu bakoitzak izena, irudia eta unitate kopurua ditu.
/// </summary>
public class Produktua
{
    /// <summary>
    /// Produktuaren izena.
    /// </summary>
    public string Izena { get; set; }

    /// <summary>
    /// Produktuaren irudi-fitxategiaren izena.
    /// </summary>
    public string Irudia { get; set; }

    /// <summary>
    /// Orgatxoan dagoen produktuaren unitate kopurua.
    /// </summary>
    public int Kopurua { get; set; }

    /// <summary>
    /// Produktu berri bat sortzen du.
    /// </summary>
    /// <param name="izena">Produktuaren izena.</param>
    /// <param name="irudia">Produktuaren irudi-fitxategia.</param>
    /// <param name="kopurua">Hasierako unitate kopurua.</param>
    public Produktua(string izena, string irudia, int kopurua = 1)
    {
        Izena = izena;
        Irudia = irudia;
        Kopurua = kopurua;
    }
}