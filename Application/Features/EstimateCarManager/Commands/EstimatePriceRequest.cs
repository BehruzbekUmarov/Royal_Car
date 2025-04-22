using FluentValidation;
using MediatR;

namespace Application.Features.EstimateCarManager.Commands;

public class EstimatePriceRequest : IRequest<decimal>
{
	public string Brand { get; set; }
	public string Model { get; set; }
	public int Year { get; set; }
	public int Mileage { get; set; }
	public string FuelType { get; set; }
}

public class EstimatePriceRequestValidator : AbstractValidator<EstimatePriceRequest>
{
	public EstimatePriceRequestValidator()
	{
		RuleFor(x => x.Brand)
			.NotEmpty().WithMessage("Brand is required.")
			.Length(2, 50).WithMessage("Brand name should be between 2 and 50 characters.");

		RuleFor(x => x.Model)
			.NotEmpty().WithMessage("Model is required.")
			.Length(1, 50).WithMessage("Model name should be between 1 and 50 characters.");

		RuleFor(x => x.Year)
			.GreaterThan(1900).WithMessage("Year should be greater than 1900.")
			.LessThanOrEqualTo(DateTime.Now.Year).WithMessage("Year should not be in the future.");

		RuleFor(x => x.Mileage)
			.GreaterThanOrEqualTo(0).WithMessage("Mileage must be a positive number.")
			.LessThanOrEqualTo(1_000_000).WithMessage("Mileage can't exceed 1,000,000.");

		RuleFor(x => x.FuelType)
			.NotEmpty().WithMessage("Fuel type is required.")
			.Must(fuel => fuel.ToLower() == "electric" || fuel.ToLower() == "diesel")
			.WithMessage("Fuel type must be either 'electric', 'diesel'.");
	}
}

public class EstimatePriceHandler : IRequestHandler<EstimatePriceRequest, decimal>
{
	public async Task<decimal> Handle(EstimatePriceRequest request, CancellationToken cancellationToken)
	{
		// Mock logic based on simple rules
		decimal basePrice = 20000;

		// Age discount
		int age = DateTime.Now.Year - request.Year;
		basePrice -= age * 800;

		// Mileage discount
		if (request.Mileage > 100000)
			basePrice -= 3000;
		else if (request.Mileage > 50000)
			basePrice -= 1500;

		// Fuel type bonus
		if (request.FuelType.ToLower() == "electric")
			basePrice += 3000;
		else if (request.FuelType.ToLower() == "diesel")
			basePrice -= 1000;

		// Brand popularity multiplier (mock)
		if (request.Brand.ToLower() == "bmw" || request.Brand.ToLower() == "mercedes")
			basePrice += 2000;

		return Math.Max(basePrice, 2000); // Don't go below 2000
	}
}
