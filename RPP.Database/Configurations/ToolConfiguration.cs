using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RPP.Database.Models;

namespace RPP.Database.Configurations;

public class ToolConfiguration : IEntityTypeConfiguration<ToolEntity>
{
    public void Configure(EntityTypeBuilder<ToolEntity> builder)
    {
        builder.HasIndex(x => x.ToolName).IsUnique();
        builder.HasIndex(x => x.IsAvailable);

        builder.HasMany(x => x.Reports)
            .WithOne(x => x.Tool)
            .HasForeignKey(x => x.ToolId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}