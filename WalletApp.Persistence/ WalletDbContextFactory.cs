using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WalletApp.Persistence
{
    public class WalletDbContextFactory : IDesignTimeDbContextFactory<WalletDbContext>
    {
        public WalletDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WalletDbContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-GEU3I6M\\SQLEXPRESS;Database=WalletApplicationDB;Trusted_Connection=True;TrustServerCertificate=True;");

            return new WalletDbContext(optionsBuilder.Options);
        }
    }
}
