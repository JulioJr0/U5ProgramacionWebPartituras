using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace U5ProgWebPartituras.Models.Entities;

public partial class PartiturasContext : DbContext
{
    public PartiturasContext()
    {
    }

    public PartiturasContext(DbContextOptions<PartiturasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Compositor> Compositor { get; set; }

    public virtual DbSet<Genero> Genero { get; set; }

    public virtual DbSet<Partitura> Partitura { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=partituras;user=root;password=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.28-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Compositor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("compositor");

            entity.Property(e => e.Biografia).HasColumnType("text");
            entity.Property(e => e.Nacionalidad).HasMaxLength(45);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Genero>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("genero");

            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(45);
        });

        modelBuilder.Entity<Partitura>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("partitura");

            entity.HasIndex(e => e.IdCompositor, "fk_Partitura_Compositor");

            entity.HasIndex(e => e.IdGenero, "fk_Partitura_Genero");

            entity.Property(e => e.Descripcion).HasColumnType("text");
            entity.Property(e => e.Dificultad)
                .HasMaxLength(45)
                .HasComment("Ej: Fácil, Intermedio, Avanzado");
            entity.Property(e => e.Instrumentacion).HasMaxLength(100);
            entity.Property(e => e.Titulo).HasMaxLength(255);

            entity.HasOne(d => d.IdCompositorNavigation).WithMany(p => p.Partitura)
                .HasForeignKey(d => d.IdCompositor)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_Partitura_Compositor");

            entity.HasOne(d => d.IdGeneroNavigation).WithMany(p => p.Partitura)
                .HasForeignKey(d => d.IdGenero)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_Partitura_Genero");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
