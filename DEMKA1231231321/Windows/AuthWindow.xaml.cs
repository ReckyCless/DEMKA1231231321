using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
        }
        private void BtnLogIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var currentUser = new Models.Users();
                var login = TextBoxLogin.Text;
                var password = PassBoxPassword.Password;
                currentUser = App.Context.Users.Where(x => x.login == login && x.password == password)
                    .AsEnumerable()
                    .FirstOrDefault(x => x.login == login && x.password == password);
                if (currentUser != null)
                {
                    App.SessionUser = currentUser;
                    MainWindow mainWindow = new MainWindow();  
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Данный пользователь - не найден", "Ошибка");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                App.SessionUser = null;

                string directory = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources/Images"));
                System.IO.Directory.CreateDirectory(directory);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnGuest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
