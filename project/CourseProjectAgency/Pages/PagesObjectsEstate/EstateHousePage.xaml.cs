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
    /// Логика взаимодействия для EstateHousePage.xaml
    /// </summary>
    public partial class EstateHousePage : Page
    {
        public DBcontext db;

        public EstateHousePage()
        {
            InitializeComponent();
            db = new DBcontext();
            db.Houses.Load();
            FillDataGrid();
        }

        public void FillDataGrid()
        {
            var resultGrid = from house in db.Houses.Local.ToList()
                             join o in db.ObjectEstates on house.house_id equals o.object_id
                             join ow in db.Clients on o.owner_id equals ow.client_id
                             join s in db.Status on o.status_id equals s.status_id


                             select new
                             {
                                 Address = o.address,
                                 Square = o.square + " кв.м.",
                                 PlotSize = house.plot_size + " кв.м.",
                                 QuantityFloors = house.quantity_floors,
                                 Price = (s.status_id == 5 || s.status_id == 6) ? o.price + " ₽ в месяц" : o.price + " ₽",
                                 FIOowner = ow.full_name,                               
                                 Status = s.name,

                                 IDHouse = house.house_id,
                             };

            EstateHouseGrid.ItemsSource = resultGrid.ToList();

            var resultGrid2 = from house in db.Houses.Local.ToList()
                             join o in db.ObjectEstates on house.house_id equals o.object_id
                             join ow in db.Clients on o.owner_id equals ow.client_id
                             join s in db.Status on o.status_id equals s.status_id
                             where s.status_id == 1 || s.status_id == 5

                             select new
                             {
                                 Address = o.address,
                                 Square = o.square + " кв.м.",
                                 PlotSize = house.plot_size + " кв.м.",
                                 QuantityFloors = house.quantity_floors,
                                 Price = (s.status_id == 5 || s.status_id == 6) ? o.price + " ₽ в месяц" : o.price + " ₽",
                                 FIOowner = ow.full_name,
                                 Status = s.name,

                                 IDHouse = house.house_id,
                             };

            EstateHouseGrid2.ItemsSource = resultGrid2.ToList();
        }

        private void TBActive_Click(object sender, RoutedEventArgs e)
        {
            if (EstateHouseGrid.Visibility == Visibility.Visible)
            {
                EstateHouseGrid.Visibility = Visibility.Collapsed;
                EstateHouseGrid2.Visibility = Visibility.Visible;
            }
            else
            {
                EstateHouseGrid.Visibility = Visibility.Visible;
                EstateHouseGrid2.Visibility = Visibility.Collapsed;
            }
        }
    }
}
