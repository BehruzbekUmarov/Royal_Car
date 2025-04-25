using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Application.Features.ConvertorManager.Queries;

public class DownloadConvertedFileQuery : IRequest<byte[]>
{
	public string FileName { get; }

	public DownloadConvertedFileQuery()
	{
	
	}
}

public class DownloadConvertedFileQueryHandler : IRequestHandler<DownloadConvertedFileQuery, byte[]>
{
	private readonly IWebHostEnvironment _env;

	public DownloadConvertedFileQueryHandler(IWebHostEnvironment env)
	{
		_env = env;
	}

	public async Task<byte[]> Handle(DownloadConvertedFileQuery request, CancellationToken cancellationToken)
	{
		var filePath = Path.Combine(_env.WebRootPath, "files", request.FileName);
		if (!File.Exists(filePath))
			return null;

		return await File.ReadAllBytesAsync(filePath, cancellationToken);
	}
}
