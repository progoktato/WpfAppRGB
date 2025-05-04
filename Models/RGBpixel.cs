using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGBSzinek.Models
{
    public class RGBpixel
    {
        byte red;
        byte green;
        byte blue;

        public byte Red { get => red; set => red = value; }
        public byte Green { get => green; set => green = value; }
        public byte Blue { get => blue; set => blue = value; }

        public RGBpixel(byte red, byte green, byte blue)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
        }

        /// <summary>
        /// A pixel színének világosítása
        /// </summary>
        /// <param name="value"></param>
        /// 
        public void Lighter(int value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// A pixel színének sötétítése
        /// </summary>
        /// <param name="value"></param>
        public void Darker(int value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// A pixel színének szöveges formában történő megjelenítéséhez
        /// </summary>
        /// <returns></returns>
        public override string? ToString()
        {
            return $"A képpont színe RGB ({this.red} , {this.green} , {this.blue})";
        }

        // 3. feladat
        public bool IsLight()
        {
            if (this.red + this.green + this.blue > 600)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Rövideben propertyvel
        public bool IsLight2 => this.red + this.green + this.blue > 600;

        // 4. feladat
        public int ColorSum => this.red + this.green + this.blue;

    }
}
