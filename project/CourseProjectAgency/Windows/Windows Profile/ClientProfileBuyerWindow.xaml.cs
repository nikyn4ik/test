using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace CourseProjectAgency
{
    /// <summary>
    /// Логика взаимодействия для ClientProfileBuyerWindow.xaml
    /// </summary>
    public partial class ClientProfileBuyerWindow : Window
    {
        public Client client { get; private set; }
        public DBcontext db;

        public ClientProfileBuyerWindow(Client c)
        {
            InitializeComponent();
            db = new DBcontext();
            client = c;

            FIO.Text = c.full_name;
            Birthday.Text = c.birthday.ToShortDateString();
            HomeAddress.Text = c.address;
            PassportDate.Text = c.passport_date.ToShortDateString();
            PassportIssuedBy.Text = c.passport_issued_by;
            PassportSeriesNumber.Text = c.passport_series_number;
            Phone.Text = c.phone_number;


            db.Sales.Load();
            db.ObjectEstates.Load();
            db.Clients.Load();
            db.Trades.Load();
            FillDataGrid(c);

            Kol.Text = ClientTrade.Items.Count.ToString();
        }

        private void FillDataGrid(Client c)
        {
            client = c;           

            var res2 = from Client in db.Clients.Local.ToList()
                       join t in db.Trades on Client.client_id equals t.buyer_id
                       join o in db.ObjectEstates on t.object_id equals o.object_id
                       where Client.role_id == 2 && Client.client_id == c.client_id

                       select new
                       {
                           Date = t.date.ToShortDateString(),
                           Object = o.address,
                           Price = t.Sale.price,

                       };

            ClientTrade.ItemsSource = res2.ToList();

        }

        private void ClientProfileWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void ClientProfile_Back(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void GoToTrades_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            GoToTrades.Visibility = Visibility.Collapsed;
            GoToInfo.Visibility = Visibility.Visible;

            ProfileGrid.Visibility = Visibility.Visible;
            InfoGrid.Visibility = Visibility.Collapsed;
        }

        private void GoToInfo_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GoToTrades.Visibility = Visibility.Visible;
            GoToInfo.Visibility = Visibility.Collapsed;
           
            ProfileGrid.Visibility = Visibility.Collapsed;
            InfoGrid.Visibility = Visibility.Visible;
            
        }
    }
}
