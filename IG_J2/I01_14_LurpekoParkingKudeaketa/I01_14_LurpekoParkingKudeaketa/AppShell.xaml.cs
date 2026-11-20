
using I01_14_LurpekoParkingKudeaketa.Kudeaketa;

namespace I01_14_LurpekoParkingKudeaketa;

/// <summary>
/// Bost pantailen nabigazioa eta partekatutako
/// aparkalekuaren kudeatzailea prestatzen ditu.
/// </summary>
public partial class AppShell : Shell
{
    /// <summary>
    /// Pantaila guztiek erabiltzen duten aparkalekuaren
    /// kudeatzaile partekatua eskuratzen du.
    /// </summary>
    public ParkingKudeatzailea Kudeatzailea { get; } =
        new(new ParkingDatuBasea());

    /// <summary>
    /// Aparkalekuaren nabigazio-egitura eta XAML osagaiak
    /// hasieratzen ditu.
    /// </summary>
    public AppShell()
    {
        InitializeComponent();
    }
}
