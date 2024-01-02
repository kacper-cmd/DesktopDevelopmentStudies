using System;
using System.Collections.Generic;
using System.Diagnostics;
using DesktopDevelopment.Models;
using Microsoft.EntityFrameworkCore;

namespace DesktopDevelopment.Models.Contexts;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerCategory> CustomerCategories { get; set; }

    public virtual DbSet<CustomerStatus> CustomerStatuses { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {//w appsettingach mozna dac connection stringa
        optionsBuilder.UseSqlServer("Server=KACPER;Integrated Security=True;Database=DesktopDevelopment;TrustServerCertificate=True");
        optionsBuilder.LogTo(item => Debug.WriteLine(item)); 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Addresse__3214EC07D0474671");

            entity.HasOne(d => d.Country).WithMany(p => p.Addresses).HasConstraintName("FK__Addresses__Count__267ABA7A");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Countrie__3214EC07E5FF3170");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC07421E6C9F");

            entity.HasOne(d => d.Address).WithMany(p => p.Customers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Customers__Addre__2F10007B");

            entity.HasOne(d => d.CustomerCategory).WithMany(p => p.Customers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Customers__Custo__2E1BDC42");

            entity.HasOne(d => d.CustomerStatus).WithMany(p => p.Customers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Customers__Custo__2D27B809");
        });

        modelBuilder.Entity<CustomerCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC073250EBE6");
        });

        modelBuilder.Entity<CustomerStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC0761C6CDB7");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Invoices__3214EC0798068A2D");

            entity.HasOne(d => d.Customer).WithMany(p => p.Invoices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Invoices__Custom__33D4B598");

            entity.HasOne(d => d.PaymentMethodNavigation).WithMany(p => p.Invoices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Invoices__Paymen__34C8D9D1");
        });

        modelBuilder.Entity<InvoiceItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__InvoiceI__3214EC07E9FAA2ED");

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__InvoiceIt__Invoi__398D8EEE");

            entity.HasOne(d => d.Product).WithMany(p => p.InvoiceItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__InvoiceIt__Produ__3A81B327");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PaymentM__3214EC074B5F8C73");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC077C14E403");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
