namespace CourseProjectAgency
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DBcontext : DbContext
    {
        public DBcontext()
            : base("name=ModelAgency")
        {
        }

        public virtual DbSet<Agent> Agents { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Flat> Flats { get; set; }
        public virtual DbSet<House> Houses { get; set; }
        public virtual DbSet<ObjectEstate> ObjectEstates { get; set; }
        public virtual DbSet<Rent> Rents { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Trade> Trades { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agent>()
                .Property(e => e.passport_series_number)
                .IsFixedLength();

            modelBuilder.Entity<Agent>()
                .Property(e => e.phone_number)
                .IsFixedLength();

            modelBuilder.Entity<Agent>()
                .Property(e => e.INN)
                .IsFixedLength();

            modelBuilder.Entity<Agent>()
                .HasMany(e => e.Trades)
                .WithRequired(e => e.Agent)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Client>()
                .Property(e => e.passport_series_number)
                .IsFixedLength();

            modelBuilder.Entity<Client>()
                .Property(e => e.phone_number)
                .IsFixedLength();

            modelBuilder.Entity<Client>()
                .HasMany(e => e.ObjectEstates)
                .WithRequired(e => e.Client)
                .HasForeignKey(e => e.owner_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Client>()
                .HasMany(e => e.Trades)
                .WithRequired(e => e.Client)
                .HasForeignKey(e => e.buyer_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ObjectEstate>()
                .HasOptional(e => e.Flat)
                .WithRequired(e => e.ObjectEstate);

            modelBuilder.Entity<ObjectEstate>()
                .HasOptional(e => e.House)
                .WithRequired(e => e.ObjectEstate);

            modelBuilder.Entity<ObjectEstate>()
                .HasMany(e => e.Trades)
                .WithRequired(e => e.ObjectEstate)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.Clients)
                .WithRequired(e => e.Role)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Status>()
                .HasMany(e => e.ObjectEstates)
                .WithRequired(e => e.Status)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Trade>()
                .HasOptional(e => e.Rent)
                .WithRequired(e => e.Trade);

            modelBuilder.Entity<Trade>()
                .HasOptional(e => e.Sale)
                .WithRequired(e => e.Trade);
        }
    }
}
