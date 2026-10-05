using Microsoft.Maui.Controls.Shapes;

namespace I01_09_ErosketaOrgatxoa;

/// <summary>
/// Erosketa-orgatxoaren aplikazioaren orri nagusia.
/// Produktuak Drag & Drop bidez orgatxora gehitzeko,
/// kopuruak kontrolatzeko eta azken ekintzak desegiteko erabiltzen da.
/// </summary>
public partial class MainPage : ContentPage
{
    /// <summary>
    /// Une honetan orgatxoan dauden produktuak gordetzen ditu.
    /// </summary>
    private readonly List<Produktua> erosketaZerrenda = new();

    /// <summary>
    /// Azken produktu-gehiketak gordetzen ditu.
    /// Gehienez 20 ekintza gordetzen dira.
    /// </summary>
    private readonly List<string> ekintzenHistoria = new();

    /// <summary>
    /// Une honetan arrastatzen ari den produktuaren izena.
    /// </summary>
    private string? arrastatutakoProduktua;

    /// <summary>
    /// Historian gorde daitezkeen gehienezko ekintzak.
    /// </summary>
    private const int HISTORIA_MAXIMOA = 20;

    /// <summary>
    /// MainPage klasearen eraikitzailea.
    /// Interfaze grafikoa hasieratzen du.
    /// </summary>
    public MainPage()
    {
        InitializeComponent();
    }


    // ============================================================
    // DRAG & DROP
    // ============================================================

    /// <summary>
    /// Sandia arrastatzen hasten denean exekutatzen da.
    /// </summary>
    /// <param name="sender">DragGestureRecognizer objektua.</param>
    /// <param name="e">Arraste-gertaeraren informazioa.</param>
    private void OnSandiaDragStarting(
        object sender,
        DragStartingEventArgs e)
    {
        PrestatuArrastea("Sandia");
    }

    /// <summary>
    /// Sagarra arrastatzen hasten denean exekutatzen da.
    /// </summary>
    /// <param name="sender">DragGestureRecognizer objektua.</param>
    /// <param name="e">Arraste-gertaeraren informazioa.</param>
    private void OnSagarraDragStarting(
        object sender,
        DragStartingEventArgs e)
    {
        PrestatuArrastea("Sagarra");
    }

    /// <summary>
    /// Udarea arrastatzen hasten denean exekutatzen da.
    /// </summary>
    /// <param name="sender">DragGestureRecognizer objektua.</param>
    /// <param name="e">Arraste-gertaeraren informazioa.</param>
    private void OnUdareaDragStarting(
        object sender,
        DragStartingEventArgs e)
    {
        PrestatuArrastea("Udarea");
    }

    /// <summary>
    /// Laranja arrastatzen hasten denean exekutatzen da.
    /// </summary>
    /// <param name="sender">DragGestureRecognizer objektua.</param>
    /// <param name="e">Arraste-gertaeraren informazioa.</param>
    private void OnLaranjaDragStarting(
        object sender,
        DragStartingEventArgs e)
    {
        PrestatuArrastea("Laranja");
    }

    /// <summary>
    /// Anana arrastatzen hasten denean exekutatzen da.
    /// </summary>
    /// <param name="sender">DragGestureRecognizer objektua.</param>
    /// <param name="e">Arraste-gertaeraren informazioa.</param>
    private void OnAnanaDragStarting(
        object sender,
        DragStartingEventArgs e)
    {
        PrestatuArrastea("Anana");
    }

    /// <summary>
    /// Meloia arrastatzen hasten denean exekutatzen da.
    /// </summary>
    /// <param name="sender">DragGestureRecognizer objektua.</param>
    /// <param name="e">Arraste-gertaeraren informazioa.</param>
    private void OnMeloiaDragStarting(
        object sender,
        DragStartingEventArgs e)
    {
        PrestatuArrastea("Meloia");
    }

    /// <summary>
    /// Arrastatzen ari den produktuaren izena gordetzen du.
    /// </summary>
    /// <param name="produktuIzena">
    /// Arrastatzen ari den produktuaren izena.
    /// </param>
    private void PrestatuArrastea(string produktuIzena)
    {
        arrastatutakoProduktua = produktuIzena;

        EgoeraLabel.Text =
            $"{produktuIzena} arrastatzen...";
    }

    /// <summary>
    /// Produktua orgatxoaren gainean askatzen denean exekutatzen da.
    /// Arrastatutako produktua erosketa-zerrendara gehitzen du.
    /// </summary>
    /// <param name="sender">DropGestureRecognizer objektua.</param>
    /// <param name="e">Drop-gertaeraren informazioa.</param>
    private void OnOrgatxoaDrop(
        object sender,
        DropEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(arrastatutakoProduktua))
        {
            EgoeraLabel.Text =
                "Ez dago produkturik arrastatzen.";

            return;
        }

        string produktuIzena =
            arrastatutakoProduktua;

        GehituProduktua(produktuIzena);

        EgoeraLabel.Text =
            $"{produktuIzena} orgatxora gehitu da.";

        arrastatutakoProduktua = null;
    }


    // ============================================================
    // PRODUKTUEN KUDEAKETA
    // ============================================================

    /// <summary>
    /// Produktua orgatxora gehitzen du.
    /// Produktua dagoeneko badago, bere kopurua handitzen du.
    /// </summary>
    /// <param name="produktuIzena">
    /// Gehitu behar den produktuaren izena.
    /// </param>
    private void GehituProduktua(string produktuIzena)
    {
        Produktua? produktua =
            BilatuProduktua(produktuIzena);

        if (produktua != null)
        {
            produktua.Kopurua++;
        }
        else
        {
            Produktua produktuBerria =
                SortuProduktua(produktuIzena);

            erosketaZerrenda.Add(produktuBerria);
        }

        GehituHistoriara(produktuIzena);

        EguneratuInterfazea();
    }

    /// <summary>
    /// Produktu berri bat sortzen du izenaren arabera.
    /// </summary>
    /// <param name="produktuIzena">
    /// Sortu behar den produktuaren izena.
    /// </param>
    /// <returns>
    /// Produktu berriaren objektua.
    /// </returns>
    private Produktua SortuProduktua(string produktuIzena)
    {
        string irudia = produktuIzena switch
        {
            "Sandia" => "sandia.png",
            "Sagarra" => "sagarra.png",
            "Udarea" => "udarea.png",
            "Laranja" => "laranja.png",
            "Anana" => "anana.png",
            "Meloia" => "meloia.png",
            _ => string.Empty
        };

        return new Produktua(
            produktuIzena,
            irudia,
            1);
    }

    /// <summary>
    /// Produktua erosketa-zerrendan bilatzen du.
    /// </summary>
    /// <param name="produktuIzena">
    /// Bilatu behar den produktuaren izena.
    /// </param>
    /// <returns>
    /// Produktua aurkitzen bada objektua itzultzen du;
    /// bestela null itzultzen du.
    /// </returns>
    private Produktua? BilatuProduktua(
        string produktuIzena)
    {
        foreach (Produktua produktua in erosketaZerrenda)
        {
            if (produktua.Izena == produktuIzena)
            {
                return produktua;
            }
        }

        return null;
    }


    // ============================================================
    // HISTORIA
    // ============================================================

    /// <summary>
    /// Produktu-gehiketa bat ekintzen historian gordetzen du.
    /// Historiak gehienez 20 ekintza mantentzen ditu.
    /// </summary>
    /// <param name="produktuIzena">
    /// Gehitu den produktuaren izena.
    /// </param>
    private void GehituHistoriara(string produktuIzena)
    {
        ekintzenHistoria.Add(produktuIzena);

        if (ekintzenHistoria.Count > HISTORIA_MAXIMOA)
        {
            ekintzenHistoria.RemoveAt(0);
        }
    }

    /// <summary>
    /// Azken produktu-gehiketa desegiten du.
    /// </summary>
    private void DeseginAzkenEkintza()
    {
        if (ekintzenHistoria.Count == 0)
        {
            return;
        }

        int azkenPosizioa =
            ekintzenHistoria.Count - 1;

        string produktuIzena =
            ekintzenHistoria[azkenPosizioa];

        ekintzenHistoria.RemoveAt(azkenPosizioa);

        Produktua? produktua =
            BilatuProduktua(produktuIzena);

        if (produktua == null)
        {
            return;
        }

        produktua.Kopurua--;

        if (produktua.Kopurua <= 0)
        {
            erosketaZerrenda.Remove(produktua);
        }

        EgoeraLabel.Text =
            $"{produktuIzena}: azken ekintza desegin da.";

        EguneratuInterfazea();
    }


    // ============================================================
    // INTERFAZEA
    // ============================================================

    /// <summary>
    /// Erosketa-zerrenda, kontagailua eta botoien egoera eguneratzen ditu.
    /// </summary>
    private void EguneratuInterfazea()
    {
        ErosketaZerrendaLayout.Children.Clear();

        if (erosketaZerrenda.Count == 0)
        {
            Label hutsik =
                new Label
                {
                    Text = "Orgatxoa hutsik dago.",
                    TextColor = Colors.Gray,
                    HorizontalTextAlignment =
                        TextAlignment.Center,
                    Margin = new Thickness(0, 20)
                };

            ErosketaZerrendaLayout.Children.Add(hutsik);
        }
        else
        {
            foreach (Produktua produktua in erosketaZerrenda)
            {
                ErosketaZerrendaLayout.Children.Add(
                    SortuProduktuLerroa(produktua));
            }
        }

        GuztiraLabel.Text =
            KalkulatuGuztira().ToString();

        DeseginBtn.IsEnabled =
            ekintzenHistoria.Count > 0;

        ItzuliBtn.IsEnabled =
            erosketaZerrenda.Count > 0;
    }

    /// <summary>
    /// Produktu baten lerro grafikoa sortzen du.
    /// </summary>
    /// <param name="produktua">
    /// Erakutsi behar den produktua.
    /// </param>
    /// <returns>
    /// Produktuaren Border kontrol grafikoa.
    /// </returns>
    private Border SortuProduktuLerroa(
        Produktua produktua)
    {
        Grid grid =
            new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition
                    {
                        Width = 50
                    },

                    new ColumnDefinition
                    {
                        Width = GridLength.Star
                    },

                    new ColumnDefinition
                    {
                        Width = 60
                    }
                }
            };

        Image irudia =
            new Image
            {
                Source = produktua.Irudia,
                WidthRequest = 38,
                HeightRequest = 38,
                Aspect = Aspect.AspectFit
            };

        Label izena =
            new Label
            {
                Text = produktua.Izena,
                FontAttributes =
                    FontAttributes.Bold,
                TextColor =
                    Color.FromArgb("#163A70"),
                VerticalTextAlignment =
                    TextAlignment.Center
            };

        Label kopurua =
            new Label
            {
                Text = $"x{produktua.Kopurua}",
                FontSize = 17,
                FontAttributes =
                    FontAttributes.Bold,
                TextColor =
                    Color.FromArgb("#176B35"),
                HorizontalTextAlignment =
                    TextAlignment.End,
                VerticalTextAlignment =
                    TextAlignment.Center
            };

        Grid.SetColumn(irudia, 0);
        Grid.SetColumn(izena, 1);
        Grid.SetColumn(kopurua, 2);

        grid.Children.Add(irudia);
        grid.Children.Add(izena);
        grid.Children.Add(kopurua);

        return new Border
        {
            Padding = 10,
            BackgroundColor =
                Color.FromArgb("#F8FAFC"),
            Stroke =
                Color.FromArgb("#E1E7F0"),
            StrokeThickness = 1,

            StrokeShape =
                new RoundRectangle
                {
                    CornerRadius = 9
                },

            Content = grid
        };
    }

    /// <summary>
    /// Orgatxoan dauden unitate guztien kopurua kalkulatzen du.
    /// </summary>
    /// <returns>
    /// Produktu-unitate guztien kopurua.
    /// </returns>
    private int KalkulatuGuztira()
    {
        int guztira = 0;

        foreach (Produktua produktua in erosketaZerrenda)
        {
            guztira += produktua.Kopurua;
        }

        return guztira;
    }


    // ============================================================
    // BOTOIAK
    // ============================================================

    /// <summary>
    /// Desegin botoiaren klik-gertaera kudeatzen du.
    /// </summary>
    /// <param name="sender">Botoia.</param>
    /// <param name="e">Klik-gertaeraren informazioa.</param>
    private void OnDeseginClicked(
        object sender,
        EventArgs e)
    {
        DeseginAzkenEkintza();
    }

    /// <summary>
    /// Itzuli botoiaren klik-gertaera kudeatzen du.
    /// Erosketa-zerrenda osoa husten du.
    /// </summary>
    /// <param name="sender">Botoia.</param>
    /// <param name="e">Klik-gertaeraren informazioa.</param>
    private void OnItzuliClicked(
        object sender,
        EventArgs e)
    {
        erosketaZerrenda.Clear();
        ekintzenHistoria.Clear();
        arrastatutakoProduktua = null;

        EgoeraLabel.Text =
            "Erosketa-zerrenda hustu da.";

        EguneratuInterfazea();
    }

    /// <summary>
    /// Irten botoiaren klik-gertaera kudeatzen du.
    /// Aplikazioaren leihoa ixten du.
    /// </summary>
    /// <param name="sender">Botoia.</param>
    /// <param name="e">Klik-gertaeraren informazioa.</param>
    private void OnIrtenClicked(
        object sender,
        EventArgs e)
    {
        Window? leihoa =
            Application.Current?.Windows.FirstOrDefault();

        if (leihoa != null)
        {
            Application.Current?.CloseWindow(leihoa);
        }
    }
}