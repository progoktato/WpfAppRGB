using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGBSzinek.Models
{
    class RGBpicManager : AbsRGBpicManager
    {
        public override void DrawCircle(int x, int y, RGBpixel szin, int radius = 20)
        {
            for (int sorIndex = Math.Max(1, y - radius); sorIndex <= Math.Min(PIC_HEIGHT, y + radius); sorIndex++)
            {
                for (int oszlopIndex = Math.Max(1, x - radius); oszlopIndex <= Math.Min(PIC_WIDTH, x + radius); oszlopIndex++)
                {
                    int dx = oszlopIndex - x;
                    int dy = sorIndex - y;
                    if (dx * dx + dy * dy <= radius * radius)
                    {
                        Pixelek[oszlopIndex, sorIndex] = szin;
                    }
                }
            }
        }

        public override int GetWhitePixelsCount()
        {
            return PixelList.Count(p => p.Red == 255 && p.Green == 255 && p.Blue == 255);
            //return Pixelek.Cast<RGBpixel>().Count(p => p.Red == 255 && p.Green == 255 && p.Blue == 255);
        }

        // 1. feladat
        public override void LoadFromTXT(string fileName)
        {
            string[] sorok = File.ReadAllLines(fileName);

            // Ensure Pixelek is initialized before use  
            if (Pixelek == null)
            {
                Pixelek = new RGBpixel[PIC_WIDTH + 1, PIC_HEIGHT + 1];
            }

            int sorIndex = 1; //!!! 1-től index  
            foreach (var sor in sorok)
            {
                var szamok = sor.Split();
                if (szamok.Length != PIC_WIDTH * 3)
                {
                    throw new ArgumentException("Eltérés van a számok számában!");
                }

                int oszlopIndex = 1; //!!! 1-től index  
                for (int index = 0; index < PIC_WIDTH * 3; index += 3)
                {
                    var ujPixel = new RGBpixel(byte.Parse(szamok[index]), byte.Parse(szamok[index + 1]), byte.Parse(szamok[index + 2]));
                    Pixelek[oszlopIndex, sorIndex] = ujPixel;
                    oszlopIndex++;
                }
                sorIndex++;
            }
        }

        // Kiegészítő feladat: 640 pixel széles és 360 pixel magas képet kezelünk
        public override void SaveToTXT(string fileName)
        {
            List<string> sorok = new List<string>();

            for (int sorIndex = 1; sorIndex <= PIC_HEIGHT; sorIndex++)
            {
                string sor = "";
                for (int oszlopIndex = 1; oszlopIndex <= PIC_WIDTH; oszlopIndex++)
                {
                    sor += $"{Pixelek[oszlopIndex, sorIndex].Red} {Pixelek[oszlopIndex, sorIndex].Green} {Pixelek[oszlopIndex, sorIndex].Blue}";
                    sor += oszlopIndex < PIC_WIDTH ? " " : "";
                }
                sorok.Add(sor);
            }

            File.WriteAllLines(fileName, sorok);
        }

        // 3. feladat

        /// <summary>
        /// A képen található pixeleket listába szervezi
        /// </summary>
        public List<RGBpixel> PixelList
        {
            get
            {
                List<RGBpixel> pixelList = new List<RGBpixel>();
                for (int i = 1; i < Pixelek.GetLength(0); i++)
                {
                    for (int j = 1; j < Pixelek.GetLength(1); j++)
                    {
                        pixelList.Add(Pixelek[i, j]);
                    }
                }
                return pixelList;
            }
        }


        // 5. feladat
        public bool VanHatarAzEgben(int sor, int kulonbseg)
        {
            for (int i = 1; i < Pixelek.GetLength(0) - 1; i++)
            {
                if (Math.Abs(Pixelek[i, sor].Blue - Pixelek[i + 1, sor].Blue) > kulonbseg)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
