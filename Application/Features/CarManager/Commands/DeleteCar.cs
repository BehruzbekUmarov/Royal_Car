using Application.Common.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.CarManager.Commands;

public class DeleteCarResult
{
    public Car? Data { get; set; }
}

public class DeleteCarRequest : IRequest<DeleteCarResult>
{
    public Guid? Id { get; set; }
    public string? DeletedById { get; set; }
}

public class DeleteCarValidator : AbstractValidator<DeleteCarRequest>
{
    public DeleteCarValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteCarHandler : IRequestHandler<DeleteCarRequest, DeleteCarResult>
{
    private readonly ICommandRepository<Car> _commandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCarHandler(ICommandRepository<Car> commandRepository,
        IUnitOfWork unitOfWork)
    {
        _commandRepository = commandRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<DeleteCarResult> Handle(DeleteCarRequest request, CancellationToken cancellationToken)
    {
        var entity = await _commandRepository.GetAsync(request.Id ?? Guid.Empty, cancellationToken);

        if (entity == null)
            throw new Exception($"Entity not found: {request.Id}");

        entity.UpdatedById = request.DeletedById;

        _commandRepository.Delete(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new DeleteCarResult
        {
            Data = entity
        };
    }
}
