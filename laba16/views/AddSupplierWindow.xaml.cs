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
using System.Windows.Shapes;

namespace laba16.views
{
    /// <summary>
    /// Логика взаимодействия для AddSupplierWindow.xaml
    /// </summary>
    public partial class AddSupplierWindow : Window
    {
        public AddSupplierWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new deliviryEntities())
            {
                var supplier = new Поставщики
                {
                    КодПоставщика = int.Parse(КодПоставщикаTextBlock.Text),
                    Адрес = АдресTextBlock.Text,
                    Примечание = ПримечаниеTextBlock.Text
                };

                context.Поставщики.Add(supplier);
                context.SaveChanges();
                this.Close();
                //КодПоставщикаTextBlock.Clear();
                //АдресTextBlock.Clear();
                //ПримечаниеTextBlock.Clear();

                MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
