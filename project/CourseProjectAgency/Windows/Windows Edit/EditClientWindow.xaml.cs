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
using System.Windows.Shapes;

namespace CourseProjectAgency
{
    /// <summary>
    /// Логика взаимодействия для EditClientWindow.xaml
    /// </summary>
    public partial class EditClientWindow : Window
    {
        public Client client { get; private set; }
        public DBcontext db;

        public EditClientWindow(Client c)
        {
            db = new DBcontext();
            db.Roles.Load();
            client = c;
            InitializeComponent();
            FIOclient.Text = c.full_name;
            BirthDate.Text = c.birthday.ToString();
            PassportSeriesNumber.Text = c.passport_series_number;
            PassportDate.Text = c.passport_date.ToString();
            PassportIssuedBy.Text = c.passport_issued_by;
            Address.Text = c.address;
            Phone.Text = c.phone_number;

            RoleCB.ItemsSource = db.Roles.Local;
            RoleCB.SelectedItem = db.Roles.Find(client.role_id);
        }

        public void CallPopup(string text)
        {
            popupMessage.Text = text;
            Storyboard s = (Storyboard)this.TryFindResource("ShowPopup");
            BeginStoryboard(s);
        }

        private void EditClientWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonEditClient(object sender, RoutedEventArgs e)
        {

            client = db.Clients.Find(client.client_id);
            client.full_name = FIOclient.Text;
            client.birthday = (DateTime)BirthDate.SelectedDate;
            client.address = Address.Text;
            client.passport_series_number = PassportSeriesNumber.Text;
            client.passport_issued_by = PassportIssuedBy.Text;
            client.passport_date = (DateTime)PassportDate.SelectedDate;
            client.phone_number = Phone.Text;

            Role r = (Role)RoleCB.SelectedItem;
            client.role_id = r.role_id;


            db.Entry(client).State = EntityState.Modified;
            db.SaveChanges();
            this.Close();

        }
    }
}
