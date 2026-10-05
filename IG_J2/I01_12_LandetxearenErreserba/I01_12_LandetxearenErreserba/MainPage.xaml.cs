using System.Collections.ObjectModel;
using Microsoft.Maui.Dispatching;

namespace I01_12_LandetxearenErreserba;

/// <summary>
/// Landetxearen erreserbak kudeatzeko aplikazioaren orri nagusia.
///
/// Aplikazioak lau gelaren irudiak CarouselView kontrolaren bidez
/// erakusten ditu.
///
/// Erabiltzaileak aste bateko erreserba egin dezake eta
/// aukeratutako gehigarrien arabera prezioa automatikoki
/// kalkulatzen da.
///
/// Aste bera ezin da bi aldiz erreserbatu.
///
/// Erabiltzaileak 10 segundoz aplikazioarekin elkarreragiten ez badu,
/// CarouselView kontrolak ausaz beste irudi bat erakusten du.
/// </summary>
public partial class MainPage : ContentPage
{
    // ============================================================
    // PREZIOAK
    // ============================================================

    /// <summary>
    /// Landetxea aste oso baterako erreserbatzeko prezioa.
    /// </summary>
    private const decimal ASTEKO_PREZIOA = 300m;

    /// <summary>
    /// Aireportuko joan-etorri zerbitzuaren prezioa.
    /// </summary>
    private const decimal AIREPORTUA_PREZIOA = 25m;

    /// <summary>
    /// Gidaria duen txangoaren prezioa.
    /// </summary>
    private const decimal GIDARIA_PREZIOA = 150m;

    /// <summary>
    /// Haurtzaindegi zerbitzuaren eguneko prezioa.
    /// </summary>
    private const decimal HAURTZAINDegia_EGUNEKO_PREZIOA = 40m;

    /// <summary>
    /// Aste osoko erreserbaren egun kopurua.
    /// </summary>
    private const int ASTEKO_EGUNAK = 7;


    // ============================================================
    // DATUAK
    // ============================================================

    /// <summary>
    /// CarouselView kontrolak erakusten dituen lau irudien izenak.
    /// </summary>
    private readonly List<string> irudiak =
        new()
        {
            "landetxea1.jpg",
            "landetxea2.jpg",
            "landetxea3.jpg",
            "landetxea4.jpg"
        };

    /// <summary>
    /// Dagoeneko erreserbatutako asteetako astelehenak gordetzen ditu.
    /// </summary>
    private readonly HashSet<DateTime> erreserbatutakoAsteak =
        new();

    /// <summary>
    /// Erreserbatutako asteak pantailan erakusteko
    /// testu-zerrenda behagarria.
    /// </summary>
    private readonly ObservableCollection<string> erreserbenTestuak =
        new();

    /// <summary>
    /// Ausazko irudia hautatzeko objektua.
    /// </summary>
    private readonly Random ausazkoa =
        new();

    /// <summary>
    /// Erabiltzailearen 10 segundoko inaktibitatea kontrolatzen duen
    /// tenporizadorea.
    /// </summary>
    private IDispatcherTimer? inaktibitateTenporizadorea;


    // ============================================================
    // ERAIKITZAILEA
    // ============================================================

    /// <summary>
    /// MainPage klasearen eraikitzailea.
    ///
    /// Interfaze grafikoa hasieratu, CarouselView prestatu,
    /// erreserben zerrenda lotu, gaurko data ezarri
    /// eta hasierako prezioak kalkulatzen ditu.
    /// </summary>
    public MainPage()
    {
        InitializeComponent();

        PrestatuCarousel();
        PrestatuErreserbak();
        PrestatuHasierakoData();
        KalkulatuPrezioak();
    }


    // ============================================================
    // ORRIAREN BIZITZA-ZIKLOA
    // ============================================================

    /// <summary>
    /// Orria agertzen denean inaktibitate-tenporizadorea
    /// prestatzen eta abiarazten du.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();

        PrestatuInaktibitateTenporizadorea();
    }

    /// <summary>
    /// Orria desagertzen denean tenporizadorea gelditzen du.
    /// </summary>
    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        inaktibitateTenporizadorea?.Stop();
    }


    // ============================================================
    // CAROUSELVIEW
    // ============================================================

    /// <summary>
    /// CarouselView kontrola lau irudiekin prestatzen du
    /// eta IndicatorView kontrolarekin lotzen du.
    /// </summary>
    private void PrestatuCarousel()
    {
        GelakCarousel.ItemsSource =
            irudiak;

        GelaIndicator.ItemsSource =
            irudiak;

        GelakCarousel.IndicatorView =
            GelaIndicator;

        GelakCarousel.Position = 0;

        EguneratuCarouselKontrolak();
    }

    /// <summary>
    /// Aurreko gelaren irudira mugitzen da.
    /// Lehenengo gelan badago, ez du posizioa aldatzen.
    /// </summary>
    /// <param name="sender">
    /// Gertaera sortu duen Aurrekoa botoia.
    /// </param>
    /// <param name="e">
    /// Klik-gertaeraren informazioa.
    /// </param>
    private void OnAurrekoaClicked(
        object? sender,
        EventArgs e)
    {
        int posizioBerria =
            GelakCarousel.Position - 1;

        if (posizioBerria < 0)
        {
            return;
        }

        GelakCarousel.Position =
            posizioBerria;

        EguneratuCarouselKontrolak();

        BerrabiaraziInaktibitateTenporizadorea();
    }

    /// <summary>
    /// Hurrengo gelaren irudira mugitzen da.
    /// Azken gelan badago, ez du posizioa aldatzen.
    /// </summary>
    /// <param name="sender">
    /// Gertaera sortu duen Hurrengoa botoia.
    /// </param>
    /// <param name="e">
    /// Klik-gertaeraren informazioa.
    /// </param>
    private void OnHurrengoaClicked(
        object? sender,
        EventArgs e)
    {
        int posizioBerria =
            GelakCarousel.Position + 1;

        if (posizioBerria >= irudiak.Count)
        {
            return;
        }

        GelakCarousel.Position =
            posizioBerria;

        EguneratuCarouselKontrolak();

        BerrabiaraziInaktibitateTenporizadorea();
    }

    /// <summary>
    /// CarouselView kontrolaren posizioa aldatzen denean
    /// gelaren izena eta nabigazio-botoiak eguneratzen ditu.
    /// </summary>
    /// <param name="sender">
    /// Posizioa aldatu duen CarouselView kontrola.
    /// </param>
    /// <param name="e">
    /// Aurreko eta uneko posizioaren informazioa.
    /// </param>
    private void OnCarouselPositionChanged(
        object? sender,
        PositionChangedEventArgs e)
    {
        EguneratuCarouselKontrolak();

        BerrabiaraziInaktibitateTenporizadorea();
    }

    /// <summary>
    /// Uneko CarouselView posizioaren arabera gelaren izena
    /// eta alboko botoien egoera eguneratzen ditu.
    /// </summary>
    private void EguneratuCarouselKontrolak()
    {
        int posizioa =
            GelakCarousel.Position;

        GelaLabel.Text =
            $"Gela {posizioa + 1:00}";

        AurrekoaBtn.IsEnabled =
            posizioa > 0;

        HurrengoaBtn.IsEnabled =
            posizioa < irudiak.Count - 1;
    }


    // ============================================================
    // PREZIOAK
    // ============================================================

    /// <summary>
    /// Gehigarri baten egoera aldatzen denean
    /// prezio guztiak automatikoki berriz kalkulatzen ditu.
    /// </summary>
    /// <param name="sender">
    /// Egoera aldatu duen CheckBox kontrola.
    /// </param>
    /// <param name="e">
    /// CheckBox kontrolaren egoera berriaren informazioa.
    /// </param>
    private void OnGehigarriaChanged(
        object? sender,
        CheckedChangedEventArgs e)
    {
        KalkulatuPrezioak();

        BerrabiaraziInaktibitateTenporizadorea();
    }

    /// <summary>
    /// Hautatutako gehigarrien prezioa eta
    /// erreserbaren guztizko prezioa kalkulatzen ditu.
    ///
    /// Tarifak:
    /// 300 euro astea.
    /// 25 euro aireportuko joan-etorria.
    /// 150 euro gidaria duen txangoa.
    /// 40 euro eguneko haurtzaindegia.
    /// </summary>
    private void KalkulatuPrezioak()
    {
        decimal gehigarriak = 0m;

        if (AireportuaCheckBox.IsChecked)
        {
            gehigarriak +=
                AIREPORTUA_PREZIOA;
        }

        if (GidariaCheckBox.IsChecked)
        {
            gehigarriak +=
                GIDARIA_PREZIOA;
        }

        if (HaurtzaindegiaCheckBox.IsChecked)
        {
            gehigarriak +=
                HAURTZAINDegia_EGUNEKO_PREZIOA
                * ASTEKO_EGUNAK;
        }

        decimal guztira =
            ASTEKO_PREZIOA
            + gehigarriak;

        EgonaldiarenPrezioaEntry.Text =
            $"{ASTEKO_PREZIOA:N0} €";

        GehigarrienPrezioaEntry.Text =
            $"{gehigarriak:N0} €";

        GuztizkoaEntry.Text =
            $"{guztira:N0} €";
    }


    // ============================================================
    // DATA
    // ============================================================

    /// <summary>
    /// DatePicker kontrolaren hasierako data gaurko egunera ezartzen du.
    /// </summary>
    private void PrestatuHasierakoData()
    {
        AsteaDatePicker.Date =
            DateTime.Today;
    }

    /// <summary>
    /// Erabiltzaileak data berri bat aukeratzen duenean
    /// inaktibitate-kontagailua berrabiarazten du.
    /// </summary>
    /// <param name="sender">
    /// Data aldatu duen DatePicker kontrola.
    /// </param>
    /// <param name="e">
    /// Aurreko eta data berriaren informazioa.
    /// </param>
    private void OnDateSelected(
        object? sender,
        DateChangedEventArgs e)
    {
        BerrabiaraziInaktibitateTenporizadorea();
    }

    /// <summary>
    /// Hautatutako datari dagokion asteko astelehena kalkulatzen du.
    /// </summary>
    /// <param name="data">
    /// Erabiltzaileak hautatutako data.
    /// </param>
    /// <returns>
    /// Hautatutako asteko astelehena.
    /// </returns>
    private DateTime KalkulatuAstekoAstelehena(
        DateTime data)
    {
        int diferentzia =
            ((int)data.DayOfWeek + 6) % 7;

        return data.Date.AddDays(
            -diferentzia);
    }


    // ============================================================
    // ERRESERBAK
    // ============================================================

    /// <summary>
    /// Erreserben CollectionView kontrola
    /// ObservableCollection zerrendarekin lotzen du.
    /// </summary>
    private void PrestatuErreserbak()
    {
        ErreserbakCollectionView.ItemsSource =
            erreserbenTestuak;
    }

    /// <summary>
    /// Hautatutako astearen erreserba egiten saiatzen da.
    ///
    /// Astea libre badago gordetzen du.
    /// Aurretik erreserbatuta badago errore-mezua erakusten du.
    /// </summary>
    /// <param name="sender">
    /// Erreserba egin botoia.
    /// </param>
    /// <param name="e">
    /// Klik-gertaeraren informazioa.
    /// </param>
    private async void OnErreserbaClicked(
        object? sender,
        EventArgs e)
    {
        BerrabiaraziInaktibitateTenporizadorea();

        // DatePicker kontrolaren data nullable izan daiteke.
        DateTime hautatutakoData =
            AsteaDatePicker.Date
            ?? DateTime.Today;

        DateTime astelehena =
            KalkulatuAstekoAstelehena(
                hautatutakoData);


        // Astea aurretik erreserbatuta dagoen egiaztatzen da.
        if (erreserbatutakoAsteak.Contains(astelehena))
        {
            await DisplayAlertAsync(
                "Erreserba",
                "Aste horretan ezin da erreserba egin.",
                "Ados");

            return;
        }


        // Aste berria gordetzen da.
        erreserbatutakoAsteak.Add(
            astelehena);


        // Pantailan erakusteko data-formatua sortzen da.
        string asteTestua =
            FormateatuDataEuskaraz(
                astelehena);

        erreserbenTestuak.Add(
            asteTestua);


        await DisplayAlertAsync(
            "Erreserba",
            "Adierazitako astean egindako erreserba.",
            "Ados");
    }


    // ============================================================
    // DATAREN FORMATUA
    // ============================================================

    /// <summary>
    /// DateTime objektu bat euskarazko testu-formatuan bihurtzen du.
    ///
    /// Adibidea:
    /// 2026ko urriaren 5a.
    /// </summary>
    /// <param name="data">
    /// Formateatu behar den data.
    /// </param>
    /// <returns>
    /// Data euskaraz formateatuta.
    /// </returns>
    private string FormateatuDataEuskaraz(
        DateTime data)
    {
        string[] hilabeteak =
        {
            "urtarrilaren",
            "otsailaren",
            "martxoaren",
            "apirilaren",
            "maiatzaren",
            "ekainaren",
            "uztailaren",
            "abuztuaren",
            "irailaren",
            "urriaren",
            "azaroaren",
            "abenduaren"
        };

        string hilabetea =
            hilabeteak[data.Month - 1];

        return
            $"{data.Year}ko {hilabetea} {data.Day}a";
    }


    // ============================================================
    // 10 SEGUNDOKO INAKTIBITATEA
    // ============================================================

    /// <summary>
    /// Erabiltzailearen inaktibitatea kontrolatzeko
    /// 10 segundoko tenporizadorea prestatzen du.
    ///
    /// 10 segundoz elkarreraginik ez badago,
    /// CarouselView kontrolaren irudia ausaz aldatzen da.
    /// </summary>
    private void PrestatuInaktibitateTenporizadorea()
    {
        if (inaktibitateTenporizadorea != null)
        {
            BerrabiaraziInaktibitateTenporizadorea();
            return;
        }

        inaktibitateTenporizadorea =
            Dispatcher.CreateTimer();

        inaktibitateTenporizadorea.Interval =
            TimeSpan.FromSeconds(10);

        inaktibitateTenporizadorea.Tick +=
            OnInaktibitateTick;

        inaktibitateTenporizadorea.Start();
    }

    /// <summary>
    /// 10 segundo inaktibo igarotzean beste irudi bat
    /// ausaz hautatzen du.
    /// </summary>
    /// <param name="sender">
    /// Gertaera sortu duen tenporizadorea.
    /// </param>
    /// <param name="e">
    /// Tick gertaeraren informazioa.
    /// </param>
    private void OnInaktibitateTick(
        object? sender,
        EventArgs e)
    {
        AldatuIrudiaAusaz();
    }

    /// <summary>
    /// Uneko iruditik desberdina den beste irudi baten
    /// posizioa ausaz hautatzen du.
    /// </summary>
    private void AldatuIrudiaAusaz()
    {
        if (irudiak.Count <= 1)
        {
            return;
        }

        int unekoPosizioa =
            GelakCarousel.Position;

        int posizioBerria;

        do
        {
            posizioBerria =
                ausazkoa.Next(
                    0,
                    irudiak.Count);
        }
        while (posizioBerria == unekoPosizioa);

        GelakCarousel.Position =
            posizioBerria;

        EguneratuCarouselKontrolak();

        BerrabiaraziInaktibitateTenporizadorea();
    }

    /// <summary>
    /// Erabiltzaileak aplikazioarekin elkarreragiten duenean
    /// 10 segundoko inaktibitate-kontagailua berrabiarazten du.
    /// </summary>
    private void BerrabiaraziInaktibitateTenporizadorea()
    {
        if (inaktibitateTenporizadorea == null)
        {
            return;
        }

        inaktibitateTenporizadorea.Stop();
        inaktibitateTenporizadorea.Start();
    }


    // ============================================================
    // IRTEN
    // ============================================================

    /// <summary>
    /// Irten botoia sakatzen denean tenporizadorea gelditu
    /// eta aplikazioaren leihoa ixten du.
    /// </summary>
    /// <param name="sender">
    /// Gertaera sortu duen botoia.
    /// </param>
    /// <param name="e">
    /// Klik-gertaeraren informazioa.
    /// </param>
    private void OnIrtenClicked(
        object? sender,
        EventArgs e)
    {
        inaktibitateTenporizadorea?.Stop();

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