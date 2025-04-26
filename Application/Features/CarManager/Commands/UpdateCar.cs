using Application.Common.Repositories;
using Application.Helper.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.CarManager.Commands;

public class UpdateCarResult
{
    public Car? Data { get; set; }
}

public class UpdateCarRequest : IRequest<UpdateCarResult>
{
    public Guid? Id { get; set; }
    public Manufacturers Manufacturer { get; set; }
    public string? Model { get; set; }
    public Colors Color { get; set; }
    public decimal Price { get; set; }
	public IFormFile? Image { get; set; }
	public string? UpdatedById { get; set; }
}

public class UpdateCarValidator : AbstractValidator<UpdateCarRequest>
{
    public UpdateCarValidator()
    {
		RuleFor(x => x.Id)
			.NotEmpty().WithMessage("Car ID is required for update.");

		RuleFor(x => x.Price)
			.NotEmpty().WithMessage("Price is required.")
			.GreaterThan(0).WithMessage("Price must be greater than zero.");

		RuleFor(x => x.Manufacturer)
			.IsInEnum().WithMessage("Manufacturer must be a valid enum value.");

		RuleFor(x => x.Model)
			.NotEmpty().WithMessage("Model is required.")
			.MaximumLength(50).WithMessage("Model must not exceed 50 characters.");

		RuleFor(x => x.Color)
			.IsInEnum().WithMessage("Color must be a valid enum value.");
	}
}

public class UpdateCarHandler : IRequestHandler<UpdateCarRequest, UpdateCarResult>
{
    private readonly IUnitOfWork _unitOfWork;
	private readonly ICarImageSaveHelper _carImageSaveHelper;
	private readonly IMapper _mapper;

	public UpdateCarHandler(IUnitOfWork unitOfWork, IMapper mapper, ICarImageSaveHelper carImageSaveHelper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_carImageSaveHelper = carImageSaveHelper;
	}

	public async Task<UpdateCarResult> Handle(UpdateCarRequest request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.CarRepository.GetAsync(request.Id ?? Guid.Empty, cancellationToken);

        if (entity == null)
            throw new Exception($"Entity not found {request.Id}");

        var imagePath = await _carImageSaveHelper.SaveImageAsync(request.Image);

        entity.ImagePath = imagePath;
        _mapper.Map(request, entity);

        _unitOfWork.CarRepository.Update(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new UpdateCarResult
        {
            Data = entity
        };
    }
}
