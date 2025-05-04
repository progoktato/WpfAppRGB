using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGBSzinek.Models
{
    abstract class AbsRGBpicManager
    {
        public const int PIC_WIDTH = 640;
        public const int PIC_HEIGHT = 360;
        //Struktúra, amiben tárolunk
        RGBpixel[,] pixelek = new RGBpixel[PIC_WIDTH + 1, PIC_HEIGHT + 1];

        public RGBpixel[,] Pixelek { get => pixelek; set => pixelek = value; }

        /// <summary>
        /// Képkezelő absztrakt osztály konstruktora
        /// Feket színű pixelek inicializálása
        /// </summary>
        public AbsRGBpicManager()
        {
            for (int i = 0; i < pixelek.GetLength(0); i++)
            {
                for (int j = 0; j < pixelek.GetLength(1); j++)
                {
                    pixelek[i, j] = new RGBpixel(0, 0, 0); // Fekete szín
                }
            }
        }

        /// <summary>
        /// Kép betöltése szöveges fájlból
        /// </summary>
        /// <param name="fileName"></param>
        abstract public void LoadFromTXT(String fileName);

        /// <summary>
        /// Kép mentése szöveges fájlba
        /// </summary>
        /// <param name="fileName"></param>
        abstract public void SaveToTXT(String fileName);


        /// <summary>
        /// Számlálás, hogy hány fehér pixel van a képen
        /// </summary>
        /// <returns></returns>
        abstract public int GetWhitePixelsCount();

        /// <summary>
        /// Kör rajzolása a képre
        /// </summary>
        /// <param name="x">A kör középpontjának X koordinátája</param>
        /// <param name="y">A kör középpontjának Y koordinátája</param>
        /// <param name="radius">A kör sugara</param>
        abstract public void DrawCircle(int x, int y, RGBpixel szin, int radius = 20);
    }
}
