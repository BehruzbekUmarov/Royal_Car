using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Car : BaseEntity
{
	public Manufacturers Manufacturer { get; set; }
    public string? Model { get; set; }
    public Colors Color { get; set; }
    public decimal Price { get; set; }      
}
