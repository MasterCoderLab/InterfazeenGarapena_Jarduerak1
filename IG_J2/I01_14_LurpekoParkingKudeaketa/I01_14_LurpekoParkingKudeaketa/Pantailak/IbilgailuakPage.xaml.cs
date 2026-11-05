using I01_14_LurpekoParkingKudeaketa.Ereduak;
using I01_14_LurpekoParkingKudeaketa.Kudeaketa;
using I01_14_LurpekoParkingKudeaketa.Resources.Strings;
using Microsoft.Maui.Devices;

namespace I01_14_LurpekoParkingKudeaketa.Pantailak;

/// <summary>
/// Ibilgailuen bilaketa, fitxa eta aparkatzeen historikoa erakusten ditu.
/// </summary>
public partial class IbilgailuakPage : ContentPage
{
    private bool? _diseinuEstua;
    private ParkingKudeatzailea Kudeatzailea =>
        ((AppShell)Shell.Current).Kudeatzailea;

    /// <summary>
    /// XAML osagaiak hasieratzen ditu.
    /// </summary>
    public IbilgailuakPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Telefono edo leiho estuetan espazio gutxiago erabiltzen du.
    /// </summary>
    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        bool estua = width < 620 || DeviceInfo.Current.Idiom == DeviceIdiom.Phone;
        if (_diseinuEstua == estua)
        {
            return;
        }

        _diseinuEstua = estua;
        DiseinuNagusia.Padding = new Thickness(estua ? 10 : 18);
        Izenburua.FontSize = estua ? 21 : 25;
    }

    /// <summary>
    /// Pantailara sartzean bilaketa eta aurrez aukeratutako ibilgailua berreskuratzen ditu.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Aukera gorde, zerrenda berriz kargatzeak hautapena ezabatu baitezake.
        var hautatua = Kudeatzailea.HautatutakoIbilgailua;
        if (hautatua is not null)
        {
            Bilaketa.Text = "";
        }

        IbilgailuakEguneratu();

        if (hautatua is not null)
        {
            IbilgailuZerrenda.SelectedItem = hautatua;
            XehetasunakEguneratu(hautatua);
        }
    }

    /// <summary>
    /// Bilaketaren emaitzak eguneratzen ditu, datuak aldatu gabe.
    /// </summary>
    private void IbilgailuakEguneratu()
    {
        var emaitzak = Kudeatzailea.IbilgailuakBilatu(Bilaketa.Text);
        IbilgailuZerrenda.ItemsSource = emaitzak;

        if (IbilgailuZerrenda.SelectedItem is Ibilgailua hautatua
            && !emaitzak.Contains(hautatua))
        {
            IbilgailuZerrenda.SelectedItem = null;
            XehetasunakEguneratu(null);
        }
    }

    /// <summary>
    /// Bilaketa-testua aldatzean ibilgailuak iragazten ditu.
    /// </summary>
    private void BilaketaAldatuta(object? sender, TextChangedEventArgs e)
    {
        if (Shell.Current is not null)
        {
            IbilgailuakEguneratu();
        }
    }

    /// <summary>
    /// Zerrendan aukeratutako ibilgailuaren datuak erakusten ditu.
    /// </summary>
    private void IbilgailuaHautatua(object? sender, SelectionChangedEventArgs e)
    {
        XehetasunakEguneratu(e.CurrentSelection.FirstOrDefault() as Ibilgailua);
    }

    /// <summary>
    /// Ibilgailuaren fitxa eta historikoa prestatzen ditu.
    /// </summary>
    /// <param name="ibilgailua">Aukeratutako ibilgailua edo null.</param>
    private void XehetasunakEguneratu(Ibilgailua? ibilgailua)
    {
        Kudeatzailea.HautatutakoIbilgailua = ibilgailua;
        if (ibilgailua is null)
        {
            IbilgailuXehetasunak.Text = AppResources.SelectVehicle;
            HistoriaZerrenda.ItemsSource = null;
            return;
        }

        IbilgailuXehetasunak.Text = string.Format(
            AppResources.VehicleDetailsFormat,
            ibilgailua.Matrikula, ibilgailua.Marka, ibilgailua.Modeloa,
            ibilgailua.Kolorea, ibilgailua.Jabea.IzenOsoa);

        // Aurkezpen-formatua pantailan mantentzen dugu, eredua garbi uzteko.
        HistoriaZerrenda.ItemsSource = Kudeatzailea.HistoriaBilatu(ibilgailua)
            .Select(a => string.Format(AppResources.HistoryEntryFormat,
                a.Plaza.Zenbakia,
                a.SarreraData.ToString("dd/MM/yyyy HH:mm"),
                a.IrteeraData?.ToString("dd/MM/yyyy HH:mm") ?? AppResources.Active,
                a.OrdaindutakoZenbatekoa.HasValue
                    ? a.OrdaindutakoZenbatekoa.Value.ToString("N2") + " €"
                    : "—"))
            .ToList();
    }
}
