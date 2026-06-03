using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<OrderService>();
builder.Services.AddSingleton<KafkaProducer>();
builder.Services.AddScoped<StockClient>();
builder.Services.AddScoped<OrderEventProducer>();
builder.Services.AddHostedService<OrderEventsConsumer>();
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();