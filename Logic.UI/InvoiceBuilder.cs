using Microsoft.Win32;
using System;
using System.Diagnostics;
using Xceed.Words.NET;

namespace Logic.UI
{
    public class InvoiceBuilder
    {
        MainViewModel mainViewModel;
        ConfigHandler configHandler = new ConfigHandler();

        public bool build(MainViewModel _mainViewModel)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            mainViewModel = _mainViewModel;

            saveFileDialog.InitialDirectory = @"C:\Users\" + Environment.UserName + @"\Documents\";
            saveFileDialog.FileName = _mainViewModel.fileName;
            saveFileDialog.Filter = "Document files (*.docx)|*.docx|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 0;

            if (saveFileDialog.ShowDialog() == true)
            {
                if (!string.IsNullOrWhiteSpace(saveFileDialog.FileName))
                {
                    DocX doc = DocX.Create(saveFileDialog.FileName);

                    doc = addHeader(doc);
                    doc = addInvoiceText(doc);
                    doc = addMaterialTable(doc);
                    
                    doc.InsertParagraph(Environment.NewLine + Environment.NewLine + Environment.NewLine + mainViewModel.invoiceComment, false, getStandardFormat());
                    doc = addFooter(doc);
                    doc.Save();

                    Process.Start("WINWORD.EXE", saveFileDialog.FileName);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        private DocX addHeader(DocX _doc)
        {
            DocX doc = _doc;

            doc.InsertParagraph(getLogoString1(), false, getLogoFormat1()).Alignment = Alignment.right;
            doc.InsertParagraph(getLogoString2(), false, getLogoFormat2()).Alignment = Alignment.right;
            doc.InsertParagraph(getSmallProfileString(), false, getSmallFormat());
            doc = addHeaderTable(doc);

            return doc;
        }

        private DocX addInvoiceText(DocX _doc)
        {
            DocX doc = _doc;

            doc.InsertParagraph("Rechnung " + mainViewModel.invoiceNumber, false, getInvoiceNumberFormat());
            doc.InsertParagraph("Projekt: " + mainViewModel.customerCompanyName, false, getStandardFormat());
            doc.InsertParagraph("Kunden Nr.: " + mainViewModel.customerNumber, false, getStandardFormat());
            doc.InsertParagraph("", false, getStandardFormat());
            doc.InsertParagraph("Durchgeführte Arbeiten: " + mainViewModel.invoiceText, false, getStandardFormat());
            doc.InsertParagraph();
            doc.InsertParagraph();

            return doc;
        }

        private DocX addMaterialTable(DocX _doc)
        {
            DocX doc = _doc;
            var columnWidhts = new float[] { 230, 60, 85, 85 };
            Table materialTable = doc.AddTable(1, columnWidhts.Length);
            Border singleLineBorder = new Border(BorderStyle.Tcbs_single, BorderSize.one, 0, System.Drawing.Color.Black);
            Border doubleLineBorder = new Border(BorderStyle.Tcbs_double, BorderSize.one, 0, System.Drawing.Color.Black);
            decimal totalMaterialPrice = 0;

            ConfigHandler configHandler = new ConfigHandler();
            Font font = null;
            string fontName = configHandler.read(mainViewModel, "invoiceTableFont");

            if (!string.IsNullOrEmpty(fontName))
                font = new Font(fontName);
            else
                font = new Font("Times New Roman");

            materialTable.Alignment = Alignment.center;
            materialTable.SetWidths(columnWidhts);
            materialTable.Design = TableDesign.TableGrid;
            materialTable.AutoFit = AutoFit.ColumnWidth;
            materialTable = setBorder(materialTable);
            materialTable = setMaterialTableHeader(materialTable, font);

            Row row = materialTable.InsertRow();

            foreach (Material m in mainViewModel.MaterialList)
            {
                row = materialTable.InsertRow();

                row.Cells[0].Paragraphs[0].Append(m.Description);
                row.Cells[0].Paragraphs[0].Alignment = Alignment.left;
                row.Cells[0].Paragraphs[0].Font(font);

                row.Cells[1].Paragraphs[0].Append(m.Count.ToString() + "x");
                row.Cells[1].Paragraphs[0].Alignment = Alignment.center;
                row.Cells[1].Paragraphs[0].Font(font);

                row.Cells[2].Paragraphs[0].Append(m.Price.ToString() + " €");
                row.Cells[2].Paragraphs[0].Alignment = Alignment.right;
                row.Cells[2].Paragraphs[0].Font(font);

                row.Cells[3].Paragraphs[0].Append((m.Count * m.Price).ToString() + " €");
                row.Cells[3].Paragraphs[0].Alignment = Alignment.right;
                row.Cells[3].Paragraphs[0].Font(font);

                totalMaterialPrice += (m.Count * m.Price);
            }

            row = materialTable.InsertRow();

            if (mainViewModel.assistentIsEnabled)
            {
                row = materialTable.InsertRow();

                if (!mainViewModel.assistent2IsEnabled)
                    row.Cells[0].Paragraphs[0].Append("Hilfsarbeiter");
                else
                    row.Cells[0].Paragraphs[0].Append("Hilfsarbeiter 1");

                row.Cells[0].Paragraphs[0].Alignment = Alignment.left;
                row.Cells[0].Paragraphs[0].Font(font);

                row.Cells[1].Paragraphs[0].Append(mainViewModel.assistentBusinessHours.ToString() + " Std.");
                row.Cells[1].Paragraphs[0].Alignment = Alignment.center;
                row.Cells[1].Paragraphs[0].Font(font);

                row.Cells[2].Paragraphs[0].Append(mainViewModel.assistentWage.ToString() + " €");
                row.Cells[2].Paragraphs[0].Alignment = Alignment.right;
                row.Cells[2].Paragraphs[0].Font(font);

                row.Cells[3].Paragraphs[0].Append((mainViewModel.assistentBusinessHours * mainViewModel.assistentWage).ToString() + " €");
                row.Cells[3].Paragraphs[0].Alignment = Alignment.right;
                row.Cells[3].Paragraphs[0].Font(font);

                totalMaterialPrice += (mainViewModel.assistentBusinessHours * mainViewModel.assistentWage);
            }

            if (mainViewModel.assistent2IsEnabled)
            {
                row = materialTable.InsertRow();

                row.Cells[0].Paragraphs[0].Append("Hilfsarbeiter 2");
                row.Cells[0].Paragraphs[0].Alignment = Alignment.left;
                row.Cells[0].Paragraphs[0].Font(font);

                row.Cells[1].Paragraphs[0].Append(mainViewModel.assistent2BusinessHours.ToString() + " Std.");
                row.Cells[1].Paragraphs[0].Alignment = Alignment.center;
                row.Cells[1].Paragraphs[0].Font(font);

                row.Cells[2].Paragraphs[0].Append(mainViewModel.assistent2Wage.ToString() + " €");
                row.Cells[2].Paragraphs[0].Alignment = Alignment.right;
                row.Cells[2].Paragraphs[0].Font(font);

                row.Cells[3].Paragraphs[0].Append((mainViewModel.assistent2BusinessHours * mainViewModel.assistent2Wage).ToString() + " €");
                row.Cells[3].Paragraphs[0].Alignment = Alignment.right;
                row.Cells[3].Paragraphs[0].Font(font);

                totalMaterialPrice += (mainViewModel.assistent2BusinessHours * mainViewModel.assistent2Wage);
            }

            row = materialTable.InsertRow();

            row = materialTable.InsertRow();
            row.Cells[0].Paragraphs[0].Append("Arbeitsaufwand");
            row.Cells[0].SetBorder(TableCellBorderType.Bottom, singleLineBorder);
            row.Cells[0].Paragraphs[0].Alignment = Alignment.left;
            row.Cells[0].Paragraphs[0].Font(font);


            row.Cells[1].Paragraphs[0].Append(mainViewModel.businessHours.ToString() + " Std.");
            row.Cells[1].Paragraphs[0].Alignment = Alignment.center;
            row.Cells[1].Paragraphs[0].Font(font);
            row.Cells[1].SetBorder(TableCellBorderType.Bottom, singleLineBorder);

            row.Cells[2].Paragraphs[0].Append(mainViewModel.hourlyWage.ToString() + " €");
            row.Cells[2].Paragraphs[0].Alignment = Alignment.right;
            row.Cells[2].Paragraphs[0].Font(font);
            row.Cells[2].SetBorder(TableCellBorderType.Bottom, singleLineBorder);

            row.Cells[3].Paragraphs[0].Append((mainViewModel.businessHours * mainViewModel.hourlyWage).ToString() + " €");
            row.Cells[3].Paragraphs[0].Alignment = Alignment.right;
            row.Cells[3].Paragraphs[0].Font(font);
            row.Cells[3].SetBorder(TableCellBorderType.Bottom, singleLineBorder);

            totalMaterialPrice += (mainViewModel.businessHours * mainViewModel.hourlyWage);

            row = materialTable.InsertRow();

            row = materialTable.InsertRow();
            row.Cells[2].Paragraphs[0].Append("Gesamt");
            row.Cells[2].Paragraphs[0].Alignment = Alignment.right;
            row.Cells[2].Paragraphs[0].Font(font);

            row.Cells[3].Paragraphs[0].Append(totalMaterialPrice.ToString() + " €");
            row.Cells[3].Paragraphs[0].Alignment = Alignment.right;
            row.Cells[3].Paragraphs[0].Font(font);
            row.Cells[3].SetBorder(TableCellBorderType.Bottom, singleLineBorder);

            row = materialTable.InsertRow();
            row.Cells[2].Paragraphs[0].Append("19% MwSt.");
            row.Cells[2].Paragraphs[0].Alignment = Alignment.right;
            row.Cells[2].Paragraphs[0].Font(font);

            row.Cells[3].Paragraphs[0].Append(((totalMaterialPrice/100) * 19).ToString("#.##") + " €");
            row.Cells[3].Paragraphs[0].Alignment = Alignment.right;
            row.Cells[3].Paragraphs[0].Font(font);

            row = materialTable.InsertRow();
            row.Cells[3].Paragraphs[0].Append((totalMaterialPrice + ((totalMaterialPrice / 100) * 19)).ToString("#.##") + " €");
            row.Cells[3].Paragraphs[0].Alignment = Alignment.right;
            row.Cells[3].Paragraphs[0].Font(font);
            row.Cells[3].SetBorder(TableCellBorderType.Bottom, doubleLineBorder);

            doc.InsertTable(materialTable);

            return doc;
        }

        private DocX addHeaderTable(DocX _doc)
        {
            DocX doc = _doc;
            var columnWidths = new float[] { 280, 180 };
            Table headerTable = doc.AddTable(15, columnWidths.Length);
            Border lastCellBorder = new Border(BorderStyle.Tcbs_single, BorderSize.one, 0, System.Drawing.Color.Black);

            ConfigHandler configHandler = new ConfigHandler();
            Formatting format = getHeaderFormat();
            Font font = null;
            string fontName = configHandler.read(mainViewModel, "invoiceStandardFont");

            if (!string.IsNullOrEmpty(fontName))
                font = new Font(fontName);
            else
                font = new Font("Times New Roman");

            headerTable.Alignment = Alignment.center;
            headerTable.SetWidths(columnWidths);
            headerTable.SetWidths(columnWidths);
            headerTable.Design = TableDesign.TableGrid;
            headerTable.AutoFit = AutoFit.ColumnWidth;
            headerTable = setBorder(headerTable);

            headerTable.Rows[0].Cells[1].Paragraphs[0].Append(mainViewModel.profileFirstName + " " + mainViewModel.profileLastName, format);
            headerTable.Rows[1].Cells[0].Paragraphs[0].Append(mainViewModel.customerCompanyName, format);
            headerTable.Rows[1].Cells[1].Paragraphs[0].Append(mainViewModel.profileAddress + " " + mainViewModel.profileAddressNumber, format);
            headerTable.Rows[2].Cells[0].Paragraphs[0].Append(mainViewModel.customerAddress + " " + mainViewModel.customerAddressNumber, format);
            headerTable.Rows[2].Cells[1].Paragraphs[0].Append(mainViewModel.profilePostalCode + " " + mainViewModel.profileCityName, format);
            headerTable.Rows[3].Cells[0].Paragraphs[0].Append(mainViewModel.customerPostalCode + " " + mainViewModel.customerCityName, format);
            headerTable.Rows[3].Cells[1].Paragraphs[0].Append("Tel: " + mainViewModel.profileTelephoneNumber, format);
            headerTable.Rows[4].Cells[1].Paragraphs[0].Append("Mobil: " + mainViewModel.profileMobileNumber, format);
            headerTable.Rows[5].Cells[1].Paragraphs[0].Append("Fax: " + mainViewModel.profileFaxNumber, format);
            headerTable.Rows[6].Cells[1].Paragraphs[0].Append("E-Mail: " + mainViewModel.profileEMailAddress, format);

            headerTable.Rows[7].Cells[1].Paragraphs[0].Append("", format); //empty

            headerTable.Rows[8].Cells[1].Paragraphs[0].Append(mainViewModel.profileBankName, format);
            headerTable.Rows[9].Cells[1].Paragraphs[0].Append("Konto Nr.: " + mainViewModel.profileBankAccountNumber, format);
            headerTable.Rows[10].Cells[1].Paragraphs[0].Append("BLZ: " + mainViewModel.profileBankCodeNumber, format);
            headerTable.Rows[11].Cells[1].Paragraphs[0].Append("IBAN: " + mainViewModel.profileIBAN, format);
            headerTable.Rows[12].Cells[1].Paragraphs[0].Append("BIC: " + mainViewModel.profileBankCodeNumber, format);

            headerTable.Rows[13].Cells[1].Paragraphs[0].Append("", format); //empty

            headerTable.Rows[14].Cells[0].SetBorder(TableCellBorderType.Bottom, lastCellBorder);
            headerTable.Rows[14].Cells[1].Paragraphs[0].Append(mainViewModel.profileCityName + ", den " + mainViewModel.invoiceDate, format);
            headerTable.Rows[14].Cells[1].SetBorder(TableCellBorderType.Bottom, lastCellBorder);

            doc.InsertTable(headerTable);

            return doc;
        }

        private DocX addFooter(DocX _doc)
        {
            DocX doc = _doc;

            doc.AddFooters();
            doc.DifferentFirstPage = true;
            doc.Footers.First.InsertParagraph(mainViewModel.profileCompanyName + "   " + mainViewModel.profilePostalCode + " " + mainViewModel.profileAddress + " " 
                + mainViewModel.profileAddressNumber + "   Finanzamt Elmshorn Steuernummer: 130800907", false, getStandardFormat()).Alignment = Alignment.center;

            return doc;
        }

        private Formatting getLogoFormat1()
        {
            Formatting ret = new Formatting();
            string fontName = configHandler.read(mainViewModel, "invoiceStandardFont");

            if (!string.IsNullOrEmpty(fontName))
            {
                ret.FontFamily = new Font(fontName);
                ret.Size = 18;
                ret.Italic = true;
                ret.Bold = true;
            }

            return ret;
        }

        private Formatting getLogoFormat2()
        {
            Formatting ret = new Formatting();
            string fontName = configHandler.read(mainViewModel, "invoiceStandardFont");

            if (!string.IsNullOrEmpty(fontName))
            {
                ret.FontFamily = new Font(fontName);
                ret.Size = 14;
                ret.Italic = true;
                ret.Bold = true;
            }

            return ret;
        }

        private Formatting getSmallFormat()
        {
            Formatting ret = new Formatting();
            string fontName = configHandler.read(mainViewModel, "invoiceStandardFont");

            if (!string.IsNullOrEmpty(fontName))
            {
                ret.FontFamily = new Font(fontName);
                ret.Size = 8;
            }

            return ret;
        }

        private Formatting getHeaderFormat()
        {
            Formatting ret = new Formatting();
            string fontName = configHandler.read(mainViewModel, "invoiceStandardFont");

            if (!string.IsNullOrEmpty(fontName))
            {
                ret.FontFamily = new Font(fontName);
                ret.Size = 11;
            }

            return ret;
        }

        private Formatting getStandardFormat()
        {
            Formatting ret = new Formatting();
            string fontName = configHandler.read(mainViewModel, "invoiceStandardFont");

            if (!string.IsNullOrEmpty(fontName))
            {
                ret.FontFamily = new Font(fontName);
                ret.Size = 11;
            }

            return ret;
        }

        private Formatting getInvoiceNumberFormat()
        {
            Formatting ret = new Formatting();
            string fontName = configHandler.read(mainViewModel, "invoiceStandardFont");

            if (!string.IsNullOrEmpty(fontName))
            {
                ret.FontFamily = new Font(fontName);
                ret.Size = 16;
                ret.Bold = true;
            }

            return ret;
        }

        private string getLogoString1()
        {
            string ret;

            ret = "Hausservice Amaro";

            return ret;
        }

        private string getLogoString2()
        {
            string ret;

            ret = "... alles rund ums Haus" + Environment.NewLine;

            return ret;
        }

        private string getSmallProfileString()
        {
            string ret;

            ret = mainViewModel.profileCompanyName + "   " + mainViewModel.profileAddress + " " + mainViewModel.profileAddressNumber + " " +
                mainViewModel.profilePostalCode + " " + mainViewModel.profileCityName;

            return ret;
        }

        private Table setBorder(Table _table)
        {
            Table table = _table;

            Border border = new Border(BorderStyle.Tcbs_none, 0, 0, System.Drawing.Color.Transparent);
            table.SetBorder(TableBorderType.Bottom, border);
            table.SetBorder(TableBorderType.Top, border);
            table.SetBorder(TableBorderType.Left, border);
            table.SetBorder(TableBorderType.Right, border);
            table.SetBorder(TableBorderType.InsideH, border);
            table.SetBorder(TableBorderType.InsideV, border);

            return table;
        }

        private Table setMaterialTableHeader(Table _table, Font _font)
        {
            Table table = _table;
            Border border = new Border(BorderStyle.Tcbs_single, BorderSize.one, 0, System.Drawing.Color.Black);

            table.Rows[0].Cells[0].Paragraphs[0].Append("Materialkosten und Aufwendungen");
            table.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.left;
            table.Rows[0].Cells[0].Paragraphs[0].Font(_font);
            table.Rows[0].Cells[0].Paragraphs[0].Bold();
            table.Rows[0].Cells[0].Paragraphs[0].FontSize(11);
            table.Rows[0].Cells[0].TextDirection = TextDirection.right;
            table.Rows[0].Cells[0].SetBorder(TableCellBorderType.Bottom, border);

            table.Rows[0].Cells[1].Paragraphs[0].Append("Menge");
            table.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.center;
            table.Rows[0].Cells[1].Paragraphs[0].Font(_font);
            table.Rows[0].Cells[1].Paragraphs[0].Bold();
            table.Rows[0].Cells[1].Paragraphs[0].FontSize(11);
            table.Rows[0].Cells[1].TextDirection = TextDirection.right;
            table.Rows[0].Cells[1].SetBorder(TableCellBorderType.Bottom, border);

            table.Rows[0].Cells[2].Paragraphs[0].Append("Einzelpreis");
            table.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.right;
            table.Rows[0].Cells[2].Paragraphs[0].Font(_font);
            table.Rows[0].Cells[2].Paragraphs[0].Bold();
            table.Rows[0].Cells[2].Paragraphs[0].FontSize(11);
            table.Rows[0].Cells[2].TextDirection = TextDirection.right;
            table.Rows[0].Cells[2].SetBorder(TableCellBorderType.Bottom, border);

            table.Rows[0].Cells[3].Paragraphs[0].Append("Gesamtpreis");
            table.Rows[0].Cells[3].Paragraphs[0].Alignment = Alignment.right;
            table.Rows[0].Cells[3].Paragraphs[0].Font(_font);
            table.Rows[0].Cells[3].Paragraphs[0].Bold();
            table.Rows[0].Cells[3].Paragraphs[0].FontSize(11);
            table.Rows[0].Cells[3].TextDirection = TextDirection.right;
            table.Rows[0].Cells[3].SetBorder(TableCellBorderType.Bottom, border);

            return table;
        }
    }
}