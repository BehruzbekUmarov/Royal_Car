using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Colors
{
	Grey = 0,
	Blue = 1,
	Black = 2,
	White = 3,
	Red = 4,
	Purple = 5
}
