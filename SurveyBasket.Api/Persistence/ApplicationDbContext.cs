using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.Api.Entities;
using SurveyBasket.Api.Extensions;
using System.Reflection;

namespace SurveyBasket.Api.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : 
    IdentityDbContext<ApplicationUser,ApplicationRole,string>(options)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public DbSet<Poll> Polls { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Vote> Votes { get; set; }
    public DbSet<VoteAnswer> VoteAnswers { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        var cascadeFKs = modelBuilder.Model
            .GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(t => t.DeleteBehavior == DeleteBehavior.Cascade && !t.IsOwnership);

        foreach (var Fk in cascadeFKs)
            Fk.DeleteBehavior = DeleteBehavior.Restrict;

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entities = ChangeTracker.Entries<AuditableEntity>();

        foreach (var entityEntry in entities)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();

            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(c => c.CreatedById).CurrentValue = currentUserId!;
            }
            else if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(c => c.UpdatedById).CurrentValue = currentUserId;
                entityEntry.Property(c => c.UpdatedOn).CurrentValue = DateTime.UtcNow;
            }

        }

        return base.SaveChangesAsync(cancellationToken);
    }
}


