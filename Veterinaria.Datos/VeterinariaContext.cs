using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Veterinaria.Entidades.Models;

namespace Veterinaria.Datos;

public partial class VeterinariaContext : DbContext
{
    public VeterinariaContext(DbContextOptions<VeterinariaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Duenio> Duenios { get; set; }

    public virtual DbSet<Mascota> Mascotas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Duenio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Duenios__3214EC0779BA9388");

            entity.HasIndex(e => e.Email, "UQ__Duenios__A9D1053463E85630").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<Mascota>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Mascotas__3214EC0785873AB6");

            entity.HasIndex(e => e.Microchip, "UX_Mascotas_Microchip")
                .IsUnique()
                .HasFilter("([Microchip] IS NOT NULL)");

            entity.Property(e => e.Especie).HasMaxLength(50);
            entity.Property(e => e.Microchip).HasMaxLength(30);
            entity.Property(e => e.Nombre).HasMaxLength(80);
            entity.Property(e => e.Peso).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.Raza).HasMaxLength(80);

            entity.HasOne(d => d.Duenio).WithMany(p => p.Mascotas)
                .HasForeignKey(d => d.DuenioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Mascotas_Duenios");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
