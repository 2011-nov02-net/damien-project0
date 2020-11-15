﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace StoreManager.DataModel
{
    public partial class StoreManagerContext : DbContext
    {
        public StoreManagerContext()
        {
        }

        public StoreManagerContext(DbContextOptions<StoreManagerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerOrder> CustomerOrders { get; set; }
        public virtual DbSet<OperatingLocation> OperatingLocations { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderProduct> OrderProducts { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<StoreInventory> StoreInventories { get; set; }
        public virtual DbSet<StoreOperatingLocation> StoreOperatingLocations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address", "Stores");

                entity.Property(e => e.AddressLine1)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.AddressLine2)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.State).HasMaxLength(128);

                entity.Property(e => e.ZipCode)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer", "Stores");

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(128);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.PhoneNumber).HasMaxLength(128);

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customer_CustomerAddressId");

                entity.HasOne(d => d.OperatingLocation)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.OperatingLocationId)
                    .HasConstraintName("FK_Customer_OperatingLocationId");
            });

            modelBuilder.Entity<CustomerOrder>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CustomerOrder", "Stores");

                entity.HasOne(d => d.Customer)
                    .WithMany()
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerOrder_CustomerId");

                entity.HasOne(d => d.Order)
                    .WithMany()
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerOrder_OrderId");
            });

            modelBuilder.Entity<OperatingLocation>(entity =>
            {
                entity.ToTable("OperatingLocation", "Stores");

                entity.HasIndex(e => e.AddressId, "UQ__Operatin__091C2AFAA71DBDC1")
                    .IsUnique();

                entity.HasOne(d => d.Address)
                    .WithOne(p => p.OperatingLocation)
                    .HasForeignKey<OperatingLocation>(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OperatingLocation_AddressId");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order", "Stores");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_CustomerId");

                entity.HasOne(d => d.OperatingLocation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.OperatingLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_OperatingLocationId");
            });

            modelBuilder.Entity<OrderProduct>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("OrderProduct", "Stores");

                entity.HasOne(d => d.Order)
                    .WithMany()
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderProduct_OrderId");

                entity.HasOne(d => d.Product)
                    .WithMany()
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderProduct_ProductId");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product", "Stores");

                entity.Property(e => e.Discount).HasColumnType("money");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Price).HasColumnType("money");
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.ToTable("Store", "Stores");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<StoreInventory>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("StoreInventory", "Stores");

                entity.HasOne(d => d.Product)
                    .WithMany()
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreInventory_ProductId");

                entity.HasOne(d => d.Store)
                    .WithMany()
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreInventory_StoreId");
            });

            modelBuilder.Entity<StoreOperatingLocation>(entity =>
            {
                entity.HasKey(e => e.StoreId)
                    .HasName("PK__StoreOpe__3B82F101545DF058");

                entity.ToTable("StoreOperatingLocation", "Stores");

                entity.Property(e => e.StoreId).ValueGeneratedOnAdd();

                entity.HasOne(d => d.OperatingLocation)
                    .WithMany(p => p.StoreOperatingLocations)
                    .HasForeignKey(d => d.OperatingLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreOperatingLocation_OperatingLocationId");

                entity.HasOne(d => d.Store)
                    .WithOne(p => p.StoreOperatingLocation)
                    .HasForeignKey<StoreOperatingLocation>(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreOperatingLocation_StoreId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}