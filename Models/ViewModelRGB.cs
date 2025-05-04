using RGBSzinek.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfAppRGB.Models
{
    class ViewModelRGB
    {
        public int Oszlop { get; }
        public int Sor { get; }
        public byte Red { get; }
        public byte Green { get; }
        public byte Blue { get; }
        public bool Vilagos { get; }

        public ViewModelRGB(int oszlop, int sor, RGBpixel pixel)
        {
            Oszlop = oszlop;
            Sor = sor;
            Red = pixel.Red;
            Green = pixel.Green;
            Blue = pixel.Blue;
            Vilagos = pixel.IsLight();
        }
        public Brush Szín => new SolidColorBrush(Color.FromRgb(Red, Green, Blue));
    }
}
