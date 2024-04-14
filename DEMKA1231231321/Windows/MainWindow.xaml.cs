using DEMKA1231231321.Models;
using DEMKA1231231321.Pages.Products;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DEMKA1231231321.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            frameMain.Navigate(new Pages.Products.ProductsView());
        }

        private void frameMain_ContentRendered(object sender, EventArgs e)
        {
            if (frameMain.CanGoBack)
            {
                btnBack.Visibility = Visibility.Visible;
            }
            else
            {
                btnBack.Visibility = Visibility.Collapsed;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (frameMain.CanGoBack)
            {
                frameMain.GoBack();
            }
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow authWindow = new AuthWindow(); 
            authWindow.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
