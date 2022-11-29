using Microsoft.EntityFrameworkCore;
using OnlineBanking;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


var connectionString = builder.Configuration.GetConnectionString("DefautConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseNpgsql(connectionString));


var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
