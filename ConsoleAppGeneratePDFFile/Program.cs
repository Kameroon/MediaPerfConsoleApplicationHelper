using iTextSharp.text;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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


            CreatePDF();

            PDFHelper.CreatePDF(dateTable, pdfPath);


            Console.WriteLine("");
        }


        private static void CreatePDF()
        {
            try
            {
                PDFManager pdfManager = new PDFManager(GetTable(), @"C:\Users\Sweet Family\Desktop\imgDrole.png");

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
