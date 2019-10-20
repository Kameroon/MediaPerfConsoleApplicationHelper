using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppGeneratePDFFile
{
    public class TestPDF
    {

        public static void CreatePDFDocument(DataTable dataTable, string repositoryPath)
        {
            //PdfWriter masterWriter = null;
            string fileName = string.Empty;
            DateTime fileCreationDatetime = DateTime.Now;
            string imgPath = "https://ftp.mediaperf.com/img/logo.gif";
            imgPath = @"C:\Users\Sweet Family\Desktop\logo.jpg";

            //fileName = string.Format("{0}.pdf", fileCreationDatetime.ToString(@"yyyyMMdd") + "_" + fileCreationDatetime.ToString(@"HHmmss"));
            string fullPdfPath = repositoryPath + "\\Output.pdf";

            if (File.Exists(fullPdfPath))
            {
                File.Delete(fullPdfPath);
            }

            using (var reader = new PdfReader(@"C:\Users\Sweet Family\Desktop\PdfFilesPath\test.pdf"))
            {
                using (var fileStream = new FileStream(fullPdfPath, FileMode.Create, FileAccess.Write))
                {
                    var document = new Document(reader.GetPageSizeWithRotation(1));
                    var writer = PdfWriter.GetInstance(document, fileStream);

                    document.Open();

                    for (var i = 1; i <= reader.NumberOfPages; i++)
                    {
                        document.NewPage();

                        var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        var importedPage = writer.GetImportedPage(reader, i);

                        var contentByte = writer.DirectContent;
                        contentByte.BeginText();
                        contentByte.SetFontAndSize(baseFont, 12);

                        var multiLineString = "Hello,\r\nWorld!".Split('\n');

                        foreach (var line in multiLineString)
                        {
                            contentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, line, 200, 200, 0);
                        }

                        contentByte.EndText();
                        contentByte.AddTemplate(importedPage, 0, 0);
                    }

                    document.Close();
                    writer.Close();
                }
            }

            // ...and start a viewer.
            Process.Start(fullPdfPath);
        }

        public static void ManipulatePdf(String src, String dest)
        {
            string fullPdfPath = dest + "\\Output.pdf";

            if (File.Exists(fullPdfPath))
            {
                File.Delete(fullPdfPath);
            }

            PdfReader reader = new PdfReader(src);
            using (var fileStream = new FileStream(fullPdfPath, FileMode.Create, FileAccess.Write))
            {
                PdfStamper stamper = new PdfStamper(reader, fileStream);
                // Get canvas for page 1
                PdfContentByte cb = stamper.GetOverContent(1);
                // Create template (aka XOBject)
                PdfTemplate xobject = cb.CreateTemplate(80, 120);
                // Add content using ColumnText
                ColumnText column = new ColumnText(xobject);
                column.SetSimpleColumn(new Rectangle(80, 120));
                column.AddElement(new Paragraph("Some long text that needs to be distributed over several lines."));
                column.Go();
                // Add the template to the canvas
                cb.AddTemplate(xobject, 36, 600);
                double angle = Math.PI / 4;
                cb.AddTemplate(xobject,
                        (float)Math.Cos(angle), -(float)Math.Sin(angle),
                    (float)Math.Cos(angle), (float)Math.Sin(angle),
                    150, 600);
                stamper.Close();
            }
            reader.Close();

            Process.Start(fullPdfPath);
        }
    }
}
