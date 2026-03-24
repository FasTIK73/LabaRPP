using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RPP.Database.Models;

namespace RPP.Database.Configurations;

public class HomeConfiguration : IEntityTypeConfiguration<HomeEntity>
{
    public void Configure(EntityTypeBuilder<HomeEntity> builder)
    {
        builder.HasIndex(x => x.Address).IsUnique();
        builder.HasIndex(x => x.Type);
        builder.HasIndex(x => x.Status);

        builder.HasMany(x => x.Reports)
            .WithOne(x => x.Home)
            .HasForeignKey(x => x.HomeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}