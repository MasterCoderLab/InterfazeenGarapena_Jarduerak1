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
    /// Leiho estuetan fitxa eta historikoa bata bestearen azpian jartzen ditu.
    /// </summary>
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

        Grid.SetRow(IbilgailuFitxa, 0);
        Grid.SetColumn(IbilgailuFitxa, 0);
        Grid.SetColumnSpan(IbilgailuFitxa, estua ? 2 : 1);

        Grid.SetRow(HistoriaPanela, estua ? 1 : 0);
        Grid.SetColumn(HistoriaPanela, estua ? 0 : 1);
        Grid.SetColumnSpan(HistoriaPanela, estua ? 2 : 1);
    }

    /// <summary>
    /// Pantailara sartzean bilaketa eta aurrez aukeratutako ibilgailua berreskuratzen ditu.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();

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
    /// Bilaketarekin bat datozen ibilgailuak taulan erakusten ditu.
    /// </summary>
    private void IbilgailuakEguneratu()
    {
        var emaitzak = Kudeatzailea.IbilgailuakBilatu(Bilaketa.Text);
        IbilgailuZerrenda.ItemsSource = emaitzak;
        IbilgailuZerrenda.HeightRequest = Math.Clamp(emaitzak.Count, 1, 6) * 47;

        if (IbilgailuZerrenda.SelectedItem is Ibilgailua hautatua
            && !emaitzak.Contains(hautatua))
        {
            IbilgailuZerrenda.SelectedItem = null;
            XehetasunakEguneratu(null);
        }
    }

    /// <summary>
    /// Bilaketa-testua aldatzean zerrenda eguneratzen du.
    /// </summary>
    private void BilaketaAldatuta(object? sender, TextChangedEventArgs e)
    {
        if (Shell.Current is not null)
        {
            IbilgailuakEguneratu();
        }
    }

    /// <summary>
    /// Zerrendan hautatutako ibilgailuaren datuak erakusten ditu.
    /// </summary>
    private void IbilgailuaHautatua(object? sender, SelectionChangedEventArgs e)
    {
        XehetasunakEguneratu(e.CurrentSelection.FirstOrDefault() as Ibilgailua);
    }

    /// <summary>
    /// Ibilgailuaren fitxa eta aparkatze-historia prestatzen ditu.
    /// </summary>
    /// <param name="ibilgailua">Hautatutako ibilgailua edo null.</param>
    private void XehetasunakEguneratu(Ibilgailua? ibilgailua)
    {
        Kudeatzailea.HautatutakoIbilgailua = ibilgailua;

        if (ibilgailua is null)
        {
            IbilgailuXehetasunak.Text = AppResources.SelectVehicle;
            HistoriaZerrenda.ItemsSource = null;
            HistoriaZerrenda.HeightRequest = 36;
            HistoriaGoiburua.IsVisible = false;
            return;
        }

        IbilgailuXehetasunak.Text = string.Format(
            AppResources.VehicleDetailsFormat,
            ibilgailua.Matrikula, ibilgailua.Marka, ibilgailua.Modeloa,
            ibilgailua.Kolorea, ibilgailua.Jabea.IzenOsoa);

        var historia = Kudeatzailea.HistoriaBilatu(ibilgailua)
            .Select(a => new
            {
                Plaza = a.Plaza.Zenbakia,
                Sarrera = a.SarreraData.ToString("dd/MM/yy\nHH:mm"),
                Irteera = a.IrteeraData?.ToString("dd/MM/yy\nHH:mm") ?? AppResources.Active,
                Zenbatekoa = a.OrdaindutakoZenbatekoa.HasValue
                    ? a.OrdaindutakoZenbatekoa.Value.ToString("N2") + " €"
                    : "—"
            })
            .ToList();

        HistoriaGoiburua.IsVisible = historia.Count > 0;
        HistoriaZerrenda.ItemsSource = historia;
        HistoriaZerrenda.HeightRequest = Math.Clamp(historia.Count, 1, 5) * 42;
    }
}
