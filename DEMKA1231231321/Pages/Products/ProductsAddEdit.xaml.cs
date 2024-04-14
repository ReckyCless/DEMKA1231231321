using DEMKA1231231321.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DEMKA1231231321.Pages.Products
{
    /// <summary>
    /// Логика взаимодействия для ProductsAddEdit.xaml
    /// </summary>
    public partial class ProductsAddEdit : Page
    {
        private Models.Products currentElem = new Models.Products();

        bool isEditing = false;

        string pathToImage = string.Empty;
        string pathInDataBase = string.Empty;
        string previewPath = string.Empty;
        string directory = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources"));

        BitmapImage imagePreviewBit = new BitmapImage();

        public List<Models.ProductsMaterials> productsMaterialsContext = App.Context.ProductsMaterials.ToList();
        public List<Materials> materialsContext = App.Context.Materials.ToList();
        public List<Materials> materialsOld = new List<Materials>();
        public List<Materials> materialsView = new List<Materials>();
        public ProductsAddEdit(Models.Products elemData)
        {
            InitializeComponent();

            if (elemData != null)
            {
                Title = "Продукты. Редактирование";
                currentElem = elemData;

                isEditing = true;
                txtArticle.IsEnabled = false;


                var productMaterials = productsMaterialsContext.Where(x => x.product_id == currentElem.article);
                foreach (var material in productMaterials ) 
                {
                    materialsOld.Add(material.Materials);
                }
                materialsView = materialsOld;
                dtgrMaterials.ItemsSource = materialsView;
            }
            DataContext = currentElem;

            if (currentElem.image_path != null && currentElem.image_path != "")
            {
                pathToImage = directory + @"/Images/" + currentElem.image_path;
                pathInDataBase = currentElem.image_path;

                imagePreview.Source = new BitmapImage(new Uri(pathToImage, UriKind.Absolute));
            }
            else
            {
                pathToImage = directory + @"/default.png";
                imagePreview.Source = new BitmapImage(new Uri(pathToImage, UriKind.Absolute));
            }
            cmbMaterials.ItemsSource = materialsContext;

            cmbCategories.ItemsSource = App.Context.Categories.ToList();
            cmbManufacturer.ItemsSource = App.Context.Manufacturers.ToList();
            cmbProvider.ItemsSource = App.Context.Providers.ToList();
            cmbMeasureUnit.ItemsSource = App.Context.MeasureUnits.ToList();
        }
        #region Regexes
        private void Space_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) 
            { 
                e.Handled = true;
            }
        }
        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9]");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void Text_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9a-zA-Zа-яА-Я<>""(),-]");
            e.Handled = !regex.IsMatch(e.Text);
        }
        #endregion
        private void cmbMaterials_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedMaterial = cmbMaterials.SelectedItem as Materials;
            if (materialsView.Contains(selectedMaterial) || selectedMaterial == null)
            {
                MessageBox.Show("Данный материал - уже в списке", "Ошибка");
            }
            else
            {
                materialsView.Add(selectedMaterial);
                ProductsMaterials productMaterials = new ProductsMaterials(){ Products = currentElem, Materials = selectedMaterial };
                App.Context.ProductsMaterials.Add(productMaterials);
                productsMaterialsContext.Add(productMaterials);
                dtgrMaterials.ItemsSource = materialsView;
                dtgrMaterials.Items.Refresh();
            }
        }

        private void DeleteMaterial_Click(object sender, RoutedEventArgs e)
        {
            if (dtgrMaterials.SelectedCells.Count > 0)
            {
                var elemToDelete = dtgrMaterials.SelectedItem as Materials;
                if (materialsOld.Contains(elemToDelete))
                {
                    materialsOld.Remove(elemToDelete);
                    var productMaterial = productsMaterialsContext.FirstOrDefault(p => p.Products == currentElem && p.Materials == elemToDelete);
                    productsMaterialsContext.Remove(productMaterial);
                    App.Context.ProductsMaterials.Remove(productMaterial);
                }
                materialsView.Remove(elemToDelete);
                dtgrMaterials.ItemsSource = materialsView;
                dtgrMaterials.Items.Refresh();
            }
        }

        private void btnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Image | *.png; *.jpg; *.jpeg" };
            if (ofd.ShowDialog() == true)
            {
                string filename = ofd.SafeFileName;
                previewPath = ofd.FileName;
                pathToImage = directory + @"\Images\" + filename;
                if (isEditing)
                {
                    if (pathInDataBase == filename)
                    {
                        filename = "copy_" + filename;
                        pathToImage = directory + @"\Images\" + filename;
                        pathInDataBase = filename;
                    }
                    else
                    {
                        pathInDataBase = filename;
                    }
                }
                else
                {
                    pathInDataBase = filename;
                }

                try
                {
                    imagePreview.Source = new BitmapImage(new Uri(previewPath, UriKind.Absolute));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(previewPath))
                    File.Copy(previewPath, pathToImage, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            currentElem.image_path = pathInDataBase;

            // Check if fields are filled
            StringBuilder err = new StringBuilder();
            if (string.IsNullOrWhiteSpace(currentElem.article))
                err.AppendLine("Укажите артикул");
            else if (currentElem.article.Length != 6)
                err.AppendLine("Артикул должен быть 6 символов в длину");
            else if (!isEditing && App.Context.Products.Any(p => p.article == currentElem.article))
                err.AppendLine("Артикль не может повторяться");
            if (string.IsNullOrWhiteSpace(currentElem.title))
                err.AppendLine("Укажите название");
            if (currentElem.max_discount < 0 || currentElem.max_discount > 100)
                err.AppendLine("Макс. скидка должна быть числом от 0 до 100");
            if (currentElem.current_discount < 0 || currentElem.current_discount > 100)
                err.AppendLine("Текущая скидка должна быть числом от 0 до 100");
            else if (currentElem.current_discount > currentElem.max_discount)
                err.AppendLine("Текущая скидка не может быть больше максимальной");
            if (currentElem.count < 0)
                err.AppendLine("Кол-во на складе должно быть положительным числом");
            if (currentElem.Categories == null)
                err.AppendLine("Выберите категорию");
            if (currentElem.Providers == null)
                err.AppendLine("Выберите поставщика");
            if (currentElem.Manufacturers == null)
                err.AppendLine("Выберите производителя");
            if (currentElem.MeasureUnits == null)
                err.AppendLine("Выберите ед. измерения");


            if (err.Length > 0)
            {
                MessageBox.Show(err.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            currentElem.title = currentElem.title.Trim();
            currentElem.title = Regex.Replace(currentElem.title, @"\s+", " ");


            if (currentElem.description != null)
            {
                currentElem.description = currentElem.description.Trim();
                currentElem.description = Regex.Replace(currentElem.description, @"\s+", " ");
            }

            if (!isEditing)
            {
                App.Context.Products.Add(currentElem);
            }

            try
            {
                App.Context.SaveChanges();
                MessageBox.Show("Данные сохранены");
                NavigationService.Navigate(new Products.ProductsView());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
