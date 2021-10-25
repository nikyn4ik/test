using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CourseProjectAgency
{
    /// <summary>
    /// Логика взаимодействия для AgentProfileWindow.xaml
    /// </summary>
    public partial class AgentProfileWindow : Window
    {
        public Agent agent { get; private set; }
        public DBcontext db;

        public AgentProfileWindow(Agent a)
        {
            InitializeComponent();

            db = new DBcontext();
            agent = a;

            FIO.Text = a.full_name;
            Birthday.Text = a.birthday.ToShortDateString();
            HomeAddress.Text = a.address;
            PassportDate.Text = a.passport_date.ToShortDateString();
            PassportIssuedBy.Text = a.passport_issued_by;
            PassportSeriesNumber.Text = a.passport_series_number;
            Phone.Text = a.phone_number;
            INN.Text = a.INN;
            

            db.Sales.Load();
            db.Rents.Load();
            db.ObjectEstates.Load();
            db.Agents.Load();
            db.Trades.Load();
            FillDataGrid(a);
            

            Kol.Text = AgentTrade.Items.Count.ToString();
            Kol2.Text = AgentTrade2.Items.Count.ToString();
        }

        
        private void FillDataGrid(Agent a)
        {
            agent = a;

            var res2 = from Agent in db.Agents.Local.ToList()
                       join t in db.Trades on Agent.agent_id equals t.agent_id
                       join o in db.ObjectEstates on t.object_id equals o.object_id
                       where Agent.agent_id == a.agent_id && o.status_id == 2

                       select new
                       {
                           Date = t.date.ToShortDateString(),
                           Object = o.address,
                           Price = t.Sale.price,

                       };

            AgentTrade.ItemsSource = res2.ToList();

            var resRent = from Agent in db.Agents.Local.ToList()
                       join t in db.Trades on Agent.agent_id equals t.agent_id
                       join o in db.ObjectEstates on t.object_id equals o.object_id
                       where Agent.agent_id == a.agent_id && o.status_id == 6

                       select new
                       {
                           Date = t.date.ToShortDateString(),
                           Object = o.address,
                           Price = t.Rent.rent_price,

                       };

            AgentTrade2.ItemsSource = resRent.ToList();

        }

        private void AgentProfileWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void AgentProfile_Back(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void GoToTrades_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            GoToTrades.Visibility = Visibility.Collapsed;
            GoToInfo.Visibility = Visibility.Visible;
            GoToTradesRent.Visibility = Visibility.Visible;
            BackToTrades.Visibility = Visibility.Collapsed;

            SaleGrid.Visibility = Visibility.Visible;
            InfoGrid.Visibility = Visibility.Collapsed;
            RentGrid.Visibility = Visibility.Collapsed;
        }

        private void GoToInfo_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GoToTrades.Visibility = Visibility.Visible;
            GoToInfo.Visibility = Visibility.Collapsed;
            GoToTradesRent.Visibility = Visibility.Collapsed;
            BackToTrades.Visibility = Visibility.Collapsed;

            SaleGrid.Visibility = Visibility.Collapsed;
            InfoGrid.Visibility = Visibility.Visible;
            RentGrid.Visibility = Visibility.Collapsed;
        }

        private void GoToTradesRent_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GoToTrades.Visibility = Visibility.Collapsed;
            GoToInfo.Visibility = Visibility.Collapsed;
            GoToTradesRent.Visibility = Visibility.Collapsed;
            BackToTrades.Visibility = Visibility.Visible;

            SaleGrid.Visibility = Visibility.Collapsed;
            InfoGrid.Visibility = Visibility.Collapsed;
            RentGrid.Visibility = Visibility.Visible;
        }

        private void BackToTrades_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GoToTrades.Visibility = Visibility.Collapsed;
            GoToInfo.Visibility = Visibility.Visible;
            GoToTradesRent.Visibility = Visibility.Visible;
            BackToTrades.Visibility = Visibility.Collapsed;

            SaleGrid.Visibility = Visibility.Visible;
            InfoGrid.Visibility = Visibility.Collapsed;
            RentGrid.Visibility = Visibility.Collapsed;
        }
    }
}
