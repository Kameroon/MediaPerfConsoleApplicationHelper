using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace ConsoleAppITextSharpPDF
{
    public class PdfRepositoryV1
    {

        public static string GetConnectionString()
        {
            string connetionString = "Data Source=DESKTOP-6DSO6AT\\SQLEXPRESS; Database=JunkEFCodeFrist1; Trusted_Connection=True";

            if (ConfigurationManager.ConnectionStrings["MyDbConnectionString"] == null)
            {
                return connetionString;
            }
            else
            {
                return ConfigurationManager.ConnectionStrings["MyDbConnectionString"].ConnectionString;
            }
        }


        public static void GetXmlDataFromDatatBase()
        {
            string SQL = "GetAllXMLDataObjects";
            SqlConnection con = new SqlConnection(GetConnectionString());
            SqlCommand com = new SqlCommand(SQL, con);
            try
            {
                con.Open();
                XmlReader reader = com.ExecuteXmlReader();
                while (reader.Read())
                {
                    Console.WriteLine(reader.Name);
                    if (reader.HasAttributes)
                    {
                        for (int i = 0; i < reader.AttributeCount; i++)
                        {
                            reader.MoveToAttribute(i);
                            Console.Write(reader.Name + ": " + reader.Value);
                        }
                        reader.MoveToElement();
                    }
                }
                reader.Close();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.ToString());
            }
            finally
            {
                con.Close();
            }
        }

        public static void TestPdfRepositoryV1()
        {
            GetXmlDataFromDatatBase();

            //Sample XML
            //var xml = CreateXml();

            #region -- ******************************* --
            // https://ehikioya.com/get-xml-document-nodes-recursively/

            XmlReader xmlFile;
            xmlFile = XmlReader.Create("..\\..\\Employees.xml", new XmlReaderSettings());
            DataSet ds = new DataSet();
            ds.ReadXml(xmlFile);
            int i = 0;
            for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                Console.WriteLine(" ===> " + ds.Tables[0].Rows[i].ItemArray[2].ToString());
            }

            // - Search in XML  --
            DataView dv;

            dv = new DataView(ds.Tables[0]);
            //dv.Sort = "Product_Name";
            //int index = dv.Find("Product2");


            #endregion

            XElement xelement = XElement.Load("..\\..\\Employees.xml");
            var employees = (from nm in xelement.Elements("Employee")
                             select nm).ToList();

            XDocument xml = XDocument.Load("..\\..\\Employees.xml");
            foreach (XElement element in xml.Descendants())
            {
                Console.WriteLine(element.Name);
            }           

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("..\\..\\Employees.xml");

            XmlNodeList xnList = xmlDocument.SelectNodes("/Employees/Employee");
            foreach (XmlNode xn in xnList)
            {
                string id = xn["EmpId"].InnerText;
                string lastName = xn["Name"].InnerText;
                string sex = xn["EmpId"].InnerText;
                string phone = xn["Phone"].InnerText;
                Console.WriteLine("Employee : {0} {1} {2} {3}", id, lastName, sex, phone);
            }

            //File to write to
            var testFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.pdf");

            //Standard PDF creation, nothing special here
            using (var fs = new FileStream(testFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (var doc = new Document())
                {
                    using (var writer = PdfWriter.GetInstance(doc, fs))
                    {
                        doc.Open();

                        //Count the columns
                        var columnCount = xml.Root.Elements("Employee").First().Nodes().Count();

                        //Create a table with one column for every child node of <cd>
                        var t = new PdfPTable(columnCount);

                        //Flag that the first row should be repeated on each page break
                        t.HeaderRows = 1;

                        //Loop through the first item to output column headers
                        foreach (var N in xml.Root.Elements("Employee").First().Elements())
                        {
                            t.AddCell(N.Name.ToString());
                        }

                        //Loop through each CD row (this is so we can call complete later on)
                        foreach (var CD in xml.Root.Elements())
                        {
                            //Loop through each child of the current CD. Limit the number of children to our initial count just in case there are extra nodes.
                            foreach (var N in CD.Elements().Take(columnCount))
                            {
                                t.AddCell(N.Value);
                            }
                            //Just in case any rows have too few cells fill in any blanks
                            t.CompleteRow();
                        }

                        //Add the table to the document
                        doc.Add(t);

                        doc.Close();

                        Process.Start(testFile);
                    }
                }
            }
        }

        #region -- Do Work --
        private static XDocument CreateXml()
        {
            //Create our sample XML document
            var xml = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));

            //Add our root node
            var root = new XElement("catalog");
            //All child nodes
            var nodeNames = new[] { "SR.No", "test", "code", "unit", "sampleid", "boreholeid", "pieceno" };
            XElement cd;

            //Create a bunch of <cd> items
            for (var i = 0; i < 1000; i++)
            {
                cd = new XElement("cd");
                foreach (var nn in nodeNames)
                {
                    cd.Add(new XElement(nn) { Value = String.Format("{0}:{1}", nn, i.ToString()) });
                }
                root.Add(cd);
            }

            xml.Add(root);

            return xml;
        }

        public static void DoWork()
        {
            //Sample XML
            var xml = CreateXml();

            //File to write to
            var testFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.pdf");

            //Standard PDF creation, nothing special here
            using (var fs = new FileStream(testFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (var doc = new Document())
                {
                    using (var writer = PdfWriter.GetInstance(doc, fs))
                    {
                        doc.Open();

                        //Count the columns
                        var columnCount = xml.Root.Elements("cd").First().Nodes().Count();

                        //Create a table with one column for every child node of <cd>
                        var t = new PdfPTable(columnCount);

                        //Flag that the first row should be repeated on each page break
                        t.HeaderRows = 1;

                        //Loop through the first item to output column headers
                        foreach (var N in xml.Root.Elements("cd").First().Elements())
                        {
                            t.AddCell(N.Name.ToString());
                        }

                        //Loop through each CD row (this is so we can call complete later on)
                        foreach (var CD in xml.Root.Elements())
                        {
                            //Loop through each child of the current CD. Limit the number of children to our initial count just in case there are extra nodes.
                            foreach (var N in CD.Elements().Take(columnCount))
                            {
                                t.AddCell(N.Value);
                            }
                            //Just in case any rows have too few cells fill in any blanks
                            t.CompleteRow();
                        }

                        //Add the table to the document
                        doc.Add(t);

                        doc.Close();

                        Process.Start(testFile);
                    }
                }
            }
        }

        #endregion

        public static void CreatePDFFile()
        {
            /*
            var data = new DataSet();
            data.ReadXml("report.xml");

            // Create empty document.
            var document = new DocumentModel();
            var section = new Section(document);
            document.Sections.Add(section);

            // Create a header content.
            var headerData = data.Tables["Header"];
            var header = new HeaderFooter(document, HeaderFooterType.HeaderDefault);
            foreach (DataColumn column in headerData.Columns)
            {
                //header.Blocks.Add(
                //    new Paragraph(document,
                //        new Run(document, column.ColumnName) { CharacterFormat = { Bold = true } },
                //        new SpecialCharacter(document, SpecialCharacterType.Tab),
                //        new Run(document, headerData.Rows[0][column].ToString()),
                //        new SpecialCharacter(document, SpecialCharacterType.LineBreak)));
            }
            section.HeadersFooters.Add(header);

            // Create table content.
            var tableData = data.Tables["Test"];
            var table = new Table(document,
                                    tableData.Rows.Count,
                                    tableData.Columns.Count,
                                    (row, column) =>
                                    {
                                        return new TableCell(document,
                                            new Paragraph(document, tableData.Rows[row][column].ToString()));
                                    });
            section.Blocks.Add(table);

            // Save as PDF.
            document.Save("report.pdf", SaveOptions.PdfDefault);
            */
        }
    }
}
