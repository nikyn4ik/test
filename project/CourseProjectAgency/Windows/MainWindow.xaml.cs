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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool maxSize = false;

        public MainWindow()
        {           
            InitializeComponent();
            Frame.Content = new TradesPage();
            LabelHeader.Content = "Таблицы сделок";
        }

        private void MainWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            if (!maxSize)
            {
                this.WindowState = WindowState.Maximized;
                maxSize = true;
            }
            else
            {
                this.WindowState = WindowState.Normal;
                maxSize = false;
            }
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
        }

        private void ListViewItemTrades_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Frame.Content = new TradesPage();
            LabelHeader.Content = "Таблицы сделок";
        }

        private void ListViewItemClients_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Frame.Content = new ClientsPage();
            LabelHeader.Content = "Таблицы клиентов";
        }

        private void ListViewItemObjects_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Frame.Content = new ObjectsPage();
            LabelHeader.Content = "Таблицы объектов недвижимости";
        }

        private void ListViewItemAgents_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Frame.Content = new AgentsPage();
            LabelHeader.Content = "Таблица агентов";
        }

        

        private void ListViewItemSettings_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Frame.Content = new SettingsPage();
            LabelHeader.Content = "Справочники";
        }
    }
}
