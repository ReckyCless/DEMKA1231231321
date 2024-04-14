using Aspose.BarCode.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DEMKA1231231321.Windows
{
    /// <summary>
    /// Логика взаимодействия для QRWindow.xaml
    /// </summary>
    public partial class QRWindow : Window
    {
        string qrUrl;
        public QRWindow(string url)
        {
            InitializeComponent();
            qrUrl = url;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                BitmapImage bitmap = new BitmapImage();
                BarcodeGenerator gen = new BarcodeGenerator(EncodeTypes.QR, qrUrl);
                gen.Parameters.Barcode.XDimension.Pixels = 34;

                string directory = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources"));
                gen.Save(directory + "/1.png", BarCodeImageFormat.Png);

                bitmap.BeginInit();
                bitmap.UriSource = new Uri(directory + "/1.png");
                bitmap.EndInit();

                imageQR.Source = bitmap;
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
