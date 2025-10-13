using ETicaret_API;
using ETicaret_Application.Interfaces;
using ETicaret_Application.Services;
using ETicaret_Application.UseCases;
using ETicaret_Infrastructure.Data.Entities;
using ETicaret_Infrastructure.Data.Repositories;
using ETicaret_Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


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
builder.Services.AddScoped<IDeliveryCompanyRepository, EfDeliveryCompanyRepository>();
builder.Services.AddScoped<IShopRepository, EfShopRepository>();
builder.Services.AddScoped<IProductImageRepository, EfProductImageRepository>();
builder.Services.AddScoped<IProductSubCategoryRepository, EfProductSubCategories>();

// HttpContext’e erişim için
builder.Services.AddHttpContextAccessor();

// CurrentUserService implementasyonu
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// UseCase
builder.Services.AddScoped<CreateOrderUseCase>();
builder.Services.AddScoped<GetOrderUseCase>();
builder.Services.AddScoped<ProductCategoryUseCase>();

builder.Services.AddHttpClient(); // OpenAI client için
builder.Services.AddScoped<ProductDescriptionUseCase>();
builder.Services.AddScoped<TemplateDescriptionGenerator>();

builder.Services.AddHttpClient<OpenAiClient>();
builder.Services.AddScoped<ILLMClient, OpenAiClient>();


//auth
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ETicaret API", Version = "v1" });

    // 🔑 JWT Authentication için ayar
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header kullanın. \r\n\r\n 'Bearer' yazıp boşluk bırakın ve ardından token'i girin.\r\n\r\nÖrnek: \"Bearer 12345abcdef\"",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
});

builder.Configuration.AddUserSecrets<Program>();


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
