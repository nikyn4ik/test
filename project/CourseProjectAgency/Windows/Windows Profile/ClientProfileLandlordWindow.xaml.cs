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
    /// Логика взаимодействия для ClientProfileLandlordWindow.xaml
    /// </summary>
    public partial class ClientProfileLandlordWindow : Window
    {
        public Client client { get; private set; }
        public DBcontext db;

        public ClientProfileLandlordWindow(Client c)
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
            KolOb.Text = ClientObjectGrid.Items.Count.ToString();
        }

        private void FillDataGrid(Client c)
        {
            client = c;
            Flat f = new Flat();

            var res = from Client in db.Clients.Local.ToList()
                      join O in db.ObjectEstates on Client.client_id equals O.owner_id
                      where Client.role_id == 3 && Client.client_id == c.client_id && O.status_id == 5

                      select new
                      {
                          Address = (db.Flats.Find(O.object_id) != null) ? O.address + " кв. " + O.Flat.flat_number : O.address,
                          Kind = (db.Flats.Find(O.object_id) != null) ? "Квартира" : "Частный дом",
                          Price = O.price,

                      };

            ClientObjectGrid.ItemsSource = res.ToList();

            var res2 = from Client in db.Clients.Local.ToList()
                       join O in db.ObjectEstates on Client.client_id equals O.owner_id
                       join t in db.Trades on O.object_id equals t.object_id
                       where Client.role_id == 3 && Client.client_id == c.client_id

                       select new
                       {
                           Date = t.date.ToShortDateString(),
                           Object = O.address,
                           Price = t.Rent.rent_price,

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
            GoToObject.Visibility = Visibility.Visible;
            BackToTrades.Visibility = Visibility.Collapsed;

            ProfileGrid.Visibility = Visibility.Visible;
            InfoGrid.Visibility = Visibility.Collapsed;
            ObjectGrid.Visibility = Visibility.Collapsed;
        }

        private void GoToInfo_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GoToTrades.Visibility = Visibility.Visible;
            GoToInfo.Visibility = Visibility.Collapsed;
            GoToObject.Visibility = Visibility.Collapsed;
            BackToTrades.Visibility = Visibility.Collapsed;

            ProfileGrid.Visibility = Visibility.Collapsed;
            InfoGrid.Visibility = Visibility.Visible;
            ObjectGrid.Visibility = Visibility.Collapsed;
        }

        private void GoToObject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GoToTrades.Visibility = Visibility.Collapsed;
            GoToInfo.Visibility = Visibility.Collapsed;
            GoToObject.Visibility = Visibility.Collapsed;
            BackToTrades.Visibility = Visibility.Visible;

            ProfileGrid.Visibility = Visibility.Collapsed;
            InfoGrid.Visibility = Visibility.Collapsed;
            ObjectGrid.Visibility = Visibility.Visible;
        }

        private void BackToTrades_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GoToTrades.Visibility = Visibility.Collapsed;
            GoToInfo.Visibility = Visibility.Visible;
            GoToObject.Visibility = Visibility.Visible; ;
            BackToTrades.Visibility = Visibility.Collapsed;

            ProfileGrid.Visibility = Visibility.Visible;
            InfoGrid.Visibility = Visibility.Collapsed;
            ObjectGrid.Visibility = Visibility.Collapsed;

        }
    }
}
