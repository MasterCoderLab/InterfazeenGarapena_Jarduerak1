using Microsoft.Maui.Graphics;

namespace I01_13_AhateariTiroEgitea;

/// <summary>
/// Jokoan ager daitezkeen izaki motak.
/// </summary>
public enum IzakiMota
{
    AhateMotela,
    AhateAzkarra,
    Untxia
}

/// <summary>
/// Pantailan mugitzen den izaki baten datuak.
/// </summary>
public class Izakia
{
    public IzakiMota Mota { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public float Abiadura { get; set; }
    public float Animazioa { get; set; }
}

/// <summary>
/// Jokoaren agertokia eta pertsonaiak marrazten ditu.
/// Koordenatu logikoak: 1000 x 600.
/// </summary>
public class JokoMarrazkia : IDrawable
{
    public List<Izakia> Izakiak { get; } = new();

    public PointF? Diana { get; set; }

    public bool Jolasten { get; set; }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        if (dirtyRect.Width <= 0 || dirtyRect.Height <= 0)
            return;

        canvas.SaveState();

        canvas.Scale(
            dirtyRect.Width / 1000f,
            dirtyRect.Height / 600f);

        MarraztuZerua(canvas);
        MarraztuMendiak(canvas);
        MarraztuLurra(canvas);

        // Izakiak oztopoen aurretik marrazten dira.
        // Horrela, zuhaitzen eta harrien atzean ezkutatzen dira.
        foreach (Izakia izakia in Izakiak)
        {
            if (izakia.Mota == IzakiMota.Untxia)
                MarraztuUntxia(canvas, izakia);
            else
                MarraztuAhatea(canvas, izakia);
        }

        MarraztuOztopoak(canvas);

        if (Diana.HasValue)
            MarraztuDiana(canvas, Diana.Value);

        canvas.RestoreState();
    }

    private static void MarraztuZerua(ICanvas canvas)
    {
        canvas.FillColor = Color.FromArgb("#8FCBE8");
        canvas.FillRectangle(0, 0, 1000, 600);

        // Eguzkia.
        canvas.FillColor = Color.FromArgb("#FFE5A1");
        canvas.FillCircle(835, 105, 52);

        // Hodeiak.
        MarraztuHodeia(canvas, 145, 105);
        MarraztuHodeia(canvas, 485, 80);
        MarraztuHodeia(canvas, 730, 175);
    }

    private static void MarraztuHodeia(
        ICanvas canvas, float x, float y)
    {
        canvas.FillColor = Color.FromArgb("#EAF5F5");

        canvas.FillEllipse(x, y + 15, 170, 45);
        canvas.FillEllipse(x + 30, y, 90, 65);
        canvas.FillEllipse(x + 90, y + 6, 65, 55);
    }

    private static void MarraztuMendiak(ICanvas canvas)
    {
        canvas.FillColor = Color.FromArgb("#6EA998");

        canvas.FillEllipse(-150, 270, 600, 250);
        canvas.FillEllipse(270, 260, 630, 270);
        canvas.FillEllipse(680, 290, 490, 240);

        canvas.FillColor = Color.FromArgb("#488879");

        canvas.FillEllipse(-100, 365, 570, 150);
        canvas.FillEllipse(350, 350, 700, 160);
    }

    private static void MarraztuLurra(ICanvas canvas)
    {
        canvas.FillColor = Color.FromArgb("#82AF69");
        canvas.FillRectangle(0, 425, 1000, 175);

        canvas.FillColor = Color.FromArgb("#5F9451");
        canvas.FillEllipse(-150, 415, 650, 150);
        canvas.FillEllipse(450, 440, 750, 170);

        // Belar txikiak.
        canvas.StrokeColor = Color.FromArgb("#407743");
        canvas.StrokeSize = 3;

        for (int x = 20; x < 1000; x += 43)
        {
            float y = 505 + (x % 5) * 12;

            canvas.DrawLine(x, y, x - 5, y - 12);
            canvas.DrawLine(x, y, x + 6, y - 15);
        }
    }

    private static void MarraztuAhatea(
        ICanvas canvas, Izakia ahatea)
    {
        canvas.SaveState();
        canvas.Translate(ahatea.X, ahatea.Y);

        bool azkarra = ahatea.Mota == IzakiMota.AhateAzkarra;

        // Ahate azkarrak gorriak dira; motelak, horiak.
        canvas.FillColor = azkarra
            ? Color.FromArgb("#D95E53")
            : Color.FromArgb("#F3C65B");

        // Gorputza.
        canvas.FillEllipse(-34, -18, 70, 39);

        // Lepoa eta burua.
        canvas.FillEllipse(14, -35, 30, 34);

        // Hegoen animazioa.
        float hegoa = MathF.Sin(ahatea.Animazioa) * 13;

        canvas.FillColor = azkarra
            ? Color.FromArgb("#A63D42")
            : Color.FromArgb("#D9A341");

        canvas.FillEllipse(-18, -18 + hegoa, 37, 24);

        // Mokoa.
        canvas.FillColor = Color.FromArgb("#F08B39");
        canvas.FillEllipse(36, -22, 20, 10);

        // Begia.
        canvas.FillColor = Colors.White;
        canvas.FillCircle(34, -26, 5);

        canvas.FillColor = Colors.Black;
        canvas.FillCircle(36, -26, 2.5f);

        // Isatsa.
        canvas.FillColor = azkarra
            ? Color.FromArgb("#A63D42")
            : Color.FromArgb("#D9A341");

        canvas.FillEllipse(-44, -22, 20, 15);

        canvas.RestoreState();
    }

    private static void MarraztuUntxia(
        ICanvas canvas, Izakia untxia)
    {
        canvas.SaveState();
        canvas.Translate(untxia.X, untxia.Y);

        canvas.FillColor = Color.FromArgb("#EEE1D2");

        // Belarriak.
        canvas.FillRoundedRectangle(-18, -48, 12, 39, 6);
        canvas.FillRoundedRectangle(5, -52, 12, 43, 6);

        // Gorputza eta burua.
        canvas.FillEllipse(-30, -20, 62, 39);
        canvas.FillCircle(7, -18, 20);

        canvas.FillColor = Color.FromArgb("#E7A8AD");
        canvas.FillCircle(20, -12, 5);

        canvas.FillColor = Colors.Black;
        canvas.FillCircle(15, -24, 3);

        canvas.RestoreState();
    }

    private static void MarraztuOztopoak(ICanvas canvas)
    {
        MarraztuZuhaitza(canvas, 310, 430, 1.15f);
        MarraztuZuhaitza(canvas, 725, 440, 1.05f);

        MarraztuHarria(canvas, 520, 460);
        MarraztuHarria(canvas, 900, 485);
    }

    private static void MarraztuZuhaitza(
        ICanvas canvas, float x, float y, float eskala)
    {
        canvas.SaveState();
        canvas.Translate(x, y);
        canvas.Scale(eskala, eskala);

        // Enborra.
        canvas.FillColor = Color.FromArgb("#79523D");
        canvas.FillRoundedRectangle(-19, -105, 38, 150, 8);

        // Hostoak.
        canvas.FillColor = Color.FromArgb("#285C43");
        canvas.FillCircle(-43, -130, 58);
        canvas.FillCircle(43, -130, 57);
        canvas.FillCircle(0, -173, 72);

        canvas.FillColor = Color.FromArgb("#367D53");
        canvas.FillCircle(-23, -188, 44);
        canvas.FillCircle(35, -167, 42);

        canvas.RestoreState();
    }

    private static void MarraztuHarria(
        ICanvas canvas, float x, float y)
    {
        canvas.FillColor = Color.FromArgb("#788985");
        canvas.FillEllipse(x - 90, y - 52, 180, 104);

        canvas.FillColor = Color.FromArgb("#9CAAA3");
        canvas.FillEllipse(x - 62, y - 45, 100, 48);
    }

    /// <summary>
    /// Ahatea oztopo baten atzean dagoen egiaztatzen du.
    /// </summary>
    public static bool Babestuta(float x, float y)
    {
        bool lehenZuhaitza =
            MathF.Pow((x - 310) / 107f, 2) +
            MathF.Pow((y - 257) / 100f, 2) <= 1;

        bool bigarrenZuhaitza =
            MathF.Pow((x - 725) / 105f, 2) +
            MathF.Pow((y - 259) / 100f, 2) <= 1;

        // Zuhaitzen enborrak ere oztopoak dira.
        bool lehenEnborra =
            x >= 310 - 19 * 1.15f &&
            x <= 310 + 19 * 1.15f &&
            y >= 430 - 105 * 1.15f &&
            y <= 430 + 45 * 1.15f;

        bool bigarrenEnborra =
            x >= 725 - 19 * 1.05f &&
            x <= 725 + 19 * 1.05f &&
            y >= 440 - 105 * 1.05f &&
            y <= 440 + 45 * 1.05f;

        // Harrien kolisioa marraztutako elipseari dagokio.
        bool lehenHarria =
            MathF.Pow((x - 520) / 90f, 2) +
            MathF.Pow((y - 460) / 52f, 2) <= 1;

        bool bigarrenHarria =
            MathF.Pow((x - 900) / 90f, 2) +
            MathF.Pow((y - 485) / 52f, 2) <= 1;

        return lehenZuhaitza || bigarrenZuhaitza ||
               lehenEnborra || bigarrenEnborra ||
               lehenHarria || bigarrenHarria;
    }

    private static void MarraztuDiana(
        ICanvas canvas, PointF puntua)
    {
        canvas.StrokeColor = Color.FromArgb("#E83333");
        canvas.StrokeSize = 3;

        canvas.DrawEllipse(
            puntua.X - 16, puntua.Y - 16, 32, 32);

        canvas.DrawLine(
            puntua.X - 25, puntua.Y,
            puntua.X - 8, puntua.Y);

        canvas.DrawLine(
            puntua.X + 8, puntua.Y,
            puntua.X + 25, puntua.Y);

        canvas.DrawLine(
            puntua.X, puntua.Y - 25,
            puntua.X, puntua.Y - 8);

        canvas.DrawLine(
            puntua.X, puntua.Y + 8,
            puntua.X, puntua.Y + 25);
    }
}
