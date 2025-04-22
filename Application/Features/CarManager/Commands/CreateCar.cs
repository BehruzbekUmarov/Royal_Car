using Application.Common.Repositories;
using Application.Common.Repositories.Cars;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;
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
	private readonly IMapper _mapper;

	public CreateCarHandler(IUnitOfWork unitOfWork,
        IMapper mapper)
	{
		_unitOfWork = unitOfWork;
        _mapper = mapper;
	}
	public async Task<CreateCarResult> Handle(CreateCarRequest request, CancellationToken cancellationToken)
    {
        var entity = new Car();

        _mapper.Map(request, entity);

        //entity.Model = request.Model;
        //entity.Color = request.Color;
        //entity.Price = request.Price;
        //entity.Manufacturer = request.Manufacturer;

        await _unitOfWork.CarRepository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new CreateCarResult
        {
            Data = entity
        };
    }
}
