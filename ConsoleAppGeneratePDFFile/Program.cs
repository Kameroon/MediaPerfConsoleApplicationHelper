using iTextSharp.text;
using iTextSharp.text.pdf;
using MigraDoc.Rendering;
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
    class Program
    {
        static void Main(string[] args)
        {

            var authors = GetAuthors();

            foreach (var author in authors)
            {
                Console.WriteLine("Author: {0},{1},{2},{3},{4}", 
                    author.Name, 
                    author.Age, 
                    author.BookTitle, 
                    author.IsMVP,
                    author.PublishedDate);
            }

            var dateTable = DataTableHelper.ToDataTable<Author>(authors);

            //var dataSet = DataTableHelper.ConvertToDataSet<Author>(authors);

            string pdfPath = @"C:\Users\Sweet Family\Desktop\PdfFilesPath";


            CreatePDFTutorial();

            //// -- !!!!! --
            //PDFHelper.AddImage(2);

            //CreatePDF(pdfPath);

            //PDFHelper.CreatePDF(dateTable, pdfPath);


            Console.WriteLine("");
        }

        private static void CreatePDFTutorial()
        {
            string fileName = string.Empty;

            DateTime fileCreationDatetime = DateTime.Now;

            fileName = string.Format("{0}.pdf", 
                fileCreationDatetime.ToString(@"yyyyMMdd") + "_" + fileCreationDatetime.ToString(@"HHmmss"));

            //string pdfPath = Server.MapPath(@"~\PDFs\") + fileName;
            string pdfPath = fileName;

            using (FileStream msReport = new FileStream(pdfPath, FileMode.Create))
            {
                //step 1
                using (Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 140f, 10f))
                {
                    try
                    {
                        // step 2
                        PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, msReport);
                        //pdfWriter.PageEvent = new Common.ITextEvents();

                        //open the stream
                        pdfDoc.Open();

                        for (int i = 0; i < 10; i++)
                        {
                            Paragraph para = new Paragraph("Hello world. Checking Header Footer",
                                new Font(Font.FontFamily.HELVETICA, 22));

                            para.Alignment = Element.ALIGN_CENTER;

                            pdfDoc.Add(para);

                            pdfDoc.NewPage();
                        }

                        pdfDoc.Close();

                    }
                    catch (Exception ex)
                    {
                        //handle exception
                    }

                    finally
                    {

                    }
                }
            }
        }


        private static void CreatePDF(string path)
        {
            try
            {
                string imgUrl = @"C:\Users\Sweet Family\Desktop\imgDrole.png";
                                
                PDFManager pdfManager = new PDFManager(GetTable(), imgUrl);

                // Create a MigraDoc document
                var document = pdfManager.CreateDocument();
                document.UseCmykColor = true;

#if DEBUG
                // for debugging only...
                MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToFile(document, "MigraDoc.mdddl");
                document = MigraDoc.DocumentObjectModel.IO.DdlReader.DocumentFromFile("MigraDoc.mdddl");
#endif

                // Create a renderer for PDF that uses Unicode font encoding
                PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true);

                // Set the MigraDoc document
                pdfRenderer.Document = document;


                // Create the PDF document
                pdfRenderer.RenderDocument();

                // Save the PDF document...
                string filename = "PatientsDetail.pdf";
#if DEBUG
                // I don't want to close the document constantly...
                filename = "Invoice-" + Guid.NewGuid().ToString("N").ToUpper() + ".pdf";
#endif
                pdfRenderer.Save(filename);

                //File.WriteAllText("your file path here", bytes);

                // ...and start a viewer.
                Process.Start(filename);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static List<Author> GetAuthors()
        {
            var authorList = new List<Author>();

            authorList.Add(new Author("Mahesh Chand", 
                35, 
                "A Prorammer's Guide to ADO.NET",
                true,
                new DateTime(2003, 7, 10)));
            authorList.Add(new Author("Neel Beniwal", 
                18,
                "Graphics Development with C#",
                false,
                new DateTime(2010, 2, 22)));
            authorList.Add(new Author("Praveen Kumar",
                28, 
                "Mastering WCF", 
                true,
                new DateTime(2012, 01, 01)));
            authorList.Add(new Author("Mahesh Chand", 
                35, 
                "Graphics Programming with GDI+", 
                true, 
                new DateTime(2008, 01, 20)));
            authorList.Add(new Author("Raj Kumar",
                30, "Building Creative Systems", false, new DateTime(2011, 6, 3)));

            return authorList;
        }


        private static DataTable GetTable()
        {
            //
            // Here we create a DataTable with four columns.
            //
            DataTable table = new DataTable();
            table.Columns.Add("Dosage", typeof(int));
            table.Columns.Add("Drug", typeof(string));
            table.Columns.Add("Patient", typeof(string));
            table.Columns.Add("Date", typeof(DateTime));

            //
            // Here we add five DataRows.
            //
            table.Rows.Add(25, "Indocin", "Shehzad", DateTime.Now);
            table.Rows.Add(50, "Enebrel", "Kashif", DateTime.Now);
            table.Rows.Add(10, "Hydralazine", "Umair", DateTime.Now);
            table.Rows.Add(21, "Combivent", "Jahanzaib", DateTime.Now);
            table.Rows.Add(100, "Dilantin", "Talha", DateTime.Now);
            return table;
        }
    }
}
