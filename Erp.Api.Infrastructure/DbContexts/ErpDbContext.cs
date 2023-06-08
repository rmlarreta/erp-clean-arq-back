using Erp.Api.Domain.Entities;
using Erp.Api.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;

namespace Erp.Api.Infraestructure.DbContexts;

public partial class ErpDbContext : DbContext
{
    private readonly AppSettings _appSettings;
   
    public ErpDbContext(DbContextOptions<ErpDbContext> options, IOptions<AppSettings> appSettings)
        : base(options)
    {
        _appSettings = appSettings.Value;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings => warnings
         .Throw(RelationalEventId.MultipleCollectionIncludeWarning));
        optionsBuilder.UseSqlServer(_appSettings.DefaultConnection, options =>
        {
            options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        });
        base.OnConfiguring(optionsBuilder);
    }

    public virtual DbSet<BusEstado>? BusEstados { get; set; }

    public virtual DbSet<BusOperacion>? BusOperacions { get; set; }

    public virtual DbSet<BusOperacionDetalle>? BusOperacionDetalles { get; set; }

    public virtual DbSet<BusOperacionObservacion>? BusOperacionObservacions { get; set; }

    public virtual DbSet<BusOperacionPago>? BusOperacionPagos { get; set; }

    public virtual DbSet<BusOperacionTipo>? BusOperacionTipos { get; set; }

    public virtual DbSet<CobCuentaMovimiento>? CobCuentaMovimientos { get; set; }

    public virtual DbSet<CobCuentum>? CobCuenta { get; set; }

    public virtual DbSet<CobPo>? CobPos { get; set; }

    public virtual DbSet<CobRecibo>? CobRecibos { get; set; }

    public virtual DbSet<CobReciboDetalle>? CobReciboDetalles { get; set; }

    public virtual DbSet<CobTipoPago>? CobTipoPagos { get; set; }

    public virtual DbSet<OpCliente>? OpClientes { get; set; }

    public virtual DbSet<OpDocumentoProveedor>? OpDocumentoProveedors { get; set; }

    public virtual DbSet<OpGender>? OpGenders { get; set; }

    public virtual DbSet<OpPago>? OpPagos { get; set; }

    public virtual DbSet<OpPai>? OpPais { get; set; }

    public virtual DbSet<OpResp>? OpResps { get; set; }

    public virtual DbSet<SecRole>? SecRoles { get; set; }

    public virtual DbSet<SecUser>? SecUsers { get; set; }

    public virtual DbSet<StockIva>? StockIvas { get; set; }

    public virtual DbSet<StockProduct>? StockProducts { get; set; }

    public virtual DbSet<StockRubro>? StockRubros { get; set; }

    public virtual DbSet<SystemEmpresa>? SystemEmpresas { get; set; }

    public virtual DbSet<SystemIndex>? SystemIndices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BusEstado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Bus_Estado_Id");

            entity.ToTable("Bus_Estado");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<BusOperacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Bus_Operacion_Id");

            entity.ToTable("Bus_Operacion");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CodAut)
                .HasMaxLength(254)
                .IsUnicode(false);
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.Operador)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Razon)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Vence).HasColumnType("datetime");

            entity.HasOne(d => d.Cliente).WithMany(p => p.BusOperacions)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bus_Operacion_ClienteId");

            entity.HasOne(d => d.Estado).WithMany(p => p.BusOperacions)
                .HasForeignKey(d => d.EstadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bus_Operacion_EstadoId");

            entity.HasOne(d => d.TipoDoc).WithMany(p => p.BusOperacions)
                .HasForeignKey(d => d.TipoDocId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bus_Operacion_TipoDocId");
        });

        modelBuilder.Entity<BusOperacionDetalle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Bus_Operacion_Detalle_Id");

            entity.ToTable("Bus_Operacion_Detalle");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Cantidad).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Codigo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Detalle)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Facturado).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Internos).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IvaValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Rubro)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Unitario).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Operacion).WithMany(p => p.BusOperacionDetalles)
                .HasForeignKey(d => d.OperacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bus_Operacion_Detalle_OperacionId");

            entity.HasOne(d => d.Producto).WithMany(p => p.BusOperacionDetalles)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bus_Operacion_Detalle_ProductoId");
        });

        modelBuilder.Entity<BusOperacionObservacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Bus_Operacion_Observacion_Id");

            entity.ToTable("Bus_Operacion_Observacion");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.Observacion)
                .HasMaxLength(254)
                .IsUnicode(false);
            entity.Property(e => e.Operador)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Operacion).WithMany(p => p.BusOperacionObservacions)
                .HasForeignKey(d => d.OperacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bus_Operacion_Observacion_OperacionId");
        });

        modelBuilder.Entity<BusOperacionPago>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Bus_Operacion_Pago_Id");

            entity.ToTable("Bus_Operacion_Pago");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Operacion).WithMany(p => p.BusOperacionPagos)
                .HasForeignKey(d => d.OperacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bus_Operacion_Pago_OperacionId");

            entity.HasOne(d => d.Recibo).WithMany(p => p.BusOperacionPagos)
                .HasForeignKey(d => d.ReciboId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bus_Operacion_Pago_ReciboId");
        });

        modelBuilder.Entity<BusOperacionTipo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Bus_Operacion_Tipo_Id");

            entity.ToTable("Bus_Operacion_Tipo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CodeExt)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CobCuentaMovimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Cob_Cuenta_Movimiento_id");

            entity.ToTable("Cob_Cuenta_Movimiento");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Detalle)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.Monto).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Operador)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.CuentaNavigation).WithMany(p => p.CobCuentaMovimientos)
                .HasForeignKey(d => d.Cuenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cob_Cuenta_Movimiento_Cuenta");
        });

        modelBuilder.Entity<CobCuentum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Cob_Cuenta_Id");

            entity.ToTable("Cob_Cuenta");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Saldo).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<CobPo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Cob_Pos_Id");

            entity.ToTable("Cob_Pos");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DeviceId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Token).IsUnicode(false);
        });

        modelBuilder.Entity<CobRecibo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Cob_Recibo_Id");

            entity.ToTable("Cob_Recibo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.Operador)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Cliente).WithMany(p => p.CobRecibos)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cob_Recibo_ClienteId");
        });

        modelBuilder.Entity<CobReciboDetalle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Cob_Recibo_Detalles_Id");

            entity.ToTable("Cob_Recibo_Detalles");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Cancelado)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.CodAut)
                .HasMaxLength(254)
                .IsUnicode(false);
            entity.Property(e => e.Monto).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Observacion)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Pos).WithMany(p => p.CobReciboDetalles)
                .HasForeignKey(d => d.PosId)
                .HasConstraintName("FK_Cob_Recibo_Detalles_PosId");

            entity.HasOne(d => d.Recibo).WithMany(p => p.CobReciboDetalles)
                .HasForeignKey(d => d.ReciboId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cob_Recibo_Detalles_ReciboId");

            entity.HasOne(d => d.TipoNavigation).WithMany(p => p.CobReciboDetalles)
                .HasForeignKey(d => d.Tipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cob_Recibo_Detalles_Tipo");
        });

        modelBuilder.Entity<CobTipoPago>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Cob_Tipo_Pago_Id");

            entity.ToTable("Cob_Tipo_Pago");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Cuenta).WithMany(p => p.CobTipoPagos)
                .HasForeignKey(d => d.CuentaId)
                .HasConstraintName("FK_Cob_Tipo_Pago_CuentaId");
        });

        modelBuilder.Entity<OpCliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Op_Clientes_Id");

            entity.ToTable("Op_Clientes");

            entity.HasIndex(e => e.Cui, "KEY_Op_Clientes_Cui").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Contacto)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Cui)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Domicilio)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Mail)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Razon)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.GenderNavigation).WithMany(p => p.OpClientes)
                .HasForeignKey(d => d.Gender)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Op_Clientes_Gender");

            entity.HasOne(d => d.PaisNavigation).WithMany(p => p.OpClientes)
                .HasForeignKey(d => d.Pais)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Op_Clientes_Pais");

            entity.HasOne(d => d.RespNavigation).WithMany(p => p.OpClientes)
                .HasForeignKey(d => d.Resp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Op_Clientes_Resp");
        });

        modelBuilder.Entity<OpDocumentoProveedor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Op_Documento_Proveedor_Id");

            entity.ToTable("Op_Documento_Proveedor");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.Monto).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Razon)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Estado).WithMany(p => p.OpDocumentoProveedors)
                .HasForeignKey(d => d.EstadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Op_Documento_Proveedor_EstadoId");

            entity.HasOne(d => d.Proveedor).WithMany(p => p.OpDocumentoProveedors)
                .HasForeignKey(d => d.ProveedorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Op_Documento_Proveedor_ProveedorId");

            entity.HasOne(d => d.TipoDoc).WithMany(p => p.OpDocumentoProveedors)
                .HasForeignKey(d => d.TipoDocId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Op_Documento_Proveedor_TipoDocId");
        });

        modelBuilder.Entity<OpGender>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Op_Gender_Id");

            entity.ToTable("Op_Gender");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<OpPago>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Op_Pago_Id");

            entity.ToTable("Op_Pago");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.Operador)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.DocumentoNavigation).WithMany(p => p.OpPagos)
                .HasForeignKey(d => d.Documento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Op_Pago_Documento");

            entity.HasOne(d => d.TipoNavigation).WithMany(p => p.OpPagos)
                .HasForeignKey(d => d.Tipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Op_Pago_Tipo");
        });

        modelBuilder.Entity<OpPai>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Op_Pais_Id");

            entity.ToTable("Op_Pais");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<OpResp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Op_Resp_Id");

            entity.ToTable("Op_Resp");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SecRole>(entity =>
        {
            entity.ToTable("Sec_Roles");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SecUser>(entity =>
        {
            entity.ToTable("Sec_Users");

            entity.HasIndex(e => e.UserName, "KEY_Sec_Users_Name").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.EndOfLife).HasColumnType("datetime");
            entity.Property(e => e.RealName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.SecUsers)
                .HasForeignKey(d => d.Role)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sec_Users_Sec_Roles");
        });

        modelBuilder.Entity<StockIva>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Stock_Ivas_Id");

            entity.ToTable("Stock_Ivas");

            entity.HasIndex(e => e.Value, "KEY_Stock_Ivas_Value").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Value).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<StockProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Stock_Products_id");

            entity.ToTable("Stock_Products");

            entity.HasIndex(e => e.Plu, "KEY_Stock_Products_Plu").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Cantidad).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Internos).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Neto).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Plu)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Tasa).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IvaNavigation).WithMany(p => p.StockProducts)
                .HasForeignKey(d => d.Iva)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Stock_Products_Iva");

            entity.HasOne(d => d.RubroNavigation).WithMany(p => p.StockProducts)
                .HasForeignKey(d => d.Rubro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Stock_Products_Rubro");
        });

        modelBuilder.Entity<StockRubro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Stock_Rubros_Id");

            entity.ToTable("Stock_Rubros");

            entity.HasIndex(e => e.Name, "KEY_Stock_Rubros_Name").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SystemEmpresa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_System_Empresa_Id");

            entity.ToTable("System_Empresa");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Cuit)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Domicilio)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Fantasia)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Iibb)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IIBB");
            entity.Property(e => e.Razon)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Respo)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SystemIndex>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_System_Index_Id");

            entity.ToTable("System_Index");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Interes).HasColumnType("decimal(18, 0)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
