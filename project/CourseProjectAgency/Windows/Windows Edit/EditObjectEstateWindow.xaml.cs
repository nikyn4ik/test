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
    /// Логика взаимодействия для EditObjectEstateWindow.xaml
    /// </summary>
    public partial class EditObjectEstateWindow : Window
    {
        public ObjectEstate objectestate { get; private set; }
        public Flat flat { get; private set; }
        public House house { get; private set; }
        public DBcontext db;

        public EditObjectEstateWindow(ObjectEstate o)
        {
            db = new DBcontext();
            db.Clients.Load();
            db.Status.Load();
            db.Flats.Load();
            db.Houses.Load();

            objectestate = o;
            InitializeComponent();

            AddressObject.Text = o.address;
            CadastralNumber.Text = o.cadastral_number;
            Square.Text = o.square.ToString();
            PriceObjectEstate.Text = o.price.ToString();

            OwnerCB.ItemsSource = db.Clients.Local;
            OwnerCB.SelectedItem = db.Clients.Find(objectestate.owner_id);

            StatusCB.ItemsSource = db.Status.Local;
            StatusCB.SelectedItem = db.Status.Find(objectestate.status_id);

            if (db.Flats.Find(o.object_id) != null)
            {
                flat = db.Flats.Find(o.object_id);
                FlatGrid.Visibility = Visibility.Visible;
                HouseGrid.Visibility = Visibility.Collapsed;

                NumberFlat.Text = flat.flat_number.ToString();
                NumberFloor.Text = flat.number_floor.ToString();
                QuantityRooms.Text = flat.quantity_rooms.ToString();
            }
            else
            {
                house = db.Houses.Find(o.object_id);
                FlatGrid.Visibility = Visibility.Collapsed;
                HouseGrid.Visibility = Visibility.Visible;

                PlotSize.Text = house.plot_size.ToString();
                QuantityFloors.Text = house.quantity_floors.ToString();
            }

        }

        public void CallPopup(string text)
        {
            popupMessage.Text = text;
            Storyboard s = (Storyboard)this.TryFindResource("ShowPopup");
            BeginStoryboard(s);
        }

        private void EditObjectWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonEditObject(object sender, RoutedEventArgs e)
        {
            objectestate = db.ObjectEstates.Find(objectestate.object_id);
            objectestate.address = AddressObject.Text;
            objectestate.cadastral_number = CadastralNumber.Text;
            objectestate.price = int.Parse(PriceObjectEstate.Text);
            objectestate.square = int.Parse(Square.Text);

            Client c = (Client)OwnerCB.SelectedItem;
            objectestate.owner_id = c.client_id;

            Status s = (Status)StatusCB.SelectedItem;
            objectestate.status_id = s.status_id;

            db.Entry(objectestate).State = EntityState.Modified;

            if (db.Flats.Find(objectestate.object_id) != null)
            {     
                flat.flat_number = int.Parse(NumberFlat.Text);
                flat.flat_id = objectestate.object_id;
                flat.number_floor = int.Parse(NumberFlat.Text);
                flat.quantity_rooms = int.Parse(QuantityRooms.Text);

                db.Entry(flat).State = EntityState.Modified;
            }
            else
            {
                house.house_id = objectestate.object_id;
                house.plot_size = int.Parse(PlotSize.Text);
                house.quantity_floors = int.Parse(QuantityFloors.Text);
                db.Entry(house).State = EntityState.Modified;
            }

            db.SaveChanges();
            this.Close();

        }
    }
}
