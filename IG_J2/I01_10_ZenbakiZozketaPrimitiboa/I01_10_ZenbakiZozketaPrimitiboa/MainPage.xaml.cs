namespace I01_10_ZenbakiZozketaPrimitiboa;

/// <summary>
/// Primitiba motako zenbaki-zozketaren aplikazioaren orri nagusia.
///
/// Aplikazioak 1etik 49ra arteko 49 CheckBox sortzen ditu.
/// Erabiltzaileak 6 zenbaki hautatu behar ditu.
///
/// Zozketa egitean, makinak ausaz eta errepikatu gabe
/// 6 zenbaki saritu sortzen ditu.
///
/// Ondoren, erabiltzaileak zenbat zenbaki asmatu dituen
/// kalkulatzen da eta dagokion saria erakusten da.
/// </summary>
public partial class MainPage : ContentPage
{
    /// <summary>
    /// Erabiltzaileak hautatutako zenbakiak gordetzen ditu.
    /// </summary>
    private readonly List<int> hautatutakoZenbakiak = new();

    /// <summary>
    /// Pantailan dinamikoki sortutako 49 CheckBox kontrolak gordetzen ditu.
    /// Zenbakia eta bere CheckBox kontrola erlazionatzen dira.
    /// </summary>
    private readonly Dictionary<int, CheckBox> checkboxak = new();

    /// <summary>
    /// Ausazko zenbakiak sortzeko erabiltzen den objektua.
    /// </summary>
    private readonly Random ausazkoa = new();

    /// <summary>
    /// CheckBox kontrolak programatikoki eguneratzen ari diren
    /// adierazten du. CheckedChanged gertaera alferrik exekutatzea
    /// saihesteko erabiltzen da.
    /// </summary>
    private bool checkboxakEguneratzen;

    /// <summary>
    /// Erabiltzaileak hautatu behar duen zenbaki kopurua.
    /// </summary>
    private const int HAUTAKETA_KOPURUA = 6;

    /// <summary>
    /// Zozketan erabil daitekeen zenbaki handiena.
    /// </summary>
    private const int GEHIENEZKO_ZENBAKIA = 49;

    /// <summary>
    /// MainPage klasearen eraikitzailea.
    /// Interfaze grafikoa hasieratu, 49 CheckBox kontrolak
    /// sortu eta aplikazioa hasierako egoeran uzten du.
    /// </summary>
    public MainPage()
    {
        InitializeComponent();

        SortuZenbakiCheckboxak();
        BerrabiaraziAplikazioa();
    }


    // ============================================================
    // 49 CHECKBOX KONTROLEN SORRERA
    // ============================================================

    /// <summary>
    /// 1etik 49ra arteko CheckBox kontrolak dinamikoki sortzen ditu
    /// eta 7x7 Grid batean kokatzen ditu.
    /// </summary>
    private void SortuZenbakiCheckboxak()
    {
        // Grid-aren aurreko konfigurazioa garbitzen da.
        ZenbakiakGrid.Children.Clear();
        ZenbakiakGrid.RowDefinitions.Clear();
        ZenbakiakGrid.ColumnDefinitions.Clear();

        // Zazpi zutabe sortzen dira.
        for (int zutabea = 0; zutabea < 7; zutabea++)
        {
            ZenbakiakGrid.ColumnDefinitions.Add(
                new ColumnDefinition
                {
                    Width = GridLength.Star
                });
        }

        // Zazpi errenkada sortzen dira.
        for (int errenkada = 0; errenkada < 7; errenkada++)
        {
            ZenbakiakGrid.RowDefinitions.Add(
                new RowDefinition
                {
                    Height = GridLength.Auto
                });
        }

        // 1etik 49ra arteko kontrolak sortzen dira.
        for (int zenbakia = 1;
             zenbakia <= GEHIENEZKO_ZENBAKIA;
             zenbakia++)
        {
            SortuZenbakiKontrola(zenbakia);
        }
    }

    /// <summary>
    /// Zenbaki bakar baten CheckBox eta Label kontrolak sortzen ditu
    /// eta dagokien posizioan kokatzen ditu.
    /// </summary>
    /// <param name="zenbakia">
    /// Sortu behar den kontrolari dagokion zenbakia.
    /// </param>
    private void SortuZenbakiKontrola(int zenbakia)
    {
        CheckBox checkbox = new CheckBox
        {
            ClassId = zenbakia.ToString(),

            HorizontalOptions =
                LayoutOptions.Center
        };

        checkbox.CheckedChanged +=
            OnZenbakiaCheckedChanged;

        Label zenbakiaLabel = new Label
        {
            Text = zenbakia.ToString(),

            FontSize = 15,

            FontAttributes =
                FontAttributes.Bold,

            TextColor =
                Color.FromArgb("#163A70"),

            VerticalTextAlignment =
                TextAlignment.Center,

            InputTransparent = true
        };

        HorizontalStackLayout edukia =
            new HorizontalStackLayout
            {
                Spacing = 2,

                HorizontalOptions =
                    LayoutOptions.Center,

                VerticalOptions =
                    LayoutOptions.Center,

                Children =
                {
                    checkbox,
                    zenbakiaLabel
                }
            };

        Border zenbakiaBorder =
            new Border
            {
                Padding = new Thickness(5),

                BackgroundColor =
                    Color.FromArgb("#F8FAFC"),

                Stroke =
                    Color.FromArgb("#E1E7F0"),

                StrokeThickness = 1,

                StrokeShape =
                    new Microsoft.Maui.Controls.Shapes.RoundRectangle
                    {
                        CornerRadius = 8
                    },

                Content = edukia
            };

        // 7x7 sareko posizioa kalkulatzen da.
        int indizea = zenbakia - 1;

        int errenkada =
            indizea / 7;

        int zutabea =
            indizea % 7;

        Grid.SetRow(
            zenbakiaBorder,
            errenkada);

        Grid.SetColumn(
            zenbakiaBorder,
            zutabea);

        ZenbakiakGrid.Children.Add(
            zenbakiaBorder);

        // Zenbakia eta CheckBox kontrola erlazionatzen dira.
        checkboxak.Add(
            zenbakia,
            checkbox);
    }


    // ============================================================
    // ZENBAKIEN HAUTAKETA
    // ============================================================

    /// <summary>
    /// Zenbaki baten CheckBox egoera aldatzen denean exekutatzen da.
    ///
    /// Gehienez 6 zenbaki hautatzea baimentzen du eta
    /// sei zenbaki hautatuta daudenean Zozketa botoia aktibatzen du.
    /// </summary>
    /// <param name="sender">
    /// Egoera aldatu duen CheckBox kontrola.
    /// </param>
    /// <param name="e">
    /// CheckBox kontrolaren egoera berriari buruzko informazioa.
    /// </param>
    private void OnZenbakiaCheckedChanged(
        object? sender,
        CheckedChangedEventArgs e)
    {
        // CheckBox kontrolak programatikoki aldatzen ari badira,
        // gertaera ez da prozesatzen.
        if (checkboxakEguneratzen)
        {
            return;
        }

        if (sender is not CheckBox checkbox)
        {
            return;
        }

        if (!int.TryParse(
                checkbox.ClassId,
                out int zenbakia))
        {
            return;
        }

        if (e.Value)
        {
            // Sei zenbaki dagoeneko hautatuta badaude,
            // zazpigarren aukera ez da baimentzen.
            if (hautatutakoZenbakiak.Count >= HAUTAKETA_KOPURUA)
            {
                checkboxakEguneratzen = true;
                checkbox.IsChecked = false;
                checkboxakEguneratzen = false;

                EgoeraLabel.Text =
                    "Gehienez 6 zenbaki hauta ditzakezu.";

                return;
            }

            hautatutakoZenbakiak.Add(
                zenbakia);
        }
        else
        {
            hautatutakoZenbakiak.Remove(
                zenbakia);
        }

        EguneratuHautaketarenEgoera();
    }

    /// <summary>
    /// Hautatutako zenbaki kopuruaren informazioa
    /// pantailan eguneratzen du.
    ///
    /// Sei zenbaki hautatzen direnean,
    /// Zozketa botoia aktibatzen da.
    /// </summary>
    private void EguneratuHautaketarenEgoera()
    {
        int kopurua =
            hautatutakoZenbakiak.Count;

        HautatutakoakLabel.Text =
            $"{kopurua} / {HAUTAKETA_KOPURUA}";

        ZozketaBtn.IsEnabled =
            kopurua == HAUTAKETA_KOPURUA;

        if (kopurua == HAUTAKETA_KOPURUA)
        {
            EgoeraLabel.Text =
                "6 zenbaki hautatu dituzu. Zozketa egin dezakezu.";
        }
        else
        {
            int falta =
                HAUTAKETA_KOPURUA - kopurua;

            EgoeraLabel.Text =
                $"Hautatu beste {falta} zenbaki.";
        }
    }


    // ============================================================
    // ZOZKETA
    // ============================================================

    /// <summary>
    /// Zozketa botoia sakatzen denean exekutatzen da.
    ///
    /// CheckBox guztiak desgaitu, sei zenbaki saritu sortu,
    /// emaitzak erakutsi eta erabiltzailearen acierto kopurua
    /// kalkulatzen du.
    /// </summary>
    /// <param name="sender">
    /// Zozketa botoia.
    /// </param>
    /// <param name="e">
    /// Klik-gertaerari buruzko informazioa.
    /// </param>
    private void OnZozketaClicked(
        object sender,
        EventArgs e)
    {
        if (hautatutakoZenbakiak.Count != HAUTAKETA_KOPURUA)
        {
            return;
        }

        DesgaituCheckboxGuztiak();

        List<int> zenbakiSarituak =
            SortuZenbakiSarituak();

        ErakutsiZenbakiSarituak(
            zenbakiSarituak);

        int asmatutakoak =
            KalkulatuAsmatutakoak(
                zenbakiSarituak);

        decimal saria =
            KalkulatuSaria(
                asmatutakoak);

        ErakutsiEmaitza(
            asmatutakoak,
            saria);

        ZozketaBtn.IsEnabled = false;
        BerriaBtn.IsEnabled = true;

        EgoeraLabel.Text =
            "Zozketa amaitu da.";
    }

    /// <summary>
    /// 1etik 49ra arteko sei zenbaki desberdin ausaz sortzen ditu.
    /// </summary>
    /// <returns>
    /// Goranzko ordenan antolatutako sei zenbaki sarituen zerrenda.
    /// </returns>
    private List<int> SortuZenbakiSarituak()
    {
        HashSet<int> zenbakiak = new();

        // HashSet-ek zenbaki errepikatuak automatikoki saihesten ditu.
        while (zenbakiak.Count < HAUTAKETA_KOPURUA)
        {
            int zenbakia =
                ausazkoa.Next(
                    1,
                    GEHIENEZKO_ZENBAKIA + 1);

            zenbakiak.Add(
                zenbakia);
        }

        List<int> emaitza =
            zenbakiak.ToList();

        emaitza.Sort();

        return emaitza;
    }

    /// <summary>
    /// Sei zenbaki sarituak pantailako sei Entry kontroletan erakusten ditu.
    /// </summary>
    /// <param name="zenbakiSarituak">
    /// Zozketan sortutako sei zenbaki sarituen zerrenda.
    /// </param>
    private void ErakutsiZenbakiSarituak(
        List<int> zenbakiSarituak)
    {
        Zenbaki1Entry.Text =
            zenbakiSarituak[0].ToString();

        Zenbaki2Entry.Text =
            zenbakiSarituak[1].ToString();

        Zenbaki3Entry.Text =
            zenbakiSarituak[2].ToString();

        Zenbaki4Entry.Text =
            zenbakiSarituak[3].ToString();

        Zenbaki5Entry.Text =
            zenbakiSarituak[4].ToString();

        Zenbaki6Entry.Text =
            zenbakiSarituak[5].ToString();
    }

    /// <summary>
    /// Erabiltzailearen zenbakien eta zenbaki sarituen artean
    /// zenbat berdintasun dauden kalkulatzen du.
    /// </summary>
    /// <param name="zenbakiSarituak">
    /// Zozketan sortutako zenbaki sarituak.
    /// </param>
    /// <returns>
    /// Erabiltzaileak asmatutako zenbaki kopurua.
    /// </returns>
    private int KalkulatuAsmatutakoak(
        List<int> zenbakiSarituak)
    {
        int asmatutakoak = 0;

        foreach (int zenbakia in hautatutakoZenbakiak)
        {
            if (zenbakiSarituak.Contains(zenbakia))
            {
                asmatutakoak++;
            }
        }

        return asmatutakoak;
    }


    // ============================================================
    // SARIA
    // ============================================================

    /// <summary>
    /// Asmatutako zenbaki kopuruaren arabera
    /// erabiltzailearen saria kalkulatzen du.
    /// </summary>
    /// <param name="asmatutakoak">
    /// Erabiltzaileak asmatu duen zenbaki kopurua.
    /// </param>
    /// <returns>
    /// Erabiltzaileari dagokion saria eurotan.
    /// </returns>
    private decimal KalkulatuSaria(
        int asmatutakoak)
    {
        return asmatutakoak switch
        {
            6 => 1_000_000m,
            5 => 1_000m,
            4 => 50m,
            3 => 8m,
            _ => 0m
        };
    }

    /// <summary>
    /// Erabiltzailearen acierto kopurua eta lortutako saria
    /// emaitzaren Entry kontrolean erakusten ditu.
    /// </summary>
    /// <param name="asmatutakoak">
    /// Erabiltzaileak asmatu duen zenbaki kopurua.
    /// </param>
    /// <param name="saria">
    /// Erabiltzaileak irabazitako saria eurotan.
    /// </param>
    private void ErakutsiEmaitza(
        int asmatutakoak,
        decimal saria)
    {
        if (saria > 0)
        {
            EmaitzaEntry.Text =
                $"Asmatutakoak: {asmatutakoak} | Saria: {saria:N0} €";
        }
        else
        {
            EmaitzaEntry.Text =
                $"Asmatutakoak: {asmatutakoak} | Saririk ez";
        }
    }

    /// <summary>
    /// Pantailako 49 CheckBox kontrolak desgaitzen ditu.
    /// Zozketa egin ondoren erabiltzaileak bere hautaketa
    /// aldatu ezin izatea bermatzen du.
    /// </summary>
    private void DesgaituCheckboxGuztiak()
    {
        foreach (CheckBox checkbox in checkboxak.Values)
        {
            checkbox.IsEnabled = false;
        }
    }


    // ============================================================
    // BERRIA
    // ============================================================

    /// <summary>
    /// Berria botoia sakatzen denean aplikazioa
    /// hasierako egoerara itzultzen du.
    /// </summary>
    /// <param name="sender">
    /// Berria botoia.
    /// </param>
    /// <param name="e">
    /// Klik-gertaerari buruzko informazioa.
    /// </param>
    private void OnBerriaClicked(
        object sender,
        EventArgs e)
    {
        BerrabiaraziAplikazioa();
    }

    /// <summary>
    /// Aplikazioaren datu guztiak garbitzen ditu eta
    /// hasierako egoera berreskuratzen du.
    ///
    /// CheckBox guztiak desmarkatu eta aktibatzen ditu,
    /// emaitzak garbitzen ditu eta Berria nahiz
    /// Zozketa botoiak desgaitzen ditu.
    /// </summary>
    private void BerrabiaraziAplikazioa()
    {
        checkboxakEguneratzen = true;

        hautatutakoZenbakiak.Clear();

        foreach (CheckBox checkbox in checkboxak.Values)
        {
            checkbox.IsChecked = false;
            checkbox.IsEnabled = true;
        }

        checkboxakEguneratzen = false;

        GarbituEmaitzak();

        HautatutakoakLabel.Text =
            "0 / 6";

        EgoeraLabel.Text =
            "Hautatu 6 zenbaki.";

        ZozketaBtn.IsEnabled = false;
        BerriaBtn.IsEnabled = false;
    }

    /// <summary>
    /// Aurreko zozketaren zenbaki sarituak eta
    /// emaitzaren informazioa pantailatik kentzen ditu.
    /// </summary>
    private void GarbituEmaitzak()
    {
        Zenbaki1Entry.Text = string.Empty;
        Zenbaki2Entry.Text = string.Empty;
        Zenbaki3Entry.Text = string.Empty;
        Zenbaki4Entry.Text = string.Empty;
        Zenbaki5Entry.Text = string.Empty;
        Zenbaki6Entry.Text = string.Empty;

        EmaitzaEntry.Text =
            string.Empty;
    }


    // ============================================================
    // IRTEN
    // ============================================================

    /// <summary>
    /// Irten botoia sakatzen denean aplikazioaren leihoa ixten du.
    /// </summary>
    /// <param name="sender">
    /// Gertaera sortu duen Irten botoia.
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