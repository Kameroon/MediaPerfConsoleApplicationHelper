using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppGeneratePDFFile
{
    public class PDFHelper
    {

        public static void CreatePDF(DataTable dataTable, string destinationPath)
        {
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            try
            {
                Document myDocument = new Document(PageSize.A4, 10, 10, 42, 35);
                string path = destinationPath + "\\TEST.pdf";
                //PdfWriter.GetInstance(myDocument, new FileStream(path, FileMode.Create));
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    PdfWriter.GetInstance(myDocument, fileStream);

                    myDocument.Open();

                    Document document = new Document();
                    PdfWriter writer = PdfWriter.GetInstance(document, fileStream);
                    document.Open();

                    System.Drawing.Font font = new System.Drawing.Font("Courier", 16);

                    int countFile = Directory.GetFiles(destinationPath).Length;
                    string strFileName = $"JobDescriptionFile{(countFile + 1)}.pdf";
                    
                    PdfPTable table = new PdfPTable(dataTable.Columns.Count);
                    table.WidthPercentage = 100;

                    //Set columns names in the pdf file
                    for (int k = 0; k < dataTable.Columns.Count; k++)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(dataTable.Columns[k].ColumnName));

                        cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(51, 102, 102);

                        table.AddCell(cell);
                    }

                    //Add values of DataTable in pdf file
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        for (int j = 0; j < dataTable.Columns.Count; j++)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(dataTable.Rows[i][j].ToString()));

                            //Align the cell in the center
                            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;

                            table.AddCell(cell);
                        }
                    }


                    Chunk chunk = new Chunk("DEMO ENTREPRISE",
                        FontFactory.GetFont("Times New Roman"));
                    chunk.Font.Color = new BaseColor(0, 0, 0);
                    chunk.Font.SetStyle(0);
                    chunk.Font.Size = 14;

                    document.Add(table);
                    document.Close();
                }

                #region  myDocument.Open();
                /*
                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(path, FileMode.Create));
                document.Open();

                PdfPTable table = new PdfPTable(dataTable.Columns.Count);
                table.WidthPercentage = 100;

                //Set columns names in the pdf file
                for (int k = 0; k < dataTable.Columns.Count; k++)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(dataTable.Columns[k].ColumnName));

                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(51, 102, 102);

                    table.AddCell(cell);
                }

                //Add values of DataTable in pdf file
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(dataTable.Rows[i][j].ToString()));

                        //Align the cell in the center
                        cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;

                        table.AddCell(cell);
                    }
                }

                document.Add(table);
                document.Close();
                */
                #endregion
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                throw;
            }
        }

        private static bool IsValidPdf(string filepath)
        {
            bool Ret = true;

            PdfReader reader = null;

            try
            {
                reader = new PdfReader(filepath);
            }
            catch
            {
                Ret = false;
            }

            return Ret;
        }


        /*
        private void CreatePDF()
        {
            string fileName = string.Empty;

            DateTime fileCreationDatetime = DateTime.Now;

            fileName = string.Format("{0}.pdf", fileCreationDatetime.ToString(@"yyyyMMdd") + "_" + fileCreationDatetime.ToString(@"HHmmss"));

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
                            Paragraph para = new Paragraph("Hello world. Checking Header Footer", new Font(Font.FontFamily.HELVETICA, 22));

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
        */

        public static void AddImage(int pageNumber)
        {
            if (pageNumber > 0)
            {
                string pdfTemplate = @"C:\Users\Sweet Family\Desktop\PdfFilesPath\Input.pdf";

                IsValidPdf(pdfTemplate);

                string newFile = @"C:\Users\Sweet Family\Desktop\PdfFilesPath\Output.pdf";
                PdfReader pdfReader = new PdfReader(pdfTemplate);

                //// Works correctly.
                //pdf.Position = Number.Zero;
                //var pdfReader = new PdfReader(pdf);

                PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(
                newFile, FileMode.Create));

                AcroFields pdfFormFields = pdfStamper.AcroFields;
                string chartLoc = string.Empty;

                chartLoc = @"C:\Users\Sweet Family\Desktop\imgDrole.png";

                Image chartImg = Image.GetInstance(chartLoc);
                PdfContentByte underContent;

                Rectangle rect;
                try
                {
                    Single X, Y; int pageCount = 0;
                    rect = pdfReader.GetPageSizeWithRotation(1);

                    if (chartImg.Width > rect.Width || chartImg.Height > rect.Height)
                    {
                        chartImg.ScaleToFit(rect.Width, rect.Height);
                        X = (rect.Width - chartImg.ScaledWidth) / 2;
                        Y = (rect.Height - chartImg.ScaledHeight) / 2;
                    }
                    else
                    {
                        X = (rect.Width - chartImg.Width) / 2;
                        Y = (rect.Height - chartImg.Height) / 2;
                    }
                    chartImg.SetAbsolutePosition(X, Y);

                    pageCount = pdfReader.NumberOfPages;
                    //Below to add image to all pages
                    //for (int i = 1; i < pageCount; i++)
                    //{

                    //    underContent = pdfStamper.GetOverContent(i);//.GetUnderContent(i);

                    //    underContent.AddImage(chartImg);

                    //}

                    if (pageCount >= pageNumber)
                    {
                        underContent = pdfStamper.GetOverContent(pageNumber);//.GetUnderContent(i);
                        underContent.AddImage(chartImg);
                    }

                    pdfStamper.Close();
                    pdfReader.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
