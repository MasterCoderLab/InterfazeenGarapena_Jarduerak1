namespace I01_14_LurpekoParkingKudeaketa.Ereduak;

/// <summary>
/// Ibilgailu baten aparkatze historikoa edo une honetako aparkatzea gordetzen du.
/// </summary>
public class Aparkatzea
{
    public Ibilgailua Ibilgailua { get; }
    public Plaza Plaza { get; }
    public DateTime SarreraData { get; }
    public DateTime? IrteeraData { get; }
    public decimal? OrdaindutakoZenbatekoa { get; }

    public bool AktiboDa => IrteeraData is null;

    /// <summary>
    /// Aparkatze-erregistro bat sortzen du, datak eta ordainketa koherenteak direla egiaztatuz.
    /// </summary>
    /// <param name="ibilgailua">Aparkatu den ibilgailua.</param>
    /// <param name="plaza">Erabilitako plaza.</param>
    /// <param name="sarreraData">Sarrera-eguna eta ordua.</param>
    /// <param name="irteeraData">Irteera-eguna eta ordua, amaitu bada.</param>
    /// <param name="ordaindutakoZenbatekoa">Ordaindutako euroak, amaitu bada.</param>
    /// <exception cref="ArgumentException">Data edo ordainketa inkohereentea denean.</exception>
    public Aparkatzea(Ibilgailua ibilgailua, Plaza plaza, DateTime sarreraData,
        DateTime? irteeraData = null, decimal? ordaindutakoZenbatekoa = null)
    {
        if (irteeraData.HasValue && irteeraData.Value < sarreraData)
        {
            throw new ArgumentException("Irteera ezin da sarrera baino lehenagokoa izan.");
        }

        if (irteeraData.HasValue != ordaindutakoZenbatekoa.HasValue
            || ordaindutakoZenbatekoa < 0)
        {
            throw new ArgumentException("Irteera eta ordainketa koherenteak izan behar dira.");
        }

        Ibilgailua = ibilgailua;
        Plaza = plaza;
        SarreraData = sarreraData;
        IrteeraData = irteeraData;
        OrdaindutakoZenbatekoa = ordaindutakoZenbatekoa;
    }
}
