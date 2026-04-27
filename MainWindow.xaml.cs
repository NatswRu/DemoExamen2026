using DemoExamen2026.DataBase;
using DemoExamen2026.Helper;
using DemoExamen2026.Statics;
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

namespace DemoExamen2026
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DemoEcsamenDBEntities _db = new DemoEcsamenDBEntities();
        private _HWShow _HW = new _HWShow();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Login_Click(object sender, RoutedEventArgs e)
        {
            string login = Login_Box.Text;
            string password = Password_Box.Password;

            var user = _db.UserTable.Where(u => u.Mail == login && u.Password == password).FirstOrDefault();

            if (user == null)
            {
                _HW._ShowWarning("Такого пользователя нет");
                return;
            }
            else 
            {
                CurrentSession.CurrentUser = user;
                ProductWindow productWindow = new ProductWindow();
                productWindow.Show();
                Close();
            }
            return;
        }

        private void User_Another_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ProductWindow productWindow = new ProductWindow();
            productWindow.Show();
            Close();
        }
    }
}
