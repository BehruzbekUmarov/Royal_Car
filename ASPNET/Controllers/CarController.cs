using Application.Features.CarManager.Commands;
using Application.Features.CarManager.Queries;
using ASPNET.Common.Base;
using ASPNET.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.Controllers;

[Route("api/[controller]")]
public class CarController : BaseApiController
{
	public CarController(ISender sender) : base(sender)
	{
	}

	/// <summary>
	/// Creates a new car with the provided details.
	/// </summary>
	/// <param name="request">The car creation request containing car details.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The result of the car creation operation.</returns>
	[HttpPost("CreateCar")]
	public async Task<ActionResult<ApiSuccessResult<CreateCarResult>>> CreateCarAsync(
		[FromForm] CreateCarRequest request,
		CancellationToken cancellationToken
		)
	{
		var response = await _sender.Send(request, cancellationToken);

		return Ok(new ApiSuccessResult<CreateCarResult>
		{
			Code = StatusCodes.Status200OK,
			Message = $"Success executing {nameof(CreateCarAsync)}",
			Content = response
		});
	}

	/// <summary>
	/// Updates an existing car with the specified details.
	/// </summary>
	/// <param name="request">The update request with new car data.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The result of the update operation.</returns>
	[HttpPost("UpdateCar")]
	public async Task<ActionResult<ApiSuccessResult<UpdateCarResult>>> UpdateCarAsync(
		UpdateCarRequest request,
		CancellationToken cancellationToken
		)
	{
		var response = await _sender.Send(request, cancellationToken);
		return Ok(new ApiSuccessResult<UpdateCarResult>
		{
			Code = StatusCodes.Status200OK,
			Message = $"Success executing {nameof(UpdateCarAsync)}",
			Content = response
		});
	}

	/// <summary>
	/// Deletes a car with the specified identifier.
	/// </summary>
	/// <param name="request">The delete request containing the car ID.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The result of the deletion operation.</returns>
	[HttpPost("DeleteCar")]
	public async Task<ActionResult<ApiSuccessResult<DeleteCarResult>>> DeleteCarAsync(
		DeleteCarRequest request,
		CancellationToken cancellationToken
		)
	{
		var response = await _sender.Send(request, cancellationToken);

		return Ok(new ApiSuccessResult<DeleteCarResult>
		{
			Code = StatusCodes.Status200OK,
			Message = $"Success executing {nameof(DeleteCarAsync)}",
			Content = response
		});
	}

	/// <summary>
	/// Get a filtered, paginated list of cars.
	/// </summary>
	/// <param name="isDeleted">Whether to include deleted records.</param>
	/// <param name="colors">Optional list of car colors to filter by.</param>
	/// <param name="manufacturers">Optional list of car manufacturers to filter by.</param>
	/// <param name="minPrice">Minimum price filter.</param>
	/// <param name="maxPrice">Maximum price filter.</param>
	/// <param name="page">Page number (1-based).</param>
	/// <param name="pageSize">Page size.</param>
	[HttpGet("GetCarList")]
	public async Task<ActionResult<ApiSuccessResult<GetCarListResult>>> GetCarListAsync(
	CancellationToken cancellationToken,
	[FromQuery] bool isDeleted = false,
	[FromQuery] string[]? colors = null,
	[FromQuery] string[]? manufacturers = null,
	[FromQuery] decimal? minPrice = null,
	[FromQuery] decimal? maxPrice = null,
	[FromQuery] int page = 1,
	[FromQuery] int pageSize = 10)
	{
		var request = new GetCarListRequest
		{
			IsDeleted = isDeleted,
			Colors = colors,
			Manufacturers = manufacturers,
			MinPrice = minPrice,
			MaxPrice = maxPrice,
			Page = page,
			PageSize = pageSize
		};

		var response = await _sender.Send(request, cancellationToken);

		return Ok(new ApiSuccessResult<GetCarListResult>
		{
			Code = StatusCodes.Status200OK,
			Message = $"Success executing {nameof(GetCarListAsync)}",
			Content = response
		});
	}
}
