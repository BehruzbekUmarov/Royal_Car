using Application.Common.CQS.Queries;
using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CarManager.Queries;

public record GetFilterDto
{
    public string[]? Colors { get; set; }
    public string[]? Manufacturers { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}

public class GetPaginationDto
{
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPage { get; set; }

    public GetPaginationDto()
    {
        
    }

    public GetPaginationDto(int totalItems, int page = 1, int pageSize = 10)
    {
        int totalPage = (int)Math.Ceiling(totalItems/(decimal)pageSize);
        int currentPage = page;

        TotalItems = totalItems;
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalPage = totalPage;
    }
}

public record GetCarListDto
{
	public Guid? Id { get; set; }
	public Manufacturers Manufacturer { get; set; }
	public string? Model { get; set; }
	public Colors Color { get; set; }
	public decimal Price { get; set; }
	public DateTime? CreatedAtUtc { get; set; }
}

public class GetCarListResult
{
    public List<GetCarListDto>? Data { get; init; }
    public GetFilterDto Filter { get; set; }
    public GetPaginationDto Pagination { get; set; }
}

public class GetCarListRequest : IRequest<GetCarListResult>
{
	public bool IsDeleted { get; init; } = false;

	public string[]? Colors { get; init; }
	public string[]? Manufacturers { get; init; }
	public decimal? MinPrice { get; init; }
	public decimal? MaxPrice { get; init; }

	public int Page { get; init; } = 1;
	public int PageSize { get; init; } = 10;
}

public class GetCarListHandler : IRequestHandler<GetCarListRequest, GetCarListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetCarListHandler(IMapper mapper, IQueryContext context)
    {
        _mapper = mapper;
        _context = context;
    }

	public async Task<GetCarListResult> Handle(GetCarListRequest request, CancellationToken cancellationToken)
	{
		var query = _context
			.Car
			.AsNoTracking()
			.ApplyIsDeletedFilter(request.IsDeleted)
			.AsQueryable();

		// Apply filtering
		if (request.Colors?.Any() == true)
		{
			var colorEnums = request.Colors.Select(c => Enum.Parse<Colors>(c)).ToList();
			query = query.Where(c => colorEnums.Contains(c.Color));
		}

		if (request.Manufacturers?.Any() == true)
		{
			var manufacturerEnums = request.Manufacturers.Select(m => Enum.Parse<Manufacturers>(m)).ToList();
			query = query.Where(c => manufacturerEnums.Contains(c.Manufacturer));
		}

		if (request.MinPrice.HasValue)
			query = query.Where(c => c.Price >= request.MinPrice.Value);

		if (request.MaxPrice.HasValue)
			query = query.Where(c => c.Price <= request.MaxPrice.Value);

		// Total count for pagination
		var totalItems = await query.CountAsync(cancellationToken);

		// Pagination
		query = query
			.OrderBy(c => c.Id)
			.Skip((request.Page - 1) * request.PageSize)
			.Take(request.PageSize);

		// Map to DTOs
		var entities = await query.ToListAsync(cancellationToken);
		var dtos = _mapper.Map<List<GetCarListDto>>(entities);

		// Build result
		return new GetCarListResult
		{
			Data = dtos,
			Pagination = new GetPaginationDto(totalItems, request.Page, request.PageSize),
			Filter = new GetFilterDto
			{
				Colors = request.Colors,
				Manufacturers = request.Manufacturers,
				MinPrice = request.MinPrice,
				MaxPrice = request.MaxPrice
			}
		};
	}
}
