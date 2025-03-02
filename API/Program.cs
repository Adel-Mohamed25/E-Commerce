using Application.Extensions;
using Application.Middlewares;
using Infrastructure.Constants;
using Infrastructure.Extensions;
using Infrastructure.Seeders;
using Infrastructure.UnitOfWorks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Services.Extensions;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// AddDependenciesInjection
builder.Services.AddInfrastructureDependencies(builder.Configuration)
                .AddApplicationDependencies()
                .AddServicesDependencies(builder.Configuration);




// Enable multi-version support
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});

// add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
    });
});

// add localization settings
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en-US"),
        new CultureInfo("ar-EG")
    };

    options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.ApplyCurrentCultureToResponseHeaders = true;
    options.RequestCultureProviders.Insert(0, new AcceptLanguageHeaderRequestCultureProvider());

});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Permissions.Categories.Create, policy => policy.RequireClaim("Permission", Permissions.Categories.Create));
    options.AddPolicy(Permissions.Categories.View, policy => policy.RequireClaim("Permission", Permissions.Categories.View));
    options.AddPolicy(Permissions.Categories.Edit, policy => policy.RequireClaim("Permission", Permissions.Categories.Edit));
    options.AddPolicy(Permissions.Categories.Delete, policy => policy.RequireClaim("Permission", Permissions.Categories.Delete));
});



var app = builder.Build();
app.UseCors("AllowAll");


//Data Seeder 

using var scop = app.Services.CreateScope();
var services = scop.ServiceProvider;
var loggerFactory = services.GetRequiredService<ILoggerFactory>();
var logger = loggerFactory.CreateLogger("app");
try
{
    var unitOfWork = services.GetRequiredService<IUnitOfWork>();
    await DefaultRoles.SeedAsync(unitOfWork);
    await DefaultUsers.SeedBasicUserAsync(unitOfWork);
    await DefaultUsers.SeedAdminUserAsync(unitOfWork);
    await DefaultUsers.SeedSuperAdminUserAsync(unitOfWork);
    logger.LogInformation("Finished Seeding Default Data");
    logger.LogInformation("Application Starting");
}
catch (Exception ex)
{
    logger.LogWarning(ex, "An error occurred seeding the DB");
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleWare>();

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
