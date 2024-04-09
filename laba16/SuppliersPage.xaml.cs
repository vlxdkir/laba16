using laba16.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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
using laba16.views;

namespace laba16
{
    /// <summary>
    /// Логика взаимодействия для SuppliersPage.xaml
    /// </summary>
    public partial class SuppliersPage : Page
    {
        List<Suppliers> SuppliersList;
        public SuppliersPage()
        {
            InitializeComponent();
            var context = new deliviryEntities();

           SuppliersList = context.Поставщики.Select(r => new Suppliers
           {
                КодПоставщика = r.КодПоставщика,
                Адрес = r.Адрес,
                Примечание = r.Примечание,
                
            }).ToList();

            SuppliersDataGrid.ItemsSource = SuppliersList;
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedSupplier = SuppliersDataGrid.SelectedItem as Suppliers;

           
            if (selectedSupplier != null)
            {
                
                MessageBox.Show($"Вы выбрали для редактирования поставщика с кодом {selectedSupplier.КодПоставщика}");
                EditSupplierWindow editWindow = new EditSupplierWindow(selectedSupplier);
                editWindow.Closed += EditWindow_Closed;
                editWindow.ShowDialog();
                
                
                
            }
            else
            {
                MessageBox.Show("Выберите поставщика для редактирования");
            }
        }

        
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

            Suppliers selectedSupplier = (Suppliers)SuppliersDataGrid.SelectedItem;

            if (selectedSupplier != null)
            {
                
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этого поставщика?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    
                    using (var context = new deliviryEntities())
                    {
                        
                        var supplierToRemove = context.Поставщики.FirstOrDefault(s => s.КодПоставщика == selectedSupplier.КодПоставщика);

                        if (supplierToRemove != null)
                        {
                            
                            context.Поставщики.Remove(supplierToRemove);
                            context.SaveChanges();
                            UpdateDataGrid();


                        }
                        else
                        {
                            MessageBox.Show("Поставщик не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите поставщика для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

            AddSupplierWindow addWindow = new AddSupplierWindow();
            addWindow.Closed += EditWindow_Closed;
            addWindow.ShowDialog();
        }

        

        private void EditWindow_Closed(object sender, EventArgs e)
        {
            UpdateDataGrid();
        }

        private void UpdateDataGrid()
        {
            var context = new deliviryEntities();

            SuppliersList = context.Поставщики.Select(r => new Suppliers
            {
                КодПоставщика = r.КодПоставщика,
                Адрес = r.Адрес,
                Примечание = r.Примечание,

            }).ToList();

            SuppliersDataGrid.ItemsSource = SuppliersList;
        }

    }
}

