using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RPP.Database.Models;

namespace RPP.Database.Configurations;

public class WorkerConfiguration : IEntityTypeConfiguration<WorkerEntity>
{
    public void Configure(EntityTypeBuilder<WorkerEntity> builder)
    {
        builder.HasIndex(x => x.PhoneNumber).IsUnique();
        builder.HasIndex(x => x.Email).IsUnique();
        builder.HasIndex(x => x.Post);
        builder.HasIndex(x => x.IsDeleted);

        builder.HasMany(x => x.Reports)
            .WithOne(x => x.Worker)
            .HasForeignKey(x => x.WorkerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}