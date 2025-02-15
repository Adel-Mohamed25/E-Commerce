using Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services.IServices;
using Services.Services;
using Services.UnitOfServices;
using System.Text;

namespace Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServicesDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            #region JwtAuthenticationSettings
            var jwtSection = configuration.GetSection($"{nameof(JWTSettings)}");
            var googleSection = configuration.GetSection($"Authentication:{nameof(GoogleSettings)}");
            var facebookSection = configuration.GetSection($"Authentication:{nameof(FacebookSettings)}");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = false;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,

                    ValidIssuer = jwtSection.GetValue<string>($"{nameof(JWTSettings.Issuer)}"),
                    ValidateAudience = true,
                    ValidAudience = jwtSection.GetValue<string>($"{nameof(JWTSettings.Audience)}"),
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes(jwtSection.GetValue<string>($"{nameof(JWTSettings.Secret)}") ?? throw new InvalidOperationException("Secret key is missing")))
                };
            });
            #endregion

            #region Add Google Authentication 
            //services.AddAuthentication().AddGoogle(options =>
            //{
            //    options.ClientId = googleSection.GetValue<string>($"{nameof(GoogleSettings.ClientId)}") ?? throw new InvalidOperationException("ClientId is missing");
            //    options.ClientSecret = googleSection.GetValue<string>($"{nameof(GoogleSettings.ClientSecret)}") ?? throw new InvalidOperationException("ClientSecret is missing");
            //});
            #endregion

            #region Add Facebook Authentication 
            //services.AddAuthentication().AddFacebook(options =>
            //{
            //    options.AppId = facebookSection.GetValue<string>($"{nameof(FacebookSettings.AppId)}") ?? throw new InvalidOperationException("AppId is missing");
            //    options.AppId = facebookSection.GetValue<string>($"{nameof(FacebookSettings.AppSecret)}") ?? throw new InvalidOperationException("AppSecret is missing");
            //    options.AppSecret = "";
            //});
            #endregion

            #region Configure Swagger with JWT Authentication and able to read version correctly
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "E-Commerce API", Version = "v1" });

                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter the token like this: Bearer {your token}",
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type =  ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme,
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            #endregion

            services.AddScoped<IUnitOfService, UnitOfService>();
            services.AddScoped<IEmailServices, EmailServices>();
            services.AddScoped<IAuthServices, AuthenticationService>();

            return services;
        }
    }
}
