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
    /// XAML osagaiak hasieratzen ditu.
    /// </summary>
    public AparkalekuaPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Gailuaren informazioa eta leihoaren zabalera erabiliz iragazkiak egokitzen ditu.
    /// Telefonoetan goitik behera jartzen dira; tabletetan hiru zutabetan.
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
    /// Pantaila agertzen denean iragazkiak eta plazen zerrenda prestatzen ditu.
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
                AppResources.All, AppResources.Available, AppResources.Occupied,
                AppResources.Reserved, AppResources.Maintenance
            })
            {
                EgoeraIragazkia.Items.Add(egoera);
            }

            foreach (var mota in new[]
            {
                AppResources.All, AppResources.Car, AppResources.Motorcycle,
                AppResources.Accessible, AppResources.Electric
            })
            {
                MotaIragazkia.Items.Add(mota);
            }

            // Hasierako hautapena aurreko saioaren hobespenetatik berreskuratu.
            // Indizeak mugatu egiten dira, aplikazioaren aukerak aldatuz gero.
            SolairuIragazkia.SelectedIndex = Math.Clamp(
                Preferences.Default.Get("parking.solairu", 0), 0, 2);
            EgoeraIragazkia.SelectedIndex = Math.Clamp(
                Preferences.Default.Get("parking.egoera", 0), 0, 4);
            MotaIragazkia.SelectedIndex = Math.Clamp(
                Preferences.Default.Get("parking.mota", 0), 0, 4);
            LibreakBakarrik.IsToggled = Preferences.Default.Get("parking.libreak", false);
            _iragazkiakHasieratuta = true;
        }

        PlazakEguneratu();
    }

    /// <summary>
    /// Hautatutako iragazkiak aplikatu eta zerrenda eguneratzen du.
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

        var plazak = Kudeatzailea.PlazakIragazi(
            solairua, egoera, mota, LibreakBakarrik.IsToggled);

        PlazaZerrenda.ItemsSource = plazak;
        PlazaKopurua.Text = string.Format(AppResources.SpaceCountFormat, plazak.Count);

        if (_hautatutakoPlaza is not null && !plazak.Contains(_hautatutakoPlaza))
        {
            PlazaZerrenda.SelectedItem = null;
            XehetasunakEguneratu(null);
        }
    }

    /// <summary>
    /// Picker baten balioa aldatzean zerrenda berriz iragazten du.
    /// </summary>
    private void IragazkiakAldatuta(object? sender, EventArgs e)
    {
        if (_iragazkiakHasieratuta)
        {
            IragazkiakGorde();
            PlazakEguneratu();
        }
    }

    /// <summary>
    /// Plaza libreen aukerak zerrenda berriz iragazten du.
    /// </summary>
    private void LibreakBakarrikAldatuta(object? sender, ToggledEventArgs e)
    {
        if (_iragazkiakHasieratuta)
        {
            IragazkiakGorde();
            PlazakEguneratu();
        }
    }

    /// <summary>
    /// Iragazkien aukera txikiak gailuko hobespen pribatuetan gordetzen ditu.
    /// Ez du bezero edo ibilgailuen daturik gordetzen.
    /// </summary>
    private void IragazkiakGorde()
    {
        Preferences.Default.Set("parking.solairu", SolairuIragazkia.SelectedIndex);
        Preferences.Default.Set("parking.egoera", EgoeraIragazkia.SelectedIndex);
        Preferences.Default.Set("parking.mota", MotaIragazkia.SelectedIndex);
        Preferences.Default.Set("parking.libreak", LibreakBakarrik.IsToggled);
    }

    /// <summary>
    /// Hautatutako plazaren eta bertako ibilgailuaren informazioa erakusten du.
    /// </summary>
    private void PlazaHautatua(object? sender, SelectionChangedEventArgs e)
    {
        XehetasunakEguneratu(e.CurrentSelection.FirstOrDefault() as Plaza);
    }

    /// <summary>
    /// Plaza- eta ibilgailu-xehetasunak eguneratzen ditu.
    /// </summary>
    /// <param name="plaza">Hautatutako plaza edo null.</param>
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

        PlazaXehetasunak.Text = string.Format(AppResources.SpaceDetailsFormat,
            plaza.Zenbakia, plaza.Solairua, plaza.MotaIzena, plaza.EgoeraIzena);
        IbilgailuXehetasunak.Text = plaza.Ibilgailua is null
            ? AppResources.NoVehicleInSpace
            : string.Format(AppResources.VehicleInSpaceFormat,
                plaza.Ibilgailua.Matrikula, plaza.Ibilgailua.Marka,
                plaza.Ibilgailua.Modeloa, plaza.Ibilgailua.Jabea.IzenOsoa);
        IbilgailuaIkusiBotoia.IsEnabled = plaza.Ibilgailua is not null;
    }

    /// <summary>
    /// Okupatutako plazako ibilgailua aukeratu eta haren pantailara doa.
    /// </summary>
    private async void IbilgailuaIkusi(object? sender, EventArgs e)
    {
        if (_hautatutakoPlaza?.Ibilgailua is null)
        {
            return;
        }

        Kudeatzailea.HautatutakoIbilgailua = _hautatutakoPlaza.Ibilgailua;
        await Shell.Current.GoToAsync("//nagusia/ibilgailuTab/ibilgailuak");
    }
}
