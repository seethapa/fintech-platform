using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Shared.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // 🔴 LOCAL DEV CONNECTION STRING ONLY
        // This is used ONLY by dotnet ef
        var connectionString =
            "Server=tcp:sql-fintech-dev.database.windows.net,1433;" +
            "Database=fintechdb;" +
            "User ID=sqladmin;" +
            "Password=YOUR_PASSWORD;" +
            "Encrypt=True;" +
            "TrustServerCertificate=False;";

        optionsBuilder.UseSqlServer(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }
}
