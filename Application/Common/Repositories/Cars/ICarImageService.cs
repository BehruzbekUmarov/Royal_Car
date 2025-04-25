using Microsoft.AspNetCore.Http;

namespace Application.Common.Repositories.Cars;

public interface ICarImageService
{
	Task<string> SaveImageAsync(IFormFile image);
}
