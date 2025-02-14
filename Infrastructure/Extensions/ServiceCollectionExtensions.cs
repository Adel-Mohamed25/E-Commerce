using Domain.Entities.Identity;
using Domain.IRepositories;
using Domain.IRepositories.IIdentityRepository;
using Infrastructure.Caching;
using Infrastructure.Repositories;
using Infrastructure.Repositories.IdentityRepository;
using Infrastructure.Settings;
using Infrastructure.UnitOfWorks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.DBConnection;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                     options.UseSqlServer(configuration.GetConnectionString("ECommerceConnection")));

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedAccount = true;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 8;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.AllowedForNewUsers = true;

            }).AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();

            services.AddStackExchangeRedisCache(option =>
            {
                option.Configuration = configuration.GetConnectionString("Redis");
                option.InstanceName = "EntityInstance";
            });

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IJwtTokenRepository, JwtTokenRepository>();
            services.AddScoped<IRedisCacheService, RedisCacheService>();


            services.Configure<EmailSettings>(configuration.GetSection("EmailSttings"));
            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
            services.Configure<GoogleSettings>(configuration.GetSection($"Authentication:{nameof(GoogleSettings)}"));
            services.Configure<FacebookSettings>(configuration.GetSection($"Authentication:{nameof(FacebookSettings)}"));


            return services;
        }
    }
}
