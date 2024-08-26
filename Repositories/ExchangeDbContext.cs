﻿using Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<ExchangeTransaction>()
                .HasOne(et => et.ToCurrency)
                .WithMany()
                .HasForeignKey(et => et.ToCurrencyId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<UserActivityReport>()
                .HasKey(u => u.Id); 

            modelBuilder.Entity<UserActivityReport>()
                .Property(u => u.TotalAmountExchanged)
                .HasColumnType("decimal(18, 2)"); 

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ExchangeSystemDB;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False")
                .LogTo(Console.WriteLine, LogLevel.Information);
        }
    }
}
