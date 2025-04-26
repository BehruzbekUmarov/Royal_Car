using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Application.Helper.Interfaces;

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
	private readonly IFileConverterHelper _fileConverterHelper;
	private readonly IWebHostEnvironment _env;

	public ConvertPdfToDocxCommandHandler(IWebHostEnvironment env, IFileConverterHelper fileConverterHelper)
	{
		_env = env;
		_fileConverterHelper = fileConverterHelper;
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

		var outputFilePath = await _fileConverterHelper.ConvertPdfToDocxAsync(inputFilePath);
		return Path.GetFileName(outputFilePath);
	}
}
