using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppHtmlToPdf
{
    public class PDFManagerV1
    {

        public static void CreatePDF(DataTable dataTable, string destinationPath, int value)
        {
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            try
            {
                string path = destinationPath + $"\\{value}_test.pdf";

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                
                #region -- ************* --

                Document document = new Document(PageSize.A4, 10, 10, 42, 35);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    string _imagePath = "https://ftp.mediaperf.com/img/logo.gif";

                    _imagePath = @"C:\Users\Sweet Family\Desktop\logo.jpg";

                    PdfWriter.GetInstance(document, fileStream);
                    document.Open();

                    #region --  --
                    document.AddAuthor("Micke Blomquist");
                    document.AddCreator("Sample application using iTextSharp");
                    document.AddKeywords("PDF tutorial education");
                    document.AddSubject("Document subject - Describing the steps creating a PDF document");
                    document.AddTitle("The document title - PDF creation using iTextSharp");
                    #endregion

                    //Document _document = new Document();
                    using (Document _document = new Document(PageSize.LETTER, 26, 16, 25, 10))
                    {
                        PdfWriter pdfWriter = PdfWriter.GetInstance(_document, fileStream);
                        _document.Open();

                        // -- Set the left header --
                        OnBeginPage(pdfWriter, _document);

                        // -- Set the rigth header --
                        OnRigthBeginPage(pdfWriter, _document);

                        ////InitalisePDFHelper(dataTable);
                        //SetImageLogo();

                        PdfPTable table = new PdfPTable(dataTable.Columns.Count)
                        {
                            WidthPercentage = 100
                        };

                        #region -- Set Logo --
                        Image imagePath = Image.GetInstance(_imagePath);
                        imagePath.ScalePercent(45f);
                        //imagePath.ScaleToFit(100f, 200f);
                        //imagePath.SetAbsolutePosition(250, 300);
                        _document.Add(imagePath);
                        #endregion

                        #region -- Set the left title --
                        var Title = new Paragraph("Relevé de redévences",
                                                new Font(Font.FontFamily.TIMES_ROMAN,
                                                18, Font.NORMAL,
                                                BaseColor.BLACK)
                                            );
                        Title.Alignment = Element.ALIGN_RIGHT;
                        _document.Add(Title);
                        #endregion

                        //_document.Add(new Paragraph("\n\n\r"));

                        Chunk verticalPositionMark = new Chunk(new VerticalPositionMark());

                        BaseFont baseFont = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, false);

                        #region ---  ---                                          

                        // -----------------------------

                        // -------------------------------------------

                        _document.Add(new Paragraph("\n\r"));
                        _document.Add(new Paragraph("\n\r"));

                        #region -- Set first ligne --
                        Font font = new Font(baseFont, 11, Font.BOLD, BaseColor.BLACK);
                        PdfPCell pdfPCell = new PdfPCell(new Phrase("N° RdR      $NoRdR", font))
                        {
                            VerticalAlignment = Element.ALIGN_MIDDLE,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            BorderWidth = 1
                        };

                        PdfPTable table0 = new PdfPTable(2);
                        PdfContentByte cb = pdfWriter.DirectContent;
                        table0 = new PdfPTable(1);
                        table0.TotalWidth = 160;
                        table0.AddCell(pdfPCell);
                        table0.WriteSelectedRows(0, -1, 35, 650, cb);

                        pdfPCell = new PdfPCell(new Phrase("Date       $Date", font))
                        {
                            VerticalAlignment = Element.ALIGN_MIDDLE,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            BorderWidth = 1
                        };

                        PdfPTable table10 = new PdfPTable(2);
                        table10 = new PdfPTable(1);
                        table10.TotalWidth = 160;
                        table10.AddCell(pdfPCell);
                        table10.WriteSelectedRows(0, -1, 220, 650, cb);

                        pdfPCell = new PdfPCell(new Phrase("Destinataire", font))
                        {
                            VerticalAlignment = Element.ALIGN_MIDDLE,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            BorderWidth = 1
                        };

                        PdfPTable table30 = new PdfPTable(2);
                        table30 = new PdfPTable(1);
                        table30.HorizontalAlignment = Element.ALIGN_CENTER;
                        table30.TotalWidth = 190;
                        //table30. = 65;
                        table10.AddCell(pdfPCell);
                        var p = new Paragraph
                        {
                            new Phrase("Destinataire", new Font(Font.FontFamily.TIMES_ROMAN, 20, Font.NORMAL, BaseColor.BLACK)),
                            "$Destinataire \n\n",
                            "$Prestataire \n\n",
                            "$Adresse$Prestataire \n\n\n"
                        };
                        pdfPCell.Border = 2;
                        table30.AddCell(p);
                        table30.WriteSelectedRows(0, -1, 400, 650, cb);

                        #endregion

                        _document.Add(new Paragraph("\n\r"));
                        // ----------------------------------------------

                        //var image = Image.GetInstance(_imagePath);
                        //var paragraph = new Paragraph();
                        //paragraph.Add(new Chunk(image, 0, 0));
                        //var tables = new PdfPTable(1);
                        //var cell = new PdfPCell { PaddingLeft = 5, PaddingTop = 5, PaddingBottom = 5, PaddingRight = 5 };
                        //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //cell.AddElement(paragraph);
                        //tables.AddCell(cell);

                        //_document.Add(new Paragraph("\n\r"));
                        //_document.Add(tables);

                        #endregion

                        #region --- Set images side by side !!!!!!!!! ---
                        //PdfPTable resimtable = new PdfPTable(2); // two colmns create tabble
                        //resimtable.WidthPercentage = 100f;//table %100 width
                        //Image imgsag = Image.GetInstance(_imagePath);
                        //resimtable.AddCell(imgsag);//Table One colmns added first image
                        //imgsag.ScalePercent(60f);

                        //Image imgsol = Image.GetInstance(_imagePath);
                        //imgsol.ScalePercent(60f);
                        //resimtable.AddCell(imgsag);//Table One colmns added first image
                        //resimtable.AddCell(imgsol);//Table two colmns added second image
                        //_document.Add(resimtable);
                        #endregion

                        #region -- Set Text Side by side --

                        //var docTitle = new Paragraph();
                        //var titleFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 10f, BaseColor.BLACK);
                        //docTitle.Font = titleFont;

                        //docTitle.Add("Simulation n° : " + 013235 + "\n");
                        //docTitle.Add(new Chunk("TS : " + "AZ5462132032003." + "\n", titleFont));
                        //docTitle.Add(new Chunk("Société : " + "CUST NAME" + "\n", titleFont));
                        //docTitle.Add(new Chunk("Date de simulation :  " + DateTime.Now.ToString() + "\n", titleFont));
                        //docTitle.Add(new Chunk("Montant total dû : " + 5265.20 + " €\n\n", titleFont));
                        ////docTitle.Font.Size = 6f;

                        //// -- Set Interline --
                        ////docTitle.SetLeading(1.8f, 1.2f);
                        //_document.Add(docTitle);

                        // -- Ajout de saut ligne --
                        _document.Add(new Paragraph("\n\n"));

                        //Chunk verticalPositionMark = new Chunk(new VerticalPositionMark());
                        Paragraph footerParagraph = new Paragraph("Text to the left");
                        var titleFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 10f, BaseColor.BLACK);
                        footerParagraph.Font = titleFont;
                        footerParagraph.Add(new Chunk(verticalPositionMark));
                        footerParagraph.Add("Suppose that some day, somebody asks you");

                        _document.Add(footerParagraph);

                        //// -- Set Interligne --
                        //footerParagraph.SetLeading(2.8f, 1.2f);
                        #endregion

                        _document.Add(new Paragraph("\n\r"));
                        VerticalPositionMark seperator = new LineSeparator();
                        seperator.Offset = -6f;

                        // -- Ajout de saut ligne --
                        _document.Add(new Paragraph("\n"));

                        #region --  Fill PDF DataGrid  --
                        ////Set columns names in the pdf file
                        for (int k = 0; k < dataTable.Columns.Count; k++)
                        {
                            PdfPCell pdfColumnCell = new PdfPCell(new Phrase(dataTable.Columns[k].ColumnName));

                            pdfColumnCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            pdfColumnCell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                            pdfColumnCell.BackgroundColor = new BaseColor(51, 102, 102);

                            table.AddCell(pdfColumnCell);
                        }

                        ////Add values of DataTable in pdf file
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            for (int j = 0; j < dataTable.Columns.Count; j++)
                            {
                                PdfPCell pdfColumnCell = new PdfPCell(new Phrase(dataTable.Rows[i][j].ToString()));

                                //Align the pdfColumnCell in the center
                                pdfColumnCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                pdfColumnCell.VerticalAlignment = PdfPCell.ALIGN_CENTER;

                                table.AddCell(pdfColumnCell);
                            }
                        }
                        #endregion

                        // -- Ajout de saut ligne --
                        _document.Add(new Paragraph("\n"));

                        System.Drawing.Font fonts = new System.Drawing.Font("Courier", 16);


                        //// -- Set Interligne --
                        //table.SetLeading(9.8f, 1.2f);

                        table.GetFooter();

                        //PdfPCell cell = new PdfPCell(new Phrase("Liste des machines déclarées par le client", new Font(Font.NORMAL, 9f, Font.NORMAL, BaseColor.WHITE)));
                        //cell.BackgroundColor = new BaseColor(223, 0, 27);    // -- RGB couleur --                        
                        //table.AddCell(cell);

                        var docFooter = new Paragraph();
                        var footerFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 10f, BaseColor.BLACK);
                        //docFooter.Font = titleFont;

                        //////// -- A supprimer -- **************************
                        //docFooter.Add("Simulation n° : \n");
                        //docFooter.Add(new Chunk("Montant total dû : "));
                        //docFooter.Alignment = Element.ALIGN_RIGHT;
                        //_document.Add(docFooter);

                        OnEndPage(pdfWriter, _document);

                        _document.Add(table);
                        _document.Close();
                        pdfWriter.Close();
                        fileStream.Close();

                        #region Display PDF
                        //Process.Start(path);
                        #endregion
                    }
                }
                #endregion
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                throw;
            }
        }

        public static void OnEndPage(PdfWriter writer, Document document)
        {
            PdfContentByte cb = writer.DirectContent;
            ColumnText ct = new ColumnText(cb);

            List<string> footerArray = new List<string> {
                "24 quai Gallieni - 92156 SURESNES CEDEX \n",
                "Tél 01 40 99 21 21 - Fax 01 40 99 80 30 \n",
                "Société par action simplifié au capital de 555.112.61€uro \n",
                "R.C. natèrre B 332 403 997 - TVA Intra FR1133240397 \n",
                "Madiaperformances" };

            cb.BeginText();
            cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 12.0f);
            cb.SetTextMatrix(document.LeftMargin, document.BottomMargin);
            cb.ShowText("24 quai Gallieni - 92156 SURESNES CEDEX \n\n");
            cb.ShowText("Tél 01 40 99 21 21 - Fax 01 40 99 80 30");
            cb.ShowText("Société par action simplifié au capital de 555.112.61€uro");
            cb.ShowText("R.C. natèrre B 332 403 997 - TVA Intra FR1133240397 \n");
            cb.ShowText("Madiaperformances");

            //var de = footerArray.Count;
            //for (int i = 1; i < footerArray.Count; i++)
            //{
            //    cb.ShowText(footerArray[i]);
            //}
            //cb.ShowText(String.Format("{0} {1}", "Testing Text", "Like this"));
            cb.EndText();
        }

        public static void OnBeginPage(PdfWriter writer, Document document)
        {
            PdfContentByte cb = writer.DirectContent;
            ColumnText ct = new ColumnText(cb);

            List<string> footerArray = new List<string> {
                "24 quai Gallieni - 92156 SURESNES CEDEX \n",
                "Tél 01 40 99 21 21 - Fax 01 40 99 80 30 \n",
                "Société par action simplifié au capital de 555.112.61€uro \n",
                "R.C. natèrre B 332 403 997 - TVA Intra FR1133240397 \n",
                "Madiaperformances" };

            cb.BeginText();
            cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 12.0f);
            cb.SetTextMatrix(document.LeftMargin, document.PageSize.Height - document.TopMargin);
            cb.ShowText("24 quai Gallieni - 92156");
            cb.EndText();
        }

        public static void OnRigthBeginPage(PdfWriter writer, Document document)
        {
            PdfContentByte cb = writer.DirectContent;
            ColumnText ct = new ColumnText(cb);

            ////define the text and style
            //Font font = new Font(Font.FontFamily.COURIER, 12, Font.NORMAL, BaseColor.MAGENTA);
            //Chunk c = new Chunk("Address 00000000000000000\n\r", font);
            //c.SetTextRenderMode(PdfContentByte.TEXT_RENDER_MODE_FILL, 0, BaseColor.PINK);
            //Phrase LongText = new Phrase(c);

            //Rectangle rect2 = new Rectangle(150f, 50f, 350f, 200f);
            //ColumnText column2 = new ColumnText(Canvas);
            //column2.SetSimpleColumn(rect2);
            //column2.SetText(LongText);
            //column2.Alignment = Element.ALIGN_CENTER;
            //column2.Go();

            cb.BeginText();
            cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 12.0f);
            cb.SetTextMatrix(document.RightMargin, document.PageSize.Height - document.TopMargin);
            //cb.ShowText("SURESNES CEDEX \n");
            //cb.ShowText(LongText.ToString());
            cb.EndText();
        }

        #region MyRegion
        // --  https://www.aspforums.net/Threads/778493/Place-Align-two-tables-side-by-side-using-iTextSharp-in-C-and-VBNet/

        public static void ADDPdf(string path)
        {
            string filePath = path + "\\TESTPDF1.pdf";
            string imgUrl = @"C:\Users\Sweet Family\Desktop\logo.jpg";

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (Document _document = new Document(PageSize.LETTER, 26, 16, 25, 10))
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                PdfWriter pdfWriter = PdfWriter.GetInstance(_document, fileStream);
                _document.Open();

                PdfContentByte content = pdfWriter.DirectContent;
                PdfPTable mtable = new PdfPTable(3);
                mtable.WidthPercentage = 100;
                mtable.DefaultCell.Border = Rectangle.NO_BORDER;

                // -- OK 1 --
                //PdfPTable table = new PdfPTable(2);
                //table.WidthPercentage = 50;
                //PdfPCell cell = new PdfPCell(new Phrase("Assesssment 1 Term 2"));
                //cell.Colspan = 2;
                //cell.HorizontalAlignment = 1;
                //table.AddCell(cell);
                //table.AddCell("Strength");
                //table.AddCell("Lowest %");

                // -- 2 image --
                PdfPTable table = new PdfPTable(2);
                table.WidthPercentage = 80;
                PdfPCell cell = new PdfPCell(new Phrase("Assesssment"));
                Image myImage = Image.GetInstance(imgUrl);
                cell.HorizontalAlignment = 1;
                //PdfPCell cell = new PdfPCell(myImage);
                //table.AddCell(myImage);


                PdfPCell points = new PdfPCell(new Phrase());
                //points.Colspan = 2;
                points.Border = 0;
                //.PaddingTop = 40f;
                //points.HorizontalAlignment = 1;//0=Left, 1=Centre, 2=Right

                // add a image
                Image jpg = Image.GetInstance(imgUrl);
                PdfPCell imageCell = new PdfPCell(jpg);
                //imageCell.Colspan = 2; // either 1 if you need to insert one cell
                imageCell.Border = 0;
                //imageCell.SetHorizontalAlignment(Element.ALIGN_CENTER);


                table.AddCell(points);
                // add a image
                table.AddCell(imageCell);

                mtable.AddCell(table);

                table = new PdfPTable(1);
                table.WidthPercentage = 80;
                //cell = new PdfPCell(new Phrase("Assesssment 1 Term 2"));
                //cell.Colspan = 2;
                //cell.HorizontalAlignment = 1;
                //table.AddCell(cell);
                //table.AddCell("Strength");
                //table.AddCell("Lowest %");

                mtable.AddCell(table);

                // -- 3 --
                table = new PdfPTable(3);
                table.WidthPercentage = 80;
                cell = new PdfPCell(new Phrase("Assesssment",
                                    new Font(Font.FontFamily.TIMES_ROMAN,
                                    22, Font.NORMAL,
                                    BaseColor.BLACK))
                               );
                cell.Colspan = 2;
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);
                table.AddCell("Strength");
                table.AddCell("Lowest %");
                table.AddCell("AZRTTY %");
                table.AddCell("AZRTTY %");

                mtable.AddCell(table);

                _document.Add(mtable);
                _document.Close();

                Process.Start(filePath);
            }
        }
        #endregion

        #region -  !!!!!!!!!! Else !!!!!!!! -
        //private static void CreatePdf4(string pdfFilename, string heading, string text, string[] photos, string emoticon)
        public static void CreatePdf4(string pdfFilename, string img)
        {
            Document document = new Document(PageSize.A4.Rotate(), 26, 36, 0, 0);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(pdfFilename + "\\TEATETE.pdf", FileMode.Create));
            document.Open();

            // Heading
            //Paragraph pHeading = new Paragraph(new Chunk(heading, FontFactory.GetFont(FontFactory.HELVETICA, 54, Font.NORMAL)));
            Paragraph pHeading = new Paragraph(new Chunk("heading", FontFactory.GetFont(FontFactory.HELVETICA, 54, Font.NORMAL)));

            // Photo 1
            Image img1 = Image.GetInstance(img);
            img1.ScaleAbsolute(350, 261);
            img1.SetAbsolutePosition(46, 220);
            img1.Alignment = Image.TEXTWRAP;

            // Photo 2
            Image img2 = Image.GetInstance(img);
            img2.ScaleAbsolute(350, 261);
            img2.SetAbsolutePosition(438, 220);
            img2.Alignment = Image.TEXTWRAP;

            // Text

            PdfContentByte cb = writer.DirectContent;
            cb.BeginText();
            cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false), 18);
            cb.SetTextMatrix(46, 175);
            cb.ShowText("00000000000000000text0000000000000000000000000000");
            cb.EndText();

            // Photo 3
            Image img3 = Image.GetInstance(img);
            img3.ScaleAbsolute(113, 153);
            img3.SetAbsolutePosition(556, 38);

            // Emoticon
            Image imgEmo = Image.GetInstance(img);
            imgEmo.ScaleToFit(80, 80);
            imgEmo.SetAbsolutePosition(692, 70);

            document.Add(pHeading);
            document.Add(img1);
            document.Add(img2);
            document.Add(img3);
            document.Add(imgEmo);

            document.Close();
        }
        #endregion
    }
}
