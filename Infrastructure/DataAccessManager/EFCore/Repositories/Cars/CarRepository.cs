using Application.Common.Repositories.Cars;
using Domain.Entities;
using Infrastructure.DataAccessManager.EFCore.Context;

namespace Infrastructure.DataAccessManager.EFCore.Repositories.Cars;

public class CarRepository(DataContext dataContext) : CommandRepository<Car>(dataContext), ICarRepository
{
}
