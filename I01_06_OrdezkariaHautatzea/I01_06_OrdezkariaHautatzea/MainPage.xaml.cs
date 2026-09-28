using System.Collections.ObjectModel;

namespace I01_06_OrdezkariaHautatzea;

/// <summary>
/// Ikasleen eta ordezkarien kudeaketa egiten duen aplikazioaren orri nagusia.
/// Ikasleak gehitzeko, ordezkariak izendatzeko, ausaz aukeratzeko
/// eta ordezkarien zerrenda antolatzeko funtzionaltasuna eskaintzen du.
/// </summary>
public partial class MainPage : ContentPage
{
    // Ikasle guztiak gordetzen dituen bilduma.
    private ObservableCollection<Ikaslea> ikasleak;

    // Ordezkari gisa izendatutako ikasleak gordetzen dituen bilduma.
    private ObservableCollection<Ikaslea> ordezkariak;

    // Une honetan ikasleen zerrendan hautatutako ikaslea gordetzen du.
    private Ikaslea? aukeratutakoIkaslea;

    // Une honetan ordezkarien zerrendan hautatutako ordezkaria gordetzen du.
    private Ikaslea? aukeratutakoOrdezkaria;

    // Ausazko ikasle bat aukeratzeko erabiltzen den objektua.
    private Random rnd = new Random();

    /// <summary>
    /// Orri nagusia hasieratzen du, XAML interfazea kargatzen du,
    /// ikasleen eta ordezkarien bildumak sortzen ditu
    /// eta interfazeko botoien hasierako egoera ezartzen du.
    /// </summary>
    public MainPage()
    {
        InitializeComponent();

        ikasleak = new ObservableCollection<Ikaslea>();
        ordezkariak = new ObservableCollection<Ikaslea>();

        // Bildumak interfazeko CollectionView kontrolekin lotzen dira.
        IkasleakCollection.ItemsSource = ikasleak;
        OrdezkariakCollection.ItemsSource = ordezkariak;

        EguneratuBotoiak();
    }

    /// <summary>
    /// "Gehitu" botoia sakatzean ikasle berri bat sortzen du
    /// eta ikasleen bildumara gehitzen du.
    /// Izena eta abizena derrigorrezko datuak dira.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen botoia.</param>
    /// <param name="e">Klik gertaeraren datuak.</param>
    private void GehituButton_Clicked(object? sender, EventArgs e)
    {
        // Erabiltzaileak sartutako izena eta abizena irakurtzen dira.
        string izena = IzenaEntry.Text?.Trim() ?? "";
        string abizena = AbizenaEntry.Text?.Trim() ?? "";

        // Daturen bat hutsik badago, ez da ikaslerik sortzen.
        if (izena == "" || abizena == "")
        {
            return;
        }

        // Ikasle objektu berria sortzen da.
        Ikaslea ikaslea = new Ikaslea();

        ikaslea.Izena = izena;
        ikaslea.Abizena = abizena;

        // Ikaslea bildumara gehitzen da.
        ikasleak.Add(ikaslea);

        // Sarrera-eremuak garbitzen dira.
        IzenaEntry.Text = "";
        AbizenaEntry.Text = "";

        EguneratuBotoiak();
    }

    /// <summary>
    /// Ikasleen zerrendan elementu bat hautatzen denean exekutatzen da.
    /// Uneko hautatutako ikaslea gordetzen du eta botoien egoera eguneratzen du.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen CollectionView kontrola.</param>
    /// <param name="e">Hautaketaren aldaketari buruzko datuak.</param>
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

    /// <summary>
    /// "Izendatu" botoia sakatzean hautatutako ikaslea
    /// ordezkarien bildumara gehitzen du.
    /// Ikasle bera ezin da bi aldiz gehitu.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen botoia.</param>
    /// <param name="e">Klik gertaeraren datuak.</param>
    private void IzendatuButton_Clicked(object? sender, EventArgs e)
    {
        if (aukeratutakoIkaslea == null)
        {
            return;
        }

        // Ikaslea aurretik ordezkarien zerrendan ez badago bakarrik gehitzen da.
        if (!ordezkariak.Contains(aukeratutakoIkaslea))
        {
            ordezkariak.Add(aukeratutakoIkaslea);
        }

        EguneratuBotoiak();
    }

    /// <summary>
    /// "Ausaz" botoia sakatzean ikasleen bildumako elementu bat
    /// ausaz aukeratzen du eta ordezkari gisa gehitzen du.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen botoia.</param>
    /// <param name="e">Klik gertaeraren datuak.</param>
    private void AusazButton_Clicked(object? sender, EventArgs e)
    {
        // Ikaslerik ez badago, ez da ekintzarik egiten.
        if (ikasleak.Count == 0)
        {
            return;
        }

        // Ikasleen bildumako ausazko posizio bat aukeratzen da.
        int posizioa = rnd.Next(ikasleak.Count);

        Ikaslea ikaslea = ikasleak[posizioa];

        // Hautatutako ikaslea aurretik ordezkaria ez bada, bildumara gehitzen da.
        if (!ordezkariak.Contains(ikaslea))
        {
            ordezkariak.Add(ikaslea);
        }

        EguneratuBotoiak();
    }

    /// <summary>
    /// "Kendu denak" botoia sakatzean ikasleen bilduma osoa garbitzen du
    /// eta uneko hautaketa ezabatzen du.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen botoia.</param>
    /// <param name="e">Klik gertaeraren datuak.</param>
    private void KenduDenakButton_Clicked(object? sender, EventArgs e)
    {
        ikasleak.Clear();

        aukeratutakoIkaslea = null;
        IkasleakCollection.SelectedItem = null;

        EguneratuBotoiak();
    }

    /// <summary>
    /// Ordezkarien zerrendan elementu bat hautatzen denean exekutatzen da.
    /// Uneko hautatutako ordezkaria gordetzen du eta botoien egoera eguneratzen du.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen CollectionView kontrola.</param>
    /// <param name="e">Hautaketaren aldaketari buruzko datuak.</param>
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

    /// <summary>
    /// "Kendu" botoia sakatzean hautatutako ordezkaria
    /// ordezkarien bildumatik ezabatzen du.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen botoia.</param>
    /// <param name="e">Klik gertaeraren datuak.</param>
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

    /// <summary>
    /// "Hustu" botoia sakatzean ordezkarien bilduma osoa garbitzen du
    /// eta uneko hautaketa ezabatzen du.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen botoia.</param>
    /// <param name="e">Klik gertaeraren datuak.</param>
    private void HustuButton_Clicked(object? sender, EventArgs e)
    {
        ordezkariak.Clear();

        aukeratutakoOrdezkaria = null;
        OrdezkariakCollection.SelectedItem = null;

        EguneratuBotoiak();
    }

    /// <summary>
    /// "Gora" botoia sakatzean hautatutako ordezkaria
    /// zerrendan posizio bat gora mugitzen du.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen botoia.</param>
    /// <param name="e">Klik gertaeraren datuak.</param>
    private void GoraButton_Clicked(object? sender, EventArgs e)
    {
        if (aukeratutakoOrdezkaria == null)
        {
            return;
        }

        // Hautatutako ordezkariaren uneko posizioa bilatzen da.
        int posizioa =
            ordezkariak.IndexOf(aukeratutakoOrdezkaria);

        // Lehenengo posizioan ez badago, posizio bat gora mugitzen da.
        if (posizioa > 0)
        {
            ordezkariak.Move(posizioa, posizioa - 1);
        }

        EguneratuBotoiak();
    }

    /// <summary>
    /// "Behera" botoia sakatzean hautatutako ordezkaria
    /// zerrendan posizio bat behera mugitzen du.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen botoia.</param>
    /// <param name="e">Klik gertaeraren datuak.</param>
    private void BeheraButton_Clicked(object? sender, EventArgs e)
    {
        if (aukeratutakoOrdezkaria == null)
        {
            return;
        }

        // Hautatutako ordezkariaren uneko posizioa bilatzen da.
        int posizioa =
            ordezkariak.IndexOf(aukeratutakoOrdezkaria);

        // Azken posizioan ez badago, posizio bat behera mugitzen da.
        if (posizioa < ordezkariak.Count - 1)
        {
            ordezkariak.Move(posizioa, posizioa + 1);
        }

        EguneratuBotoiak();
    }

    /// <summary>
    /// Interfazeko botoien aktibazio-egoera eguneratzen du.
    /// Bildumen edukia eta uneko hautaketak kontuan hartzen ditu.
    /// </summary>
    private void EguneratuBotoiak()
    {
        // Ikasleen bildumarekin lotutako botoien egoera eguneratzen da.
        AusazButton.IsEnabled =
            ikasleak.Count > 0;

        KenduDenakButton.IsEnabled =
            ikasleak.Count > 0;

        IzendatuButton.IsEnabled =
            aukeratutakoIkaslea != null;

        // Ordezkarien bildumarekin lotutako botoien egoera eguneratzen da.
        HustuButton.IsEnabled =
            ordezkariak.Count > 0;

        KenduButton.IsEnabled =
            aukeratutakoOrdezkaria != null;

        // Hautatutako ordezkaria gora edo behera mugitu daitekeen egiaztatzen da.
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