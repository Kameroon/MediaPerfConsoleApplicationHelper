using DataAccess;
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
using System.Threading;
using System.Threading.Tasks;
using Tools;

namespace ConsoleAppGeneratePDFFile
{
    class Program
    {
        private static readonly SQLDataBase sqlDB = new SQLDataBase(
            "fr-sqlp02.hilti.com",
            "E2MKI_Dataminer", 
            "E2-MKI-ROBOT", 
            "hiltisql0234");

        #region -- Async method --
        static int CountCharacters()
        {
            int count = 0;
            string pdfPath = @"C:\Users\Sweet Family\Desktop\PdfFilesPath\text.pdf";

            using (StreamReader streamReader = new StreamReader(pdfPath))
            {
                string content = streamReader.ReadToEnd();
                count = content.Length;
                Thread.Sleep(5000);
            }
            
            return count;
        }

        static async void Run2Methods()
        {
            Task<int> task = new Task<int>(CountCharacters);
            task.Start();

            Console.WriteLine("Begin create PDF File !");
            var result = await task;

            Console.WriteLine($"End create PDF File ! \n\r Result : { result }");
        }

        static async Task<bool> CallCreatePDFFile(DataTable dataTable, string pdfPath)
        {
            var result = false;
            try
            {
                Console.WriteLine("Begin create PDF File !");

                result = await Task.Run(() => Manager.CreatePDFV2(dataTable, pdfPath));
               
                Console.WriteLine("End create PDF File !");
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        private void RunAsync()
        {
            string param = "Hi";
            Task.Run(() => MethodWithParameter(param));
        }

        private void MethodWithParameter(string param)
        {
            //Do stuff
        }
        #endregion

        static void Main(string[] args)
        {



            var authors = GetAuthors();

            //foreach (var author in authors)
            //{
            //    Console.WriteLine("Author: {0},{1},{2},{3},{4}", 
            //        author.Name, 
            //        author.Age, 
            //        author.BookTitle, 
            //        author.IsMVP,
            //        author.PublishedDate);
            //}

            var dateTable = DataTableHelper.ToDataTable<Author>(authors);

            //var dataSet = DataTableHelper.ConvertToDataSet<Author>(authors);

            string pdfPath = @"C:\Users\Sweet Family\Desktop\PdfFilesPath";


            string _imagePath = "https://ftp.mediaperf.com/img/logo.gif";
            //PDFManagerV1.CreatePdf4(pdfPath, _imagePath);


            ////  - ************************************* -
            //PDFManagerV1.ADDPdf(pdfPath);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var res = Manager.CreatePDFV2(dateTable, pdfPath);
            stopwatch.Stop();
            TimeSpan stopwatchElapsed = stopwatch.Elapsed;
            Console.WriteLine("TEMPS MIS pour générer un PDF SYNC " + Convert.ToInt32(stopwatchElapsed.TotalMilliseconds));

            //stopwatch.Start();
            //Task task = CallCreatePDFFile(dateTable, pdfPath);
            //TimeSpan stopwatchElapsed = stopwatch.Elapsed;
            //Console.WriteLine("TEMPS MIS pour générer un PDF ASYNC " + Convert.ToInt32(stopwatchElapsed.TotalMilliseconds));

            //TestPDF.ManipulatePdf(pdfPath + "\\test.pdf", @"C:\Users\Sweet Family\Desktop\PdfFilesPath");

            ////TestPDF.CreatePDFDocument(dateTable, pdfPath);

            ////CustomReports customReports = new CustomReports();
            ////customReports.CreatePDF("45555555555555555555555555");

            //PDFManagerV1.CreatePDF(dateTable, pdfPath);

            //GeneratePDF(pdfPath);

            ;
            //GeneratePDFFile(pdfPath);

            //CreatePDFTutorial();

            //// -- !!!!! --
            //PDFHelper.AddImage(2);

            //CreatePDF(pdfPath);

            //PDFHelper.CreatePDF(dateTable, pdfPath);

            #region MyRegion.
            /*
            string filePath = pdfPath + "\\SpacingTest.pdf";
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {

                Document doc = new Document();
                PdfWriter.GetInstance(doc, fs);
                doc.Open();

                Paragraph paragraphTable1 = new Paragraph();
                paragraphTable1.SpacingAfter = 15f;

                PdfPTable table = new PdfPTable(3);
                PdfPCell cell = new PdfPCell(new Phrase("This is table 1"));
                cell.Colspan = 3;
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);
                table.AddCell("Col 1 Row 1");
                table.AddCell("Col 2 Row 1");
                table.AddCell("Col 3 Row 1");
                //table.AddCell("Col 1 Row 2"); 
                //table.AddCell("Col 2 Row 2"); 
                //table.AddCell("Col 3 Row 2"); 
                paragraphTable1.Add(table);
                doc.Add(paragraphTable1);

                Paragraph paragraphTable2 = new Paragraph();
                paragraphTable2.SpacingAfter = 10f;

                table = new PdfPTable(3);
                cell = new PdfPCell(new Phrase("This is table 2"));
                cell.Colspan = 3;
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);
                table.AddCell("Col 1 Row 1");
                table.AddCell("Col 2 Row 1");
                table.AddCell("Col 3 Row 1");
                table.AddCell("Col 1 Row 2");
                table.AddCell("Col 2 Row 2");
                table.AddCell("Col 3 Row 2");
                paragraphTable2.Add(table);
                doc.Add(paragraphTable2);
                doc.Close();
            }

            // ...and start a viewer.
            Process.Start(filePath);
            */
            #endregion

            Console.WriteLine("");

            //Console.ReadKey();
        }

        #region -- SQL Methods --
        private string GetCustomerStreet(string customerID)
        {
            // - Get street from database -
            string street = sqlDB.ExecuteScalar("SELECT Street FROM E2MKI_Dataminer.dbo.TD_Customer WHERE SalesOrg = '0900' AND CustomerID = '{0}'",
                Parser.ToInt32(customerID).ToString()) as string;

            // - Return an empty string if street not found -
            return street ?? string.Empty;
        }

        private string GetUserFullName(string userID)
        {
            // - Get user full name from database -
            string fullName = sqlDB.ExecuteScalar("SELECT FirstName + ' ' + UPPER(LastName) FROM E2MKI_VOC.dbo.TD_User WHERE UserID = '{0}'", userID) as string;

            // - Return an empty string if name not found -
            return fullName ?? string.Empty;
        }

        private static  void  r()
        {
            /*
            // - Check if there is at least 1 machine -
            if (customer.MachineList != null && customer.MachineList.Count > 0)
            {
                // - Generate IN clause (distinct materials) -
                string inClause = customer.MachineList.Select(x => x.Code_Article).Distinct().Aggregate((total, next) => total + ',' + next);

                using (DataTable pictureTable = sqlDB.GetDataTable("SELECT Item, Picture FROM E2MKI_CustomPrice.dbo.VD_Picture WHERE SalesOrg = 0 AND [Language] = 1 AND Item IN ({0})", inClause))
                {
                    foreach (DataRow pictureRow in pictureTable.Rows)
                    {
                        customer.PictureList.Add(new MaterialPicture()
                        {
                            Material = pictureRow["Item"].ToString(),
                            Picture = (byte[])pictureRow["Picture"]
                        });
                    }
                }

                using (DataTable descTable = sqlDB.GetDataTable("SELECT Material, Description, IsCombo = CASE WHEN EXISTS (SELECT * FROM E2MKI_MaterialMaster.pricing.TD_MAST WHERE Material = MAKT.Material) THEN 1 ELSE 0 END FROM E2MKI_MaterialMaster.pricing.TD_MAKT AS MAKT WHERE Material IN ({0}) AND [Language] = 'F'", inClause))
                {
                    customer.MachineList = (from machine in customer.MachineList
                                            from descRow in descTable.AsEnumerable().Where(x => x["Material"].ToString() == machine.Code_Article).DefaultIfEmpty()
                                            where descRow == null || (int)descRow["IsCombo"] == 0
                                            select new Machine()
                                            {
                                                Code_Article = machine.Code_Article,
                                                Designation = descRow == null ? null : (string)descRow["Description"],
                                                Serial = machine.Serial,
                                                Duree = machine.Duree,
                                                Date_Debut_Contrat = machine.Date_Debut_Contrat,
                                                Date_Fin_Contrat = machine.Date_Fin_Contrat,
                                                Ref_Organisation = machine.Ref_Organisation,
                                                Num_Inventaire = machine.Num_Inventaire,
                                                Centre_De_Coût = machine.Centre_De_Coût,
                                                Type_Ligne = machine.Type_Ligne
                                            }).ToList();
                }

                // - Build "IN" clause -
                string inClause = contractTable.AsEnumerable()
                    .Select(x => string.Format("'{0}'", x["ZZCONTRACTNO"])).
                    Aggregate((total, next) => total + ',' + next);
            }
               */
        }
        #endregion

        #region -- SAVE --
        private static PdfPCell GetCell(string text)
        {
            return GetCell(text, 1, 1);
        }

        private static PdfPCell GetCell(string text, int colSpan, int rowSpan)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text));
            cell.HorizontalAlignment = 1;
            cell.Rowspan = rowSpan;
            cell.Colspan = colSpan;

            return cell;
        }

        private static void GeneratePDF(string path)
        {
            //string imgUrl = @"C:\Users\Sweet Family\Desktop\imgDrole.png";
            string imgUrl = @"C:\Users\Sweet Family\Desktop\logo.jpg";

            string filePath = path + "\\TESTPDF1.pdf";

            var output = new FileStream(filePath, FileMode.Create);

            using (Document document = new Document(PageSize.A4, 10, 10, 42, 35))
            using (var writer = PdfWriter.GetInstance(document, output))
            {
                document.Open();

                // -- Add logo à gauche --
                var logo = Image.GetInstance(imgUrl);
                logo.SetAbsolutePosition(20, 770);
                logo.ScaleAbsoluteHeight(50);
                logo.ScaleAbsoluteWidth(90);
                document.Add(logo);

                // -- Add logo à droite --
                var logo10 = Image.GetInstance(imgUrl);
                logo10.SetAbsolutePosition(490, 770);
                logo10.ScaleAbsoluteHeight(50);
                logo10.ScaleAbsoluteWidth(90);
                document.Add(logo10);

                Paragraph paragraphTable1 = new Paragraph();
                paragraphTable1.SpacingAfter = 15f;

                var titleFont = new Font(Font.FontFamily.UNDEFINED, 24);
                var subTitleFont = new Font(Font.FontFamily.UNDEFINED, 16);

                PdfPTable table0 = new PdfPTable(2);
                PdfContentByte cb = writer.DirectContent;
                table0 = new PdfPTable(1);
                table0.TotalWidth = 140;
                table0.AddCell("TEST GAUCHE");
                table0.WriteSelectedRows(0, -1, 35, 650, cb);

                PdfPTable table10 = new PdfPTable(2);
                table10 = new PdfPTable(1);
                table10.TotalWidth = 150;
                table10.AddCell("TEST CENTRE");
                table10.WriteSelectedRows(0, -1, 220, 650, cb);


                PdfPTable table30 = new PdfPTable(2);
                table30 = new PdfPTable(1);
                table30.TotalWidth = 140;
                table30.AddCell("TEST DROITE");
                table30.WriteSelectedRows(0, -1, 400, 650, cb);



                PdfPTable table1 = new PdfPTable(2);
                table1.DefaultCell.Border = 0;
                table1.WidthPercentage = 30;
                //table1.WriteSelectedRows(0, -1, 300, 300, pcb);

                Paragraph paragraphTable20 = new Paragraph();
                paragraphTable20.SpacingBefore = 25f;


                //PdfPCell cell11 = new PdfPCell();
                // cell11.Colspan = 2;
                // cell11.AddElement(new Paragraph("ABC Traders Receipt", titleFont));
                // cell11.AddElement(new Paragraph("Thankyou for shoping at ABC traders,your order details are below",
                //     subTitleFont));

                // cell11.VerticalAlignment = Element.ALIGN_LEFT;
                // table1.AddCell(cell11);

                //paragraphTable1.Add(table1);
                //document.Add(paragraphTable1);

                PdfPCell cell12 = new PdfPCell();
                //cell12.VerticalAlignment = Element.ALIGN_CENTER;
                //table1.AddCell(cell11);
                //table1.AddCell(cell12);


                Paragraph paragraphTable2 = new Paragraph();
                paragraphTable2.SpacingAfter = 15f;

                PdfContentByte pcb = writer.DirectContent;
                PdfPTable table = new PdfPTable(9);
                table.TotalWidth = 595;

                // there isn't any content in the table: there's nothing to write yet

                // Header row.
                table.AddCell(GetCell("Header 1", 1, 2));
                table.AddCell(GetCell("Header 2", 1, 2));
                table.AddCell(GetCell("Header 3", 5, 1));
                table.AddCell(GetCell("Header 4", 1, 2));
                table.AddCell(GetCell("Header 5", 1, 2));

                // Inner middle row.
                table.AddCell(GetCell("H1"));
                table.AddCell(GetCell("H2"));
                table.AddCell(GetCell("H3"));
                table.AddCell(GetCell("H4"));
                table.AddCell(GetCell("H5"));

                //paragraphTable2.Add(table);

                table.WriteSelectedRows(0, 580, 10, 350, pcb);

                document.Add(paragraphTable2);



                document.Close();
                writer.Close();
            }


            // ...and start a viewer.
            Process.Start(filePath);
        }

        private static void GeneratePDFFile(string pdfPath)
        {
            string imgUrl = @"C:\Users\Sweet Family\Desktop\imgDrole.png";

            string filePath = pdfPath + "\\TESTPDF.pdf";

            Document doc = new Document(PageSize.A4);
            var output = new FileStream(filePath, FileMode.Create);
            var writer = PdfWriter.GetInstance(doc, output);

            doc.Open();

            var logo = Image.GetInstance(imgUrl);
            logo.SetAbsolutePosition(430, 770);
            logo.ScaleAbsoluteHeight(30);
            logo.ScaleAbsoluteWidth(70);
            doc.Add(logo);

            PdfPTable table1 = new PdfPTable(2);
            table1.DefaultCell.Border = 0;
            table1.WidthPercentage = 80;

            var titleFont = new Font(Font.FontFamily.UNDEFINED, 24);
            var subTitleFont = new Font(Font.FontFamily.UNDEFINED, 16);

            PdfPCell cell11 = new PdfPCell();
            cell11.Colspan = 1;
            cell11.AddElement(new Paragraph("ABC Traders Receipt", titleFont));

            cell11.AddElement(new Paragraph("Thankyou for shoping at ABC traders,your order details are below",
                subTitleFont));

            cell11.VerticalAlignment = Element.ALIGN_LEFT;
            PdfPCell cell12 = new PdfPCell();
            cell12.VerticalAlignment = Element.ALIGN_CENTER;
            table1.AddCell(cell11);
            table1.AddCell(cell12);

            PdfPTable table2 = new PdfPTable(3);
            //One row added
            PdfPCell cell21 = new PdfPCell();
            cell21.AddElement(new Paragraph("Photo Type"));
            PdfPCell cell22 = new PdfPCell();
            cell22.AddElement(new Paragraph("No. of Copies"));
            PdfPCell cell23 = new PdfPCell();
            cell23.AddElement(new Paragraph("Amount"));

            table2.AddCell(cell21);
            table2.AddCell(cell22);
            table2.AddCell(cell23);

            //New Row Added
            PdfPCell cell31 = new PdfPCell();
            cell31.AddElement(new Paragraph("Safe"));
            cell31.FixedHeight = 300.0f;
            PdfPCell cell32 = new PdfPCell();
            cell32.AddElement(new Paragraph("2"));
            cell32.FixedHeight = 300.0f;
            PdfPCell cell33 = new PdfPCell();
            cell33.AddElement(new Paragraph("20.00 * " + "2" + " = " + (20 * Convert.ToInt32("2")) + ".00"));

            cell33.FixedHeight = 300.0f;

            table2.AddCell(cell31);
            table2.AddCell(cell32);
            table2.AddCell(cell33);

            PdfPCell cell2A = new PdfPCell(table2);
            cell2A.Colspan = 2;
            table1.AddCell(cell2A);

            PdfPCell cell41 = new PdfPCell();
            cell41.AddElement(new Paragraph("Name : " + "ABC"));
            cell41.AddElement(new Paragraph("Advance : " + "advance"));
            cell41.VerticalAlignment = Element.ALIGN_LEFT;

            PdfPCell cell42 = new PdfPCell();
            cell42.AddElement(new Paragraph("Customer ID : " + "011"));
            cell42.AddElement(new Paragraph("Balance : " + "3993"));
            cell42.VerticalAlignment = Element.ALIGN_RIGHT;

            table1.AddCell(cell41);
            table1.AddCell(cell42);

            doc.Add(table1);
            doc.Close();

            // ...and start a viewer.
            Process.Start(filePath);
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
            var date = Convert.ToDateTime(DateTime.Now.Date.ToShortDateString());
            var authorList = new List<Author>();

            authorList.Add(new Author("Mahesh Chand",
                35,
                "A Prorammer's Guide to ADO.NET",
                true,
                date));
            authorList.Add(new Author("Neel Beniwal",
                18,
                "Graphics Development with C#",
                false,
                date));
            authorList.Add(new Author("Praveen Kumar",
                28,
                "Mastering WCF",
                true,
                date));
            authorList.Add(new Author("Mahesh Chand",
                35,
                "Graphics Programming with GDI+",
                true,
                date));
            authorList.Add(new Author("Raj Kumar",
                30, 
                "Building Creative Systems", 
                false, 
                date));
            authorList.Add(new Author("Neel Beniwal",
               18,
               "Graphics Development with C#",
               false,
               date));
            authorList.Add(new Author("Praveen Kumar",
                28,
                "Mastering WCF",
                true,
                date));
            authorList.Add(new Author("Mahesh Chand",
                35,
                "Graphics Programming with GDI+",
                true,
                date));
            authorList.Add(new Author("Mahesh Chand",
               35,
               "A Prorammer's Guide to ADO.NET",
               true,
               date));
            authorList.Add(new Author("Neel Beniwal",
                18,
                "Graphics Development with C#",
                false,
                date));
            authorList.Add(new Author("Praveen Kumar",
                28,
                "Mastering WCF",
                true,
                date));
            authorList.Add(new Author("Mahesh Chand",
                35,
                "Graphics Programming with GDI+",
                true,
                date));
            authorList.Add(new Author("Raj Kumar",
                30,
                "Building Creative Systems",
                false,
                date));
            authorList.Add(new Author("Neel Beniwal",
               18,
               "Graphics Development with C#",
               false,
               date));
            authorList.Add(new Author("Praveen Kumar",
                28,
                "Mastering WCF",
                true,
                date));
            authorList.Add(new Author("Mahesh Chand",
                35,
                "Graphics Programming with GDI+",
                true,
                date));
            authorList.Add(new Author("Mahesh Chand",
               35,
               "A Prorammer's Guide to ADO.NET",
               true,
               date));
            authorList.Add(new Author("Neel Beniwal",
                18,
                "Graphics Development with C#",
                false,
                date));
            authorList.Add(new Author("Praveen Kumar",
                28,
                "Mastering WCF",
                true,
                date));
            authorList.Add(new Author("Mahesh Chand",
               35,
               "Graphics Programming with GDI+",
               true,
               date));
            authorList.Add(new Author("Mahesh Chand",
               35,
               "A Prorammer's Guide to ADO.NET",
               true,
               date));
            authorList.Add(new Author("Neel Beniwal",
                18,
                "Graphics Development with C#",
                false,
                date));
            authorList.Add(new Author("Praveen Kumar",
                28,
                "Mastering WCF",
                true,
                date));
            authorList.Add(new Author("Mahesh Chand",
                35,
                "Graphics Programming with GDI+",
                true,
                date));
            authorList.Add(new Author("Raj Kumar",
                30,
                "Building Creative Systems",
                false,
                date));
            authorList.Add(new Author("Neel Beniwal",
               18,
               "Graphics Development with C#",
               false,
               date));
            authorList.Add(new Author("Praveen Kumar",
                28,
                "Mastering WCF",
                true,
                date));
            authorList.Add(new Author("Mahesh Chand",
                35,
                "A Prorammer's Guide to ADO.NET",
                true,
                date));
            authorList.Add(new Author("Neel Beniwal",
                18,
                "Graphics Development with C#",
                false,
                date));
            authorList.Add(new Author("Praveen Kumar",
                28,
                "Mastering WCF",
                true,
                date));
            authorList.Add(new Author("Mahesh Chand",
               35,
               "Graphics Programming with GDI+",
               true,
               date));
            authorList.Add(new Author("Mahesh Chand",
               35,
               "A Prorammer's Guide to ADO.NET",
               true,
               date));
            authorList.Add(new Author("Neel Beniwal",
                18,
                "Graphics Development with C#",
                false,
                date));
            authorList.Add(new Author("Praveen Kumar",
                28,
                "Mastering WCF",
                true,
                date));
            authorList.Add(new Author("Mahesh Chand",
                35,
                "Graphics Programming with GDI+",
                true,
                date));
            authorList.Add(new Author("Raj Kumar",
                30,
                "Building Creative Systems",
                false,
                date));
            authorList.Add(new Author("Neel Beniwal",
               18,
               "Graphics Development with C#",
               false,
               date));
            authorList.Add(new Author("Praveen Kumar",
                28,
                "Mastering WCF",
                true,
                date));


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
        #endregion
    }
}
