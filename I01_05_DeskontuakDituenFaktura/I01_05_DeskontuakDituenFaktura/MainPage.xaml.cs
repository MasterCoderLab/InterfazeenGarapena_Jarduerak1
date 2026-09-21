namespace I01_05_DeskontuakDituenFaktura;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        KalkulatuFaktura();
    }


    // Fakturaren datuak kalkulatzen ditu
    private void KalkulatuFaktura()
    {
        int kantitatea;
        double prezioa;
        double bez;


        // Sarrerako datuak irakurri
        if (!int.TryParse(KantitateaEntry.Text, out kantitatea))
        {
            kantitatea = 0;
        }

        if (!double.TryParse(PrezioaEntry.Text, out prezioa))
        {
            prezioa = 0;
        }

        if (!double.TryParse(BezEntry.Text, out bez))
        {
            bez = 21;
        }


        // Guztira deskonturik gabe
        double denera = kantitatea * prezioa;


        // Deskontu ehunekoa
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


        // Deskontuaren zenbatekoa
        double deskontuaDenera =
            denera * deskontua / 100;


        // Deskontua kendu ondorengo prezioa
        double oinarria =
            denera - deskontuaDenera;


        // BEZaren zenbatekoa
        double bezDenera =
            oinarria * bez / 100;


        // Azken prezioa
        double ordaintzekoa =
            oinarria + bezDenera;


        // Emaitzak erakutsi
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


    // Kantitatea, prezioa edo BEZa aldatzen denean
    private void Datuak_TextChanged(
        object? sender,
        TextChangedEventArgs e)
    {
        KalkulatuFaktura();
    }


    // Kalkulatu botoia
    private void KalkulatuButton_Clicked(
        object? sender,
        EventArgs e)
    {
        KalkulatuFaktura();
    }


    // Irten botoia
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