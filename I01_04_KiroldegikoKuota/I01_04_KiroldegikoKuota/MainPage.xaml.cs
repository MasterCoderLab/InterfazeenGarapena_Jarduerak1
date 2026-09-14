namespace I01_04_KiroldegikoKuota;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        EzarriDefektuzkoBalioak();
        EguneratuPrezioa();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (Window != null)
        {
            Window.Title = "Kiroldegi txartela";
        }
    }


    // Hasierako balioak ezartzen ditu
    private void EzarriDefektuzkoBalioak()
    {
        JubilatuakRadio.IsChecked = true;

        DesgaitasunaCheck.IsChecked = false;
        FamiliaUgariaCheck.IsChecked = false;

        // Hilabetekoa
        IraupenaPicker.SelectedIndex = 1;
    }


    // Kiroldegiko kuota kalkulatzen du
    private decimal KalkulatuKuota()
    {
        decimal hilabetekoPrezioa;


        // KATEGORIA
        if (UmeakRadio.IsChecked)
        {
            hilabetekoPrezioa = 120m;
        }
        else if (FamiliaRadio.IsChecked)
        {
            hilabetekoPrezioa = 250m;
        }
        else
        {
            // Jubilatuak
            hilabetekoPrezioa = 198m;
        }


        // IRAUPENA
        decimal prezioa = IraupenaPicker.SelectedIndex switch
        {
            0 => hilabetekoPrezioa * 10m,      // Urtekoa
            1 => hilabetekoPrezioa,            // Hilabetekoa
            2 => hilabetekoPrezioa * 0.60m,    // 2 astekoa
            3 => hilabetekoPrezioa * 0.35m,    // Astebetekoa
            4 => hilabetekoPrezioa * 0.08m,    // Egunekoa
            _ => 0m
        };


        // DESKONTUAK
        decimal deskontua = 0m;

        if (DesgaitasunaCheck.IsChecked)
        {
            deskontua += 0.50m;
        }

        if (FamiliaUgariaCheck.IsChecked)
        {
            deskontua += 0.25m;
        }


        prezioa = prezioa * (1m - deskontua);

        return prezioa;
    }


    // Pantailako prezioa eguneratzen du
    private void EguneratuPrezioa()
    {
        decimal prezioa = KalkulatuKuota();

        PrezioaLabel.Text = $"{prezioa:F2} €";
    }


    // RadioButton bat aldatzean
    private void Kategoria_Changed(
        object? sender,
        CheckedChangedEventArgs e)
    {
        EguneratuPrezioa();
    }


    // CheckBox bat aldatzean
    private void Deskontua_Changed(
        object? sender,
        CheckedChangedEventArgs e)
    {
        EguneratuPrezioa();
    }


    // Iraupena aldatzean
    private void IraupenaPicker_SelectedIndexChanged(
        object? sender,
        EventArgs e)
    {
        EguneratuPrezioa();
    }


    // Kalkulatu botoia
    private void KalkulatuButton_Clicked(
        object? sender,
        EventArgs e)
    {
        EguneratuPrezioa();
    }


    // Garbitu botoia
    private void GarbituButton_Clicked(
        object? sender,
        EventArgs e)
    {
        EzarriDefektuzkoBalioak();
        EguneratuPrezioa();
    }


    // Irten botoia
    private void IrtenButton_Clicked(
        object? sender,
        EventArgs e)
    {
        if (Window != null)
        {
            Application.Current?.CloseWindow(Window);
        }
    }
}