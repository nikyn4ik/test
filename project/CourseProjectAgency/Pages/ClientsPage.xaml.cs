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
    /// Логика взаимодействия для ClientsPage.xaml
    /// </summary>
    public partial class ClientsPage : Page
    {
        public ClientsPage()
        {
            InitializeComponent();
            FrameClients.Content = new SellersPage();
        }

        private void ButtonSellers_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameClients.Content = new SellersPage();
        }

        private void ButtonBuyers_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameClients.Content = new BuyersPage();
        }

        private void ButtonLandlords_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameClients.Content = new LandlordsPage();
        }

        private void ButtonTenants_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameClients.Content = new TenantsPage();
        }
    }
}
