using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SistemaCorteDeCaja;
using SistemaCorteDeCaja.Auth.Services;
using SistemaCorteDeCaja.Authorization.Services;
using SistemaCorteDeCaja.Models;
using SistemaCorteDeCaja.Permissions;
using SistemaCorteDeCaja.Roles.Services;
using SistemaCorteDeCaja.Shared.Settings;
using SistemaCorteDeCaja.Users.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Database
builder.Services.AddDbContext<CorteDeCajaContext>(op => op
    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    .EnableSensitiveDataLogging(builder.Environment.IsDevelopment()));


// Autehntication
var jwtSettings = builder.Configuration.GetSection("JWT");
builder.Services.Configure<JwtSettings>(jwtSettings);

var secret = jwtSettings.GetValue<string>("SecretKey")
             ?? throw new Exception("JWT SecretKey not found");

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

builder.Services.AddAuthorization();

//Services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthorizationService>();
builder.Services.AddScoped<PermissionService>();


// Add services to the container.
builder.Services.AddControllers(options =>
{
    //Handling Errors
    options.Filters.Add<ExceptionHandlingFilter>();
});
builder.Services.AddEndpointsApiExplorer();



//Swagger
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa solo en token"
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
            Array.Empty<string>()
        }
    });
});

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

//Authentication
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
