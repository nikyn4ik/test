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
    /// Логика взаимодействия для LandlordsPage.xaml
    /// </summary>
    public partial class LandlordsPage : Page
    {
        public DBcontext db;

        public LandlordsPage()
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
            db.ObjectEstates.Load();

            var resultGrid = from Client in db.Clients.Local.ToList()
                             where Client.role_id == 3

                             select new
                             {
                                 FIOlandlord = Client.full_name,
                                 BirthDate = Client.birthday.ToShortDateString(),
                                 Address = Client.address,
                                 Phone = Client.phone_number,
                                 PassportDate = Client.passport_date.Date,
                                 PassportIssuedBy = Client.passport_issued_by,
                                 PassportSeriesNumber = Client.passport_series_number,
                                 RoleLandlords = Client.role_id,

                                 dateBirthToDispley = Client.birthday.Date,
                                 datePassportToDispley = Client.passport_date.Date,

                                 IDLandlord = Client.client_id
                             };

            LandlordsGrid.ItemsSource = resultGrid.ToList();

            var resultGrid2 = from Client in db.Clients.Local.ToList()
                              join o in db.ObjectEstates on Client.client_id equals o.owner_id
                              where Client.role_id == 3 && o.status_id == 5

                             select new
                             {
                                 FIOlandlord = Client.full_name,
                                 BirthDate = Client.birthday.ToShortDateString(),
                                 Address = Client.address,
                                 Phone = Client.phone_number,
                                 PassportDate = Client.passport_date.Date,
                                 PassportIssuedBy = Client.passport_issued_by,
                                 PassportSeriesNumber = Client.passport_series_number,
                                 RoleLandlords = Client.role_id,

                                 dateBirthToDispley = Client.birthday.Date,
                                 datePassportToDispley = Client.passport_date.Date,

                                 IDLandlord = Client.client_id
                             };

            LandlordsGrid2.ItemsSource = resultGrid2.ToList();
        }

        private void TBActive_Click(object sender, RoutedEventArgs e)
        {
            if (LandlordsGrid.Visibility == Visibility.Visible)
            {
                LandlordsGrid.Visibility = Visibility.Collapsed;
                LandlordsGrid2.Visibility = Visibility.Visible;
            }
            else
            {
                LandlordsGrid.Visibility = Visibility.Visible;
                LandlordsGrid2.Visibility = Visibility.Collapsed;
            }
        }

        public void CallPopup(string text)
        {
            popupMessage.Text = text;
            Storyboard s = (Storyboard)this.TryFindResource("ShowPopup");
            BeginStoryboard(s);
        }

        private void ButtonDeleteLandlord_Click(object sender, RoutedEventArgs e)
        {
            // если ни одного объекта не выделено, выходим
            if (LandlordsGrid.SelectedItem == null)
            {
                CallPopup("Не выбран арендодатель");
                return;
            }

            // получаем выделенный объект
            dynamic deleteLandlord = LandlordsGrid.SelectedItem;
            int IDLandlord = deleteLandlord.IDLandlord;

            Client c = db.Clients.Find(IDLandlord);
            if (c != null)
            {
                db.Clients.Remove(c);
                db.SaveChanges();
            }
            FillDataGrid();
        }

        private void ButtonEditLandlord_Click(object sender, RoutedEventArgs e)
        {
            // если ни одного объекта не выделено, выходим
            if (LandlordsGrid.SelectedItem == null)
            {
                CallPopup("Не выбран арендодатель");
                return;
            }

            // редактирование
            dynamic tempL = LandlordsGrid.SelectedItem;
            Client landlord = new Client();
            landlord.client_id = tempL.IDLandlord;
            landlord.address = tempL.Address;
            landlord.birthday = tempL.dateBirthToDispley;
            landlord.full_name = tempL.FIOlandlord;
            landlord.passport_date = tempL.datePassportToDispley;
            landlord.passport_issued_by = tempL.PassportIssuedBy;
            landlord.passport_series_number = tempL.PassportSeriesNumber;
            landlord.phone_number = tempL.Phone;
            landlord.role_id = tempL.RoleLandlords;

            EditClientWindow edc = new EditClientWindow(landlord);
            edc.ShowDialog();
            FillDataGrid();
        }

        private void ButtonAddLandlord_CLick(object sender, RoutedEventArgs e)
        {
            AddClientWindow taskWindow = new AddClientWindow();
            taskWindow.Show();
            FillDataGrid();
        }

        private void LandlordsGrid_DC(object sender, MouseButtonEventArgs e)
        {
            dynamic proT = LandlordsGrid.SelectedItem;
            Client prLandlord = new Client();
            prLandlord.client_id = proT.IDLandlord;
            prLandlord.full_name = proT.FIOlandlord;
            prLandlord.birthday = proT.dateBirthToDispley;
            prLandlord.address = proT.Address;
            prLandlord.passport_date = proT.datePassportToDispley;
            prLandlord.passport_issued_by = proT.PassportIssuedBy;
            prLandlord.passport_series_number = proT.PassportSeriesNumber;
            prLandlord.phone_number = proT.Phone;

            ClientProfileLandlordWindow prof = new ClientProfileLandlordWindow(prLandlord);
            prof.ShowDialog();
        }
    }
}
