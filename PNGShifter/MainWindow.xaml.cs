using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PNGShifter
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public OpenFileDialog ofd = new OpenFileDialog();

        public int numFiles;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ofd.Multiselect = true;
            ofd.Filter = "PNG|*.png";
            if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                imagebox.Source = LoadBitmapImage(ofd.FileName);
                LeftButton_Clicked(null, null);
                numFiles = ofd.FileNames.Length;
                label.Content = "0/" + numFiles;
            }
        }

        static Bitmap ShiftImage(Bitmap img, bool left, bool right, bool up, bool down)
        {
            if (left)
            {
                for (int i = 0; i < img.Width; i++)
                {
                    for (int j = 0; j < img.Height; j++)
                    {
                        if (img.GetPixel(i, j).A != 0)
                        {
                            for (int l = 0; l < img.Height; l++)
                            {
                                for (int k = 0; k < img.Width - i; k++)
                                {
                                    img.SetPixel(k, l, img.GetPixel(k + i, l));

                                }
                                for (int k = img.Width - i; k < img.Width; k++)
                                {
                                    img.SetPixel(k, l, System.Drawing.Color.FromArgb(0, System.Drawing.Color.White));

                                }
                            }

                            goto sec;
                        }

                    }
                }
            }
            else if (right)
            {
                for (int i = img.Width - 1; i >= 0; i--)
                {
                    for (int j = 0; j < img.Height; j++)
                    {
                        if (img.GetPixel(i, j).A != 0)
                        {
                            int diff = (img.Width - 1) - i;

                            for (int l = 0; l < img.Height; l++)
                            {
                                for (int k = img.Width - 1; k >= diff; k--)
                                {
                                    img.SetPixel(k, l, img.GetPixel(k - diff, l));
                                }
                                for (int k = diff-1; k >= 0; k--)
                                {
                                    img.SetPixel(k, l, System.Drawing.Color.FromArgb(0, System.Drawing.Color.White));
                                }
                            }

                            goto sec;

                        }

                    }
                }
            }

            sec:

            if (up)
            {
                for (int i = 0; i < img.Height; i++)
                {
                    for (int j = 0; j < img.Width; j++)
                    {
                        if (img.GetPixel(j, i).A != 0)
                        {
                            for (int l = 0; l < img.Width; l++)
                            {
                                for (int k = 0; k < img.Height - i; k++)
                                {
                                    img.SetPixel(l, k, img.GetPixel(l, k + i));
                                }
                                for (int k = img.Height - i; k < img.Height; k++)
                                {
                                    img.SetPixel(l, k, System.Drawing.Color.FromArgb(0, System.Drawing.Color.White));
                                }
                            }

                            return img;
                        }

                    }
                }
            }
            else if (down)
            {
                for (int i = img.Height - 1; i >= 0; i--)
                {
                    for (int j = 0; j < img.Width; j++)
                    {
                        if (img.GetPixel(j, i).A != 0)
                        {
                            int diff = (img.Height - 1) - i;

                            for (int l = 0; l < img.Width; l++)
                            {
                                for (int k = img.Height - 1; k >= diff; k--)
                                {
                                    img.SetPixel(l, k, img.GetPixel(l, k - diff));
                                }
                                for (int k = diff-1; k >= 0; k--)
                                {
                                    img.SetPixel(l, k, System.Drawing.Color.FromArgb(0, System.Drawing.Color.White));
                                }
                            }

                            return img;

                        }

                    }
                }
            }


            return img;
        }

        BitmapImage LoadBitmapImage(string path)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(path);
            image.EndInit();
            return image;
        }

        BitmapImage LoadBitmapImage(Bitmap image)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                image.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }

        }

        private void LeftButton_Clicked(object sender, RoutedEventArgs e)
        {
            RightButton.IsChecked = false;
            Bitmap img = ShiftImage((Bitmap)System.Drawing.Image.FromFile(ofd.FileName), (bool)LeftButton.IsChecked, (bool)RightButton.IsChecked, (bool)UpButton.IsChecked, (bool)DownButton.IsChecked);
            imagebox.Source = LoadBitmapImage(img);
            img.Dispose();
        }

        private void RightButton_Clicked(object sender, RoutedEventArgs e)
        {
            LeftButton.IsChecked = false;
            Bitmap img = ShiftImage((Bitmap)System.Drawing.Image.FromFile(ofd.FileName), (bool)LeftButton.IsChecked, (bool)RightButton.IsChecked, (bool)UpButton.IsChecked, (bool)DownButton.IsChecked);
            imagebox.Source = LoadBitmapImage(img);
            img.Dispose();
        }

        private void UpButton_Clicked(object sender, RoutedEventArgs e)
        {
            DownButton.IsChecked = false;
            Bitmap img = ShiftImage((Bitmap)System.Drawing.Image.FromFile(ofd.FileName), (bool)LeftButton.IsChecked, (bool)RightButton.IsChecked, (bool)UpButton.IsChecked, (bool)DownButton.IsChecked);
            imagebox.Source = LoadBitmapImage(img);
            img.Dispose();
        }

        private void DownButton_Clicked(object sender, RoutedEventArgs e)
        {
            UpButton.IsChecked = false;
            Bitmap img = ShiftImage((Bitmap)System.Drawing.Image.FromFile(ofd.FileName), (bool)LeftButton.IsChecked, (bool)RightButton.IsChecked, (bool)UpButton.IsChecked, (bool)DownButton.IsChecked);
            imagebox.Source = LoadBitmapImage(img);
            img.Dispose();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            bool l = (bool)LeftButton.IsChecked;
            bool r = (bool)RightButton.IsChecked;
            bool u = (bool)UpButton.IsChecked;
            bool d = (bool)DownButton.IsChecked;
            Thread t = new Thread(() => Work(l,r,u,d));
            t.Start();

        }

        void Work(bool l, bool r, bool u, bool d)
        {
            int i = 0;
            foreach (var item in ofd.FileNames)
            {
                Bitmap original = ShiftImage((Bitmap)System.Drawing.Image.FromFile(item), l, r, u, d);
                Bitmap copy = new Bitmap(original);
                original.Dispose();
                if (System.IO.File.Exists(item))
                    System.IO.File.Delete(item);

                copy.Save(item, System.Drawing.Imaging.ImageFormat.Png);

                Dispatcher.Invoke((Action)(() =>
                {
                    label.Content = ++i + "/" + numFiles;
                }));

            }
        }
    }
}
