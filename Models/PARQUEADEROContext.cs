using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ParqueaderoApi.Models
{
    public partial class PARQUEADEROContext : DbContext
    {
        public PARQUEADEROContext()
        {
        }

        public PARQUEADEROContext(DbContextOptions<PARQUEADEROContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Estacionamiento> Estacionamientos { get; set; } = null!;
        public virtual DbSet<TipoVehiculo> TipoVehiculos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=DESKTOP-L1B63F0\\SQLEXPRESS; Database=PARQUEADERO; Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Estacionamiento>(entity =>
            {
                entity.ToTable("ESTACIONAMIENTO");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AplicaDescuento)
                    .HasColumnName("APLICA_DESCUENTO")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IdTipoVehiculo).HasColumnName("ID_TIPO_VEHICULO");

                entity.Property(e => e.Ingreso)
                    .HasColumnType("datetime")
                    .HasColumnName("INGRESO");

                entity.Property(e => e.Placa)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("PLACA");

                entity.Property(e => e.Salida)
                    .HasColumnType("datetime")
                    .HasColumnName("SALIDA");

                entity.HasOne(d => d.IdTipoVehiculoNavigation)
                    .WithMany(p => p.Estacionamientos)
                    .HasForeignKey(d => d.IdTipoVehiculo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ESTACIONA__ID_TI__395884C4");
            });

            modelBuilder.Entity<TipoVehiculo>(entity =>
            {
                entity.HasKey(e => e.IdTipoVehiculo)
                    .HasName("PK__TIPO_VEH__4E5451B0576B8A32");

                entity.ToTable("TIPO_VEHICULO");

                entity.Property(e => e.IdTipoVehiculo).HasColumnName("ID_TIPO_VEHICULO");

                entity.Property(e => e.Tarifa).HasColumnName("TARIFA");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TIPO");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
