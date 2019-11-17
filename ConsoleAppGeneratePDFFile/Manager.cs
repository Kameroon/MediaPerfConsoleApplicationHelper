using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using iTextSharp.text.pdf.draw;
using System.Globalization;

namespace ConsoleAppGeneratePDFFile
{
    public class Manager
    {
        private static Report _report = new Report();

        public static string FilePath { get; set; } = "\\test.pdf";

        #region -- ************** !!!!!!!!!!!!!!!!!!! *************** --
        public static void GeneratePDFDocument(string path)
        {
            string imgUrl = @"C:\Users\Sweet Family\Desktop\logo.jpg";
            string filePath = path + "\\TESTPDF12.pdf";

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

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
            cell11.AddElement(new Paragraph("Thankyou for shoping at ABC traders,your order details are below", subTitleFont));
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


            Process.Start(filePath);
        }
                
        /// <summary>
        /// -- !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! --
        /// </summary>
        public static void GeneratePDFDocumentOOOOOOOO()
        {
            BaseFont bf = BaseFont.CreateFont("c:/windows/fonts/arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            Font fontRupee = new Font(bf, 8, Font.ITALIC);
            Font fontRupee1 = new Font(bf, 10, Font.BOLDITALIC);

            var Smallspace = FontFactory.GetFont("Calibri", 1, BaseColor.BLACK);
            var boldHeadFont = FontFactory.GetFont("Calibri", 13, BaseColor.RED);
            var boldTableFont = FontFactory.GetFont("Calibri", 11, BaseColor.BLACK);
            var TableFontSmall = FontFactory.GetFont("Calibri", 8, BaseColor.BLACK);

            var TableFontmini_ARBold8Sub = FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.BLACK);
            var TableFontmini_ARBoldCom = FontFactory.GetFont("Calibri", 16, Font.BOLD, BaseColor.BLACK);
            var TableFontmini_ARBoldComAdd = FontFactory.GetFont("Calibri", 10, Font.NORMAL, BaseColor.BLACK);
            var TableFontmini_ARBold82 = FontFactory.GetFont("Tahoma", 7, Font.BOLDITALIC, BaseColor.BLACK);
            var TableFontmini_ARBold81 = FontFactory.GetFont("Tahoma", 7, Font.BOLDITALIC, BaseColor.BLACK);
            var TableFontmini_Ver = FontFactory.GetFont("Arial", 7, Font.ITALIC, BaseColor.BLACK);
            var TableFontmini_VerBold = FontFactory.GetFont("Arial", 8, Font.BOLDITALIC, BaseColor.BLACK);
            var TableFontmini_ARBoldWef8 = FontFactory.GetFont("Calibri", 9, Font.BOLDITALIC, BaseColor.BLACK);
            var TableFontmini_ARBold8 = FontFactory.GetFont("Calibri", 8, Font.BOLDITALIC, BaseColor.BLACK);
            var TableFontmini_ARBold8Nor = FontFactory.GetFont("Arial", 8.5f, Font.ITALIC, BaseColor.BLACK);
            //var TableFontmini_ARBold8Nor = FontFactory.GetFont("Calibri", 7, Font.ITALIC, iTextSharp.text.Color.BLACK);
            var TableFontmini_ARBold8inc = FontFactory.GetFont("Calibri", 8.5f, Font.BOLDITALIC, BaseColor.BLACK);
            var TableFontmini_ARBoldRef = FontFactory.GetFont("Calibri", 9, Font.BOLDITALIC, BaseColor.BLACK);
            var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLDOBLIQUE, 10);
            var boldFont1 = FontFactory.GetFont(FontFactory.HELVETICA_BOLDOBLIQUE, 8, Font.UNDERLINE);
            var boldFontm = FontFactory.GetFont(FontFactory.TIMES_BOLDITALIC, 9);
            //var boldFontm = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            //var boldFontm= FontFactory.GetFont(FontFactory.TIMES_BOLD, 10, iTextSharp.text.Font.UNDERLINE);
            var TableFontmini_Ar = FontFactory.GetFont("Calibri", 8, BaseColor.BLACK);
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            Font times = new Font(bfTimes, 12, Font.ITALIC, BaseColor.BLACK);
            Font timessmall = new Font(bfTimes, 9, Font.ITALIC, BaseColor.BLACK);


            var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
            var boldFonts = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);

            var blackListTextFont = FontFactory.GetFont("Arial", 28, BaseColor.BLACK);
            var redListTextFont = FontFactory.GetFont("Arial", 28, BaseColor.RED);

            //rnPL.Id = Id.SelectedValue.Trim();
            //rnPL.Code = Code;
            //rnPL.CodeNo = CodeNo;

            //taSet ds = rnBL.GetDetilForPDF(rnPL);
            DataSet ds = new DataSet();
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables["tbl_Basic"];
                Document doc = new Document(PageSize.A4, 0, 0, 0, 0);
                // lblHidId.Value = dt.Rows[0]["Id"].ToString();

                //if (dt.Rows[0]["Id"].ToString() == "4")
                //{
                //    FilePath = Server.MapPath("images") + "\\1.jpg";
                //    FilePathstamplogo = Server.MapPath("images") + "\\6.png";
                //}

                //if (dt.Rows[0]["Id"].ToString() == "1")
                //{
                //    FilePath = Server.MapPath("images") + "\\2.jpg";
                //    FilePathslogo = Server.MapPath("images") + "\\5.png";
                //}

                //iTextSharp.text.Image stamplogo = iTextSharp.text.Image.GetInstance(FilePathstamplogo);
                //stamplogo.ScalePercent(75f);

                ////stamplogo.SetAbsolutePosition(doc.PageSize.Width - 36f - 140f, doc.PageSize.Height - 36f - 640f);/*ByAbhishek*/
                //stamplogo.SetAbsolutePosition(doc.PageSize.Width - 38f - 160f, doc.PageSize.Height - 38f - 700f);
                //doc.Add(stamplogo);


                string imgUrl = @"C:\Users\Sweet Family\Desktop\logo.jpg";

                Image jpg = Image.GetInstance(imgUrl);
                jpg.ScaleAbsoluteHeight(830);
                jpg.ScaleAbsoluteWidth(600);
                jpg.Alignment = Image.UNDERLYING;
                //fofile = "";
                //fofile = Server.MapPath("PDFComRNew");
                string crefilename = null;
                //crefilename = Convert.ToInt32(Code.ToString()).ToString() + Convert.ToInt32(CodeNo.ToString()).ToString() + ".Pdf";
                string newPathfile = Path.Combine("fofile", crefilename);
                PdfWriter pdfwrite = PdfWriter.GetInstance(doc, new FileStream(newPathfile, FileMode.Create));

                doc.Open();
                doc.Add(jpg);

                PdfPTable tableHeader = new PdfPTable(1);
                tableHeader.WidthPercentage = 50;
                PdfPCell Headspace;

                Headspace = new PdfPCell(new Phrase(" ", TableFontSmall));
                Headspace.BorderWidth = 0;
                Headspace.HorizontalAlignment = 0;/**Left=0,Centre=1,Right=2**/
                tableHeader.AddCell(Headspace);

                Headspace = new PdfPCell(new Phrase(" ", TableFontSmall));
                Headspace.BorderWidth = 0;
                Headspace.HorizontalAlignment = 0;/**Left=0,Centre=1,Right=2**/
                tableHeader.AddCell(Headspace);

                Headspace = new PdfPCell(new Phrase(" ", TableFontSmall));
                Headspace.BorderWidth = 0;
                Headspace.HorizontalAlignment = 0;/**Left=0,Centre=1,Right=2**/
                tableHeader.AddCell(Headspace);
                doc.Add(tableHeader);

                PdfPTable tblAcNo = new PdfPTable(1);
                float[] colWidthsaccingo = { 1000 };
                tblAcNo.SetWidths(colWidthsaccingo);
                PdfPCell celladdingo;

                celladdingo = new PdfPCell(new Phrase("  ", Smallspace));
                celladdingo.HorizontalAlignment = 1;
                celladdingo.BorderWidth = 0;
                celladdingo.Colspan = 2;
                tblAcNo.AddCell(celladdingo);
                celladdingo = new PdfPCell(new Phrase("  ", Smallspace));
                celladdingo.HorizontalAlignment = 1;
                celladdingo.BorderWidth = 0;
                celladdingo.Colspan = 2;
                tblAcNo.AddCell(celladdingo);
                celladdingo = new PdfPCell(new Phrase("  ", Smallspace));
                celladdingo.HorizontalAlignment = 1;
                celladdingo.BorderWidth = 0;
                celladdingo.Colspan = 2;
                tblAcNo.AddCell(celladdingo);
                celladdingo = new PdfPCell(new Phrase("  ", Smallspace));
                celladdingo.HorizontalAlignment = 1;
                celladdingo.BorderWidth = 0;
                celladdingo.Colspan = 2;
                tblAcNo.AddCell(celladdingo);
                celladdingo = new PdfPCell(new Phrase("  ", Smallspace));
                celladdingo.HorizontalAlignment = 1;
                celladdingo.BorderWidth = 0;
                celladdingo.Colspan = 2;
                tblAcNo.AddCell(celladdingo);
                celladdingo = new PdfPCell(new Phrase("  ", Smallspace));
                celladdingo.HorizontalAlignment = 1;
                celladdingo.BorderWidth = 0;
                celladdingo.Colspan = 2;
                tblAcNo.AddCell(celladdingo);
                celladdingo = new PdfPCell(new Phrase("  ", Smallspace));
                celladdingo.HorizontalAlignment = 1;
                celladdingo.BorderWidth = 0;
                celladdingo.Colspan = 2;
                tblAcNo.AddCell(celladdingo);

                celladdingo = new PdfPCell(new Phrase("  ", Smallspace));
                celladdingo.HorizontalAlignment = 1;
                celladdingo.BorderWidth = 0;
                celladdingo.Colspan = 2;
                tblAcNo.AddCell(celladdingo);

                celladdingo = new PdfPCell(new Phrase("  ", Smallspace));
                celladdingo.HorizontalAlignment = 1;
                celladdingo.BorderWidth = 0;
                celladdingo.Colspan = 2;
                tblAcNo.AddCell(celladdingo);


                celladdingo = new PdfPCell(new Phrase("  ", Smallspace));
                celladdingo.HorizontalAlignment = 1;
                celladdingo.BorderWidth = 0;
                celladdingo.Colspan = 2;
                tblAcNo.AddCell(celladdingo);

                celladdingo = new PdfPCell(new Phrase("  ", Smallspace));
                celladdingo.HorizontalAlignment = 1;
                celladdingo.BorderWidth = 0;
                celladdingo.Colspan = 2;
                tblAcNo.AddCell(celladdingo);

                celladdingo = new PdfPCell(new Phrase(" ", TableFontmini_ARBold8));
                celladdingo.HorizontalAlignment = 0;
                celladdingo.BorderWidth = 0;
                celladdingo.Colspan = 1;
                tblAcNo.AddCell(celladdingo);

                //Chunk c111 = new Chunk("Ref No : ", TableFontmini_ARBoldRef);
                //Chunk c211 = new Chunk((dt.Rows[0]["RefrenceNo"]).ToString(), TableFontmini_ARBold8Nor);
                //Phrase p211 = new Phrase();
                //p211.Add(c111);
                //p211.Add(c211);
                Paragraph pS = new Paragraph();
                //pS.Add(p211);
                /*For gst*/
                /*For space*/
                Chunk cspc = new Chunk("                                                                                                    ", TableFontmini_ARBold8);
                Phrase pcspc = new Phrase();
                pcspc.Add(cspc);
                pS.Add(pcspc);
                /*For space*/

                /*For statecode*/
                Chunk c1111 = new Chunk("Date : ", TableFontmini_ARBoldRef);
                Chunk c2111 = new Chunk((dt.Rows[0]["GenearteDate"]).ToString(), TableFontmini_ARBold8Nor);
                Phrase p2111 = new Phrase();
                p2111.Add(c1111);
                p2111.Add(c2111);

                pS.Add(p2111);
                /*For statecode*/

                /*For finally add*/
                PdfPCell cellDet_4 = new PdfPCell(pS);
                cellDet_4.HorizontalAlignment = 0; /**Left=0,Centre=1,Right=2**/
                cellDet_4.BorderWidth = 0;
                cellDet_4.Colspan = 2;
                tblAcNo.AddCell(cellDet_4);

                doc.Add(tblAcNo);



                PdfPTable tblto = new PdfPTable(1);
                float[] colWidthTo = { 1000 };
                tblto.SetWidths(colWidthTo);
                PdfPCell cellTo;

                cellTo = new PdfPCell(new Phrase("  ", Smallspace));
                cellTo.HorizontalAlignment = 1;
                cellTo.BorderWidth = 0;
                cellTo.Colspan = 2;
                tblto.AddCell(cellTo);

                cellTo = new PdfPCell(new Phrase("  ", Smallspace));
                cellTo.HorizontalAlignment = 1;
                cellTo.BorderWidth = 0;
                cellTo.Colspan = 2;
                tblto.AddCell(cellTo);

                cellTo = new PdfPCell(new Phrase("  ", Smallspace));
                cellTo.HorizontalAlignment = 1;
                cellTo.BorderWidth = 0;
                cellTo.Colspan = 2;
                tblto.AddCell(cellTo);

                cellTo = new PdfPCell(new Phrase("  ", Smallspace));
                cellTo.HorizontalAlignment = 1;
                cellTo.BorderWidth = 0;
                cellTo.Colspan = 2;
                tblto.AddCell(cellTo);

                cellTo = new PdfPCell(new Phrase("  ", Smallspace));
                cellTo.HorizontalAlignment = 1;
                cellTo.BorderWidth = 0;
                cellTo.Colspan = 2;
                tblto.AddCell(cellTo);

                cellTo = new PdfPCell(new Phrase("To, ", TableFontmini_ARBold8Nor));
                cellTo.HorizontalAlignment = 0;
                cellTo.BorderWidth = 0;
                cellTo.Colspan = 1;
                tblto.AddCell(cellTo);

                doc.Add(tblto);



                PdfPTable tblToManager = new PdfPTable(1);
                float[] colWidthToManager = { 1000 };
                tblToManager.SetWidths(colWidthToManager);
                PdfPCell cellToManager;

                cellToManager = new PdfPCell(new Phrase("  ", Smallspace));
                cellToManager.HorizontalAlignment = 1;
                cellToManager.BorderWidth = 0;
                cellToManager.Colspan = 2;
                tblToManager.AddCell(cellToManager);

                cellToManager = new PdfPCell(new Phrase(" ", TableFontmini_ARBold8Nor));
                cellToManager.HorizontalAlignment = 0;
                cellToManager.BorderWidth = 0;
                cellToManager.Colspan = 1;
                tblToManager.AddCell(cellToManager);
                doc.Add(tblToManager);

                PdfPTable tblBillHead = new PdfPTable(1);
                float[] colWidthBillHead = { 1000 };
                tblBillHead.SetWidths(colWidthBillHead);

                PdfPCell celltblBillHead = new PdfPCell(new Paragraph(dt.Rows[0]["Header"].ToString(), TableFontmini_ARBold8));
                celltblBillHead.HorizontalAlignment = 0;
                celltblBillHead.BorderWidth = 0;
                celltblBillHead.Colspan = 1;
                tblBillHead.AddCell(celltblBillHead);

                doc.Add(tblBillHead);

                PdfPTable tblSiteAdd = new PdfPTable(1);
                float[] colWidthSiteAdd = { 1000 };
                tblSiteAdd.SetWidths(colWidthSiteAdd);

                PdfPCell celltblSiteAdd = new PdfPCell(new Paragraph(dt.Rows[0]["Address"].ToString(), TableFontmini_ARBold8Nor));
                celltblSiteAdd.HorizontalAlignment = 0;
                celltblSiteAdd.BorderWidth = 0;
                celltblSiteAdd.Colspan = 1;
                tblSiteAdd.AddCell(celltblSiteAdd);

                doc.Add(tblSiteAdd);

                PdfPTable tblSiteCity = new PdfPTable(1);
                float[] colWidthSiteCity = { 1000 };
                tblSiteCity.SetWidths(colWidthSiteCity);

                PdfPCell celltblSiteCity = new PdfPCell(new Paragraph(dt.Rows[0]["City"].ToString(), TableFontmini_ARBold8));
                celltblSiteCity.HorizontalAlignment = 0;
                celltblSiteCity.BorderWidth = 0;
                celltblSiteCity.Colspan = 1;
                tblSiteCity.AddCell(celltblSiteCity);
                doc.Add(tblSiteCity);



                PdfPTable tblSubject = new PdfPTable(1);
                float[] colWidthSubject = { 1000 };
                tblSubject.SetWidths(colWidthSubject);
                PdfPCell cellSubject;

                cellSubject = new PdfPCell(new Phrase("  ", Smallspace));
                cellSubject.HorizontalAlignment = 1;
                cellSubject.BorderWidth = 0;
                cellSubject.Colspan = 2;
                tblSubject.AddCell(cellSubject);

                cellSubject = new PdfPCell(new Phrase("  ", Smallspace));
                cellSubject.HorizontalAlignment = 1;
                cellSubject.BorderWidth = 0;
                cellSubject.Colspan = 2;
                tblSubject.AddCell(cellSubject);


                cellSubject = new PdfPCell(new Phrase("  ", Smallspace));
                cellSubject.HorizontalAlignment = 1;
                cellSubject.BorderWidth = 0;
                cellSubject.Colspan = 2;
                tblSubject.AddCell(cellSubject);

                cellSubject = new PdfPCell(new Phrase("  ", Smallspace));
                cellSubject.HorizontalAlignment = 1;
                cellSubject.BorderWidth = 0;
                cellSubject.Colspan = 2;
                tblSubject.AddCell(cellSubject);

                cellSubject = new PdfPCell(new Phrase("   Sub.: Application For leave", TableFontmini_ARBold8Sub));
                cellSubject.HorizontalAlignment = 1;/**Left=0,Centre=1,Right=2**/
                cellSubject.BorderWidth = 0;
                cellSubject.Colspan = 1;
                tblSubject.AddCell(cellSubject);

                doc.Add(tblSubject);



                PdfPTable tblDEarSir = new PdfPTable(1);
                float[] colWidthDEarSir = { 1000 };
                tblDEarSir.SetWidths(colWidthDEarSir);
                PdfPCell cellDEarSir;

                cellDEarSir = new PdfPCell(new Phrase("  ", Smallspace));
                cellDEarSir.HorizontalAlignment = 1;
                cellDEarSir.BorderWidth = 0;
                cellDEarSir.Colspan = 2;
                tblDEarSir.AddCell(cellDEarSir);

                cellDEarSir = new PdfPCell(new Phrase("  ", Smallspace));
                cellDEarSir.HorizontalAlignment = 1;
                cellDEarSir.BorderWidth = 0;
                cellDEarSir.Colspan = 2;
                tblDEarSir.AddCell(cellDEarSir);

                cellDEarSir = new PdfPCell(new Phrase("  ", Smallspace));
                cellDEarSir.HorizontalAlignment = 1;
                cellDEarSir.BorderWidth = 0;
                cellDEarSir.Colspan = 2;
                tblDEarSir.AddCell(cellDEarSir);

                cellDEarSir = new PdfPCell(new Phrase("Dear Sir, ", TableFontmini_ARBold8));
                cellDEarSir.HorizontalAlignment = 0;
                cellDEarSir.BorderWidth = 0;
                cellDEarSir.Colspan = 1;
                tblDEarSir.AddCell(cellDEarSir);
                doc.Add(tblDEarSir);

                PdfPTable tblPara1 = new PdfPTable(1);
                float[] colWidthPara1 = { 1200 };
                tblPara1.SetWidths(colWidthPara1);
                PdfPCell cellPara1;

                cellPara1 = new PdfPCell(new Phrase("  ", Smallspace));
                cellPara1.HorizontalAlignment = 1;
                cellPara1.BorderWidth = 0;
                cellPara1.Colspan = 4;
                tblPara1.AddCell(cellPara1);

                cellPara1 = new PdfPCell(new Phrase("  ", Smallspace));
                cellPara1.HorizontalAlignment = 1;
                cellPara1.BorderWidth = 0;
                cellPara1.Colspan = 4;
                tblPara1.AddCell(cellPara1);

                cellPara1 = new PdfPCell(new Phrase("  ", Smallspace));
                cellPara1.HorizontalAlignment = 1;
                cellPara1.BorderWidth = 0;
                cellPara1.Colspan = 4;
                tblPara1.AddCell(cellPara1);

                cellPara1 = new PdfPCell(new Paragraph("row beg to say that row m feelling unwell", TableFontmini_ARBold8Nor));
                cellPara1.HorizontalAlignment = 3;
                cellPara1.BorderWidth = 0;
                cellPara1.Colspan = 1;
                tblPara1.AddCell(cellPara1);
                doc.Add(tblPara1);

                PdfPTable tblPara2 = new PdfPTable(1);
                float[] colWidthPara2 = { 1400 };
                tblPara2.SetWidths(colWidthPara2);
                PdfPCell cellPara2;

                cellPara2 = new PdfPCell(new Phrase("  ", Smallspace));
                cellPara2.HorizontalAlignment = 1;
                cellPara2.BorderWidth = 0;
                cellPara2.Colspan = 4;
                tblPara2.AddCell(cellPara2);
                cellPara2 = new PdfPCell(new Phrase("  ", Smallspace));
                cellPara2.HorizontalAlignment = 1;
                cellPara2.BorderWidth = 0;
                cellPara2.Colspan = 4;
                tblPara2.AddCell(cellPara2);

                cellPara2 = new PdfPCell(new Paragraph("Kindly give me leave for four days ", TableFontmini_ARBold8Nor));
                cellPara2.HorizontalAlignment = 3;
                cellPara2.BorderWidth = 0;
                cellPara2.Colspan = 1;

                tblPara2.AddCell(cellPara2);

                doc.Add(tblPara2);

                PdfPTable tblPara3 = new PdfPTable(1);
                float[] colWidthPara3 = { 1200 };
                tblPara3.SetWidths(colWidthPara3);
                PdfPCell cellPara3;
                cellPara3 = new PdfPCell(new Phrase("  ", Smallspace));
                cellPara3.HorizontalAlignment = 1;
                cellPara3.BorderWidth = 0;
                cellPara3.Colspan = 4;
                tblPara3.AddCell(cellPara3);

                cellPara3 = new PdfPCell(new Paragraph(" from Date" + dt.Rows[0]["Date"].ToString(), TableFontmini_ARBold8Nor));
                cellPara3.HorizontalAlignment = 3;
                cellPara3.BorderWidth = 0;
                cellPara3.Colspan = 1;
                tblPara3.AddCell(cellPara3);
                doc.Add(tblPara3);

                PdfPTable tblLastPara = new PdfPTable(1);
                float[] colWidthLastPara = { 1200 };
                tblPara1.SetWidths(colWidthLastPara);
                PdfPCell cellLastPara;

                cellLastPara = new PdfPCell(new Phrase("  ", Smallspace));
                cellLastPara.HorizontalAlignment = 1;
                cellLastPara.BorderWidth = 0;
                cellLastPara.Colspan = 2;
                tblLastPara.AddCell(cellLastPara);

                cellLastPara = new PdfPCell(new Phrase("  ", Smallspace));
                cellLastPara.HorizontalAlignment = 1;
                cellLastPara.BorderWidth = 0;
                cellLastPara.Colspan = 2;
                tblLastPara.AddCell(cellLastPara);

                cellLastPara = new PdfPCell(new Phrase("  ", Smallspace));
                cellLastPara.HorizontalAlignment = 1;
                cellLastPara.BorderWidth = 0;
                cellLastPara.Colspan = 2;
                tblLastPara.AddCell(cellLastPara);

                cellLastPara = new PdfPCell(new Paragraph("Thank you so much for giving me leave", TableFontmini_ARBold8Nor));
                cellLastPara.HorizontalAlignment = 3;
                cellLastPara.BorderWidth = 0;
                cellLastPara.Colspan = 1;
                tblLastPara.AddCell(cellLastPara);
                doc.Add(tblLastPara);

                PdfPTable tblThankingYou = new PdfPTable(1);
                float[] colWidthThankingYou = { 1000 };
                tblSiteCity.SetWidths(colWidthSiteCity);
                PdfPCell celltblThankingYou;

                celltblThankingYou = new PdfPCell(new Phrase("  ", Smallspace));
                celltblThankingYou.HorizontalAlignment = 1;
                celltblThankingYou.BorderWidth = 0;
                celltblThankingYou.Colspan = 2;
                tblThankingYou.AddCell(celltblThankingYou);

                celltblThankingYou = new PdfPCell(new Phrase("  ", Smallspace));
                celltblThankingYou.HorizontalAlignment = 1;
                celltblThankingYou.BorderWidth = 0;
                celltblThankingYou.Colspan = 2;
                tblThankingYou.AddCell(celltblThankingYou);

                celltblThankingYou = new PdfPCell(new Phrase("  ", Smallspace));
                celltblThankingYou.HorizontalAlignment = 1;
                celltblThankingYou.BorderWidth = 0;
                celltblThankingYou.Colspan = 2;
                tblThankingYou.AddCell(celltblThankingYou);

                celltblThankingYou = new PdfPCell(new Paragraph("Thanking You,", TableFontmini_ARBold8Nor));
                celltblThankingYou.HorizontalAlignment = 0;
                celltblThankingYou.BorderWidth = 0;
                celltblThankingYou.Colspan = 1;
                tblThankingYou.AddCell(celltblThankingYou);
                doc.Add(tblThankingYou);

                PdfPTable tblYorsSinc = new PdfPTable(1);
                float[] colWidthYorsSinc = { 1000 };
                tblYorsSinc.SetWidths(colWidthYorsSinc);
                PdfPCell cellYorsSinc;

                cellYorsSinc = new PdfPCell(new Phrase("  ", Smallspace));
                cellYorsSinc.HorizontalAlignment = 1;
                cellYorsSinc.BorderWidth = 0;
                cellYorsSinc.Colspan = 2;
                tblYorsSinc.AddCell(cellYorsSinc);

                cellYorsSinc = new PdfPCell(new Paragraph("Sincerely Yours,", TableFontmini_ARBold8Nor));
                cellYorsSinc.HorizontalAlignment = 0;
                cellYorsSinc.BorderWidth = 0;
                cellYorsSinc.Colspan = 1;
                tblYorsSinc.AddCell(cellYorsSinc);
                doc.Add(tblYorsSinc);

                PdfPTable tblAuthSignat = new PdfPTable(1);
                float[] colWidthAuthSignat = { 1000 };
                tblAuthSignat.SetWidths(colWidthAuthSignat);
                PdfPCell cellAuthSignat;

                cellAuthSignat = new PdfPCell(new Phrase("  ", Smallspace));
                cellAuthSignat.HorizontalAlignment = 1;
                cellAuthSignat.BorderWidth = 0;
                cellAuthSignat.Colspan = 2;
                tblAuthSignat.AddCell(cellAuthSignat);
                cellAuthSignat = new PdfPCell(new Phrase("  ", Smallspace));
                cellAuthSignat.HorizontalAlignment = 1;
                cellAuthSignat.BorderWidth = 0;
                cellAuthSignat.Colspan = 2;
                tblAuthSignat.AddCell(cellAuthSignat);

                cellAuthSignat = new PdfPCell(new Phrase("  ", Smallspace));
                cellAuthSignat.HorizontalAlignment = 1;
                cellAuthSignat.BorderWidth = 0;
                cellAuthSignat.Colspan = 2;
                tblAuthSignat.AddCell(cellAuthSignat);

                cellAuthSignat = new PdfPCell(new Phrase("  ", Smallspace));
                cellAuthSignat.HorizontalAlignment = 1;
                cellAuthSignat.BorderWidth = 0;
                cellAuthSignat.Colspan = 2;
                tblAuthSignat.AddCell(cellAuthSignat);

                cellAuthSignat = new PdfPCell(new Phrase("  ", Smallspace));
                cellAuthSignat.HorizontalAlignment = 1;
                cellAuthSignat.BorderWidth = 0;
                cellAuthSignat.Colspan = 2;
                tblAuthSignat.AddCell(cellAuthSignat);

                cellAuthSignat = new PdfPCell(new Phrase("  ", Smallspace));
                cellAuthSignat.HorizontalAlignment = 1;
                cellAuthSignat.BorderWidth = 0;
                cellAuthSignat.Colspan = 2;
                tblAuthSignat.AddCell(cellAuthSignat);

                cellAuthSignat = new PdfPCell(new Phrase("  ", Smallspace));
                cellAuthSignat.HorizontalAlignment = 1;
                cellAuthSignat.BorderWidth = 0;
                cellAuthSignat.Colspan = 2;
                tblAuthSignat.AddCell(cellAuthSignat);

                cellAuthSignat = new PdfPCell(new Phrase("  ", Smallspace));
                cellAuthSignat.HorizontalAlignment = 1;
                cellAuthSignat.BorderWidth = 0;
                cellAuthSignat.Colspan = 2;
                tblAuthSignat.AddCell(cellAuthSignat);

                cellAuthSignat = new PdfPCell(new Phrase("  ", Smallspace));
                cellAuthSignat.HorizontalAlignment = 1;
                cellAuthSignat.BorderWidth = 0;
                cellAuthSignat.Colspan = 2;
                tblAuthSignat.AddCell(cellAuthSignat);

                cellAuthSignat = new PdfPCell(new Phrase("  ", Smallspace));
                cellAuthSignat.HorizontalAlignment = 1;
                cellAuthSignat.BorderWidth = 0;
                cellAuthSignat.Colspan = 2;

                cellAuthSignat = new PdfPCell(new Phrase("  ", Smallspace));
                cellAuthSignat.HorizontalAlignment = 1;
                cellAuthSignat.BorderWidth = 0;
                cellAuthSignat.Colspan = 2;
                tblAuthSignat.AddCell(cellAuthSignat);
                tblAuthSignat.AddCell(cellAuthSignat);

                cellAuthSignat = new PdfPCell(new Paragraph("(Student Signatature)", TableFontmini_ARBold8));
                cellAuthSignat.HorizontalAlignment = 0;
                cellAuthSignat.BorderWidth = 0;
                cellAuthSignat.Colspan = 1;
                tblAuthSignat.AddCell(cellAuthSignat);
                doc.Add(tblAuthSignat);

                PdfPTable tblForCom = new PdfPTable(1);
                float[] colWidthForCom = { 1000 };
                tblYorsSinc.SetWidths(colWidthForCom);
                PdfPCell cellForCom;

                cellForCom = new PdfPCell(new Phrase("  ", Smallspace));
                cellForCom.HorizontalAlignment = 1;
                cellForCom.BorderWidth = 0;
                cellForCom.Colspan = 2;
                tblForCom.AddCell(cellForCom);

                cellForCom = new PdfPCell(new Paragraph("For  " + dt.Rows[0]["Name"].ToString(), TableFontmini_ARBold8));
                cellForCom.HorizontalAlignment = 0;
                cellForCom.BorderWidth = 0;
                cellForCom.Colspan = 1;
                tblForCom.AddCell(cellForCom);
                doc.Add(tblForCom);


                //pdfwrite.PageEvent = new FooterRN(dt.Rows[0]["Address"].ToString(), Convert.ToInt32(Code.ToString()).ToString(), dt.Rows[0]["Id"].ToString(), dt.Rows[0]["Studentmail"].ToString(), dt.Rows[0]["PhoneNo1"].ToString(), dt.Rows[0]["StudentName"].ToString());


                doc.Close();
            }

        }
        #endregion

        public static void CreatePDF(DataTable dataTable, string repositoryPath)
        {
            //PdfWriter masterWriter = null;
            string fileName = string.Empty;
            DateTime fileCreationDatetime = DateTime.Now;
            string imgPath = "https://ftp.mediaperf.com/img/logo.gif";
            imgPath = @"C:\Users\Sweet Family\Desktop\logo.jpg";

            //fileName = string.Format("{0}.pdf", fileCreationDatetime.ToString(@"yyyyMMdd") + "_" + fileCreationDatetime.ToString(@"HHmmss"));
            string fullPdfPath = repositoryPath + FilePath;

            if (File.Exists(fullPdfPath))
            {
                File.Delete(fullPdfPath);
            }

            try
            {
                #region MyRegion
                FileStream masterStream = new FileStream(fullPdfPath, FileMode.Create);
                using (Document masterDocument = new Document(PageSize.A4, 10, 10, 42, 10))
                using (PdfWriter masterWriter = PdfWriter.GetInstance(masterDocument, masterStream))
                {
                    masterDocument.Open();

                    Chunk verticalPositionMark = new Chunk(new VerticalPositionMark());
                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, false);
                    PdfContentByte masterContent = masterWriter.DirectContent;

                    PdfPTable gridTable = new PdfPTable(dataTable.Columns.Count)
                    {
                        WidthPercentage = 100
                    };

                    // -- Populate dynamycs feelds --
                    ConsolidateReport(dataTable);

                    #region -- Set Header --.
                    Image imagePath = Image.GetInstance(imgPath);
                    imagePath.ScalePercent(80f);

                    PdfPTable mtable = new PdfPTable(2);
                    mtable.WidthPercentage = 100;
                    mtable.DefaultCell.Border = Rectangle.NO_BORDER;

                    PdfPTable table = new PdfPTable(5);
                    table.WidthPercentage = 100;
                    PdfPCell cell = new PdfPCell(new Phrase("2"));
                    cell.Colspan = 6;
                    //cell.Rowspan = 2;
                    cell.HorizontalAlignment = 1;
                    Paragraph para = new Paragraph();
                    para.Add(new Chunk(imagePath, 5, -40));
                    cell.AddElement(para);
                    cell.BorderColor = BaseColor.WHITE;
                    table.AddCell(cell);
                    mtable.AddCell(table);

                    table = new PdfPTable(3);
                    table.WidthPercentage = 70;
                    cell = new PdfPCell(new Phrase("Relevé de redévences", new Font(Font.FontFamily.TIMES_ROMAN,
                                        20, Font.BOLD, BaseColor.BLACK)));
                    cell.Colspan = 2;
                    cell.Padding = 2;
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                    table.AddCell(new PdfPCell(new Phrase($"              Page \n\r              { _report.CurrentPage }/{ _report.TotalPageNumber }",
                                               new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL, BaseColor.BLACK))));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell = new PdfPCell(new Phrase("Ceci n'est pas une facture", new Font(Font.FontFamily.TIMES_ROMAN, 12,
                                                   Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = Element.ALIGN_CENTER;
                    cell.PaddingBottom = 2;
                    cell.Colspan = 3;
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);

                    mtable.AddCell(table);
                    masterDocument.Add(mtable);
                    #endregion

                    // -- Duplicata --
                    var docFooter = new Paragraph();
                    var footerFont = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 42f, BaseColor.LIGHT_GRAY);
                    docFooter.Font = footerFont;
                    docFooter.Add("\n\r");
                    docFooter.Add(new Chunk("DUPLICATA" /*, new Font(baseFont, 16, Font.BOLD, BaseColor.LIGHT_GRAY)*/));
                    docFooter.Alignment = Element.ALIGN_RIGHT;
                    masterDocument.Add(docFooter);


                    #region -- Set first line  --
                    Font font = new Font(baseFont, 11, Font.BOLD, BaseColor.BLACK);
                    PdfPCell pdfPCell = new PdfPCell(new Phrase($"N° RdR      { _report.RoyaltyFeeNumber }", font))
                    {
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        BorderWidth = 1,
                        Padding = 3
                    };

                    PdfPTable table0 = new PdfPTable(2);
                    PdfContentByte cb = masterWriter.DirectContent;
                    table0 = new PdfPTable(1);
                    table0.TotalWidth = 160;
                    table0.AddCell(pdfPCell);
                    table0.WriteSelectedRows(0, -1, 20, 675, cb);

                    pdfPCell = new PdfPCell(new Phrase($"Date       { _report.CurrentDate }", font))
                    {
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        BorderWidth = 1,
                        Padding = 3
                    };

                    PdfPTable table10 = new PdfPTable(2);
                    table10 = new PdfPTable(1);
                    table10.TotalWidth = 160;
                    table10.AddCell(pdfPCell);
                    table10.WriteSelectedRows(0, -1, 205, 675, cb);

                    pdfPCell = new PdfPCell(new Phrase("Destinataire", font))
                    {
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        BorderWidth = 1
                    };

                    /*
                    PdfPTable headerRightSubTable = new PdfPTable(2);
                    headerRightSubTable = new PdfPTable(1);
                    //headerRightSubTable.HorizontalAlignment = Element.ALIGN_CENTER;
                    headerRightSubTable.TotalWidth = 190;
                    headerRightTable.AddCell(pdfPCell);
                    var paragraph = new Paragraph
                        {
                            new Phrase("Destinataire \n\n", new Font(Font.FontFamily.TIMES_ROMAN, 
                                                        11, Font.BOLD, BaseColor.BLACK)),
                            $"{ _report.Destinataire } \n\n",
                            $"{ _report.Prestataire } \n\n",
                            $"{ _report.AdressePrestataire } \n\n\n"
                        };
                    pdfPCell.Border = 2;
                    pdfPCell.VerticalAlignment = Element.ALIGN_CENTER;
                    pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    headerRightSubTable.AddCell(paragraph);
                    headerRightSubTable.WriteSelectedRows(0, -1, 385, 650, cb);
                    */
                    PdfContentByte pdfContentByte30 = masterWriter.DirectContent;
                    PdfPTable table30 = new PdfPTable(1);
                    table30.TotalWidth = 190;

                    PdfPTable sTable30 = new PdfPTable(2);
                    pdfPCell = new PdfPCell();
                    Paragraph paragraph30 = new Paragraph
                        {
                            new Phrase("Destinataire", new Font(Font.FontFamily.TIMES_ROMAN,
                                                        11, Font.BOLD, BaseColor.BLACK))
                        };
                    pdfPCell.AddElement(paragraph30);
                    sTable30.AddCell(pdfPCell);
                    table30.AddCell(paragraph30);

                    pdfPCell = new PdfPCell();
                    paragraph30 = new Paragraph
                        {
                            new Phrase($" Nombre de Pv                          { _report.Destinataire } \n\n",
                                       new Font(Font.FontFamily.TIMES_ROMAN,
                                       11, Font.NORMAL, BaseColor.BLACK)),
                            new Phrase($" Nombre de Campagne             { _report.Prestataire } \n\n\n\n\n\n\n",
                                       new Font(Font.FontFamily.TIMES_ROMAN,
                                       11, Font.NORMAL, BaseColor.BLACK))
                        };
                    pdfPCell.AddElement(paragraph30);
                    sTable30.AddCell(pdfPCell);
                    table30.AddCell(paragraph30);
                    table30.WriteSelectedRows(0, -1, 385, 675, cb);
                    #endregion

                    #region -- Second ligne --
                    PdfContentByte pdfContentByte = masterWriter.DirectContent;
                    PdfPTable table100 = new PdfPTable(1);
                    table100.TotalWidth = 345f;

                    PdfPTable sTable = new PdfPTable(2);
                    pdfPCell = new PdfPCell();
                    Paragraph paragraph = new Paragraph
                        {
                            new Phrase($"{ _report.BfpParam1 } \n\n", new Font(Font.FontFamily.TIMES_ROMAN,
                                       11, Font.NORMAL, BaseColor.BLACK)),
                            new Phrase($"{ _report.BfpParam2 } \n\n", new Font(Font.FontFamily.TIMES_ROMAN,
                                       11, Font.NORMAL, BaseColor.BLACK))
                        };
                    pdfPCell.AddElement(paragraph);
                    sTable.AddCell(pdfPCell);
                    table100.AddCell(paragraph);

                    pdfPCell = new PdfPCell();
                    paragraph = new Paragraph
                        {
                            new Phrase($" Nombre de Pv                          { _report.CountPv } \n\n",
                                       new Font(Font.FontFamily.TIMES_ROMAN,
                                       11, Font.NORMAL, BaseColor.BLACK)),
                            new Phrase($" Nombre de Campagne             { _report.CountCampagne } \n\n",
                                       new Font(Font.FontFamily.TIMES_ROMAN,
                                       11, Font.NORMAL, BaseColor.BLACK))
                        };
                    pdfPCell.AddElement(paragraph);
                    sTable.AddCell(pdfPCell);
                    table100.AddCell(paragraph);
                    table100.WriteSelectedRows(0, -1, 20, 630, cb);
                    #endregion

                    masterDocument.Add(new Paragraph("\n\r"));
                    masterDocument.Add(new Paragraph("\n\r"));
                    masterDocument.Add(new Paragraph("\n\r"));
                    masterDocument.Add(new Paragraph("\n\r"));
                    masterDocument.Add(new Paragraph("\n\r"));
                    Paragraph ligneParagraphe = new Paragraph(new Chunk(new LineSeparator(0.0F, 100.0F,
                                      BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                    masterDocument.Add(ligneParagraphe);

                    #region -- Generate Grid --
                    pdfContentByte = masterWriter.DirectContent;

                    //// -- Ajout de saut ligne --
                    //masterDocument.Add(new Paragraph("\n\r"));
                    VerticalPositionMark seperator = new LineSeparator();
                    seperator.Offset = -20f;

                    //Get columnHeaders names in the pdf file
                    for (int column = 0; column < dataTable.Columns.Count; column++)
                    {
                        PdfPCell pdfColumnCell = new PdfPCell(new Phrase(dataTable.Columns[column].ColumnName));

                        SetColumnAlignment(dataTable, pdfColumnCell, column);
                        pdfColumnCell.BackgroundColor = new BaseColor(51, 102, 102);

                        gridTable.AddCell(pdfColumnCell);
                    }

                    //Add values of DataTable in pdf file
                    for (int row = 0; row < dataTable.Rows.Count; row++)
                    {
                        for (int col = 0; col < dataTable.Columns.Count; col++)
                        {
                            PdfPCell pdfColumnCell = new PdfPCell(new Phrase(dataTable.Rows[row][col].ToString()));

                            //Align the pdfColumnCell in the center
                            SetColumnAlignment(dataTable, pdfColumnCell, col);

                            gridTable.AddCell(pdfColumnCell);
                        }
                    }

                    //gridTable.WriteSelectedRows(0, -1, 300, 300, pdfContentByte);

                    #endregion


                    #region -- Total --

                    ligneParagraphe = new Paragraph(new Chunk(new LineSeparator(0.20F, 100.0F,
                                     BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                    //ligneParagraphe.Font.Color = Font.BOLD;
                    masterDocument.Add(ligneParagraphe);

                    font = new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.BOLD, BaseColor.BLACK);

                    PdfPTable royalFeeTotalTable = new PdfPTable(4);

                    PdfPCell royalFeeTotalCell = new PdfPCell(new Phrase(""));
                    royalFeeTotalCell.Border = 0;
                    royalFeeTotalTable.AddCell(royalFeeTotalCell);
                    royalFeeTotalCell = new PdfPCell(new Phrase("HT", font));
                    royalFeeTotalCell.VerticalAlignment = Element.ALIGN_CENTER;
                    royalFeeTotalCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    royalFeeTotalTable.AddCell(royalFeeTotalCell);
                    royalFeeTotalCell = new PdfPCell(new Phrase("TVA 20,00", font));
                    royalFeeTotalCell.VerticalAlignment = Element.ALIGN_CENTER;
                    royalFeeTotalCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    royalFeeTotalTable.AddCell(royalFeeTotalCell);

                    royalFeeTotalCell = new PdfPCell(new Phrase("TTC", font));
                    royalFeeTotalCell.VerticalAlignment = Element.ALIGN_CENTER;
                    royalFeeTotalCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    royalFeeTotalTable.AddCell(royalFeeTotalCell);
                    royalFeeTotalCell = new PdfPCell(new Phrase("Total du relevé", font));
                    royalFeeTotalCell.VerticalAlignment = Element.ALIGN_CENTER;
                    royalFeeTotalCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    royalFeeTotalTable.AddCell(royalFeeTotalCell);
                    royalFeeTotalCell = new PdfPCell(new Phrase($"{ _report.HtMontant }", font));
                    royalFeeTotalCell.VerticalAlignment = Element.ALIGN_CENTER;
                    royalFeeTotalCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    royalFeeTotalCell.Rowspan = 2;
                    royalFeeTotalTable.AddCell(royalFeeTotalCell);

                    royalFeeTotalCell = new PdfPCell(new Phrase($"{ _report.TVA }", font));
                    royalFeeTotalCell.VerticalAlignment = Element.ALIGN_CENTER;
                    royalFeeTotalCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    royalFeeTotalTable.AddCell(royalFeeTotalCell);
                    royalFeeTotalCell = new PdfPCell(new Phrase($"{ _report.TTCMontant }", font));
                    royalFeeTotalCell.VerticalAlignment = Element.ALIGN_CENTER;
                    royalFeeTotalCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    royalFeeTotalTable.AddCell(royalFeeTotalCell);
                    royalFeeTotalTable.HorizontalAlignment = Element.ALIGN_LEFT;

                    masterDocument.Add(royalFeeTotalTable);

                    ligneParagraphe = new Paragraph(new Chunk(new LineSeparator(0.0F, 100.0F,
                                     BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                    masterDocument.Add(ligneParagraphe);
                    #endregion

                    //// -- Define footer --
                    //OnEndPage(masterWriter, masterDocument);
                    OnEndPage(masterWriter, masterDocument, baseFont);

                    masterDocument.Add(gridTable);

                    pdfContentByte.SetLineWidth(3);

                    masterDocument.Close();
                    masterStream.Close();
                    masterWriter.Close();

                    Process.Start(fullPdfPath);
                }

                #endregion
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
            finally
            {

            }
        }

        // -- https://forums.asp.net/t/2000508.aspx?Pdf+File+Creation+itextsharp+multiple+user+at+sametime --
        // -- https://www.codeproject.com/Articles/691723/Csharp-Generate-and-Deliver-PDF-Files-On-Demand-fr --
        /// <summary>
        /// -- async --
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="repositoryPath"></param>
        public static bool CreatePDFV2(DataTable dataTable, string repositoryPath, int num)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            bool result = false;

            //PdfWriter masterWriter = null;
            string fileName = string.Empty;
            DateTime fileCreationDatetime = DateTime.Now;
            string imgPath = "https://ftp.mediaperf.com/img/logo.gif";
            imgPath = @"C:\Users\Sweet Family\Desktop\logo.jpg";
            var widthPercentage = 96;

            fileName = string.Format(@"\{0}.pdf", fileCreationDatetime.ToString(@"yyyyMMdd") + "_" + num + "_" + fileCreationDatetime.ToString(@"HHmmss" + ".pdf"));
            string fullPdfPath = repositoryPath + fileName;

            //if (File.Exists(fullPdfPath))
            //{               
            //    File.Delete(fullPdfPath);
            //}

            try
            {
                FileStream masterStream = new FileStream(fullPdfPath, FileMode.Create);
                using (Document masterDocument = new Document(PageSize.A4, 8, 8, 35, 10))
                using (PdfWriter masterWriter = PdfWriter.GetInstance(masterDocument, masterStream))
                {
                    //masterWriter.PageEvent = new MyPageHeader();

                    masterDocument.Open();

                    PdfContentByte pdfContentByte = masterWriter.DirectContent;
                    Chunk verticalPositionMark = new Chunk(new VerticalPositionMark());
                    var lineSeparator = new LineSeparator(2.0F, 96.0F, BaseColor.BLACK, Element.ALIGN_CENTER, 1);

                    #region -- Define fonts --
                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, false);
                    var noneFontOneWhite = new Font(baseFont, 1, Font.NORMAL, BaseColor.WHITE);
                    var boldFontEleventBlack = new Font(baseFont, 11, Font.BOLD, BaseColor.BLACK);
                    var normalFontEleventBlack = new Font(baseFont, 11, Font.NORMAL, BaseColor.BLACK);
                    var boldFontNineBlack = new Font(baseFont, 9, Font.BOLD, BaseColor.BLACK);
                    var normalFontNimeBlack = new Font(baseFont, 9, Font.NORMAL, BaseColor.BLACK);
                    var boldFontTwelveBlack = new Font(baseFont, 12, Font.BOLD, BaseColor.BLACK);
                    var normalFontTwelveBlack = new Font(baseFont, 12, Font.NORMAL, BaseColor.BLACK);
                    var boldFontTenBlack = new Font(baseFont, 10, Font.BOLD, BaseColor.BLACK);
                    var normalFontTenBlack = new Font(baseFont, 10, Font.NORMAL, BaseColor.BLACK);
                    //var TableFontmini_ARBold8 = FontFactory.GetFont("Calibri", 8, Font.BOLDITALIC, BaseColor.BLACK);
                    #endregion

                    // -- Populate dynamycs feelds --
                    ConsolidateReport(dataTable);

                    #region -- Set Header --.
                    Image imagePath = Image.GetInstance(imgPath);
                    imagePath.ScalePercent(80f);

                    PdfPTable mtable = new PdfPTable(2);
                    mtable.WidthPercentage = 100;
                    mtable.DefaultCell.Border = Rectangle.NO_BORDER;

                    PdfPTable table = new PdfPTable(5);
                    table.WidthPercentage = 30;
                    table.TotalWidth = 30;
                    PdfPCell cell = new PdfPCell(new Phrase("2"));
                    cell.Colspan = 6;
                    //cell.Rowspan = 2;
                    cell.HorizontalAlignment = 1;
                    Paragraph para = new Paragraph();
                    para.Add(new Chunk(imagePath, 5, -40));
                    cell.AddElement(para);
                    cell.BorderColor = BaseColor.WHITE;
                    table.AddCell(cell);
                    mtable.AddCell(table);

                    table = new PdfPTable(3);
                    table.WidthPercentage = 70;
                    cell = new PdfPCell(new Phrase("Relevé de redévences", new Font(Font.FontFamily.TIMES_ROMAN,
                                        20, Font.BOLD, BaseColor.BLACK)));
                    cell.Colspan = 2;
                    cell.Padding = 2;
                    cell.PaddingTop = 5;
                    cell.PaddingBottom = 5;
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                    table.AddCell(new PdfPCell(new Phrase($"              Page \n\r              { _report.CurrentPage }/{ _report.TotalPageNumber }",
                                               new Font(Font.FontFamily.TIMES_ROMAN, 10, Font.NORMAL, BaseColor.BLACK))));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell = new PdfPCell(new Phrase("Ceci n'est pas une facture", new Font(Font.FontFamily.TIMES_ROMAN, 12,
                                                   Font.BOLD, BaseColor.BLACK)));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.MinimumHeight = 18;
                    cell.PaddingBottom = 2;
                    cell.Colspan = 3;
                    table.AddCell(cell);

                    mtable.AddCell(table);
                    masterDocument.Add(mtable);

                    #endregion

                    #region -- Duplicata --
                    var docFooter = new Paragraph();
                    docFooter.Font = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 42f, BaseColor.LIGHT_GRAY);
                    docFooter.Add("\n\r");
                    docFooter.Add(new Chunk("DUPLICATA"));
                    docFooter.Alignment = Element.ALIGN_RIGHT;
                    masterDocument.Add(docFooter);
                    #endregion

                    #region -- Set first line  --
                    #region -- Left --
                    PdfPCell pdfPCell = new PdfPCell();
                    PdfPTable table0 = new PdfPTable(2);
                    table0 = new PdfPTable(1);
                    table0.TotalWidth = 165;

                    pdfPCell = new PdfPCell();
                    Paragraph numRdvParagraph = new Paragraph();
                    numRdvParagraph.Add(new Phrase("   N° RdR", boldFontEleventBlack));
                    numRdvParagraph.Add(new Chunk($"     { _report.RoyaltyFeeNumber } ", normalFontEleventBlack));
                    pdfPCell.AddElement(numRdvParagraph);
                    pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.VerticalAlignment = Element.ALIGN_CENTER;
                    table0.AddCell(pdfPCell);
                    table0.WriteSelectedRows(0, -1, 20, 670, pdfContentByte);

                    pdfPCell = new PdfPCell();
                    Paragraph dateParagraph = new Paragraph();
                    dateParagraph.Add(new Phrase("   Date", boldFontEleventBlack));
                    dateParagraph.Add(new Chunk($"     { _report.CurrentDate } ", normalFontEleventBlack));
                    pdfPCell.AddElement(dateParagraph);
                    pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table0.AddCell(dateParagraph);
                    
                    PdfPTable headerRightTable = new PdfPTable(2);
                    headerRightTable = new PdfPTable(1);
                    headerRightTable.TotalWidth = 165;
                    headerRightTable.AddCell(pdfPCell);
                    headerRightTable.WriteSelectedRows(0, -1, 200, 670, pdfContentByte);
                    #endregion

                    #region -- Destinataire Right --
                    PdfPTable headerRightSubTable = new PdfPTable(1);
                    headerRightSubTable.TotalWidth = 190;
                    PdfPTable sTable30 = new PdfPTable(2);
                    pdfPCell = new PdfPCell();
                    pdfPCell.MinimumHeight = 40;
                    pdfPCell.Padding = 5;
                    Paragraph paragraph30 = new Paragraph
                        {
                            new Phrase("Destinataire\n", boldFontEleventBlack)
                        };
                    pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdfPCell.AddElement(paragraph30);
                    sTable30.AddCell(pdfPCell);
                    headerRightSubTable.AddCell(paragraph30);

                    pdfPCell = new PdfPCell();
                    paragraph30 = new Paragraph();
                    paragraph30.Add(new Phrase($"\n\n{ _report.Destinataire } \n\n", normalFontEleventBlack));
                    paragraph30.Add(new Phrase($"{ _report.Prestataire } \n\n", normalFontEleventBlack));
                    paragraph30.Add(new Phrase($"{ _report.AdressePrestataire } \n\n\n\n\n\n\n\n", normalFontEleventBlack));
                    pdfPCell.AddElement(paragraph30);
                    paragraph30.SetLeading(2.8f, 1.2f);
                    sTable30.AddCell(pdfPCell);
                    headerRightSubTable.AddCell(paragraph30);
                    headerRightSubTable.WriteSelectedRows(0, -1, 385, 670, pdfContentByte);
                    #endregion

                    #endregion

                    #region -- Second line --
                    PdfPTable table100 = new PdfPTable(1);
                    table100.TotalWidth = 345f;

                    PdfPTable sTable = new PdfPTable(2);
                    pdfPCell = new PdfPCell();
                    Paragraph paragraph = new Paragraph
                        {
                            new Phrase($"{ _report.BfpParam1 } \n\n", boldFontEleventBlack),
                            new Phrase($"{ _report.BfpParam2 } \n\n\n\n", boldFontEleventBlack)
                        };
                    pdfPCell.AddElement(paragraph);
                    sTable.AddCell(pdfPCell);
                    table100.AddCell(paragraph);

                    pdfPCell = new PdfPCell();
                    paragraph = new Paragraph
                        {
                            new Phrase($"\n Nombre de Pv                 { _report.CountPv } \n\r", normalFontEleventBlack),
                            new Phrase($" Nombre de Campagne          { _report.CountCampagne } \n\n", normalFontEleventBlack)
                        };
                    pdfPCell.AddElement(paragraph);
                    sTable.AddCell(pdfPCell);
                    table100.AddCell(paragraph);
                    paragraph.SetLeading(5.8f, 5.2f);
                    table100.WriteSelectedRows(0, -1, 20, 645, pdfContentByte);
                    #endregion

                    // -- Draw horizontal line. --
                    Paragraph firstLineSeparator = new Paragraph(new Chunk(lineSeparator));
                    firstLineSeparator.SpacingBefore = 210f;
                    masterDocument.Add(firstLineSeparator);
                    
                    //// -- Trait de fin de prémière page --
                    //PdfContentByte contentByte = pdfContentByte;
                    //contentByte.SetLineWidth(3);
                    //contentByte.MoveTo(22, 14);
                    //contentByte.LineTo(masterDocument.PageSize.Width - 22, 14);
                    //contentByte.SetColorStroke(BaseColor.BLACK);
                    //contentByte.Stroke();

                    #region -- Generate Grid --
                    PdfPTable masterTable = new PdfPTable(dataTable.Columns.Count);
                    PdfPTable objectTable = new PdfPTable(dataTable.Columns.Count);
                    //objectTable.WidthPercentage = masterDocument.PageSize.Width - masterDocument.LeftMargin - masterDocument.RightMargin;
                    objectTable.WidthPercentage = widthPercentage;
                                       
                    // -- Add Header row for every page --
                    objectTable.HeaderRows = 1;

                    // -- Set spacing between gridView and  --
                    objectTable.SpacingBefore = 7f;

                    List<string> columnHeaders = new List<string>();

                    // -- Set columns widths --
                    float[] widths = new float[] { 12f, 12f, 60f, 12f, 30f };
                    objectTable.SetWidths(widths);

                    // -- Get columnHeaders names in the pdf file --
                    for (int column = 0; column < dataTable.Columns.Count; column++)
                    {
                        PdfPCell pdfColumnCell = new PdfPCell(new Phrase(
                                dataTable.Columns[column].ColumnName)
                            );

                        pdfColumnCell.MinimumHeight = 25;
                        pdfColumnCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                        var columnName = pdfColumnCell.Phrase[0].ToString();
                        columnHeaders.Add(columnName);                                             

                        SetColumnAlignment(dataTable, pdfColumnCell, column);
                        pdfColumnCell.BackgroundColor = BaseColor.WHITE;

                        objectTable.AddCell(pdfColumnCell);
                    }

                    var _objectTable = new PdfPTable(columnHeaders.Count) { WidthPercentage = 100 };
                    _objectTable.SetWidths(GetHeaderWidths(boldFontEleventBlack, columnHeaders.ToArray()));

                    // -- Add values of DataTable in pdf file --
                    for (int row = 0; row < dataTable.Rows.Count; row++)
                    {
                        for (int col = 0; col < dataTable.Columns.Count; col++)
                        {
                            PdfPCell pdfColumnCell = new PdfPCell(new Phrase(
                                    dataTable.Rows[row][col].ToString(),
                                    normalFontTenBlack)
                                );

                            // -- Align the pdfColumnCell in the center --
                            SetColumnAlignment(dataTable, pdfColumnCell, col);

                            objectTable.AddCell(pdfColumnCell);
                        }
                    }

                    masterDocument.Add(objectTable);

                    //PdfPCell masterCell = new PdfPCell(objectTable);
                    //masterTable.AddCell(masterCell);
                    ///
                    //masterTable.WriteSelectedRows(0, -1, 500, 300, pdfContentByte);
                    #endregion

                    #region -- Resume --
                    PdfPTable billanTable = new PdfPTable(1);
                    billanTable.WidthPercentage = widthPercentage;

                    PdfPCell billanCell = new PdfPCell();
                    string billan = $"1498 - { _report.Destinataire } - { _report.Destinataire }      Total   39       533,34";
                    billanCell = new PdfPCell(new Phrase(billan, boldFontTwelveBlack));
                    billanCell.BackgroundColor = BaseColor.LIGHT_GRAY; ;
                    billanCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    billanCell.MinimumHeight = 25;
                    billanTable.AddCell(billanCell);
                    masterDocument.Add(billanTable);
                    #endregion
                                       
                    #region -- Total --
                    PdfPTable royalFeeTotalMasterTable = new PdfPTable(1);
                    royalFeeTotalMasterTable.TotalWidth = 400;
                    PdfPTable royalFeeTotalTable = new PdfPTable(4);

                    PdfPCell firstRoyalFeeTotalCell = new PdfPCell(new Phrase("NOT Texte here", noneFontOneWhite));
                    firstRoyalFeeTotalCell.BorderColor = BaseColor.WHITE;
                    firstRoyalFeeTotalCell.Border = 5;
                    royalFeeTotalTable.AddCell(firstRoyalFeeTotalCell);
                    PdfPCell royalFeeTotalCell = new PdfPCell(new Phrase("HT", boldFontNineBlack));
                    royalFeeTotalCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    royalFeeTotalCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    royalFeeTotalCell.MinimumHeight = 20;
                    royalFeeTotalTable.AddCell(royalFeeTotalCell);

                    royalFeeTotalCell = new PdfPCell(new Phrase("TVA 20,00", boldFontNineBlack));
                    royalFeeTotalCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    royalFeeTotalCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    royalFeeTotalCell.MinimumHeight = 20;
                    royalFeeTotalTable.AddCell(royalFeeTotalCell);

                    royalFeeTotalCell = new PdfPCell(new Phrase("TTC", boldFontNineBlack));
                    royalFeeTotalCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    royalFeeTotalCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    royalFeeTotalCell.MinimumHeight = 20;
                    royalFeeTotalTable.AddCell(royalFeeTotalCell);

                    royalFeeTotalCell = new PdfPCell(new Phrase("Total du relevé", boldFontNineBlack));
                    royalFeeTotalCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    royalFeeTotalCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    royalFeeTotalCell.MinimumHeight = 20;
                    royalFeeTotalTable.AddCell(royalFeeTotalCell);

                    royalFeeTotalCell = new PdfPCell(new Phrase($"{ _report.HtMontant }", boldFontNineBlack));
                    royalFeeTotalCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    royalFeeTotalCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    royalFeeTotalCell.MinimumHeight = 20;
                    royalFeeTotalTable.AddCell(royalFeeTotalCell);

                    royalFeeTotalCell = new PdfPCell(new Phrase($"{ _report.TVA }", boldFontNineBlack));
                    royalFeeTotalCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    royalFeeTotalCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    royalFeeTotalCell.MinimumHeight = 20;
                    royalFeeTotalTable.AddCell(royalFeeTotalCell);

                    royalFeeTotalCell = new PdfPCell(new Phrase($"{ _report.TTCMontant }", boldFontNineBlack));
                    royalFeeTotalCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    royalFeeTotalCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    royalFeeTotalCell.MinimumHeight = 20;
                    royalFeeTotalTable.AddCell(royalFeeTotalCell);
                    
                    PdfPCell royalFeeTotalMasterCell = new PdfPCell(royalFeeTotalTable);

                    royalFeeTotalMasterTable.AddCell(royalFeeTotalMasterCell);
                    royalFeeTotalMasterTable.WriteSelectedRows(0, -1, 17, 122, pdfContentByte);
                    #endregion

                    // -- Draw horizontal line. --
                    Paragraph secondLineSeparator = new Paragraph(new Chunk(lineSeparator));
                    secondLineSeparator.SpacingBefore = 15f;
                    masterDocument.Add(secondLineSeparator);

                    //// -- Define footer --
                    OnEndPage(masterWriter, masterDocument, baseFont);
                   
                    masterDocument.Close();
                    masterStream.Close();
                    masterWriter.Close();
                    
                    result = true;

                    stopwatch.Stop();
                    TimeSpan stopwatchElapsed = stopwatch.Elapsed;
                    //Console.WriteLine("TEMPS MIS pour générer un PDF " + Convert.ToInt32(stopwatchElapsed.TotalMilliseconds));

                    Process.Start(fullPdfPath);
                }
            }
            catch (DocumentException de)
            {
                result = false;
                throw de;
            }
            catch (IOException ioe)
            {
                result = false;
                throw ioe;
            }
            catch (Exception exception)
            {
                result = false;
                Console.WriteLine(exception.ToString());
            }
            finally
            {

            }

            return result;
        }

        #region -- Set Header and Footer --
        /// <summary>
        /// -- OK --
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="document"></param>
        /// <param name="baseFont"></param>
        public static void OnEndPage(PdfWriter writer, Document document, BaseFont baseFont)
        {
            PdfPTable endPageTable = new PdfPTable(1);
           
            endPageTable.TotalWidth = 70;
            endPageTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            PdfPTable table2 = new PdfPTable(1);

            List<string> footerTextList = new List<string>()
            {
                "5 quai de Dion Bouton - 92816 Puteaux Cedex",
                "Tél 01 40 99 21 21 - Fax 01 40 99 80 30",
                "Société Anonyme au capital de 555.112.61€",
                "R.C. Nantère B 332 403 997 - TVA Intra FR1133240397",
                "Mediaperfomances"
            };

            PdfPCell cell2 = null;
            foreach (var footerText in footerTextList)
            {
                cell2 = new PdfPCell(new Phrase(footerText, new Font(baseFont, 8, Font.NORMAL, BaseColor.BLACK)));
                cell2.Border = Rectangle.NO_BORDER;
                table2.AddCell(cell2);
            }

            table2.DefaultCell.Border = Rectangle.NO_BORDER;
            PdfPCell cell = new PdfPCell(table2);
            cell.BorderColor = BaseColor.WHITE;
            endPageTable.AddCell(cell);
            endPageTable.WriteSelectedRows(0, -1, 15, 70, writer.DirectContent);
        }
        
        public static void OnHeaderPage(PdfWriter writer, Document document)
        {
            PdfContentByte pdfContentByte = writer.DirectContent;
            ColumnText ct = new ColumnText(pdfContentByte);

            pdfContentByte.BeginText();
            pdfContentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 12.0f);
            pdfContentByte.SetTextMatrix(document.LeftMargin, document.PageSize.Height - document.TopMargin);

            pdfContentByte.ShowText("5 quai de Dion Bouton - 92816 Puteaux Cedex \n\r");
        }
        #endregion
        
        #region Methods
        private static float[] GetHeaderWidths(Font font, params string[] headers)
        {
            var total = 0;
            var columns = headers.Length;
            var widths = new int[columns];
            for (var i = 0; i < columns; ++i)
            {
                var w = font.GetCalculatedBaseFont(true).GetWidth(headers[i]);
                total += w;
                widths[i] = w;
            }
            var result = new float[columns];
            for (var i = 0; i < columns; ++i)
            {
                result[i] = (float)widths[i] / total * 100;
            }
            return result;
        }

        private static void SetColumnAlignment(DataTable dataTable, PdfPCell cell, int i)
        {
            if (dataTable.Columns[i].DataType == typeof(string))
            {
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            }
            if (dataTable.Columns[i].DataType == typeof(Boolean))
            {
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            }
            if (dataTable.Columns[i].DataType == typeof(DateTime))
            {
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            }
            if (dataTable.Columns[i].DataType == typeof(int))
            {
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            }
        }

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

        private static void ConsolidateReport(DataTable dataTable)
        {
            var dataSet = dataTable.Rows;
            _report.CurrentDate = DateTime.Today.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            _report.RoyaltyFeeNumber = 86327;
            _report.BfpParam1 = "When an object of this type is passed to a PdfDocument (class), every element added to this document will be written to the file specified.";
            _report.BfpParam2 = "Create a new PdfPage class using the addNewPage() method of the PdfDocument class.";

            _report.Destinataire = "MEDIAPERFORMANCES";
            _report.Prestataire = "CARREFOUR MARKET (94258)";
            _report.AdressePrestataire = "24 QUAI GALLIENI 92150 SURESNES FRANCE";
            _report.CountCampagne = 33;
            _report.TotalPageNumber = 3;
            _report.CurrentPage = 1;
            _report.CountPv = 2;

            foreach (DataRow dataRow in dataSet)
            {

                //_report.AdressePrestataire = dataRow["BookTitle"].ToString();
            }

            var results = from myRow in dataTable.AsEnumerable()
                          where myRow.Field<int>("Age") == 1
                          select myRow;

            string[] columnNames = (from dc in dataTable.Columns.Cast<DataColumn>()
                                    select dc.ColumnName).ToArray();
        } 
        #endregion
    }

    // https://stackoverflow.com/questions/2321526/pdfptable-as-a-header-in-itextsharp

    public class Report
    {
        public int CountPv { get; set; }
        public int CountCampagne { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPageNumber { get; set; }
        public string CurrentDate { get; set; }
        public int RoyaltyFeeNumber { get; set; }
        public string Prestataire { get; set; }
        public string AdressePrestataire { get; set; }
        public string Destinataire { get; set; }
        public string BfpParam1 { get; set; }
        public string BfpParam2 { get; set; }
        public decimal HtMontant { get; set; } = 533.39m;
        public decimal TVA { get; set; } = 106.67m;
        public decimal TTCMontant { get; set; } = 640.01m;
    }
    
    public class MyPageHeader : PdfPageEventHelper
    {
        //PdfPTable header = new PdfPTable(3);
        //PdfPTable billanTable = new PdfPTable(1);
        ////billanTable.WidthPercentage = 97;

        //            PdfPCell billanCell = new PdfPCell();
        //string billan = $"1498 - _report.Destinataire - _report.Destinataire     Total   39       533,34";
        //billanCell = new PdfPCell(new Phrase(billan, boldFontTwelveBlack));
        //            billanCell.BackgroundColor = BaseColor.LIGHT_GRAY; ;
        //            billanCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            billanCell.MinimumHeight = 25;
        //            billanTable.AddCell(billanCell);
        //            masterDocument.Add(billanTable);

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            PdfPTable headerTable = new PdfPTable(3);
            headerTable.WidthPercentage = 97;

            PdfPCell headerCell = new PdfPCell();
            string billan = $"1498 - _report.Destinataire - _report.Destinataire     Total   39       533,34";
            headerCell = new PdfPCell(new Phrase(billan));
            headerCell.BackgroundColor = BaseColor.LIGHT_GRAY; ;
            headerCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            headerCell.MinimumHeight = 25;
            headerTable.AddCell(headerCell);

            headerTable.WriteSelectedRows(0, -1, document.Left, document.Top, writer.DirectContent);
        }
    }
}
