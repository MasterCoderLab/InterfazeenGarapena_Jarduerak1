using System;
using System.Collections.Generic;
using System.Text;

namespace I01_06_OrdezkariaHautatzea;

class Ikaslea
{
    private string _izena = "";
    private string _abizena = "";

    public Color testuKolore = Colors.White;

    public string Izena
    {
        get
        {
            return _izena;
        }
        set
        {
            _izena = value;
            ErakustekoIzena = _izena + " " + _abizena;
        }
    }

    public string Abizena
    {
        get
        {
            return _abizena;
        }
        set
        {
            _abizena = value;
            ErakustekoIzena = _izena + " " + _abizena;
        }
    }

    public string? ErakustekoIzena { get; set; }
}
