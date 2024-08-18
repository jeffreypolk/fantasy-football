using FF.Backend.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FF.Backend.Contexts.Configuration
{
    internal class PlayerTeamConfiguration : IEntityTypeConfiguration<PlayerTeam>
    {
        public void Configure(EntityTypeBuilder<PlayerTeam> builder)
        {
            builder.ToTable("PlayerTeam");
            builder.HasKey(x => new { x.Id });

            builder.HasOne(f => f.Player)
                .WithMany(p => p.Teams)
                .HasForeignKey(f => f.PlayerId);

            builder.HasOne(f => f.Team)
               .WithMany(p => p.Players)
               .HasForeignKey(f => f.TeamId);
        }
    }
}
