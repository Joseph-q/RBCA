using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaCorteDeCaja;
using SistemaCorteDeCaja.Models;
using SistemaCorteDeCaja.Roles.Services;
using SistemaCorteDeCaja.Users.Services;

var builder = WebApplication.CreateBuilder(args);

//Database
builder.Services.AddDbContext<CorteDeCajaContext>(op => op
.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
.EnableSensitiveDataLogging(false) // Deshabilita información sensible
);

//Services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoleService>();

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
