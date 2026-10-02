using Microsoft.Maui.Controls.Shapes;
using System.Text;

namespace I01_08_TxandakAusazOsatzekoAplikazioa;

/// <summary>
/// Aplikazioaren orri nagusia.
/// Ikasleen izenak testu-fitxategi batetik kargatu,
/// ikasleak ausaz eta errepikatu gabe aukeratu,
/// ateratako ordena erakutsi eta azken emaitza
/// testu-fitxategi batean gordetzeko erabiltzen da.
/// </summary>
public partial class MainPage : ContentPage
{
    /// <summary>
    /// Fitxategitik kargatutako ikasle guztien
    /// jatorrizko zerrenda gordetzen du.
    /// </summary>
    private readonly List<string> ikasleGuztiak = new List<string>();

    /// <summary>
    /// Zozketan oraindik atera ez diren
    /// ikasleen izenak gordetzen ditu.
    /// </summary>
    private readonly List<string> geratzenDirenIkasleak = new List<string>();

    /// <summary>
    /// Zozketan dagoeneko atera diren ikasleak
    /// ateratako ordena berean gordetzen ditu.
    /// </summary>
    private readonly List<string> ateratakoIkasleak = new List<string>();

    /// <summary>
    /// Ikasleak ausaz aukeratzeko erabiltzen den
    /// ausazko zenbakien sortzailea.
    /// </summary>
    private readonly Random ausazkoa = new Random();


    /// <summary>
    /// MainPage klasearen eraikitzailea.
    /// XAML fitxategian definitutako interfaze grafikoa
    /// sortu eta hasieratzen du.
    /// </summary>
    public MainPage()
    {
        InitializeComponent();
    }


    /// <summary>
    /// "Kargatu izen-zerrenda" botoia sakatzean exekutatzen da.
    /// Erabiltzaileari .txt fitxategi bat aukeratzeko aukera ematen dio
    /// eta fitxategiko ikasleen izenak aplikazioan kargatzen ditu.
    /// </summary>
    /// <param name="sender">
    /// Gertaera sortu duen objektua, kasu honetan kargatzeko botoia.
    /// </param>
    /// <param name="e">
    /// Klik-gertaerari buruzko informazioa.
    /// </param>
    private async void OnKargatuClicked(object sender, EventArgs e)
    {
        try
        {
            // Erabiltzaileari fitxategi bat aukeratzeko leihoa erakusten zaio.
            FileResult? fitxategia = await FilePicker.Default.PickAsync(
                new PickOptions
                {
                    PickerTitle = "Aukeratu ikasleen izenen testu-fitxategia"
                });

            // Erabiltzaileak leihoa bertan behera uzten badu,
            // ez da beste ekintzarik egiten.
            if (fitxategia == null)
            {
                return;
            }

            // Aukeratutako fitxategiak .txt luzapena duela egiaztatzen da.
            if (!fitxategia.FileName.EndsWith(
                    ".txt",
                    StringComparison.OrdinalIgnoreCase))
            {
                await DisplayAlertAsync(
                    "Errorea",
                    "Testu-fitxategi bat (.txt) aukeratu behar duzu.",
                    "Ados");

                return;
            }

            await KargatuIkasleak(fitxategia);
        }
        catch (Exception ex)
        {
            // Fitxategia irekitzean ustekabeko errore bat gertatzen bada,
            // erabiltzaileari mezua erakusten zaio.
            await DisplayAlertAsync(
                "Errorea",
                "Ezin izan da fitxategia kargatu:\n" + ex.Message,
                "Ados");
        }
    }


    /// <summary>
    /// Aukeratutako testu-fitxategia irakurtzen du eta
    /// lerro bakoitzeko ikaslearen izena aplikazioaren
    /// zerrendetan gordetzen du.
    /// Lerro hutsak ez dira kontuan hartzen.
    /// </summary>
    /// <param name="fitxategia">
    /// Erabiltzaileak FilePicker bidez aukeratutako testu-fitxategia.
    /// </param>
    private async Task KargatuIkasleak(FileResult fitxategia)
    {
        // Aurreko datuak ezabatzen dira fitxategi berri bat kargatzean.
        ikasleGuztiak.Clear();
        geratzenDirenIkasleak.Clear();
        ateratakoIkasleak.Clear();

        // Fitxategia irakurtzeko korrontea irekitzen da.
        using Stream stream = await fitxategia.OpenReadAsync();
        using StreamReader reader = new StreamReader(stream);

        string? lerroa;

        // Fitxategia lerroz lerro irakurtzen da.
        while ((lerroa = await reader.ReadLineAsync()) != null)
        {
            string izena = lerroa.Trim();

            // Lerro hutsek ez dute ikaslerik adierazten,
            // beraz ez dira zerrendan sartzen.
            if (izena != "")
            {
                ikasleGuztiak.Add(izena);
            }
        }

        // Fitxategian baliozko izenik ez badago,
        // erabiltzaileari errore-mezua erakusten zaio.
        if (ikasleGuztiak.Count == 0)
        {
            FitxategiaLabel.Text = "Ez da fitxategirik kargatu";

            await DisplayAlertAsync(
                "Errorea",
                "Fitxategiak ez dauka baliozko ikasle-izenik.",
                "Ados");

            return;
        }

        // Hasierako zozketan ikasle guztiak geratzen diren
        // ikasleen zerrendara kopiatzen dira.
        geratzenDirenIkasleak.AddRange(ikasleGuztiak);

        // Aukeratutako fitxategiaren izena interfazean erakusten da.
        FitxategiaLabel.Text = fitxategia.FileName;

        // Uneko ikaslearen eremua hasierako egoerara itzultzen da.
        UnekoIkasleaLabel.Text = "---";

        // Eskuineko zerrenda hasieratzen da.
        ErakutsiHasierakoMezua();

        // Kontagailuak eguneratzen dira.
        EguneratuKontagailuak();

        // Zozketa egiteko eta berrabiarazteko botoiak aktibatzen dira.
        HurrengoaBtn.IsEnabled = true;
        GarbituBtn.IsEnabled = true;

        // Oraindik ez dago azken emaitzarik gordetzeko.
        GordeBtn.IsEnabled = false;

        await DisplayAlertAsync(
            "Fitxategia kargatuta",
            $"{ikasleGuztiak.Count} ikasle kargatu dira.",
            "Ados");
    }


    /// <summary>
    /// "Hurrengo izena atera" botoia sakatzean exekutatzen da.
    /// Oraindik atera ez den ikasle bat ausaz aukeratu,
    /// geratzen diren ikasleen zerrendatik kendu eta
    /// ateratakoen zerrendara gehitzen du.
    /// </summary>
    /// <param name="sender">
    /// Gertaera sortu duen objektua, kasu honetan zozketako botoia.
    /// </param>
    /// <param name="e">
    /// Klik-gertaerari buruzko informazioa.
    /// </param>
    private async void OnHurrengoaClicked(object sender, EventArgs e)
    {
        // Fitxategirik kargatu ez bada, ezin da zozketarik egin.
        if (ikasleGuztiak.Count == 0)
        {
            await DisplayAlertAsync(
                "Abisua",
                "Lehenengo ikasleen fitxategia kargatu behar duzu.",
                "Ados");

            return;
        }

        // Ikasle guztiak dagoeneko atera badira,
        // ez dago beste izenik aukeratzeko.
        if (geratzenDirenIkasleak.Count == 0)
        {
            await DisplayAlertAsync(
                "Zozketa amaituta",
                "Ikasle guztiak atera dira.",
                "Ados");

            return;
        }

        // Geratzen diren ikasleen artean ausazko posizio bat aukeratzen da.
        int posizioa = ausazkoa.Next(geratzenDirenIkasleak.Count);

        // Aukeratutako ikaslearen izena lortzen da.
        string ikaslea = geratzenDirenIkasleak[posizioa];

        // Ikaslea geratzen diren ikasleen zerrendatik kentzen da,
        // berriro atera ez dadin.
        geratzenDirenIkasleak.RemoveAt(posizioa);

        // Ikaslea ateratako ikasleen zerrendara gehitzen da.
        ateratakoIkasleak.Add(ikaslea);

        // Uneko ikaslea pantailan erakusten da.
        ErakutsiIkaslea(ikaslea);

        // Ikaslea eskuineko zerrenda ikusgarrian gehitzen da.
        GehituIkasleaZerrendara(ikaslea);

        // Kontagailuak egoera berriarekin eguneratzen dira.
        EguneratuKontagailuak();

        // Ikasle guztiak atera badira, zozketa amaitzen da.
        if (geratzenDirenIkasleak.Count == 0)
        {
            HurrengoaBtn.IsEnabled = false;
            GordeBtn.IsEnabled = true;

            await DisplayAlertAsync(
                "Zozketa amaituta",
                "Ikasle guztiak atera dira. Orain emaitza gorde dezakezu.",
                "Ados");
        }
    }


    /// <summary>
    /// Aukeratutako ikaslearen izena pantailako
    /// "Uneko ikaslea" eremuan erakusten du.
    /// </summary>
    /// <param name="izena">
    /// Pantailan erakutsi behar den ikaslearen izena.
    /// </param>
    private void ErakutsiIkaslea(string izena)
    {
        UnekoIkasleaLabel.Text = izena;
    }


    /// <summary>
    /// Ausaz ateratako ikasle berri bat eskuineko
    /// zerrenda ikusgarrian gehitzen du.
    /// Ikaslearen posizioa eta izena erakusten dira.
    /// </summary>
    /// <param name="izena">
    /// Zerrenda ikusgarrian gehitu behar den ikaslearen izena.
    /// </param>
    private void GehituIkasleaZerrendara(string izena)
    {
        // Lehenengo ikaslea ateratzen denean
        // hasierako laguntza-mezua ezabatzen da.
        if (ateratakoIkasleak.Count == 1)
        {
            AteratakoenZerrenda.Children.Clear();
        }

        int zenbakia = ateratakoIkasleak.Count;

        // Ikasle bakoitza Border baten barruan erakusten da
        // zerrenda argiago eta irakurgarriago egiteko.
        Border ikasleBorder = new Border
        {
            Padding = 12,
            Stroke = Color.FromArgb("#D9E2F2"),
            StrokeThickness = 1,
            BackgroundColor = Colors.White,

            StrokeShape = new RoundRectangle
            {
                CornerRadius = 8
            },

            Content = new Label
            {
                Text = $"{zenbakia}. {izena}",
                FontSize = 17,
                TextColor = Color.FromArgb("#163A70")
            }
        };

        AteratakoenZerrenda.Children.Add(ikasleBorder);
    }


    /// <summary>
    /// Aplikazioko hiru kontagailuak eguneratzen ditu:
    /// ikasle guztien kopurua, ateratakoen kopurua
    /// eta oraindik falta diren ikasleen kopurua.
    /// </summary>
    private void EguneratuKontagailuak()
    {
        GuztiraLabel.Text = ikasleGuztiak.Count.ToString();
        AteraDirenakLabel.Text = ateratakoIkasleak.Count.ToString();
        FaltaDirenakLabel.Text = geratzenDirenIkasleak.Count.ToString();
    }


    /// <summary>
    /// Ateratako ikasleen zerrenda ikusgarria garbitu
    /// eta hasierako laguntza-mezua erakusten du.
    /// </summary>
    private void ErakutsiHasierakoMezua()
    {
        AteratakoenZerrenda.Children.Clear();

        Label mezua = new Label
        {
            Text = "Oraindik ez da ikaslerik atera.",
            TextColor = Color.FromArgb("#777777"),
            FontSize = 15
        };

        AteratakoenZerrenda.Children.Add(mezua);
    }


    /// <summary>
    /// "Garbitu" botoia sakatzean exekutatzen da.
    /// Uneko zozketa ezabatu eta hasieratik berrabiarazten du,
    /// aurretik kargatutako ikasleen fitxategia mantenduz.
    /// </summary>
    /// <param name="sender">
    /// Gertaera sortu duen objektua, kasu honetan garbitzeko botoia.
    /// </param>
    /// <param name="e">
    /// Klik-gertaerari buruzko informazioa.
    /// </param>
    private void OnGarbituClicked(object sender, EventArgs e)
    {
        // Fitxategirik kargatu ez bada, ez dago berrabiarazteko ezer.
        if (ikasleGuztiak.Count == 0)
        {
            return;
        }

        // Aurreko zozketako datuak ezabatzen dira.
        ateratakoIkasleak.Clear();
        geratzenDirenIkasleak.Clear();

        // Ikasle guztiak berriro zozketan sartzen dira.
        geratzenDirenIkasleak.AddRange(ikasleGuztiak);

        // Interfazea hasierako egoerara itzultzen da.
        UnekoIkasleaLabel.Text = "---";

        ErakutsiHasierakoMezua();
        EguneratuKontagailuak();

        HurrengoaBtn.IsEnabled = true;
        GordeBtn.IsEnabled = false;
    }


    /// <summary>
    /// Ateratako ikasleen azken ordenarekin
    /// testu-fitxategian gordeko den edukia sortzen du.
    /// </summary>
    /// <returns>
    /// Zozketaren emaitza zenbakitua duen testu-katea.
    /// </returns>
    private string SortuEmaitzaTestua()
    {
        StringBuilder testua = new StringBuilder();

        testua.AppendLine("TXANDAK AUSAZ OSATZEKO APLIKAZIOA");
        testua.AppendLine("--------------------------------");
        testua.AppendLine();

        // Ikasle bakoitza ateratako ordenarekin idazten da.
        for (int i = 0; i < ateratakoIkasleak.Count; i++)
        {
            testua.AppendLine($"{i + 1}. {ateratakoIkasleak[i]}");
        }

        return testua.ToString();
    }


    /// <summary>
    /// "Emaitza fitxategian gorde" botoia sakatzean exekutatzen da.
    /// Zozketaren azken emaitza .txt fitxategi batean gordetzeko
    /// Windows-eko gordetzeko leihoa irekitzen du.
    /// </summary>
    /// <param name="sender">
    /// Gertaera sortu duen objektua, kasu honetan gordetzeko botoia.
    /// </param>
    /// <param name="e">
    /// Klik-gertaerari buruzko informazioa.
    /// </param>
    private async void OnGordeClicked(object sender, EventArgs e)
    {
        // Zozketa oraindik amaitu ez bada,
        // ezin da azken emaitza gorde.
        if (ateratakoIkasleak.Count == 0 ||
            geratzenDirenIkasleak.Count != 0)
        {
            await DisplayAlertAsync(
                "Abisua",
                "Lehenengo zozketa amaitu behar duzu.",
                "Ados");

            return;
        }

#if WINDOWS

        try
        {
            // Windows-eko fitxategia gordetzeko hautatzailea sortzen da.
            Windows.Storage.Pickers.FileSavePicker picker =
                new Windows.Storage.Pickers.FileSavePicker();

            picker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;

            picker.SuggestedFileName = "txanden_emaitza";

            // Erabiltzaileak .txt fitxategi bat sortuko du.
            picker.FileTypeChoices.Add(
                "Testu-fitxategia",
                new List<string> { ".txt" });

            // MAUI aplikazioaren Windows leiho nagusia lortzen da.
            Microsoft.Maui.Controls.Application? aplikazioa =
                Microsoft.Maui.Controls.Application.Current;

            if (aplikazioa == null || aplikazioa.Windows.Count == 0)
            {
                await DisplayAlertAsync(
                    "Errorea",
                    "Ezin izan da aplikazioaren leihoa aurkitu.",
                    "Ados");

                return;
            }

            Microsoft.Maui.Controls.Window mauiLeihoa =
                aplikazioa.Windows[0];

            Microsoft.UI.Xaml.Window? windowsLeihoa =
                mauiLeihoa.Handler?.PlatformView as Microsoft.UI.Xaml.Window;

            if (windowsLeihoa == null)
            {
                await DisplayAlertAsync(
                    "Errorea",
                    "Ezin izan da Windows leihoa aurkitu.",
                    "Ados");

                return;
            }

            // Windows-eko hautatzailea aplikazioaren leihoarekin lotzen da.
            IntPtr hwnd =
                WinRT.Interop.WindowNative.GetWindowHandle(windowsLeihoa);

            WinRT.Interop.InitializeWithWindow.Initialize(
                picker,
                hwnd);

            // Erabiltzaileak fitxategiaren izena eta kokapena aukeratzen ditu.
            Windows.Storage.StorageFile? fitxategia =
                await picker.PickSaveFileAsync();

            // Erabiltzaileak gordetzeko leihoa bertan behera utzi badu,
            // ez da fitxategirik sortzen.
            if (fitxategia == null)
            {
                return;
            }

            // Zozketaren emaitza testu bihurtzen da.
            string emaitza = SortuEmaitzaTestua();

            // Testua erabiltzaileak aukeratutako fitxategian idazten da.
            await Windows.Storage.FileIO.WriteTextAsync(
                fitxategia,
                emaitza);

            await DisplayAlertAsync(
                "Emaitza gordeta",
                "Zozketaren emaitza behar bezala gorde da.",
                "Ados");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Errorea",
                "Ezin izan da fitxategia gorde:\n" + ex.Message,
                "Ados");
        }

#else

        await DisplayAlertAsync(
            "Abisua",
            "Fitxategia gordetzeko aukera Windows bertsiorako prestatu da.",
            "Ados");

#endif
    }
}