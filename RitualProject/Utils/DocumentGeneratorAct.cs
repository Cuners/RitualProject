using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RitualProject
{
    internal class DocumentGeneratorAct
    {
        double nds=0;
        double totalCostFact=0;
        double fullCost=0;
        public void GenerateDocument(ObservableCollection<ActDTO> acts, string name)
        {
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            Document doc = wordApp.Documents.Add();

            try
            {
                AddTitle(doc);
                AddDate(doc);
                AddPartiesInfo(doc, name);
                AddTable(doc, acts);
                AddSummary(doc);
                AddSignatures(doc);
                SaveDocument(doc);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании документа: " + ex.Message);
            }
            finally
            {
                try
                {
                    doc.Close();
                    wordApp.Quit();
                }
                catch(Exception ex) 
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void AddTitle(Document doc)
        {
            Paragraph title = doc.Paragraphs.Add();
            title.Range.Text = "Акт о приеме выполненных работ № ____";
            title.Range.Font.Size = 14;
            title.Range.Font.Bold = 1;
            title.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            title.Range.InsertParagraphAfter();
        }

        private void AddDate(Document doc)
        {
            Paragraph date = doc.Paragraphs.Add();
            date.Range.Text = "от " + DateTime.Now.Day.ToString() + " " + DateTime.Now.Month.ToString("MMMM") + " " + DateTime.Now.Year.ToString();
            date.Format.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
            date.Range.InsertParagraphAfter();
        }

        private void AddPartiesInfo(Document doc, string name)
        {
            Paragraph parties = doc.Paragraphs.Add();
            parties.Range.Text = "Исполнитель: ООО \"Ритуальник\"\nЗаказчик: " + name;
            parties.Range.InsertParagraphAfter();
        }

        private void AddTable(Document doc, ObservableCollection<ActDTO> acts)
        {
            Paragraph para = doc.Paragraphs.Add();
            Table table = doc.Tables.Add(para.Range, acts.Count+4, 6);
            table.Borders.Enable = 1;

            table.Cell(1, 1).Range.Text = "№";
            table.Cell(1, 2).Range.Text = "Наименование работы (услуги)";
            table.Cell(1, 3).Range.Text = "Ед. изм.";
            table.Cell(1, 4).Range.Text = "Количество";
            table.Cell(1, 5).Range.Text = "Цена";
            table.Cell(1, 6).Range.Text = "Сумма";
            int j = 0;
            double costFact = 0;
            foreach (ActDTO act in acts)
            {
                j++;
                table.Cell(j + 1, 1).Range.Text = j.ToString();
                table.Cell(j + 1, 2).Range.Text = act.Zakaz;
                table.Cell(j + 1, 3).Range.Text = "шт.";
                table.Cell(j + 1, 4).Range.Text = act.Quantity.ToString();
                costFact = (double)(act.Quantity * act.Price);
                table.Cell(j + 1, 5).Range.Text = act.Price.ToString();
                table.Cell(j + 1, 6).Range.Text =costFact.ToString();
                totalCostFact += costFact;
            }
            table.Cell(j + 2, 1).Range.Text = "Итого";
            table.Cell(j + 2, 6).Range.Text = totalCostFact.ToString();
            table.Cell(j + 3, 1).Range.Text = "НДС(18%)";
            nds = (totalCostFact / 100) * 18;
            fullCost = totalCostFact+nds;
            table.Cell(j + 3, 6).Range.Text = nds.ToString();
            table.Cell(j + 4, 1).Range.Text = "Всего";
            table.Cell(j + 4, 6).Range.Text = fullCost.ToString();
        }
        private void AddSummary(Document doc)
        {
            Paragraph summary = doc.Paragraphs.Add();
            summary.Range.Text = "Всего оказано услуг на сумму: " + fullCost.ToString() + " рублей" + "\nв т.ч. НДС - " + nds.ToString() + " рублей";
            summary.Range.InsertParagraphAfter();

            Paragraph conclusion = doc.Paragraphs.Add();
            conclusion.Range.Text = "Вышеперечисленные работы (услуги) выполнены полностью и в срок. Заказчик претензий по объему, качеству и срокам оказания услуг не имеет.";
            conclusion.Range.InsertParagraphAfter();
        }

        private void AddSignatures(Document doc)
        {
            Paragraph signatures = doc.Paragraphs.Add();
            signatures.Range.Text = "Исполнитель ______________________ Заказчик ______________________\nМ.П. М.П.";
            signatures.Range.InsertParagraphAfter();
        }

        private void SaveDocument(Document doc)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Выберите папку для сохранения файла";
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string directory = folderBrowserDialog.SelectedPath;
                string fileName = "Акт_выполненных_работ.docx";
                string filePath = Path.Combine(directory, fileName);
                int count = 1;
                while (File.Exists(filePath))
                {
                    fileName = $"Акт_выполненных_работ_{count}.docx";
                    filePath = Path.Combine(directory, fileName);
                    count++;
                }

                doc.SaveAs2(filePath);
            }
        }
    }
}
