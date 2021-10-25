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
    /// Логика взаимодействия для EditAgentWindow.xaml
    /// </summary>
    public partial class EditAgentWindow : Window
    {
        public Agent agent { get; private set; }
        public DBcontext db;

        public EditAgentWindow(Agent a)
        {
            db = new DBcontext();
            agent = a;
            InitializeComponent();
            FIOagent.Text = a.full_name;
            BirthDate.Text = a.birthday.ToString();
            PassportSeriesNumber.Text = a.passport_series_number;
            PassportDate.Text = a.passport_date.ToString();
            PassportIssuedBy.Text = a.passport_issued_by;
            Address.Text = a.address;
            Phone.Text = a.phone_number;
            INN.Text = a.INN;         
        }

        public void CallPopup(string text)
        {
            popupMessage.Text = text;
            Storyboard s = (Storyboard)this.TryFindResource("ShowPopup");
            BeginStoryboard(s);
        }

        private void EditAgentWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            agent = db.Agents.Find(agent.agent_id);
            agent.full_name = FIOagent.Text;
            agent.birthday = (DateTime)BirthDate.SelectedDate;
            agent.address = Address.Text;
            agent.passport_series_number = PassportSeriesNumber.Text;
            agent.passport_issued_by = PassportIssuedBy.Text;
            agent.passport_date = (DateTime)PassportDate.SelectedDate;
            agent.phone_number = Phone.Text;
            agent.INN = INN.Text;
            db.Entry(agent).State = EntityState.Modified;
            db.SaveChanges();
            this.Close();
           
        }

        private void PassportIssuedBy_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
