using laba16.model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace laba16.views
{
    /// <summary>
    /// Логика взаимодействия для EditSupplierWindow.xaml
    /// </summary>
    public partial class EditSupplierWindow : Window
    {
        private Suppliers _supplier;

       
        public EditSupplierWindow(Suppliers supplier)
        {
            InitializeComponent();

            _supplier = supplier;

            Loaded += (sender, e) =>
            {
                if (_supplier != null)
                {
                    КодПоставщикаTextBlock.Text = _supplier.КодПоставщика.ToString();
                    АдресTextBlock.Text = _supplier.Адрес;
                    ПримечаниеTextBlock.Text = _supplier.Примечание;
                }
            };
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            int новыйКодПоставщика = int.Parse(КодПоставщикаTextBlock.Text);
            string новыйАдрес = АдресTextBlock.Text;
            string новоеПримечание = ПримечаниеTextBlock.Text;

            
            UpdateSupplier(новыйКодПоставщика, новыйАдрес, новоеПримечание);

            
            this.Close();
        }

        private void UpdateSupplier(int КодПоставщика, string Адрес, string Примечание)
        {
            using (var context = new deliviryEntities())
            {
                
                int кодПоставщика = int.Parse(КодПоставщикаTextBlock.Text);

                
                var supplier = context.Поставщики.FirstOrDefault(s => s.КодПоставщика == кодПоставщика);

                if (supplier != null)
                {
                    
                    supplier.Адрес = АдресTextBlock.Text;
                    supplier.Примечание = ПримечаниеTextBlock.Text;

                    
                    context.SaveChanges();

                    MessageBox.Show("Данные успешно обновлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Поставщик с указанным кодом не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}
