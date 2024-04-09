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
using laba16.model;

namespace laba16.views
{
    /// <summary>
    /// Логика взаимодействия для EditPhysicalPersonWindow.xaml
    /// </summary>
    public partial class EditPhysicalPersonWindow : Window
    {
        public EditPhysicalPersonWindow(PhysicPerson physicperson)
        {
            InitializeComponent();
            one.Text = physicperson.КодПоставщика.ToString();
            two.Text = physicperson.Имя;
            three.Text = physicperson.Фамилия;
            four.Text = physicperson.Отчество;
            five.Text = physicperson.НомерИздательства;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new deliviryEntities())
            {
                int кодПоставщика = int.Parse(one.Text);

                var physicperson = context.ФизическоеЛицо.FirstOrDefault(c => c.КодПоставщика == кодПоставщика);

                if (physicperson != null)
                {

                    physicperson.Имя = two.Text;
                    physicperson.Фамилия = three.Text;
                    physicperson.Отчество = four.Text;
                    physicperson.НомерИздательства = five.Text;

                    context.SaveChanges();

                    MessageBox.Show("Данные успешно обновлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Элемент с указанным кодом не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
