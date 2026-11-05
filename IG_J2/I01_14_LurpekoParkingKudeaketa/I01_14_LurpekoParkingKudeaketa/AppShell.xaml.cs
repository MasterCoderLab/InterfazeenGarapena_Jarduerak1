using I01_14_LurpekoParkingKudeaketa.Kudeaketa;

namespace I01_14_LurpekoParkingKudeaketa;

/// <summary>
/// Bi pantailen nabigazioa eta partekatutako kudeatzailea prestatzen ditu.
/// </summary>
public partial class AppShell : Shell
{
    public ParkingKudeatzailea Kudeatzailea { get; } = new(new DatuBiltegia());

    /// <summary>
    /// Shell nabigazioa hasieratzen du.
    /// </summary>
    public AppShell()
    {
        InitializeComponent();
    }
}
