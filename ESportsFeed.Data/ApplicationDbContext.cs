using ESportsFeed.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ESportsFeed.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() 
        { 
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Sport> Sports { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Market> Markets { get; set; }
        public DbSet<Odd> Odds { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=esportsFeed;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");
            }
        }
    }
}