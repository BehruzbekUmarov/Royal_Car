namespace Application.Helper.Interfaces;

public interface IFileConverterHelper
{
	Task<string> ConvertDocxToPdfAsync(string inputPath);
	Task<string> ConvertPdfToDocxAsync(string inputPath);
}
