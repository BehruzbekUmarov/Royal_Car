using Application.Common.Repositories.Files;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace Application.Features.ConvertorManager.Commands;

public class ConvertPdfToDocxRequest : IRequest<string>
{
	public required IFormFile File { get; set; }

    public ConvertPdfToDocxRequest()
    {
        
    }
}

public class ConvertPdfToDocxCommandHandler : IRequestHandler<ConvertPdfToDocxRequest, string>
{
	private readonly IFileConverterService _fileConverterService;
	private readonly IWebHostEnvironment _env;

	public ConvertPdfToDocxCommandHandler(IFileConverterService fileConverterService, IWebHostEnvironment env)
	{
		_fileConverterService = fileConverterService;
		_env = env;
	}

	public async Task<string> Handle(ConvertPdfToDocxRequest request, CancellationToken cancellationToken)
	{
		var uploadsFolder = Path.Combine(_env.WebRootPath, "files");
		Directory.CreateDirectory(uploadsFolder);

		var inputFilePath = Path.Combine(uploadsFolder, Path.GetFileName(request.File.FileName));

		using (var stream = new FileStream(inputFilePath, FileMode.Create))
		{
			await request.File.CopyToAsync(stream, cancellationToken);
		}

		var outputFilePath = await _fileConverterService.ConvertPdfToDocxAsync(inputFilePath);
		return Path.GetFileName(outputFilePath);
	}
}
