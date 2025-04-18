using Domain.Entities;
using Infrastructure.DataAccessManager.EFCore.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccessManager.EFCore.Configurations;

public class CarConfiguration : BaseEntityConfiguration<Car>
{
	public override void Configure(EntityTypeBuilder<Car> builder)
	{
		base.Configure(builder);

		builder.Property(x => x.Price).IsRequired(true);
		builder.Property(x => x.Manufacturer).IsRequired(true);
		builder.Property(x => x.Color).IsRequired(true);
		builder.Property(x => x.Model).IsRequired(true);
	}
}
