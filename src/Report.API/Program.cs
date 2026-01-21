using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Middleware;
using System;

var builder = WebApplication.CreateBuilder(args);

// Key Vault
builder.Configuration.AddAzureKeyVault(
    new Uri(builder.Configuration["KeyVaultUrl"]!),
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
    options.AddPolicy("ReportAccess",
        policy => policy.RequireRole("SUPER_ADMIN", "AUDITOR"));
});

// SQL (read-heavy)
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration["sqlconnectiondev"]));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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