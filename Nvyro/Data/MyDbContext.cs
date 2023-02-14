using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nvyro.Models;

namespace Nvyro.Data
{
    public class MyDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IConfiguration _configuration;
        public MyDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = _configuration.GetConnectionString("MyConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }

/*        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Event>().HasMany(c => c.Categories).WithMany(o => o.).OnDelete(DeleteBehavior.Cascade);
            base.OnModelCreating(builder);
        }*/
        public DbSet<RecycleCategory> RecycleCategory { get; set; }
        public DbSet<Event> Events { get; set; }

        public DbSet<Reward> Reward { get; set; }

        public DbSet<Request> Requests { get; set; }

        public DbSet<Request_Images> Request_Images { get; set; }

        public DbSet<Post> Posts { get; set; }
    }
}
