using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.EntityFrameworkCore;
using DataModelAccess;

namespace API_ISD.Models
{
    public class ISDContext: DbContext
    {
        public ISDContext() { }

        public ISDContext(DbContextOptions<ISDContext> options) : base(options) { }
        
        public DbSet<Community> Communities { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Division> Divisions { get; set; }

        public DbSet<Language> Languages { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<LocationType> locationTypes { get; set; }

        public DbSet<Program> Programs { get; set; }

        public DbSet<ProgramCommunityAssociation> programCommunityAssociations { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<ServiceLanguageAssociation> ServiceLanguageAssociations { get; set; }

        public DbSet<ServiceLocationAssociation> serviceLocationAssociations { get; set; }
    }
}