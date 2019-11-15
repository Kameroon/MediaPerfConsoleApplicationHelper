using Dapper;
using DataAccess;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
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
        
        private void MethodWithParameter(DataTable dataTable)
        {
            

        }
        #endregion

        #region -- XML --
        private static void XMLData()
        {
            var doc = new XDocument(new XDeclaration("1.0", "UTF-8", ""),
                new XElement("CUSTOMER",
                new XElement("FIRST_NAME", "FirstNameTextBox.Text"),
                new XElement("LAST_NAME", "LastNameTextBox.Text")
                ));

            //var reader = new PdfReader(templateFilename);
            //var stamper = new PdfStamper(reader, new FileStream(bolFilename, FileMode.Create));
            //var xml = GenerateXml(); // Above code
            //var stream = new MemoryStream(UTF8Encoding.Default.GetBytes(xml ?? String.Empty));

            //stamper.AcroFields.Xfa.FillXfaForm(stream);
            //stamper.Close();
        }

        #endregion

        private static bool Process00(DataTable dateTable, string pdfPath)
        {
            Stopwatch stopwatch = new Stopwatch();

            for (int i = 0; i < 2; i++)
            {
                stopwatch.Start();

                Console.WriteLine("" + i.ToString());
                PDFManagerV1.CreatePDF(dateTable, pdfPath);

                stopwatch.Stop();
                TimeSpan stopwatchElapsed = stopwatch.Elapsed;
                Console.WriteLine("\r\n Temps mis pour : \r\n - Récupération des de la selection \r\n - La génération du PDF \r\n - L'envoi de celui-ci par mail : " + Convert.ToInt32(stopwatchElapsed.TotalMilliseconds));

            }

            return true;
        }


        private static object _lockObj = new object();

        static void Main(string[] args)
        {
            //ManageXMLFile();

            #region --   --
            var authors = GetAuthors();
            string pdfPath = @"C:\Users\Sweet Family\Desktop\PdfFilesPath";

            var dateTable = DataTableHelper.ToDataTable<Author>(authors);


            //DoWork(dateTable);
            #endregion

            bool res = false;

            Task currentThread = new Task(() =>
            {
                Parallel.ForEach(authors, (action) =>
                {
                    //res = Process00(dateTable, pdfPath);
                    lock (_lockObj)
                    {
                        res = Process00(dateTable, pdfPath);
                    }
                });
            });

            currentThread.Start();
            currentThread.Wait();

           

            //XMLData();
            ;
            #region -- HIDE ---
            #region -- ************** XML **************  --
            //GetEmployesXml();
            #endregion

            ////GetData();
            ;


            #endregion

            #region -- ************** XML **************  --
            //GetEmployesXml();

            var thread = new Thread(() =>
            {
                Console.WriteLine("Starting download ...");
                var webClient = new HttpClient();
                var html = webClient.GetStringAsync("http://angelsix.com/download/solidwords-files");
                Console.WriteLine("Done download!");
            });
            thread.Start();

            thread.Join();
            Console.WriteLine("Tout est bien ...");

            for (int i = 0; i < 5; i++)
            {
                new Thread(() =>
                {
                    //var res0 = Manager.CreatePDFV2(dateTable, pdfPath, i);

                }).Start();
            }

            #endregion


            #region --- ********************************** ---

            var query = from row in dateTable.AsEnumerable()
                        group row by row.Field<string>("Name") into grp
                        select grp;

            var dataTableGroup = (from row in dateTable.AsEnumerable()
                                  group row by row.Field<string>("Name") into grp
                                  select grp.CopyToDataTable()).ToList();


            #region -- ************* --
            var grouped = from table in dateTable.AsEnumerable()
                          group table by new { placeCol = table["Name"] } into groupby
                          select new
                          {
                              Value = groupby.Key,
                              ColumnValues = groupby
                          };

            foreach (var key in grouped)
            {
                Console.WriteLine(key.Value.placeCol);
                Console.WriteLine("---------------------------");
                foreach (var columnValue in key.ColumnValues)
                {
                    Console.WriteLine(columnValue["Name"].ToString());
                }

                Console.WriteLine();
            }
            #endregion

            DataTable dtOutput = new DataTable();
            //foreach (var item in dataTableGroup)
            //{
            //    dtOutput.Rows.Add(item.Rows);
            //}

            var query3 = (from row in dateTable.AsEnumerable()
                          group row by row.Field<string>("Name")
                         into grp
                          select new
                          {
                              grp.Key,
                              //grp.Key.ProductID,
                              Quantity = grp.Sum(r => r.Field<int>("Age"))
                          }).ToList();


            var result = dateTable.AsEnumerable()
                .GroupBy(r => new { Col1 = r["Name"] })
                .Select(g =>
                {
                    var row = dateTable.NewRow();

                    row["Name"] = g.Min(r => r.Field<string>("Name"));
                    row["Age"] = g.Min(r => r.Field<int>("Age"));
                    row["IsMVP"] = g.Min(r => r.Field<bool>("IsMVP"));
                    row["BookTitle"] = g.Min(r => r.Field<string>("BookTitle"));
                    row["PublishedDate"] = g.Min(r => r.Field<DateTime>("PublishedDate"));
                    row["Age"] = g.Sum(r => r.Field<int>("Age"));

                    return row;
                }).CopyToDataTable();

            //LINQToDataTable<Author>(query);

            ;

            /*
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Id", typeof(int)),
                    new DataColumn("Name", typeof(string)),
                    new DataColumn("Country",typeof(string)) });
            dt.Rows.Add(1, "John Hammond", "United States");
            dt.Rows.Add(2, "Mudassar Khan", "India");
            dt.Rows.Add(3, "Suzanne Mathews", "France");
            dt.Rows.Add(4, "Robert Schidner", "Russia");

            var result = from rows in dt.AsEnumerable()
                         group rows by new
                         {
                             Name = rows["Name"],
                             Country = rows["Country"]
                         } into grp
                         select grp;

            List<DataTable> dts = new List<DataTable>();
            foreach (var item in result)
            {
                dts.Add(item.CopyToDataTable());
            }
            */
            ;
            #endregion


            //var dataSet = DataTableHelper.ConvertToDataSet<Author>(authors);

            string _imagePath = "https://ftp.mediaperf.com/img/logo.gif";
            //PDFManagerV1.CreatePdf4(pdfPath, _imagePath);

            ////  - ************************************* -
            //PDFManagerV1.ADDPdf(pdfPath);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //var res = Manager.CreatePDFV2(dateTable, pdfPath);

            //for (int i = 0; i < 15; i++)
            //{
            //    Task<int> ts = Task<int>.Run(() =>
            //    {
            //        var res0 = Manager.CreatePDFV2(dateTable, pdfPath, i);

            //        return 0;
            //    });

            //    Task.WhenAll(ts);
            //}


            stopwatch.Stop();
            TimeSpan stopwatchElapsed = stopwatch.Elapsed;
            Console.WriteLine("TEMPS MIS pour générer un PDF SYNC " + Convert.ToInt32(stopwatchElapsed.TotalMilliseconds));
            ;
            // 141314

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
            //#endregion
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

        #region -- ************************************** --
        private static void ManageXMLFile()
        {
            #region -- ********************************************* --
            XElement xEl = XElement.Load("..\\..\\Product.xml");
            var values = from page in xEl.Elements("Product")
                         select page.Element("Dp").Value;

            IEnumerable<XElement> accounts = xEl.Elements("Product");
            foreach (XElement item in accounts)
            {
                var v = item.Element("Dp").Value;
                var d = item.Element("Dp").Name;
                var e = item.Name;
                var a = item.Value;
                var i = item.NextNode;
                var o = item.LastNode;
                var u = item.DescendantNodes();
                var x = item.DescendantNodesAndSelf();
                var n = item.Descendants();
                var m = item.Attributes();

                foreach (var it in item.DescendantNodes())
                {

                }

                foreach (var it in item.Descendants())
                {

                }

                
            }
            //var xmlNodes = xEl.SelectNodes("Users/Account/User");

            //foreach (XmlNode node in xmlNodes)
            //{
            //    Console.WriteLine("Username: {0}; Password: {1}; Active:{2}; Account: {3}; Details Account: {4}",
            //                              node["Username"].InnerText,
            //                              node["Password"].InnerText,
            //                              node["Active"].InnerText,
            //                              node["Account"].InnerText,
            //                              node["Details"].InnerText);
            //}

            string t = "ByPhone";
            var val = from page in xEl.Elements("Product")
                    //where page.Element("Dp").Name == t
                      select page.Element("Dp").Name;


            string street = "7A Cox Street";
            var prods = from phoneno in xEl.Elements("Product")
                        //where phoneno.Element("Dp").Value == "ByMmail"
                        select new
                        {                             
                               Dp = phoneno.Element("Dp")
                        };

            IEnumerable<XElement> produits = xEl.Elements();
            Console.WriteLine("List of all Employee Names along with their ID:");
            foreach (var produit in produits)
            {
                //Console.WriteLine("{0} ",
                //    produit.Element("Product").Value);
                //produit.Element("Street").Value,
                //produit.Element("Zip").Value,
                //produit.Element("Country").Value,
                //produit.Element("City").Value);
            }

            var products = from phoneno in xEl.Elements("Product")
                            //where (string)phoneno.Element("Street").Attribute("Type") == "Home"
                            select phoneno;
            Console.WriteLine("List products Nos....");
            foreach (XElement xEle in products)
            {
                Console.WriteLine(xEle.Element("Street").Value);
            }


            XElement xElement = XElement.Load("..\\..\\Employees.xml");
            var homePhone = from phoneno in xElement.Elements("Employee")
                            where (string)phoneno.Element("Phone").Attribute("Type") == "Home"
                            select phoneno;
            Console.WriteLine("List HomePhone Nos.");
            foreach (XElement xEle in homePhone)
            {
                Console.WriteLine(xEle.Element("Phone").Value);
            }

            IEnumerable<XElement> employees = xElement.Elements();
            Console.WriteLine("List of all Employee Names along with their ID:");
            foreach (var employee in employees)
            {
                Console.WriteLine("{0} has Employee ID {1}",
                employee.Element("Name").Value,
                employee.Element("EmpId").Value);
            }
            #endregion

            #region -- ************************************* --
            //File.WriteAllText("Test.xml", @"<Root>  
            //                    <Child1>1</Child1>  
            //                    <Child2>2</Child2>  
            //                    <Child3>3</Child3>  
            //                </Root>");

            //Console.WriteLine("Querying tree loaded with XDocument.Load");
            //Console.WriteLine("-----------------------------------------");
            //XDocument doc = XDocument.Load("Test.xml");
            //IEnumerable<XElement> childList =
            //    from el in doc.Elements()
            //    select el;
            //foreach (XElement e in childList)
            //    Console.WriteLine(e);
            #endregion

            XElement xelement = XElement.Load("..\\..\\Product.xml");
            string xmlPath = @"C:\Users\Sweet Family\source\repos\ConsoleAppGeneratePDFFile\Contacts.xml";
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(xmlPath);  // use the .Load() method - not .LoadXml() !!

            // get a list of all <Contact> nodes
            XmlNodeList listOfContacts = xmldoc.SelectNodes("/Contacts/Contact");

            // iterate over the <Contact> nodes
            foreach (XmlNode singleContact in listOfContacts)
            {
                // get the Profiles/Personal subnode
                XmlNode personalNode = singleContact.SelectSingleNode("Profiles/Personal");

                // get the values from the <Personal> node
                if (personalNode != null)
                {
                    string firstName = personalNode.SelectSingleNode("FirstName").InnerText;
                    string lastName = personalNode.SelectSingleNode("LastName").InnerText;
                }

                // get the <Email> nodes
                XmlNodeList emailNodes = singleContact.SelectNodes("Emails/Email");

                foreach (XmlNode emailNode in emailNodes)
                {
                    string emailTyp = emailNode.SelectSingleNode("EmailType").InnerText;
                    string emailAddress = emailNode.SelectSingleNode("Address").InnerText;
                }
            }
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

                // Create a renderer for PDF that uses Unicode font12Bold encoding
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

        #region --  --
        
        public async Task MultipleSpResultsWithDapper()
        {
            // Act
            using (var conn = new SqlConnection("Data Source=YourDatabase"))
            {
                await conn.OpenAsync();
                var result = await conn.QueryMultipleAsync(
                    "YourStoredProcedureName",
                    new { param1 = 1, param2 = 2 },
                    null, null, CommandType.StoredProcedure);

                // read as IEnumerable<dynamic>
                var table1 = await result.ReadAsync();
                var table2 = await result.ReadAsync();

                //// read as typed IEnumerable
                //var table3 = await result.ReadAsync<Table1>();
                //var table4 = await result.ReadAsync<Table2>();

                ////Assert
                //Assert.IsNotEmpty(table1);
                //Assert.IsNotEmpty(table2);
                //Assert.IsNotEmpty(table3);
                //Assert.IsNotEmpty(table4);
            }
        }

        public static void UseDapper()
        {
            using (IDbConnection db = new SqlConnection("Server=myServer;Trusted_Connection=true"))
            {
                db.Open();
                var result = db.Query<string>("SELECT 'Hello World'").Single();
                Console.WriteLine(result);
            }
        }
        #endregion

        #region --- TASK ---
        static void UseTask()
        {
            Task task = new Task(() =>
            {
                //Parallel.ForEach(datas, pdfData =>
                //{
                //    var labOrderInvoices = GetLabOrderInvoices(trans.Practices.fPracticeID, pdfData.TaxInvoiceNumber);
                //    CreateLabOrderInvoice(PopulateHTML(labOrderInvoices), pdfData.TaxInvoiceNumber);

                //    Console.WriteLine("Processing {0} on thread {1}", pdfData.TaxInvoiceNumber,
                //                Thread.CurrentThread.ManagedThreadId);
                //});
            });
            task.Start();
            task.Wait();
        }

        static void CreateLabOrderInvoice(string html, string invoiceNumber)
        {
            try
            {
                string strHtml = null;
                MemoryStream memStream = new MemoryStream();

                strHtml = html;

                string strFileShortName = invoiceNumber + ".pdf";
                string strFileName = @"~\Invoices\" + strFileShortName;
                iTextSharp.text.Document docWorkingDocument = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 40, 40, 40, 40);
                StringReader srdDocToString = null;

                try
                {
                    PdfWriter pdfWrite = default(PdfWriter);

                    pdfWrite = PdfWriter.GetInstance(docWorkingDocument, new FileStream(strFileName, FileMode.Create));
                    srdDocToString = new StringReader(strHtml);

                    docWorkingDocument.Open();

                    Image logo = Image.GetInstance(@"~\images\Image_PPNLOGO.jpg");
                    logo.Alignment = Image.ALIGN_RIGHT;

                    docWorkingDocument.AddTitle("Lab Order Invoice");
                    docWorkingDocument.Add(logo);

                    //XMLWorkerHelper.GetInstance().ParseXHtml(pdfWrite, docWorkingDocument, srdDocToString);
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if ((docWorkingDocument != null))
                    {
                        docWorkingDocument.Close();
                    }
                    if ((srdDocToString != null))
                    {
                        srdDocToString.Close();
                        srdDocToString.Dispose();
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }


        #region -- XML --

        private static XElement GetElement(XDocument doc, string elementName)
        {
            foreach (XNode node in doc.DescendantNodes())
            {
                if (node is XElement)
                {
                    XElement element = (XElement)node;
                    if (element.Name.LocalName.Equals(elementName))
                        return element;
                }
            }
            return null;
        }

        public static void GetEmployesXml()
        {           
            XElement xelement = XElement.Load("..\\..\\Employees.xml");
            var employees = from nm in xelement.Elements("Employee")
                                 select nm;


            //var data = GetElement();

            //var emplDataTable = XmlParser.BuildDataTableFromXml("My_Table", "xelement");

            var employeeFemale = from nm in xelement.Elements("Employee")
                       where (string)nm.Element("Sex") == "Female"
                       select nm;

            Console.WriteLine("Details of Female Employees:");
            foreach (XElement xEle in employeeFemale)
                Console.WriteLine(xEle);

            var employeeMale = from nm in xelement.Elements("Employee")
                                 where (string)nm.Element("Sex") == "Male"
                               select nm;
            Console.WriteLine("Details of Female Employees:");
            foreach (XElement xEle in employeeMale)
                Console.WriteLine(xEle);

            var stCnt = from address in xelement.Elements("Employee")
                        where (string)address.Element("Address").Element("State") == "CA"
                        select address;
            Console.WriteLine("No of Employees living in CA State are {0}", stCnt.Count());

                        
            // http://www.colome.org/linq-xml-group-c-alternatives/
            

            //// -- Convert Xml to DataTable --
            //ToDataTable(xelement);

        }


        private static void DoWork(DataTable dataTable)
        {
            // https://www.c-sharpcorner.com/forums/creating-pdf-using-c-sharp-xml-and-itextsharp
            //https://stackoverflow.com/questions/19566237/how-can-convert-xml-to-pdf-using-itextsharp

            XElement xelement = XElement.Load("..\\..\\Employees.xml");
            //var employees = (from nm in xelement.Elements("Employee")
            //                select nm).ToList();

            ////Sample XML
            //var xml = employees;

            //File to write to
            var testFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "TEST.pdf");

            //Standard PDF creation, nothing special here
            using (var fs = new FileStream(testFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var doc0 = new Document(PageSize.A4, 5f, 5f, 5f, 5f);
                using (var doc = new Document())
                {
                    using (var writer = PdfWriter.GetInstance(doc, fs))
                    {
                        doc.Open();

                        #region --XML  Save !!! --
                        ////Count the columns
                        //var columnCount = xml.Root.Elements("Employee").First().Nodes().Count();

                        ////Create a table with one column for every child node of <cd>
                        //var t = new PdfPTable(columnCount);

                        ////Flag that the first row should be repeated on each page break
                        //t.HeaderRows = 1;

                        ////Loop through the first item to output column headers
                        //foreach (var N in xml.Root.Elements("cd").First().Elements())
                        //{
                        //    t.AddCell(N.Name.ToString());
                        //}

                        ////Loop through each CD row (this is so we can call complete later on)
                        //foreach (var CD in xml.Root.Elements())
                        //{
                        //    //Loop through each child of the current CD. Limit the number of children to our initial count just in case there are extra nodes.
                        //    foreach (var N in CD.Elements().Take(columnCount))
                        //    {
                        //        t.AddCell(N.Value);
                        //    }
                        //    //Just in case any rows have too few cells fill in any blanks
                        //    t.CompleteRow();
                        //}

                        ////Add the table to the document
                        //doc.Add(t);

                        #endregion

                        // https://www.mikesdotnetting.com/article/86/itextsharp-introducing-tables

                        #region -- Simulation Dynamic table --
                        PdfPTable table0 = new PdfPTable(4);

                        var font12Bold = new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD);
                        var font11Bold = new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD);
                        var font10Bold = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD);

                        var font12Normal = new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL);
                        var font11Normal = new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.NORMAL);
                        var font10Normal = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL);

                        #region -- Table Header --
                        PdfPCell cell0 = new PdfPCell(new Phrase("No"));
                        cell0.Colspan = 0;
                        cell0.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        cell0.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table0.AddCell(cell0);

                        cell0 = new PdfPCell(new Phrase("Campagne"));
                        cell0.Colspan = 0;
                        cell0.MinimumHeight = 30;
                        cell0.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell0.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table0.AddCell(cell0);

                        cell0 = new PdfPCell(new Phrase("Du"));
                        cell0.Colspan = 0;
                        cell0.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell0.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table0.AddCell(cell0);

                        cell0 = new PdfPCell(new Phrase("Au"));
                        cell0.Colspan = 0;
                        cell0.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell0.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table0.AddCell(cell0);
                        #endregion

                        #region -- First table group --
                        table0.AddCell("Col 1 Row 0");
                        table0.AddCell("Col 2 Row 0");
                        table0.AddCell("Col 3 Row 0");
                        table0.AddCell("Col 4 Row 0");

                        table0.AddCell("Col 1 Row 1");
                        table0.AddCell("Col 2 Row 1");
                        table0.AddCell("Col 3 Row 1");
                        table0.AddCell("Col 4 Row 1");

                        table0.AddCell("Col 1 Row 2");
                        table0.AddCell("Col 2 Row 2");
                        table0.AddCell("Col 3 Row 2");
                        table0.AddCell("Col 4 Row 2");

                        PdfPCell cell1 = new PdfPCell(new Phrase("First total row", font12Bold));
                        cell1.Colspan = 2;
                        cell1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table0.AddCell(cell1);

                        table0.AddCell("Col 3 Row 4");
                        table0.AddCell("Col 4 Row 4");
                        
                        cell1 = new PdfPCell(new Phrase("Second total row", font12Bold));
                        cell1.Colspan = 2;
                        cell1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table0.AddCell(cell1);

                        table0.AddCell("Col 3 Row 4");
                        table0.AddCell("Col 4 Row 4");
                        #endregion

                        #region -- Second table group --
                        PdfPCell productCell = new PdfPCell(new Phrase("Actishef\n   SubTitle", font12Bold));
                        //productCell = new PdfPCell(new Phrase("Actishef"));
                        productCell.Colspan = 4;
                        productCell.MinimumHeight = 23;
                        productCell.Padding = 5;
                        productCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table0.AddCell(productCell);

                        table0.AddCell("Col 1 Row 5");
                        table0.AddCell("Col 2 Row 5");
                        table0.AddCell("Col 3 Row 5");
                        table0.AddCell("Col 4 Row 5");

                        table0.AddCell("Col 1 Row 6");
                        table0.AddCell("Col 2 Row 6");
                        table0.AddCell("Col 3 Row 6");
                        table0.AddCell("Col 4 Row 6");

                        table0.AddCell("Col 1 Row 7");
                        table0.AddCell("Col 2 Row 7");
                        table0.AddCell("Col 3 Row 7");
                        table0.AddCell("Col 4 Row 7");

                        table0.AddCell("Col 1 Row 5");
                        table0.AddCell("Col 2 Row 5");
                        table0.AddCell("Col 3 Row 5");
                        table0.AddCell("Col 4 Row 5");

                        table0.AddCell("Col 1 Row 6");
                        table0.AddCell("Col 2 Row 6");
                        table0.AddCell("Col 3 Row 6");
                        table0.AddCell("Col 4 Row 6");

                        table0.AddCell("Col 1 Row 7");
                        table0.AddCell("Col 2 Row 7");
                        table0.AddCell("Col 3 Row 7");
                        table0.AddCell("Col 4 Row 7");

                        PdfPCell cell2 = new PdfPCell(new Phrase("First total row"));
                        cell2.Colspan = 2;
                        cell2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table0.AddCell(cell2);

                        table0.AddCell("Col 3 Row 4");
                        table0.AddCell("Col 4 Row 4");

                        cell1 = new PdfPCell(new Phrase("Second total row"));
                        cell1.Colspan = 2;
                        cell1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table0.AddCell(cell1);

                        table0.AddCell("Col 3 Row 4");
                        table0.AddCell("Col 4 Row 4");

                        productCell = new PdfPCell(new Phrase("Actishef\n   SubTitle"));
                        productCell.Colspan = 4;
                        productCell.MinimumHeight = 23;
                        productCell.Padding = 5;
                        productCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table0.AddCell(productCell);
                        #endregion

                        #region -- ************************ --
                        table0.AddCell("Col 1 Row 5");
                        table0.AddCell("Col 2 Row 5");
                        table0.AddCell("Col 3 Row 5");
                        table0.AddCell("Col 4 Row 5");

                        table0.AddCell("Col 1 Row 6");
                        table0.AddCell("Col 2 Row 6");
                        table0.AddCell("Col 3 Row 6");
                        table0.AddCell("Col 4 Row 6");

                        table0.AddCell("Col 1 Row 7");
                        table0.AddCell("Col 2 Row 7");
                        table0.AddCell("Col 3 Row 7");
                        table0.AddCell("Col 4 Row 7");

                        table0.AddCell("Col 1 Row 5");
                        table0.AddCell("Col 2 Row 5");
                        table0.AddCell("Col 3 Row 5");
                        table0.AddCell("Col 4 Row 5");

                        table0.AddCell("Col 1 Row 6");
                        table0.AddCell("Col 2 Row 6");
                        table0.AddCell("Col 3 Row 6");
                        table0.AddCell("Col 4 Row 6");

                        table0.AddCell("Col 1 Row 7");
                        table0.AddCell("Col 2 Row 7");
                        table0.AddCell("Col 3 Row 7");
                        table0.AddCell("Col 4 Row 7");

                        PdfPCell cell3 = new PdfPCell(new Phrase("First total row", font12Bold));
                        cell3.Colspan = 2;
                        cell3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table0.AddCell(cell3);

                        table0.AddCell("Col 3 Row 4");
                        table0.AddCell("Col 4 Row 4");

                        cell3 = new PdfPCell(new Phrase("Static headline" + Chunk.NEWLINE + "richTextBox1.Text", font12Bold));
                        cell3.Colspan = 2;
                        cell3.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table0.AddCell(cell3);

                        table0.AddCell("Col 3 Row 4");
                        table0.AddCell("Col 4 Row 4");
                        #endregion                        

                        doc.Add(table0);
                        
                        #endregion


                        #region --- Loop dataTable XML ---
                        PdfPTable tableColumns = new PdfPTable(5);

                        PdfPCell cellColumnHeader = null;

                        //tableColumns.SetWidthPercentage(90f);

                        List<string> columnNames = new List<string>();
                        /*
                        foreach (DataRow row in dataTable.Rows)
                        {
                            foreach (DataColumn column in dataTable.Columns)
                            {
                                var colName = column.ToString();

                                string cName = colName;
                                ;

                                #region MyRegion
                                if (true)
                                {
                                    #region -- Set Columns names --
                                    if (colName.Contains("Name"))
                                    {
                                        columnNames.Add(colName);
                                        cellColumnHeader = new PdfPCell(new Phrase(colName));
                                        cellColumnHeader.MinimumHeight = 30;
                                        cellColumnHeader.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                        cellColumnHeader.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        tableColumns.AddCell(cellColumnHeader);
                                    }

                                    if (colName.Contains("Age"))
                                    {
                                        cellColumnHeader = new PdfPCell(new Phrase(colName));
                                        cellColumnHeader.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        cellColumnHeader.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                        tableColumns.AddCell(cellColumnHeader);
                                    }

                                    if (colName.Contains("BookTitle"))
                                    {
                                        cellColumnHeader = new PdfPCell(new Phrase(colName));
                                        cellColumnHeader.Colspan = 0;
                                        cellColumnHeader.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                        cellColumnHeader.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        tableColumns.AddCell(cellColumnHeader);
                                    }

                                    if (colName.Contains("IsMVP"))
                                    {
                                        cellColumnHeader = new PdfPCell(new Phrase(colName));
                                        cellColumnHeader.Colspan = 0;
                                        cellColumnHeader.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                        cellColumnHeader.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        tableColumns.AddCell(cellColumnHeader);
                                    }

                                    if (colName.Contains("PublishedDate"))
                                    {
                                        cellColumnHeader = new PdfPCell(new Phrase(colName));
                                        cellColumnHeader.Colspan = 0;
                                        cellColumnHeader.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                        cellColumnHeader.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        tableColumns.AddCell(cellColumnHeader);
                                    }
                                    #endregion
                                }
                                #endregion

                                #region MyRegion
                                /*
                                // --  https://stackoverflow.com/questions/12916110/datatable-group-the-result-in-one-row --
                                var name = row.ItemArray[0].ToString();

                                var variableColumnNames = dataTable.Columns.Cast<DataColumn>()
                                            .Select(c => c.ColumnName)
                                            .Except(new[] { "Lastname", "Comment" });

                                var result10 = from r in dataTable.AsEnumerable()
                                                   //group r by new
                                                   //{
                                                   //    Name = r.Field<string>("Name")
                                                   //} into grp
                                               group r by r.Field<string>("Name") into grp
                                               select new
                                               {
                                                   Value = grp.Key,
                                                   //ColumnValues = grp

                                                   Values = variableColumnNames.ToDictionary(
                                                       columnName => columnName)//,
                                                       //columnName => grp.Count(r => r.Field<string>(columnName)))
                                               };


                                var result11 = from t1 in dataTable.AsEnumerable()
                                               group t1 by new
                                               {
                                                   Name = t1.Field<string>("Name")//
                                                   //LastName = t1.Field<String>("LastName"),
                                                   //Comment = t1.Field<String>("Comment"),
                                               } into grp
                                               select new
                                               {
                                                   grp.Key.Name,
                                                   //grp.Key.LastName,
                                                   //grp.Key.Comment,

                                                   Values = variableColumnNames.ToDictionary(
                                                       columnName => columnName,
                                                       columnName => grp.Max(r => r.Field<string>(columnName)))
                                               };

                                /*
                                var query = from r in dataTable.AsEnumerable()
                                            where r.Field<string>("Name") == name
                                            group r by r.Field<string>(name) into grp
                                            select grp;

                                var dataTableGroup = (from r in dataTable.AsEnumerable()
                                                      group r by r.Field<string>(name) into grp
                                                      select grp.CopyToDataTable()).ToList();


                                var grouped = from table in dataTable.AsEnumerable()
                                              group table by new { placeCol = table[name] } into groupby
                                              select new
                                              {
                                                  Value = groupby.Key,
                                                  ColumnValues = groupby
                                              };
                                */

                                /*
                                //if (name.Contains(cName))
                                //{
                                tableColumns.AddCell(row.ItemArray[0].ToString());
                                tableColumns.AddCell(row.ItemArray[1].ToString());
                                tableColumns.AddCell(row.ItemArray[2].ToString());
                                tableColumns.AddCell(row.ItemArray[3].ToString());
                                tableColumns.AddCell(row.ItemArray[4].ToString());
                                */
                                /*
                                #endregion
                            }
                        }

                        */

                        #region MyRegion
                        //for (int k = 0; k < dataTable.Columns.Count; k++)
                        //{
                        //    string colName = dataTable.Columns[k].ColumnName;

                        //    #region -- Set Columns names --
                        //    if (colName.Contains("Name"))
                        //    {
                        //        columnNames.Add(colName);
                        //        cellColumnHeader = new PdfPCell(new Phrase(colName));
                        //        cellColumnHeader.MinimumHeight = 30;
                        //        cellColumnHeader.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        //        cellColumnHeader.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //        tableColumns.AddCell(cellColumnHeader);
                        //    }

                        //    if (colName.Contains("Age"))
                        //    {
                        //        cellColumnHeader = new PdfPCell(new Phrase(colName));
                        //        cellColumnHeader.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //        cellColumnHeader.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        //        tableColumns.AddCell(cellColumnHeader);
                        //    }

                        //    if (colName.Contains("BookTitle"))
                        //    {
                        //        cellColumnHeader = new PdfPCell(new Phrase(colName));
                        //        cellColumnHeader.Colspan = 0;
                        //        cellColumnHeader.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        //        cellColumnHeader.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //        tableColumns.AddCell(cellColumnHeader);
                        //    }

                        //    if (colName.Contains("IsMVP"))
                        //    {
                        //        cellColumnHeader = new PdfPCell(new Phrase(colName));
                        //        cellColumnHeader.Colspan = 0;
                        //        cellColumnHeader.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        //        cellColumnHeader.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //        tableColumns.AddCell(cellColumnHeader);
                        //    }

                        //    if (colName.Contains("PublishedDate"))
                        //    {
                        //        cellColumnHeader = new PdfPCell(new Phrase(colName));
                        //        cellColumnHeader.Colspan = 0;
                        //        cellColumnHeader.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        //        cellColumnHeader.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //        tableColumns.AddCell(cellColumnHeader);
                        //    }
                        //    #endregion

                        //    //PdfPCell pdfColumnCell = new PdfPCell(new Phrase(dataTable.Columns[k].ColumnName));
                        //    //pdfColumnCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        //    //pdfColumnCell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                        //    //pdfColumnCell.BackgroundColor = new BaseColor(51, 102, 102);
                        //    //tableColumns.AddCell(pdfColumnCell);
                        //}

                        //string cellVal = null;
                        //for (int i = 0; i < dataTable.Rows.Count; i++)
                        //{
                        //    //JObject eachRowObj = new JObject();

                        //    for (int j = 0; j < dataTable.Columns.Count; j++)
                        //    {
                        //        #region --- !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! ---
                        //        var cellValue = dataTable.Rows[i][j].ToString();
                        //        cellVal = cellValue;
                        //        ;

                        //        var query = from row in dataTable.AsEnumerable()
                        //                    group row by row.Field<string>("Name") into grp
                        //                    select grp;  /*).ToList()*/

                        //        //var dat = query["Name"];

                        //        var grouped = (from table in dataTable.AsEnumerable()
                        //                       group table by new { placeCol = table["Name"] } into groupby
                        //                       select new
                        //                       {
                        //                           Value = groupby.Key,
                        //                           Sum = groupby.Count(),
                        //                           ColumnValues = groupby
                        //                       }).ToList();



                        //        var result = dataTable.AsEnumerable()
                        //                     .GroupBy(r => new { Col1 = r["Name"] })
                        //                     .Select(g =>
                        //                     {
                        //                         var row = dataTable.NewRow();

                        //                         row["Name"] = g.Min(r => r.Field<string>("Name"));
                        //                         row["Age"] = g.Min(r => r.Field<int>("Age"));
                        //                         row["IsMVP"] = g.Min(r => r.Field<bool>("IsMVP"));
                        //                         row["BookTitle"] = g.Min(r => r.Field<string>("BookTitle"));
                        //                         row["PublishedDate"] = g.Min(r => r.Field<DateTime>("PublishedDate"));
                        //                         row["Age"] = g.Sum(r => r.Field<int>("Age"));

                        //                         return row;
                        //                     }).CopyToDataTable(); 
                        //        #endregion

                        //        //eachRowObj.Add(key, value);

                        //        string key = Convert.ToString(dataTable.Columns[j]);
                        //        string value = Convert.ToString(dataTable.Rows[i].ItemArray[j]);

                        //        if (value.Contains(cellValue))
                        //        {
                        //            //tableColumns.AddCell(value);
                        //            //tableColumns.AddCell(row.ItemArray[1].ToString());
                        //            //tableColumns.AddCell(row.ItemArray[2].ToString());
                        //            //tableColumns.AddCell(row.ItemArray[3].ToString());
                        //            //tableColumns.AddCell(row.ItemArray[4].ToString());
                        //        }

                        //        PdfPCell pdfColumnCell = new PdfPCell(new Phrase(dataTable.Rows[i][j].ToString()));

                        //        //Align the pdfColumnCell in the center
                        //        pdfColumnCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        //        pdfColumnCell.VerticalAlignment = PdfPCell.ALIGN_CENTER;

                        //        //table.AddCell(pdfColumnCell);
                        //    }
                        //} 
                        #endregion
                        #endregion

                        float[] widths = new float[] { 20f, 20f, 50f, 20f, 30f };
                        tableColumns.SetWidths(widths);
                        //doc.Add(tableColumns);

                        doc.Close();

                        Process.Start(testFile);
                    }
                }
            }
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(XDocument element)
        {
            if (element != null)
            {
                DataSet ds = new DataSet();
                string rawXml = element.ToString();
                ds.ReadXml(new StringReader(rawXml));
                return ds.Tables[0];
            }
            else
                return null;
        }
        #endregion
        /*    https://stackoverflow.com/questions/5603284/linq-to-xml-groupby
         * 
         var data = from acct in chartData.Elements()
           select new {
               Summary = (string)acct.Element("Summary"),
               Comprehensive = (string)acct.Element("Comprehensive"),
               Currency = (string)acct.Element("Currency"),
               Balance = (decimal)acct.Element("Balance"),
           };

var grouped = from acct in data
              group acct by acct.Summary into g
              select new {
                  Summary = g.Key,
                  Comprehensive = g.First().Comprehensive,
                  Currency = g.First().Comprehensive,
                  Balance = g.Sum(),
              };

var groupData = new XElement("Root",
     from g in grouped
     select new XElement("Account",
             new XElement("Summary", g.Summary),
             new XElement("Comprehensive", g.Comprehensive),
             new XElement("Currency", g.Currency),
             new XElement("Balance", g.Balance.ToString("0.000000"))
         )
     );
         * 
         * */
        #endregion

        public static void GetData()
        {

            string query = @"SELECT TOP 1000 [PersonId]
                  ,[FirstName]
                  ,[Adress_Street]
                  ,[State]
                  ,[DateAdded]
                  ,[PersonTypeId]
                  ,[DateModified]
              FROM[JunkEFCodeFrist1].[dbo].[People]";

            query += @"SELECT TOP 1000 [CompagnyId]
                  ,[CompagnyName]
                  ,[Adress_Street]
                  ,[State]
                    FROM[JunkEFCodeFrist1].[dbo].[Compagnies]";

            //string connectionString = @"Data Source=DESKTOP-6DSO6AT\\SQLEXPRESS; Database=JunkEFCodeFrist1; Trusted_Connection=True";

            string connetionString = "Data Source=DESKTOP-6DSO6AT\\SQLEXPRESS; Database=JunkEFCodeFrist1; Trusted_Connection=True";

            //using (var conn = new SqlConnection(Helper.GetConnectionString()))
            using (var conn = new SqlConnection(connetionString))
            {
                using (var data = conn.QueryMultiple(query, null))
                {
                    //var totalRecords = data.Read<int>().Single();
                    var records = data.Read<dynamic>();
                    var record = data.Read<dynamic>();
                    //var remaining = totalRecords - records.Count();
                }
            }
        }


        #region -- ********************** --
        public DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            // column names
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }
        #endregion
    }

    public class Person
    {
        public int PersonId { get; set; }
        public string  FirstName { get; set; }
        public string Adress_Street { get; set; }
        public DateTime DateAdded { get; set; }
        public int  PersonTypeId { get; set; }
        public DateTime DateModified { get; set; }
    }

    #region -- CLASS --
	/// <summary>
    /// 
    /// </summary>
    public class DynamicMultilineTextbox : IPdfPCellEvent
    {
        private string fieldname;

        public DynamicMultilineTextbox(string name)
        {
            fieldname = name;
        }

        public void CellLayout(PdfPCell cell, Rectangle rectangle, PdfContentByte[] canvases)
        {
            Rectangle wreckTangle = new Rectangle(30, 60); // changed from 300, 600
            PdfWriter writer = canvases[0].PdfWriter;
            TextField text = new TextField(writer, wreckTangle, fieldname);
            PdfFormField field = text.GetTextField();
            writer.AddAnnotation(field);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DynamicTextbox : IPdfPCellEvent
    {
        private string fieldname;

        public DynamicTextbox(string name)
        {
            fieldname = name;
        }

        public void CellLayout(PdfPCell cell, Rectangle rectangle, PdfContentByte[] canvases)
        {
            PdfWriter writer = canvases[0].PdfWriter;
            TextField text = new TextField(writer, rectangle, fieldname);
            //Microsoft.SharePoint.WebControls.TextField text = new TextField(writer, rectangle, fieldname);
            PdfFormField field = text.GetTextField();
            writer.AddAnnotation(field);
        }
    }


    #region -- Using --
       /*
    PdfPCell cellNotesMultilineTextBox = new PdfPCell()
    {
        CellEvent = new DynamicTextbox("multilineTextboxNotes"),
        Phrase = new Phrase("I will be darned like a sock. I will be darned like a sock")
    };
    //tblMultilineTextAreas.AddCell(cellNotesMultilineTextBox);
    table0.AddCell(cellNotesMultilineTextBox);

                        Phrase blankPhrase = new Phrase();
    PdfPCell blankCell = new PdfPCell(blankPhrase);
    blankCell.BorderWidth = 0;
                        //tblMultilineTextAreas.AddCell(blankCell);
                        table0.AddCell(blankCell);

                        PdfPCell cellAccountCodesMultilineTextBox = new PdfPCell()
                        {
                            CellEvent = new DynamicTextbox("multilineTextboxAccountCodes"),
                            Phrase = new Phrase("I will be dammed like a reservoir")
                        };
    //tblMultilineTextAreas.AddCell(cellAccountCodesMultilineTextBox);
    table0.AddCell(cellAccountCodesMultilineTextBox);

                        Phrase blankPhrase2 = new Phrase();
    PdfPCell blankCell2 = new PdfPCell(blankPhrase2);
    blankCell2.BorderWidth = 0;
                        //tblMultilineTextAreas.AddCell(blankCell2);
                        table0.AddCell(blankCell2);

                        PdfPCell cell1099TaxReportableMultilineTextBox = new PdfPCell()
                        {
                            CellEvent = new DynamicTextbox("multilineTextbox1099TaxReportable"),
                            Phrase = new Phrase("I will be the uncle of a monkey")
                        };
    //tblMultilineTextAreas.AddCell(cell1099TaxReportableMultilineTextBox);
    table0.AddCell(cell1099TaxReportableMultilineTextBox);
        */
    #endregion
    #endregion
}
