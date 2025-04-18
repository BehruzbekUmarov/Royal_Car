using Domain.Entities;
using Domain.Enums;
using Infrastructure.DataAccessManager.EFCore.Context;

namespace Infrastructure.SeedManager;


public static class DbInitializer
{
	public static void SeedDatabase(DataContext context)
	{
		if (!context.Car.Any())
		{
			var cars = new List<Car>
		{
			new Car 
			{ 
				Id = Guid.NewGuid(),
				Model = "Civic",
				Manufacturer = Manufacturers.Honda,
				Color = Colors.Red,
				Price = 22000,
				CreatedAtUtc = DateTime.UtcNow 
			},
			new Car 
			{
				Id = Guid.NewGuid(),
				Model = "Accord",
				Manufacturer = Manufacturers.Honda,
				Color = Colors.Blue,
				Price = 25000,
				CreatedAtUtc = DateTime.UtcNow 
			},
			new Car 
			{ 
				Id = Guid.NewGuid(),
				Model = "Corolla",
				Manufacturer = Manufacturers.BYD,
				Color = Colors.White,
				Price = 21000,
				CreatedAtUtc = DateTime.UtcNow },
			new Car 
			{ 
				Id = Guid.NewGuid(),
				Model = "Camry",
				Manufacturer = Manufacturers.Mercedes,
				Color = Colors.Black,
				Price = 26000,
				CreatedAtUtc = DateTime.UtcNow 
			},
			new Car 
			{
				Id = Guid.NewGuid(),
				Model = "Model 3",
				Manufacturer = Manufacturers.Audi,
				Color = Colors.Red,
				Price = 35000,
				CreatedAtUtc = DateTime.UtcNow 
			},
			new Car 
			{ 
				Id = Guid.NewGuid(),
				Model = "Model Y",
				Manufacturer = Manufacturers.Chevrolet,
				Color = Colors.White,
				Price = 45000,
				CreatedAtUtc = DateTime.UtcNow 
			},
			new Car 
			{
				Id = Guid.NewGuid(),
				Model = "Mustang",
				Manufacturer = Manufacturers.Chevrolet,
				Color = Colors.Red,
				Price = 40000,
				CreatedAtUtc = DateTime.UtcNow 
			},
			new Car 
			{
				Id = Guid.NewGuid(),
				Model = "F-150",
				Manufacturer = Manufacturers.Mercedes,
				Color = Colors.Blue,
				Price = 38000,
				CreatedAtUtc = DateTime.UtcNow
			},
			new Car 
			{ 
				Id = Guid.NewGuid(),
				Model = "A4",
				Manufacturer = Manufacturers.Audi,
				Color = Colors.Black,
				Price = 37000,
				CreatedAtUtc = DateTime.UtcNow 
			},
			new Car 
			{
				Id = Guid.NewGuid(),
				Model = "Q5",
				Manufacturer = Manufacturers.Audi,
				Color = Colors.White,
				Price = 42000,
				CreatedAtUtc = DateTime.UtcNow
			},
			new Car 
			{
				Id = Guid.NewGuid(),
				Model = "3 Series",
				Manufacturer = Manufacturers.BMW,
				Color = Colors.Blue,
				Price = 39000,
				CreatedAtUtc = DateTime.UtcNow
			},
			new Car 
			{
				Id = Guid.NewGuid(),
				Model = "X5",
				Manufacturer = Manufacturers.BMW,
				Color = Colors.Black,
				Price = 58000,
				CreatedAtUtc = DateTime.UtcNow
			}
		};

			context.Car.AddRange(cars);
			context.SaveChanges();
		}
	}
}
 