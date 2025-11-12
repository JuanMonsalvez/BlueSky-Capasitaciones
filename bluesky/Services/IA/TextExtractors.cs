using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace bluesky.Services.IA
{
    public static class TextExtractors
    {
        public static string HtmlToPlainText(string html)
        {
            if (string.IsNullOrWhiteSpace(html)) return "";
            // quita scripts/estilos
            html = Regex.Replace(html, "(?is)<(script|style)[^>]*>.*?</\\1>", "");
            // quita tags
            var text = Regex.Replace(html, "<.*?>", " ");
            // normaliza espacios
            text = Regex.Replace(text, "\\s+", " ").Trim();
            return text;
        }

        public static string PdfToText(string pdfPath)
        {
            // Requiere NuGet: itext7  (iText.Kernel, iText.Layout)
            try
            {
                var sb = new StringBuilder();
                using (var pdf = new iText.Kernel.Pdf.PdfDocument(new iText.Kernel.Pdf.PdfReader(pdfPath)))
                {
                    int n = pdf.GetNumberOfPages();
                    for (int i = 1; i <= n; i++)
                    {
                        var page = pdf.GetPage(i);
                        var strat = new iText.Kernel.Pdf.Canvas.Parser.Listener.LocationTextExtractionStrategy();
                        var text = iText.Kernel.Pdf.Canvas.Parser.PdfTextExtractor.GetTextFromPage(page, strat);
                        if (!string.IsNullOrWhiteSpace(text)) sb.AppendLine(text);
                    }
                }
                return Normalize(sb.ToString());
            }
            catch
            {
                return "";
            }
        }

        public static string PptxToText(string path)
        {
            // Requiere NuGet: DocumentFormat.OpenXml
            try
            {
                var sb = new StringBuilder();
                using (var doc = DocumentFormat.OpenXml.Packaging.PresentationDocument.Open(path, false))
                {
                    var presentation = doc.PresentationPart.Presentation;
                    foreach (var sref in presentation.SlideIdList.ChildElements)
                    {
                        var sid = (DocumentFormat.OpenXml.Presentation.SlideId)sref;
                        var part = (DocumentFormat.OpenXml.Packaging.SlidePart)doc.PresentationPart.GetPartById(sid.RelationshipId);
                        var texts = part.Slide.Descendants<DocumentFormat.OpenXml.Drawing.Text>();
                        foreach (var t in texts)
                        {
                            var v = t.Text;
                            if (!string.IsNullOrWhiteSpace(v))
                                sb.AppendLine(v);
                        }
                    }
                }
                return Normalize(sb.ToString());
            }
            catch
            {
                return "";
            }
        }

        public static string JoinAndTrim(System.Collections.Generic.IEnumerable<string> chunks, int hardLimit)
        {
            var sb = new StringBuilder();
            foreach (var c in chunks)
            {
                if (string.IsNullOrWhiteSpace(c)) continue;
                if (sb.Length + c.Length > hardLimit) break;
                sb.AppendLine(c);
            }
            return Normalize(sb.ToString());
        }

        private static string Normalize(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "";
            s = Regex.Replace(s, "\\s+", " ").Trim();
            return s;
        }
    }
}
