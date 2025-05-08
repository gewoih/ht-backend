using HT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HT.Infrastructure.Configurations;

public class JournalLogConfiguration: IEntityTypeConfiguration<JournalLog>
{
    public void Configure(EntityTypeBuilder<JournalLog> builder)
    {
    }
}
