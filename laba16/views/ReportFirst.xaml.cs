using Microsoft.Win32;
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
using Xceed.Words.NET;

namespace laba16.views
{
    /// <summary>
    /// Логика взаимодействия для ReportFirst.xaml
    /// </summary>
    public partial class ReportFirst : Page
    {
        public ReportFirst()
        {
            InitializeComponent();
        }

        private void btnCreateReport_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Word documents (*.docx)|*.docx";
            saveFileDialog.DefaultExt = "docx";
            saveFileDialog.AddExtension = true;
            saveFileDialog.FileName = $"Отчет_Договоры_{DateTime.Now.ToString("yyyy-mm-dd_HH-mm-ss")}.docx";

            if (saveFileDialog.ShowDialog() == true)
            {
                var filePath = saveFileDialog.FileName;
                using (var context = new deliviryEntities())
                {
                    var contracts = context.Договоры.ToList();
                    var postavlenoData = context.поставлено.ToList();

                    using (var doc = DocX.Create(filePath))
                    {
                        doc.InsertParagraph("Отчет о договорах")
                           .FontSize(15d)
                           .Bold()
                           .Alignment = Alignment.center;
                        doc.InsertParagraph($"Дата создания отчета: {DateTime.Now}").FontSize(12).Italic().Alignment = Alignment.center;
                        var table = doc.AddTable(contracts.Count() + 1, 4);

                        table.Rows[0].Cells[0].Paragraphs.First().Append("Номер");
                        table.Rows[0].Cells[1].Paragraphs.First().Append("Дата заключения");
                        table.Rows[0].Cells[2].Paragraphs.First().Append("Количество");
                        table.Rows[0].Cells[3].Paragraphs.First().Append("Сумма поставки");

                        int rowIndex = 1;
                        foreach (var contract in contracts)
                        {
                            var contractItems = postavlenoData.Where(p => p.НомерДоговора == contract.НомерДоговора).ToList();
                            var totalQuantity = contractItems.Sum(ci => ci.Колличество);
                            var totalAmount = contractItems.Sum(ci => ci.Колличество * ci.Цена);

                            table.Rows[rowIndex].Cells[0].Paragraphs.First().Append(contract.НомерДоговора.ToString());
                            table.Rows[rowIndex].Cells[1].Paragraphs.First().Append(contract.ДатаДоговора.ToString());
                            table.Rows[rowIndex].Cells[2].Paragraphs.First().Append(totalQuantity.ToString());
                            table.Rows[rowIndex].Cells[3].Paragraphs.First().Append(totalAmount.ToString("C2"));

                            rowIndex++;
                        }



                        doc.InsertTable(table);
                        doc.Save();
                    }
                }
            }

            MessageBox.Show($"Отчет был успешно создан.");
        }
    }
}
