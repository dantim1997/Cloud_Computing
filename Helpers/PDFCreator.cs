using Domain;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class PDFCreator
    {
        public static byte[] CreatePDF(Mortgage mortgage)
        {
            Document doc = new Document(PageSize.A4, 50, 50, 50, 50);

            using (MemoryStream output = new MemoryStream())
            {
                PdfWriter wri = PdfWriter.GetInstance(doc, output);
                doc.Open();

                Paragraph header = new Paragraph("Your Mortgage") { Alignment = Element.ALIGN_CENTER };
                Paragraph paragraph = new Paragraph($"Hello {mortgage.Name}.");
                Phrase phrase = new Phrase($"Your mortgage is calculated and is: \u20AC {mortgage.MortgageAmount}.");

                doc.Add(header);
                doc.Add(paragraph);
                doc.Add(phrase);

                doc.Close();
                return output.ToArray();
            }
        }
    }
}
