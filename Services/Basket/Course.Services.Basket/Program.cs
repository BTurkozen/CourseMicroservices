using Course.Services.Basket.Services;
using Course.Services.Basket.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Kimliği doğrulanmış kullanıcıyı verir.
var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy));
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

// Identity içindeki Claim nesnelerinin Mapleme işlemini kaldırıyoruz.
// NameIdentifier yerine sub olarak gelmesini sağladık kendi simlendirme işlemlerini es geçmesini sağladık. 
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = builder.Configuration["IdentityServerURL"];
                    options.Audience = "resource_basket";
                    options.RequireHttpsMetadata = false;
                });

builder.Services.AddHttpContextAccessor();

builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("RedisSettings"));

builder.Services.AddSingleton<RedisService>(sp =>
{
    // Configuration dosyalarını okuyoruz.
    var settings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;

    // Redis service oluşturuyoruz.
    var redis = new RedisService(settings.Host, settings.Port);

    // Redis'e bağlanıyor.
    redis.Connect();

    return redis;
});

builder.Services.AddScoped<IBasketService, BasketService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthentication();

app.MapControllers();

app.Run();