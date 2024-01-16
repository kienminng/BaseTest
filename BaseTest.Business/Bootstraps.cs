using System.Text;
using BaseTest.Businiss.IService;
using BaseTest.Businiss.MailService;
using BaseTest.Businiss.ProductService;
using BaseTest.Businiss.Service;
using BaseTest.Common;
using BaseTest.Models.Entities;
using BaseTest.Models.Form;
using BaseTest.Repository;
using BaseTest.Repository.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;

namespace BaseTest.Businiss;

public class Bootstraps
{
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IAppDbContext>(service => new AppDbContext(new DbContextOptionsBuilder()
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .UseSqlServer(configuration.GetConnectionString(Constant.AppSettingKeys.DEFAULT_CONNECTION))
            .Options
        ));
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
            
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration.GetSection(Constant.AppSettingKeys.SECRET_KEY).Value!))
                };
            });

        var emailConfig = configuration.GetSection("EmailInformation").Get<MailInformation>();
        services.AddSingleton(emailConfig);

        services.AddScoped<IAppDbContext, AppDbContext>();
        services.AddScoped<IMailService, MailService.MailService>();
        services.AddScoped<ITokenService,TokenService>();
        services.AddScoped<IUserCardService,UserCardService>();
        services.AddScoped<IProductService, ProductService.ProductService>();
        RegisterRepositoryDependencies(services);
    }

    private static void RegisterRepositoryDependencies(IServiceCollection services)
    {
        services.AddScoped<IBaseRepository<UserCard>>(service =>
            new BaseRepository<UserCard>(service.GetService<IAppDbContext>() ?? throw new InvalidOperationException()));
        services.AddScoped<IBaseRepository<Product>>(service =>
            new BaseRepository<Product>( service.GetService<IAppDbContext>() ?? throw new InvalidOperationException()));
        services.AddScoped<IBaseRepository<Token>>(service =>
            new BaseRepository<Token>(service.GetService<IAppDbContext>() ?? throw new InvalidOperationException()));
        services.AddScoped<IBaseRepository<Roles>>(service =>
            new BaseRepository<Roles>(service.GetService<IAppDbContext>() ?? throw new InvalidOperationException()));
    }
}