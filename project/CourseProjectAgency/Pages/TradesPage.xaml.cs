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

namespace CourseProjectAgency
{
    /// <summary>
    /// Логика взаимодействия для TradesPage.xaml
    /// </summary>
    public partial class TradesPage : Page
    {
        public TradesPage()
        {
            InitializeComponent();
            FrameTrades.Content = new SaleTradePage();
        }

        private void ButtonSale_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameTrades.Content = new SaleTradePage();
        }

        private void ButtonRent_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameTrades.Content = new RentTradePage();
        }
                
    }
}
