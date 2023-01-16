using ImageMagick;
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

      MagickNET.SetTempDirectory(@"D:\GitWare\Apps\Sample.Maui.Desktop\Temp");

      if (File.Exists(FileName))
      {
         DisplayAlert("File OK", FileName, "ok");

         //https://www.c-sharpcorner.com/UploadFile/a0927b/create-pdf-document-and-convert-to-image-programmatically/

         ConvertPDFToMultipleImages(FileName);
         //var list = SplitPDF(FileName);

         //foreach (var item in list)
         //{
         //   PDF2Img(item);
         //};

      }
   }

   List<string> SplitPDF(string filepath)
   {
      List<string> result = new List<string>();

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

         result.Add(outfile);

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

      return result;
   }


   string PDF2Img(string filepath)
   {
      string imgFileName = filepath + ".jpg";

      MagickReadSettings settings = new MagickReadSettings();
      settings.Density = new Density(300);

      using (MagickImageCollection images = new MagickImageCollection())
      {
         if (File.Exists(filepath))
         {
            images.Read(filepath, settings);

            IMagickImage image = images.AppendVertically();
            image.Format = MagickFormat.Jpg;

            //image.Quality = 70; 
            //if (image.Width > 1024)
            //{
            //    int heightRatio = Convert.ToInt32(Math.Round((decimal)(image.Height / (image.Width / 1024)), 0));
            //    image.Resize(1024, heightRatio);
            //}

            image.Write(imgFileName);
            image.Dispose();
         };
      }

      return imgFileName;
   }


   public static void ConvertPDFToMultipleImages(string filepath)
   {
      var settings = new MagickReadSettings();
      // Settings the density to 300 dpi will create an image with a better quality
      settings.Density = new Density(300, 300);

      using (var images = new MagickImageCollection())
      {
         // Add all the pages of the pdf file to the collection
         images.Read(filepath, settings);

         int page = 1;
         foreach (var image in images)
         {
            // Write page to file that contains the page number
            image.Write(filepath + $"_{page}.png");

            //// Writing to a specific format works the same as for a single image
            //image.Format = MagickFormat.Ptif;
            //image.Write(filepath + $"_{page}.tif");
            page++;
         }
      }
   }

}

