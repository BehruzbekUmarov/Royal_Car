using Application.Features.CarManager.Commands;
using Application.Features.CarManager.Queries;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings.Profiles;

public class CarProfile : Profile
{
    public CarProfile()
    {
        CreateMap<Car, GetCarListDto>();
        CreateMap<CreateCarRequest, Car>();
        CreateMap<UpdateCarRequest, Car>();
        CreateMap<DeleteCarRequest, Car>();
    }
}
