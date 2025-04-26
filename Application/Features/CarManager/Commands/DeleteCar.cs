using Application.Common.Repositories;
using AutoMapper;
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
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

	public DeleteCarHandler(IUnitOfWork unitOfWork,
        IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}
	public async Task<DeleteCarResult> Handle(DeleteCarRequest request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.CarRepository.GetAsync(request.Id ?? Guid.Empty, cancellationToken);

        if (entity == null)
            throw new Exception($"Entity not found: {request.Id}");

        _mapper.Map(request, entity);

        _unitOfWork.CarRepository.Delete(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new DeleteCarResult
        {
            Data = entity
        };
    }
}
