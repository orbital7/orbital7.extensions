using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace Orbital7.Extensions.Windows
{
    public static class PDFHelper
    {
        public static string ExtractText(string pdfFilePath, bool includePageBreaks)
        {
            return ExtractText(new PdfReader(pdfFilePath), includePageBreaks);
        }

        public static string ExtractText(byte[] pdfFileContents, bool includePageBreaks)
        {
            return ExtractText(new PdfReader(pdfFileContents), includePageBreaks);
        }

        public static void CreateSinglePDF(List<byte[]> pages, string pdfFilePath)
        {
            // If only a single PDF is provided, check for early exit.
            if ((pages.Count == 1) && IsPDF(pages[0]))
            {
                File.WriteAllBytes(pdfFilePath, pages[0]);
                return;
            }

            // Create the document.
            Document doc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
            PdfCopy writer = new PdfCopy(doc, new FileStream(pdfFilePath, FileMode.Create));
            doc.Open();

            foreach (byte[] page in pages)
            {
                if (page != null)
                {
                    // New page.
                    doc.NewPage();

                    // If the page is a PDF, simply add the PDF document.
                    if (IsPDF(page))
                    {
                        writer.AddDocument(new PdfReader(page));
                    }
                    // Else, let's try to convert it to an image.
                    else
                    {
                        try
                        {
                            writer.AddDocument(new PdfReader(ImageToPDF(page)));
                        }
                        catch
                        {
                            throw new Exception("File contents must comprise either a PDF or image file.");
                        }
                    }
                }
            }

            // Close.
            doc.Close();
        }

        public static byte[] CreateSinglePDF(params Stream[] pages)
        {
            return CreateSinglePDF((from x in pages where x != null select x.ReadAll()).ToList());
        }

        public static byte[] CreateSinglePDF(List<byte[]> pages)
        {
            // Gather the set of non-null pages.
            var actualPages = (from x in pages where x != null select x).ToList();

            // If only a single PDF is provided, check for early exit.
            if ((actualPages.Count == 1) && IsPDF(actualPages[0]))
                return actualPages[0];

            // Create the document.
            string tempPath = FileSystemHelper.GetTempPath(Guid.NewGuid().ToString() + ".pdf");
            CreateSinglePDF(actualPages, tempPath);

            // Read in the closed document.
            byte[] content = File.ReadAllBytes(tempPath);
            FileSystemHelper.DeleteFile(tempPath);
            return content;
        }
        
        public static void ImageToPDF(byte[] image, string pdfFilePath)
        {
            // Create the document.
            Document doc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
            doc.Open();

            // Add the image.
            doc.NewPage();
            doc.Add(iTextSharp.text.Image.GetInstance(image));
            doc.Close();
        }

        public static byte[] ImageToPDF(byte[] image)
        {
            // Create the document.
            string tempPath = FileSystemHelper.GetTempPath(Guid.NewGuid().ToString() + ".pdf");
            ImageToPDF(image, tempPath);

            // Read in the closed document.
            byte[] content = File.ReadAllBytes(tempPath);
            FileSystemHelper.DeleteFile(tempPath);
            return content;
        }

        public static bool IsPDF(byte[] contents)
        {
            try
            {
                using (PdfReader reader = new PdfReader(contents)) { }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsPDF(string filePath)
        {
            try
            {
                using (PdfReader reader = new PdfReader(filePath)) { }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string ExtractText(PdfReader reader, bool includePageBreaks)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                sb.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                if (includePageBreaks) sb.Append("\f");
                sb.AppendLine();
            }
            reader.Close();

            return sb.ToString();
        }
    }
}
