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
    /// Логика взаимодействия для TenantsPage.xaml
    /// </summary>
    public partial class TenantsPage : Page
    {
        public DBcontext db;

        public TenantsPage()
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
                             where Client.role_id == 4

                             select new
                             {
                                 FIOtenant = Client.full_name,
                                 BirthDate = Client.birthday.ToShortDateString(),
                                 Address = Client.address,
                                 Phone = Client.phone_number,
                                 PassportDate = Client.passport_date.Date,
                                 PassportIssuedBy = Client.passport_issued_by,
                                 PassportSeriesNumber = Client.passport_series_number,
                                 RoleTenant = Client.role_id,

                                 dateBirthToDispley = Client.birthday.Date,
                                 datePassportToDispley = Client.passport_date.Date,

                                 IDTenant = Client.client_id,
                             };

            TenantsGrid.ItemsSource = resultGrid.ToList();

            var resultGrid2 = from Client in db.Clients.Local.ToList()
                              join t in db.Trades on Client.client_id equals t.buyer_id
                              where Client.role_id == 4

                             select new
                             {
                                 FIOtenant = Client.full_name,
                                 BirthDate = Client.birthday.ToShortDateString(),
                                 Address = Client.address,
                                 Phone = Client.phone_number,
                                 PassportDate = Client.passport_date.Date,
                                 PassportIssuedBy = Client.passport_issued_by,
                                 PassportSeriesNumber = Client.passport_series_number,
                                 RoleTenant = Client.role_id,

                                 dateBirthToDispley = Client.birthday.Date,
                                 datePassportToDispley = Client.passport_date.Date,

                                 IDTenant = Client.client_id,
                             };

            var endResult = resultGrid.Except(resultGrid2).ToList();

            TenantsGrid2.ItemsSource = endResult.ToList();
        }

        private void TBActive_Click(object sender, RoutedEventArgs e)
        {
            if (TenantsGrid.Visibility == Visibility.Visible)
            {
                TenantsGrid.Visibility = Visibility.Collapsed;
                TenantsGrid2.Visibility = Visibility.Visible;
            }
            else
            {
                TenantsGrid.Visibility = Visibility.Visible;
                TenantsGrid2.Visibility = Visibility.Collapsed;
            }
        }

        public void CallPopup(string text)
        {
            popupMessage.Text = text;
            Storyboard s = (Storyboard)this.TryFindResource("ShowPopup");
            BeginStoryboard(s);
        }

        private void ButtonDeleteTenant_Click(object sender, RoutedEventArgs e)
        {
            // если ни одного объекта не выделено, выходим
            if (TenantsGrid.SelectedItem == null)
            {
                CallPopup("Не выбран арендатор");
                return;
            }

            // получаем выделенный объект
            dynamic deleteTenant = TenantsGrid.SelectedItem;
            int IDTenant = deleteTenant.IDTenant;

            Client c = db.Clients.Find(IDTenant);
            if (c != null)
            {
                db.Clients.Remove(c);
                db.SaveChanges();
            }
            FillDataGrid();
        }

        private void ButtonEditTenant_Click(object sender, RoutedEventArgs e)
        {
            // если ни одного объекта не выделено, выходим
            if (TenantsGrid.SelectedItem == null)
            {
                CallPopup("Не выбран арендатор");
                return;
            }

            // редактирование
            dynamic tempT = TenantsGrid.SelectedItem;
            Client tenant = new Client();
            tenant.client_id = tempT.IDTenant;
            tenant.address = tempT.Address;
            tenant.birthday = tempT.dateBirthToDispley;
            tenant.full_name = tempT.FIOtenant;
            tenant.passport_date = tempT.datePassportToDispley;
            tenant.passport_issued_by = tempT.PassportIssuedBy;
            tenant.passport_series_number = tempT.PassportSeriesNumber;
            tenant.phone_number = tempT.Phone;
            tenant.role_id = tempT.RoleTenant;

            EditClientWindow edc = new EditClientWindow(tenant);
            edc.ShowDialog();
            FillDataGrid();
        }

        private void ButtonAddTenant_Click(object sender, RoutedEventArgs e)
        {
            AddClientWindow taskWindow = new AddClientWindow();
            taskWindow.Show();
            FillDataGrid();
        }

        private void TenantsGrid_DC(object sender, MouseButtonEventArgs e)
        {
            dynamic proT = TenantsGrid.SelectedItem;
            Client prTenant = new Client();
            prTenant.client_id = proT.IDTenant;
            prTenant.full_name = proT.FIOtenant;
            prTenant.birthday = proT.dateBirthToDispley;
            prTenant.address = proT.Address;
            prTenant.passport_date = proT.datePassportToDispley;
            prTenant.passport_issued_by = proT.PassportIssuedBy;
            prTenant.passport_series_number = proT.PassportSeriesNumber;
            prTenant.phone_number = proT.Phone;

            ClientProfileTenantWindow prof = new ClientProfileTenantWindow(prTenant);
            prof.ShowDialog();
        }

        private void TenantsGrid2_DC(object sender, MouseButtonEventArgs e)
        {
            dynamic proT = TenantsGrid2.SelectedItem;
            Client prTenant = new Client();
            prTenant.client_id = proT.IDTenant;
            prTenant.full_name = proT.FIOtenant;
            prTenant.birthday = proT.dateBirthToDispley;
            prTenant.address = proT.Address;
            prTenant.passport_date = proT.datePassportToDispley;
            prTenant.passport_issued_by = proT.PassportIssuedBy;
            prTenant.passport_series_number = proT.PassportSeriesNumber;
            prTenant.phone_number = proT.Phone;

            ClientProfileTenantWindow prof = new ClientProfileTenantWindow(prTenant);
            prof.ShowDialog();
        }
    }
}
