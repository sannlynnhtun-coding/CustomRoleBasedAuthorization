using System;
using System.Collections.Generic;
using CustomRoleBasedAuthorization.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomRoleBasedAuthorization.Database.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblRole> TblRoles { get; set; }

    public virtual DbSet<TblRolePermission> TblRolePermissions { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    public virtual DbSet<TblUserLogin> TblUserLogins { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Tbl_Role__8AFACE1A41E7E62A");

            entity.ToTable("Tbl_Role");

            entity.Property(e => e.RoleDescription).HasMaxLength(255);
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<TblRolePermission>(entity =>
        {
            entity.HasKey(e => e.RolePermissionId).HasName("PK__Tbl_Role__120F46BA05D62405");

            entity.ToTable("Tbl_RolePermission");

            entity.HasOne(d => d.Role).WithMany(p => p.TblRolePermissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_RoleP__RoleI__286302EC");

            entity.HasOne(d => d.User).WithMany(p => p.TblRolePermissions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_RoleP__UserI__29572725");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Tbl_User__1788CC4C85BD9AB2");

            entity.ToTable("Tbl_User");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(256);
            entity.Property(e => e.Username).HasMaxLength(100);
        });

        modelBuilder.Entity<TblUserLogin>(entity =>
        {
            entity.HasKey(e => e.UserLoginId).HasName("PK__Tbl_User__107D568C9BE5A3B9");

            entity.ToTable("Tbl_UserLogin");

            entity.Property(e => e.LogoutDate).HasColumnType("datetime");
            entity.Property(e => e.SessionExpiredDate).HasColumnType("datetime");
            entity.Property(e => e.SessionId).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.TblUserLogins)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_UserL__UserI__2C3393D0");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
