﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Cwiczenia_5.Models
{
    public partial class s16467Context : DbContext
    {
        public s16467Context()
        {
        }

        public s16467Context(DbContextOptions<s16467Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<ClientTrip> ClientTrips { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<CountryTrip> CountryTrips { get; set; }
        public virtual DbSet<Trip> Trips { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s16467;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Polish_CI_AS");

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.IdClient)
                    .HasName("Client_pk");

                entity.ToTable("Client");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(120);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(120);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(120);

                entity.Property(e => e.Pesel)
                    .IsRequired()
                    .HasMaxLength(120);

                entity.Property(e => e.Telephone)
                    .IsRequired()
                    .HasMaxLength(120);
            });

            modelBuilder.Entity<ClientTrip>(entity =>
            {
                entity.HasKey(e => new { e.IdClient, e.IdTrip })
                    .HasName("Client_Trip_pk");

                entity.ToTable("Client_Trip");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.Property(e => e.RegisteredAt).HasColumnType("datetime");

                entity.HasOne(d => d.IdClientNavigation)
                    .WithMany(p => p.ClientTrips)
                    .HasForeignKey(d => d.IdClient)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Table_5_Client");

                entity.HasOne(d => d.IdTripNavigation)
                    .WithMany(p => p.ClientTrips)
                    .HasForeignKey(d => d.IdTrip)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Table_5_Trip");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.IdCountry)
                    .HasName("Country_pk");

                entity.ToTable("Country");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(120);
            });

            modelBuilder.Entity<CountryTrip>(entity =>
            {
                entity.HasKey(e => new { e.IdCountry, e.IdTrip })
                    .HasName("Country_Trip_pk");

                entity.ToTable("Country_Trip");

                entity.HasOne(d => d.IdCountryNavigation)
                    .WithMany(p => p.CountryTrips)
                    .HasForeignKey(d => d.IdCountry)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Country_Trip_Country");

                entity.HasOne(d => d.IdTripNavigation)
                    .WithMany(p => p.CountryTrips)
                    .HasForeignKey(d => d.IdTrip)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Country_Trip_Trip");
            });

            modelBuilder.Entity<Trip>(entity =>
            {
                entity.HasKey(e => e.IdTrip)
                    .HasName("Trip_pk");

                entity.ToTable("Trip");

                entity.Property(e => e.DateFrom).HasColumnType("datetime");

                entity.Property(e => e.DateTo).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(220);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(120);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
