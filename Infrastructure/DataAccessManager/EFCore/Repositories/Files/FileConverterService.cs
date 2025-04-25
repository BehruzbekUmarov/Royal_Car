using Application.Common.Repositories.Files;
using Aspose.Words;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Hosting;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Text;

namespace Infrastructure.DataAccessManager.EFCore.Repositories.Files;

public class FileConverterService : IFileConverterService
{
	private readonly IWebHostEnvironment _env;
	public FileConverterService(IWebHostEnvironment env)
	{
		_env = env;
	}

	public async Task<string> ConvertDocxToPdfAsync(string inputPath)
	{
		var text = ExtractTextFromDocx(inputPath);

		var pdf = new PdfDocument();
		var page = pdf.AddPage();
		var gfx = XGraphics.FromPdfPage(page);
		var font = new XFont("Verdana", 12);
		gfx.DrawString(text, font, XBrushes.Black,
			new XRect(20, 20, page.Width - 40, page.Height - 40), XStringFormats.TopLeft);

		var outputPath = Path
			.Combine(Path.GetDirectoryName(inputPath), Path.GetFileNameWithoutExtension(inputPath) + ".pdf");
		using (var stream = File.Create(outputPath))
		{
			pdf.Save(stream);
		}

		return outputPath;
	}

	public async Task<string> ConvertPdfToDocxAsync(string inputPath)
	{
		var docxPath = Path
			.Combine(Path.GetDirectoryName(inputPath), Path.GetFileNameWithoutExtension(inputPath) + ".docx");

		using (var wordDoc = WordprocessingDocument
			.Create(docxPath, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
		{
			var mainPart = wordDoc.AddMainDocumentPart();
			mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();
			var body = mainPart.Document.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Body());
			var para = body.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
			var run = para.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
			run.AppendChild(new DocumentFormat.OpenXml.Wordprocessing
				.Text("This is a placeholder. PDF to DOCX needs commercial libs."));
		}

		return docxPath;
	}
	private string ExtractTextFromDocx(string path)
	{
		StringBuilder sb = new StringBuilder();
		using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(path, false))
		{
			var body = wordDoc.MainDocumentPart.Document.Body;
			sb.Append(body.InnerText);
		}
		return sb.ToString();
	}
}

//using Application.Common.Repositories.Files;
//using Aspose.Words;
//using Aspose.Pdf;
//using Microsoft.AspNetCore.Hosting;
//using System.IO;
//using System.Text;
//using System.Threading.Tasks;
//using DocumentFormat.OpenXml.Packaging;

//namespace Infrastructure.DataAccessManager.EFCore.Repositories.Files
//{
//	public class FileConverterService : IFileConverterService
//	{
//		private readonly IWebHostEnvironment _env;

//		public FileConverterService(IWebHostEnvironment env)
//		{
//			_env = env;
//		}

//		// Convert DOCX to PDF using Aspose.Words
//		public async Task<string> ConvertDocxToPdfAsync(string inputPath)
//		{
//			// Validate input path
//			if (string.IsNullOrEmpty(inputPath) || !File.Exists(inputPath))
//			{
//				throw new FileNotFoundException("The DOCX file was not found.", inputPath);
//			}

//			var outputPath = Path.Combine(Path.GetDirectoryName(inputPath), Path.GetFileNameWithoutExtension(inputPath) + ".pdf");

//			try
//			{
//				// Load the DOCX file using Aspose.Words
//				var doc = new Aspose.Pdf.Document(inputPath);

//				// Save the document as a PDF
//				doc.Save(outputPath, Aspose.Pdf.SaveFormat.Pdf);
//			}
//			catch (Exception ex)
//			{
//				throw new InvalidOperationException("Failed to convert DOCX to PDF", ex);
//			}

//			return outputPath;
//		}

//		// Convert PDF to DOCX using Aspose.Pdf
//		public async Task<string> ConvertPdfToDocxAsync(string inputPath)
//		{
//			// Validate input path
//			if (string.IsNullOrEmpty(inputPath) || !File.Exists(inputPath))
//			{
//				throw new FileNotFoundException("The PDF file was not found.", inputPath);
//			}

//			var docxPath = Path.Combine(Path.GetDirectoryName(inputPath), Path.GetFileNameWithoutExtension(inputPath) + ".docx");

//			try
//			{
//				// Load PDF using Aspose.Pdf
//				var pdfDocument = new Aspose.Pdf.Document(inputPath);

//				// Convert PDF to DOCX using Aspose.Pdf's method
//				pdfDocument.Save(docxPath, Aspose.Pdf.SaveFormat.DocX);
//			}
//			catch (Exception ex)
//			{
//				throw new InvalidOperationException("Failed to convert PDF to DOCX", ex);
//			}

//			return docxPath;
//		}

//		// Extract text from DOCX using OpenXML (still useful for certain scenarios)
//		private string ExtractTextFromDocx(string path)
//		{
//			StringBuilder sb = new StringBuilder();
//			try
//			{
//				using (var wordDoc = WordprocessingDocument.Open(path, false))
//				{
//					var body = wordDoc.MainDocumentPart.Document.Body;
//					sb.Append(body.InnerText);
//				}
//			}
//			catch (Exception ex)
//			{
//				throw new InvalidOperationException("Failed to extract text from DOCX", ex);
//			}

//			return sb.ToString();
//		}
//	}
//}

