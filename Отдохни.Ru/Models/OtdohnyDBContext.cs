using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Отдохни.Ru.Models
{
    public class OtdohnyDBContext : DbContext
    {
        public OtdohnyDBContext() : base("name=OtdohnyBD_add") { }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Landlord> Landlords { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<PhotoUser> PhotoUsers { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<TypeOfProperty> TypesOfProperties { get; set; }
        public DbSet<Video> Videos { get; set; }
    }
}
