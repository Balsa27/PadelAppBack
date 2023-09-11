using DrealStudio.Application.Services;
using DrealStudio.Application.Services.Interface;
using DrealStudio.Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Authentication;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Application.Handlers;
using PadelApp.Options.Authentication;
using PadelApp.Persistance.EFC;
using PadelApp.Persistance.Repositories;
using PadelApp.Persistance.UnitOfWork;
using PadelApp.Presentation.Contracts.Player;

var builder = WebApplication.CreateBuilder(args);

//Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>(); 

//Mediator
builder.Services.AddMediatR(c =>
    c.RegisterServicesFromAssemblies(typeof(Program).Assembly)
);
builder.Services.AddScoped<IRequestHandler<PlayerRegisterCommand, Result<string>>, PlayerRegisterRequestHandler>();

//DatabaseContext
builder.Services.AddDbContext<ApplicationDbContext>(ServiceLifetime.Singleton);

//Services and Repositories
builder.Services.AddSingleton<IUserContextService, UserContextService>();
builder.Services.AddSingleton<IPlayerRepository, PlayerRepository>();
builder.Services.AddSingleton<IUnitOfWork, UnitOfWork>();


//Other
builder.Services.AddHttpContextAccessor();  
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Padel App Montenegro", Version = "v1" });
    c.CustomSchemaIds(type => type.FullName);
});

var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
app.UseCors(corsPolicyBuilder 
    => corsPolicyBuilder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()); 
app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Padel App Development");
    });
// }

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();