using Application.Features.ConvertorManager.Commands;
using Application.Features.ConvertorManager.Queries;
using Application.Features.ImageConvertorManager.Commands;
using ASPNET.Common.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.Controllers;

[Route("api/[controller]")]
public class FileController : BaseApiController
{
	public FileController(ISender sender) : base(sender)
	{
	}

	[HttpPost("convertImageToPdf")]
	public async Task<IActionResult> ConvertImagesToPdf([FromForm] ConvertImagesToPdfCommand command)
	{
		var pdfBytes = await _sender.Send(command);

		string sanitizedFileName = string.IsNullOrWhiteSpace(command.FileName)
		? $"converted_{DateTime.UtcNow:yyyyMMdd_HHmmss}.pdf"
		: Path.ChangeExtension(command.FileName.Trim(), ".pdf");

		return File(pdfBytes, "application/pdf", sanitizedFileName);
	}

	[HttpPost("docx-to-pdf")]
	public async Task<IActionResult> ConvertDocxToPdf(IFormFile file)
	{
		var request = new ConvertDocxToPdfRequest
		{
			File = file 
		};

		var result = await _sender.Send(request);
		return Ok(new { fileName = result });
	}

	[HttpPost("pdf-to-docx")]
	public async Task<IActionResult> ConvertPdfToDocx(IFormFile file)
	{
		var request = new ConvertPdfToDocxRequest
		{
			File = file 
		};

		var result = await _sender.Send(request);
		return Ok(new { fileName = result });
	}

	[HttpGet("download/{fileName}")]
	public async Task<IActionResult> Download(string fileName)
	{
		var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", fileName);
		if (!System.IO.File.Exists(filePath))
			return NotFound();

		var mimeType = fileName
			.EndsWith(".pdf") ? "application/pdf" : "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
		var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
		return File(fileBytes, mimeType, fileName);

		//var request = new DownloadConvertedFileQuery
		//{
		//	FileName = fileName
		//};


		//var fileBytes = await _sender.Send(request);
		//if (fileBytes == null)
		//	return NotFound();

		//var mimeType = fileName.EndsWith(".pdf") ? "application/pdf" :
		//			   "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
		//return File(fileBytes, mimeType, fileName);
	}
}
