using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CourseworkDB
{
    public partial class MainContext : DbContext
    {
        public MainContext()
        {
        }

        public MainContext(DbContextOptions<MainContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Advert> Adverts { get; set; } = null!;
        public virtual DbSet<Body> Bodies { get; set; } = null!;
        public virtual DbSet<Brand> Brands { get; set; } = null!;
        public virtual DbSet<Color> Colors { get; set; } = null!;
        public virtual DbSet<FuelType> FuelTypes { get; set; } = null!;
        public virtual DbSet<Model> Models { get; set; } = null!;
        public virtual DbSet<Transmission> Transmissions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Vehicle> Vehicles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Database=main;Username=manager;Password=password");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Advert>(entity =>
            {
                entity.ToTable("adverts");

                entity.HasIndex(e => e.VehicleId, "adverts_vehicle_id_key")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreationDate)
                    .HasColumnName("creation_date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Price)
                    .HasPrecision(10, 2)
                    .HasColumnName("price");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Adverts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("adverts_user_id_fkey");

                entity.HasOne(d => d.Vehicle)
                    .WithOne(p => p.Advert)
                    .HasForeignKey<Advert>(d => d.VehicleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("adverts_vehicle_id_fkey");
            });

            modelBuilder.Entity<Body>(entity =>
            {
                entity.ToTable("bodies");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("brands");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<Color>(entity =>
            {
                entity.ToTable("colors");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<FuelType>(entity =>
            {
                entity.ToTable("fuel_types");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<Model>(entity =>
            {
                entity.ToTable("models");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.BrandId).HasColumnName("brand_id");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Models)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("models_brand_id_fkey");
            });

            modelBuilder.Entity<Transmission>(entity =>
            {
                entity.ToTable("transmissions");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Phone).HasColumnName("phone");
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.ToTable("vehicles");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.BodyId).HasColumnName("body_id");

                entity.Property(e => e.ColorId).HasColumnName("color_id");

                entity.Property(e => e.Displacement).HasColumnName("displacement");

                entity.Property(e => e.FuelTypeId).HasColumnName("fuel_type_id");

                entity.Property(e => e.Mileage).HasColumnName("mileage");

                entity.Property(e => e.ModelId).HasColumnName("model_id");

                entity.Property(e => e.Power).HasColumnName("power");

                entity.Property(e => e.ProductionYear).HasColumnName("production_year");

                entity.Property(e => e.TransmissionId).HasColumnName("transmission_id");

                entity.HasOne(d => d.Body)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.BodyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("vehicles_body_id_fkey");

                entity.HasOne(d => d.Color)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.ColorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("vehicles_color_id_fkey");

                entity.HasOne(d => d.FuelType)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.FuelTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("vehicles_fuel_type_id_fkey");

                entity.HasOne(d => d.Model)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.ModelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("vehicles_model_id_fkey");

                entity.HasOne(d => d.Transmission)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.TransmissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("vehicles_transmission_id_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
