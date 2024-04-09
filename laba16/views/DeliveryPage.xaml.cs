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
using laba16.model;
using System.Threading;
using System.Diagnostics.Contracts;
using System.Runtime.Remoting.Contexts;

namespace laba16.views
{
    /// <summary>
    /// Логика взаимодействия для DeliveryPage.xaml
    /// </summary>
    public partial class DeliveryPage : Page
    {
        List<Contracts> ContractsList;
        List<Suppliers> SuppliersList;
        public DeliveryPage()
        {
            InitializeComponent();
            using (var context = new deliviryEntities())
            {
                var contractsList = (from contracts in context.Договоры
                                     join physicalPersons in context.ФизическоеЛицо
                                     on contracts.КодПоставщика equals physicalPersons.КодПоставщика into physicalPersonsGroup
                                     join legalPersons in context.ЮридическиеЛица
                                     on contracts.КодПоставщика equals legalPersons.КодПоставщика into legalPersonsGroup
                                     from physicalPerson in physicalPersonsGroup.DefaultIfEmpty()
                                     from legalPerson in legalPersonsGroup.DefaultIfEmpty()
                                     select new Contracts
                                     {
                                         НомерДоговора = contracts.НомерДоговора,
                                         ДатаДоговора = contracts.ДатаДоговора,
                                         КодПоставщика = contracts.КодПоставщика,
                                         ИмяПоставщика = legalPerson != null ? legalPerson.Название : (physicalPerson.Фамилия + " " + physicalPerson.Имя + " " + physicalPerson.Отчество),
                                         Комментарий = contracts.Коментарий
                                     }).ToList();

                DataGridConracts.ItemsSource = contractsList;
                DataGridDelivery.ItemsSource = context.поставлено.ToList();
            }
            DataGridConracts.SelectionChanged += DataGridContracts_SelectionChanged;
        }

        private void UpdateName()
        {
            using (var context = new deliviryEntities())
            {
                var contractsList = (from contracts in context.Договоры
                                     join physicalPersons in context.ФизическоеЛицо
                                     on contracts.КодПоставщика equals physicalPersons.КодПоставщика into physicalPersonsGroup
                                     join legalPersons in context.ЮридическиеЛица
                                     on contracts.КодПоставщика equals legalPersons.КодПоставщика into legalPersonsGroup
                                     from physicalPerson in physicalPersonsGroup.DefaultIfEmpty()
                                     from legalPerson in legalPersonsGroup.DefaultIfEmpty()
                                     select new Contracts
                                     {
                                         НомерДоговора = contracts.НомерДоговора,
                                         ДатаДоговора = contracts.ДатаДоговора,
                                         КодПоставщика = contracts.КодПоставщика,
                                         ИмяПоставщика = legalPerson != null ? legalPerson.Название : (physicalPerson.Фамилия + " " + physicalPerson.Имя + " " + physicalPerson.Отчество),
                                         Комментарий = contracts.Коментарий
                                     }).ToList();

                DataGridConracts.ItemsSource = contractsList;
                DataGridDelivery.ItemsSource = context.поставлено.ToList();
            }




        }
        private void BtnDeleteContract_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = DataGridConracts.SelectedItem as dynamic;

            if (selectedItem != null)
            {
                int номерДоговора = selectedItem.НомерДоговора;
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот элемент?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    using (var context = new deliviryEntities())
                    {
                        var itemToRemove = context.Договоры.FirstOrDefault(s => s.НомерДоговора == номерДоговора);

                        if (itemToRemove != null)
                        {
                            context.Договоры.Remove(itemToRemove);
                            context.SaveChanges();
                            UpdateDataGrid();
                            UpdateName();
                        }
                        else
                        {
                            MessageBox.Show("Элемент не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите элемент для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateDataGrid()
        {
            var context = new deliviryEntities();

            ContractsList = context.Договоры.Select(a => new Contracts
            {
                НомерДоговора = a.НомерДоговора,
                ДатаДоговора = a.ДатаДоговора,
                КодПоставщика = a.КодПоставщика,
                Комментарий = a.Коментарий,

            }).ToList();

            DataGridConracts.ItemsSource = ContractsList;
        }

        private void BtnEditContract_Click(object sender, EventArgs e)
        {
            using (var context = new deliviryEntities())
            {
                int НомерДоговора = int.Parse(one.Text);

                var contracts = context.Договоры.FirstOrDefault(c => c.НомерДоговора == НомерДоговора);

                if (contracts != null)
                {

                    contracts.ДатаДоговора = two.SelectedDate;
                    contracts.КодПоставщика = int.Parse(three.Text);
                    contracts.Коментарий = four.Text;
                    

                    context.SaveChanges();
                    UpdateName();
                    one.Clear();
                    two.SelectedDate = null;
                    three.Clear();
                    four.Clear() ;


                    MessageBox.Show("Данные успешно обновлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                }
                else
                {
                    MessageBox.Show("Элемент с указанным кодом не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnSaveContract_Click(object sender, EventArgs e)
        {
            using (var context = new deliviryEntities())
            {
                var contracts = new Договоры
                {
                    НомерДоговора = int.Parse(one.Text),
                    ДатаДоговора = two.SelectedDate,
                    КодПоставщика = int.Parse(three.Text),
                    Коментарий = four.Text,
                   
                };

                context.Договоры.Add(contracts);
                context.SaveChanges();
                UpdateDataGrid();
                UpdateName();

                one.Clear();
                two.SelectedDate = null;
                three.Clear();
                four.Clear();

                MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

 

        private void DataGridContracts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridConracts.SelectedItem != null)
            {
                var selectedContract = (Contracts)DataGridConracts.SelectedItem;
                int selectedContractNumber = selectedContract.НомерДоговора;

                using (var context = new deliviryEntities())
                {
                    var query = from item in context.поставлено
                                where item.НомерДоговора == selectedContractNumber
                                select new
                                {
                                    НомерДоговора = item.НомерДоговора,
                                    Товар = item.Товар,
                                    Колличество = item.Колличество,
                                    Цена = item.Цена
                                };

                    
                    var productList = query.ToList();

                    
                    decimal totalCount = productList.Sum(item => item.Колличество);

                    
                    decimal totalSum = productList.Sum(item => item.Цена * item.Колличество);

                    
                    SummaTextBox.Text = totalSum.ToString();
                    EdinicTextBox.Text = totalCount.ToString();

                    
                    DataGridDelivery.ItemsSource = productList;
                }
            }


        }

        private void BtnSaveDelivery_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new deliviryEntities())
            {
                var delivery = new поставлено
                {
                    НомерДоговора = int.Parse(one1.Text),
                    Товар = two1.Text,
                    Колличество = decimal.Parse(thee1.Text),
                    Цена = decimal.Parse(four1.Text),

                };

                context.поставлено.Add(delivery);
                context.SaveChanges();
                UpdateDataGrid();
                UpdateName();

                one1.Clear();
                two1.Clear();
                thee1.Clear();
                four1.Clear();

                MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnDeleteDelivery_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = DataGridDelivery.SelectedItem as dynamic;

            if (selectedItem != null)
            {
                int номерДоговора = selectedItem.НомерДоговора;
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот элемент?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    using (var context = new deliviryEntities())
                    {
                        var itemToRemove = context.поставлено.FirstOrDefault(s => s.НомерДоговора == номерДоговора);

                        if (itemToRemove != null)
                        {
                            context.поставлено.Remove(itemToRemove);
                            context.SaveChanges();
                            UpdateDataGrid();
                            UpdateName();
                        }
                        else
                        {
                            MessageBox.Show("Элемент не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите элемент для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
