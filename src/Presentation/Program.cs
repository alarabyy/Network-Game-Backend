using Application.Extensions;
using DataAccess.Extensions;
using Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddDataAccessConfiguration(config);
builder.Services.AddApplicationConfiguration();
builder.Services.AddWebApiServices(config);

var app = builder.Build();

app.UseHttpsRedirection();

app.UsePresentationServices();

await DatabaseConfiguration.SeedIdentityDataAsync(app.Services);

app.Run();