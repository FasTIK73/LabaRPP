using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RPP.Database.Models;

namespace RPP.Database.Configurations;

public class ReportConfiguration : IEntityTypeConfiguration<ReportEntity>
{
    public void Configure(EntityTypeBuilder<ReportEntity> builder)
    {
        builder.HasIndex(x => x.WorkDate);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => new { x.HomeId, x.WorkDate });
        builder.HasIndex(x => new { x.WorkerId, x.WorkDate });
    }
}