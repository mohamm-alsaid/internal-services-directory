using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MultCo_ISD_API.Models
{
    public partial class InternalServicesDirectoryV1Context : DbContext
    {
        public InternalServicesDirectoryV1Context()
        {
        }

        public InternalServicesDirectoryV1Context(DbContextOptions<InternalServicesDirectoryV1Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Community> Community { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Division> Division { get; set; }
        public virtual DbSet<Language> Language { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<LocationType> LocationType { get; set; }
        public virtual DbSet<Program> Program { get; set; }
        public virtual DbSet<ServiceCommunityAssociation> ServiceCommunityAssociation { get; set; }
        public virtual DbSet<Service> Service { get; set; }
        public virtual DbSet<ServiceLanguageAssociation> ServiceLanguageAssociation { get; set; }
        public virtual DbSet<ServiceLocationAssociation> ServiceLocationAssociation { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost;Database=InternalServicesDirectoryV1;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Community>(entity =>
            {
                entity.HasIndex(e => e.CommunityName)
                    .HasName("UQ__Communit__C43D6630B3562EB2")
                    .IsUnique();

                entity.Property(e => e.CommunityId).HasColumnName("communityID");

                entity.Property(e => e.CommunityDescription)
                    .HasColumnName("communityDescription")
                    .HasMaxLength(255);

                entity.Property(e => e.CommunityName)
                    .HasColumnName("communityName")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.Property(e => e.ContactId).HasColumnName("contactID");

                entity.Property(e => e.ContactName)
                    .HasColumnName("contactName")
                    .HasMaxLength(255);

                entity.Property(e => e.EmailAddress)
                    .HasColumnName("emailAddress")
                    .HasMaxLength(255);

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phoneNumber")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasIndex(e => e.DepartmentCode)
                    .HasName("UQ__Departme__7BF423ADD0DE60CE")
                    .IsUnique();

                entity.Property(e => e.DepartmentId).HasColumnName("departmentID");

                entity.Property(e => e.DepartmentCode).HasColumnName("departmentCode");

                entity.Property(e => e.DepartmentName)
                    .HasColumnName("departmentName")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Division>(entity =>
            {
                entity.HasIndex(e => e.DivisionCode)
                    .HasName("UQ__Division__B000E8B4DA1322F9")
                    .IsUnique();

                entity.Property(e => e.DivisionId).HasColumnName("divisionID");

                entity.Property(e => e.DivisionCode).HasColumnName("divisionCode");

                entity.Property(e => e.DivisionName)
                    .HasColumnName("divisionName")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.HasIndex(e => e.LanguageName)
                    .HasName("UQ__Language__6E492863D8D61341")
                    .IsUnique();

                entity.Property(e => e.LanguageId).HasColumnName("languageID");

                entity.Property(e => e.LanguageName)
                    .HasColumnName("languageName")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasIndex(e => e.LocationName)
                    .HasName("UQ__Location__930DB01657A2685B")
                    .IsUnique();

                entity.Property(e => e.LocationId).HasColumnName("locationID");

                entity.Property(e => e.BuildingId)
                    .HasColumnName("buildingID")
                    .HasMaxLength(255);

                entity.Property(e => e.FloorNumber)
                    .HasColumnName("floorNumber")
                    .HasMaxLength(255);

                entity.Property(e => e.LocationAddress)
                    .HasColumnName("locationAddress")
                    .HasMaxLength(255);

                entity.Property(e => e.LocationName)
                    .HasColumnName("locationName")
                    .HasMaxLength(255);

                entity.Property(e => e.LocationTypeId).HasColumnName("locationTypeID");

                entity.Property(e => e.RoomNumber)
                    .HasColumnName("roomNumber")
                    .HasMaxLength(255);

                entity.HasOne(d => d.LocationType)
                    .WithMany(p => p.Location)
                    .HasForeignKey(d => d.LocationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Location__locati__46E78A0C");
            });

            modelBuilder.Entity<LocationType>(entity =>
            {
                entity.HasIndex(e => e.LocationTypeName)
                    .HasName("UQ__Location__6DC168678A6A427A")
                    .IsUnique();

                entity.Property(e => e.LocationTypeId).HasColumnName("locationTypeID");

                entity.Property(e => e.LocationTypeName)
                    .IsRequired()
                    .HasColumnName("locationTypeName")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Program>(entity =>
            {
                entity.Property(e => e.ProgramId).HasColumnName("programID");

                entity.Property(e => e.OfferType)
                    .HasColumnName("offerType")
                    .HasMaxLength(255);

                entity.Property(e => e.SponsorName)
                    .HasColumnName("sponsorName")
                    .HasMaxLength(255);

                entity.Property(e => e.ProgramName)
                    .HasColumnName("programName")
                    .HasMaxLength(255);

                entity.Property(e => e.ProgramOfferNumber)
                    .HasColumnName("programOfferNumber")
                    .HasMaxLength(255);

                entity.HasIndex(e => e.ProgramOfferNumber)
                    .HasName("Unique_ProgramOfferNumber")
                    .IsUnique();
            });

            modelBuilder.Entity<ServiceCommunityAssociation>(entity =>
            {

                entity.HasIndex(e => new { e.ServiceId, e.CommunityId })
                    .HasName("uk_ServiceCommunityAssociation")
                    .IsUnique();

                entity.Property(e => e.ServiceCommunityAssociationId).HasColumnName("serviceCommunityAssociationID");

                entity.Property(e => e.CommunityId).HasColumnName("communityID");

                entity.Property(e => e.ServiceId).HasColumnName("serviceID");

                entity.HasOne(d => d.Community)
                    .WithMany(p => p.ServiceCommunityAssociation)
                    .HasForeignKey(d => d.CommunityId)
                    .HasConstraintName("FK__ProgramCo__commu__440B1D61");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ServiceCommunityAssociation)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK__ProgramCo__servi__4316F928");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.Property(e => e.ServiceId).HasColumnName("serviceID");

                entity.Property(e => e.ContactId).HasColumnName("contactID");

                entity.Property(e => e.CustomerConnectMethod)
                    .HasColumnName("customerConnectMethod")
                    .HasMaxLength(255);

                entity.Property(e => e.DepartmentId).HasColumnName("departmentID");

                entity.Property(e => e.DivisionId).HasColumnName("divisionID");

                entity.Property(e => e.EmployeeConnectMethod)
                    .HasColumnName("employeeConnectMethod")
                    .HasMaxLength(255);

                entity.Property(e => e.ExecutiveSummary)
                    .HasColumnName("executiveSummary")
                    .HasMaxLength(6000);

                entity.Property(e => e.ExpirationDate)
                    .HasColumnName("expirationDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasColumnType("bit");

                entity.Property(e => e.ProgramId).HasColumnName("programID");

                entity.Property(e => e.ServiceArea)
                    .HasColumnName("serviceArea")
                    .HasMaxLength(255);

                entity.Property(e => e.ServiceDescription)
                    .HasColumnName("serviceDescription")
                    .HasMaxLength(6000);

                entity.Property(e => e.ServiceName)
                    .HasColumnName("serviceName")
                    .HasMaxLength(255);

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.Service)
                    .HasForeignKey(d => d.ContactId)
                    .HasConstraintName("FK__Service__contact__403A8C7D");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Service)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK__Service__departm__412EB0B6");

                entity.HasOne(d => d.Division)
                    .WithMany(p => p.Service)
                    .HasForeignKey(d => d.DivisionId)
                    .HasConstraintName("FK__Service__divisio__4222D4EF");

                entity.HasOne(d => d.Program)
                    .WithMany(p => p.Service)
                    .HasForeignKey(d => d.ProgramId)
                    .HasConstraintName("FK__Service__program__3F466844");
            });

            modelBuilder.Entity<ServiceLanguageAssociation>(entity =>
            {
                entity.HasIndex(e => new { e.ServiceId, e.LanguageId })
                    .HasName("uk_ServiceLanguageAssociation")
                    .IsUnique();

                entity.HasKey(e => e.ServiceLanguageAssociation1)
                    .HasName("PK__ServiceL__01B8A41162110311");

                entity.Property(e => e.ServiceLanguageAssociation1).HasColumnName("serviceLanguageAssociation");

                entity.Property(e => e.LanguageId).HasColumnName("languageID");

                entity.Property(e => e.ServiceId).HasColumnName("serviceID");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.ServiceLanguageAssociation)
                    .HasForeignKey(d => d.LanguageId)
                    .HasConstraintName("FK__ServiceLa__langu__44FF419A");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ServiceLanguageAssociation)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK__ServiceLa__servi__48CFD27E");
            });

            modelBuilder.Entity<ServiceLocationAssociation>(entity =>
            {
                entity.HasIndex(e => new { e.ServiceId, e.LocationId })
                    .HasName("uk_ServiceLocationAssociation")
                    .IsUnique();

                entity.HasKey(e => e.ServiceLocationAssociation1)
                    .HasName("PK__ServiceL__9D5CE7263ED14FFB");

                entity.Property(e => e.ServiceLocationAssociation1).HasColumnName("serviceLocationAssociation");

                entity.Property(e => e.LocationId).HasColumnName("locationID");

                entity.Property(e => e.ServiceId).HasColumnName("serviceID");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.ServiceLocationAssociation)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK__ServiceLo__locat__47DBAE45");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ServiceLocationAssociation)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK__ServiceLo__servi__45F365D3");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
