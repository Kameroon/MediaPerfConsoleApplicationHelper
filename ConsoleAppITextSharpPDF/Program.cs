using ConsoleAppITextSharpPDF.DataAccess;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleAppITextSharpPDF
{
    class Program
    {
        static void Main(string[] args)
        {
            //DoWork(null);

            #region --   --
            var authors = MyDataAccess.GetAuthors();
            string pdfPath = @"C:\Users\Sweet Family\Desktop\PdfFilesPath";

            var dataTable = DataTableHelper.ToDataTable<Author>(authors);
            var dt = DataTableHelper.LinqToDataTable(authors);


            // -- Create PDF XML --
            PdfRepositoryV1.TestPdfRepositoryV1();

            PdfRepositoryV1.DoWork();

            //TestLinqToDataTable();

            //var peoples = MyDataAccess.GetDataSync();

            var persons = MyDataAccess.GetSingleDataObjectSyncOK();

            //DoWork(dataTable);
            #endregion


            Console.WriteLine("");
        }


        #region --  --


        private static void TestLinqToDataTable()
        {
            #region -- ************************************* ---
            XElement xelement = XElement.Load("..\\..\\CustomEmployee.xml");
            var employees = (from nm in xelement.Elements("Permanent")
                             select nm).ToList();

            DataTable table = new DataTable("TestDataTable");
            string fileName = "..\\..\\CustomEmployee.xml";
            table.WriteXml(fileName, XmlWriteMode.WriteSchema);

            DataTable newTable = new DataTable();
            newTable.ReadXml(fileName);

            PrintValues(newTable, "New table");
            // -----------------------------------------------------

            string xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
                    <urlset>    
                        <url>
                            <loc>element1</loc>
                            <changefreq>daily</changefreq>
                            <priority>0.2</priority>
                        </url>
                        <url>
                            <loc>element2</loc>
                            <changefreq>daily</changefreq>
                            <priority>0.2</priority>
                        </url>
                    </urlset>";

            XDocument doc = XDocument.Parse(xml);
            List<string> urlList = doc.Root
                                      .Elements("url")
                                      .Elements("loc")
                                      .Select(x => (string)x)
                                      .ToList();
            Console.WriteLine(urlList.Count);


            StringWriter objSW = new StringWriter();
            DataTable oDt = new DataTable("DataTableName");//Your DataTable which you want to convert
            oDt.WriteXml(objSW);
            string result = objSW.ToString();

            XmlParser.BuildDataTableFromXml("DataTableName", xml);
            #endregion

            /*
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
            */
        }



        private static void DoWork(DataTable dataTable)
        {
            // https://www.c-sharpcorner.com/forums/creating-pdf-using-c-sharp-xml-and-itextsharp
            //https://stackoverflow.com/questions/19566237/how-can-convert-xml-to-pdf-using-itextsharp

            XElement xelement = XElement.Load("..\\..\\CustomEmployee.xml");
            var employees = (from nm in xelement.Elements("Permanent")
                             select nm).ToList();
            
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

                        #region --  --
                        DataTable table1 = new DataTable("Kunde");
                        table1.Columns.Add("Comment", typeof(String));
                        table1.Columns.Add("Name", typeof(String));
                        table1.Columns.Add("Lastname", typeof(String));

                        DataTable comment = new DataTable("Comment");
                        comment.Columns.Add("ID", typeof(String));
                        comment.Columns.Add("Comment", typeof(String));
                        comment.Columns.Add("Attribute", typeof(String));

                        DataSet ds = new DataSet("DataSet");
                        ds.Tables.Add(table1);
                        ds.Tables.Add(comment);

                        object[] o1 = { "hello", "kiki", "ha" };
                        object[] o2 = { "hi", "lola", "mi" };
                        object[] o3 = { "what", "ka", "xe" };
                        object[] c1 = { 1, "hello", "FF" };
                        object[] c2 = { 3, "hi", "AA" };
                        object[] c3 = { 2, "what", "UU" };
                        object[] c4 = { 2, "hello", "SS" };

                        table1.Rows.Add(o1);
                        table1.Rows.Add(o2);
                        table1.Rows.Add(o3);
                        comment.Rows.Add(c1);
                        comment.Rows.Add(c2);
                        comment.Rows.Add(c3);
                        comment.Rows.Add(c4);

                        var results = from tb1 in comment.AsEnumerable()
                                      join tb2 in table1.AsEnumerable()
                                      on tb1.Field<string>("Comment") equals tb2.Field<string>("Comment")
                                      select new
                                      {
                                          ID = tb1.Field<String>("ID"),
                                          Name = tb2.Field<String>("Name"),
                                          Lastname = tb2.Field<String>("Lastname"),
                                          Comment = tb1.Field<String>("Comment"),
                                          Attribute = tb1.Field<String>("Attribute"),
                                      };
                        DataTable result = DataTableHelper.LinqToDataTable(results);
                        var products = result.AsEnumerable()
                                            .GroupBy(c => c["ID"])
                                            .Where(g => !(g.Key is DBNull))
                                            .Select(g => (string)g.Key)
                                            .ToList();
                        var newtable = result.Copy();
                        products.ForEach(p => newtable.Columns.Add(p, typeof(string)));

                        foreach (var row in newtable.AsEnumerable())
                        {
                            if (!(row["ID"] is DBNull)) row[(string)row["ID"]] = row["Attribute"];
                        }
                        newtable.Columns.Remove("ID");
                        newtable.Columns.Remove("Attribute");

                        var result11 = from t1 in newtable.AsEnumerable()
                                       group t1 by new { Name = t1.Field<String>("Name"), LastName = t1.Field<String>("LastName"), Comment = t1.Field<String>("Comment"), } into grp
                                       select new
                                       {
                                           Name = grp.Key.Name,
                                           LastName = grp.Key.LastName,
                                           Comment = grp.Key.Comment,
                                           //Something here
                                       };

                        var variableColumnNames = newtable.Columns.Cast<DataColumn>()
                                .Select(c => c.ColumnName)
                                .Except(new[] { "Name", "Lastname", "Comment" });

                        var result10 = from t1 in newtable.AsEnumerable()
                                       group t1 by new
                                       {
                                           Name = t1.Field<String>("Name"),
                                           LastName = t1.Field<String>("LastName"),
                                           Comment = t1.Field<String>("Comment"),
                                       } into grp
                                       select new
                                       {
                                           grp.Key.Name,
                                           grp.Key.LastName,
                                           grp.Key.Comment,

                                           Values = variableColumnNames.ToDictionary(
                                               columnName => columnName,
                                               columnName => grp.Max(r => r.Field<String>(columnName)))
                                       };

                        //doc.Add(newtable);
                        #endregion

                        //float[] widths = new float[] { 20f, 20f, 50f, 20f, 30f };
                        //tableColumns.SetWidths(widths);
                        //doc.Add(tableColumns);

                        //doc.Close();

                        //Process.Start(testFile);
                    }
                }
            }
        }


        #endregion

        #region -- https://docs.microsoft.com/fr-fr/dotnet/api/system.data.datatable.readxml?view=netframework-4.8 --
        private static void DemonstrateReadWriteXMLDocumentWithString()
        {
            DataTable table = CreateTestTable("XmlDemo");
            PrintValues(table, "Original table");

            string fileName = "C:\\TestData.xml";
            table.WriteXml(fileName, XmlWriteMode.WriteSchema);

            DataTable newTable = new DataTable();
            newTable.ReadXml(fileName);

            // Print out values in the table.
            PrintValues(newTable, "New table");
        }

        private static DataTable CreateTestTable(string tableName)
        {
            // Create a test DataTable with two columns and a few rows.
            DataTable table = new DataTable(tableName);
            DataColumn column = new DataColumn("id", typeof(System.Int32));
            column.AutoIncrement = true;
            table.Columns.Add(column);

            column = new DataColumn("item", typeof(System.String));
            table.Columns.Add(column);

            // Add ten rows.
            DataRow row;
            for (int i = 0; i <= 9; i++)
            {
                row = table.NewRow();
                row["item"] = "item " + i;
                table.Rows.Add(row);
            }

            table.AcceptChanges();
            return table;
        }

        private static void PrintValues(DataTable table, string label)
        {
            Console.WriteLine(label);
            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn column in table.Columns)
                {
                    Console.Write("\t{0}", row[column]);
                }
                Console.WriteLine();
            }
        }
        #endregion
    }
}
