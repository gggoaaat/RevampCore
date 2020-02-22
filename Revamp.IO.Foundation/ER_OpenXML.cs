using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using System.Data;

namespace Revamp.IO.Foundation
{
    public class ER_OpenXML
    {
        private Cell CreateContentCell(string header, UInt32 index, string text, CellValues contentType)
        {
            Cell cell = new Cell
            {
                DataType = contentType,
                CellReference = header + index
            };

            if (contentType != CellValues.Number)
            {
                InlineString istring = new InlineString();
                Text t = new Text { Text = text };

                istring.AppendChild(t);
                cell.AppendChild(istring);
            }
            else
            {
                cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(text);
            }

            return cell;
        }

        public static byte[] _GenerateExcel(List<System.Data.DataTable> _DataSet, string Classification)
        {
            ER_OpenXML ox = new ER_OpenXML();

            return ox.GenerateExcel(_DataSet, Classification);
        }

        public byte[] GenerateExcel(List<System.Data.DataTable> _DataSet, string Classification)
        {
            var stream = new MemoryStream();
            SpreadsheetDocument document = SpreadsheetDocument.Create(stream, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook);

            WorkbookPart wbp = document.AddWorkbookPart();
            wbp.Workbook = new Workbook();

            Sheets sheets = new Sheets();

            FileVersion fv = new FileVersion();
            fv.ApplicationName = "Microsoft Office Excel";

            UInt32 TotalSheets = 1;

            WorksheetPart[] wsp = new WorksheetPart[TotalSheets];
            //Worksheet[] ws = new Worksheet[TotalSheets];
            SheetData[] sd = new SheetData[TotalSheets];
            Sheet[] sheet = new Sheet[TotalSheets];
            Columns thisColumn = new Columns();

            for (int i = 0; i < TotalSheets; i++)
            {
                wsp[i] = wbp.AddNewPart<WorksheetPart>();

                sd[i] = new SheetData();

                wsp[i].Worksheet = new Worksheet();
                wsp[i].Worksheet.Append(thisColumn);
                wsp[i].Worksheet.Append(sd[i]);

                sheet[i] = new Sheet();
            }

            WorkbookStylesPart wbsp = wbp.AddNewPart<WorkbookStylesPart>();
            wbsp.Stylesheet = CreateStylesheet();
            wbsp.Stylesheet.Save();

            _GetExcelInfo _ge = new _GetExcelInfo();

            UInt32 HeaderRow = 1;

            for (int i = 0; i < TotalSheets; i++)
            {
                DataColumnCollection _dccColumnID = _DataSet[i].Columns;

                if (_dccColumnID.Contains("rownumb"))
                {
                    _DataSet[i].Columns.Remove("rownumb");
                }

                if (_dccColumnID.Contains("SSN"))
                {
                    _DataSet[i].Columns.Remove("SSN");
                }

                if (_dccColumnID.Contains("DOC_MAP")) //Catch all to rename this column whenever present.
                {
                    _DataSet[i].Columns["DOC_MAP"].ColumnName = "ASG";
                }

                CreateColumnHeader(_DataSet[i], sd[i], _ge, HeaderRow);
                CreateHeaderFooter(Classification, wsp, sd, i);
                CreateContent(_DataSet[i], sd[i], HeaderRow, _ge);
                AutoSizeColumns(wsp, sd, thisColumn, i);
            }

            for (UInt32 i = 0; i < TotalSheets; i++)
            {
                //wsp[i].Worksheet.Append(sd[i]);
                wsp[i].Worksheet.Save();

                sheet[i].SheetId = i + 1;
                sheet[i].Name = "Sheet " + (i + 1);
                sheet[i].Id = wbp.GetIdOfPart(wsp[i]);
                sheets.Append(sheet[i]);
            }

            wbp.Workbook.Append(fv);
            wbp.Workbook.Append(sheets);

            document.WorkbookPart.Workbook.Save();
            document.Close();

            return stream.ToArray();
        }

        private static void AutoSizeColumns(WorksheetPart[] wsp, SheetData[] sd, Columns thisColumn, int i)
        {
            for (int eachExcelColumn = 0; eachExcelColumn < sd[i].ChildElements[0].ChildElements.Count(); eachExcelColumn++)
            {
                int thisColumnMaxLength = 0;

                for (int eachExcelRow = 0; eachExcelRow < sd[i].ChildElements.Count(); eachExcelRow++)
                {
                    int currentLength = sd[i].ChildElements[eachExcelRow].ChildElements[eachExcelColumn].InnerText.ToString().Length;
                    thisColumnMaxLength = currentLength > thisColumnMaxLength ? currentLength : thisColumnMaxLength;
                }

                UInt32Value thisTemp = Convert.ToUInt32(eachExcelColumn) + 1;

                thisColumn.Append(new Column()
                {
                    CustomWidth = true,
                    Width = thisColumnMaxLength + 5,
                    Min = thisTemp,
                    Max = thisTemp,
                });
            }

            Columns tempCols = wsp[i].Worksheet.Descendants<Columns>().FirstOrDefault();
            tempCols = (Columns)thisColumn;
        }

        private static void CreateHeaderFooter(string Classification, WorksheetPart[] wsp, SheetData[] sd, int i)
        {
            HeaderFooter hf = sd[i].Descendants<HeaderFooter>().FirstOrDefault();

            if (hf != null)
            {
                foreach (var element in new List<OpenXmlLeafTextElement> { hf.EvenFooter, hf.EvenHeader, hf.OddFooter, hf.FirstFooter, hf.FirstHeader })
                {
                    if (element != null)
                    {
                        element.Text = Classification;
                    }
                }
            }
            else
            {
                hf = new HeaderFooter();
                hf.DifferentOddEven = false;
                hf.OddHeader = new OddHeader();
                hf.OddHeader.Text = Classification;
                hf.OddFooter = new OddFooter();
                hf.OddFooter.Text = Classification;
                wsp[i].Worksheet.AppendChild<HeaderFooter>(hf);

            }
        }

        private void CreateContent(System.Data.DataTable _DataSet, SheetData sheetData, UInt32 StartRow, _GetExcelInfo _ge)
        {
            DataColumnCollection dsColumns = _DataSet.Columns;

            Cell cell;

            for (int r = 0; r < _DataSet.Rows.Count; r++)
            {
                Row row = new Row();
                row.RowIndex = ++StartRow;

                // row = new DocumentFormat.OpenXml.Spreadsheet.Row { RowIndex = ++rowIndex };
                sheetData.AppendChild(row);

                DataRow dr = _DataSet.Rows[r];

                for (int c = 0; c < _DataSet.Columns.Count; c++)
                {
                    string CurrentColumn = _ge.GetXCellViaNumber(c + 1);

                    DataColumn thisColumn = _DataSet.Columns[c];

                    CellValues CellDataType = CellValues.InlineString;

                    string colName = thisColumn.ColumnName.ToUpper();

                    if (colName == "REQ"
                        || colName == "AUTH"
                        || colName == "DOC_MAP"
                        || colName == "ASG")
                    {
                        CellDataType = CellValues.Number;
                    }


                    //strip time formatting for DEROS_DATE
                    if (colName == "DEROS_DATE"
                        || colName == "DATE_ARRIVED")
                    {
                        string val = _DataSet.Rows[r][c].ToString();

                        if (!string.IsNullOrEmpty(val))
                        {
                            DateTime dt;
                            if (DateTime.TryParse(val, out dt))
                            {
                                val = dt.ToString("yyyy-MM-dd");
                            }
                        }
                        cell = CreateContentCell(CurrentColumn, StartRow, val, CellDataType);
                    }
                    else
                    {
                        cell = CreateContentCell(CurrentColumn, StartRow, _DataSet.Rows[r][c].ToString(), CellDataType);
                    }


                    //cell.StyleIndex = 0;

                    row.AppendChild(cell);
                }
            }
        }

        private void CreateColumnHeader(System.Data.DataTable _DataSet, SheetData sheetData, _GetExcelInfo _ge, UInt32 StartRow)
        {
            Row row = new Row();
            row.RowIndex = StartRow;

            Cell cell;

            sheetData.AppendChild(row);

            for (int c = 0; c < _DataSet.Columns.Count; c++)
            {
                string CurrentColumn = _ge.GetXCellViaNumber(c + 1);

                cell = CreateContentCell(CurrentColumn, StartRow, _DataSet.Columns[c].ToString(), CellValues.InlineString);

                //cell.StyleIndex = 0;

                row.AppendChild(cell);
            }
        }

        private static Stylesheet CreateStylesheet()
        {
            Stylesheet ss = new Stylesheet();

            /*NumberingFormats nfs = CreateNumberingFormats();

            Fonts fts = CreateFonts();

            Fills fills = CreateFills();

            Borders borders = CreateBorders();

            CellStyleFormats csfs = CreateCellStyleFormats();

            CellFormats cfs = CreateCellFormats(nfs);

            ss.Append(nfs);
            ss.Append(fts);
            ss.Append(fills);
            ss.Append(borders);
            ss.Append(csfs);
            ss.Append(cfs);

            CellStyles css = CreateCellStyles();
            ss.Append(css);

            DifferentialFormats dfs = CreateDiffFormats();
            ss.Append(dfs);

            TableStyles tss = CreateTableStyles();           
            ss.Append(tss);*/

            return ss;
        }

        private static TableStyles CreateTableStyles()
        {
            TableStyles tss = new TableStyles();
            tss.Count = 0;
            tss.DefaultTableStyle = StringValue.FromString("TableStyleMedium9");
            tss.DefaultPivotStyle = StringValue.FromString("PivotStyleLight16");

            return tss;
        }

        private static CellStyles CreateCellStyles()
        {
            CellStyles css = new CellStyles();
            CellStyle cs = new CellStyle();
            cs.Name = StringValue.FromString("Normal");
            cs.FormatId = 0;
            cs.BuiltinId = 0;
            css.Append(cs);
            css.Count = UInt32Value.FromUInt32((uint)css.ChildElements.Count);

            return css;
        }

        private static DifferentialFormats CreateDiffFormats()
        {
            DifferentialFormats dfs = new DifferentialFormats();
            dfs.Count = 0;

            return dfs;
        }

        private static Fills CreateFills()
        {
            Fills fills = new Fills();
            Fill fill;
            PatternFill patternFill;

            //fill 1
            fill = new Fill();
            patternFill = new PatternFill();
            patternFill.PatternType = PatternValues.None;
            fill.PatternFill = patternFill;
            fills.Append(fill);

            //fill 2
            fill = new Fill();
            patternFill = new PatternFill();
            patternFill.PatternType = PatternValues.Gray125;
            fill.PatternFill = patternFill;
            fills.Append(fill);

            //fill 3
            fill = new Fill();
            patternFill = new PatternFill();
            patternFill.PatternType = PatternValues.Solid;
            patternFill.ForegroundColor = new ForegroundColor();
            patternFill.ForegroundColor.Rgb = HexBinaryValue.FromString("00ff9728");
            patternFill.BackgroundColor = new BackgroundColor();
            patternFill.BackgroundColor.Rgb = patternFill.ForegroundColor.Rgb;
            fill.PatternFill = patternFill;
            fills.Append(fill);

            //fills count
            fills.Count = UInt32Value.FromUInt32((uint)fills.ChildElements.Count);

            return fills;
        }

        private static Fonts CreateFonts()
        {
            Fonts fts = new Fonts();
            Font ft = new Font();
            FontName ftn = new FontName();
            ftn.Val = StringValue.FromString("Arial");
            FontSize ftsz = new FontSize();
            ftsz.Val = DoubleValue.FromDouble(10);
            ft.FontName = ftn;
            ft.FontSize = ftsz;
            fts.Append(ft);

            fts = new Fonts();
            ft = new Font();
            ftn = new FontName();
            ftn.Val = StringValue.FromString("Arial");
            ftsz = new FontSize();
            ftsz.Val = DoubleValue.FromDouble(18);
            ft.FontName = ftn;
            ft.FontSize = ftsz;
            fts.Append(ft);

            //fonts count
            fts.Count = UInt32Value.FromUInt32((uint)fts.ChildElements.Count);

            return fts;
        }

        private static Borders CreateBorders()
        {
            Borders borders = new Borders();
            Border border = new Border();
            border.LeftBorder = new LeftBorder();
            border.RightBorder = new RightBorder();
            border.TopBorder = new TopBorder();
            border.BottomBorder = new BottomBorder();
            border.DiagonalBorder = new DiagonalBorder();
            borders.Append(border);

            border = new Border();
            border.LeftBorder = new LeftBorder();
            border.LeftBorder.Style = BorderStyleValues.Thin;
            border.RightBorder = new RightBorder();
            border.RightBorder.Style = BorderStyleValues.Thin;
            border.TopBorder = new TopBorder();
            border.TopBorder.Style = BorderStyleValues.Thin;
            border.BottomBorder = new BottomBorder();
            border.BottomBorder.Style = BorderStyleValues.Thin;
            border.DiagonalBorder = new DiagonalBorder();
            //border.DiagonalBorder.Style = BorderStyleValues.Thin;
            borders.Append(border);

            borders.Count = UInt32Value.FromUInt32((uint)borders.ChildElements.Count);

            return borders;
        }

        private static CellStyleFormats CreateCellStyleFormats()
        {
            CellStyleFormats csfs = new CellStyleFormats();
            CellFormat cf = new CellFormat();

            cf.NumberFormatId = 0;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            csfs.Append(cf);

            csfs.Count = UInt32Value.FromUInt32((uint)csfs.ChildElements.Count);

            return csfs;
        }

        private static CellFormats CreateCellFormats(NumberingFormats nfs)
        {
            CellFormats cfs = new CellFormats();
            CellFormat cf = new CellFormat();

            cf = new CellFormat();
            cf.NumberFormatId = 0;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cfs.Append(cf);

            //index 1
            cf = new CellFormat();
            cf.NumberFormatId = 164 + 1;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            cfs.Append(cf);

            //index 2
            cf = new CellFormat();
            cf.NumberFormatId = 164 + 2;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            cfs.Append(cf);

            //index 3
            cf = new CellFormat();
            cf.NumberFormatId = 164 + 3;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            cfs.Append(cf);

            //index 4
            cf = new CellFormat();
            cf.NumberFormatId = 164 + 4;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            cfs.Append(cf);

            //index 5
            cf = new CellFormat();
            cf.NumberFormatId = 164 + 5;
            cf.FontId = 1;
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            cfs.Append(cf);

            //index 6
            cf = new CellFormat();
            cf.NumberFormatId = 164 + 6;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 1;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            cfs.Append(cf);

            //index 7
            cf = new CellFormat();
            cf.NumberFormatId = 164 + 7;
            cf.FontId = 0;
            cf.FillId = 2;
            cf.BorderId = 1;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            cfs.Append(cf);

            //index 8
            cf = new CellFormat();
            cf.NumberFormatId = 164 + 8;
            cf.FontId = 0;
            cf.FillId = 2;
            cf.BorderId = 1;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            cfs.Append(cf);

            cfs.Count = UInt32Value.FromUInt32((uint)cfs.ChildElements.Count);

            return cfs;
        }

        private static NumberingFormats CreateNumberingFormats()
        {
            uint iExcelIndex = 164;
            NumberingFormats nfs = new NumberingFormats();

            NumberingFormat nfDateTime = new NumberingFormat();
            nfDateTime.NumberFormatId = UInt32Value.FromUInt32(iExcelIndex++);
            nfDateTime.FormatCode = StringValue.FromString("dd/mm/yyyy hh:mm:ss");
            nfs.Append(nfDateTime);

            NumberingFormat nf4decimal = new NumberingFormat();
            nf4decimal.NumberFormatId = UInt32Value.FromUInt32(iExcelIndex++);
            nf4decimal.FormatCode = StringValue.FromString("#,##0.0000");
            nfs.Append(nf4decimal);

            // #,##0.00 is also Excel style index 4
            NumberingFormat nf2decimal = new NumberingFormat();
            nf2decimal.NumberFormatId = UInt32Value.FromUInt32(iExcelIndex++);
            nf2decimal.FormatCode = StringValue.FromString("#,##0.00");
            nfs.Append(nf2decimal);

            // @ is also Excel style index 49
            NumberingFormat nfForcedText = new NumberingFormat();
            nfForcedText.NumberFormatId = UInt32Value.FromUInt32(iExcelIndex++);
            nfForcedText.FormatCode = StringValue.FromString("@");
            nfs.Append(nfForcedText);

            nfs.Count = UInt32Value.FromUInt32((uint)nfs.ChildElements.Count);

            return nfs;
        }
    }

    public class _GetExcelInfo
    {
        public string GetXCellViaNumber(Int32 colIndex)
        {
            int div = colIndex;
            string colLetter = String.Empty;
            int mod;

            while (div > 0)
            {
                mod = (div - 1) % 26;
                colLetter = (char)(65 + mod) + colLetter;
                div = (int)((div - mod) / 26);
            }

            return colLetter;
        }
    }
}
