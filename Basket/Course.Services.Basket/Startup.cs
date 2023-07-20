using Course.Services.Basket.Services;
using Course.Services.Basket.Settings;
using Course.Shared.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace Course.Services.Basket
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Kimliği doğrulanmış kullanıcıyı verir.
            var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.Authority = Configuration["IdentityServerURL"];
                        options.Audience = "resource_basket";
                        options.RequireHttpsMetadata = true;
                    });

            // IHttpContextAccesor interface'ni kullanabilmek için burada eklememiz gerekmektedir.
            services.AddHttpContextAccessor();

            services.Configure<RedisSettings>(Configuration.GetSection("RedisSettings"));

            services.AddControllers(options =>
            {
                options.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy));
            });

            // Geriye redis service döndermek için içerisine girerek redis implementasyonu yapıyoruz.
            services.AddSingleton<RedisService>(sp =>
            {
                // Configuration dosyalarını okuyoruz.
                var settings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;

                // Redis service oluşturuyoruz.
                var redis = new RedisService(settings.Host, settings.Port);

                // Redis'e bağlanıyor.
                redis.Connect();

                return redis;
            });

            services.AddScoped<IBasketService, BasketService>();

            // Identity bilgilerini alabilmek için service
            services.AddScoped<ISharedIdentityService, SharedIdentityService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Course.Services.Basket", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Course.Services.Basket v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
