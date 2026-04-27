using DemoExamen2026.DataBase;
using DemoExamen2026.Statics;
using DemoExamen2026.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DemoExamen2026
{
    public partial class ProductWindow : Window
    {
        private DemoEcsamenDBEntities _db = new DemoEcsamenDBEntities();
        private List<TovarViewModel> _tovarViewModels = new List<TovarViewModel>();
        private string[] sortingTypes = new string[] { "По умолчанию", "По убыванию", "По возрастанию" };
        private List<string> _filterTypes = new List<string>() { "Все поставщики" };

        // Свойство для привязки выбранного товара
        public TovarViewModel SelectedProduct { get; set; }

        public ProductWindow()
        {
            InitializeComponent();
            DataContext = this; // для привязки SelectedProduct
            LoadProducts();
            LoadData();

            if (CurrentSession.CurrentUser != null)
            {
                FullUserName.Text = CurrentSession.CurrentUser.SecondName + " " +
                                    CurrentSession.CurrentUser.FirstName + " " +
                                    CurrentSession.CurrentUser.ThirdName;
            }
        }

        private void LoadData()
        {
            SortingComboBox.ItemsSource = sortingTypes;
            SortingComboBox.SelectedIndex = 0;

            var providers = _db.ProviderTable.ToList();
            foreach (var provider in providers)
                _filterTypes.Add(provider.Provider);

            FilteringComboBox.ItemsSource = _filterTypes;
            FilteringComboBox.SelectedIndex = 0;
        }

        private void LoadProducts()
        {
            var products = _db.TovarTable.ToList();
            _tovarViewModels = products.Select(p => new TovarViewModel(p)).ToList();
            UpdateProducts();
        }

        private void UpdateProducts()
        {
            // Создаём новую ObservableCollection для сброса старой связи
            ProductList.ItemsSource = new ObservableCollection<TovarViewModel>(_tovarViewModels);
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentSession.CurrentUser = null;
            new MainWindow().Show();
            Close();
        }

        private void SearchingTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchingTextBox.Text.ToLower();
            if (string.IsNullOrWhiteSpace(searchText))
            {
                LoadProducts();
            }
            else
            {
                var filtered = _db.TovarTable.Where(p =>
                    p.CategoryTable.Category.ToLower().Contains(searchText) ||
                    p.Commentary.ToLower().Contains(searchText) ||
                    p.ProviderTable.Provider.ToLower().Contains(searchText) ||
                    p.ManufacturerTable.Manufacturer.ToLower().Contains(searchText)
                ).ToList();
                _tovarViewModels = filtered.Select(p => new TovarViewModel(p)).ToList();
                UpdateProducts();
            }
        }

        private void SortingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int sortingType = SortingComboBox.SelectedIndex;
            if (sortingType == 0)
            {
                LoadProducts(); // сброс к исходному порядку
            }
            else if (sortingType == 1)
            {
                _tovarViewModels = _tovarViewModels.OrderByDescending(p => p.Count).ToList();
                UpdateProducts();
            }
            else if (sortingType == 2)
            {
                _tovarViewModels = _tovarViewModels.OrderBy(p => p.Count).ToList();
                UpdateProducts();
            }
        }

        private void FilteringComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string filterText = FilteringComboBox.SelectedValue?.ToString();
            if (string.IsNullOrEmpty(filterText) || filterText == "Все поставщики")
            {
                LoadProducts();
                return;
            }

            // Фильтруем текущий список _tovarViewModels (или можно из БД)
            var filtered = _tovarViewModels.Where(p => p.ProviderTable.Provider == filterText).ToList();
            _tovarViewModels = filtered;
            UpdateProducts();
        }

        private void AddEditButton_Click(object sender, RoutedEventArgs e)
        {
            TovarTable productToEdit = null;
            if (SelectedProduct != null)
            {
                productToEdit = _db.TovarTable.Find(SelectedProduct.ID);
            }

            var addEditWindow = new AddEditWindow(productToEdit);
            bool? result = addEditWindow.ShowDialog();
            if (result == true)
            {
                // Обновляем список товаров после сохранения
                LoadProducts();
                // Сбрасываем выделение
                SelectedProduct = null;
                // Обновляем фильтры, сортировку (можно просто перезагрузить)
                // Принудительно вызываем текущие настройки фильтрации/сортировки
                ApplyCurrentFiltersAndSorting();
            }
        }

        // Вспомогательный метод, чтобы после добавления/редактирования применить текущие настройки
        private void ApplyCurrentFiltersAndSorting()
        {
            // Сначала фильтрация
            string filterText = FilteringComboBox.SelectedValue?.ToString();
            if (filterText != null && filterText != "Все поставщики")
            {
                _tovarViewModels = _tovarViewModels.Where(p => p.ProviderTable.Provider == filterText).ToList();
            }
            // Потом сортировка
            int sortingType = SortingComboBox.SelectedIndex;
            if (sortingType == 1)
                _tovarViewModels = _tovarViewModels.OrderByDescending(p => p.Count).ToList();
            else if (sortingType == 2)
                _tovarViewModels = _tovarViewModels.OrderBy(p => p.Count).ToList();

            // И поиск
            string searchText = SearchingTextBox.Text;
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var lowerSearch = searchText.ToLower();
                _tovarViewModels = _tovarViewModels.Where(p =>
                    p.CategoryTable.Category.ToLower().Contains(lowerSearch) ||
                    (p.Commentary?.ToLower().Contains(lowerSearch) ?? false) ||
                    p.ProviderTable.Provider.ToLower().Contains(lowerSearch) ||
                    p.ManufacturerTable.Manufacturer.ToLower().Contains(lowerSearch)
                ).ToList();
            }

            UpdateProducts();
        }
    }
}