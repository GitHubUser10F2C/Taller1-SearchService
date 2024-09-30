using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("mongodb");

// Registra el cliente de MongoDB como un servicio
builder.Services.AddSingleton<MongoClient>(sp => new MongoClient(connectionString));
builder.Services.AddScoped<MongoDbContext>(sp =>
{
    var client = sp.GetRequiredService<MongoClient>();
    return MongoDbContext.Create(client.GetDatabase("db1"));
});

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
