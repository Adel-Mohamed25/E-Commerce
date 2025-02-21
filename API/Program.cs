using Application.Extensions;
using Application.Middlewares;
using Infrastructure.Extensions;
using Infrastructure.Seeders;
using Infrastructure.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using Services.Extensions;

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

var app = builder.Build();

//Data Seeder 
using var scop = app.Services.CreateScope();
var services = scop.ServiceProvider;
var logger = services.GetRequiredService<ILogger<Program>>();
try
{
    var unitOfWork = services.GetRequiredService<IUnitOfWork>();
    await RoleSeeder.SeedRoleAsync(unitOfWork);
}
catch (Exception ex)
{
    logger.LogError(ex, "Error occurred while seeding roles");
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleWare>();
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
