using System;
using System.Collections.Generic;
using FrietGeenPatatZaakMVC.Models;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary1.Models // Update de namespace om ClassLibrary1 te gebruiken
{
    public class FrietGeenPatatZaakContext : DbContext
    {
        public FrietGeenPatatZaakContext(DbContextOptions<FrietGeenPatatZaakContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A2B4FB130D6");
                entity.ToTable("Category");
                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B87F7CF51D");
                entity.ToTable("Customer");
                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.PasswordHash).HasMaxLength(100);
                entity.Property(e => e.Status).HasDefaultValue(true);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId).HasName("PK__Order__C3905BAF74C5EA43");
                entity.ToTable("Order");
                entity.Property(e => e.OrderId).HasColumnName("OrderID");
                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
                entity.Property(e => e.OrderDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Customer_Order");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__D3B9D30C070502AD");
                entity.ToTable("OrderDetail");
                entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
                entity.Property(e => e.OrderId).HasColumnName("OrderID");
                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_Order_OrderDetail");

                entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Product_OrderDetail");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId).HasName("PK__Product__B40CC6EDED794E21");
                entity.ToTable("Product");
                entity.Property(e => e.ProductId).HasColumnName("ProductID");
                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Price).HasColumnType("decimal(5, 2)");
                entity.Property(e => e.Status).HasDefaultValue(true);

                entity.HasOne(d => d.Category).WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Category_Product");
            });
        }
    }
}
