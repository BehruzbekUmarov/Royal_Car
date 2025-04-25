using FluentValidation;
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

public class ConvertImagesToPdfCommandValidator : AbstractValidator<ConvertImagesToPdfCommand>
{
	public ConvertImagesToPdfCommandValidator()
	{
		RuleFor(x => x.Images)
			.NotEmpty().WithMessage("At least one image must be provided.")
			.Must(images => images.All(IsSupportedImage))
			.WithMessage("Only JPG, JPEG, and PNG image files are allowed.");

		RuleForEach(x => x.Images)
			.Must(file => file.Length > 0).WithMessage("Image file cannot be empty.")
			.Must(file => file.Length < 5 * 1024 * 1024).WithMessage("Image file must be less than 5MB.");

		RuleFor(x => x.FileName)
			.MaximumLength(100).WithMessage("Filename is too long.")
			.Matches(@"^[\w\-\. ]*$").WithMessage("Filename contains invalid characters.")
			.When(x => !string.IsNullOrWhiteSpace(x.FileName));
	}

	private bool IsSupportedImage(IFormFile file)
	{
		var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
		return file != null && allowedExtensions.Any(ext => file.FileName.EndsWith(ext, System.StringComparison.OrdinalIgnoreCase));
	}
}

public class ConvertImagesToPdfCommandHandler : IRequestHandler<ConvertImagesToPdfCommand, byte[]>
{
	public async Task<byte[]> Handle(ConvertImagesToPdfCommand request, CancellationToken cancellationToken)
	{
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