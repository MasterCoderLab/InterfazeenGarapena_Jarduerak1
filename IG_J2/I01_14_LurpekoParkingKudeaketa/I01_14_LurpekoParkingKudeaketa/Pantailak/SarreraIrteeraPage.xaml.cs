using I01_14_LurpekoParkingKudeaketa.Kudeaketa;

namespace I01_14_LurpekoParkingKudeaketa.Pantailak;

/// <summary>
/// Ibilgailuen sarrerak, irteerak eta ordainketak kudeatzen ditu.
/// </summary>
public partial class SarreraIrteeraPage : ContentPage
{
    private bool? _diseinuEstua;
    private string? _proposatutakoPlaza;
    private string? _proposatutakoMatrikula;
    private string? _irteerarakoMatrikula;

    private ParkingKudeatzailea Kudeatzailea =>
        ((AppShell)Shell.Current).Kudeatzailea;

    /// <summary>
    /// Pantaila hasieratzen du.
    /// </summary>
    public SarreraIrteeraPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Pantaila estuetan sarrera eta irteera panelak bertikalki kokatzen ditu.
    /// </summary>
    /// <param name="width">Pantailaren zabalera erabilgarrian, unitate logikoetan.</param>
    /// <param name="height">Pantailaren altuera erabilgarrian, unitate logikoetan.</param>
    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        bool estua = width < 800;
        if (_diseinuEstua == estua)
            return;
        _diseinuEstua = estua;
        Grid.SetRow(IrteeraPanela, estua ? 1 : 0);
        Grid.SetColumn(IrteeraPanela, estua ? 0 : 1);
        Grid.SetColumnSpan(IrteeraPanela, estua ? 2 : 1);
        Grid.SetColumnSpan(SarreraPanela, estua ? 2 : 1);
    }

    /// <summary>
    /// Sarrerako matrikula aldatzen denean aurreko plaza-proposamena baliogabetzen du.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen kontrola.</param>
    /// <param name="e">Gertaerari lotutako informazioa.</param>
    private void SarreraTestuaAldatuta(object? sender, TextChangedEventArgs e) =>
        SarreraGarbitu();

    /// <summary>
    /// Irisgarritasun-aukera aldatzean aurreko plaza-proposamena baliogabetzen du.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen kontrola.</param>
    /// <param name="e">Gertaerari lotutako informazioa.</param>
    private void SarreraAukeraAldatuta(object? sender, ToggledEventArgs e) =>
        SarreraGarbitu();

    /// <summary>
    /// Sarrera-proposamena eta haren baieztapen-botoia garbitzen ditu.
    /// </summary>
    private void SarreraGarbitu()
    {
        _proposatutakoPlaza = null;
        _proposatutakoMatrikula = null;
        if (SarreraBaieztatuBotoia is not null)
            SarreraBaieztatuBotoia.IsEnabled = false;
        if (Proposamena is not null)
            Proposamena.Text = "Sartu matrikula bat.";
    }

    /// <summary>
    /// Sartutako matrikularentzat plaza bateragarria proposatu eta baieztapen-botoia aktibatzen du.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen kontrola.</param>
    /// <param name="e">Gertaerari lotutako informazioa.</param>
    private async void PlazaProposatu(object? sender, EventArgs e)
    {
        try
        {
            var plaza = Kudeatzailea.PlazaProposatu(SarreraMatrikula.Text, Irisgarria.IsToggled);
            _proposatutakoPlaza = plaza.Zenbakia;
            _proposatutakoMatrikula = SarreraMatrikula.Text?.Trim().ToUpperInvariant();
            Proposamena.Text = $"{plaza.Zenbakia} · {plaza.Solairua}. solairua · {plaza.MotaIzena}";
            SarreraBaieztatuBotoia.IsEnabled = true;
        }
        catch (InvalidOperationException ex)
        {
            SarreraGarbitu();
            await DisplayAlertAsync("Sarrera", ex.Message, "Ados");
        }
    }

    /// <summary>
    /// Proposatutako plazan sarrera gordetzen du, eta emaitza erabiltzaileari erakusten dio.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen kontrola.</param>
    /// <param name="e">Gertaerari lotutako informazioa.</param>
    private async void SarreraBaieztatu(object? sender, EventArgs e)
    {
        if (_proposatutakoPlaza is null || _proposatutakoMatrikula is null)
            return;
        try
        {
            Kudeatzailea.SarreraErregistratu(_proposatutakoMatrikula,
                _proposatutakoPlaza, Irisgarria.IsToggled);
            string zenbakia = _proposatutakoPlaza;
            SarreraGarbitu();
            SarreraMatrikula.Text = "";
            await DisplayAlertAsync("Sarrera", $"Sarrera gordeta. Plaza: {zenbakia}", "Ados");
        }
        catch (InvalidOperationException ex)
        {
            SarreraGarbitu();
            await DisplayAlertAsync("Sarrera", ex.Message, "Ados");
        }
    }

    /// <summary>
    /// Irteerako matrikula aldatzen denean aurreko ordainketa-kalkulua baliogabetzen du.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen kontrola.</param>
    /// <param name="e">Gertaerari lotutako informazioa.</param>
    private void IrteeraTestuaAldatuta(object? sender, TextChangedEventArgs e) =>
        IrteeraGarbitu();

    /// <summary>
    /// Irteera-kalkulua eta haren baieztapen-botoia garbitzen ditu.
    /// </summary>
    private void IrteeraGarbitu()
    {
        _irteerarakoMatrikula = null;
        if (IrteeraBaieztatuBotoia is not null)
            IrteeraBaieztatuBotoia.IsEnabled = false;
        if (IrteeraXehetasunak is not null)
            IrteeraXehetasunak.Text = "Sartu matrikula bat.";
    }

    /// <summary>
    /// Irteeraren iraupena eta zenbatekoa kontsultatu, eta pantailan erakusten ditu.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen kontrola.</param>
    /// <param name="e">Gertaerari lotutako informazioa.</param>
    private async void IrteeraKalkulatu(object? sender, EventArgs e)
    {
        try
        {
            var aurreikuspena = Kudeatzailea.IrteeraAurreikusi(IrteeraMatrikula.Text);
            _irteerarakoMatrikula = IrteeraMatrikula.Text?.Trim().ToUpperInvariant();
            IrteeraXehetasunak.Text = $"Plaza: {aurreikuspena.Plaza}\n" +
                $"Denbora: {aurreikuspena.Iraupena.Days} egun, " +
                $"{aurreikuspena.Iraupena.Hours} ordu eta " +
                $"{aurreikuspena.Iraupena.Minutes} minutu\n" +
                $"Ordaindu: {aurreikuspena.Zenbatekoa:N2} €";
            IrteeraBaieztatuBotoia.IsEnabled = true;
        }
        catch (InvalidOperationException ex)
        {
            IrteeraGarbitu();
            await DisplayAlertAsync("Irteera", ex.Message, "Ados");
        }
    }

    /// <summary>
    /// Irteera erregistratzen du, eta ordaindutako zenbatekoa jakinarazten du.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen kontrola.</param>
    /// <param name="e">Gertaerari lotutako informazioa.</param>
    private async void IrteeraBaieztatu(object? sender, EventArgs e)
    {
        if (_irteerarakoMatrikula is null)
            return;
        try
        {
            decimal ordainduta = Kudeatzailea.IrteeraErregistratu(_irteerarakoMatrikula);
            _irteerarakoMatrikula = null;
            IrteeraBaieztatuBotoia.IsEnabled = false;
            IrteeraMatrikula.Text = "";
            await DisplayAlertAsync("Irteera", $"Irteera gordeta. Ordainduta: {ordainduta:N2} €", "Ados");
        }
        catch (InvalidOperationException ex)
        {
            _irteerarakoMatrikula = null;
            IrteeraBaieztatuBotoia.IsEnabled = false;
            await DisplayAlertAsync("Irteera", ex.Message, "Ados");
        }
    }
}
