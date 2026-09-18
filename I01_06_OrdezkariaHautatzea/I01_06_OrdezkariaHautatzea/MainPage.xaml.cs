using System.Collections.ObjectModel;

namespace I01_06_OrdezkariaHautatzea;

public partial class MainPage : ContentPage
{
    // Ikasleen eta ordezkarien zerrendak
    private ObservableCollection<Ikaslea> ikasleak;
    private ObservableCollection<Ikaslea> ordezkariak;

    // Aukeratutako elementuak
    private Ikaslea? aukeratutakoIkaslea;
    private Ikaslea? aukeratutakoOrdezkaria;

    // Ausazko ikaslea aukeratzeko
    private Random rnd = new Random();


    public MainPage()
    {
        InitializeComponent();

        ikasleak = new ObservableCollection<Ikaslea>();
        ordezkariak = new ObservableCollection<Ikaslea>();

        IkasleakCollection.ItemsSource = ikasleak;
        OrdezkariakCollection.ItemsSource = ordezkariak;

        EguneratuBotoiak();
    }


    // Ikasle berria gehitzen du
    private void GehituButton_Clicked(object? sender, EventArgs e)
    {
        string izena = IzenaEntry.Text?.Trim() ?? "";
        string abizena = AbizenaEntry.Text?.Trim() ?? "";

        if (izena == "" || abizena == "")
        {
            return;
        }

        Ikaslea ikaslea = new Ikaslea();

        ikaslea.Izena = izena;
        ikaslea.Abizena = abizena;

        ikasleak.Add(ikaslea);

        IzenaEntry.Text = "";
        AbizenaEntry.Text = "";

        EguneratuBotoiak();
    }


    // Ikasleen listan elementu bat aukeratzean
    private void IkasleakCollection_SelectionChanged(
        object? sender,
        SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0)
        {
            aukeratutakoIkaslea =
                (Ikaslea)e.CurrentSelection[0];
        }
        else
        {
            aukeratutakoIkaslea = null;
        }

        EguneratuBotoiak();
    }


    // Aukeratutako ikaslea ordezkari izendatzen du
    private void IzendatuButton_Clicked(object? sender, EventArgs e)
    {
        if (aukeratutakoIkaslea == null)
        {
            return;
        }

        // Aurretik ez badago bakarrik gehitzen da
        if (!ordezkariak.Contains(aukeratutakoIkaslea))
        {
            ordezkariak.Add(aukeratutakoIkaslea);
        }

        EguneratuBotoiak();
    }


    // Ikasle bat ausaz aukeratzen du
    private void AusazButton_Clicked(object? sender, EventArgs e)
    {
        if (ikasleak.Count == 0)
        {
            return;
        }

        int posizioa = rnd.Next(ikasleak.Count);

        Ikaslea ikaslea = ikasleak[posizioa];

        // Ordezkarien listan aurretik ez badago gehitzen du
        if (!ordezkariak.Contains(ikaslea))
        {
            ordezkariak.Add(ikaslea);
        }

        EguneratuBotoiak();
    }


    // Ikasleen lista osoa garbitzen du
    private void KenduDenakButton_Clicked(object? sender, EventArgs e)
    {
        ikasleak.Clear();

        aukeratutakoIkaslea = null;
        IkasleakCollection.SelectedItem = null;

        EguneratuBotoiak();
    }


    // Ordezkarien listan elementu bat aukeratzean
    private void OrdezkariakCollection_SelectionChanged(
        object? sender,
        SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0)
        {
            aukeratutakoOrdezkaria =
                (Ikaslea)e.CurrentSelection[0];
        }
        else
        {
            aukeratutakoOrdezkaria = null;
        }

        EguneratuBotoiak();
    }


    // Aukeratutako ordezkaria kentzen du
    private void KenduButton_Clicked(object? sender, EventArgs e)
    {
        if (aukeratutakoOrdezkaria == null)
        {
            return;
        }

        ordezkariak.Remove(aukeratutakoOrdezkaria);

        aukeratutakoOrdezkaria = null;
        OrdezkariakCollection.SelectedItem = null;

        EguneratuBotoiak();
    }


    // Ordezkarien lista osoa garbitzen du
    private void HustuButton_Clicked(object? sender, EventArgs e)
    {
        ordezkariak.Clear();

        aukeratutakoOrdezkaria = null;
        OrdezkariakCollection.SelectedItem = null;

        EguneratuBotoiak();
    }


    // Aukeratutako ordezkaria posizio bat gora mugitzen du
    private void GoraButton_Clicked(object? sender, EventArgs e)
    {
        if (aukeratutakoOrdezkaria == null)
        {
            return;
        }

        int posizioa =
            ordezkariak.IndexOf(aukeratutakoOrdezkaria);

        if (posizioa > 0)
        {
            ordezkariak.Move(posizioa, posizioa - 1);
        }

        EguneratuBotoiak();
    }


    // Aukeratutako ordezkaria posizio bat behera mugitzen du
    private void BeheraButton_Clicked(object? sender, EventArgs e)
    {
        if (aukeratutakoOrdezkaria == null)
        {
            return;
        }

        int posizioa =
            ordezkariak.IndexOf(aukeratutakoOrdezkaria);

        if (posizioa < ordezkariak.Count - 1)
        {
            ordezkariak.Move(posizioa, posizioa + 1);
        }

        EguneratuBotoiak();
    }


    // Botoien egoera eguneratzen du
    private void EguneratuBotoiak()
    {
        // Ikasleen lista
        AusazButton.IsEnabled =
            ikasleak.Count > 0;

        KenduDenakButton.IsEnabled =
            ikasleak.Count > 0;

        IzendatuButton.IsEnabled =
            aukeratutakoIkaslea != null;


        // Ordezkarien lista
        HustuButton.IsEnabled =
            ordezkariak.Count > 0;

        KenduButton.IsEnabled =
            aukeratutakoOrdezkaria != null;


        // Gora eta behera
        if (aukeratutakoOrdezkaria != null)
        {
            int posizioa =
                ordezkariak.IndexOf(aukeratutakoOrdezkaria);

            GoraButton.IsEnabled =
                posizioa > 0;

            BeheraButton.IsEnabled =
                posizioa >= 0 &&
                posizioa < ordezkariak.Count - 1;
        }
        else
        {
            GoraButton.IsEnabled = false;
            BeheraButton.IsEnabled = false;
        }
    }
}