using Application;
using ASPNET.Common.Handlers;
using ASPNET.Common.Middlewares;
using Infrastructure;
using Infrastructure.DataAccessManager.EFCore;
using Infrastructure.DataAccessManager.EFCore.Context;
using Infrastructure.SeedManager;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
	options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddInfrastructureServices(configuration);
builder.Services.AddApplicationServices();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
	options.UseInlineDefinitionsForEnums(); // Shows names instead of numbers

	var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
	DbInitializer.SeedDatabase(dbContext);
}

app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
