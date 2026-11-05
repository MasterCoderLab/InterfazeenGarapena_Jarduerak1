using I01_14_LurpekoParkingKudeaketa.Resources.Strings;

namespace I01_14_LurpekoParkingKudeaketa.Ereduak;

/// <summary>
/// Aparkalekuko plaza baten kokapena, mota eta egoera gordetzen ditu.
/// </summary>
public class Plaza
{
    public string Zenbakia { get; }
    public int Solairua { get; }
    public PlazaMota Mota { get; }
    public PlazaEgoera Egoera { get; }
    public Ibilgailua? Ibilgailua { get; }

    /// <summary>Plazaren motaren izena lokalizatzen du.</summary>
    public string MotaIzena => Mota switch
    {
        PlazaMota.Autoa => AppResources.Car,
        PlazaMota.Motorra => AppResources.Motorcycle,
        PlazaMota.MugikortasunUrrikoa => AppResources.Accessible,
        PlazaMota.Elektrikoa => AppResources.Electric,
        _ => Mota.ToString()
    };

    /// <summary>Plazaren egoeraren izena lokalizatzen du.</summary>
    public string EgoeraIzena => Egoera switch
    {
        PlazaEgoera.Libre => AppResources.Available,
        PlazaEgoera.Okupatuta => AppResources.Occupied,
        PlazaEgoera.Erreserbatuta => AppResources.Reserved,
        PlazaEgoera.MantentzeLanetan => AppResources.Maintenance,
        _ => Egoera.ToString()
    };

    /// <summary>Solairua erabiltzailearen hizkuntzan erakusten du.</summary>
    public string SolairuIzena => string.Format(AppResources.FloorFormat, Solairua);

    /// <summary>
    /// Plaza bat sortzen du eta okupazio-egoeraren koherentzia bermatzen du.
    /// </summary>
    /// <param name="zenbakia">Plazaren identifikatzailea.</param>
    /// <param name="solairua">Plaza dagoen solairua.</param>
    /// <param name="mota">Plazaren mota.</param>
    /// <param name="egoera">Plazaren une honetako egoera.</param>
    /// <param name="ibilgailua">Plazan dagoen ibilgailua, okupatuta badago.</param>
    /// <exception cref="ArgumentException">Okupazio-datuak bat ez datozenean.</exception>
    public Plaza(string zenbakia, int solairua, PlazaMota mota,
        PlazaEgoera egoera, Ibilgailua? ibilgailua = null)
    {
        if ((egoera == PlazaEgoera.Okupatuta) != (ibilgailua is not null))
        {
            throw new ArgumentException("Plazaren egoera eta ibilgailua ez datoz bat.");
        }

        Zenbakia = zenbakia;
        Solairua = solairua;
        Mota = mota;
        Egoera = egoera;
        Ibilgailua = ibilgailua;
    }
}
