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
    /// Логика взаимодействия для AddTradeWindow.xaml
    /// </summary>
    public partial class AddTradeWindow : Window
    {
        public DBcontext db;
        private Trade newTrade;
        private Sale newSale;
        private Rent newRent;
        public ObjectEstate ob;

        public AddTradeWindow()
        {
            InitializeComponent();
            db = new DBcontext();
            db.Clients.Load();
            db.ObjectEstates.Load();
            db.Agents.Load();
            db.Roles.Load();
            Fill_AddClientWindow();
        }             

        void Fill_AddClientWindow()
        {          
            var ad = from adr in db.ObjectEstates.Local.ToList()
                     join s in db.Status on adr.status_id equals s.status_id
                     where s.status_id == 1 || s.status_id == 5
                     select new
                     {
                         address = adr.address + ' ' + '(' + s.name + ')',
                         owner = adr.owner_id,
                         status = adr.status_id,
                         name = s.name,
                         id = adr.object_id,
                     };
            AddressCB.ItemsSource = ad.ToList();

            var buy = from b in db.Clients.Local.ToList()
                      join r in db.Roles on b.role_id equals r.role_id
                      where r.role_id == 2
                      select new
                      {
                         full_name = b.full_name,
                         id = b.client_id,
                      };
            BuyerCB.ItemsSource = buy.ToList();

            var ag = from a in db.Agents.Local.ToList()
                     select new
                     {
                         full_name = a.full_name,
                         id = a.agent_id,
                     };
            AgentCB.ItemsSource = ag.ToList();

            DateTrade.SelectedDate = DateTime.Now;          
        }

        

        public void CallPopup(string text)
        {
            popupMessage.Text = text;
            Storyboard s = (Storyboard)this.TryFindResource("ShowPopup");
            BeginStoryboard(s);
        }

        private void AddTradeWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TBSale_Checked(object sender, RoutedEventArgs e)
        {
            SaleGrid.Visibility = Visibility.Visible;
            RentGrid.Visibility = Visibility.Collapsed;
            TBRent.IsChecked = false;
        }

        private void TBRent_Checked(object sender, RoutedEventArgs e)
        {
            SaleGrid.Visibility = Visibility.Collapsed;
            RentGrid.Visibility = Visibility.Visible;
            TBSale.IsChecked = false;
        }

        private void ButtonAddObjectEstate_Click(object sender, RoutedEventArgs e)
        {
            newTrade = new Trade();
            dynamic b = BuyerCB.SelectedItem;
            dynamic a = AddressCB.SelectedItem;
            dynamic ag = AgentCB.SelectedItem;
            

            newTrade.date = (DateTime)DateTrade.SelectedDate;
            newTrade.object_id = a.id;
            newTrade.buyer_id = b.id;
            newTrade.agent_id = ag.id;

            db.Trades.Add(newTrade);

            if (TBRent.IsChecked == false)
            {
                newSale = new Sale();
                newSale.sale_id = newTrade.trade_id;
                newSale.price = int.Parse(Price.Text);
                db.Sales.Add(newSale);

                ob = db.ObjectEstates.Find(a.id);
                ob.status_id = 2;
                db.Entry(ob).State = EntityState.Modified;
            }
            else
            {
                newRent = new Rent();
                newRent.rent_id = newTrade.trade_id;
                newRent.date_start = (DateTime)DateStartRent.SelectedDate;
                newRent.date_end = (DateTime)DateEndRent.SelectedDate;
                newRent.rent_price = int.Parse(PriceRent.Text);
                db.Rents.Add(newRent);
                ob = db.ObjectEstates.Find(a.id);
                ob.status_id = 6;
                db.Entry(ob).State = EntityState.Modified;
            }
           
            db.SaveChanges();

            this.Close();
        }
    }
}
