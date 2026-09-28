namespace I01_05_DeskontuakDituenFaktura;

/// <summary>
/// Deskontuak dituen faktura kalkulatzeko aplikazioaren orri nagusia.
/// Kantitatea, unitateko prezioa, deskontua eta BEZa kontuan hartuta
/// fakturaren azken zenbatekoa kalkulatzen du.
/// </summary>
public partial class MainPage : ContentPage
{
    /// <summary>
    /// Orri nagusia hasieratzen du, XAML interfazea kargatzen du
    /// eta hasierako fakturaren kalkulua egiten du.
    /// </summary>
    public MainPage()
    {
        InitializeComponent();

        KalkulatuFaktura();
    }

    /// <summary>
    /// Interfazean sartutako datuak irakurtzen ditu eta
    /// fakturaren guztizko zenbatekoa kalkulatzen du.
    /// Kantitatearen arabera dagokion deskontua aplikatzen du
    /// eta ondoren BEZa gehitzen du.
    /// </summary>
    private void KalkulatuFaktura()
    {
        int kantitatea;
        double prezioa;
        double bez;

        // Erabiltzaileak sartutako kantitatea irakurtzen da.
        // Balio baliogabea bada, zero erabiltzen da.
        if (!int.TryParse(KantitateaEntry.Text, out kantitatea))
        {
            kantitatea = 0;
        }

        // Erabiltzaileak sartutako unitateko prezioa irakurtzen da.
        // Balio baliogabea bada, zero erabiltzen da.
        if (!double.TryParse(PrezioaEntry.Text, out prezioa))
        {
            prezioa = 0;
        }

        // Erabiltzaileak sartutako BEZ ehunekoa irakurtzen da.
        // Balio baliogabea bada, %21 erabiltzen da lehenetsitako balio gisa.
        if (!double.TryParse(BezEntry.Text, out bez))
        {
            bez = 21;
        }

        // Deskonturik aplikatu aurreko guztizko zenbatekoa kalkulatzen da.
        double denera = kantitatea * prezioa;

        // Kantitatearen arabera aplikatu beharreko deskontu ehunekoa zehazten da.
        double deskontua;

        if (kantitatea >= 1000)
        {
            deskontua = 10;
        }
        else if (kantitatea >= 100)
        {
            deskontua = 5;
        }
        else if (kantitatea >= 10)
        {
            deskontua = 2;
        }
        else
        {
            deskontua = 0;
        }

        // Deskontuaren zenbatekoa kalkulatzen da.
        double deskontuaDenera =
            denera * deskontua / 100;

        // Deskontua kendu ondorengo zerga-oinarria kalkulatzen da.
        double oinarria =
            denera - deskontuaDenera;

        // Zerga-oinarriari dagokion BEZaren zenbatekoa kalkulatzen da.
        double bezDenera =
            oinarria * bez / 100;

        // Bezeroak ordaindu beharreko azken zenbatekoa kalkulatzen da.
        double ordaintzekoa =
            oinarria + bezDenera;

        // Kalkulatutako emaitzak interfazeko eremuetan erakusten dira.
        DeneraEntry.Text =
            denera.ToString("F2");

        DeskontuaEntry.Text =
            deskontua.ToString("F0");

        DeskontuaDeneraEntry.Text =
            deskontuaDenera.ToString("F2");

        BezDeneraEntry.Text =
            bezDenera.ToString("F2");

        OrdaintzekoaEntry.Text =
            ordaintzekoa.ToString("F2");
    }

    /// <summary>
    /// Kantitatea, prezioa edo BEZa aldatzen denean
    /// fakturaren emaitzak automatikoki berriz kalkulatzen ditu.
    /// </summary>
    /// <param name="sender">Testua aldatu duen kontrola.</param>
    /// <param name="e">Testu-aldaketaren gertaera-datuak.</param>
    private void Datuak_TextChanged(
        object? sender,
        TextChangedEventArgs e)
    {
        KalkulatuFaktura();
    }

    /// <summary>
    /// "Kalkulatu" botoia sakatzean faktura berriz kalkulatzen du.
    /// Kalkulua egiten ari dela adierazteko botoiaren testua eta egoera
    /// denbora labur batez aldatzen ditu.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen botoia.</param>
    /// <param name="e">Klik gertaeraren datuak.</param>
    private async void KalkulatuButton_Clicked(
        object? sender,
        EventArgs e)
    {
        // Botoiak kalkulua egiten ari dela adierazten du
        // eta aldi baterako desgaitzen da.
        KalkulatuButton.Text = "Kalkulatzen...";
        KalkulatuButton.IsEnabled = false;

        await Task.Delay(400);

        // Fakturaren emaitzak berriz kalkulatzen dira.
        KalkulatuFaktura();

        // Botoia hasierako egoerara itzultzen da.
        KalkulatuButton.Text = "Kalkulatu";
        KalkulatuButton.IsEnabled = true;
    }

    /// <summary>
    /// "Irten" botoia sakatzean aplikazioaren uneko leihoa ixten du.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen botoia.</param>
    /// <param name="e">Klik gertaeraren datuak.</param>
    private void IrtenButton_Clicked(
        object? sender,
        EventArgs e)
    {
        if (Window != null)
        {
            Microsoft.Maui.Controls.Application.Current?.CloseWindow(Window);
        }
    }
}