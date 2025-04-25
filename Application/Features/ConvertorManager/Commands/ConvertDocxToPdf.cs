using Application.Common.Repositories.Files;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Application.Features.ConvertorManager.Commands;

public class ConvertDocxToPdfRequest : IRequest<string>
{
	public required IFormFile File { get; set; }

    public ConvertDocxToPdfRequest()
    {
        
    }
}

public class ConvertDocxToPdfCommandHandler : IRequestHandler<ConvertDocxToPdfRequest, string>
{
	private readonly IFileConverterService _fileConverterService;
	private readonly IWebHostEnvironment _env;

	public ConvertDocxToPdfCommandHandler(IFileConverterService fileConverterService, IWebHostEnvironment env)
	{
		_fileConverterService = fileConverterService;
		_env = env;
	}

	public async Task<string> Handle(ConvertDocxToPdfRequest request, CancellationToken cancellationToken)
	{
		var uploadsFolder = Path.Combine(_env.WebRootPath, "files");
		Directory.CreateDirectory(uploadsFolder);

		var inputFilePath = Path.Combine(uploadsFolder, Path.GetFileName(request.File.FileName));

		using (var stream = new FileStream(inputFilePath, FileMode.Create))
		{
			await request.File.CopyToAsync(stream, cancellationToken);
		}

		var outputFilePath = await _fileConverterService.ConvertDocxToPdfAsync(inputFilePath);
		return Path.GetFileName(outputFilePath); // Return just the file name
	}
}

