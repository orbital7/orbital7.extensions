using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemBox.Spreadsheet
{
    public static class GemBoxSpreadsheetExtensions
    {
        public static SpreadsheetColor Heading1Background { get; set; } = SpreadsheetColor.FromName(ColorName.Black);
        public static SpreadsheetColor Heading1Foreground { get; set; } = SpreadsheetColor.FromName(ColorName.White);
        public static SpreadsheetColor Heading2Background { get; set; } = SpreadsheetColor.FromArgb(128, 128, 128);
        public static SpreadsheetColor Heading2Foreground { get; set; } = SpreadsheetColor.FromName(ColorName.White);
        public static SpreadsheetColor Heading3Background { get; set; } = SpreadsheetColor.FromArgb(217, 217, 217);
        public static SpreadsheetColor Heading3Foreground { get; set; } = SpreadsheetColor.FromName(ColorName.Black);
        public static int Heading1FontSize = 12;
        public static int Heading2FontSize = 11;
        public static int Heading3FontSize = 11;

        public static CellStyle ToBold(this CellStyle style)
        {
            style.Font.Weight = ExcelFont.BoldWeight;
            return style;
        }

        public static CellStyle ToItalic(this CellStyle style)
        {
            style.Font.Italic = true;
            return style;
        }

        public static CellStyle ToUnderline(this CellStyle style, UnderlineStyle underlineStyle = UnderlineStyle.Single)
        {
            style.Font.UnderlineStyle = underlineStyle;
            return style;
        }

        public static CellStyle ToFontSize(this CellStyle style, int fontSize)
        {
            style.Font.Size = 20 * fontSize;
            return style;
        }

        public static CellStyle ToColorForeground(this CellStyle style, SpreadsheetColor color)
        {
            style.Font.Color = color;
            return style;
        }

        public static CellStyle ToColorBackground(this CellStyle style, SpreadsheetColor color)
        {
            style.FillPattern.SetSolid(color);
            return style;
        }

        public static CellStyle To2Digits(this CellStyle style)
        {
            return style.ToCurrency(false, true);
        }

        public static CellStyle ToCurrency(this CellStyle style, bool includeSymbol = true, bool includeChange = true)
        {
            style.NumberFormat = string.Format("{0}#,##0{1}",
                includeSymbol ? "$" : "",
                includeChange ? ".00" : "").Trim();
            return style;
        }

        public static CellStyle ToPercent(this CellStyle style)
        {
            style.NumberFormat = "0.0%";
            return style;
        }

        public static CellStyle ToDate(this CellStyle style)
        {
            style.NumberFormat = "mm/dd/yyyy";
            return style;
        }

        public static CellStyle SetBorders(this CellStyle style, MultipleBorders borders, LineStyle lineStyle, 
            SpreadsheetColor borderColor = default(SpreadsheetColor))
        {
            if (borderColor.IsEmpty)
                borderColor = SpreadsheetColor.FromName(ColorName.Black);

            style.Borders.SetBorders(borders, borderColor, lineStyle);
            return style;
        }

        public static ExcelColumnRowBase ToBold(this ExcelColumnRowBase range)
        {
            range.Style.ToBold();
            return range;
        }

        public static ExcelColumnRowBase ToItalic(this ExcelColumnRowBase range)
        {
            range.Style.ToItalic();
            return range;
        }

        public static ExcelColumnRowBase ToUnderline(this ExcelColumnRowBase range, UnderlineStyle underlineStyle = UnderlineStyle.Single)
        {
            range.Style.ToUnderline(underlineStyle);
            return range;
        }

        public static ExcelColumnRowBase ToFontSize(this ExcelColumnRowBase range, int fontSize)
        {
            range.Style.ToFontSize(fontSize);
            return range;
        }

        public static ExcelColumnRowBase ToColorForeground(this ExcelColumnRowBase range, SpreadsheetColor color)
        {
            range.Style.ToColorForeground(color);
            return range;
        }

        public static ExcelColumnRowBase ToColorBackground(this ExcelColumnRowBase range, SpreadsheetColor color)
        {
            range.Style.ToColorBackground(color);
            return range;
        }

        public static ExcelColumnRowBase To2Digits(this ExcelColumnRowBase range)
        {
            range.Style.To2Digits();
            return range;
        }

        public static ExcelColumnRowBase ToCurrency(this ExcelColumnRowBase range, bool includeSymbol = true, bool includeChange = true)
        {
            range.Style.ToCurrency(includeSymbol, includeChange);
            return range;
        }

        public static ExcelColumnRowBase ToPercent(this ExcelColumnRowBase range)
        {
            range.Style.ToPercent();
            return range;
        }

        public static ExcelColumnRowBase ToDate(this ExcelColumnRowBase range)
        {
            range.Style.ToDate();
            return range;
        }

        public static ExcelColumnRowBase ToHeading1(this ExcelColumnRowBase range)
        {
            return range.ToColorBackground(Heading1Background)
                        .ToColorForeground(Heading1Foreground)
                        .ToFontSize(Heading1FontSize)
                        .ToBold();
        }

        public static ExcelColumnRowBase ToHeading2(this ExcelColumnRowBase range)
        {
            return range.ToColorBackground(Heading2Background)
                        .ToColorForeground(Heading2Foreground)
                        .ToFontSize(Heading2FontSize)
                        .ToBold();
        }

        public static ExcelColumnRowBase ToHeading3(this ExcelColumnRowBase range)
        {
            return range.ToColorBackground(Heading3Background)
                        .ToColorForeground(Heading3Foreground)
                        .ToFontSize(Heading3FontSize)
                        .ToItalic();
        }

        public static CellRange GetRowCellRange(this ExcelWorksheet ws, int row, int startCol, int endCol)
        {
            return ws.Cells.GetSubrangeAbsolute(row, startCol, row, endCol);
        }

        public static CellRange GetRowCellRange(this ExcelWorksheet ws, int row, int endCol)
        {
            return ws.GetRowCellRange(row, 0, endCol);
        }

        public static ExcelWorksheet BorderIndividualCells(this ExcelWorksheet ws, int startRow, int endRow,
            SpreadsheetColor borderColor = default(SpreadsheetColor))
        {
            var colCount = ws.CalculateMaxUsedColumns();
            for (int col = 0; col < colCount; col++)
                for (int row = startRow; row <= endRow; row++)
                    ws.Cells[row, col].SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Bottom, LineStyle.Thin, borderColor);

            return ws;
        }

        public static ExcelWorksheet AutoFitColumns(this ExcelWorksheet ws, int startRow, int endRow)
        {
            var firstRow = ws.Rows[startRow];
            var lastRow = ws.Rows[endRow];

            var colCount = ws.CalculateMaxUsedColumns();
            for (int col = 0; col < colCount; col++)
                ws.Columns[col].AutoFit(1, firstRow, lastRow);

            return ws;
        }

        public static AbstractRange ToBold(this AbstractRange range)
        {
            range.Style.ToBold();
            return range;
        }

        public static AbstractRange ToItalic(this AbstractRange range)
        {
            range.Style.ToItalic();
            return range;
        }

        public static AbstractRange ToUnderline(this AbstractRange range, UnderlineStyle underlineStyle = UnderlineStyle.Single)
        {
            range.Style.ToUnderline(underlineStyle);
            return range;
        }

        public static AbstractRange ToFontSize(this AbstractRange range, int fontSize)
        {
            range.Style.ToFontSize(fontSize);
            return range;
        }

        public static AbstractRange ToColorForeground(this AbstractRange range, SpreadsheetColor color)
        {
            range.Style.ToColorForeground(color);
            return range;
        }

        public static AbstractRange ToColorBackground(this AbstractRange range, SpreadsheetColor color)
        {
            range.Style.ToColorBackground(color);
            return range;
        }

        public static AbstractRange To2Digits(this AbstractRange range)
        {
            range.Style.To2Digits();
            return range;
        }

        public static AbstractRange ToCurrency(this AbstractRange range, bool includeSymbol = true, bool includeChange = true)
        {
            range.Style.ToCurrency(includeSymbol, includeChange);
            return range;
        }

        public static AbstractRange ToPercent(this AbstractRange range)
        {
            range.Style.ToPercent();
            return range;
        }

        public static AbstractRange ToDate(this AbstractRange range)
        {
            range.Style.ToDate();
            return range;
        }

        public static AbstractRange SetValue(this AbstractRange range, string value, int indentLevel = 0)
        {
            range.Value = string.Format("{0}{1}",
                new string(Convert.ToChar(" "), 3 * indentLevel), value);
            return range;
        }

        public static AbstractRange SetValue(this AbstractRange range, int value, bool emptyIfZero)
        {
            if (value != 0 || !emptyIfZero)
                range.Value = value;
            return range;
        }

        public static AbstractRange SetValue(this AbstractRange range, double value, bool emptyIfZero)
        {
            if (value != 0 || !emptyIfZero)
                range.Value = value;
            return range;
        }

        public static AbstractRange SetValue(this AbstractRange range, decimal value, bool emptyIfZero)
        {
            if (value != 0 || !emptyIfZero)
                range.Value = value;
            return range;
        }

        public static AbstractRange SetValue(this AbstractRange range, DateTime? date)
        {
            if (date.HasValue)
                range.Value = date.Value.ToShortDateString();
            return range;
        }

        public static AbstractRange SetHyperlink(this AbstractRange range, string locationUrl, bool isExternal = true)
        {
            range.Hyperlink.Location = locationUrl;
            range.Hyperlink.IsExternal = isExternal;
            return range;
        }

        public static AbstractRange SetFormula(this AbstractRange range, string formula)
        {
            range.Formula = formula;
            return range;
        }

        public static AbstractRange SetBorders(this AbstractRange range, MultipleBorders borders, LineStyle lineStyle,
            SpreadsheetColor borderColor = default(SpreadsheetColor))
        {
            range.Style.SetBorders(borders, lineStyle, borderColor);
            return range;
        }

        public static AbstractRange ToHeading1(this AbstractRange range)
        {
            return range.ToColorBackground(Heading1Background)
                        .ToColorForeground(Heading1Foreground)
                        .ToFontSize(Heading1FontSize)
                        .ToBold();
        }

        public static AbstractRange ToHeading2(this AbstractRange range)
        {
            return range.ToColorBackground(Heading2Background)
                        .ToColorForeground(Heading2Foreground)
                        .ToFontSize(Heading2FontSize)
                        .ToBold();
        }

        public static AbstractRange ToHeading3(this AbstractRange range)
        {
            return range.ToColorBackground(Heading3Background)
                        .ToColorForeground(Heading3Foreground)
                        .ToFontSize(Heading3FontSize)
                        .ToItalic();
        }
    }
}
