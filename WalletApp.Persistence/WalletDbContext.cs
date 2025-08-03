using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WalletApp.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WalletApp.Persistence
{
    public class WalletDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
    {
        public WalletDbContext(DbContextOptions<WalletDbContext> options) : base(options) { }

        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<AppWallet> Wallets { get; set; }
        public DbSet<AppBankAccount> BankAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<WalletTransfer> WalletTransfers { get; set; }
        public DbSet<AppPayment> Payments { get; set; }
        public DbSet<BankTransaction> BankTransactions { get; set; }
        public DbSet<ProviderBank> ProviderBanks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // AppUser config (sadece ek alanlar)
            modelBuilder.Entity<AppUser>(builder =>
            {
                builder.Property(u => u.Role).IsRequired().HasMaxLength(20);
            });

            // UserDetail configuration
            modelBuilder.Entity<UserDetail>(builder =>
            {
                builder.HasKey(ud => ud.Id);
                builder.Property(ud => ud.Name).IsRequired().HasMaxLength(50);
                builder.Property(ud => ud.Surname).IsRequired().HasMaxLength(50);
                builder.Property(ud => ud.BirthDay).IsRequired();
                builder.Property(ud => ud.Occupation).IsRequired().HasMaxLength(50);
                builder.Property(ud => ud.PhoneNumber).IsRequired();
                builder.Property(ud => ud.Address).IsRequired().HasMaxLength(200);
                builder.Property(ud => ud.Gender).IsRequired().HasMaxLength(10);

                builder.HasOne(ud => ud.User)
                       .WithOne(u => u.UserDetail)
                       .HasForeignKey<UserDetail>(ud => ud.AppUserId);
            });

            // Wallet configuration
            modelBuilder.Entity<AppWallet>(builder =>
            {
                builder.HasKey(w => w.Id);
                builder.Property(w => w.TotalBalance).HasPrecision(18, 2);
                builder.Property(w => w.CreatedDate).IsRequired();
                builder.Property(w => w.Currency);
                builder.Property(w => w.Assest).IsRequired().HasMaxLength(50);

                builder.HasOne(w => w.User)
                       .WithMany(u => u.Wallet)
                       .HasForeignKey(w => w.AppUserId)
                       .OnDelete(DeleteBehavior.Restrict);

                builder.HasMany(w => w.Transactions)
                       .WithOne(t => t.Wallet)
                       .HasForeignKey(t => t.WalletId)
                       .OnDelete(DeleteBehavior.Restrict);
            });

            // BankAccount configuration
            modelBuilder.Entity<AppBankAccount>(builder =>
            {
                builder.HasKey(ba => ba.Id);

                builder.HasOne(ba => ba.User)
                       .WithMany(u => u.BankaHesap)
                       .HasForeignKey(ba => ba.AppUserId)
                       .OnDelete(DeleteBehavior.Restrict);
            });

            // Transaction configuration
            modelBuilder.Entity<Transaction>(builder =>
            {
                builder.HasKey(t => t.Id);
                builder.Property(t => t.Amount).HasPrecision(18, 2);
                builder.Property(t => t.Currency).IsRequired();
                builder.Property(t => t.Type).IsRequired();
                builder.Property(t => t.Description).IsRequired().HasMaxLength(200);
                builder.Property(t => t.CreatedDate).IsRequired();

                builder.HasOne(t => t.Wallet)
                       .WithMany(w => w.Transactions)
                       .HasForeignKey(t => t.WalletId)
                       .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(t => t.AppBankAccount)
                       .WithMany(a => a.Transactions)
                       .HasForeignKey(t => t.AppBankAccountId)
                       .OnDelete(DeleteBehavior.Restrict);
            });

            // AppPayment configuration
            modelBuilder.Entity<AppPayment>(builder =>
            {
                builder.HasKey(p => p.Id);
                builder.Property(p => p.Institution).IsRequired();
                builder.Property(p => p.Amount).HasPrecision(18, 2);

                builder.HasOne(p => p.Transaction)
                       .WithOne(t => t.AppPayment)
                       .HasForeignKey<AppPayment>(p => p.TransactionId)
                       .OnDelete(DeleteBehavior.Restrict);
            });

            // BankTransaction configuration
            modelBuilder.Entity<BankTransaction>(builder =>
            {
                builder.HasKey(bt => bt.Id);
                builder.Property(bt => bt.Iban).IsRequired().HasMaxLength(50);
                builder.Property(bt => bt.TargetBankId).IsRequired();
                builder.Property(bt => bt.SourceBankId).IsRequired();
                builder.Property(bt => bt.Commission).IsRequired();

                builder.HasOne(bt => bt.Transaction)
                       .WithOne(t => t.BankTransaction)
                       .HasForeignKey<BankTransaction>(bt => bt.TransactionId)
                       .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(bt => bt.ProviderBank)
                       .WithMany(pb => pb.BankTransactions)
                       .HasForeignKey(bt => bt.TargetBankId)
                       .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(bt => bt.ProviderBank)
                       .WithMany(pb => pb.BankTransactions)
                       .HasForeignKey(bt => bt.SourceBankId)
                       .OnDelete(DeleteBehavior.Restrict);
            });

            // WalletTransfer configuration
            modelBuilder.Entity<WalletTransfer>(builder =>
            {
                builder.HasKey(wt => wt.Id);
                builder.Property(wt => wt.Target).IsRequired().HasMaxLength(100);
                builder.Property(wt => wt.IslemNo).IsRequired().HasMaxLength(50);
                builder.Property(wt => wt.SourceWalletId).IsRequired();

                builder.HasOne(wt => wt.Transaction)
                       .WithMany(t => t.WalletTransfers)
                       .HasForeignKey(wt => wt.TransactionId)
                       .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(wt => wt.Wallet)
                       .WithMany(w => w.WalletTransfers)
                       .HasForeignKey(wt => wt.WalletId)
                       .OnDelete(DeleteBehavior.Restrict);
            });

            // ProviderBank configuration
            modelBuilder.Entity<ProviderBank>(builder =>
            {
                builder.HasKey(pb => pb.Id);
                builder.Property(pb => pb.BankName).IsRequired().HasMaxLength(100);
            });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity &&
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;
                var now = DateTime.UtcNow;
                var user = "system"; // Bu kısmı istersen kullanıcıyla dinamikleştir

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate = now;
                    entity.CreatedUser = user;
                    entity.ModifiedDate = now;
                    entity.ModifiedUser = user;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.ModifiedDate = now;
                    entity.ModifiedUser = user;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
