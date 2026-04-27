using DemoExamen2026.DataBase;
using Microsoft.Win32;
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

namespace DemoExamen2026
{
    /// <summary>
    /// Логика взаимодействия для AddEditWindow.xaml
    /// </summary>
    public partial class AddEditWindow : Window
    {
        private DemoEcsamenDBEntities _db = new DemoEcsamenDBEntities();
        private TovarTable _editingProduct;

        public AddEditWindow(TovarTable product = null)
        {
            InitializeComponent();
            LoadReferenceData();
            if (product != null)
            {
                _editingProduct = product;
                LoadProductData();
                Title = "Редактирование товара";
            }
            else
            {
                Title = "Добавление нового товара";
            }
        }

        private void LoadReferenceData()
        {
            ProductCombo.ItemsSource = _db.ProductTable.ToList();
            CategoryCombo.ItemsSource = _db.CategoryTable.ToList();
            ManufacturerCombo.ItemsSource = _db.ManufacturerTable.ToList();
            ProviderCombo.ItemsSource = _db.ProviderTable.ToList();
            MeasurementCombo.ItemsSource = _db.MeasurementTable.ToList();
        }

        private void LoadProductData()
        {
            ArticuleBox.Text = _editingProduct.Articule;
            ProductCombo.SelectedValue = _editingProduct.ProductID;
            CategoryCombo.SelectedValue = _editingProduct.CategoryID;
            ManufacturerCombo.SelectedValue = _editingProduct.ManufacturerID;
            ProviderCombo.SelectedValue = _editingProduct.ProviderID;
            MeasurementCombo.SelectedValue = _editingProduct.MeasurementID;
            PriceBox.Text = _editingProduct.Price.ToString();
            DiscountBox.Text = _editingProduct.Discount.ToString();
            CountBox.Text = _editingProduct.Count.ToString();
            CommentaryBox.Text = _editingProduct.Commentary;
            PhotoBox.Text = _editingProduct.Photo;
        }

        private void BrowsePhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                PhotoBox.Text = openFileDialog.FileName;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(ArticuleBox.Text) ||
                ProductCombo.SelectedValue == null ||
                CategoryCombo.SelectedValue == null ||
                ManufacturerCombo.SelectedValue == null ||
                ProviderCombo.SelectedValue == null ||
                MeasurementCombo.SelectedValue == null ||
                !int.TryParse(PriceBox.Text, out int price) ||
                !int.TryParse(DiscountBox.Text, out int discount) ||
                !int.TryParse(CountBox.Text, out int count))
            {
                MessageBox.Show("Заполните все обязательные поля корректными значениями.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (discount < 0 || discount > 100)
            {
                MessageBox.Show("Скидка должна быть от 0 до 100.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (_editingProduct == null) // Добавление
                {
                    var newProduct = new TovarTable
                    {
                        Articule = ArticuleBox.Text,
                        ProductID = (int)ProductCombo.SelectedValue,
                        CategoryID = (int)CategoryCombo.SelectedValue,
                        ManufacturerID = (int)ManufacturerCombo.SelectedValue,
                        ProviderID = (int)ProviderCombo.SelectedValue,
                        MeasurementID = (int)MeasurementCombo.SelectedValue,
                        Price = price,
                        Discount = discount,
                        Count = count,
                        Commentary = CommentaryBox.Text,
                        Photo = PhotoBox.Text
                    };
                    _db.TovarTable.Add(newProduct);
                }
                else
                {
                    _editingProduct.Articule = ArticuleBox.Text;
                    _editingProduct.ProductID = (int)ProductCombo.SelectedValue;
                    _editingProduct.CategoryID = (int)CategoryCombo.SelectedValue;
                    _editingProduct.ManufacturerID = (int)ManufacturerCombo.SelectedValue;
                    _editingProduct.ProviderID = (int)ProviderCombo.SelectedValue;
                    _editingProduct.MeasurementID = (int)MeasurementCombo.SelectedValue;
                    _editingProduct.Price = price;
                    _editingProduct.Discount = discount;
                    _editingProduct.Count = count;
                    _editingProduct.Commentary = CommentaryBox.Text;
                    _editingProduct.Photo = PhotoBox.Text;
                }

                _db.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
