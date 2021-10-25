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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CourseProjectAgency
{
    /// <summary>
    /// Логика взаимодействия для BuyersPage.xaml
    /// </summary>
    public partial class BuyersPage : Page
    {
        public DBcontext db;

        public BuyersPage()
        {
            InitializeComponent();
            db = new DBcontext();
            db.Clients.Load();
            FillDataGrid();
        }

        public void FillDataGrid()
        {
            db = new DBcontext();
            db.Clients.Load();
            db.Trades.Load();
            
            var resultGrid = from Client in db.Clients.Local.ToList()
                             where Client.role_id == 2

                             select new
                             {
                                 FIObuyer = Client.full_name,
                                 BirthDate = Client.birthday.ToShortDateString(),
                                 Address = Client.address,
                                 Phone = Client.phone_number,
                                 PassportDate = Client.passport_date.Date,
                                 PassportIssuedBy = Client.passport_issued_by,
                                 PassportSeriesNumber = Client.passport_series_number,
                                 RoleBuyer = Client.role_id,

                                 dateBirthToDispley = Client.birthday.Date,
                                 datePassportToDispley = Client.passport_date.Date,

                                 IDBuyer = Client.client_id,

                             };

            BuyersGrid.ItemsSource = resultGrid.ToList();

            
            
                var resultGrid2 = from Client in db.Clients.Local.ToList()
                                  join t in db.Trades on Client.client_id equals t.buyer_id
                                  where Client.role_id == 2 
                                  select new
                                  {
                                      FIObuyer = Client.full_name,
                                      BirthDate = Client.birthday.ToShortDateString(),
                                      Address = Client.address,
                                      Phone = Client.phone_number,
                                      PassportDate = Client.passport_date.Date,
                                      PassportIssuedBy = Client.passport_issued_by,
                                      PassportSeriesNumber = Client.passport_series_number,
                                      RoleBuyer = Client.role_id,

                                      dateBirthToDispley = Client.birthday.Date,
                                      datePassportToDispley = Client.passport_date.Date,

                                      IDBuyer = Client.client_id,

                                  };            

            var endResult = resultGrid.Except(resultGrid2).ToList();

            BuyersGrid2.ItemsSource = endResult.ToList();
        }

        private void TBActive_Click(object sender, RoutedEventArgs e)
        {
            if (BuyersGrid.Visibility == Visibility.Visible)
            {
                BuyersGrid.Visibility = Visibility.Collapsed;
                BuyersGrid2.Visibility = Visibility.Visible;
            }
            else
            {
                BuyersGrid.Visibility = Visibility.Visible;
                BuyersGrid2.Visibility = Visibility.Collapsed;
            }
        }

        public void CallPopup(string text)
        {
            popupMessage.Text = text;
            Storyboard s = (Storyboard)this.TryFindResource("ShowPopup");
            BeginStoryboard(s);
        }

        private void ButtonDeleteBuyer_Click(object sender, RoutedEventArgs e)
        {
            // если ни одного объекта не выделено, выходим
            if (BuyersGrid.SelectedItem == null)
            {
                CallPopup("Не выбран покупатель");
                return;
            }

            // получаем выделенный объект
            dynamic deleteBuyer = BuyersGrid.SelectedItem;
            int IDBuyer = deleteBuyer.IDBuyer;

            Client c = db.Clients.Find(IDBuyer);
            if (c != null)
            {
                db.Clients.Remove(c);
                db.SaveChanges();
            }
            FillDataGrid();
        }

        private void ButtonEditBuyer_Click(object sender, RoutedEventArgs e)
        {
            // если ни одного объекта не выделено, выходим
            if (BuyersGrid.SelectedItem == null)
            {
                CallPopup("Не выбран покупатель");
                return;
            }

            // редактирование
            dynamic tempB = BuyersGrid.SelectedItem;
            Client buyer = new Client();
            buyer.client_id = tempB.IDBuyer;
            buyer.address = tempB.Address;
            buyer.birthday = tempB.dateBirthToDispley;
            buyer.full_name = tempB.FIObuyer;
            buyer.passport_date = tempB.datePassportToDispley;
            buyer.passport_issued_by = tempB.PassportIssuedBy;
            buyer.passport_series_number = tempB.PassportSeriesNumber;
            buyer.phone_number = tempB.Phone;
            buyer.role_id = tempB.RoleBuyer;

            EditClientWindow edc = new EditClientWindow(buyer);
            edc.ShowDialog();
            FillDataGrid();
        }

        private void ButtonAddClient_Click(object sender, RoutedEventArgs e)
        {
            AddClientWindow taskWindow = new AddClientWindow();
            taskWindow.Show();
            FillDataGrid();
        }

        private void BuyersGrid_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            dynamic proB = BuyersGrid.SelectedItem;
            Client prBuyer = new Client();
            prBuyer.client_id = proB.IDBuyer;
            prBuyer.full_name = proB.FIObuyer;
            prBuyer.birthday = proB.dateBirthToDispley;
            prBuyer.address = proB.Address;
            prBuyer.passport_date = proB.datePassportToDispley;
            prBuyer.passport_issued_by = proB.PassportIssuedBy;
            prBuyer.passport_series_number = proB.PassportSeriesNumber;
            prBuyer.phone_number = proB.Phone;

            ClientProfileBuyerWindow prof = new ClientProfileBuyerWindow(prBuyer);
            prof.ShowDialog();
        }

        private void BuyersGrid2_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            dynamic proB = BuyersGrid2.SelectedItem;
            Client prBuyer = new Client();
            prBuyer.client_id = proB.IDBuyer;
            prBuyer.full_name = proB.FIObuyer;
            prBuyer.birthday = proB.dateBirthToDispley;
            prBuyer.address = proB.Address;
            prBuyer.passport_date = proB.datePassportToDispley;
            prBuyer.passport_issued_by = proB.PassportIssuedBy;
            prBuyer.passport_series_number = proB.PassportSeriesNumber;
            prBuyer.phone_number = proB.Phone;

            ClientProfileBuyerWindow prof = new ClientProfileBuyerWindow(prBuyer);
            prof.ShowDialog();
        }
    }
}
