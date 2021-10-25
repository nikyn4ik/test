using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для ObjectsPage.xaml
    /// </summary>
    public partial class ObjectsPage : Page
    {
        public ObjectsPage()
        {
            InitializeComponent();
            FrameObjectsEstate.Content = new EstateAllPage();
        }

        private void ButtonEstateAll_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameObjectsEstate.Content = new EstateAllPage();
        }

        private void ButtonEstateFlat_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameObjectsEstate.Content = new EstateFlatPage();
        }

        private void ButtonEstateHouse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameObjectsEstate.Content = new EstateHousePage();
        }
    }
}
