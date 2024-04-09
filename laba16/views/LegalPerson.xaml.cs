
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
using laba16.views;
using laba16.model;
using System.Collections.ObjectModel;


namespace laba16.views
{
    /// <summary>
    /// Логика взаимодействия для LegalPerson.xaml
    /// </summary>
    public partial class LegalPerson : Page
    {
        public List<LegalacyPerson> LegalPersonList;
        public List<Suppliers> SuppliersList { get; set; }
        public LegalPerson()
        {
            
            InitializeComponent();
            
            var context = new deliviryEntities();

            LegalPersonList = context.ЮридическиеЛица.Select(c => new LegalacyPerson
            {
                КодПоставщика = c.КодПоставщика,
                Название = c.Название,
                НалоговыйНомер = c.НалоговыйНомер,
                НомерСвидетельстваНДС = c.НомерСвидетельстваНДС,




            }).ToList();

            LegalPersonDataGrid.ItemsSource = LegalPersonList;



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
            var selectedLegalPerson = LegalPersonDataGrid.SelectedItem as LegalacyPerson;


            if (selectedLegalPerson != null)
            {

                MessageBox.Show($"Вы выбрали для редактирования Юр.Лицо с кодом {selectedLegalPerson.КодПоставщика}");
                EditLegalPersonWindow editWindow = new EditLegalPersonWindow(selectedLegalPerson);
                editWindow.Closed += EditWindow_Closed;
                editWindow.ShowDialog();



            }
            else
            {
                MessageBox.Show("Выберите Юр.Лицо для редактирования");
            }
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

            LegalacyPerson selectedLegalPerson = (LegalacyPerson)LegalPersonDataGrid.SelectedItem;

            if (selectedLegalPerson != null)
            {

                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этого поставщика?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {

                    using (var context = new deliviryEntities())
                    {

                        var LegalPersonToRemove = context.ЮридическиеЛица.FirstOrDefault(c => c.КодПоставщика == selectedLegalPerson.КодПоставщика);

                        if (LegalPersonToRemove != null)
                        {

                            context.ЮридическиеЛица.Remove(LegalPersonToRemove);
                            context.SaveChanges();
                            UpdateDataGrid();


                        }
                        else
                        {
                            MessageBox.Show("Юр.Лицо не найдено.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите Юр.Лицо для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

            AddLegalPersonWindow addWindow = new AddLegalPersonWindow();
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

            LegalPersonList = context.ЮридическиеЛица.Select(c => new LegalacyPerson
            {
                КодПоставщика = c.КодПоставщика,
                Название = c.Название,
                НалоговыйНомер = c.НалоговыйНомер,
                НомерСвидетельстваНДС = c.НомерСвидетельстваНДС,

            }).ToList();

            LegalPersonDataGrid.ItemsSource = LegalPersonList;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            
            if (e.AddedItems.Count > 0 && sender is ComboBox comboBox)
            {
                var selectedSupplier = comboBox.SelectedItem as Поставщики;
                var legalPerson = comboBox.DataContext as LegalacyPerson;

                if (selectedSupplier != null && legalPerson != null)
                {
                    UpdateSupplierForLegalPerson(legalPerson, selectedSupplier.КодПоставщика);
                }
            }


        }
        

        

        private void UpdateSupplierForLegalPerson(LegalacyPerson legalPerson, int newSupplierId)
        {
            using (var context = new deliviryEntities())
            {
                var newLegalPerson = new ЮридическиеЛица
                {
                    КодПоставщика = newSupplierId,
                    Название = legalPerson.Название,
                    НалоговыйНомер = legalPerson.НалоговыйНомер,
                    НомерСвидетельстваНДС = legalPerson.НомерСвидетельстваНДС
                };

                context.ЮридическиеЛица.Add(newLegalPerson);

                var entityToRemove = context.ЮридическиеЛица.FirstOrDefault(lp => lp.КодПоставщика == legalPerson.КодПоставщика);
                if (entityToRemove != null)
                {
                    context.ЮридическиеЛица.Remove(entityToRemove);
                }

                context.SaveChanges();
            }
        }
    }
}
