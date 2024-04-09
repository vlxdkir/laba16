using laba16.model;
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

namespace laba16.views
{
    /// <summary>
    /// Логика взаимодействия для PhysicalPerson.xaml
    /// </summary>
    public partial class PhysicalPerson : Page
    {
        public List<PhysicPerson> PhysicalPersonList;
        public List<Suppliers> SuppliersList { get; set; }
        public PhysicalPerson()
        {
            InitializeComponent();
            var context = new deliviryEntities();

            PhysicalPersonList = context.ФизическоеЛицо.Select(s => new PhysicPerson
            {
                КодПоставщика = s.КодПоставщика,
                Имя = s.Имя,
                Фамилия = s.Фамилия,
                Отчество = s.Отчество,
                НомерИздательства = s.НомерИздательства,




            }).ToList();

            PhysicPersonDataGrid.ItemsSource = PhysicalPersonList;



            SuppliersList = context.Поставщики.Select(r => new Suppliers
            {
                КодПоставщика = r.КодПоставщика,
                Адрес = r.Адрес,
                Примечание = r.Примечание,

            }).ToList();
            DataContext = this;

        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedPhysicalPerson = PhysicPersonDataGrid.SelectedItem as PhysicPerson;


            if (selectedPhysicalPerson != null)
            {

                MessageBox.Show($"Вы выбрали для редактирования Физ.Лицо с кодом {selectedPhysicalPerson.КодПоставщика}");
                EditPhysicalPersonWindow editWindow = new EditPhysicalPersonWindow(selectedPhysicalPerson);
                editWindow.Closed += EditWindow_Closed;
                editWindow.ShowDialog();



            }
            else
            {
                MessageBox.Show("Выберите Физ.Лицо для редактирования");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

            PhysicPerson selectedPhysicalPerson = PhysicPersonDataGrid.SelectedItem as PhysicPerson;

            if (selectedPhysicalPerson != null)
            {

                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить это физ.лицо?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {

                    using (var context = new deliviryEntities())
                    {

                        var PhysicalPersonToRemove = context.ФизическоеЛицо.FirstOrDefault(c => c.КодПоставщика == selectedPhysicalPerson.КодПоставщика);

                        if (PhysicalPersonToRemove != null)
                        {

                            context.ФизическоеЛицо.Remove(PhysicalPersonToRemove);
                            context.SaveChanges();
                            UpdateDataGrid();


                        }
                        else
                        {
                            MessageBox.Show("Физ.Лицо не найдено.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите Физ.Лицо для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

            AddPhysicalPersonWindow addWindow = new AddPhysicalPersonWindow();
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

            PhysicalPersonList = context.ФизическоеЛицо.Select(s => new PhysicPerson
            {
                КодПоставщика = s.КодПоставщика,
                Имя = s.Имя,
                Фамилия = s.Фамилия,
                Отчество = s.Отчество,
                НомерИздательства = s.НомерИздательства,




            }).ToList();

            PhysicPersonDataGrid.ItemsSource = PhysicalPersonList;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;

            // Проверяем, что ComboBox не равен null
            if (comboBox != null && comboBox.SelectedItem != null)
            {
                // Получаем выбранный поставщик из ComboBox
                Suppliers selectedSupplier = comboBox.SelectedItem as Suppliers;

                // Обновляем данные в базе данных
                using (var context = new deliviryEntities())
                {
                    // Находим поставщика в базе данных по его коду
                    var dbSupplier = context.Поставщики.FirstOrDefault(s => s.КодПоставщика == selectedSupplier.КодПоставщика);

                    if (dbSupplier != null)
                    {
                        // Обновляем код поставщика
                        dbSupplier.КодПоставщика = selectedSupplier.КодПоставщика;

                        // Сохраняем изменения в базе данных
                        context.SaveChanges();
                        UpdateDataGrid();
                    }
                }
            }
        }
    }
}
