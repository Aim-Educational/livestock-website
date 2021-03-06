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

        public virtual DbSet<AdmmGroupMap> AdmmGroupMap { get; set; }
        public virtual DbSet<AdmuGroup> AdmuGroup { get; set; }
        public virtual DbSet<AlUserInfo> AlUserInfo { get; set; }
        public virtual DbSet<Breed> Breed { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<Critter> Critter { get; set; }
        public virtual DbSet<CritterImage> CritterImage { get; set; }
        public virtual DbSet<CritterImageVariant> CritterImageVariant { get; set; }
        public virtual DbSet<CritterLifeEvent> CritterLifeEvent { get; set; }
        public virtual DbSet<CritterLifeEventDatetime> CritterLifeEventDatetime { get; set; }
        public virtual DbSet<CritterType> CritterType { get; set; }
        public virtual DbSet<EnumCritterLifeEventCategory> EnumCritterLifeEventCategory { get; set; }
        public virtual DbSet<EnumCritterLifeEventType> EnumCritterLifeEventType { get; set; }
        public virtual DbSet<EnumLocationType> EnumLocationType { get; set; }
        public virtual DbSet<EnumProductType> EnumProductType { get; set; }
        public virtual DbSet<EnumVehicleLifeEventType> EnumVehicleLifeEventType { get; set; }
        public virtual DbSet<Holding> Holding { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<MenuHeader> MenuHeader { get; set; }
        public virtual DbSet<MenuHeaderItemMap> MenuHeaderItemMap { get; set; }
        public virtual DbSet<MenuItem> MenuItem { get; set; }
        public virtual DbSet<PoultryClassification> PoultryClassification { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductPurchase> ProductPurchase { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }
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
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<AdmmGroupMap>(entity =>
            {
                entity.HasIndex(e => new { e.GroupEntityUserId, e.GroupEntityDataId })
                    .HasName("IX_admm_group_map_user_data_ids")
                    .IsUnique();

                entity.HasIndex(e => new { e.GroupEntityUserId, e.GroupEntityDataType })
                    .HasName("IX_admm_group_map_user_datatypes");

                entity.HasIndex(e => new { e.GroupEntityUserId, e.GroupEntityDataType, e.GroupEntityDataId })
                    .HasName("IX_admm_group_map_id")
                    .IsUnique();

                entity.Property(e => e.Timestamp).IsRowVersion();
            });

            modelBuilder.Entity<AdmuGroup>(entity =>
            {
                entity.Property(e => e.Timestamp).IsRowVersion();
            });

            modelBuilder.Entity<AlUserInfo>(entity =>
            {
                entity.Property(e => e.Timestamp).IsRowVersion();
            });

            modelBuilder.Entity<Breed>(entity =>
            {
                entity.Property(e => e.Timestamp).IsRowVersion();

                entity.HasOne(d => d.BreedSocietyContact)
                    .WithMany(p => p.Breed)
                    .HasForeignKey(d => d.BreedSocietyContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_breed_contact");

                entity.HasOne(d => d.CritterType)
                    .WithMany(p => p.Breed)
                    .HasForeignKey(d => d.CritterTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_breed_critter_type");
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.Property(e => e.Timestamp).IsRowVersion();
            });

            modelBuilder.Entity<Critter>(entity =>
            {
                entity.HasIndex(e => e.TagNumber)
                    .HasName("IX_UNIQ_tag_number")
                    .IsUnique();

                entity.Property(e => e.Gender).IsUnicode(false);

                entity.Property(e => e.Timestamp).IsRowVersion();

                entity.HasOne(d => d.Breed)
                    .WithMany(p => p.Critter)
                    .HasForeignKey(d => d.BreedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_critter_breed");

                entity.HasOne(d => d.CritterImage)
                    .WithMany(p => p.Critter)
                    .HasForeignKey(d => d.CritterImageId)
                    .HasConstraintName("FK_critter_critter_image");

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

                entity.HasOne(d => d.OwnerContact)
                    .WithMany(p => p.Critter)
                    .HasForeignKey(d => d.OwnerContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_critter_contact");
            });

            modelBuilder.Entity<CritterImage>(entity =>
            {
                entity.Property(e => e.Timestamp).IsRowVersion();
            });

            modelBuilder.Entity<CritterImageVariant>(entity =>
            {
                entity.HasIndex(e => new { e.CritterImageOriginalId, e.CritterImageModifiedId })
                    .HasName("IX_critter_image_variant_id_pairs")
                    .IsUnique();

                entity.HasIndex(e => new { e.CritterImageOriginalId, e.Width, e.Height })
                    .HasName("IX_critter_image_variant_id_size")
                    .IsUnique();

                entity.Property(e => e.Timestamp).IsRowVersion();

                entity.HasOne(d => d.CritterImageModified)
                    .WithMany(p => p.CritterImageVariantCritterImageModified)
                    .HasForeignKey(d => d.CritterImageModifiedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_critter_image_variant_critter_image2");

                entity.HasOne(d => d.CritterImageOriginal)
                    .WithMany(p => p.CritterImageVariantCritterImageOriginal)
                    .HasForeignKey(d => d.CritterImageOriginalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_critter_image_variant_critter_image");
            });

            modelBuilder.Entity<CritterLifeEvent>(entity =>
            {
                entity.Property(e => e.Timestamp).IsRowVersion();

                entity.HasOne(d => d.Critter)
                    .WithMany(p => p.CritterLifeEvent)
                    .HasForeignKey(d => d.CritterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_critter_life_event_critter");

                entity.HasOne(d => d.EnumCritterLifeEventType)
                    .WithMany(p => p.CritterLifeEvent)
                    .HasForeignKey(d => d.EnumCritterLifeEventTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_critter_life_event_enum_critter_life_event_type");
            });

            modelBuilder.Entity<CritterLifeEventDatetime>(entity =>
            {
                entity.HasKey(e => e.CritterLifeEventGiveBirthId)
                    .HasName("PK_critter_life_event_give_birth");

                entity.Property(e => e.Timestamp).IsRowVersion();
            });

            modelBuilder.Entity<CritterType>(entity =>
            {
                entity.Property(e => e.Timestamp).IsRowVersion();
            });

            modelBuilder.Entity<EnumCritterLifeEventCategory>(entity =>
            {
                entity.Property(e => e.Timestamp).IsRowVersion();
            });

            modelBuilder.Entity<EnumCritterLifeEventType>(entity =>
            {
                entity.Property(e => e.Timestamp).IsRowVersion();
            });

            modelBuilder.Entity<EnumLocationType>(entity =>
            {
                entity.Property(e => e.Timestamp).IsRowVersion();
            });

            modelBuilder.Entity<EnumProductType>(entity =>
            {
                entity.Property(e => e.Timestamp).IsRowVersion();
            });

            modelBuilder.Entity<EnumVehicleLifeEventType>(entity =>
            {
                entity.Property(e => e.Timestamp).IsRowVersion();
            });

            modelBuilder.Entity<Holding>(entity =>
            {
                entity.Property(e => e.Timestamp).IsRowVersion();

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.Holding)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_holding_contact");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.Timestamp).IsRowVersion();

                entity.HasOne(d => d.EnumLocationType)
                    .WithMany(p => p.Location)
                    .HasForeignKey(d => d.EnumLocationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_location_enum_location_type");

                entity.HasOne(d => d.Holding)
                    .WithMany(p => p.Location)
                    .HasForeignKey(d => d.HoldingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_location_holding");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_location_location");
            });

            modelBuilder.Entity<MenuHeader>(entity =>
            {
                entity.Property(e => e.Timestamp).IsRowVersion();

                entity.HasOne(d => d.MenuHeaderParent)
                    .WithMany(p => p.InverseMenuHeaderParent)
                    .HasForeignKey(d => d.MenuHeaderParentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_menu_header_menu_header");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.MenuHeader)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_menu_header_role");
            });

            modelBuilder.Entity<MenuHeaderItemMap>(entity =>
            {
                entity.Property(e => e.Timestamp).IsRowVersion();

                entity.HasOne(d => d.MenuHeader)
                    .WithMany(p => p.MenuHeaderItemMap)
                    .HasForeignKey(d => d.MenuHeaderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_menu_header_item_map_menu_header");

                entity.HasOne(d => d.MenuItem)
                    .WithMany(p => p.MenuHeaderItemMap)
                    .HasForeignKey(d => d.MenuItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_menu_header_item_map_menu_item");
            });

            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.Property(e => e.Timestamp).IsRowVersion();
            });

            modelBuilder.Entity<PoultryClassification>(entity =>
            {
                entity.Property(e => e.Timestamp).IsRowVersion();

                entity.HasOne(d => d.CritterType)
                    .WithMany(p => p.PoultryClassification)
                    .HasForeignKey(d => d.CritterTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_poultry_classification_critter_type");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Timestamp).IsRowVersion();

                entity.HasOne(d => d.EnumProductType)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.EnumProductTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_product_enum_product_type");
            });

            modelBuilder.Entity<ProductPurchase>(entity =>
            {
                entity.Property(e => e.Timestamp).IsRowVersion();

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.ProductPurchase)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_product_purchase_location");

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

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Timestamp).IsRowVersion();
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.Timestamp).IsRowVersion();

                entity.HasOne(d => d.Critter)
                    .WithMany(p => p.Tag)
                    .HasForeignKey(d => d.CritterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tag_critter");
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.Property(e => e.Timestamp).IsRowVersion();
            });

            modelBuilder.Entity<VehicleLifeEvent>(entity =>
            {
                entity.Property(e => e.Timestamp).IsRowVersion();

                entity.HasOne(d => d.EnumVehicleLifeEventType)
                    .WithMany(p => p.VehicleLifeEvent)
                    .HasForeignKey(d => d.EnumVehicleLifeEventTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vehicle_life_event_enum_vehicle_life_event_type");

                entity.HasOne(d => d.VehicleTrailerMap)
                    .WithMany(p => p.VehicleLifeEvent)
                    .HasForeignKey(d => d.VehicleTrailerMapId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vehicle_life_event_vehicle_trailer_map");
            });

            modelBuilder.Entity<VehicleLifeEventWash>(entity =>
            {
                entity.Property(e => e.Timestamp).IsRowVersion();
            });

            modelBuilder.Entity<VehicleTrailerMap>(entity =>
            {
                entity.Property(e => e.Timestamp).IsRowVersion();

                entity.HasOne(d => d.Trailer)
                    .WithMany(p => p.VehicleTrailerMapTrailer)
                    .HasForeignKey(d => d.TrailerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vehicle_trailer_map_vehicle1");

                entity.HasOne(d => d.VehicleMain)
                    .WithMany(p => p.VehicleTrailerMapVehicleMain)
                    .HasForeignKey(d => d.VehicleMainId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vehicle_trailer_map_vehicle");
            });
        }
    }
}
