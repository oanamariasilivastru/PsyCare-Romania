using backend.Repo;
using backend.Service;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDbContext<PSYCareDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("PSYCare")));

builder.Services.AddScoped<IRepo, SSMSRepo>();
builder.Services.AddScoped<IService, PSYCareService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();