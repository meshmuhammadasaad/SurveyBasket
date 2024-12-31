using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyBasket.Api.Entities;

namespace SurveyBasket.Api.Persistence.EntitiesConfigurations;

public class QuestianConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasIndex(q => new { q.PollId, q.Content }).IsUnique();

        builder.Property(q => q.Content).HasMaxLength(1000);
    }
}
