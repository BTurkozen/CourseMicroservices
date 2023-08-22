using Course.Services.Order.Infrastructure;
using Course.Shared.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static System.Net.Mime.MediaTypeNames;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc.Authorization;
using MediatR;
using MassTransit;
using Course.Services.Order.Application.Consumers;

var builder = WebApplication.CreateBuilder(args);

var requeireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter(requeireAuthorizePolicy));
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(ms =>
{
    // Consumer ekliyoruz.
    ms.AddConsumer<CreateOrderMessageCommandConsumer>();

    ms.AddConsumer<CourseNameChangeEventConsumer>();

    // Varsayılan Port : 5672
    // Management Varsayılan portu olarak : 15672
    ms.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQUrl"], "/", host =>
        {
            // RabitMq tarfından varsayılan olarak geliyor.
            host.Username("guest");
            host.Password("guest");
        });

        // Hangi Endpointten dataları alıcağız.
        cfg.ReceiveEndpoint("create-order-service", e =>
        {
            e.ConfigureConsumer<CreateOrderMessageCommandConsumer>(context);
        });

        cfg.ReceiveEndpoint("course-name-changed-event-order-service", e =>
        {
            e.ConfigureConsumer<CourseNameChangeEventConsumer>(context);
        });


    });
});

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["IdentityServerURL"];
    options.Audience = "resource_order";
    options.RequireHttpsMetadata = false;
});

builder.Services.AddDbContext<OrderDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), configure =>
    {
        configure.MigrationsAssembly("Course.Services.Order.Infrastructure");
    });
});

// MediatR ile ilgili handlerları belirtmemiz gerekmektedir.
// Handler class'ının assembly'si alıyoruz.
builder.Services.AddMediatR(typeof(Course.Services.Order.Application.Handlers.CreateOrderCommandHandler).Assembly);

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();

var app = builder.Build();

using var scope = app.Services.CreateScope();

var serviceProvider = scope.ServiceProvider;

var orderDbContext = serviceProvider.GetRequiredService<OrderDbContext>();

orderDbContext.Database.Migrate();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
