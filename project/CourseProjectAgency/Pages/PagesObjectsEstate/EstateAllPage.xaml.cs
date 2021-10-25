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
    /// Логика взаимодействия для EstateAllPage.xaml
    /// </summary>
    public partial class EstateAllPage : Page
    {
        public DBcontext db;

        public EstateAllPage()
        {
            InitializeComponent();
            db = new DBcontext();
            db.ObjectEstates.Load();
            db.Clients.Load();
            db.Status.Load();
            db.Flats.Load();
            db.Houses.Load();
            FillDataGrid();
        }

        public void FillDataGrid()
        {
            db = new DBcontext();
            db.ObjectEstates.Load();

            var resultGrid = from ObjectE in db.ObjectEstates.Local.ToList()
                             join c in db.Clients on ObjectE.owner_id equals c.client_id
                             join s in db.Status on ObjectE.status_id equals s.status_id

                             select new
                             {
                                 Address = (db.Flats.Find(ObjectE.object_id) != null) ? ObjectE.address + ", кв. " + ObjectE.Flat.flat_number : ObjectE.address,
                                 AddressToDispley = ObjectE.address,
                                 Square = ObjectE.square.ToString() + " кв.м.",
                                 Price = ObjectE.price.ToString() + " ₽",
                                 FIOowner = c.full_name,
                                 FIOowner_id = c.client_id,
                                 Kind = (db.Houses.Find(ObjectE.object_id) != null) ? "Частный дом" : "Квартира",
                                 Status = s.name,
                                 Status_id = s.status_id,
                                 CadastralNumber = ObjectE.cadastral_number,

                                 PriceToDispley = ObjectE.price.ToString(),
                                 SquareToDispley = ObjectE.square.ToString(),
                         
                                 IDObject = ObjectE.object_id,                                 
                             };

            EstateObjectsGrid.ItemsSource = resultGrid.ToList();

            var resultGrid2 = from ObjectE in db.ObjectEstates.Local.ToList()
                             join c in db.Clients on ObjectE.owner_id equals c.client_id
                             join s in db.Status on ObjectE.status_id equals s.status_id
                             where s.status_id ==1 || s.status_id == 5

                              select new
                             {
                                 Address = (db.Flats.Find(ObjectE.object_id) != null) ? ObjectE.address + ", кв. " + ObjectE.Flat.flat_number : ObjectE.address,
                                 AddressToDispley = ObjectE.address,
                                 Square = ObjectE.square.ToString() + " кв.м.",
                                 Price = ObjectE.price.ToString() + " ₽",
                                 FIOowner = c.full_name,
                                 FIOowner_id = c.client_id,
                                 Kind = (db.Houses.Find(ObjectE.object_id) != null) ? "Частный дом" : "Квартира",
                                 Status = s.name,
                                 Status_id = s.status_id,
                                 CadastralNumber = ObjectE.cadastral_number,

                                 PriceToDispley = ObjectE.price.ToString(),
                                 SquareToDispley = ObjectE.square.ToString(),

                                 IDObject = ObjectE.object_id,
                             };

            EstateObjectsGrid2.ItemsSource = resultGrid2.ToList();
        }

        private void TBActive_Click(object sender, RoutedEventArgs e)
        {
            if (EstateObjectsGrid.Visibility == Visibility.Visible)
            {
                EstateObjectsGrid.Visibility = Visibility.Collapsed;
                EstateObjectsGrid2.Visibility = Visibility.Visible;
            }
            else
            {
                EstateObjectsGrid.Visibility = Visibility.Visible;
                EstateObjectsGrid2.Visibility = Visibility.Collapsed;
            }
        }

        public void CallPopup(string text)
        {
            popupMessage.Text = text;
            Storyboard s = (Storyboard)this.TryFindResource("ShowPopup");
            BeginStoryboard(s);
        }

        private void ButtonDeleteObject_Click(object sender, RoutedEventArgs e)
        {
            // если ни одного объекта не выделено, выходим
            if (EstateObjectsGrid.SelectedItem == null)
            {
                CallPopup("Не выбран объект");
                return;
            }

            // получаем выделенный объект
            dynamic deleteObject = EstateObjectsGrid.SelectedItem;           
            int IDObject = deleteObject.IDObject;
                        
            ObjectEstate c = db.ObjectEstates.Find(IDObject);

            if (c != null) 
            {
                db.ObjectEstates.Remove(c);
                db.SaveChanges();
            }
            FillDataGrid();

        }

        private void ButtonEditObject_Click(object sender, RoutedEventArgs e)
        {
            // если ни одного объекта не выделено, выходим
            if (EstateObjectsGrid.SelectedItem == null)
            {
                CallPopup("Не выбран объект");
                return;
            }

            // редактирование
            dynamic tempO = EstateObjectsGrid.SelectedItem;
            ObjectEstate objectestate = new ObjectEstate();
            objectestate.address = tempO.AddressToDispley;
            objectestate.cadastral_number = tempO.CadastralNumber;
            objectestate.object_id = tempO.IDObject;
            objectestate.owner_id = tempO.FIOowner_id;
            objectestate.price = int.Parse(tempO.PriceToDispley);
            objectestate.square = int.Parse(tempO.SquareToDispley);
            objectestate.status_id = tempO.Status_id;

            EditObjectEstateWindow edo = new EditObjectEstateWindow(objectestate);
            edo.ShowDialog();
            FillDataGrid();

        }

        private void ButtonAddObject_Click(object sender, RoutedEventArgs e)
        {
            AddObjectEstateWindow taskSh = new AddObjectEstateWindow();
            taskSh.Show();
            return;
        }
    }
}
