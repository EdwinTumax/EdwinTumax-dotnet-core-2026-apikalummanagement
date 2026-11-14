using System.Text.Json.Serialization;
using ApiKalumManagement.DBContexts;
using ApiKalumManagement.DTOs;
using ApiKalumManagement.Services;
using ApiKalumManagement.Services.Interfaces;
using ApiKalumManagement.Shared.Helpers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowKalumApp", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddTransient<IRabbitExchangeService,RabbitExchangeService>();
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddDbContext<KalumDBContext>(options => options.UseSqlServer("name=KalumManagement"));
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddTransient<IUtils, Utils>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowKalumApp");

//app.UseHttpsRedirection();
app.MapControllers();
app.Run();
