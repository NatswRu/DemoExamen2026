using DemoExamen2026.DataBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DemoExamen2026.ViewModels
{
    public class TovarViewModel
    {

        public TovarViewModel(TovarTable tovarTable)
        {
            ID = tovarTable.ID;
            Articule = tovarTable.Articule;
            Price = tovarTable.Price;
            Discount = tovarTable.Discount;
            Count = tovarTable.Count;
            Commentary = tovarTable.Commentary;
            Photo = tovarTable.Photo;
            CategoryTable = tovarTable.CategoryTable;
            ManufacturerTable = tovarTable.ManufacturerTable;
            MeasurementTable = tovarTable.MeasurementTable;
            ProductTable = tovarTable.ProductTable;
            ProviderTable = tovarTable.ProviderTable;

            GetBackground();
            GetPhoto();
            GetPrice();
        }

        public int ID { get; set; }
        public string Articule { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
        public int Count { get; set; }
        public string Commentary { get; set; }
        public string Photo { get; set; }
        public CategoryTable CategoryTable { get; set; }
        public ManufacturerTable ManufacturerTable { get; set; }
        public MeasurementTable MeasurementTable { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<OrderToTovarTable> OrderToTovarTable { get; set; }
        public ProductTable ProductTable { get; set; }
        public ProviderTable ProviderTable { get; set; }


        public Brush _background { get; set; }
        public int NewPrice { get; set; }

        private void GetBackground()
        {
            if (Count >= 15)
            {
                _background = (Brush)new BrushConverter().ConvertFromString("#2e8b57");
                return;
            } else if (Count == 0)
            {
                _background = Brushes.LightBlue;
                return;
            }
            else
            {
                _background = (Brush)new BrushConverter().ConvertFromString("#7fff00");
                return;
            }
        }

        private void GetPhoto()
        {
            if (Photo == null || Photo == "")
            {
                Photo = "/Res/picture.png";
                return;
            }
            return;
        }

        private void GetPrice()
        {
            NewPrice = Convert.ToInt32(Price - (Price * Discount * 0.01));
        }

    }
}
