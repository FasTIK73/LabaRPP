using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RPP.Database.Models;

namespace RPP.Database.Configurations;

public class WorkTypeConfiguration : IEntityTypeConfiguration<WorkTypeEntity>
{
    public void Configure(EntityTypeBuilder<WorkTypeEntity> builder)
    {
        builder.HasIndex(x => x.WorkName).IsUnique();
        builder.HasIndex(x => x.Unit);

        builder.HasMany(x => x.Reports)
            .WithOne(x => x.WorkType)
            .HasForeignKey(x => x.WorkTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}