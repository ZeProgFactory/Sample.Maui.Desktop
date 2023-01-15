using Microsoft.Maui.Graphics;
using Microsoft.VisualBasic.FileIO;

namespace MauiDesktopApp;

public partial class MainPage : ContentPage
{
   public MainPage()
   {
      InitializeComponent();
   }

   private void Button_Clicked(object sender, EventArgs e)
   {
      var cmd = (sender as Button).CommandParameter.ToString();
      var FileName = "";

      switch (cmd)
      {
         case "1":
            FileName = @"Get_Started_With_Smallpdf.pdf";
            break;

         case "2":
            FileName = @"sample.pdf";
            break;

         case "3":
            FileName = @"sample-pdf-download-10-mb.pdf";
            break;
      };

      FileName = Path.Combine(@"D:\GitWare\Apps\Sample.Maui.Desktop\Data", FileName);

      if (File.Exists(FileName))
      {
         DisplayAlert("File OK", FileName, "ok");

      https://www.c-sharpcorner.com/UploadFile/a0927b/create-pdf-document-and-convert-to-image-programmatically/

         SplitPDF(FileName);
      }
   }

   void SplitPDF(string filepath)
   {
      iTextSharp.text.pdf.PdfReader reader = null;
      int currentPage = 1;
      int pageCount = 0;

      System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
      reader = new iTextSharp.text.pdf.PdfReader(filepath);
      reader.RemoveUnusedObjects();
      pageCount = reader.NumberOfPages;
      string ext = System.IO.Path.GetExtension(filepath);

      for (int i = 1; i <= pageCount; i++)
      {
         iTextSharp.text.pdf.PdfReader reader1 = new iTextSharp.text.pdf.PdfReader(filepath);
         string outfile = filepath.Replace((System.IO.Path.GetFileName(filepath)), (System.IO.Path.GetFileName(filepath).Replace(".pdf", "") + "_" + i.ToString()) + ext);
         reader1.RemoveUnusedObjects();
         iTextSharp.text.Document doc = new iTextSharp.text.Document(reader.GetPageSizeWithRotation(currentPage));
         iTextSharp.text.pdf.PdfCopy pdfCpy = new iTextSharp.text.pdf.PdfCopy(doc, new System.IO.FileStream(outfile, System.IO.FileMode.Create));
         doc.Open();
         for (int j = 1; j <= 1; j++)
         {
            iTextSharp.text.pdf.PdfImportedPage page = pdfCpy.GetImportedPage(reader1, currentPage);
            //? pdfCpy.SetFullCompression();
            pdfCpy.AddPage(page);
            currentPage += 1;
         }
         doc.Close();
         pdfCpy.Close();
         reader1.Close();
         reader.Close();
      }
   }
}

