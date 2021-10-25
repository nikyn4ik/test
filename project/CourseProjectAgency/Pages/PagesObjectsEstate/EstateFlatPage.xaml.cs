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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CourseProjectAgency
{
    /// <summary>
    /// Логика взаимодействия для EstateFlatPage.xaml
    /// </summary>
    public partial class EstateFlatPage : Page
    {
        public DBcontext db;

        public EstateFlatPage()
        {
            InitializeComponent();
            db = new DBcontext();
            db.Flats.Load();
            FillDataGrid();
        }

        public void FillDataGrid()
        {
            var resultGrid = from flat in db.Flats.Local.ToList()
                             join o in db.ObjectEstates on flat.flat_id equals o.object_id
                             join ow in db.Clients on o.owner_id equals ow.client_id
                             join s in db.Status on o.status_id equals s.status_id
                            

                             select new
                             {
                                 Address =  o.address + ", кв. " + flat.flat_number,
                                 Square = o.square + " кв.м.",
                                 Floor = flat.number_floor,
                                 QuantityRooms = flat.quantity_rooms,
                                 //Price = o.price + " ₽",
                                 Price = (s.status_id == 5 || s.status_id == 6) ? o.price + " ₽ в месяц" : o.price + " ₽",
                                 FIOowner = ow.full_name,
                                 Status = s.name,

                                 IDFlat = flat.flat_id,
                             };

            EstateFlatGrid.ItemsSource = resultGrid.ToList();

            var resultGrid2 = from flat in db.Flats.Local.ToList()
                             join o in db.ObjectEstates on flat.flat_id equals o.object_id
                             join ow in db.Clients on o.owner_id equals ow.client_id
                             join s in db.Status on o.status_id equals s.status_id
                             where s.status_id == 1 || s.status_id == 5

                             select new
                             {
                                 Address = o.address + ", кв. " + flat.flat_number,
                                 Square = o.square + " кв.м.",
                                 Floor = flat.number_floor,
                                 QuantityRooms = flat.quantity_rooms,
                                 //Price = o.price + " ₽",
                                 Price = (s.status_id == 5 || s.status_id == 6) ? o.price + " ₽ в месяц" : o.price + " ₽",
                                 FIOowner = ow.full_name,
                                 Status = s.name,

                                 IDFlat = flat.flat_id,
                             };

            EstateFlatGrid2.ItemsSource = resultGrid2.ToList();
        }

        private void TBActive_Click(object sender, RoutedEventArgs e)
        {
            if (EstateFlatGrid.Visibility == Visibility.Visible)
            {
                EstateFlatGrid.Visibility = Visibility.Collapsed;
                EstateFlatGrid2.Visibility = Visibility.Visible;
            }
            else
            {
                EstateFlatGrid.Visibility = Visibility.Visible;
                EstateFlatGrid2.Visibility = Visibility.Collapsed;
            }
        }
    }
}
