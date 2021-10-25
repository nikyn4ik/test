using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для AddClientWindow.xaml
    /// </summary>
    public partial class AddClientWindow : Window
    {
        public DBcontext db;
        private Client newClient;

        public AddClientWindow()
        {
            InitializeComponent();
            db = new DBcontext();
            db.Roles.Load();
            Fill_AddClientWindow();
        }

        public void CallPopup(string text)
        {
            popupMessage.Text = text;
            Storyboard s = (Storyboard)this.TryFindResource("ShowPopup");
            BeginStoryboard(s);
        }

        private void AddClientWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        void Fill_AddClientWindow()
        {
            RoleCB.ItemsSource = db.Roles.Local;
        }

        private void ButtonAddClient(object sender, RoutedEventArgs e)
        {

            // Проверка на ввод всех данных

            if (FIOclient.Text.Length == 0 || PassportSeriesNumber.Text.Length == 0 || PassportIssuedBy.Text.Length == 0 || Address.Text.Length == 0 || Phone.Text.Length == 0 || RoleCB.SelectedIndex == -1)
            {
                CallPopup("Не заполнены все поля");
                return;
            }

            // Проверка серии и номера паспорта на количество введенных цифр

            if (PassportSeriesNumber.Text.Length != 10)
            {
                CallPopup("Некорректные серия и номер");
                return;
            }

            // Проверка номера телефона с помошью регулярного выражения

            string patternPhone = @"^[8][9]\d{9}$";
            Regex regexPh = new Regex(patternPhone);
            if (regexPh.IsMatch(Phone.Text) == false)
            {
                CallPopup("Некорректный телефон");
                return;
            }

            newClient = new Client();
            dynamic r = RoleCB.SelectedItem;

            newClient.full_name = FIOclient.Text;
            newClient.birthday = (DateTime)BirthDate.SelectedDate;
            newClient.passport_series_number = PassportSeriesNumber.Text;
            newClient.passport_issued_by = PassportIssuedBy.Text;
            newClient.passport_date = (DateTime)PassportDate.SelectedDate;
            newClient.address = Address.Text;
            newClient.phone_number = Phone.Text;
            newClient.role_id = r.role_id;

            db.Clients.Add(newClient);
            db.SaveChanges();

            //int index = RoleCB.SelectedIndex;

            FIOclient.Clear();
            BirthDate.SelectedDate = DateTime.Now;
            PassportSeriesNumber.Clear();
            PassportIssuedBy.Clear();
            PassportDate.SelectedDate = DateTime.Now;
            Address.Clear();
            Phone.Clear();
            RoleCB.SelectedIndex = -1;

            this.Close();
                                      
        }
             

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
