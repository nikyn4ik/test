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
    /// Логика взаимодействия для EditTradeWindow.xaml
    /// </summary>
    public partial class EditTradeWindow : Window
    {
        public Trade trade { get; private set; }
        public Sale sale { get; private set; }
        public Rent rent { get; private set; }
        public DBcontext db;

        public EditTradeWindow(Trade t)
        {
            db = new DBcontext();
           
            db.Clients.Load();
            db.Agents.Load();
            db.ObjectEstates.Load();
            db.Rents.Load();
            db.Sales.Load();
            trade = t;
            InitializeComponent();

            DateTrade.Text = t.date.ToShortDateString();

            AddressCB.ItemsSource = db.ObjectEstates.Local;
            AddressCB.SelectedItem = db.ObjectEstates.Find(trade.object_id);

            BuyerCB.ItemsSource = db.Clients.Local;
            BuyerCB.SelectedItem = db.Clients.Find(trade.buyer_id);

            AgentCB.ItemsSource = db.Agents.Local;
            AgentCB.SelectedItem = db.Agents.Find(trade.agent_id);

            if (db.Sales.Find(t.trade_id) != null)
            {
                sale = db.Sales.Find(t.trade_id);
                SaleGrid.Visibility = Visibility.Visible;
                RentGrid.Visibility = Visibility.Collapsed;

                Price.Text = sale.price.ToString();
            }
            else
            {
                rent = db.Rents.Find(t.trade_id);
                SaleGrid.Visibility = Visibility.Collapsed;
                RentGrid.Visibility = Visibility.Visible;

                DateStartRent.Text = rent.date_start.ToShortDateString();
                DateEndRent.Text = rent.date_end.ToShortDateString();
                PriceRent.Text = rent.rent_price.ToString();
            }
        }

        public void CallPopup(string text)
        {
            popupMessage.Text = text;
            Storyboard s = (Storyboard)this.TryFindResource("ShowPopup");
            BeginStoryboard(s);
        }

        private void EditTradeWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonEditTrade_Click(object sender, RoutedEventArgs e)
        {
            trade = db.Trades.Find(trade.trade_id);
            trade.date = (DateTime)DateTrade.SelectedDate;

            Agent a = (Agent)AgentCB.SelectedItem;
            trade.agent_id = a.agent_id;

            Client c = (Client)BuyerCB.SelectedItem;
            trade.buyer_id = c.client_id;

            ObjectEstate o = (ObjectEstate)AddressCB.SelectedItem;
            trade.object_id = o.object_id;

            db.Entry(trade).State = EntityState.Modified;

            if (db.Sales.Find(trade.trade_id) != null)
            {
                sale.sale_id = trade.trade_id;
                sale.price = int.Parse(Price.Text);

                db.Entry(sale).State = EntityState.Modified;
            }
            else
            {
                rent.rent_id = trade.trade_id;
                rent.date_start = (DateTime)DateStartRent.SelectedDate;
                rent.date_end = (DateTime)DateEndRent.SelectedDate;
                rent.rent_price = int.Parse(PriceRent.Text);

                db.Entry(rent).State = EntityState.Modified;
            }

            db.SaveChanges();
            this.Close();

        }
    }
}
