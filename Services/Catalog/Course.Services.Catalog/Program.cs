using Course.Services.Catalog.Dtos;
using Course.Services.Catalog.Services;
using Course.Services.Catalog.Settings;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter());
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(options =>
{
    options.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQUrl"], "/", host =>
        {
            host.Username("guest");
            host.Password("guest");
        });
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            // Bu microservice kimin dağıttığı bilgisini vereceğiz.
            // Token Dağıtmaktan görevli arkadaş.
            // IdentityServer Url'lini veriyoruz.
            options.Authority = builder.Configuration["IdentityServerURL"];

            // Audience Parametrelerini belirtiyoruz.
            // Tekdir Birden fazla belirtilemez.
            options.Audience = "resource_catalog";

            // Https kullanılmadığı için burada belirtiyoruz.
            options.RequireHttpsMetadata = true;

        });

// Typeof içerisine Startup verdiğimiz de Assembly de olan bütün mapperları bulup ekleyecek.
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// Option Pattern uyguluyoruz.
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

// Değişmeyen değerler olduğu için Singleton olarak kullandık.
builder.Services.AddSingleton<IDatabaseSettings>(sp =>
{
    return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
});

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICourseService, CourseService>();


var app = builder.Build();

using var scope = app.Services.CreateScope();

var serviceProvider = scope.ServiceProvider;

var categoryService = serviceProvider.GetRequiredService<ICategoryService>();

if (!(await categoryService.GetAllAsync()).Data.Any())
{
    await categoryService.CreateAsync(new CategoryCreateDto { Name = "Asp.net Core Kursu" });
    await categoryService.CreateAsync(new CategoryCreateDto { Name = "Asp.net Core API Kursu" });
}

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