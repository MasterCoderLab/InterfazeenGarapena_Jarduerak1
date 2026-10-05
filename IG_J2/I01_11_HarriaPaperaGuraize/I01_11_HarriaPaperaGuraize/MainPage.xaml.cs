namespace I01_11_HarriaPaperaGuraize;

/// <summary>
/// Harria, papera eta guraize jokoaren orri nagusia.
///
/// Jokalariak hiru aukeretako bat hautatzen du eta makinak
/// bere aukera ausaz sortzen du.
///
/// Txanda bakoitzeko irabazleak puntu bat jasotzen du.
/// Lehenengo 10 puntura iristen denak partida irabazten du.
///
/// Partida amaitzean alerta-mezu bat erakusten da eta,
/// erabiltzaileak mezua baieztatu ondoren, partida berria
/// automatikoki hasten da.
/// </summary>
public partial class MainPage : ContentPage
{
    /// <summary>
    /// Jokalariaren uneko puntu kopurua.
    /// </summary>
    private int jokalariaPuntuak;

    /// <summary>
    /// Makinaren uneko puntu kopurua.
    /// </summary>
    private int makinaPuntuak;

    /// <summary>
    /// Makinaren aukerak ausaz sortzeko erabiltzen den objektua.
    /// </summary>
    private readonly Random ausazkoa = new();

    /// <summary>
    /// Partida irabazteko behar den puntu kopurua.
    /// </summary>
    private const int IRABAZTEKO_PUNTUAK = 10;

    /// <summary>
    /// Harria, papera eta guraize jokoan erabil daitezkeen aukerak.
    /// </summary>
    private enum Aukera
    {
        Harria,
        Papera,
        Guraizeak
    }

    /// <summary>
    /// MainPage klasearen eraikitzailea.
    /// Interfaze grafikoa hasieratu eta partida berria prestatzen du.
    /// </summary>
    public MainPage()
    {
        InitializeComponent();

        BerrabiaraziPartida();
    }

    /// <summary>
    /// Orria pantailan agertzen denean exekutatzen da.
    /// Windows plataforman leihoaren tamaina finkatzen du.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();

#if WINDOWS
        FinkatuLeihoarenTamaina();
#endif
    }

#if WINDOWS

    /// <summary>
    /// Windows plataforman aplikazioaren leihoaren tamaina
    /// finkatzen du eta erabiltzaileak tamaina aldatzea eragozten du.
    /// </summary>
    private void FinkatuLeihoarenTamaina()
    {
        if (Window?.Handler?.PlatformView
            is not Microsoft.UI.Xaml.Window windowsLeihoa)
        {
            return;
        }

        IntPtr hwnd =
            WinRT.Interop.WindowNative.GetWindowHandle(
                windowsLeihoa);

        Microsoft.UI.WindowId windowId =
            Microsoft.UI.Win32Interop.GetWindowIdFromWindow(
                hwnd);

        Microsoft.UI.Windowing.AppWindow appWindow =
            Microsoft.UI.Windowing.AppWindow.GetFromWindowId(
                windowId);

        appWindow.Resize(
            new Windows.Graphics.SizeInt32(
                950,
                620));

        if (appWindow.Presenter
            is Microsoft.UI.Windowing.OverlappedPresenter presenter)
        {
            presenter.IsResizable = false;
            presenter.IsMaximizable = false;
        }
    }

#endif


    // ============================================================
    // JOKALARIAREN AUKERAK
    // ============================================================

    /// <summary>
    /// Jokalariak Harria aukera sakatzen duenean exekutatzen da.
    /// </summary>
    /// <param name="sender">
    /// Gertaera sortu duen TapGestureRecognizer objektua.
    /// </param>
    /// <param name="e">
    /// Ukipen-gertaerari buruzko informazioa.
    /// </param>
    private async void OnHarriaTapped(
        object sender,
        TappedEventArgs e)
    {
        await JokatuTxanda(Aukera.Harria);
    }

    /// <summary>
    /// Jokalariak Papera aukera sakatzen duenean exekutatzen da.
    /// </summary>
    /// <param name="sender">
    /// Gertaera sortu duen TapGestureRecognizer objektua.
    /// </param>
    /// <param name="e">
    /// Ukipen-gertaerari buruzko informazioa.
    /// </param>
    private async void OnPaperaTapped(
        object sender,
        TappedEventArgs e)
    {
        await JokatuTxanda(Aukera.Papera);
    }

    /// <summary>
    /// Jokalariak Guraizeak aukera sakatzen duenean exekutatzen da.
    /// </summary>
    /// <param name="sender">
    /// Gertaera sortu duen TapGestureRecognizer objektua.
    /// </param>
    /// <param name="e">
    /// Ukipen-gertaerari buruzko informazioa.
    /// </param>
    private async void OnGuraizeakTapped(
        object sender,
        TappedEventArgs e)
    {
        await JokatuTxanda(Aukera.Guraizeak);
    }


    // ============================================================
    // TXANDAREN LOGIKA
    // ============================================================

    /// <summary>
    /// Jokoaren txanda oso bat exekutatzen du.
    ///
    /// Aurreko makinaren aukera ezkutatzen du,
    /// makinaren aukera berria ausaz sortzen du,
    /// txandako irabazlea kalkulatzen du eta
    /// puntuazioa eguneratzen du.
    /// </summary>
    /// <param name="jokalariarenAukera">
    /// Jokalariak hautatutako aukera.
    /// </param>
    /// <returns>
    /// Txanda eta beharrezko interfazearen eguneraketak
    /// amaitzean osatzen den Task objektua.
    /// </returns>
    private async Task JokatuTxanda(
        Aukera jokalariarenAukera)
    {
        /*
         * Aurreko txandako irudia kentzen da.
         * Horrela, makinak aukera bera berriro ateratzen badu ere,
         * erabiltzaileak argi ikusiko du txanda berria dela.
         */
        PrestatuTxandaBerria();

        // Aldaketa bisual txiki bat uzten da
        // makinaren aukera berria erakutsi aurretik.
        await Task.Delay(300);

        Aukera makinarenAukera =
            SortuMakinarenAukera();

        ErakutsiMakinarenAukera(
            makinarenAukera);

        int emaitza =
            KalkulatuIrabazlea(
                jokalariarenAukera,
                makinarenAukera);

        if (emaitza > 0)
        {
            jokalariaPuntuak++;
        }
        else if (emaitza < 0)
        {
            makinaPuntuak++;
        }

        EguneratuPuntuazioa();

        await EgiaztatuPartidarenAmaiera();
    }

    /// <summary>
    /// Txanda berri bat hasi aurretik makinaren aurreko
    /// aukera pantailatik ezkutatzen du.
    ///
    /// Honela, makina aukera bera hainbat aldiz jarraian
    /// ateratzen badu ere, txanda berria dela ikus daiteke.
    /// </summary>
    private void PrestatuTxandaBerria()
    {
        MakinaIrudia.IsVisible = false;

        MakinaHasieraLabel.Text = "...";
        MakinaHasieraLabel.IsVisible = true;

        MakinaAukeraLabel.Text =
            "Makina aukeratzen...";
    }

    /// <summary>
    /// Makinaren aukera ausaz sortzen du.
    /// </summary>
    /// <returns>
    /// Harria, Papera edo Guraizeak aukeretako bat.
    /// </returns>
    private Aukera SortuMakinarenAukera()
    {
        int zenbakia =
            ausazkoa.Next(0, 3);

        return (Aukera)zenbakia;
    }

    /// <summary>
    /// Jokalariaren eta makinaren aukerak konparatzen ditu
    /// eta txandako irabazlea kalkulatzen du.
    /// </summary>
    /// <param name="jokalaria">
    /// Jokalariak hautatutako aukera.
    /// </param>
    /// <param name="makina">
    /// Makinak ausaz hautatutako aukera.
    /// </param>
    /// <returns>
    /// 1 jokalariak irabazten badu,
    /// -1 makinak irabazten badu eta
    /// 0 berdinketa gertatzen bada.
    /// </returns>
    private int KalkulatuIrabazlea(
        Aukera jokalaria,
        Aukera makina)
    {
        if (jokalaria == makina)
        {
            return 0;
        }

        bool jokalariakIrabazi =
            (jokalaria == Aukera.Harria &&
             makina == Aukera.Guraizeak)
            ||
            (jokalaria == Aukera.Papera &&
             makina == Aukera.Harria)
            ||
            (jokalaria == Aukera.Guraizeak &&
             makina == Aukera.Papera);

        return jokalariakIrabazi ? 1 : -1;
    }


    // ============================================================
    // MAKINAREN AUKERA
    // ============================================================

    /// <summary>
    /// Makinaren aukera pantailan erakusten du.
    /// Aukerari dagokion irudia eta testua kargatzen ditu.
    /// </summary>
    /// <param name="aukera">
    /// Makinak ausaz hautatutako aukera.
    /// </param>
    private void ErakutsiMakinarenAukera(
        Aukera aukera)
    {
        MakinaHasieraLabel.IsVisible = false;
        MakinaIrudia.IsVisible = true;

        switch (aukera)
        {
            case Aukera.Harria:

                MakinaIrudia.Source =
                    "harria.png";

                MakinaAukeraLabel.Text =
                    "Harria";

                break;

            case Aukera.Papera:

                MakinaIrudia.Source =
                    "papera.png";

                MakinaAukeraLabel.Text =
                    "Papera";

                break;

            case Aukera.Guraizeak:

                MakinaIrudia.Source =
                    "guraizeak.png";

                MakinaAukeraLabel.Text =
                    "Guraizeak";

                break;
        }
    }


    // ============================================================
    // PUNTUAZIOA
    // ============================================================

    /// <summary>
    /// Jokalariaren eta makinaren puntuazioa
    /// pantailako Label kontroletan eguneratzen du.
    /// </summary>
    private void EguneratuPuntuazioa()
    {
        JokalariaPuntuakLabel.Text =
            jokalariaPuntuak.ToString();

        MakinaPuntuakLabel.Text =
            makinaPuntuak.ToString();
    }

    /// <summary>
    /// Jokalarietako bat 10 puntura iritsi den egiaztatzen du.
    ///
    /// Partida amaitu bada, irabazlearen alerta-mezua
    /// erakusten du eta erabiltzaileak "Ados" sakatu arte
    /// ez du partida berrabiarazten.
    /// </summary>
    /// <returns>
    /// Alerta eta partida berrabiarazteko prozesua
    /// amaitzean osatzen den Task objektua.
    /// </returns>
    private async Task EgiaztatuPartidarenAmaiera()
    {
        if (jokalariaPuntuak < IRABAZTEKO_PUNTUAK &&
            makinaPuntuak < IRABAZTEKO_PUNTUAK)
        {
            return;
        }

        string mezua;

        if (jokalariaPuntuak >= IRABAZTEKO_PUNTUAK)
        {
            mezua =
                $"Zorionak! Partida irabazi duzu.\n\n" +
                $"Azken emaitza: {jokalariaPuntuak} - {makinaPuntuak}";
        }
        else
        {
            mezua =
                $"Makinak partida irabazi du.\n\n" +
                $"Azken emaitza: {jokalariaPuntuak} - {makinaPuntuak}";
        }

        /*
         * ALERTA.
         * Puntuazioa ez da berrabiarazten erabiltzaileak
         * "Ados" botoia sakatu arte.
         */
        await DisplayAlertAsync(
            "Partida amaitu da",
            mezua,
            "Ados");

        // Alertaren ondoren partida berria hasten da.
        BerrabiaraziPartida();
    }


    // ============================================================
    // PARTIDA BERRABiaraztea
    // ============================================================

    /// <summary>
    /// Jokoaren puntuazioa eta makinaren aukera
    /// hasierako egoerara itzultzen ditu.
    ///
    /// Partida berriaren hasieran ez da aurreko
    /// partidako irudirik erakusten.
    /// </summary>
    private void BerrabiaraziPartida()
    {
        jokalariaPuntuak = 0;
        makinaPuntuak = 0;

        EguneratuPuntuazioa();

        // Aurreko partidako irudia kentzen da.
        MakinaIrudia.Source = null;
        MakinaIrudia.IsVisible = false;

        // Hasierako galdera-ikurra berriro erakusten da.
        MakinaHasieraLabel.Text = "?";
        MakinaHasieraLabel.IsVisible = true;

        MakinaAukeraLabel.Text =
            "Aukeraren zain";
    }


    // ============================================================
    // IRTEN BOTOIA
    // ============================================================

    /// <summary>
    /// Irten botoia sakatzen denean aplikazioaren leihoa ixten du.
    /// </summary>
    /// <param name="sender">
    /// Gertaera sortu duen botoia.
    /// </param>
    /// <param name="e">
    /// Klik-gertaerari buruzko informazioa.
    /// </param>
    private void OnIrtenClicked(
        object sender,
        EventArgs e)
    {
        Window? leihoa =
            Application.Current?
                .Windows
                .FirstOrDefault();

        if (leihoa != null)
        {
            Application.Current?
                .CloseWindow(leihoa);
        }
    }
}