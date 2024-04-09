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
    /// Логика взаимодействия для AddPhysicalPersonWindow.xaml
    /// </summary>
    public partial class AddPhysicalPersonWindow : Window
    {
        public AddPhysicalPersonWindow()
        {
            InitializeComponent();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new deliviryEntities())
            {
                var physicalperson = new ФизическоеЛицо
                {
                    КодПоставщика = int.Parse(one.Text),
                    Имя = two.Text,
                    Фамилия = three.Text,
                    Отчество = four.Text,
                    НомерИздательства = five.Text
                };

                context.ФизическоеЛицо.Add(physicalperson);
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
