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
    /// Логика взаимодействия для SaleTradePage.xaml
    /// </summary>
    public partial class SaleTradePage : Page
    {
        public DBcontext db;

        public SaleTradePage()
        {
            InitializeComponent();
            db = new DBcontext();
            db.Trades.Load();
            db.Clients.Load();
            db.ObjectEstates.Load();
            db.Agents.Load();
            db.Sales.Load();
            db.Rents.Load();
            FillDataGrid();
        }

        public void FillDataGrid()
        {
            db = new DBcontext();
            db.Trades.Load();

            var resultGrid = from trade in db.Trades.Local.ToList()
                             join s in db.Sales on trade.trade_id equals s.sale_id
                             join c in db.Clients on trade.buyer_id equals c.client_id
                             join a in db.Agents on trade.agent_id equals a.agent_id
                             join o in db.ObjectEstates on trade.object_id equals o.object_id
                             join sell in db.Clients on o.owner_id equals sell.client_id
                             

                             select new
                             {    
                                 date = trade.date.ToShortDateString(),
                                 dateToDispley = trade.date.Date,
                                 address = o.address,
                                 addressToDispley = o.object_id,
                                 price = s.price + " ₽",
                                 priceToDispley = s.price.ToString(),
                                 FIObuyer = c.full_name,
                                 FIObuyer_id = c.client_id,
                                 FIOseller = sell.full_name,
                                 FIOseller_id = sell.client_id,
                                 FIOagent = a.full_name,
                                 FIOagent_id = a.agent_id,

                                 IDSale = trade.trade_id,
                             };

                            
            SaleTradeGrid.ItemsSource = resultGrid.ToList();
        }

        public void CallPopup(string text)
        {
            popupMessage.Text = text;
            Storyboard s = (Storyboard)this.TryFindResource("ShowPopup");
            BeginStoryboard(s);
        }

        private void ButtonDeleteTrade_Click(object sender, RoutedEventArgs e)
        {
            // если ни одного объекта не выделено, выходим
            if (SaleTradeGrid.SelectedItem == null)
            {
                CallPopup("Не выбрана сделка");
                return;
            }

            // получаем выделенный объект
            dynamic deleteSale = SaleTradeGrid.SelectedItem;
            int IDSale = deleteSale.IDSale;

            Trade c = db.Trades.Find(IDSale);
            if (c != null)
            {
                db.Trades.Remove(c);
                db.SaveChanges();
            }
            FillDataGrid();
        }

        private void ButtonEditTrade_Click(object sender, RoutedEventArgs e)
        {
            // если ни одного объекта не выделено, выходим
            if (SaleTradeGrid.SelectedItem == null)
            {
                CallPopup("Не выбрана сделка");
                return;
            }

            // редактирование
            dynamic tempT = SaleTradeGrid.SelectedItem;
            Trade trade = new Trade();
            trade.trade_id = tempT.IDSale;
            trade.date = tempT.dateToDispley;
            trade.agent_id = tempT.FIOagent_id;
            trade.buyer_id = tempT.FIObuyer_id;
            trade.object_id = tempT.addressToDispley;

            EditTradeWindow edt = new EditTradeWindow(trade);
            edt.ShowDialog();
            FillDataGrid();
        }

        private void ButtonAddTrade_Click(object sender, RoutedEventArgs e)
        {
            AddTradeWindow taskWindow = new AddTradeWindow();
            taskWindow.Show();
            FillDataGrid();
        }

        private void ButtonFilter_Click(object sender, RoutedEventArgs e)
        {
            using (DBcontext db1 = new DBcontext())
            {
                db1.Agents.Load();
                db1.Clients.Load();
                db1.ObjectEstates.Load();
                db1.Sales.Load();
                db1.Trades.Load();

                var resultGrid = from t in db1.Trades.Local.ToList()
                                 join a in db1.Agents on t.agent_id equals a.agent_id
                                 join c in db1.Clients on t.buyer_id equals c.client_id
                                 join o in db1.ObjectEstates on t.object_id equals o.object_id
                                 join s in db1.Sales on t.trade_id equals s.sale_id
                                 join sell in db.Clients on o.owner_id equals sell.client_id

                                 where ( 
                                       (FromDP.Text == "" && ToDP.Text == "" || FromDP.Text == "1/1/0001" && ToDP.Text == "1/1/0001") ||
                                        (DateTime.Parse(ToDP.Text) >= DateTime.Parse(FromDP.Text) && t.date >= DateTime.Parse(FromDP.Text) && t.date <= DateTime.Parse(ToDP.Text))
                                        )

                                 select new
                                 {
                                     date = t.date.ToShortDateString(),
                                     address = o.address,
                                     price = s.price + " ₽",
                                     FIObuyer = c.full_name,
                                     FIOseller = sell.full_name,
                                     FIOagent = a.full_name,

                                     IDSale = t.trade_id,
                                 };

                SaleTradeGrid.ItemsSource = resultGrid.ToList();
            }
            //FromDP.Text = "";
            //ToDP.Text = "";
        }
    }
}
