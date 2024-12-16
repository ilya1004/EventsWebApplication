using EventsWebApplication.API;
using EventsWebApplication.API.Extensions;
using EventsWebApplication.API.Middlewares;
using EventsWebApplication.API.Utils;
using EventsWebApplication.Application;
using EventsWebApplication.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllers();

services.AddEndpointsApiExplorer()
        .AddSwaggerGen();

services.AddApplication(builder.Configuration);
services.AddPersistence(builder.Configuration);
services.AddAPI(builder.Configuration);

services.AddTransient<GlobalExceptionHandlingMiddleware>();


//services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        //options.Authority = "http://eventwebapp_identityserver";
//        //options.Audience = "http://eventwebapp_identityserver/resources";

//        options.Authority = "http://localhost:7013";
//        options.Audience = "http://localhost:7013/resources";
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidIssuer = "http://localhost:7013",
//            //ValidIssuer = "http://eventwebapp_identityserver",
//            ValidateLifetime = true,
//        };
//        options.RequireHttpsMetadata = false; // Уберите, если в dev-среде

//        options.Events = new JwtBearerEvents
//        {
//            OnMessageReceived = context =>
//            {
//                Console.WriteLine("Token received: " + context.Token);
//                return Task.CompletedTask;
//            },
//            OnAuthenticationFailed = context =>
//            {
//                Console.WriteLine("Authentication failed: " + context.Exception);
//                return Task.CompletedTask;
//            },
//            OnTokenValidated = context =>
//            {
//                Console.WriteLine("Token successfully validated");
//                return Task.CompletedTask;
//            }
//        };

//    });

var identityBase = builder.Configuration["AUTHORITY_URI"] ?? "http://localhost:7013";
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
        .AddJwtBearer(opt =>
        {
            opt.Authority = identityBase;
            opt.RequireHttpsMetadata = false;
            opt.Audience = $"{identityBase}/resources";
        });

//services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.Authority = "http://localhost:7013";
//        options.Audience = "http://localhost:7013/resources";
//        options.RequireHttpsMetadata = false; // Уберите, если dev-среда
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidIssuer = "http://localhost:7013",
//            ValidateAudience = true,
//            ValidAudience = "http://localhost:7013/resources",
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true, // Проверка подписи
//        };
//    });

//builder.Services.AddAuthentication(opt =>
//    {
//        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    })
//        .AddJwtBearer(opt =>
//        {
//            opt.Authority = "http://localhost:7013";
//            opt.RequireHttpsMetadata = false;
//            opt.Audience = "http://localhost:7013/resources";
//        });




services.AddAuthorizationBuilder()
    .AddPolicy(AuthPolicies.UserPolicy, policy =>
    {
        policy.RequireRole("User");
        policy.RequireClaim(ClaimTypes.NameIdentifier);
    })
    .AddPolicy(AuthPolicies.AdminPolicy, policy =>
    {
         policy.RequireRole("Admin");
    });

services.AddCors(options =>
{
    options.AddPolicy("ReactClientCors", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://eventwebapp.client:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});


builder.Host.UseSerilog();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(7012);
    // options.ListenAnyIP(7002, listenOptions => listenOptions.UseHttps()); 
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MakeMigrations();
    app.CreateContainerIfNotExist(builder.Configuration);
}

app.UseRouting();

app.UseCors("ReactClientCors");

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();


app.Run();

Log.CloseAndFlush();