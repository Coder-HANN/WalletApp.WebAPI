using Microsoft.EntityFrameworkCore;
using WalletApp.Domain.Base;

namespace WalletApp.Persistence
{
    public class WalletDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<BankAccount> BankaHesaps { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<WalletTransfer> WalletTransfers { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<BankTransaction> BankTransactions { get; set; }
        

        public WalletDbContext(DbContextOptions<WalletDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<UserDetail>(builder =>
            {
                
                builder.HasKey(ud => ud.Id);
                builder.Property(ud => ud.Name).IsRequired().HasMaxLength(50);
                builder.Property(ud => ud.BirthDay).IsRequired();
                builder.Property(ud => ud.Occupation).IsRequired().HasMaxLength(50);
                builder.Property(ud => ud.PhoneNumber).IsRequired();


                builder
                    .HasOne(ud => ud.User)
                    .WithOne(u => u.UserDetail)
                    .HasForeignKey<UserDetail>(ud => ud.UserId);
            });

            
            modelBuilder.Entity<User>(builder =>
            {
                builder.HasKey(u => u.Id);

                builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
                builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(100);
                builder.Property(u => u.CreatedDate).IsRequired();
                
            });

          
            modelBuilder.Entity<Wallet>(builder =>
            { builder.HasKey(w => w.Id);
                builder.Property(w => w.TotalBalance).HasPrecision(18, 2);
                builder.Property(w => w.CreatedDate).IsRequired();
                builder.Property(w => w.Currency);
                builder.Property(w => w.Assest).IsRequired().HasMaxLength(50);


                builder
                .HasOne(w => w.User)
                .WithMany(u => u.Wallet)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Restrict);

                builder
                .HasMany(w => w.Transactions)
                .WithOne(t => t.Wallet)
                .HasForeignKey(t => t.WalletId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            
            modelBuilder.Entity<BankAccount>(builder =>
            {
                builder.HasKey(ba => ba.Id);
                builder.Property(ba => ba.Bilgiler).IsRequired().HasMaxLength(200);
                builder
                    .HasOne(ba => ba.User)
                    .WithMany(u => u.BankaHesap)
                    .HasForeignKey(bh => bh.UserId)
                    .OnDelete(DeleteBehavior.Restrict);


            });


            modelBuilder.Entity<Transaction>(builder =>
            {
                builder.HasKey(t => t.Id);
                builder.Property(t => t.Amount).HasPrecision(18, 2);
                builder.Property(t => t.Currency).IsRequired();
                builder.Property(t => t.Type).IsRequired();
                builder.Property(t => t.Description).IsRequired().HasMaxLength(200);
                builder.Property(t => t.CreatedDate).IsRequired();

                builder
                    .HasOne(t => t.Wallet)
                    .WithMany(w => w.Transactions)
                    .HasForeignKey(t => t.WalletId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            
            modelBuilder.Entity<Payment>(builder =>
            {
                builder.HasKey(p => p.Id);
                builder.Property(p => p.Institution).IsRequired();
              
                builder
                    .HasOne(p => p.Transaction)
                    .WithOne(t => t.Payment)  
                    .HasForeignKey<Payment>(p => p.TransactionId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<BankTransaction>(builder =>
            {
            builder.HasKey(bt => bt.Id);
            builder.Property(bt => bt.Iban).IsRequired().HasMaxLength(50);
            builder.Property(bt => bt.TargetBankId).IsRequired();
            builder.Property(bt => bt.SourceBankId).IsRequired();
            builder.Property(bt => bt.Commission).IsRequired();

            builder
            .HasOne(bt => bt.Transaction)
                .WithOne(t => t.BankTransaction)
                .HasForeignKey<BankTransaction>(p => p.TransactionId)  // Foreign key to Transaction. => Direkt string yerine kullanılmaz o yüzden bu yapı var 
                .OnDelete(DeleteBehavior.Restrict);


                builder
                .HasOne(t => t.ProviderBank)
                    .WithMany(pbI => pbI.BankTransactions)
                    .HasForeignKey(p => p.TargetBankId)
                .OnDelete(DeleteBehavior.Restrict);
                builder
                .HasOne(t => t.ProviderBank)
                    .WithMany(pbI => pbI.BankTransactions)
                    .HasForeignKey(p => p.SourceBankId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            
            
            modelBuilder.Entity<WalletTransfer>(builder =>
            {
                builder.HasKey(wt => wt.Id);
                builder.Property(wt => wt.Target).IsRequired().HasMaxLength(100);
                builder.Property(wt => wt.IslemNo).IsRequired().HasMaxLength(50);
                builder.Property(wt => wt.SourceWalletId).IsRequired();   
                builder 
                    .HasOne(wt => wt.Transaction)
                    .WithMany(t => t.WalletTransfers)
                    .HasForeignKey(wt => wt.TransactionId)
                    .OnDelete(DeleteBehavior.Restrict);
                builder
                    .HasOne(wt => wt.Wallet)
                    .WithMany(w => w.WalletTransfers)
                    .HasForeignKey(wt => wt.WalletId)
                    .OnDelete(DeleteBehavior.Restrict);

            });
            
            modelBuilder.Entity<ProviderBank>(builder =>
            {
                builder.HasKey(pb => pb.Id);
                builder.Property(pb => pb.BankName).IsRequired().HasMaxLength(100);

                
            });


            base.OnModelCreating(modelBuilder);
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
                var user = "system"; // Giriş yapan kullanıcı adı ile değiştirilebilir

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

