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
    /// Логика взаимодействия для SellersPage.xaml
    /// </summary>
    public partial class SellersPage : Page
    {
        public DBcontext db;

        public SellersPage()
        {
            InitializeComponent();
            //db = new DBcontext();
            //db.Clients.Load();
            FillDataGrid();
        }

        public void FillDataGrid()
        {
            db = new DBcontext();
            db.Clients.Load();
            db.Trades.Load();
            db.ObjectEstates.Load();

            var resultGrid = from Client in db.Clients.Local.ToList()
                             where Client.role_id == 1

                             select new
                             {
                                 FIOseller = Client.full_name,
                                 BirthDate = Client.birthday.ToShortDateString(),
                                 Address = Client.address,
                                 Phone = Client.phone_number,
                                 PassportDate = Client.passport_date.Date,
                                 PassportIssuedBy = Client.passport_issued_by,
                                 PassportSeriesNumber = Client.passport_series_number,
                                 RoleSeller = Client.role_id,

                                 dateBirthToDispley = Client.birthday.Date,
                                 datePassportToDispley = Client.passport_date.Date,

                                 IDSeller = Client.client_id,
                             };

            SellersGrid.ItemsSource = resultGrid.ToList();

            var resultGrid2 = from Client in db.Clients.Local.ToList()
                              join o in db.ObjectEstates on Client.client_id equals o.owner_id
                              where Client.role_id == 1 && o.status_id == 1

                             select new
                             {
                                 FIOseller = Client.full_name,
                                 BirthDate = Client.birthday.ToShortDateString(),
                                 Address = Client.address,
                                 Phone = Client.phone_number,
                                 PassportDate = Client.passport_date.Date,
                                 PassportIssuedBy = Client.passport_issued_by,
                                 PassportSeriesNumber = Client.passport_series_number,
                                 RoleSeller = Client.role_id,

                                 dateBirthToDispley = Client.birthday.Date,
                                 datePassportToDispley = Client.passport_date.Date,

                                 IDSeller = Client.client_id,
                             };

            SellersGrid2.ItemsSource = resultGrid2.ToList();
        }

        private void TBActive_Click(object sender, RoutedEventArgs e)
        {
            if (SellersGrid.Visibility == Visibility.Visible)
            {
                SellersGrid.Visibility = Visibility.Collapsed;
                SellersGrid2.Visibility = Visibility.Visible;
            }
            else
            {
                SellersGrid.Visibility = Visibility.Visible;
                SellersGrid2.Visibility = Visibility.Collapsed;
            }
        }


        public void CallPopup(string text)
        {
            popupMessage.Text = text;
            Storyboard s = (Storyboard)this.TryFindResource("ShowPopup");
            BeginStoryboard(s);
        }

        private void ButtonDeleteSeller_Click(object sender, RoutedEventArgs e)
        {
            // если ни одного объекта не выделено, выходим
            if (SellersGrid.SelectedItem == null)
            {
                CallPopup("Не выбран продавец");
                return;
            }

            // получаем выделенный объект
            dynamic deleteSeller = SellersGrid.SelectedItem;
            int IDSeller = deleteSeller.IDSeller;

            Client c = db.Clients.Find(IDSeller);
            if (c != null)
            {
                db.Clients.Remove(c);
                db.SaveChanges();
            }
            FillDataGrid();
        }

        private void ButtonEditSeller_Click(object sender, RoutedEventArgs e)
        {
            // если ни одного объекта не выделено, выходим
            if (SellersGrid.SelectedItem == null)
            {
                CallPopup("Не выбран продавец");
                return;
            }
            // редактирование
            dynamic tempS = SellersGrid.SelectedItem;
            Client seller = new Client();
            seller.client_id = tempS.IDSeller;
            seller.address = tempS.Address;
            seller.birthday = tempS.dateBirthToDispley;
            seller.full_name = tempS.FIOseller;
            seller.passport_date = tempS.datePassportToDispley;
            seller.passport_issued_by = tempS.PassportIssuedBy;
            seller.passport_series_number = tempS.PassportSeriesNumber;
            seller.phone_number = tempS.Phone;
            seller.role_id = tempS.RoleSeller;

            EditClientWindow edc = new EditClientWindow(seller);
            edc.ShowDialog();
            FillDataGrid();
        }

        private void ButtonAddSeller_Click(object sender, RoutedEventArgs e)
        {
            AddClientWindow taskWindow = new AddClientWindow();
            taskWindow.Show();
            FillDataGrid();           
        }

        private void SellersGrid_DC(object sender, MouseButtonEventArgs e)
        {
            dynamic proS = SellersGrid.SelectedItem;
            Client prSeller = new Client();
            prSeller.client_id = proS.IDSeller;
            prSeller.full_name = proS.FIOseller;
            prSeller.birthday = proS.dateBirthToDispley;
            prSeller.address = proS.Address;
            prSeller.passport_date = proS.datePassportToDispley;
            prSeller.passport_issued_by = proS.PassportIssuedBy;
            prSeller.passport_series_number = proS.PassportSeriesNumber;
            prSeller.phone_number = proS.Phone;

            ClientProfileWindow prof = new ClientProfileWindow(prSeller);
            prof.ShowDialog();
        }

        private void SellersGrid2_DC(object sender, MouseButtonEventArgs e)
        {
            dynamic proS = SellersGrid2.SelectedItem;
            Client prSeller = new Client();
            prSeller.client_id = proS.IDSeller;
            prSeller.full_name = proS.FIOseller;
            prSeller.birthday = proS.dateBirthToDispley;
            prSeller.address = proS.Address;
            prSeller.passport_date = proS.datePassportToDispley;
            prSeller.passport_issued_by = proS.PassportIssuedBy;
            prSeller.passport_series_number = proS.PassportSeriesNumber;
            prSeller.phone_number = proS.Phone;

            ClientProfileWindow prof = new ClientProfileWindow(prSeller);
            prof.ShowDialog();
        }
    }
}
