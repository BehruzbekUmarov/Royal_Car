using Application.Features.ImageConvertorManager.Commands;
using ASPNET.Common.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.Controllers;

[Route("api/[controller]")]
public class PdfController : BaseApiController
{
	public PdfController(ISender sender) : base(sender)
	{
	}

	[HttpPost("convertToPdf")]
	public async Task<IActionResult> ConvertImagesToPdf([FromForm] ConvertImagesToPdfCommand command)
	{
		var pdfBytes = await _sender.Send(command);
		//return File(pdfBytes, "application/pdf", "converted.pdf");

		string sanitizedFileName = string.IsNullOrWhiteSpace(command.FileName)
		? $"converted_{DateTime.UtcNow:yyyyMMdd_HHmmss}.pdf"
		: Path.ChangeExtension(command.FileName.Trim(), ".pdf");

		return File(pdfBytes, "application/pdf", sanitizedFileName);
	}
}
