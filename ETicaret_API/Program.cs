using ETicaret_Application.Interfaces;
using ETicaret_Application.Services;
using ETicaret_Application.UseCases;
using ETicaret_Infrastructure.Data.Entities;
using ETicaret_Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using ETicaret_API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ETicaretDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IOrderRepository, EfOrderRepository>();
builder.Services.AddScoped<IProductRepository, EfProductRepository>();
builder.Services.AddScoped<IProductCategoryRepository, EfProductCategoryRepository>();
// HttpContext’e eriþim için
builder.Services.AddHttpContextAccessor();

// CurrentUserService implementasyonu
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// UseCase
builder.Services.AddScoped<CreateOrderUseCase>();
builder.Services.AddScoped<GetOrderUseCase>();
builder.Services.AddScoped<ProductCategoryUseCase>();



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
