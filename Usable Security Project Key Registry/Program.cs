using Microsoft.EntityFrameworkCore;
using Usable_Security_Project_Key_Registry.Data;
using Usable_Security_Project_Key_Registry.Repositories;
using Usable_Security_Project_Key_Registry.Services.User_Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});


builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();



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
