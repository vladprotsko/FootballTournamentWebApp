using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FTWebApplication
{
    public partial class Football_Toutnament_LabContext : DbContext
    {
        public Football_Toutnament_LabContext()
        {
        }

        public Football_Toutnament_LabContext(DbContextOptions<Football_Toutnament_LabContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Match> Matches { get; set; } = null!;
        public virtual DbSet<Player> Players { get; set; } = null!;
        public virtual DbSet<Stadium> Stadiums { get; set; } = null!;
        public virtual DbSet<Team> Teams { get; set; } = null!;
        public virtual DbSet<staff> staff { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-49VRF50\\SQLEXPRESS; Database=Football_Toutnament_Lab; Trusted_Connection=True; ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Match>(entity =>
            {
                entity.HasKey(e => e.IdMatch);

                entity.ToTable("MATCHES");

                entity.Property(e => e.IdMatch)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_match");

                entity.Property(e => e.BallFirst).HasColumnName("ball_first");

                entity.Property(e => e.BallSecond).HasColumnName("ball_second");

                entity.Property(e => e.CostTicket)
                    .HasColumnType("money")
                    .HasColumnName("cost_ticket");

                entity.Property(e => e.DateMatch)
                    .HasColumnType("datetime")
                    .HasColumnName("date_match");

                entity.Property(e => e.IdStadium).HasColumnName("id_stadium");

                entity.Property(e => e.IdTeamAt).HasColumnName("id_team_at");

                entity.Property(e => e.IdTeamHt)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id_team_ht");

                entity.HasOne(d => d.IdStadiumNavigation)
                    .WithMany(p => p.Matches)
                    .HasForeignKey(d => d.IdStadium)
                    .HasConstraintName("FK_MATCHES_STADIUMS");

                entity.HasOne(d => d.IdTeamAtNavigation)
                    .WithMany(p => p.MatchIdTeamAtNavigations)
                    .HasForeignKey(d => d.IdTeamAt)
                    .HasConstraintName("FK_MATCHES_TEAMS");

                entity.HasOne(d => d.IdTeamHtNavigation)
                    .WithMany(p => p.MatchIdTeamHtNavigations)
                    .HasForeignKey(d => d.IdTeamHt)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MATCHES_TEAMS1");
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasKey(e => e.IdPlayer);

                entity.ToTable("PLAYERS");

                entity.Property(e => e.IdPlayer).HasColumnName("ID_player");

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.Fio)
                    .HasMaxLength(255)
                    .HasColumnName("fio");

                entity.Property(e => e.IdTeamFk).HasColumnName("id_team_fk");

                entity.Property(e => e.NumberPlayer).HasColumnName("number_player");

                entity.Property(e => e.RolePlayer)
                    .HasMaxLength(255)
                    .HasColumnName("role_player");

                entity.HasOne(d => d.IdTeamFkNavigation)
                    .WithMany(p => p.Players)
                    .HasForeignKey(d => d.IdTeamFk)
                    .HasConstraintName("FK_PLAYERS_TEAMS");
            });

            modelBuilder.Entity<Stadium>(entity =>
            {
                entity.HasKey(e => e.IdStadium);

                entity.ToTable("STADIUMS");

                entity.Property(e => e.IdStadium).HasColumnName("ID_stadium");

                entity.Property(e => e.Capacity).HasColumnName("capacity");

                entity.Property(e => e.City)
                    .HasMaxLength(255)
                    .HasColumnName("city");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(e => e.IdTeam);

                entity.ToTable("TEAMS");

                entity.Property(e => e.IdTeam).HasColumnName("ID_team");

                entity.Property(e => e.Base)
                    .HasMaxLength(255)
                    .HasColumnName("base");

                entity.Property(e => e.Coach)
                    .HasMaxLength(255)
                    .HasColumnName("coach");

                entity.Property(e => e.Defeat).HasColumnName("defeat");

                entity.Property(e => e.Draw).HasColumnName("draw");

                entity.Property(e => e.NameTeam)
                    .HasMaxLength(255)
                    .HasColumnName("name_team");

                entity.Property(e => e.Points).HasColumnName("points");

                entity.Property(e => e.PositionTeam).HasColumnName("position_team");

                entity.Property(e => e.Win).HasColumnName("win");
            });

            modelBuilder.Entity<staff>(entity =>
            {
                entity.HasKey(e => e.IdStaff);

                entity.ToTable("STAFF");

                entity.Property(e => e.IdStaff).HasColumnName("ID_staff");

                entity.Property(e => e.FioStaff)
                    .HasMaxLength(255)
                    .HasColumnName("fio_staff");

                entity.Property(e => e.IdTeamFkk).HasColumnName("id_team_fkk");

                entity.Property(e => e.RoleStaff)
                    .HasMaxLength(255)
                    .HasColumnName("role_staff");

                entity.HasOne(d => d.IdTeamFkkNavigation)
                    .WithMany(p => p.staff)
                    .HasForeignKey(d => d.IdTeamFkk)
                    .HasConstraintName("FK_STAFF_TEAMS");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
