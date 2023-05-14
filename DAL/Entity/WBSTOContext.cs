using System;
using DAL.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DAL
{
    public partial class WBSTOContext : IdentityDbContext<User>
    {
        public WBSTOContext()
        {
        }

        public WBSTOContext(DbContextOptions<WBSTOContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cruise> Cruises { get; set; }
        public virtual DbSet<Day> Days { get; set; }
        public virtual DbSet<Halt> Halts { get; set; }
        public virtual DbSet<Locality> Localities { get; set; }
        public virtual DbSet<Route> Routes { get; set; }
        public virtual DbSet<RouteHalt> RouteHalts { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-TSE3JH5\\SQLEXPRESS;Database=WBSTO;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Cruise>(entity =>
            {
                entity.ToTable("Cruise");

                entity.Property(e => e.CruiseId).HasColumnName("cruise_id");

                entity.Property(e => e.Day).HasColumnName("day");

                entity.Property(e => e.EndingDate)
                    .HasColumnType("date")
                    .HasColumnName("ending_date");

                entity.Property(e => e.Hidden).HasColumnName("hidden");

                entity.Property(e => e.Places).HasColumnName("places");

                entity.Property(e => e.RouteId).HasColumnName("route_id");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("start_date");

                entity.Property(e => e.Time)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("time");

                entity.HasOne(d => d.DayNavigation)
                    .WithMany(p => p.Cruises)
                    .HasForeignKey(d => d.Day)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cruise_Days");

                entity.HasOne(d => d.Route)
                    .WithMany(p => p.Cruises)
                    .HasForeignKey(d => d.RouteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cruise_Route");
            });

            modelBuilder.Entity<Day>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Day1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Day");
            });

            modelBuilder.Entity<Halt>(entity =>
            {
                entity.ToTable("Halt");

                entity.Property(e => e.HaltId).HasColumnName("halt_id");

                entity.Property(e => e.Adress)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("adress");

                entity.Property(e => e.Hidden).HasColumnName("hidden");

                entity.Property(e => e.LocalityId).HasColumnName("locality_id");

                entity.HasOne(d => d.Locality)
                    .WithMany(p => p.Halts)
                    .HasForeignKey(d => d.LocalityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Halt_Localities");
            });

            modelBuilder.Entity<Locality>(entity =>
            {
                entity.Property(e => e.LocalityId).HasColumnName("locality_id");

                entity.Property(e => e.LocalityName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("locality_name");
            });

            modelBuilder.Entity<Route>(entity =>
            {
                entity.ToTable("Route");

                entity.Property(e => e.RouteId).HasColumnName("route_id");

                entity.Property(e => e.Hidden).HasColumnName("hidden");
            });

            modelBuilder.Entity<RouteHalt>(entity =>
            {
                entity.ToTable("RouteHalt");

                entity.Property(e => e.RouteHaltId).HasColumnName("route_halt_id");

                entity.Property(e => e.Cost).HasColumnName("cost");

                entity.Property(e => e.HaltId).HasColumnName("halt_id");

                entity.Property(e => e.Hidden).HasColumnName("hidden");

                entity.Property(e => e.NumberInRoute).HasColumnName("number_in_route");

                entity.Property(e => e.RouteId).HasColumnName("route_id");

                entity.Property(e => e.Time).HasColumnName("time");

                entity.HasOne(d => d.Halt)
                    .WithMany(p => p.RouteHalts)
                    .HasForeignKey(d => d.HaltId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RouteHalt_Halt");

                entity.HasOne(d => d.Route)
                    .WithMany(p => p.RouteHalts)
                    .HasForeignKey(d => d.RouteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RouteHalt_Route");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("Ticket");

                entity.Property(e => e.TicketId).HasColumnName("ticket_id");

                entity.Property(e => e.Closed).HasColumnName("closed");

                entity.Property(e => e.Cost).HasColumnName("cost");

                entity.Property(e => e.CruiseId).HasColumnName("cruise_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email");

                entity.Property(e => e.EndHaltId).HasColumnName("end_halt_id");

                entity.Property(e => e.Place).HasColumnName("place");

                entity.Property(e => e.Returned).HasColumnName("returned");

                entity.Property(e => e.RouteId).HasColumnName("route_id");

                entity.Property(e => e.Rplace).HasColumnName("rplace");

                entity.Property(e => e.Rtime).HasColumnName("rtime");

                entity.Property(e => e.SellingTime)
                    .HasColumnType("datetime")
                    .HasColumnName("selling_time");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("start_date");

                entity.Property(e => e.StartHaltId).HasColumnName("start_halt_id");

                entity.HasOne(d => d.Cruise)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.CruiseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ticket_Cruise");

                entity.HasOne(d => d.EndHalt)
                    .WithMany(p => p.TicketEndHalts)
                    .HasForeignKey(d => d.EndHaltId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ticket_RouteHalt");

                entity.HasOne(d => d.StartHalt)
                    .WithMany(p => p.TicketStartHalts)
                    .HasForeignKey(d => d.StartHaltId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ticket_Halt");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
