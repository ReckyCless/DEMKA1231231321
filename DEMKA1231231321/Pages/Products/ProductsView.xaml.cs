using DEMKA1231231321.Models;
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
using DEMKA1231231321.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Data.SqlTypes;
using Aspose.BarCode.Generation;
using DEMKA1231231321.Windows;

namespace DEMKA1231231321.Pages.Products
{
    /// <summary>
    /// Логика взаимодействия для ProductsView.xaml
    /// </summary>
    public partial class ProductsView : Page
    {

        public ProductsView()
        {
            InitializeComponent();

            DataContext = new Models.Products();

            List<Categories> categories = App.Context.Categories.ToList();
            categories.Insert(0, new Categories
            {
                name = "Без фильтрации"
            });
            ComboBoxFilter.ItemsSource = categories;
            ComboBoxFilter.SelectedIndex = 0;
            ComboBoxSort.SelectedIndex = 0;

            UpdateDataSource();
        }

        private void UpdateDataSource() { 
            var dataList = App.Context.Products.ToList();
            int allElemsCount = dataList.Count;
            int afterFilterCount = 0;

            if (App.SessionUser == null)
            {
                dataList = null;
            }
            else
            {
                if (ComboBoxFilter.SelectedIndex > 0)
                {
                    dataList = dataList.Where(x => x.Categories == (Categories)ComboBoxFilter.SelectedItem).ToList();
                }

                switch (ComboBoxSort.SelectedIndex)
                {
                    case 1:
                        dataList = dataList.OrderBy(x => x.title).ToList();
                        break;
                    case 2:
                        dataList = dataList.OrderByDescending(x => x.title).ToList();
                        break;
                    case 3:
                        dataList = dataList.OrderBy(x => x.price).ToList();
                        break;
                    case 4:
                        dataList = dataList.OrderByDescending(x => x.price).ToList();
                        break;
                    default:
                        dataList = dataList.OrderBy(x => x.article).ToList();
                        break;
                }

                var searchText = TextBoxSearch.Text.ToLower();
                if (searchText != "" && searchText != null)
                {
                    dataList = dataList.Where(x => x.title.ToLower().Contains(searchText) ||
                    x.Categories.name.ToLower().Contains(searchText) ||
                    x.Providers.name.ToLower().Contains(searchText) ||
                    x.Manufacturers.name.ToLower().Contains(searchText)                   
                    ).ToList();
                }

                afterFilterCount = dataList.Count;
            }

            ListViewMain.ItemsSource = dataList;

            TextBlockCounter.Text = $"{afterFilterCount} из {allElemsCount}";

            if (afterFilterCount < 0)
            {
                TextBlockCounter.Text += $"Ничего не найдено. Измените фильтры или войдите в аккаунт";
            }
        }

        private void ComboBoxFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDataSource();
        }

        private void ComboBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDataSource();
        }

        private void TextBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateDataSource();
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (App.SessionUser != null)
                {
                    if (App.SessionUser.role_id == 1)
                    {
                        if (ListViewMain.SelectedItems != null)
                        {
                            NavigationService.Navigate(new ProductsAddEdit(ListViewMain.SelectedItem as Models.Products));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new ProductsAddEdit(null));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (App.SessionUser.role_id == 1)
                {
                    if (ListViewMain.SelectedItems.Count > 0)
                    {
                        var elemsToDelete = ListViewMain.SelectedItems.Cast<Models.Products>().ToList();
                        if (MessageBox.Show($"Вы действительно хотите удалить {elemsToDelete.Count} элементов?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            foreach (var item in elemsToDelete)
                            {
                                App.Context.Products.Remove(item);
                            }
                            App.Context.SaveChanges();
                            MessageBox.Show("Данные удалены");
                            UpdateDataSource();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateDataSource();
        }

        private void ButtonPDF_Click(object sender, RoutedEventArgs e)
        {
            Document doc = new Document(); 
            string directory = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources"));
            try
            {
                PdfWriter.GetInstance(doc, new FileStream(directory + @"/output.pdf", FileMode.Create));

                doc.Open();
                BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                Font font = new Font(baseFont, 12);
                Font fontHeader = new Font(baseFont, 24, 3, BaseColor.BLUE);
                iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph("СПИСОК ТОВАРОВ", fontHeader);
                paragraph.Alignment = Element.ALIGN_CENTER;
                doc.Add(paragraph);
                decimal sum = 0;

                foreach (var item in App.Context.Products.ToList()) 
                {
                    if (item is Models.Products)
                    {
                        Models.Products data = (Models.Products)item;

                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(data.ImagePath);
                        img.ScaleAbsolute(100f, 100f);
                        doc.Add(img);
                        doc.Add(new iTextSharp.text.Paragraph("Название: " + data.TitleCategory, font));
                        doc.Add(new iTextSharp.text.Paragraph("Описание: " + data.description, font));
                        doc.Add(new iTextSharp.text.Paragraph(data.ManufacturerText, font));
                        doc.Add(new iTextSharp.text.Paragraph(data.ProviderText, font));
                        doc.Add(new iTextSharp.text.Paragraph("Стоимость без скидки: " + data.price.ToString() +" руб.", font));                    
                        doc.Add(new iTextSharp.text.Paragraph("Скидка: " + data.current_discount.ToString() + " руб.", font));

                        sum += data.price;
                    }
                }
                iTextSharp.text.Paragraph paragraphSum = new iTextSharp.text.Paragraph("Сумма: " + sum.ToString(), font);
                paragraphSum.Alignment = Element.ALIGN_RIGHT;
                doc.Add(paragraphSum);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally 
            { 
                doc.Close();
                MessageBox.Show("PDF был создан. Путь до PDF: " + directory + @"/output.pdf");
            }
        }

        private void ButtonQR_Click(object sender, RoutedEventArgs e)
        {
            QRWindow qrWindow = new QRWindow("https://bom.firpo.ru/Public/86");
            qrWindow.ShowDialog();
        }
    }
}
