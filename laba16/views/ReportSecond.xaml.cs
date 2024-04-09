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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Words.NET;

namespace laba16.views
{
    /// <summary>
    /// Логика взаимодействия для ReportSecond.xaml
    /// </summary>
    public partial class ReportSecond : Page
    {
        public ReportSecond()
        {
            InitializeComponent();
        }

        private void btnCreateReport_Click(object sender, RoutedEventArgs e)
        {
            int contractNumber;
            if (int.TryParse(txtNumberContract.Text, out contractNumber))
            {
                GenerateContractReport(contractNumber);
            }
            else
            {
                MessageBox.Show("Введите корректный номер договора.");
            }
        }

        private void btnCreateReportTwo_Click(object sender, RoutedEventArgs e)
        {
            if (StartDate != null && EndDate != null)
            {
                GenerateContractReportFromDateToDate(StartDate.SelectedDate, EndDate.SelectedDate);
            }
            else
            {
                MessageBox.Show("Выберете дату.");
            }
        }

        private void GenerateContractReport(int contractNumber)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Word documents (*.docx)|*.docx";
            saveFileDialog.DefaultExt = "docx";
            saveFileDialog.AddExtension = true;
            saveFileDialog.FileName = $"Отчет_Договоры_Больше_{contractNumber}_{DateTime.Now.ToString("yyyy-mm-dd_HH-mm-ss")}.docx";

            if (saveFileDialog.ShowDialog() == true)
            {
                var filePath = saveFileDialog.FileName;
                using (var context = new deliviryEntities())
                {
                    var contracts = context.Договоры.Where(d => d.НомерДоговора > contractNumber).ToList();
                    var postavlenoData = context.поставлено.ToList();

                    using (var doc = DocX.Create(filePath))
                    {
                        doc.InsertParagraph("Отчет о договорах с номером больше " + contractNumber)
                           .FontSize(15d)
                           .Bold()
                           .Alignment = Alignment.center;
                        doc.InsertParagraph($"Дата создания отчета: {DateTime.Now}").FontSize(12).Italic().Alignment = Alignment.center;
                        var table = doc.AddTable(contracts.Count() + 2, 4);

                        table.Rows[0].Cells[0].Paragraphs.First().Append("Номер");
                        table.Rows[0].Cells[1].Paragraphs.First().Append("Дата заключения");
                        table.Rows[0].Cells[2].Paragraphs.First().Append("Количество");
                        table.Rows[0].Cells[3].Paragraphs.First().Append("Сумма поставки");

                        int rowIndex = 1;
                        decimal sumQuantity = 0;
                        decimal sumAmount = 0;
                        foreach (var contract in contracts)
                        {
                            var contractItems = postavlenoData.Where(p => p.НомерДоговора == contract.НомерДоговора).ToList();
                            var totalQuantity = contractItems.Sum(ci => ci.Колличество);
                            var totalAmount = contractItems.Sum(ci => ci.Колличество * ci.Цена);
                            sumQuantity += totalQuantity;
                            sumAmount += totalAmount;
                            table.Rows[rowIndex].Cells[0].Paragraphs.First().Append(contract.НомерДоговора.ToString());
                            table.Rows[rowIndex].Cells[1].Paragraphs.First().Append(contract.ДатаДоговора.ToString());
                            table.Rows[rowIndex].Cells[2].Paragraphs.First().Append(totalQuantity.ToString());
                            table.Rows[rowIndex].Cells[3].Paragraphs.First().Append(totalAmount.ToString("C2"));

                            rowIndex++;
                        }
                        table.Rows[rowIndex].Cells[2].Paragraphs.First().Append(sumQuantity.ToString());
                        table.Rows[rowIndex].Cells[3].Paragraphs.First().Append(sumAmount.ToString("C2"));



                        doc.InsertTable(table);
                        doc.Save();
                    }
                }
            }

            MessageBox.Show($"Отчет был успешно создан.");
        }

        private void GenerateContractReportFromDateToDate(DateTime? start, DateTime? end)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Word documents (*.docx)|*.docx";
            saveFileDialog.DefaultExt = "docx";
            saveFileDialog.AddExtension = true;
            saveFileDialog.FileName = $"Отчет_Договоры_от_{start:dd.MM.yyyy}_до_{end:dd.MM.yyyy}_{DateTime.Now.ToString("yyyy-mm-dd_HH-mm-ss")}.docx";

            if (saveFileDialog.ShowDialog() == true)
            {
                var filePath = saveFileDialog.FileName;
                using (var context = new deliviryEntities())
                {
                    var contracts = context.Договоры.Where(d => d.ДатаДоговора >= start && d.ДатаДоговора <= end).ToList();
                    var postavlenoData = context.поставлено.ToList();

                    using (var doc = DocX.Create(filePath))
                    {
                        doc.InsertParagraph($"Отчет о договорах в периоде с {start:dd.MM.yyyy} по {end:dd.MM.yyyy}")
                           .FontSize(15d)
                           .Bold()
                           .Alignment = Alignment.center;
                        doc.InsertParagraph($"Дата создания отчета: {DateTime.Now}")
                            .FontSize(12)
                            .Italic()
                            .Alignment = Alignment.center;
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
