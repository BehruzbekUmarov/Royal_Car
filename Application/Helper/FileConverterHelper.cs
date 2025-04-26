using Application.Helper.Interfaces;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Hosting;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Text;

namespace Application.Helper;

public class FileConverterHelper : IFileConverterHelper
{
	private readonly IWebHostEnvironment _env;
	public FileConverterHelper(IWebHostEnvironment env)
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
