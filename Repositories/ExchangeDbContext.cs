using Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ExchangeDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ExchangeDbContext(DbContextOptions<ExchangeDbContext> options) : base(options)
        {
        }

        public DbSet<Currency> Currencies { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<ExchangeTransaction> ExchangeTransactions { get; set; }

        public DbSet<UserActivityReport> UserActivityReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserActivityReport>()
                .HasKey(u => u.Id); // Define primary key

            modelBuilder.Entity<User>()
                .HasOne(u => u.Wallet)
                .WithOne(w => w.User)
                .HasForeignKey<Wallet>(w => w.UserId);

            modelBuilder.Entity<ExchangeRate>()
                .Property(e => e.Rate)
                .HasColumnType("decimal(18, 6)");

            modelBuilder.Entity<Wallet>()
                .Property(w => w.Balance)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ExchangeRate>()
                .HasOne(e => e.FromCurrency)
                .WithMany()
                .HasForeignKey(e => e.FromCurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ExchangeRate>()
                .HasOne(e => e.ToCurrency)
                .WithMany()
                .HasForeignKey(e => e.ToCurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ExchangeTransaction>()
                .HasOne(et => et.FromCurrency)
                .WithMany()
                .HasForeignKey(et => et.FromCurrencyId)
                .OnDelete(DeleteBehavior.Restrict); // Change to Restrict

            modelBuilder.Entity<ExchangeTransaction>()
                .HasOne(et => et.ToCurrency)
                .WithMany()
                .HasForeignKey(et => et.ToCurrencyId)
                .OnDelete(DeleteBehavior.Restrict); // Change to Restrict

            base.OnModelCreating(modelBuilder);
        }

    }
}
