using Common.Logging;
using Customer.API.Controllers;
using Customer.API.Persistence;
using Customer.API.Repositories.Interfaces;
using Customer.API.Repositories;
using Customer.API.Services.Interfaces;
using Customer.API.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Customer.API;

Log.Logger = new LoggerConfiguration()
.WriteTo.Console()
.CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Serilogger.Configure);


Log.Information($"Start {builder.Environment.ApplicationName} up");

try
{
    // Add services to the container.
    builder.Host.UseSerilog(Serilogger.Configure);
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
    builder.Services.AddDbContext<CustomerContext>(
        options => options.UseNpgsql(connectionString));
    builder.Services.AddScoped<ICustomerRepository, CustomerRepository>()
        .AddScoped<ICustomerService, CustomerService>();

    var app = builder.Build();

    app.UseRouting();

    app.MapGet("/", () => $"Welcome to {builder.Environment.ApplicationName}!");

    app.MapCustomersAPI();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json",
                $"{builder.Environment.ApplicationName} v1"));
        });
    }

    // app.UseHttpsRedirection(); //production only

    app.UseHttpsRedirection();

    app.UseAuthorization();

    //app.UseEndpoints(endpoints =>
    //{
    //    endpoints.MapHealthChecks("/hc", new HealthCheckOptions
    //    {
    //        Predicate = _ => true,
    //        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    //    });
    //    endpoints.MapDefaultControllerRoute();
    //});

    app.SeedCustomerData()
        .Run();
}
catch (Exception ex)
{
    var type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;

    Log.Fatal(ex, $"Unhandled exception: {ex.Message}");
}
finally
{
    Log.Information($"Shutdown {builder.Environment.ApplicationName} complete");
    Log.CloseAndFlush();
}
