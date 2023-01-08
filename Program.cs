using Microsoft.EntityFrameworkCore;
using QaDashboardApi.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<QashboardContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

//Add Config
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

app.UsePathBase("/api/");
app.UseSwagger();
app.UseSwaggerUI();

app.MapHealthChecks("/");

app.UseHttpsRedirection();
app.UseCors("corsapp");
app.UseAuthorization();
app.UseRouting();
app.MapControllers();

app.Run();
