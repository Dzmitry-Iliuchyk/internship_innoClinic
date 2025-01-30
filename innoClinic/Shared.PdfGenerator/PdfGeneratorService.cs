using iText.Html2pdf;

namespace Shared.PdfGenerator {
    public class PdfGeneratorService {
        public byte[] GeneratePdf( string htmlTemplate ) {
            using MemoryStream stream = new MemoryStream();
            
            ConverterProperties properties = new ConverterProperties();
            HtmlConverter.ConvertToPdf( htmlTemplate, stream, properties );
            return stream.ToArray();

        }
    }
}
