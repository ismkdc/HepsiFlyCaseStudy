using HepsiFlyCaseStudy.Extensions;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register MongoDB
builder.Services.AddMongoDB();

// Register MediatR
builder.Services.AddMediatR(typeof(Program));

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Register Redis
builder.Services.AddRedis();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// For unit testing
public partial class Program { }