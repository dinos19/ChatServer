using ChatServer.Models;
using ChatServer.Models.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ChatServer.Infrastructure
{
    public class ApiDbContext : DbContext
    {
        public DbSet<Account> Users { get; set; }
        public DbSet<ChatMessage> Messages { get; set; }
        public DbSet<UserConnection> UserConnections { get; set; }

        public ApiDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Name column in the Account table to be unique
            modelBuilder.Entity<Account>()
                .HasIndex(a => a.Email)
                .IsUnique();
            // Shadow properties
            modelBuilder.Entity<ChatMessage>()
                .Property<int>("FromAccountId");

            modelBuilder.Entity<ChatMessage>()
                .Property<int>("ToAccountId");

            // Configure relationships using shadow properties
            modelBuilder.Entity<ChatMessage>()
                .HasOne<Account>("FromAccount")
                .WithMany()
                .HasForeignKey("FromAccountId")
                .OnDelete(DeleteBehavior.Restrict); // Adjust delete behavior as needed

            modelBuilder.Entity<ChatMessage>()
                .HasOne<Account>("ToAccount")
                .WithMany()
                .HasForeignKey("ToAccountId")
                .OnDelete(DeleteBehavior.Restrict); // Adjust delete behavior as needed
        }
    }
}