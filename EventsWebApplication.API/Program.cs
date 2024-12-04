
using EventsWebApplication.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddPersistence(builder.Configuration);

var app = builder.Build();

app.Run();
