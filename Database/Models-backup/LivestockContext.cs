using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Database.Models
{
    public partial class LivestockContext : DbContext
    {
public string db;
public LivestockContext(string db){ this.db = db; }
        public LivestockContext()
        {
        }

        public LivestockContext(DbContextOptions<LivestockContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Breed> Breed { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<Critter> Critter { get; set; }
        public virtual DbSet<CritterLifeEvent> CritterLifeEvent { get; set; }
        public virtual DbSet<CritterLifeEventGiveBirth> CritterLifeEventGiveBirth { get; set; }
        public virtual DbSet<CritterType> CritterType { get; set; }
        public virtual DbSet<EnumCritterLifeEventType> EnumCritterLifeEventType { get; set; }
        public virtual DbSet<EnumLocationType> EnumLocationType { get; set; }
        public virtual DbSet<EnumProductType> EnumProductType { get; set; }
        public virtual DbSet<EnumVehicleLifeEventType> EnumVehicleLifeEventType { get; set; }
        public virtual DbSet<Holding> Holding { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<PoultryClassification> PoultryClassification { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductPurchase> ProductPurchase { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Vehicle> Vehicle { get; set; }
        public virtual DbSet<VehicleLifeEvent> VehicleLifeEvent { get; set; }
        public virtual DbSet<VehicleLifeEventWash> VehicleLifeEventWash { get; set; }
        public virtual DbSet<VehicleTrailerMap> VehicleTrailerMap { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
optionsBuilder.UseSqlServer(this.db);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Breed>(entity =>
            {
                entity.ToTable("breed");

                entity.Property(e => e.BreedId).HasColumnName("breed_id");

                entity.Property(e => e.BreedSocietyContactId).HasColumnName("breed_society_contact_id");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("comment")
                    .HasMaxLength(50);

                entity.Property(e => e.CritterTypeId).HasColumnName("critter_type_id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(50);

                entity.Property(e => e.Registerable).HasColumnName("registerable");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.VersionNumber).HasColumnName("version_number");
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("contact");

                entity.Property(e => e.ContactId).HasColumnName("contact_id");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasMaxLength(100);

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("comment")
                    .HasMaxLength(50);

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasColumnName("email_address")
                    .HasMaxLength(100);

                entity.Property(e => e.IsCustomer).HasColumnName("is_customer");

                entity.Property(e => e.IsSupplier).HasColumnName("is_supplier");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.PhoneNumber1)
                    .IsRequired()
                    .HasColumnName("phone_number1")
                    .HasMaxLength(50);

                entity.Property(e => e.PhoneNumber2)
                    .IsRequired()
                    .HasColumnName("phone_number2")
                    .HasMaxLength(50);

                entity.Property(e => e.PhoneNumber3)
                    .IsRequired()
                    .HasColumnName("phone_number3")
                    .HasMaxLength(50);

                entity.Property(e => e.PhoneNumber4)
                    .IsRequired()
                    .HasColumnName("phone_number4")
                    .HasMaxLength(50);

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.VersionNumber).HasColumnName("version_number");
            });

            modelBuilder.Entity<Critter>(entity =>
            {
                entity.ToTable("critter");

                entity.Property(e => e.CritterId).HasColumnName("critter_id");

                entity.Property(e => e.BreedId).HasColumnName("breed_id");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("comment")
                    .HasMaxLength(50);

                entity.Property(e => e.CritterTypeId).HasColumnName("critter_type_id");

                entity.Property(e => e.DadCritterId).HasColumnName("dad_critter_id");

                entity.Property(e => e.DadFurther)
                    .IsRequired()
                    .HasColumnName("dad_further")
                    .HasMaxLength(255);

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasColumnName("gender")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.MumCritterId).HasColumnName("mum_critter_id");

                entity.Property(e => e.MumFurther)
                    .IsRequired()
                    .HasColumnName("mum_further")
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.OwnerContactId).HasColumnName("owner_contact_id");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.VersionNumber).HasColumnName("version_number");

                entity.HasOne(d => d.CritterType)
                    .WithMany(p => p.Critter)
                    .HasForeignKey(d => d.CritterTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_critter_critter_type");

                entity.HasOne(d => d.DadCritter)
                    .WithMany(p => p.InverseDadCritter)
                    .HasForeignKey(d => d.DadCritterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_critter_critter1");

                entity.HasOne(d => d.MumCritter)
                    .WithMany(p => p.InverseMumCritter)
                    .HasForeignKey(d => d.MumCritterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_critter_critter");
            });

            modelBuilder.Entity<CritterLifeEvent>(entity =>
            {
                entity.ToTable("critter_life_event");

                entity.Property(e => e.CritterLifeEventId).HasColumnName("critter_life_event_id");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("comment")
                    .HasMaxLength(50);

                entity.Property(e => e.CritterId).HasColumnName("critter_id");

                entity.Property(e => e.DateTime)
                    .HasColumnName("date_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(50);

                entity.Property(e => e.EnumCritterLifeEventTypeId).HasColumnName("enum_critter_life_event_type_id");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.VersionNumber).HasColumnName("version_number");
            });

            modelBuilder.Entity<CritterLifeEventGiveBirth>(entity =>
            {
                entity.ToTable("critter_life_event_give_birth");

                entity.Property(e => e.CritterLifeEventGiveBirthId).HasColumnName("critter_life_event_give_birth_id");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("comment")
                    .HasMaxLength(50);

                entity.Property(e => e.DateTime)
                    .HasColumnName("date_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.VersionNumber).HasColumnName("version_number");
            });

            modelBuilder.Entity<CritterType>(entity =>
            {
                entity.ToTable("critter_type");

                entity.Property(e => e.CritterTypeId).HasColumnName("critter_type_id");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("comment")
                    .HasMaxLength(50);

                entity.Property(e => e.GestrationPeriod).HasColumnName("gestration_period");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.VersionNumber).HasColumnName("version_number");
            });

            modelBuilder.Entity<EnumCritterLifeEventType>(entity =>
            {
                entity.ToTable("enum_critter_life_event_type");

                entity.Property(e => e.EnumCritterLifeEventTypeId).HasColumnName("enum_critter_life_event_type_id");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("comment")
                    .HasMaxLength(50);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(50);

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.VersionNumber).HasColumnName("version_number");
            });

            modelBuilder.Entity<EnumLocationType>(entity =>
            {
                entity.ToTable("enum_location_type");

                entity.Property(e => e.EnumLocationTypeId).HasColumnName("enum_location_type_id");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("comment")
                    .HasMaxLength(50);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(50);

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.VersionNumber).HasColumnName("version_number");
            });

            modelBuilder.Entity<EnumProductType>(entity =>
            {
                entity.ToTable("enum_product_type");

                entity.Property(e => e.EnumProductTypeId).HasColumnName("enum_product_type_id");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("comment")
                    .HasMaxLength(50);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(50);

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.VersionNumber).HasColumnName("version_number");
            });

            modelBuilder.Entity<EnumVehicleLifeEventType>(entity =>
            {
                entity.ToTable("enum_vehicle_life_event_type");

                entity.Property(e => e.EnumVehicleLifeEventTypeId).HasColumnName("enum_vehicle_life_event_type_id");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("comment")
                    .HasMaxLength(50);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(50);

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.VersionNumber).HasColumnName("version_number");
            });

            modelBuilder.Entity<Holding>(entity =>
            {
                entity.ToTable("holding");

                entity.Property(e => e.HoldingId).HasColumnName("holding_id");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasMaxLength(100);

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("comment")
                    .HasMaxLength(50);

                entity.Property(e => e.ContactId).HasColumnName("contact_id");

                entity.Property(e => e.GridReference)
                    .IsRequired()
                    .HasColumnName("grid_reference")
                    .HasMaxLength(50);

                entity.Property(e => e.HoldingNumber)
                    .IsRequired()
                    .HasColumnName("holding_number")
                    .HasMaxLength(50);

                entity.Property(e => e.Postcode)
                    .IsRequired()
                    .HasColumnName("postcode")
                    .HasMaxLength(50);

                entity.Property(e => e.RegisterForCows).HasColumnName("register_for_cows");

                entity.Property(e => e.RegisterForFish).HasColumnName("register_for_fish");

                entity.Property(e => e.RegisterForPigs).HasColumnName("register_for_pigs");

                entity.Property(e => e.RegisterForPoultry).HasColumnName("register_for_poultry");

                entity.Property(e => e.RegisterForSheepGoats).HasColumnName("register_for_sheep_goats");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.VersionNumber).HasColumnName("version_number");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("location");

                entity.Property(e => e.LocationId).HasColumnName("location_id");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("comment")
                    .HasMaxLength(50);

                entity.Property(e => e.EnumLocationTypeId).HasColumnName("enum_location_type_id");

                entity.Property(e => e.HoldingId).HasColumnName("holding_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.ParentId).HasColumnName("parent_id");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.VersionNumber).HasColumnName("version_number");
            });

            modelBuilder.Entity<PoultryClassification>(entity =>
            {
                entity.ToTable("poultry_classification");

                entity.Property(e => e.PoultryClassificationId).HasColumnName("poultry_classification_id");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("comment")
                    .HasMaxLength(50);

                entity.Property(e => e.CritterTypeId).HasColumnName("critter_type_id");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.VersionNumber).HasColumnName("version_number");

                entity.HasOne(d => d.CritterType)
                    .WithMany(p => p.PoultryClassification)
                    .HasForeignKey(d => d.CritterTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_poultry_classification_critter_type");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("product");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("comment")
                    .HasMaxLength(50);

                entity.Property(e => e.DefaultVolume).HasColumnName("default_volume");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(100);

                entity.Property(e => e.EnumProductTypeId).HasColumnName("enum_product_type_id");

                entity.Property(e => e.RequiresRefridgeration).HasColumnName("requires_refridgeration");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.VersionNumber).HasColumnName("version_number");

                entity.HasOne(d => d.EnumProductType)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.EnumProductTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_product_enum_product_type");
            });

            modelBuilder.Entity<ProductPurchase>(entity =>
            {
                entity.ToTable("product_purchase");

                entity.Property(e => e.ProductPurchaseId).HasColumnName("product_purchase_id");

                entity.Property(e => e.BatchNumber)
                    .IsRequired()
                    .HasColumnName("batch_number")
                    .HasMaxLength(50);

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("comment")
                    .HasMaxLength(50);

                entity.Property(e => e.Cost)
                    .HasColumnName("cost")
                    .HasColumnType("money");

                entity.Property(e => e.DateTime)
                    .HasColumnName("date_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.Expiry)
                    .HasColumnName("expiry")
                    .HasColumnType("date");

                entity.Property(e => e.LocationId).HasColumnName("location_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.SupplierId).HasColumnName("supplier_id");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.VersionNumber).HasColumnName("version_number");

                entity.Property(e => e.Volume).HasColumnName("volume");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductPurchase)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_product_purchase_product");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.ProductPurchase)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_product_purchase_supplier");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("tag");

                entity.Property(e => e.TagId).HasColumnName("tag_id");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("comment")
                    .HasMaxLength(50);

                entity.Property(e => e.CritterId).HasColumnName("critter_id");

                entity.Property(e => e.DateTime)
                    .HasColumnName("date_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.Rfid)
                    .IsRequired()
                    .HasColumnName("rfid")
                    .HasMaxLength(255);

                entity.Property(e => e.Tag1)
                    .IsRequired()
                    .HasColumnName("tag")
                    .HasMaxLength(50);

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.VersionNumber).HasColumnName("version_number");

                entity.HasOne(d => d.Critter)
                    .WithMany(p => p.Tag)
                    .HasForeignKey(d => d.CritterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tag_critter");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("comment")
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.VersionNumber).HasColumnName("version_number");
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.ToTable("vehicle");

                entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("comment")
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.RegistrationNumber)
                    .IsRequired()
                    .HasColumnName("registration_number")
                    .HasMaxLength(50);

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.VersionNumber).HasColumnName("version_number");

                entity.Property(e => e.WeightKg).HasColumnName("weight_kg");
            });

            modelBuilder.Entity<VehicleLifeEvent>(entity =>
            {
                entity.ToTable("vehicle_life_event");

                entity.Property(e => e.VehicleLifeEventId).HasColumnName("vehicle_life_event_id");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("comment")
                    .HasMaxLength(50);

                entity.Property(e => e.DateTime)
                    .HasColumnName("date_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(50);

                entity.Property(e => e.EnumVehicleLifeEventTypeId).HasColumnName("enum_vehicle_life_event_type_id");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.VehicleTrailerMapId).HasColumnName("vehicle_trailer_map_id");

                entity.Property(e => e.VersionNumber).HasColumnName("version_number");
            });

            modelBuilder.Entity<VehicleLifeEventWash>(entity =>
            {
                entity.ToTable("vehicle_life_event_wash");

                entity.Property(e => e.VehicleLifeEventWashId).HasColumnName("vehicle_life_event_wash_id");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("comment")
                    .HasMaxLength(50);

                entity.Property(e => e.DateTime)
                    .HasColumnName("date_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.VehicleLifeEventId).HasColumnName("vehicle_life_event_id");

                entity.Property(e => e.VersionNumber).HasColumnName("version_number");
            });

            modelBuilder.Entity<VehicleTrailerMap>(entity =>
            {
                entity.ToTable("vehicle_trailer_map");

                entity.Property(e => e.VehicleTrailerMapId).HasColumnName("vehicle_trailer_map_id");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("comment")
                    .HasMaxLength(50);

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.TrailerId).HasColumnName("trailer_id");

                entity.Property(e => e.VehicleMainId).HasColumnName("vehicle_main_id");

                entity.Property(e => e.VersionNumber).HasColumnName("version_number");
            });
        }
    }
}
