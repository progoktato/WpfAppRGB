using Microsoft.Win32;
using RGBSzinek.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAppRGB.Models;

namespace WpfAppRGB;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    ObservableCollection<ViewModelRGB> keppontok = new();
    RGBpicManager picManager = new RGBpicManager();

    public MainWindow()
    {
        InitializeComponent();
        dgPixelek.ItemsSource = keppontok;
        cbSzuro.SelectedIndex = 0;
    }


    private void sliderRed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        UpdateRGBTextAndColor();
    }

    private void UpdateRGBTextAndColor()
    {
        txtRGB.Text = $"R: {sliderRed.Value} G: {sliderGreen.Value} B: {sliderBlue.Value}";
        recSzín.Fill = new SolidColorBrush(Color.FromRgb((byte)sliderRed.Value, (byte)sliderGreen.Value, (byte)sliderBlue.Value));
    }

    private void sliderBlue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        UpdateRGBTextAndColor();
    }

    private void sliderGreen_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        UpdateRGBTextAndColor();
    }

    private void btnLoad_Click(object sender, RoutedEventArgs e)
    {
        if (keppontok.Count > 0)
        {
            if (MessageBox.Show("Elvész a jelenlegi képe, biztosan másikat szeretne betölteni? ", "Figyelem!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
        }

        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
        openFileDialog.Title = "Válassza ki a fájlt!";
        openFileDialog.CheckFileExists = true;
        openFileDialog.CheckPathExists = true;
        openFileDialog.Multiselect = false;
        openFileDialog.ShowDialog();
        string fileName = openFileDialog.FileName;
        if (fileName != "")
        {
            try
            {
                picManager.LoadFromTXT(fileName);
                UpdatePixelCollection();
                txtPixelekSzama.Text = $"A pixelek száma: {dgPixelek.Items.Count} db";

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt a fájl beolvasásakor: {ex.Message}");
            }
        }

    }

    private void UpdatePixelCollection()
    {
        keppontok.Clear();
        for (int sorIndex = 1; sorIndex <= RGBpicManager.PIC_HEIGHT; sorIndex++)
        {
            for (int oszlopIndex = 1; oszlopIndex <= RGBpicManager.PIC_WIDTH; oszlopIndex++)
            {
                var pixel = picManager.Pixelek[oszlopIndex, sorIndex];
                var ujPixel = new ViewModelRGB(oszlopIndex, sorIndex, pixel);
                keppontok.Add(ujPixel);
            }
        }
        DisplayImageOnCanvas();
    }

    private void DisplayImageOnCanvas()
    {
        WriteableBitmap bitmap = new WriteableBitmap(
            RGBpicManager.PIC_WIDTH,
            RGBpicManager.PIC_HEIGHT,
            96, // DPI (vízszintes)
            96, // DPI (függőleges)
            PixelFormats.Bgr32, // Pixel formátum
            null // Paletta (nem szükséges Bgr32 esetén)
        );

        int stride = RGBpicManager.PIC_WIDTH * 4; // 4 byte per pixel (Bgr32)
        byte[] pixelData = new byte[RGBpicManager.PIC_HEIGHT * stride];

        for (int sorIndex = 1; sorIndex <= RGBpicManager.PIC_HEIGHT; sorIndex++)
        {
            for (int oszlopIndex = 1; oszlopIndex <= RGBpicManager.PIC_WIDTH; oszlopIndex++)
            {
                var pixel = picManager.Pixelek[oszlopIndex, sorIndex];
                int index = ((sorIndex - 1) * stride) + ((oszlopIndex - 1) * 4);

                pixelData[index] = pixel.Blue;      // Blue
                pixelData[index + 1] = pixel.Green; // Green
                pixelData[index + 2] = pixel.Red;   // Red
                pixelData[index + 3] = 255;         // Alpha (teljesen átlátszatlan)
            }
        }

        bitmap.WritePixels(
            new Int32Rect(0, 0, RGBpicManager.PIC_WIDTH, RGBpicManager.PIC_HEIGHT),
            pixelData,
            stride,
            0
        );

        Image image = new Image
        {
            Source = bitmap,
            Width = RGBpicManager.PIC_WIDTH,
            Height = RGBpicManager.PIC_HEIGHT
        };

        canvasKep.Children.Clear();
        canvasKep.Children.Add(image);
    }

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
        if (keppontok.Count == 0)
        {
            MessageBox.Show("Nincs mit menteni!");
            return;
        }
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
        saveFileDialog.Title = "Válassza ki a fájlt!";
        saveFileDialog.CheckFileExists = false;
        saveFileDialog.CheckPathExists = true;
        saveFileDialog.ShowDialog();
        string fileName = saveFileDialog.FileName;
        if (fileName == "")
        {
            return;
        }
        picManager.SaveToTXT(fileName);
    }

    private void comboBoxColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (cbSzuro.SelectedIndex == 1) //piros
        {
            ObservableCollection<ViewModelRGB> szurtPiros = new();
            foreach (var pixel in keppontok.Where(p => p.Red > 200))
            {
                szurtPiros.Add(pixel);
            }
            dgPixelek.ItemsSource = szurtPiros;
        }
        else if (cbSzuro.SelectedIndex == 2) //zöld
        {
            ObservableCollection<ViewModelRGB> szurtZold = new();
            foreach (var pixel in keppontok.Where(p => p.Green > 200))
            {
                szurtZold.Add(pixel);
            }
            dgPixelek.ItemsSource = szurtZold;

        }
        else if (cbSzuro.SelectedIndex == 3) //kék
        {
            ObservableCollection<ViewModelRGB> szurtKek = new();
            foreach (var pixel in keppontok.Where(p => p.Blue > 200))
            {
                szurtKek.Add(pixel);
            }
            dgPixelek.ItemsSource = szurtKek;
        }
        else if (cbSzuro.SelectedIndex == 0)
        {
            dgPixelek.ItemsSource = keppontok;
        }
        txtPixelekSzama.Text = $"A pixelek száma: {dgPixelek.Items.Count} db";

    }

    private void dgPixelek_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (dgPixelek.SelectedItem == null)
        {
            return;
        }
        sliderRed.Value = ((ViewModelRGB)dgPixelek.SelectedItem).Red;
        sliderBlue.Value = ((ViewModelRGB)dgPixelek.SelectedItem).Blue;
        sliderGreen.Value = ((ViewModelRGB)dgPixelek.SelectedItem).Green;
    }

    private void btnSzín_Click(object sender, RoutedEventArgs e)
    {
        if (cbSzuro.SelectedIndex != 0)
        {
            MessageBox.Show("Kérem először állítsa vissza a szűrőt az összes pixelre!");
            return;
        }

        if (dgPixelek.SelectedItem == null)
        {
            MessageBox.Show("Kérem válasszon ki egy pixelt!");
            return;
        }
        var selectedPixel = (ViewModelRGB)dgPixelek.SelectedItem;
        RGBpixel pixel = picManager.Pixelek[selectedPixel.Oszlop, selectedPixel.Sor];
        pixel.Red = (byte)sliderRed.Value;
        pixel.Green = (byte)sliderGreen.Value;
        pixel.Blue = (byte)sliderBlue.Value;
        var ujPixel = new ViewModelRGB(selectedPixel.Oszlop, selectedPixel.Sor, pixel);
        var melyikaz = keppontok.IndexOf(selectedPixel);
        keppontok[melyikaz] = ujPixel;
    }

    private void btnFrissit_Click(object sender, RoutedEventArgs e)
    {
        if (keppontok.Count == 0)
        {
            MessageBox.Show("Nincs mit frissíteni!");
            return;
        }
        DisplayImageOnCanvas();
    }

    private void btnKor_Click(object sender, RoutedEventArgs e)
    {
        if (keppontok.Count == 0)
        {
            MessageBox.Show("Nincs mire rajzolni!");
            return;
        }
        int kx, ky, sugar;
        try
        {
            kx = int.Parse(txtKozepX.Text.Trim());
            ky = int.Parse(txtKozepY.Text.Trim());
            sugar = int.Parse(txtSugar.Text.Trim());
        }
        catch (Exception)
        {
            MessageBox.Show($"Valamelyik adat nem értelmezhető számként!");
            txtKozepX.Text = "";
            txtKozepY.Text = "";
            txtSugar.Text = "";
            return;
        }
        if (kx < 1 || kx > RGBpicManager.PIC_WIDTH || ky < 1 || ky > RGBpicManager.PIC_HEIGHT)
        {
            MessageBox.Show($"A kör középpontja nem lehet a képen kívül!");
            return;
        }
        if (sugar < 1 || sugar > 300)
        {
            MessageBox.Show($"A kör sugara 1..300 lehet!");
            return;
        }

        picManager.DrawCircle(kx, ky, new RGBpixel((byte)sliderRed.Value, (byte)sliderGreen.Value, (byte)sliderBlue.Value), sugar);
        UpdatePixelCollection();
    }
}
