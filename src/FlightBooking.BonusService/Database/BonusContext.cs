using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using FlightBooking.BonusService.Database.Entities;

namespace FlightBooking.BonusService.Database
{
    public partial class BonusContext : DbContext
    {
        public BonusContext()
        {
        }

        public BonusContext(DbContextOptions<BonusContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Privilege> Privileges { get; set; } = null!;
        public virtual DbSet<PrivilegeHistory> PrivilegeHistories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Privilege>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValueSql("'BRONZE'::character varying");
            });

            modelBuilder.Entity<PrivilegeHistory>(entity =>
            {
                entity.HasOne(d => d.Privilege)
                    .WithMany(p => p.PrivilegeHistories)
                    .HasForeignKey(d => d.PrivilegeId)
                    .HasConstraintName("privilege_history_privilege_id_fkey");
            });
        }
    }
}
