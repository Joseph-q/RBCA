using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SistemaCorteDeCaja;
using SistemaCorteDeCaja.Auth.Services;
using SistemaCorteDeCaja.Models;
using SistemaCorteDeCaja.Roles.Services;
using SistemaCorteDeCaja.Shared.Settings;
using SistemaCorteDeCaja.Users.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Database
builder.Services.AddDbContext<CorteDeCajaContext>(op => op
.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
.EnableSensitiveDataLogging(false) // Deshabilita información sensible
);



// Autehntication
IConfiguration jwtSettings = builder.Configuration.GetSection("JWT");

if (jwtSettings == null)
{
    throw new Exception("JWT settings not found");
}

var secret = jwtSettings.GetValue<string>("SecretKey");

if (string.IsNullOrEmpty(secret))
{
    throw new Exception("JWT settings not found");
}


builder.Services.Configure<JwtSettings>(jwtSettings);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
        };
    });


//Services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<AuthService>();

// Add services to the container.
builder.Services.AddControllers(options =>
{
    //Handling Errors
    options.Filters.Add<ExceptionHandlingFilter>();
});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Mapper
var mapperConfig = new MapperConfiguration(m =>
{
    m.AddProfile(new MapperProfile());
});

IMapper mapper = mapperConfig.CreateMapper();

builder.Services.AddSingleton(mapper);


//Build App
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
