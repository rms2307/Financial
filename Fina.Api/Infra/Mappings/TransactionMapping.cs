using Fina.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fina.Api.Infra.Mappings
{
    public class TransactionMapping : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transaction");

            builder.HasKey(c => c.Id);
            builder.HasIndex(c => c.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(80);

            builder.Property(x => x.Type)
                .IsRequired();

            builder.Property(x => x.Amount)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.PaidOrReceivedAt)
                .IsRequired();

            builder.Property(x => x.UserId)
                .IsRequired()
                .HasMaxLength(160);
        }
    }
}
