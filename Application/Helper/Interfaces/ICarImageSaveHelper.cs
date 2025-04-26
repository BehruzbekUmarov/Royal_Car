using Microsoft.AspNetCore.Http;

namespace Application.Helper.Interfaces;

public interface ICarImageSaveHelper
{
	Task<string> SaveImageAsync(IFormFile image);
}
