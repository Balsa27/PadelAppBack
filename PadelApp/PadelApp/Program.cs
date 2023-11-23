using System.Configuration;
using System.Security.Claims;
using System.Text.Json;
using DrealStudio.Application.Services;
using DrealStudio.Application.Services.Interface;
using DrealStudio.Infrastructure.Authentication;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Authentication;
using PadelApp.Application.Abstractions.Emai;
using PadelApp.Application.Abstractions.Google;
using PadelApp.Application.Abstractions.Jobs;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Application.Commands.Auth.Login;
using PadelApp.Application.Commands.Booking.AcceptBooking;
using PadelApp.Application.Commands.Booking.CreateBooking;
using PadelApp.Application.Commands.Booking.RejectBooking;
using PadelApp.Application.Commands.Court.AddCourt;
using PadelApp.Application.Commands.Court.AddCourtPrice;
using PadelApp.Application.Commands.Court.RemoveCourt;
using PadelApp.Application.Commands.Court.RemoveCourtPrice;
using PadelApp.Application.Commands.Court.UpdateCourt;
using PadelApp.Application.Commands.Court.UpdateCourtPrice;
using PadelApp.Application.Commands.Court.UpdateCourtStatus;
using PadelApp.Application.Commands.Organization.OrganizationRegister;
using PadelApp.Application.Commands.Organization.RemoveOrganization;
using PadelApp.Application.Commands.Organization.UpdateOrganization;
using PadelApp.Application.Commands.Player.AppleSignIn;
using PadelApp.Application.Commands.Player.CreateBooking;
using PadelApp.Application.Commands.Player.DeleteUser;
using PadelApp.Application.Commands.Player.GoogleSignIn;
using PadelApp.Application.Commands.Player.Login;
using PadelApp.Application.Events.Domain;
using PadelApp.Application.Handlers;
using PadelApp.Application.Queries.Booking.AllPendingBookings;
using PadelApp.Application.Queries.Booking.BookingById;
using PadelApp.Application.Queries.Booking.CourtUpcommingBookings;
using PadelApp.Application.Queries.Booking.UserPendingBookings;
using PadelApp.Application.Queries.Booking.UserUpcomingBookings;
using PadelApp.Application.Queries.Court.CourtById;
using PadelApp.Application.Queries.Court.GetOrganizationCourts;
using PadelApp.Application.Queries.Player.PlayerById;
using PadelApp.Application.Services;
using PadelApp.Domain.DomainEvents;
using PadelApp.Domain.Enums;
using PadelApp.Domain.Events.DomainEvents;
using PadelApp.Domain.Events.DomainEvents.DomainEventConverter;
using PadelApp.Infrastructure.Authentication.Google;
using PadelApp.Infrastructure.BackroundJobs;
using PadelApp.Infrastructure.BookingBackroundService;
using PadelApp.Infrastructure.Email;
using PadelApp.Infrastructure.SignalR;
using PadelApp.Middleware;
using PadelApp.Options.Authentication;
using PadelApp.Options.Authentication.Apple;
using PadelApp.Persistance.EFC;
using PadelApp.Persistance.Interceptors;
using PadelApp.Persistance.Repositories;
using PadelApp.Persistance.UnitOfWork;
using PadelApp.Presentation.Contracts.Player;
using PadelApp.Presentation.Deserializers;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

//Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();
    //.AddApple();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Player", policy =>
        policy.RequireClaim(ClaimTypes.Role, Role.Player.ToString()));

    options.AddPolicy("Organization", policy =>
        policy.RequireClaim(ClaimTypes.Role, Role.Organization.ToString()));
});

//Jwt
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();


builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IJobFactory, QuartzJobFactory>();


//Apple
//builder.Services.ConfigureOptions<AppleSignInOptions>();
//builder.Services.ConfigureOptions<AppleSignInOptionsSetup>();

builder.Services.AddHttpContextAccessor();

//Mediator
builder.Services.AddMediatR(c =>
    c.RegisterServicesFromAssemblies(typeof(Program).Assembly)
);

//events
builder.Services.AddScoped<INotificationHandler<PlayerRegisteredDomainEvent>, PlayerRegisteredDomainEventHandler>();
builder.Services.AddScoped<INotificationHandler<BookingAcceptedDomainEvent>, BookingAcceptedEventHandler>();
builder.Services.AddScoped<INotificationHandler<BookingRejectedDomainEvent>, BookingRejectedEventHandler>();
builder.Services.AddScoped<INotificationHandler<CourtStatusChangeDomainEvent>, CourtStatusChangeEventHandler>();
builder.Services.AddScoped<INotificationHandler<OrganizationRegisteredDomainEvent>, OrganizationRegisteredDomainEventHandler>();
builder.Services.AddScoped<INotificationHandler<CourtCreatedDomainEvent>, CourtCreatedDomainEventHandler>();
builder.Services.AddScoped<INotificationHandler<RemoveCourtFromOrganizationDomainEvent>, RemoveCourtFromOrganizationDomainEventHandler>();
builder.Services.AddScoped<INotificationHandler<BookingCreatedDomainEvent>, BookingCreatedDomainEventHandler>();

//Auth
builder.Services.AddScoped<IRequestHandler<GoogleSignInCommand, Result<GoogleSignInResponse>>, GoogleSignInRequestHandler>();
builder.Services.AddScoped<IRequestHandler<AppleSignInCommand, Result<AppleSignInResponse>>, AppleSignInRequestHandler>();
builder.Services.AddScoped<IRequestHandler<UserLoginCommand, UserLoginResponse>, UserLoginRequestHandler>();
builder.Services.AddScoped<IRequestHandler<PlayerRegisterCommand, Result<string>>, PlayerRegisterRequestHandler>();
builder.Services.AddScoped<IRequestHandler<OrganizationRegisterCommand, OrganizationRegisterResponse>, OrganizationRegisterRequestHandler>();

//Player
builder.Services.AddScoped<IRequestHandler<PlayerByIdCommand, PlayerByIdResponse>, PlayerByIdRequestHandler>();
builder.Services.AddScoped<IRequestHandler<DeletePlayerCommand, string>, DeletePlayerRequestHandler>();

//Court
builder.Services.AddScoped<IRequestHandler<AddCourtCommand, AddCourtResponse>, AddCourtRequestHandler>();
builder.Services.AddScoped<IRequestHandler<RemoveCourtCommand, RemoveCourtResponse>, RemoveCourtRequestHandler>();
builder.Services.AddScoped<IRequestHandler<UpdateCourtStatusCommand, UpdateCourtStatusResponse>, UpdateCourtStatusRequestHandler>();
builder.Services.AddScoped<IRequestHandler<CourtByIdCommand, CourtByIdResponse>, CourtByIdRequestHandler>();
builder.Services.AddScoped<IRequestHandler<OrganizationCourtsCommand, List<OrganizationCourtResponse>>, OrganizationCourtsRequestHandler>();
builder.Services.AddScoped<IRequestHandler<UpdateCourtCommand, UpdateCourtResponse>, UpdateCourtRequestHandler>();
builder.Services.AddScoped<IRequestHandler<UpdateCourtPriceCommand, UpdateCourtPriceResponse>, UpdateCourtPriceRequestHandler>();
builder.Services.AddScoped<IRequestHandler<RemoveCourtPriceCommand, RemoveCourtPriceResponse>, RemoveCourtPriceRequestHandler>();
builder.Services.AddScoped<IRequestHandler<AddCourtPriceCommand, AddCourtPriceResponse>, AddCourtPriceRequestHandler>();

//Booking
builder.Services.AddScoped<IRequestHandler<CreateBookingCommand, CreateBookingResponse>, CreateBookingRequestHandler>();
builder.Services.AddScoped<IRequestHandler<AcceptBookingCommand, AcceptBookingResponse>, AcceptBookingRequestHandler>();
builder.Services.AddScoped<IRequestHandler<RejectBookingCommand, RejectBookingResponse>, RejectBookingRequestHandler>();
builder.Services.AddScoped<IRequestHandler<CourtPendingBookingsCommand, List<CourtPendingBookingsResponse>?>, CourtPendingBookingsRequestHandler>();
builder.Services.AddScoped<IRequestHandler<UserPendingBookingsCommand, List<UserPendingBookingsResponse>?>, UserPendingBookingsRequestHandler>();
builder.Services.AddScoped<IRequestHandler<UserUpcomingBookingsCommand, List<UserUpcomingBookingsResponse>?>,UserUpcomingBookingsRequestHandler>();
builder.Services.AddScoped<IRequestHandler<BookingByIdCommand, BookingByIdResponse>,BookingByIdRequestHandler>();
builder.Services.AddScoped<IRequestHandler<CourtUpcomingBookingsCommand, List<CourtUpcomingBookingsResponse>?>, CourtUpcomingBookingsCommandHandler>();

//Organization
builder.Services.AddScoped<IRequestHandler<OrganizationRegisterCommand, OrganizationRegisterResponse>, OrganizationRegisterRequestHandler>();
builder.Services.AddScoped<IRequestHandler<UpdateOrganizationCommand, UpdateOrganizationResponse>, UpdateOrganizationRequestHandler>();
builder.Services.AddScoped<IRequestHandler<RemoveOrganizationCommand, RemoveOrganizationResponse>, RemoveOrganizationRequestHandler>();

//DatabaseContext
builder.Services.AddSingleton<ConvertDomainEventsToOutboxInterceptor>();
builder.Services.AddDbContext<ApplicationDbContext>((sp, optionsBuilder) =>
{
    var interceptor = sp.GetRequiredService<ConvertDomainEventsToOutboxInterceptor>();
    optionsBuilder.AddInterceptors(interceptor);
});
    
builder.Services.AddQuartz(cfg =>
{
    var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

    cfg.AddJob<ProcessOutboxMessagesJob>(jobKey)
        .AddTrigger(t =>
        t.ForJob(jobKey)
            .WithSimpleSchedule(schedule => schedule
                .WithIntervalInSeconds(10)
                .RepeatForever()));
    
    //var closeCourtJobKey = new JobKey(nameof(CloseCourtJob));
    
    // cfg.AddJob<CloseCourtJob>(j => j
    //         .WithIdentity(closeCourtJobKey)
    //         .StoreDurably()) // Marking the job as durable
    //     .AddTrigger(opts => 
    //         opts.ForJob(closeCourtJobKey) // Make sure to reference the correct job key
    //             .StartNow()
    //             .WithSimpleSchedule(x => x
    //                 .WithIntervalInSeconds(30))); // for example, run every 30 seconds
});

builder.Services.AddQuartzHostedService();
builder.Services.AddScoped<IJobFactory, QuartzJobFactory>();

//builder.Services.AddHostedService<BookingNotificationService>();

builder.Services.AddScoped<IScheduler>(s =>
{
    var schedulerFactory = s.GetRequiredService<ISchedulerFactory>();
    return schedulerFactory.GetScheduler().Result;
});

//Signal
builder.Services.AddSignalR();

//Services and Repositories
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddScoped<INotificationService, SignalRNotificationService>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();  
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<ICourtRepository, CourtRepository>();
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();  
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IGoogleTokenValidator, GoogleTokenValidator>();

//Other
builder.Services.AddControllers().AddNewtonsoftJson(o =>
{
    o.SerializerSettings.Converters.Add(new AddCourtRequestDeserializer());
    o.SerializerSettings.Converters.Add(new UpdateCourtPriceRequestDeserializer());
    o.SerializerSettings.Converters.Add(new AddCourtPriceRequestDeserializer());
});

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

app.UseMiddleware<ExceptionMiddleware>();
app.UseRouting();
app.UseAuthentication();
app.UseMiddleware<JwtMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub");
app.Run();