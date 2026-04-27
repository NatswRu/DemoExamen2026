using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DemoExamen2026.Helper
{
    internal class _HWShow
    {
        public void _ShowMessage(string message)
        {
            MessageBox.Show(message, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public void _ShowError(string error)
        {
            MessageBox.Show(error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public void _ShowWarning(string warning)
        {
            MessageBox.Show(warning, "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
