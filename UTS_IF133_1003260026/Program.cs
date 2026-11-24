using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UTS_IF133_1003260026.Data;

var builder = WebApplication.CreateBuilder(args);

// EF Core + SQLite (connection string "DefaultConnection" di appsettings.json)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

// Swagger (Swashbuckle) untuk dokumentasi & testing API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Inventory Product API",
        Version = "v1",
        Description = "UTS IF133 Pemrograman .NET - Sayyid Maulana Khakul Yakin (1003260026)"
    });
});

var app = builder.Build();

// Menerapkan migration yang belum ter-apply saat aplikasi start
// (sama seperti menjalankan `dotnet ef database update`).
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

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
