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
    /// Логика взаимодействия для AddObjectEstateWindow.xaml
    /// </summary>
    public partial class AddObjectEstateWindow : Window
    {
        public DBcontext db;
        private ObjectEstate newObjectEstate;
        private Flat newFlat;
        private House newHouse;

        public AddObjectEstateWindow()
        {
            InitializeComponent();
            db = new DBcontext();
            db.Status.Load();
            db.Clients.Load();
            db.Roles.Load();
            Fill_AddObjectEstateWindow();
        }

       
        

        void Fill_AddObjectEstateWindow()
        {
            var st = from s in db.Status.Local.ToList()
                      where s.status_id == 1 || s.status_id == 5 
                      select new
                      {
                          name = s.name,
                          id = s.status_id,
                      };
            StatusCB.ItemsSource = st.ToList();
        
            var cl = from c in db.Clients.Local.ToList()
                     join r in db.Roles on c.role_id equals r.role_id
                     where r.role_id == 1 || r.role_id == 3
                      select new
                      {
                          full_name = c.full_name,
                          id = c.client_id,
                      };
            OwnerCB.ItemsSource = cl.ToList();
        }

        
        public void CallPopup(string text)
        {
            popupMessage.Text = text;
            Storyboard s = (Storyboard)this.TryFindResource("ShowPopup");
            BeginStoryboard(s);
        }

        private void AddObjectWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TBFlat_Checked(object sender, RoutedEventArgs e)
        {
            FlatGrid.Visibility = Visibility.Visible;
            HouseGrid.Visibility = Visibility.Collapsed;
            TBHouse.IsChecked = false;
        }

        private void TBHouse_Checked(object sender, RoutedEventArgs e)
        {
            FlatGrid.Visibility = Visibility.Collapsed;
            HouseGrid.Visibility = Visibility.Visible;
            TBFlat.IsChecked = false;
        }

        private void ButtonAddObjectEstate_Click(object sender, RoutedEventArgs e)
        {
            
            newObjectEstate = new ObjectEstate();
            dynamic st = StatusCB.SelectedItem;
            dynamic ow = OwnerCB.SelectedItem;

            newObjectEstate.address = AddressObject.Text;
            newObjectEstate.cadastral_number = CadastralNumber.Text;
            newObjectEstate.square = int.Parse(Square.Text);
            newObjectEstate.price = int.Parse(PriceObjectEstate.Text);
            newObjectEstate.status_id = st.id;
            newObjectEstate.owner_id = ow.id;

            db.ObjectEstates.Add(newObjectEstate);

            if (TBHouse.IsChecked == false)
            {
                newFlat = new Flat();
                newFlat.flat_id = newObjectEstate.object_id;
                newFlat.flat_number = int.Parse(NumberFlat.Text);
                newFlat.number_floor = int.Parse(NumberFloor.Text);
                newFlat.quantity_rooms = int.Parse(QuantityRooms.Text);
                db.Flats.Add(newFlat);
            }
            else
            {
                newHouse = new House();
                newHouse.house_id = newObjectEstate.object_id;
                newHouse.plot_size = int.Parse(PlotSize.Text);
                newHouse.quantity_floors = int.Parse(QuantityFloors.Text);
                db.Houses.Add(newHouse);
            }
            
            db.SaveChanges();

            this.Close();
        }
    }
}
