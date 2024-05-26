using Blasterify.Services.Data;
using Blasterify.Services.Services;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace Blasterify.Services
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection"));
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Hangfire
            builder.Services.AddHangfire((sp, config) =>
            {
                var connectionString = sp.GetRequiredService<IConfiguration>().GetConnectionString("DatabaseConnection");
                config.UseSqlServerStorage(connectionString);
            });

            builder.Services.AddHangfireServer();

            // Yuno
            YunoServices.SetKeys(
                builder.Configuration.GetConnectionString("YunoPublicAPIKey"), 
                builder.Configuration.GetConnectionString("YunoPrivateSecretKey")
            );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //FOR PUBLISH
            //Swagger
            app.UseSwagger();
            app.UseSwaggerUI();

            //Hangfire
            app.MapHangfireDashboard();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}