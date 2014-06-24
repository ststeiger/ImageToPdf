
using System;
using System.Windows.Forms;


namespace Image2Pdf
{


    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        } // End Constructor


        public class PdfSettings
        {
            public static int X = 20;
            public static int Y = 20;
            public static int Width = 550;
            public static int Height = 800;

            public static double Thickness = 0.25;

            public static bool Outline = false;
            public static PdfSharp.Drawing.XPen OutlinePen = new PdfSharp.Drawing.XPen(PdfSharp.Drawing.XColors.Red, Thickness);
            public static System.Drawing.Size sze = new System.Drawing.Size(Width, Height);
        } // End Class PdfSettings


        public static PdfSharp.Pdf.PdfDocument CreateDocument()
        {
            PdfSharp.Pdf.PdfDocument document = new PdfSharp.Pdf.PdfDocument();
            document.Info.Title = "Legenden";
            document.Info.Author = "COR Managementsysteme GmbH - Stefan Steiger";
            document.Info.Subject = "Zeichnung";
            document.Info.Keywords = "PDFsharp, XGraphics";

            return document;
        } // End Function CreateDocument


        public static void SaveDocument(PdfSharp.Pdf.PdfDocument document, string strPath, string strFileName)
        {
            string strFileToSave = System.IO.Path.Combine(strPath, strFileName);
            document.Save(strFileToSave);
            System.Diagnostics.Process.Start(strFileToSave);
        } // End Sub SaveDocument


        public static void AddPage(PdfSharp.Pdf.PdfDocument document, string strFilename)
        {
            PdfSharp.Pdf.PdfPage page = document.AddPage();
            // page.Width = 1500;

            // page.Size = PdfSharp.PageSize.A5;
            page.Size = PdfSharp.PageSize.A4;
            // page.Size = PdfSharp.PageSize.A2;

            page.Orientation = PdfSharp.PageOrientation.Portrait;

            

            using (System.Drawing.Image img = System.Drawing.Image.FromFile(strFilename))
            {

                PdfSharp.Drawing.XRect rectImage = new PdfSharp.Drawing.XRect(PdfSettings.X, PdfSettings.Y, PdfSettings.Width, PdfSettings.Height);

                using (PdfSharp.Drawing.XGraphics gfx = PdfSharp.Drawing.XGraphics.FromPdfPage(page))
                {

                    //using (System.Drawing.Image img2 = HighQualityResizeImage(img, PdfSettings.sze)){

                        //using (PdfSharp.Drawing.XImage ximg = PdfSharp.Drawing.XImage.FromGdiPlusImage(img2))
                        using (PdfSharp.Drawing.XImage ximg = PdfSharp.Drawing.XImage.FromGdiPlusImage(img))
                        {
                            gfx.DrawImage(ximg, rectImage);
                        } // End using ximg

                    //} // End Using img2

                    if (PdfSettings.Outline)
                        gfx.DrawRectangle(PdfSettings.OutlinePen, rectImage);

                } // End Using gfx

            } // End Using img

        } // End Sub AddPage


        public static System.Drawing.Image HighQualityResizeImage(System.Drawing.Image image, System.Drawing.Size size)
        {
            System.Drawing.Bitmap result = new System.Drawing.Bitmap(size.Width, size.Height);

            // Set the resolutions the same to avoid cropping due to resolution differences
            result.SetResolution(image.HorizontalResolution, image.VerticalResolution);


            //use a graphics object to draw the resized image into the bitmap
            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(result))
            {
                //set the resize quality modes to high quality
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //draw the image into the target bitmap
                graphics.DrawImage(image, 0, 0, result.Width, result.Height);
            } // graphics

            return result;
        } // HighQualityResizeImage


        private void btnCreatePdf_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("FileName", typeof(string));

            System.Data.DataRow dr = null;

            string[] filez = System.IO.Directory.GetFiles(@"D:\stefan.steiger\Downloads\Raiffeissss\Privatkunden", @"*.*");

            PdfSharp.Pdf.PdfDocument document = CreateDocument();



            string strTopDirectoryName = new System.IO.FileInfo(filez[0]).Directory.Name;
            Console.WriteLine(strTopDirectoryName);

            foreach (string strFileName in filez)
            {
                dr = dt.NewRow();
                // Console.WriteLine(strFileName);
                dr["FileName"] = strFileName;

                AddPage(document, strFileName);
                dt.Rows.Add(dr);
            }

            this.dataGridView1.DataSource = dt.DefaultView;
            this.dataGridView1.Columns[0].Width = 800;

            string strPath = @"D:\stefan.steiger\Downloads\Raiffeissss\";
            SaveDocument(document, strPath, strTopDirectoryName + ".pdf");
        } // End Sub btnCreatePdf_Click


    } // End Class Form1 : Form


} // End Namespace Image2Pdf
