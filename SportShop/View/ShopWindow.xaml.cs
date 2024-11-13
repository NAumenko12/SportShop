using SportShop.ViewModel;
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

namespace SportShop.View
{
    /// <summary>
    /// Логика взаимодействия для ShopWindow.xaml
    /// </summary>
    public partial class ShopWindow : UserControl
    {
        public ShopWindow(int userId)
        {
            InitializeComponent();
            DataContext = new ShopViewModel(userId);
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Логика обработки события TextChanged
        }
    }
}
