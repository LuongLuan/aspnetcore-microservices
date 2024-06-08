using Serilog;

namespace Ordering.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            Log.Information($"Start {builder.Environment.ApplicationName} up");
            try
            {
                // Add services to the container.
                builder.Services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseAuthorization();


                app.MapControllers();

                app.Run();
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

        }
    }
}