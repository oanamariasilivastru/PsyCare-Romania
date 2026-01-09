using backend.Repo;
using backend.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// =======================
// Configuration
// =======================
// Connection string din appsettings.json
// Ex: "ConnectionStrings": { "PSYCare": "Server=DESKTOP-URV53RQ;Database=PSYCare;Trusted_Connection=True;Encrypt=False;" }
var configuration = builder.Configuration;

// =======================
// Add Services / Dependency Injection
// =======================
builder.Services.AddDbContext<PSYCareDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("PSYCare")));

// Adaugam Repo si Service
builder.Services.AddScoped<IRepo, SSMSRepo>();
builder.Services.AddScoped<IService, PSYCareService>();

// Adaugam controller-e
builder.Services.AddControllers();

// Adaugam Swagger doar daca vrei sa testezi endpoint-urile
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =======================
// Build App
// =======================
var app = builder.Build();

// =======================
// Middleware
// =======================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

// =======================
// Map Controllers
// =======================
app.MapControllers();

// =======================
// Run
// =======================
app.Run();