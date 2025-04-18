using Application.Common.Repositories;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;

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
    private readonly ICommandRepository<Car> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCarHandler(ICommandRepository<Car> repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateCarResult> Handle(UpdateCarRequest request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id ?? Guid.Empty, cancellationToken);

        if (entity == null)
            throw new Exception($"Entity not found {request.Id}");

        entity.Manufacturer = request.Manufacturer;
        entity.Price = request.Price;
        entity.UpdatedById = request.UpdatedById;
        entity.Model = request.Model;
        entity.Color = request.Color;

        _repository.Update(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new UpdateCarResult
        {
            Data = entity
        };
    }
}
