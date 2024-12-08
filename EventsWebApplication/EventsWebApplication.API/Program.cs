using EventsWebApplication.API;
using EventsWebApplication.API.Middlewares;
using EventsWebApplication.Application;
using EventsWebApplication.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllers();

services.AddEndpointsApiExplorer()
        .AddSwaggerGen();

services.AddApplication();
services.AddPersistence(builder.Configuration);
services.AddAPI(builder.Configuration);

services.AddTransient<GlobalExceptionHandlingMiddleware>();

//services.AddAuthentication(options =>
//{
//    //options.DefaultScheme =
//    options.DefaultChallengeScheme = "oidc";
//})
//    .AddOpenIdConnect("oidc", options =>
//    {
//        options.Authority = builder.Configuration.GetRequiredSection("ServiceUrls:IdentityAPI").Value;
//        options.GetClaimsFromUserInfoEndpoint = true;
//        options.ClientId = "events";
//        options.ClientSecret = "secret";
//        options.ResponseType = "code";

//        options.TokenValidationParameters.NameClaimType = "name";
//        options.TokenValidationParameters.RoleClaimType = "role";
//        options.Scope.Add("events_scope");
//        options.SaveTokens = true;
//    });

services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = builder.Configuration.GetRequiredSection("ServiceUrls:IdentityAPI").Value;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
        };
    });

services.AddAuthorizationBuilder()
    .AddPolicy("User", policy =>
    {
        policy.RequireRole("User");
    })
    .AddPolicy("Admin", policy =>
    {
         policy.RequireRole("Admin");
    });

services.AddCors(options =>
{
    options.AddPolicy("ReactClientCors", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});


builder.Host.UseSerilog();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors("ReactClientCors");

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();


app.Run();

Log.CloseAndFlush();