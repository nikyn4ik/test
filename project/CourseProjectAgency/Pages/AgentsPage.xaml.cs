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
    /// Логика взаимодействия для AgentsPage.xaml
    /// </summary>
    public partial class AgentsPage : Page
    {
        public DBcontext db;

        public AgentsPage()
        {
            InitializeComponent();
            //db = new DBcontext();
            //db.Agents.Load();
            FillDataGrid();
        }

        public void FillDataGrid()
        {
            db = new DBcontext();
            db.Agents.Load();
            var resultGrid = from Agent in db.Agents.Local.ToList()

                             select new
                             {
                                 agentFIO = Agent.full_name,
                                 dateBirth = Agent.birthday.ToShortDateString(),
                                 passportNumber = Agent.passport_series_number,
                                 passportDate = Agent.passport_date.ToShortDateString(),
                                 passportIssuedBy = Agent.passport_issued_by,
                                 homeAdress = Agent.address,
                                 phone = Agent.phone_number,
                                 inn = Agent.INN,
                                 IDAgent = Agent.agent_id,

                                 dateBirthToDispley = Agent.birthday.Date,
                                 datePassportToDispley = Agent.passport_date.Date,
                             };

            AgentsGrid.ItemsSource = resultGrid.ToList();
          
        }


        private void ButtonAddAgent_Click(object sender, RoutedEventArgs e)
        {
            AddAgentWindow taskWindow = new AddAgentWindow();
            taskWindow.Show();
            FillDataGrid();
        }

        private void ButtonDeliteAgent_Click(object sender, RoutedEventArgs e)
        {
            // если ни одного объекта не выделено, выходим
            if (AgentsGrid.SelectedItem == null)
            {
                CallPopup("Не выбран агент");
                return;
            }
            // получаем выделенный объект
            dynamic deleteAgent = AgentsGrid.SelectedItem;
            int IDAgent = deleteAgent.IDAgent;

            Agent c = db.Agents.Find(IDAgent);
            if (c != null)
            {
                db.Agents.Remove(c);
                db.SaveChanges();
            }
            FillDataGrid();
        }

        private void ButtonEditAgent_Click(object sender, RoutedEventArgs e)
        {
            // если ни одного объекта не выделено, выходим
            if (AgentsGrid.SelectedItem == null)
            {
                CallPopup("Не выбран агент");
                return;
            }

            // редактирование

            dynamic tempA = AgentsGrid.SelectedItem;
            Agent agent = new Agent();
            agent.agent_id = tempA.IDAgent;
            agent.address = tempA.homeAdress;
            agent.birthday = tempA.dateBirthToDispley;
            agent.full_name = tempA.agentFIO;
            agent.INN = tempA.inn;
            agent.passport_date = tempA.datePassportToDispley;
            agent.passport_issued_by = tempA.passportIssuedBy;
            agent.passport_series_number = tempA.passportNumber;
            agent.phone_number = tempA.phone;

            EditAgentWindow ed = new EditAgentWindow(agent);
            ed.ShowDialog();
            FillDataGrid();

        }

        public void CallPopup(string text)
        {
            popupMessage.Text = text;
            Storyboard s = (Storyboard)this.TryFindResource("ShowPopup");
            BeginStoryboard(s);
        }

        public void RefreshAgentsGrid()
        {
            db = new DBcontext();
            db.Agents.Load();
            AgentsGrid.ItemsSource = db.Agents.Local;
        }

        private void AgentsGrid_DC(object sender, MouseButtonEventArgs e)
        {
            dynamic proA = AgentsGrid.SelectedItem;
            Agent prAgent = new Agent();
            prAgent.agent_id = proA.IDAgent;
            prAgent.full_name = proA.agentFIO;
            prAgent.birthday = proA.dateBirthToDispley;
            prAgent.address = proA.homeAdress;
            prAgent.passport_date = proA.datePassportToDispley;
            prAgent.passport_issued_by = proA.passportIssuedBy;
            prAgent.passport_series_number = proA.passportNumber;
            prAgent.phone_number = proA.phone;
            prAgent.INN = proA.inn;

            AgentProfileWindow prof = new AgentProfileWindow(prAgent);
            prof.ShowDialog();
        }
    }
}
