using DevExpress.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace DebtChecking.SLIK
{
    public class Watermark
    {
        public void AddWatermark(string text, string fileName, string resultFileName)
        {
            using (PdfDocumentProcessor documentProcessor = new PdfDocumentProcessor())
            {
                string fontName = "Arial Black";
                int fontSize = 12;
                PdfStringFormat stringFormat = PdfStringFormat.GenericTypographic;
                stringFormat.Alignment = PdfStringAlignment.Center;
                stringFormat.LineAlignment = PdfStringAlignment.Center;
                documentProcessor.LoadDocument(fileName);
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(63, Color.Black)))
                {
                    using (Font font = new Font(fontName, fontSize))
                    {
                        foreach (var page in documentProcessor.Document.Pages)
                        {
                            var watermarkSize = page.CropBox.Width * 0.75;
                            using (PdfGraphics graphics = documentProcessor.CreateGraphics())
                            {
                                SizeF stringSize = graphics.MeasureString(text, font);
                                Single scale = Convert.ToSingle(watermarkSize / stringSize.Width);
                                graphics.TranslateTransform(Convert.ToSingle(page.CropBox.Width * 0.5), Convert.ToSingle(page.CropBox.Height * 0.5));
                                graphics.RotateTransform(-45);
                                graphics.TranslateTransform(Convert.ToSingle(-stringSize.Width * scale * 0.5), Convert.ToSingle(-stringSize.Height * scale * 0.5));
                                using (Font actualFont = new Font(fontName, fontSize * scale))
                                {
                                    RectangleF rect = new RectangleF(0, 0, stringSize.Width * scale, stringSize.Height * scale);
                                    graphics.DrawString(text, actualFont, brush, rect, stringFormat);
                                }

                                graphics.AddToPageForeground(page, 72, 72);
                            }
                        }
                    }
                }

                documentProcessor.SaveDocument(resultFileName);
            }
        }

    }
}