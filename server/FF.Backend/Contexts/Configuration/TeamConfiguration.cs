using FF.Backend.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FF.Backend.Contexts.Configuration
{
    internal class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.ToTable("Team");
            builder.HasKey(x => new { x.Id });

            builder.HasOne(f => f.League)
                .WithMany(p => p.Teams)
                .HasForeignKey(f => f.LeagueId);

            builder.HasOne(f => f.Manager)
               .WithMany(p => p.Teams)
               .HasForeignKey(f => f.ManagerId);
        }
    }
}
