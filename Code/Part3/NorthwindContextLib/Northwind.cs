﻿using Microsoft.EntityFrameworkCore;

namespace Packt.CS7
{
	public class Northwind : DbContext
	{
		public DbSet<Category> Categories { get; set; }

		public DbSet<Customer> Customers { get; set; }

		public DbSet<Employee> Employees { get; set; }

		public DbSet<Order> Orders { get; set; }

		public DbSet<OrderDetail> OrderDetails { get; set; }

		public DbSet<Product> Products { get; set; }

		public DbSet<Shipper> Shippers { get; set; }

		public DbSet<Supplier> Suppliers { get; set; }

		public Northwind(DbContextOptions options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Category>()
				.Property(c => c.CategoryName)
				.IsRequired()
				.HasMaxLength(15);

			// Define a one-to-many relationship.
			modelBuilder.Entity<Category>()
				.HasMany(c => c.Products)
				.WithOne(p => p.Category);

			modelBuilder.Entity<Customer>()
				.Property(c => c.CustomerID)
				.IsRequired()
				.HasMaxLength(5);

			modelBuilder.Entity<Customer>()
				.Property(c => c.CompanyName)
				.IsRequired()
				.HasMaxLength(40);

			modelBuilder.Entity<Customer>()
				.Property(c => c.ContactName)
				.HasMaxLength(30);

			modelBuilder.Entity<Customer>()
				.Property(c => c.Country)
				.HasMaxLength(15);

			modelBuilder.Entity<Employee>()
				.Property(e => e.LastName)
				.IsRequired()
				.HasMaxLength(20);

			modelBuilder.Entity<Employee>()
				.Property(e => e.FirstName)
				.IsRequired()
				.HasMaxLength(10);

			modelBuilder.Entity<Employee>()
				.Property(e => e.Country)
				.HasMaxLength(15);

			modelBuilder.Entity<Product>()
				.Property(p => p.ProductName)
				.IsRequired()
				.HasMaxLength(40);

			modelBuilder.Entity<Product>()
				.HasOne(p => p.Category)
				.WithMany(c => c.Products);

			modelBuilder.Entity<Product>()
				.HasOne(p => p.Supplier)
				.WithMany(s => s.Products);

			modelBuilder.Entity<OrderDetail>()
				.ToTable("Order Details");

			// Define multi-column primary key
			// for Order Details table.
			modelBuilder.Entity<OrderDetail>()
				.HasKey(od => new { od.OrderID, od.ProductID });

			modelBuilder.Entity<Supplier>()
				.Property(s => s.CompanyName)
				.IsRequired()
				.HasMaxLength(40);

			modelBuilder.Entity<Supplier>()
				.HasMany(s => s.Products)
				.WithOne(p => p.Supplier);
		}
	}
}
