using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Data;
using ProductsAPI.Repository;
using ProductsAPI.Services;
using WebAPI_Project.ErrorHandling;

namespace ProductsAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen();
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlite("Data Source=products.db"));

            builder.Services.AddControllersWithViews();


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseExceptionHandler();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
