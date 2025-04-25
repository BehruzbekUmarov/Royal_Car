namespace Application.Common.Repositories.Files;

public interface IFileConverterService
{
	Task<string> ConvertDocxToPdfAsync(string inputPath);
	Task<string> ConvertPdfToDocxAsync(string inputPath);
}
