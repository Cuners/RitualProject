using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using System.Windows.Forms;

namespace RitualProject
{
    internal class DocumentGeneratorInvent
    {
        public void GenerateDocument(ObservableCollection<InventarizationDTO> inventarization, string Name)
        {
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            Document doc = wordApp.Documents.Add();

            try
            {
                AddTitle(doc);
                AddDate(doc);
                AddCompanyInfo(doc, Name);
                AddInventoryInfo(doc,inventarization);
                SaveDocument(doc);
            }
            catch
            {

            }
            finally
            {
                doc.Close();
                wordApp.Quit();
            }
        }

        private void AddTitle(Document doc)
        {
            Paragraph title = doc.Paragraphs.Add();
            title.Range.Text = "Отчет об инвентаризации";
            title.Range.Font.Size = 20;
            title.Range.Font.Bold = 1;
            title.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            title.Range.InsertParagraphAfter();
        }

        private void AddDate(Document doc)
        {
            Paragraph date = doc.Paragraphs.Add();
            date.Range.Text = "Дата: " + DateTime.Now.ToShortDateString();
            date.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
            date.Range.InsertParagraphAfter();
        }

        private void AddCompanyInfo(Document doc, string name)
        {
            Paragraph companyInfoTitle = doc.Paragraphs.Add();
            companyInfoTitle.Range.Text = "Информация о компании:";
            companyInfoTitle.Range.Font.Size = 14;
            companyInfoTitle.Range.Font.Bold = 1;
            companyInfoTitle.Range.InsertParagraphAfter();

            Paragraph companyInfo = doc.Paragraphs.Add();
            companyInfo.Range.Text = "Название компании: Ритуал\n" +
                                     "Адрес: ул.Декабистов 55\n" +
                                     "Телефон: +8 927 325 49 50]\n" +
                                     "E-mail: ads@mail.ru\n" +
                                     "Ответственное лицо за отчет: " 
                                     + name +"\n";
            companyInfo.Range.InsertParagraphAfter();
        }

        private void AddInventoryInfo(Document doc, ObservableCollection<InventarizationDTO> inventarization)
        {
            Paragraph inventoryInfoTitle = doc.Paragraphs.Add();
            inventoryInfoTitle.Range.Text = "Информация об инвентаре:";
            inventoryInfoTitle.Range.Font.Size = 14;
            inventoryInfoTitle.Range.Font.Bold = 1;
            inventoryInfoTitle.Range.InsertParagraphAfter();

            // Создаем таблицу после добавления заголовка
            Table inventoryTable = doc.Tables.Add(inventoryInfoTitle.Range, inventarization.Count + 2, 6);
            inventoryTable.Borders.Enable = 1;
            inventoryTable.Cell(1, 1).Range.Text = "№";
            inventoryTable.Cell(1, 2).Range.Text = "Наименование";
            inventoryTable.Cell(1, 3).Range.Text = "Количество";
            inventoryTable.Cell(1, 4).Range.Text = "Кол-во\n" +
                "по учету";
            inventoryTable.Cell(1, 5).Range.Text = "Сумма";
            inventoryTable.Cell(1, 6).Range.Text = "Сумма\n" +
                "по учету";

            int j = 0;
            double totalCost = 0;
            double totalCostFact = 0;
            double CostFact = 0;
            foreach (InventarizationDTO invent in inventarization)
            {
                j++;
                inventoryTable.Cell(j + 1, 1).Range.Text = invent.CompositionID.ToString();
                inventoryTable.Cell(j + 1, 2).Range.Text = invent.Product;
                inventoryTable.Cell(j + 1, 3).Range.Text = invent.Quantity.ToString();
                inventoryTable.Cell(j + 1, 4).Range.Text = invent.QuantityFact.ToString();
                CostFact = invent.QuantityFact * invent.Prices;
                inventoryTable.Cell(j + 1, 5).Range.Text = invent.Price.ToString();
                inventoryTable.Cell(j + 1, 6).Range.Text = CostFact.ToString();
                totalCost += invent.Price;
                totalCostFact += CostFact;
            }
            
            // Добавляем строку с общей стоимостью после цикла
            int rowCount = inventoryTable.Rows.Count;
            inventoryTable.Cell(rowCount+1, 1).Range.Text = "Итого";
            inventoryTable.Cell(rowCount+1, 5).Range.Text = totalCost.ToString();
            inventoryTable.Cell(rowCount+1, 6).Range.Text = totalCostFact.ToString();
            double total=totalCost-totalCostFact;
            // Добавляем строки для подписей
            doc.Paragraphs.Add();
            if (totalCost > 0)
            {
                Paragraph inventLine = doc.Paragraphs.Add();
                inventLine.Range.Text = "Недостатки на: " + total;
                inventLine.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                inventLine.Range.InsertParagraphAfter();
            }
            else if (totalCost < 0)
            {
                Paragraph inventLine = doc.Paragraphs.Add();
                inventLine.Range.Text = "Избытки на: " + total;
                inventLine.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                inventLine.Range.InsertParagraphAfter();
            }
            Paragraph signatureLine = doc.Paragraphs.Add();
            signatureLine.Range.Text = "Отправил: ____________________________";
            signatureLine.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
            signatureLine.Range.InsertParagraphAfter();
            Paragraph signatureLine2 = doc.Paragraphs.Add();
            signatureLine2.Range.Text = "Получил: ____________________________";
            signatureLine2.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
            signatureLine2.Range.InsertParagraphAfter();
        }

        private void SaveDocument(Document doc)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Выберите папку для сохранения файла";

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string directory = folderBrowserDialog.SelectedPath;
                string fileName = "Отчет_об_инвентаризации.docx";
                string filePath = Path.Combine(directory, fileName);

                // Дальше код сохранения файла остается без изменений

                int count = 1;
                while (File.Exists(filePath))
                {
                    fileName = $"Отчет_об_инвентаризации_{count}.docx";
                    filePath = Path.Combine(directory, fileName);
                    count++;
                }

                doc.SaveAs2(filePath);
               
            }
        }
    }
}
