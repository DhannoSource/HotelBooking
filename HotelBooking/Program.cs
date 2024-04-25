using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Repositories;
using Hotel.RabbitMQ;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddDbContext<HotelDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("HotelDb")));
builder.Services.AddScoped<IHotelRespository,HotelRepository>();
//var rabbitMQConnectionString = configuration.GetConnectionString("RabbitMQConnectionString");
//return ConnectionFactory.CreateConnection(options.UseSqlServer(builder.Configuration.GetConnectionString("RabbitMQConnectionString")));
//);
builder.Services.AddScoped<IRabbitMQProducer, RabbitMQProducer>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
