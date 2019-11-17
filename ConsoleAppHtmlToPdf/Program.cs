using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAppHtmlToPdf
{
    class Program
    {
        // https://www.supinfo.com/articles/single/6064-multithreading-in-c-partie1

        #region --   --

        private static DataTable dataTable = null;
        private static string pdfPath = @"C:\Users\Sweet Family\Desktop\PdfFilesPath\TestPDF";

        #endregion

        private static bool stop = false;

        static void Main(string[] args)
        {
            #region --   --
            var authors = GetAuthors();

            dataTable = DataTableHelper.ToDataTable<Author>(authors);


            //DoWork(dataTable);
            #endregion
            int numThreads = 2;

            #region --   --
            #region --- OK OK OK OK ---
            ////Thread letgoThread = new Thread(Do);

            ////// Commencer Thread (start thread).
            ////letgoThread.Start();

            ////// Dites au thread principal (voici main principal)
            ////// Attendez que letgoThread finisse, puis continuez à fonctionner.
            ////letgoThread.Join();            
            #endregion

            // -- Force le nbre de coeur --
            Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)3;

            // -- Retrouve et affiche le nbre de processeur d'une machine--
            var process = new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem").Get();
            foreach (var item in process)
            {
                Console.WriteLine("Number Of Physical Processors: {0} ", item["NumberOfProcessors"]);
            }

            bool res = false;
            Stopwatch stopwatch0 = new Stopwatch();
            stopwatch0.Start();
            Task currentThread = new Task(() =>
            {
                lock (_lockObj)
                {
                    res = Process00(dataTable, pdfPath);

                    Console.WriteLine("\r\n Processing {0} on thread {1}", "TaxInvoiceNumber",
                                Thread.CurrentThread.ManagedThreadId);
                }
            });

            currentThread.Start();
            currentThread.Wait();

            stopwatch0.Stop();
            TimeSpan stopwatchElapsed = stopwatch0.Elapsed;
            Console.WriteLine("\r\n Temps mis pour : " + Convert.ToInt32(stopwatchElapsed.TotalMilliseconds));

            #endregion


            #region -  -
            string htmlCode = "<p>This is a p tag</p>";

            Thread userThread = new Thread(new ThreadStart(doSomeWork));
            userThread.Start();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            stop = true;
            #endregion

            Console.WriteLine("The END !!!");
        }
               
        #region -- Methods --
        #region --  --


        #region --  MULTITHREADING --
        private static void Do()
        {
            Process00(dataTable, pdfPath);
        }

        // https://www.codemag.com/Article/1211071/Tasks-and-Parallelism-The-New-Wave-of-Multithreading
        // https://stackoverflow.com/questions/29847700/parallel-processing-to-create-pdfs
        // https://docs.microsoft.com/fr-fr/dotnet/standard/threading/managed-threading-best-practices
        private static bool Process00(DataTable dateTable, string pdfPath)
        {
            Stopwatch stopwatch = new Stopwatch();

            for (int i = 0; i < 5; i++)
            {
                stopwatch.Start();

                Console.WriteLine("" + i.ToString());
                PDFManagerV1.CreatePDF(dateTable, pdfPath, i);

                stopwatch.Stop();
                TimeSpan stopwatchElapsed = stopwatch.Elapsed;
                Console.WriteLine("\r\n Temps mis pour : \r\n - Récupération des de la selection \r\n - La génération du PDF \r\n - L'envoi de celui-ci par mail : " +
                    Convert.ToInt32(stopwatchElapsed.TotalMilliseconds));
            }

            return true;
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
                Document docWorkingDocument = new Document(PageSize.A4, 40, 40, 40, 40);
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
                catch (Exception ex)
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

        /// <summary>
        /// --  --
        /// </summary>
        private static void CREATEPDFFROMHTML()
        {
            Task task = new Task(() =>
            {
                //Parallel.ForEach(orders, currentOrder =>
                //{
                //    LiveTrainingEntities db = new LiveTrainingEntities();
                //    var trans = db.Transactions.SingleOrDefault(x => x.fTransactionID == currentOrder.TransactionID);

                //    if (trans != null)
                //    {
                //        var labOrderInvoices = GetLabOrderInvoices(trans.Practices.fPracticeID, currentOrder.TaxInvoiceNumber);
                //        CreateLabOrderInvoice(PopulateHTML(labOrderInvoices), currentOrder.TaxInvoiceNumber);

                //        Console.WriteLine("Processing {0} on thread {1}", currentOrder.TaxInvoiceNumber,
                //                    Thread.CurrentThread.ManagedThreadId);

                //        //orders.Remove(currentOrder);
                //    }
                //});
            });
            task.Start();
            task.Wait();
        }
               

        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            //Table start.
            string html = "<table cellpadding='5' cellspacing='0' style='border: 1px solid #ccc;font-size: 9pt'>";

            ////Adding HeaderRow.
            html += "<tr>";
            //foreach (DataGridViewColumn column in dataGridView1.Columns)
            //{
            //    html += "<th style='background-color: #B8DBFD;border: 1px solid #ccc'>" + column.HeaderText + "</th>";
            //}
            html += "</tr>";

            ////Adding DataRow.
            //foreach (DataGridViewRow row in dataGridView1.Rows)
            //{
            //    html += "<tr>";
            //    foreach (DataGridViewCell cell in row.Cells)
            //    {
            //        html += "<td style='width:120px;border: 1px solid #ccc'>" + cell.Value.ToString() + "</td>";
            //    }
            //    html += "</tr>";
            //}

            //Table end.
            html += "</table>";

            //Creating Folder for saving PDF.
            string folderPath = "C:\\PDFs\\";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            //Exporting HTML to PDF file.
            using (FileStream stream = new FileStream(folderPath + "DataGridViewExport.pdf", FileMode.Create))
            {
                Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                StringReader sr = new StringReader(html);
                //XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                stream.Close();
            }
        }
        #endregion
        private static object _lockObj = new object();

        #endregion

        private static void doSomeWork()
        {
            int value = 0;
            Console.WriteLine("Enter work of user Thread");
            while (!stop)
            {
                Console.WriteLine($"User Thread say : {value++}");
                Thread.Sleep(1000);
            }
            Console.WriteLine("End of user Thread");
        }

        #region -- Test 1 --
        public static Byte[] PdfSharpConvert(String html)
        {
            Byte[] res = null;
            using (MemoryStream ms = new MemoryStream())
            {
                var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4);
                pdf.Save(ms);
                res = ms.ToArray();
            }
            return res;
        }
        #endregion

        #endregion

        #region --   --
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

        #endregion
    }

    #region -- Author --
    public class Author : IAuthor
    {
        private string name;
        private int age;
        private string title;
        private bool mvp;
        private DateTime pubdate;

        public Author(string name, int age, string title, bool mvp, DateTime pubdate)
        {
            this.name = name;
            this.age = age;
            this.title = title;
            this.mvp = mvp;
            this.pubdate = pubdate;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Age
        {
            get { return age; }
            set { age = value; }
        }
        public string BookTitle
        {
            get { return title; }
            set { title = value; }
        }
        public bool IsMVP
        {
            get { return mvp; }
            set { mvp = value; }
        }
        public DateTime PublishedDate
        {
            get { return pubdate; }
            set { pubdate = value; }
        }
    }

    public interface IAuthor
    {
        int Age { get; set; }
        string BookTitle { get; set; }
        bool IsMVP { get; set; }
        string Name { get; set; }
        DateTime PublishedDate { get; set; }
    }
    #endregion
}
