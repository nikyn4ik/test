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
    /// Логика взаимодействия для AddAgentWindow.xaml
    /// </summary>
    public partial class AddAgentWindow : Window
    {
        public DBcontext db;
        private Agent newAgent;

        public AddAgentWindow()
        {
            InitializeComponent();
            db = new DBcontext();
        }

        public void CallPopup(string text)
        {
            popupMessage.Text = text;
            Storyboard s = (Storyboard)this.TryFindResource("ShowPopup");
            BeginStoryboard(s);
        }

        private void AddAgentWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

              
        private void ButtonAddAgent(object sender, RoutedEventArgs e)
        {
            if (FIOagent.Text.Length == 0 || PassportSeriesNumber.Text.Length == 0 || PassportIssuedBy.Text.Length == 0 || Address.Text.Length == 0 || Phone.Text.Length == 0 || INN.Text.Length == 0)
            {
                CallPopup("Не заполнены все поля");
                return;
            }

            if (PassportSeriesNumber.Text.Length != 10)
            {
                CallPopup("Некорректные серия и номер");
                return;
            }

            string patternPhone = @"^[8][9]\d{9}$";
            Regex regexPh = new Regex(patternPhone);
            if (regexPh.IsMatch(Phone.Text) == false)
            {
                CallPopup("Некорректный телефон");
                return;
            }
                      
            if (INN.Text.Length != 10)
            {
                CallPopup("Некорректный ИНН");
                return;
            }
                        
            newAgent = new Agent();
                        
            newAgent.full_name = FIOagent.Text;
            newAgent.birthday = (DateTime)BirthDate.SelectedDate;
            newAgent.passport_series_number = PassportSeriesNumber.Text;
            newAgent.passport_issued_by = PassportIssuedBy.Text;
            newAgent.passport_date = (DateTime)PassportDate.SelectedDate;
            newAgent.address = Address.Text;
            newAgent.phone_number = Phone.Text;
            newAgent.INN = INN.Text;

            db.Agents.Add(newAgent);
            db.SaveChanges();
            
            FIOagent.Clear();
            BirthDate.SelectedDate = DateTime.Now;
            PassportSeriesNumber.Clear();
            PassportIssuedBy.Clear();
            PassportDate.SelectedDate = DateTime.Now;
            Address.Clear();
            Phone.Clear();
            INN.Clear();
            this.Close();

        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
