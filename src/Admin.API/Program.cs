using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Middleware;
using Shared.Auth;


var builder = WebApplication.CreateBuilder(args);

// Key Vault
builder.Configuration.AddAzureKeyVault(
    new Uri(builder.Configuration["KeyVaultUrl"]),
    new DefaultAzureCredential());

// Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["JwtIssuer"];
    options.Audience = builder.Configuration["JwtAudience"];
});
// Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly",
        policy => policy.RequireRole("SUPER_ADMIN"));
});

// DB
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration["SqlConnectionString"]));

builder.Services.AddControllers();

builder.Services.AddHttpClient<Shared.Auth.IAuditClient, Shared.Auth.AuditClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["AuditApiUrl"]!);
});

// Add services to the container.
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

app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();


