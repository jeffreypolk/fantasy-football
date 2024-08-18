using FF.Backend.Domain;
using FF.Backend.Contexts.Configuration;
using Microsoft.EntityFrameworkCore;

namespace FF.Backend.Contexts
{
    public class FFContext : DbContext
    {
        private readonly string _connectionString;
        public FFContext(string connectionString) : base()
        {
            _connectionString = connectionString;
        }
        public FFContext(DbContextOptions<FFContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new LeagueConfiguration());
            modelBuilder.ApplyConfiguration(new ManagerConfiguration());
            modelBuilder.ApplyConfiguration(new PlayerConfiguration());
            modelBuilder.ApplyConfiguration(new TeamConfiguration());

        }

        public DbSet<League> Leagues { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }

    }
}