using MediatR;
using Microsoft.AspNetCore.Http;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Application.Features.ImageConvertorManager.Commands;

public class ConvertImagesToPdfCommand : IRequest<byte[]>
{
	public string? FileName { get; set; } 
	public List<IFormFile> Images { get; set; } = new();
}

public class ConvertImagesToPdfCommandHandler : IRequestHandler<ConvertImagesToPdfCommand, byte[]>
{
	public async Task<byte[]> Handle(ConvertImagesToPdfCommand request, CancellationToken cancellationToken)
	{
		//using var document = new PdfSharpCore.Pdf.PdfDocument();

		//foreach (var file in request.Images)
		//{
		//	using var ms = new MemoryStream();
		//	await file.CopyToAsync(ms, cancellationToken);
		//	ms.Seek(0, SeekOrigin.Begin);

		//	using var image = SixLabors.ImageSharp.Image.Load<SixLabors.ImageSharp.PixelFormats.Rgba32>(ms);
		//	var page = document.AddPage();
		//	page.Width = PdfSharpCore.Drawing.XUnit.FromPoint(image.Width);
		//	page.Height = PdfSharpCore.Drawing.XUnit.FromPoint(image.Height);

		//	using var gfx = PdfSharpCore.Drawing.XGraphics.FromPdfPage(page);
		//	using var imgStream = new MemoryStream();
		//	await image.SaveAsPngAsync(imgStream, cancellationToken);
		//	imgStream.Seek(0, SeekOrigin.Begin);

		//	var xImage = PdfSharpCore.Drawing.XImage.FromStream(() => imgStream);
		//	gfx.DrawImage(xImage, 0, 0, page.Width, page.Height);
		//}

		//using var outputMs = new MemoryStream();
		//document.Save(outputMs);
		//return outputMs.ToArray();

		using var document = new PdfDocument();

		foreach (var file in request.Images)
		{
			// Load the image
			using var ms = new MemoryStream();
			await file.CopyToAsync(ms, cancellationToken);
			ms.Seek(0, SeekOrigin.Begin);

			using var image = SixLabors.ImageSharp.Image.Load<Rgba32>(ms);

			// PDF page
			var page = document.AddPage();
			double margin = 40; // points (~0.5 inch)

			double pageWidth = 595;   // A4 width in points (8.27 in × 72)
			double pageHeight = 842;  // A4 height in points (11.69 in × 72)

			page.Width = XUnit.FromPoint(pageWidth);
			page.Height = XUnit.FromPoint(pageHeight);

			// Scaling
			double maxWidth = pageWidth - 2 * margin;
			double maxHeight = pageHeight - 2 * margin;

			double imageWidth = image.Width;
			double imageHeight = image.Height;

			double ratioX = maxWidth / imageWidth;
			double ratioY = maxHeight / imageHeight;
			double scale = Math.Min(ratioX, ratioY);

			double scaledWidth = imageWidth * scale;
			double scaledHeight = imageHeight * scale;

			double offsetX = (pageWidth - scaledWidth) / 2;
			double offsetY = (pageHeight - scaledHeight) / 2;

			// Convert to stream again
			using var imageStream = new MemoryStream();
			await image.SaveAsPngAsync(imageStream, cancellationToken);
			imageStream.Seek(0, SeekOrigin.Begin);

			// Draw image
			using var gfx = XGraphics.FromPdfPage(page);
			using var xImage = XImage.FromStream(() => imageStream);
			gfx.DrawImage(xImage, offsetX, offsetY, scaledWidth, scaledHeight);
		}

		using var output = new MemoryStream();
		document.Save(output);
		return output.ToArray();
	}
}