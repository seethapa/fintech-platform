using Azure.Identity;
using MongoDB.Driver;
using Shared.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Key Vault
builder.Configuration.AddAzureKeyVault(
    new Uri(builder.Configuration["KeyVaultUrl"]!),
    new DefaultAzureCredential());

// MongoDB
builder.Services.AddSingleton<IMongoClient>(
    new MongoClient(builder.Configuration["MongoConnectionString"]));

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();
app.Run();
