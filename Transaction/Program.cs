using Hotel.RabbitMQ;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Transaction.Data;
using Transaction.Extensions;
using Transaction.Repository;

var builder = WebApplication.CreateBuilder(args);

//Specific DbContext for use from singleton AzServiceBusConsumer
var optionsBuilder = new DbContextOptionsBuilder<TransactionDbContext>();
optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("TransactionDb"));

// Add services to the container.
builder.Services.AddDbContext<TransactionDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("TransactionDb")));
builder.Services.AddScoped<ITransactionRepository, TransactionRespository>();

builder.Services.AddSingleton(new TransactionRespository(optionsBuilder.Options));

builder.Services.AddSingleton<IRabbitMQConsumer,RabbitMQConsumer>();
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

app.UseAzServiceBusConsumer();

app.Run();
