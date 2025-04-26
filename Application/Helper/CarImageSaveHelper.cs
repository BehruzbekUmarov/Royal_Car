﻿using Application.Helper.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.Helper;

public class CarImageSaveHelper : ICarImageSaveHelper
{
	private readonly string _imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/cars");

	public async Task<string> SaveImageAsync(IFormFile image)
	{
		if (!Directory.Exists(_imageDirectory))
			Directory.CreateDirectory(_imageDirectory);

		var extension = Path.GetExtension(image.FileName);
		var fileName = $"{Guid.NewGuid()}{extension}";
		var filePath = Path.Combine(_imageDirectory, fileName);

		using (var stream = new FileStream(filePath, FileMode.Create))
		{
			await image.CopyToAsync(stream);
		}

		return $"/images/cars/{fileName}";
	}
}
