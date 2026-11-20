using I01_14_LurpekoParkingKudeaketa.Ereduak;
using I01_14_LurpekoParkingKudeaketa.Kudeaketa;

namespace I01_14_LurpekoParkingKudeaketa.Pantailak;

/// <summary>
/// Plazak gehitu, aldatu, mantentzera bidali eta ezabatzeko pantaila.
/// </summary>
public partial class AdministrazioaPage : ContentPage
{
    private bool? _diseinuEstua;
    private string? _hautatutakoZenbakia;
    private ParkingKudeatzailea Kudeatzailea =>
        ((AppShell)Shell.Current).Kudeatzailea;

    /// <summary>
    /// Plazen administrazio-pantaila hasieratu eta motaren eta egoeraren hasierako aukerak ezartzen ditu.
    /// </summary>
    public AdministrazioaPage()
    {
        InitializeComponent();
        PlazaMotaAukera.SelectedIndex = 0;
        PlazaEgoeraAukera.SelectedIndex = 0;
    }

    /// <summary>
    /// Pantaila estuetan formularioko eremuak eta ekintza-botoiak berrantolatzen ditu.
    /// </summary>
    /// <param name="width">Pantailaren zabalera erabilgarrian, unitate logikoetan.</param>
    /// <param name="height">Pantailaren altuera erabilgarrian, unitate logikoetan.</param>
    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        bool estua = width < 780;
        if (_diseinuEstua == estua)
            return;
        _diseinuEstua = estua;
        Grid.SetRow(PlazaMotaAukera, estua ? 1 : 0);
        Grid.SetRow(PlazaEgoeraAukera, estua ? 1 : 0);
        Grid.SetColumn(Solairua, estua ? 2 : 1);
        Grid.SetColumn(PlazaMotaAukera, estua ? 0 : 2);
        Grid.SetColumn(PlazaEgoeraAukera, estua ? 2 : 3);
        Grid.SetRow(MantentzeBotoia, estua ? 1 : 0);
        Grid.SetRow(EzabatuBotoia, estua ? 1 : 0);
        Grid.SetColumn(AldatuBotoia, estua ? 2 : 1);
        Grid.SetColumn(MantentzeBotoia, estua ? 0 : 2);
        Grid.SetColumn(EzabatuBotoia, estua ? 2 : 3);
        foreach (var kontrola in new View[] { PlazaZenbakia, Solairua, PlazaMotaAukera, PlazaEgoeraAukera,
            GehituBotoia, AldatuBotoia, MantentzeBotoia, EzabatuBotoia })
            Grid.SetColumnSpan(kontrola, estua ? 2 : 1);
    }

    /// <summary>
    /// Pantaila irekitzean plazen zerrenda eguneratzen du.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        PlazakEguneratu();
    }

    /// <summary>
    /// Plazen zerrenda berritu eta aurreko hautapena ezabatzen du.
    /// </summary>
    private void PlazakEguneratu()
    {
        PlazaZerrenda.SelectedItem = null;
        _hautatutakoZenbakia = null;
        PlazaZerrenda.ItemsSource = Kudeatzailea.PlazakIragazi(null, null, null, false);
    }

    /// <summary>
    /// Hautatutako plazaren balioak formularioan kargatzen ditu.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen kontrola.</param>
    /// <param name="e">Gertaerari lotutako informazioa.</param>
    private void PlazaHautatua(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not Plaza plaza)
            return;
        _hautatutakoZenbakia = plaza.Zenbakia;
        PlazaZenbakia.Text = plaza.Zenbakia;
        Solairua.Text = plaza.Solairua.ToString();
        PlazaMotaAukera.SelectedIndex = (int)plaza.Mota;
        PlazaEgoeraAukera.SelectedIndex = plaza.Egoera switch
        {
            PlazaEgoera.Okupatuta => 1,
            PlazaEgoera.Erreserbatuta => 2,
            PlazaEgoera.MantentzeLanetan => 3,
            _ => 0
        };
    }

    /// <summary>
    /// Formularioko datuak irakurri, zenbakia normalizatu eta nahitaezko eremuak egiaztatzen ditu.
    /// </summary>
    /// <returns>Plazaren zenbakia, solairua, mota eta egoera.</returns>
    /// <exception cref="InvalidOperationException">Nahitaezko eremuren bat hutsik edo baliogabe dagoenean.</exception>
    private (string Zenbakia, int Solairua, PlazaMota Mota, PlazaEgoera Egoera) DatuakLortu()
    {
        string zenbakia = PlazaZenbakia.Text?.Trim().ToUpperInvariant() ?? "";
        if (string.IsNullOrWhiteSpace(zenbakia) || !int.TryParse(Solairua.Text, out int solairua)
            || PlazaMotaAukera.SelectedIndex < 0 || PlazaEgoeraAukera.SelectedIndex < 0)
            throw new InvalidOperationException("Plaza, solairua, mota eta egoera bete behar dira.");
        PlazaEgoera egoera = PlazaEgoeraAukera.SelectedIndex switch
        {
            1 => PlazaEgoera.Okupatuta,
            2 => PlazaEgoera.Erreserbatuta,
            3 => PlazaEgoera.MantentzeLanetan,
            _ => PlazaEgoera.Libre
        };
        return (zenbakia, solairua, (PlazaMota)PlazaMotaAukera.SelectedIndex, egoera);
    }

    /// <summary>
    /// Formularioko datuekin plaza berri bat gordetzen du eta emaitza jakinarazten du.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen kontrola.</param>
    /// <param name="e">Gertaerari lotutako informazioa.</param>
    private async void PlazaGehitu(object? sender, EventArgs e)
    {
        try
        {
            var d = DatuakLortu();
            Kudeatzailea.PlazaGehitu(d.Zenbakia, d.Solairua, d.Mota, d.Egoera);
            PlazakEguneratu();
            await DisplayAlertAsync("Administrazioa", "Plaza gehituta.", "Ados");
        }
        catch (InvalidOperationException ex)
        {
            await DisplayAlertAsync("Administrazioa", ex.Message, "Ados");
        }
    }

    /// <summary>
    /// Hautatutako plazaren datuak aldatzen ditu, zenbakia aldatu gabe.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen kontrola.</param>
    /// <param name="e">Gertaerari lotutako informazioa.</param>
    private async void PlazaAldatu(object? sender, EventArgs e)
    {
        try
        {
            var d = DatuakLortu();
            if (_hautatutakoZenbakia is null || d.Zenbakia != _hautatutakoZenbakia)
                throw new InvalidOperationException("Hautatu plaza bat. Zenbakia ezin da aldatu.");
            Kudeatzailea.PlazaAldatu(d.Zenbakia, d.Solairua, d.Mota, d.Egoera);
            PlazakEguneratu();
            await DisplayAlertAsync("Administrazioa", "Plaza aldatuta.", "Ados");
        }
        catch (InvalidOperationException ex)
        {
            await DisplayAlertAsync("Administrazioa", ex.Message, "Ados");
        }
    }

    /// <summary>
    /// Hautatutako plaza mantentze-lanetan jartzen du, posible bada.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen kontrola.</param>
    /// <param name="e">Gertaerari lotutako informazioa.</param>
    private async void MantentzeraJarri(object? sender, EventArgs e)
    {
        try
        {
            if (_hautatutakoZenbakia is null)
                throw new InvalidOperationException("Hautatu plaza bat.");
            Kudeatzailea.MantentzeraJarri(_hautatutakoZenbakia);
            PlazakEguneratu();
            await DisplayAlertAsync("Administrazioa", "Plaza mantentze-lanetan dago.", "Ados");
        }
        catch (InvalidOperationException ex)
        {
            await DisplayAlertAsync("Administrazioa", ex.Message, "Ados");
        }
    }

    /// <summary>
    /// Erabiltzailearen baieztapenaren ondoren hautatutako plaza ezabatzen du, baldin eta okupatuta ez badago.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen kontrola.</param>
    /// <param name="e">Gertaerari lotutako informazioa.</param>
    private async void PlazaEzabatu(object? sender, EventArgs e)
    {
        try
        {
            if (_hautatutakoZenbakia is null)
                throw new InvalidOperationException("Hautatu plaza bat.");
            bool bai = await DisplayAlertAsync("Administrazioa",
                $"{_hautatutakoZenbakia} plaza ezabatu?", "Bai", "Ez");
            if (!bai)
                return;
            Kudeatzailea.PlazaEzabatu(_hautatutakoZenbakia);
            PlazakEguneratu();
            await DisplayAlertAsync("Administrazioa", "Plaza ezabatuta.", "Ados");
        }
        catch (InvalidOperationException ex)
        {
            await DisplayAlertAsync("Administrazioa", ex.Message, "Ados");
        }
    }
}
