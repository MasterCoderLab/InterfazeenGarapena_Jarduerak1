using I01_14_LurpekoParkingKudeaketa.Ereduak;
using I01_14_LurpekoParkingKudeaketa.Kudeaketa;
using Microsoft.Maui.Devices;

namespace I01_14_LurpekoParkingKudeaketa.Pantailak;

/// <summary>
/// Bezeroen zerrenda, haien datuak eta ibilgailuak erakusten ditu.
/// </summary>
public partial class BezeroakPage : ContentPage
{
    private bool? _diseinuEstua;

    private ParkingKudeatzailea Kudeatzailea =>
        ((AppShell)Shell.Current).Kudeatzailea;

    /// <summary>
    /// Bezeroen pantailako XAML osagaiak hasieratzen ditu.
    /// </summary>
    public BezeroakPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Tabletetan zerrenda ezkerrean eta fitxak eskuinean kokatzen ditu;
    /// telefonoetan, berriz, bata bestearen azpian.
    /// </summary>
    /// <param name="width">Pantailaren zabalera erabilgarria, unitate logikoetan.</param>
    /// <param name="height">Pantailaren altuera erabilgarria, unitate logikoetan.</param>
    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        bool estua = width < 800 || DeviceInfo.Current.Idiom == DeviceIdiom.Phone;
        if (_diseinuEstua == estua)
        {
            return;
        }

        _diseinuEstua = estua;
        DiseinuNagusia.Padding = new Thickness(estua ? 10 : 18);
        Izenburua.FontSize = estua ? 21 : 25;

        Grid.SetRow(BezeroZerrendaPanela, 1);
        Grid.SetColumn(BezeroZerrendaPanela, 0);
        Grid.SetRowSpan(BezeroZerrendaPanela, estua ? 1 : 2);
        Grid.SetColumnSpan(BezeroZerrendaPanela, estua ? 2 : 1);

        Grid.SetRow(XehetasunSarea, estua ? 2 : 1);
        Grid.SetColumn(XehetasunSarea, estua ? 0 : 1);
        Grid.SetRowSpan(XehetasunSarea, estua ? 1 : 2);
        Grid.SetColumnSpan(XehetasunSarea, estua ? 2 : 1);
    }

    /// <summary>
    /// Bezeroen zerrenda kargatzen du pantaila irekitzean.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BezeroZerrenda.ItemsSource = Kudeatzailea.BezeroakLortu();
    }

    /// <summary>
    /// Hautatutako bezeroaren datuak eta ibilgailuak erakusten ditu.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen kontrola.</param>
    /// <param name="e">Hautatutako bezeroaren informazioa.</param>
    private void BezeroaHautatua(object? sender, SelectionChangedEventArgs e)
    {
        var bezeroa = e.CurrentSelection.FirstOrDefault() as Bezeroa;

        if (bezeroa is null)
        {
            BezeroXehetasunak.Text = "Hautatu bezero bat.";
            IbilgailuZerrenda.ItemsSource = null;
            return;
        }

        BezeroXehetasunak.Text =
            $"NAN: {bezeroa.Nan}\n" +
            $"Izena: {bezeroa.Izena}\n" +
            $"Abizenak: {bezeroa.Abizenak}\n" +
            $"Telefonoa: {bezeroa.Telefonoa}\n" +
            $"Emaila: {bezeroa.Emaila}";

        IbilgailuZerrenda.ItemsSource =
            Kudeatzailea.BezeroarenIbilgailuakLortu(bezeroa);
    }

    /// <summary>
    /// Bi aldiz sakatutako ibilgailuaren fitxara nabigatzen du.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen kontrola.</param>
    /// <param name="e">Bi aldiz sakatutako ibilgailua duen gertaera.</param>
    private async void IbilgailuaBiAldizSakatuta(object? sender, TappedEventArgs e)
    {
        if (e.Parameter is not Ibilgailua ibilgailua)
        {
            return;
        }

        Kudeatzailea.HautatutakoIbilgailua = ibilgailua;
        await Shell.Current.GoToAsync("//nagusia/ibilgailuTab/ibilgailuak");
    }
}
