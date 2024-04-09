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
    /// Логика взаимодействия для AddLegalPersonWindow.xaml
    /// </summary>
    public partial class AddLegalPersonWindow : Window
    {
        public AddLegalPersonWindow()
        {
            InitializeComponent();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new deliviryEntities())
            {
                var legalperson = new ЮридическиеЛица
                {
                    КодПоставщика = int.Parse(one.Text),
                    Название = two.Text,
                    НалоговыйНомер = three.Text,
                    НомерСвидетельстваНДС = four.Text
                };

                context.ЮридическиеЛица.Add(legalperson);
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
