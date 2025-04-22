using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Manufacturers
{
	Chevrolet = 0,
	Mercedes = 1,
	BMW = 2,
	Audi = 3,
	BYD = 4,
	Honda = 5
}
