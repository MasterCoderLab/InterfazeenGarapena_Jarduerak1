using System.Diagnostics;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Plugin.Maui.Audio;

namespace I01_13_AhateariTiroEgitea;

/// <summary>
/// Ahateei tiro egiteko jokoaren logika nagusia.
/// </summary>
public partial class MainPage : ContentPage
{
    private const int JokoIraupena = 20;
    private const float EremuZabalera = 1000f;
    private const float EremuAltuera = 600f;

    private readonly JokoMarrazkia marrazkia = new();
    private readonly IDispatcherTimer tenporizadorea;
    private readonly Random ausaz = new();

    private IAudioPlayer? tiroSoinua;
    private Stream? tiroFitxategia;

    private bool jolasten;
    private int puntuak;
    private int bolada;
    private int jaurtiketak;
    private int asmatutakoak;

    private double agerpenDenbora;
    private DateTime azkenFotograma;
    private DateTime amaieraOrdua;
    private DateTime dianaAzkenMugimendua;

    /// <summary>
    /// Pantaila eta jokoaren tenporizadorea prestatzen ditu.
    /// </summary>
    public MainPage()
    {
        InitializeComponent();

        JokoEremua.Drawable = marrazkia;

#if ANDROID
        // Androiden, saguaren gezi arrunta ezkutatzen dugu
        // joko-eremuan gure diana gorria erabiltzeko.
        JokoEremua.HandlerChanged += (_, _) =>
        {
            if (JokoEremua.Handler?.PlatformView
                is Android.Views.View ikuspegia)
            {
                ikuspegia.PointerIcon =
                    Android.Views.PointerIcon.GetSystemIcon(
                        ikuspegia.Context!,
                        (Android.Views.PointerIconType)0);
            }
        };
#endif

        tenporizadorea = Dispatcher.CreateTimer();
        tenporizadorea.Interval = TimeSpan.FromMilliseconds(33);
        tenporizadorea.Tick += Tenporizadorea_Tick;

        EgokituHasierakoEgoera();

        _ = KargatuSoinuaAsync();
    }

    /// <summary>
    /// tiro.wav fitxategia kargatzen du aplikazioaren baliabideetatik.
    /// </summary>
    private async Task KargatuSoinuaAsync()
    {
        try
        {
            tiroFitxategia =
                await FileSystem.OpenAppPackageFileAsync("tiro.wav");

            tiroSoinua =
                AudioManager.Current.CreatePlayer(tiroFitxategia);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Soinuaren errorea: {ex.Message}");
            EgoeraLabel.Text = "Soinua ezin izan da kargatu";
        }
    }

    /// <summary>
    /// Jokoaren hasierako egoera ezartzen du.
    /// </summary>
    private void EgokituHasierakoEgoera()
    {
        jolasten = false;
        puntuak = 0;
        bolada = 0;
        jaurtiketak = 0;
        asmatutakoak = 0;
        agerpenDenbora = 0;

        marrazkia.Izakiak.Clear();
        marrazkia.Diana = null;
        marrazkia.Jolasten = false;

        PuntuakLabel.Text = "0";
        DenboraLabel.Text = "0:20";
        EgoeraLabel.Text = "Hasi jolasteko";

        HasiBotoia.IsEnabled = true;
        MezuPanela.IsVisible = true;
        MezuIzenburua.Text = "PREST TIRO EGITEKO?";
        MezuAzalpena.Text =
            "Ahate horia +10 | Gorria +20 | Untxia -15";

        JokoEremua.Invalidate();
    }

    /// <summary>
    /// Hasi botoia sakatzean partida abiarazten du.
    /// </summary>
    private void HasiBotoia_Clicked(
        object? sender, EventArgs e)
    {
        HasiJokoa();
    }

    /// <summary>
    /// Partida berri bat abiarazten du.
    /// </summary>
    private void HasiJokoa()
    {
        tenporizadorea.Stop();
        EgokituHasierakoEgoera();

        jolasten = true;
        marrazkia.Jolasten = true;

        azkenFotograma = DateTime.UtcNow;
        amaieraOrdua =
            azkenFotograma.AddSeconds(JokoIraupena);

        HasiBotoia.IsEnabled = false;
        MezuPanela.IsVisible = false;
        EgoeraLabel.Text = "AHATEAK HELTZEN ARI DIRA!";

        // Lehen ahatea berehala agertuko da.
        SortuIzakia();

        tenporizadorea.Start();
    }

    /// <summary>
    /// 33 milisegundoro jokoaren egoera eguneratzen du.
    /// </summary>
    private void Tenporizadorea_Tick(
        object? sender, EventArgs e)
    {
        if (!jolasten)
            return;

        DateTime orain = DateTime.UtcNow;

        double delta = Math.Clamp(
            (orain - azkenFotograma).TotalSeconds,
            0.0,
            0.1);

        azkenFotograma = orain;

        // Diana ez da azken tiroan itsatsita geratzen.
        if (marrazkia.Diana.HasValue &&
            (orain - dianaAzkenMugimendua).TotalMilliseconds > 350)
        {
            marrazkia.Diana = null;
        }

        double geratzen = Math.Max(
            0,
            (amaieraOrdua - orain).TotalSeconds);

        DenboraLabel.Text =
            $"0:{Math.Ceiling(geratzen):00}";

        if (geratzen <= 0)
        {
            AmaituJokoa();
            return;
        }

        // Izaki berriak tarteka sortzen dira.
        agerpenDenbora += delta;

        if (agerpenDenbora >= 0.75)
        {
            agerpenDenbora = 0;

            if (marrazkia.Izakiak.Count < 9)
                SortuIzakia();
        }

        // Izakiak ezkerretik eskuinera mugitzen dira.
        for (int i = marrazkia.Izakiak.Count - 1;
             i >= 0; i--)
        {
            Izakia izakia = marrazkia.Izakiak[i];

            izakia.X += izakia.Abiadura * (float)delta;
            izakia.Animazioa += (float)delta * 12f;

            if (izakia.X > EremuZabalera + 65)
                marrazkia.Izakiak.RemoveAt(i);
        }

        JokoEremua.Invalidate();
    }

    /// <summary>
    /// Ahate motela, ahate azkarra edo untxia sortzen du.
    /// </summary>
    private void SortuIzakia()
    {
        int aukera = ausaz.Next(100);

        IzakiMota mota = aukera switch
        {
            < 45 => IzakiMota.AhateMotela,
            < 85 => IzakiMota.AhateAzkarra,
            _ => IzakiMota.Untxia
        };

        float abiadura = mota switch
        {
            IzakiMota.AhateMotela =>
                ausaz.Next(105, 155),

            IzakiMota.AhateAzkarra =>
                ausaz.Next(235, 315),

            _ => ausaz.Next(135, 190)
        };

        marrazkia.Izakiak.Add(new Izakia
        {
            Mota = mota,
            X = -55,
            Y = ausaz.Next(95, 435),
            Abiadura = abiadura,
            Animazioa = (float)ausaz.NextDouble() * 6f
        });
    }

    /// <summary>
    /// Saguaren mugimenduarekin diana eguneratzen du.
    /// </summary>
    private void JokoEremua_SaguaMugituta(
        object? sender, TouchEventArgs e)
    {
        if (e.Touches.Length == 0)
            return;

        PointF posizioa = e.Touches[0];

        marrazkia.Diana = BihurtuKoordenatuak(
            posizioa.X,
            posizioa.Y);

        dianaAzkenMugimendua = DateTime.UtcNow;
        JokoEremua.Invalidate();
    }

    /// <summary>
    /// Saguaren klik edo pantailako ukitu bakoitza tiro bat da.
    /// </summary>
    private void JokoEremua_Ukituta(
        object? sender, TouchEventArgs e)
    {
        if (!jolasten || e.Touches.Length == 0)
            return;

        PointF ukitua = e.Touches[0];

        PointF tiroa = BihurtuKoordenatuak(
            ukitua.X,
            ukitua.Y);

        marrazkia.Diana = tiroa;
        dianaAzkenMugimendua = DateTime.UtcNow;

        EginTiroa(tiroa);

        JokoEremua.Invalidate();
    }

    /// <summary>
    /// Pantailako posizioa 1000x600 koordenatuetara bihurtzen du.
    /// </summary>
    private PointF BihurtuKoordenatuak(
        double x, double y)
    {
        if (JokoEremua.Width <= 0 ||
            JokoEremua.Height <= 0)
            return new PointF(0, 0);

        float jokoX = (float)(
            x / JokoEremua.Width * EremuZabalera);

        float jokoY = (float)(
            y / JokoEremua.Height * EremuAltuera);

        return new PointF(jokoX, jokoY);
    }

    /// <summary>
    /// Tiroaren soinua erreproduzitzen du.
    /// </summary>
    private void ErreproduzituTiroa()
    {
        try
        {
            if (tiroSoinua is null)
                return;

            tiroSoinua.Stop();
            tiroSoinua.Play();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Tiro soinua: {ex.Message}");
        }
    }

    /// <summary>
    /// Tiroaren talka, puntuazioa eta oztopoak aztertzen ditu.
    /// </summary>
    private void EginTiroa(PointF tiroa)
    {
        jaurtiketak++;

        ErreproduzituTiroa();

        // Oztopoek aurrean dauden izakiak babesten dituzte.
        if (JokoMarrazkia.Babestuta(tiroa.X, tiroa.Y))
        {
            EgoeraLabel.Text = "OZTOPOA! Babestuta dago";
            bolada = 0;
            return;
        }

        // Azken marraztutako izakitik bilatzen da.
        for (int i = marrazkia.Izakiak.Count - 1;
             i >= 0; i--)
        {
            Izakia izakia = marrazkia.Izakiak[i];

            float distantziaX = tiroa.X - izakia.X;
            float distantziaY = tiroa.Y - izakia.Y;

            // Tiroaren eta izakiaren arteko talka.
            bool asmatu =
                MathF.Abs(distantziaX) <= 37 &&
                MathF.Abs(distantziaY) <= 35;

            if (!asmatu)
                continue;

            // Izakia zuhaitz edo harri baten atzean badago,
            // tiroak ez dio kalterik egiten.
            if (JokoMarrazkia.Babestuta(
                izakia.X, izakia.Y))
            {
                EgoeraLabel.Text = "AHATEA BABESTUTA!";
                bolada = 0;
                return;
            }

            marrazkia.Izakiak.RemoveAt(i);
            asmatutakoak++;

            if (izakia.Mota == IzakiMota.Untxia)
            {
                puntuak -= 15;
                bolada = 0;
                EgoeraLabel.Text = "UNTXIA! -15 puntu";
            }
            else
            {
                int irabazitakoPuntuak =
                    izakia.Mota == IzakiMota.AhateAzkarra
                    ? 20
                    : 10;

                bolada++;

                // Gehigarria: hiru asmaketa jarraian,
                // bost puntu gehigarri.
                if (bolada >= 3)
                    irabazitakoPuntuak += 5;

                puntuak += irabazitakoPuntuak;

                EgoeraLabel.Text =
                    $"+{irabazitakoPuntuak} puntu | Bolada: {bolada}";
            }

            PuntuakLabel.Text = puntuak.ToString();
            return;
        }

        // Tiro hutsak bolada eteten du.
        bolada = 0;
        EgoeraLabel.Text = "HUTS!";
    }

    /// <summary>
    /// Denbora agortzen denean partida amaitzen du.
    /// </summary>
    private void AmaituJokoa()
    {
        tenporizadorea.Stop();

        jolasten = false;
        marrazkia.Jolasten = false;

        DenboraLabel.Text = "0:00";
        HasiBotoia.IsEnabled = true;

        MezuPanela.IsVisible = true;
        MezuIzenburua.Text = "JOKOA AMAITU DA!";

        double zehaztasuna = jaurtiketak == 0
            ? 0
            : asmatutakoak * 100.0 / jaurtiketak;

        MezuAzalpena.Text =
            $"Puntuak: {puntuak}\n" +
            $"Asmatutakoak: {asmatutakoak}\n" +
            $"Zehaztasuna: {zehaztasuna:F1}%\n" +
            "Sakatu Hasi berriro jokatzeko.";

        EgoeraLabel.Text = "Partida amaituta";

        JokoEremua.Invalidate();
    }

    /// <summary>
    /// Berrabiarazi botoiarekin partida berria hasten du.
    /// </summary>
    private void BerrabiaraziBotoia_Clicked(
        object? sender, EventArgs e)
    {
        HasiJokoa();
    }

    /// <summary>
    /// Irten botoiak aplikazioa ixten du.
    /// </summary>
    private void IrtenBotoia_Clicked(
        object? sender, EventArgs e)
    {
        tenporizadorea.Stop();
        Application.Current?.Quit();
    }

    protected override void OnDisappearing()
    {
        tenporizadorea.Stop();
        base.OnDisappearing();
    }
}
