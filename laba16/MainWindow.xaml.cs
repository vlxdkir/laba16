using laba16.views;
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
using laba16.model;

namespace laba16
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ShowSuppliersPage(object sender, RoutedEventArgs e)
        {
            // Создаем новый экземпляр страницы с DataGrid
            SuppliersPage suppliersPage = new SuppliersPage();

            // Устанавливаем эту страницу как содержимое Frame
            mainFrame.Content = suppliersPage;
        }

        private void ShowLegalPerson(object sender, RoutedEventArgs e)
        {
            LegalPerson legalPerson = new LegalPerson();
            mainFrame.Content = legalPerson;
        }

        private void ShowPhysicalPerson(object sender, RoutedEventArgs e)
        {
            PhysicalPerson physicalPerson = new PhysicalPerson();
            mainFrame.Content = physicalPerson;
        }

        private void ShowDeliveryPage(object sender, RoutedEventArgs e)
        {
            DeliveryPage deliveryPage = new DeliveryPage();
            mainFrame.Content = deliveryPage;
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            ReportFirst reportFirst = new ReportFirst();
            mainFrame.Content = reportFirst;
        }

        private void ContractReport_Click(object sender, RoutedEventArgs e)
        {
            ReportSecond reportSecond = new ReportSecond();
            mainFrame.Content = reportSecond;
        }

        
    }
}
