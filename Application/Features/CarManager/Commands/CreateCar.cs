using Application.Common.Repositories;
using Application.Common.Repositories.Cars;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Application.Features.CarManager.Commands;

public class CreateCarResult
{
    public Car? Data { get; set; }
}

public class CreateCarRequest : IRequest<CreateCarResult>
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public Manufacturers Manufacturer { get; set; }
    public string? Model { get; set; }
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public Colors Color { get; set; }
    public decimal Price { get; set; }
	public IFormFile? Image { get; set; }
}

public class CreateCarValidator : AbstractValidator<CreateCarRequest>
{
    public CreateCarValidator()
    {
        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price is required.")
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(x => x.Manufacturer)
            .IsInEnum().WithMessage("Manufacturer must be a valid value.");

        RuleFor(x => x.Model)
            .NotEmpty().WithMessage("Model is required.")
            .MaximumLength(50).WithMessage("Model must not exceed 50 characters.");

        RuleFor(x => x.Color)
            .IsInEnum().WithMessage("Color must be a valid value.");
    }
}

public class CreateCarHandler : IRequestHandler<CreateCarRequest, CreateCarResult>
{
	private readonly IUnitOfWork _unitOfWork;
    private readonly ICarImageService _imageService;
	private readonly IMapper _mapper;

	public CreateCarHandler(IUnitOfWork unitOfWork,
		IMapper mapper,
		ICarImageService imageService)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_imageService = imageService;
	}
	public async Task<CreateCarResult> Handle(CreateCarRequest request, CancellationToken cancellationToken)
    {
        var imagePath = await _imageService.SaveImageAsync(request.Image);
        var entity = new Car();
        entity.ImagePath = imagePath;

        _mapper.Map(request, entity);

        await _unitOfWork.CarRepository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new CreateCarResult
        {
            Data = entity
        };
    }
}
