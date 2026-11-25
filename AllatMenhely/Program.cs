using AllatMenhely.Models;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace AllatMenhely
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<AnimalshelterContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("Mysql")!)
);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }


            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
