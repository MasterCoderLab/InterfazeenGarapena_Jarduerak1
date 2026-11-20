
using I01_14_LurpekoParkingKudeaketa.Ereduak;
using I01_14_LurpekoParkingKudeaketa.Kudeaketa;
using I01_14_LurpekoParkingKudeaketa.Resources.Strings;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;

namespace I01_14_LurpekoParkingKudeaketa.Pantailak;

/// <summary>
/// Aparkalekuko plazak eta haien iragazkiak erakusten ditu.
/// Datuen kontsultak ParkingKudeatzailea klasearen bidez egiten dira.
/// </summary>
public partial class AparkalekuaPage : ContentPage
{
    private bool _iragazkiakHasieratuta;
    private bool? _diseinuEstua;
    private Plaza? _hautatutakoPlaza;

    private ParkingKudeatzailea Kudeatzailea =>
        ((AppShell)Shell.Current).Kudeatzailea;

    /// <summary>
    /// Aparkalekuaren pantailako XAML osagaiak hasieratzen ditu.
    /// </summary>
    public AparkalekuaPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Gailuaren informazioa eta leihoaren zabalera erabiliz
    /// iragazkiak egokitzen ditu.
    /// Telefonoetan bertikalki eta tabletetan hiru zutabetan kokatzen ditu.
    /// </summary>
    /// <param name="width">
    /// Pantailaren zabalera erabilgarria, unitate logikoetan.
    /// </param>
    /// <param name="height">
    /// Pantailaren altuera erabilgarria, unitate logikoetan.
    /// </param>
    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        bool estua = width < 620
            || DeviceInfo.Current.Idiom == DeviceIdiom.Phone;

        if (_diseinuEstua == estua)
        {
            return;
        }

        _diseinuEstua = estua;

        DiseinuNagusia.Padding = new Thickness(estua ? 10 : 18);
        Izenburua.FontSize = estua ? 21 : 25;

        Grid.SetRow(SolairuIragazkiaMultzoa, 0);
        Grid.SetRow(EgoeraIragazkiaMultzoa, estua ? 1 : 0);
        Grid.SetRow(MotaIragazkiaMultzoa, estua ? 2 : 0);

        Grid.SetColumn(SolairuIragazkiaMultzoa, 0);
        Grid.SetColumn(EgoeraIragazkiaMultzoa, estua ? 0 : 1);
        Grid.SetColumn(MotaIragazkiaMultzoa, estua ? 0 : 2);

        Grid.SetColumnSpan(SolairuIragazkiaMultzoa, estua ? 3 : 1);
        Grid.SetColumnSpan(EgoeraIragazkiaMultzoa, estua ? 3 : 1);
        Grid.SetColumnSpan(MotaIragazkiaMultzoa, estua ? 3 : 1);
    }

    /// <summary>
    /// Pantaila agertzean iragazkiak hasieratzen ditu
    /// eta plazen zerrenda eguneratzen du.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (!_iragazkiakHasieratuta)
        {
            SolairuIragazkia.Items.Add(AppResources.All);
            SolairuIragazkia.Items.Add("-1");
            SolairuIragazkia.Items.Add("-2");

            foreach (var egoera in new[]
            {
                AppResources.All,
                AppResources.Available,
                AppResources.Occupied,
                AppResources.Reserved,
                AppResources.Maintenance
            })
            {
                EgoeraIragazkia.Items.Add(egoera);
            }

            foreach (var mota in new[]
            {
                AppResources.All,
                AppResources.Car,
                AppResources.Motorcycle,
                AppResources.Accessible,
                AppResources.Electric
            })
            {
                MotaIragazkia.Items.Add(mota);
            }

            // Aurreko iragazkiak berreskuratu eta indizeak mugatzen ditu.
            SolairuIragazkia.SelectedIndex = Math.Clamp(
                Preferences.Default.Get("parking.solairu", 0), 0, 2);

            EgoeraIragazkia.SelectedIndex = Math.Clamp(
                Preferences.Default.Get("parking.egoera", 0), 0, 4);

            MotaIragazkia.SelectedIndex = Math.Clamp(
                Preferences.Default.Get("parking.mota", 0), 0, 4);

            LibreakBakarrik.IsToggled =
                Preferences.Default.Get("parking.libreak", false);

            _iragazkiakHasieratuta = true;
        }

        PlazakEguneratu();
    }

    /// <summary>
    /// Hautatutako iragazkiak aplikatzen ditu eta
    /// plaza hautatua zenbakiaren bidez berreskuratzen du.
    /// </summary>
    private void PlazakEguneratu()
    {
        int? solairua = SolairuIragazkia.SelectedIndex switch
        {
            1 => -1,
            2 => -2,
            _ => null
        };

        PlazaEgoera? egoera = EgoeraIragazkia.SelectedIndex switch
        {
            1 => PlazaEgoera.Libre,
            2 => PlazaEgoera.Okupatuta,
            3 => PlazaEgoera.Erreserbatuta,
            4 => PlazaEgoera.MantentzeLanetan,
            _ => null
        };

        PlazaMota? mota = MotaIragazkia.SelectedIndex switch
        {
            1 => PlazaMota.Autoa,
            2 => PlazaMota.Motorra,
            3 => PlazaMota.MugikortasunUrrikoa,
            4 => PlazaMota.Elektrikoa,
            _ => null
        };

        string? hautatutakoZenbakia = _hautatutakoPlaza?.Zenbakia;

        var plazak = Kudeatzailea.PlazakIragazi(
            solairua,
            egoera,
            mota,
            LibreakBakarrik.IsToggled);

        var hautatua = plazak.FirstOrDefault(
            p => p.Zenbakia == hautatutakoZenbakia);

        PlazaZerrenda.ItemsSource = plazak;

        PlazaKopurua.Text = string.Format(
            AppResources.SpaceCountFormat, plazak.Count);

        PlazaZerrenda.SelectedItem = hautatua;
        XehetasunakEguneratu(hautatua);
    }

    /// <summary>
    /// Picker baten balioa aldatzean iragazkiak gorde
    /// eta plazen zerrenda eguneratzen du.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen kontrola.</param>
    /// <param name="e">Gertaerari lotutako informazioa.</param>
    private void IragazkiakAldatuta(object? sender, EventArgs e)
    {
        if (_iragazkiakHasieratuta)
        {
            IragazkiakGorde();
            PlazakEguneratu();
        }
    }

    /// <summary>
    /// Plaza libreak soilik erakusteko aukera aldatzean
    /// iragazkiak gorde eta zerrenda eguneratzen du.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen kontrola.</param>
    /// <param name="e">Aukeraren egoera berriari buruzko informazioa.</param>
    private void LibreakBakarrikAldatuta(object? sender, ToggledEventArgs e)
    {
        if (_iragazkiakHasieratuta)
        {
            IragazkiakGorde();
            PlazakEguneratu();
        }
    }

    /// <summary>
    /// Iragazkien aukerak gailuko hobespen pribatuetan gordetzen ditu.
    /// Ez ditu parkingeko erregistroak aldatzen.
    /// </summary>
    private void IragazkiakGorde()
    {
        Preferences.Default.Set(
            "parking.solairu", SolairuIragazkia.SelectedIndex);

        Preferences.Default.Set(
            "parking.egoera", EgoeraIragazkia.SelectedIndex);

        Preferences.Default.Set(
            "parking.mota", MotaIragazkia.SelectedIndex);

        Preferences.Default.Set(
            "parking.libreak", LibreakBakarrik.IsToggled);
    }

    /// <summary>
    /// Hautatutako plazaren eta bertako ibilgailuaren
    /// informazioa erakusten du.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen kontrola.</param>
    /// <param name="e">Hautatutako plazaren informazioa.</param>
    private void PlazaHautatua(object? sender, SelectionChangedEventArgs e)
    {
        XehetasunakEguneratu(
            e.CurrentSelection.FirstOrDefault() as Plaza);
    }

    /// <summary>
    /// Hautatutako plazaren eta bertako ibilgailuaren
    /// xehetasunak eguneratzen ditu.
    /// </summary>
    /// <param name="plaza">
    /// Hautatutako plaza; null bada, xehetasunak garbitzen ditu.
    /// </param>
    private void XehetasunakEguneratu(Plaza? plaza)
    {
        _hautatutakoPlaza = plaza;

        if (plaza is null)
        {
            PlazaXehetasunak.Text = AppResources.SelectSpace;
            IbilgailuXehetasunak.Text = "";
            IbilgailuaIkusiBotoia.IsEnabled = false;
            return;
        }

        PlazaXehetasunak.Text = string.Format(
            AppResources.SpaceDetailsFormat,
            plaza.Zenbakia,
            plaza.Solairua,
            plaza.MotaIzena,
            plaza.EgoeraIzena);

        IbilgailuXehetasunak.Text = plaza.Ibilgailua is null
            ? AppResources.NoVehicleInSpace
            : string.Format(
                AppResources.VehicleInSpaceFormat,
                plaza.Ibilgailua.Matrikula,
                plaza.Ibilgailua.Marka,
                plaza.Ibilgailua.Modeloa,
                plaza.Ibilgailua.Jabea.IzenOsoa);

        IbilgailuaIkusiBotoia.IsEnabled =
            plaza.Ibilgailua is not null;
    }

    /// <summary>
    /// Okupatutako plazako ibilgailua aukeratu
    /// eta ibilgailuen pantailara nabigatzen du.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen kontrola.</param>
    /// <param name="e">Gertaerari lotutako informazioa.</param>
    private async void IbilgailuaIkusi(object? sender, EventArgs e)
    {
        if (_hautatutakoPlaza?.Ibilgailua is null)
        {
            return;
        }

        Kudeatzailea.HautatutakoIbilgailua =
            _hautatutakoPlaza.Ibilgailua;

        await Shell.Current.GoToAsync(
            "//nagusia/ibilgailuTab/ibilgailuak");
    }
}
