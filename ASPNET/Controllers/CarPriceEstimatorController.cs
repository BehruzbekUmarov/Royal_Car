using Application.Features.EstimateCarManager.Commands;
using ASPNET.Common.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.Controllers;

[Route("api/[controller]")]
public class CarPriceEstimatorController : BaseApiController
{
	public CarPriceEstimatorController(ISender sender) : base(sender)
	{
	}

	/// <summary>
	/// This endpoint accepts a car price estimation request, processes it using MediatR, and returns the estimated price and fuel type.
	/// It expects an `EstimatePriceRequest` object in the request body and responds with the calculated estimated price and the fuel type in the response body.
	/// </summary>
	/// <param name="request">The car price estimation request containing the vehicle's details such as brand, model, year, mileage, and fuel type.</param>
	/// <returns>An HTTP 200 response with the estimated car price and fuel type in the format: { "EstimatedPrice": <price>, "FuelType": <fuelType> }.</returns>
	[HttpPost]
	public async Task<IActionResult> Estimate([FromBody] EstimatePriceRequest request)
	{
		var estimatedPrice = await _sender.Send(request);
		return Ok(new { EstimatedPrice = estimatedPrice });
	}
}
