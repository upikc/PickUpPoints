using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace StorageApi.Model;

public partial class DbstorageContext : DbContext
{
    public DbstorageContext()
    {
    }

    public DbstorageContext(DbContextOptions<DbstorageContext> options)
        : base(options)
    {
    }

    public virtual DbSet<OperationType> OperationTypes { get; set; }

    public virtual DbSet<Package> Packages { get; set; }

    public virtual DbSet<PackagesWithstatus> PackagesWithstatuses { get; set; }

    public virtual DbSet<PkgOperation> PkgOperations { get; set; }

    public virtual DbSet<PkgOperationsWithtype> PkgOperationsWithtypes { get; set; }

    public virtual DbSet<PkqOperationsWithstorage> PkqOperationsWithstorages { get; set; }

    public virtual DbSet<Storage> Storages { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<UsersWithrole> UsersWithroles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseLazyLoadingProxies().UseMySql("server=localhost;user=root;password=1234;port=3307;database=dbstorage", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.39-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<OperationType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PRIMARY");

            entity.ToTable("operation_types");

            entity.Property(e => e.TypeId)
                .ValueGeneratedNever()
                .HasColumnName("type_id");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .HasColumnName("type");
        });

        modelBuilder.Entity<Package>(entity =>
        {
            entity.HasKey(e => e.PackageId).HasName("PRIMARY");

            entity.ToTable("packages");

            entity.Property(e => e.PackageId)
                .ValueGeneratedNever()
                .HasColumnName("package_id");
            entity.Property(e => e.ClientFullname)
                .HasMaxLength(150)
                .HasColumnName("client_fullname");
            entity.Property(e => e.ClientMail)
                .HasMaxLength(90)
                .HasColumnName("client_mail");
            entity.Property(e => e.ClientNumber)
                .HasMaxLength(12)
                .HasColumnName("client_number");
            entity.Property(e => e.Weight)
                .HasPrecision(7, 2)
                .HasColumnName("weight");
        });

        modelBuilder.Entity<PackagesWithstatus>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("packages_withstatus");

            entity.Property(e => e.ActionstorageId).HasColumnName("actionstorage_id");
            entity.Property(e => e.ClientFullname)
                .HasMaxLength(150)
                .HasColumnName("client_fullname");
            entity.Property(e => e.ClientMail)
                .HasMaxLength(90)
                .HasColumnName("client_mail");
            entity.Property(e => e.ClientNumber)
                .HasMaxLength(12)
                .HasColumnName("client_number");
            entity.Property(e => e.PackageId).HasColumnName("package_id");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");
            entity.Property(e => e.StatusDate)
                .HasColumnType("datetime")
                .HasColumnName("status_date");
            entity.Property(e => e.Weight)
                .HasPrecision(7, 2)
                .HasColumnName("weight");
        });

        modelBuilder.Entity<PkgOperation>(entity =>
        {
            entity.HasKey(e => e.OperationId).HasName("PRIMARY");

            entity.ToTable("pkg_operations");

            entity.HasIndex(e => e.PackageId, "fk6_idx");

            entity.HasIndex(e => e.TypeId, "fk7_idx");

            entity.HasIndex(e => e.UserId, "fk8_idx");

            entity.HasIndex(e => e.ActionstorageId, "fk9_idx");

            entity.Property(e => e.OperationId).HasColumnName("operation_id");
            entity.Property(e => e.ActionstorageId).HasColumnName("actionstorage_id");
            entity.Property(e => e.OperationDate)
                .HasColumnType("datetime")
                .HasColumnName("operation_date");
            entity.Property(e => e.PackageId).HasColumnName("package_id");
            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Actionstorage).WithMany(p => p.PkgOperations)
                .HasForeignKey(d => d.ActionstorageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk9");

            entity.HasOne(d => d.Package).WithMany(p => p.PkgOperations)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk6");

            entity.HasOne(d => d.Type).WithMany(p => p.PkgOperations)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk7");

            entity.HasOne(d => d.User).WithMany(p => p.PkgOperations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk8");
        });

        modelBuilder.Entity<PkgOperationsWithtype>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("pkg_operations_withtype");

            entity.Property(e => e.ActionstorageId).HasColumnName("actionstorage_id");
            entity.Property(e => e.OperationDate)
                .HasColumnType("datetime")
                .HasColumnName("operation_date");
            entity.Property(e => e.OperationId).HasColumnName("operation_id");
            entity.Property(e => e.PackageId).HasColumnName("package_id");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .HasColumnName("type");
            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<PkqOperationsWithstorage>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("pkq_operations_withstorages");

            entity.Property(e => e.ActionstorageId).HasColumnName("actionstorage_id");
            entity.Property(e => e.CommandingstorageId).HasColumnName("commandingstorage_id");
            entity.Property(e => e.OperationDate)
                .HasColumnType("datetime")
                .HasColumnName("operation_date");
            entity.Property(e => e.OperationId).HasColumnName("operation_id");
            entity.Property(e => e.PackageId).HasColumnName("package_id");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .HasColumnName("type");
            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<Storage>(entity =>
        {
            entity.HasKey(e => e.StorageId).HasName("PRIMARY");

            entity.ToTable("storages");

            entity.Property(e => e.StorageId)
                .ValueGeneratedNever()
                .HasColumnName("storage_id");
            entity.Property(e => e.StorageAddr)
                .HasMaxLength(90)
                .HasColumnName("storage_addr");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.RoleId, "fk1_idx");

            entity.HasIndex(e => e.StorageId, "fk2_idx");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(45)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(45)
                .HasColumnName("last_name");
            entity.Property(e => e.Login)
                .HasMaxLength(20)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNum)
                .HasMaxLength(12)
                .HasColumnName("phone_num");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.StorageId).HasColumnName("storage_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk4");

            entity.HasOne(d => d.Storage).WithMany(p => p.Users)
                .HasForeignKey(d => d.StorageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk5");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.ToTable("user_roles");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("role_id");
            entity.Property(e => e.Role)
                .HasMaxLength(45)
                .HasColumnName("role");
        });

        modelBuilder.Entity<UsersWithrole>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("users_withroles");

            entity.Property(e => e.FirstName)
                .HasMaxLength(45)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(45)
                .HasColumnName("last_name");
            entity.Property(e => e.Login)
                .HasMaxLength(20)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNum)
                .HasMaxLength(12)
                .HasColumnName("phone_num");
            entity.Property(e => e.Role)
                .HasMaxLength(45)
                .HasColumnName("role");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.StorageId).HasColumnName("storage_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
