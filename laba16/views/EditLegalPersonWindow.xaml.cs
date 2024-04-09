using laba16.model;
using System;
using System.Collections.Generic;
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
using laba16.model;

namespace laba16.views
{
    /// <summary>
    /// Логика взаимодействия для EditLegalPersonWindow.xaml
    /// </summary>
    public partial class EditLegalPersonWindow : Window
    {
        
        public EditLegalPersonWindow(LegalacyPerson legalperson)
        {
            InitializeComponent();
            one.Text = legalperson.КодПоставщика.ToString();
            two.Text = legalperson.Название;
            three.Text = legalperson.НалоговыйНомер;
            four.Text = legalperson.НомерСвидетельстваНДС;
            
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new deliviryEntities())
            {
                int кодПоставщика = int.Parse(one.Text);

                var legalperson = context.ЮридическиеЛица.FirstOrDefault(c => c.КодПоставщика == кодПоставщика);

                if (legalperson != null)
                {

                    legalperson.Название = two.Text;
                    legalperson.НалоговыйНомер = three.Text;
                    legalperson.НомерСвидетельстваНДС = four.Text;

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
