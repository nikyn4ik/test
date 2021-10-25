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
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public DBcontext db;
        private Status newStatus;
        private Role newRole;
        public Status status { get; private set; }

        public SettingsPage()
        {
            InitializeComponent();
            FillDataGrid();
           
        }

        public void FillDataGrid()
        {
            db = new DBcontext();
            db.Status.Load();
            db.Roles.Load();

            var resultGrid = from s in db.Status.Local.ToList()
                             select new
                             {
                                 NameStatus = s.name,
                                 IDstatus = s.status_id,
                             };
            StatusGrid.ItemsSource = resultGrid.ToList();

            var resultGrid2 = from r in db.Roles.Local.ToList()
                              select new
                              {
                                 NameRole = r.name,
                                 IDrole = r.role_id,
                              };
            RoleGrid.ItemsSource = resultGrid2.ToList();
        }

        private void ButtonDeliteStatus_Click(object sender, RoutedEventArgs e)
        {
            // если ни одного объекта не выделено, выходим
            if (StatusGrid.SelectedItem == null)
            {
                return;
            }
            // получаем выделенный объект
            dynamic deleteStatus = StatusGrid.SelectedItem;
            int IDstatus = deleteStatus.IDstatus;

            Status c = db.Status.Find(IDstatus);
            if (c != null)
            {
                db.Status.Remove(c);
                db.SaveChanges();
            }
            FillDataGrid();
        }

        private void ButtonDeliteRole_Click(object sender, RoutedEventArgs e)
        {
            // если ни одного объекта не выделено, выходим
            if (RoleGrid.SelectedItem == null)
            {
                return;
            }
            // получаем выделенный объект
            dynamic deleteRole = RoleGrid.SelectedItem;
            int IDrole = deleteRole.IDrole;

            Role c = db.Roles.Find(IDrole);
            if (c != null)
            {
                db.Roles.Remove(c);
                db.SaveChanges();
            }
            FillDataGrid();
        }

        private void ButtonAddStatus_Click(object sender, RoutedEventArgs e)
        {
            GridAddStatus.Visibility = Visibility.Visible;

        }

        private void ButtonAddS_Click(object sender, RoutedEventArgs e)
        {
           
            newStatus = new Status();
            newStatus.name = StatusTB.Text;

            db.Status.Add(newStatus);
            db.SaveChanges();

            StatusTB.Clear();
            GridAddStatus.Visibility = Visibility.Collapsed;
        }

        private void ButtonAddRole_Click(object sender, RoutedEventArgs e)
        {
            GridAddRole.Visibility = Visibility.Visible;

        }

        private void ButtonAddR_Click(object sender, RoutedEventArgs e)
        {

            newRole = new Role();
            newRole.name = RoleTB.Text;

            db.Roles.Add(newRole);
            db.SaveChanges();

            RoleTB.Clear();
            GridAddRole.Visibility = Visibility.Collapsed;
        }

        private void ButtonEditStatus_Click(object sender, RoutedEventArgs e)
        {
            // если ни одного объекта не выделено, выходим
            if (StatusGrid.SelectedItem == null)
            {
                return;
            }

            GridEditStatus.Visibility = Visibility.Visible;
            GridAddStatus.Visibility = Visibility.Collapsed;
                       
            // редактирование
            dynamic tempS = StatusGrid.SelectedItem;
            Status status = new Status();

            status.status_id = tempS.IDstatus;
            status.name = tempS.NameStatus;

            StTB.Text = status.name;
            
        }        

        private void ButtonEditS_Click(object sender, RoutedEventArgs e)
        {
            dynamic tempS = StatusGrid.SelectedItem;
            Status status = new Status();

            status.status_id = tempS.IDstatus;
            status.name = tempS.NameStatus;

            FillEditS(status);
            GridEditStatus.Visibility = Visibility.Collapsed;
        }

        private void FillEditS(Status status)
        {
            status = db.Status.Find(status.status_id);
            status.name = StTB.Text;
            db.Entry(status).State = EntityState.Modified;
            db.SaveChanges();
            FillDataGrid();
        }

        private void ButtonEditRole_Click(object sender, RoutedEventArgs e)
        {
            // если ни одного объекта не выделено, выходим
            if (RoleGrid.SelectedItem == null)
            {
                return;
            }

            GridEditRole.Visibility = Visibility.Visible;
            GridAddRole.Visibility = Visibility.Collapsed;

            // редактирование
            dynamic tempR = RoleGrid.SelectedItem;
            Role role = new Role();

            role.role_id = tempR.IDrole;
            role.name = tempR.NameRole;

            RlTB.Text = role.name;

        }

        private void ButtonEditR_Click(object sender, RoutedEventArgs e)
        {
            dynamic tempR = RoleGrid.SelectedItem;
            Role role = new Role();

            role.role_id = tempR.IDrole;
            role.name = tempR.NameRole;

            FillEditR(role);
            GridEditRole.Visibility = Visibility.Collapsed;
        }

        private void FillEditR(Role role)
        {
            role = db.Roles.Find(role.role_id);
            role.name = RlTB.Text;
            db.Entry(role).State = EntityState.Modified;
            db.SaveChanges();
            FillDataGrid();
        }

    }
}
