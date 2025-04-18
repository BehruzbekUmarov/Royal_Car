using Application.Common.Repositories;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Features.CarManager.Commands;

public class CreateCarResult
{
    public Car? Data { get; set; }
}

public class CreateCarRequest : IRequest<CreateCarResult>
{
    public Manufacturers Manufacturer { get; set; }
    public string? Model { get; set; }
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
    private readonly ICommandRepository<Car> _commandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCarHandler(ICommandRepository<Car> commandRepository,
        IUnitOfWork unitOfWork)
    {
        _commandRepository = commandRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<CreateCarResult> Handle(CreateCarRequest request, CancellationToken cancellationToken)
    {
        var entity = new Car();

        entity.Model = request.Model;
        entity.Color = request.Color;
        entity.Price = request.Price;
        entity.Manufacturer = request.Manufacturer;

        await _commandRepository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new CreateCarResult
        {
            Data = entity
        };
    }
}
