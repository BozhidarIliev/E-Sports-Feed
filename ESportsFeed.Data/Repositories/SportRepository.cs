using Microsoft.EntityFrameworkCore;
using ESportsFeed.Data.Common.Repositories;
using ESportsFeed.Data.Models;
using ESportsFeed.Services.Messaging.Interfaces;

namespace ESportsFeed.Data.Repositories
{
    public class SportRepository : EfRepository<Sport>, ISportRepository
    {
        private readonly IMessageProducer messageProducer;

        public SportRepository(ApplicationDbContext context, IMessageProducer messageProducer)
            : base(context, messageProducer)
        {
            this.messageProducer = messageProducer;
        }

        public override IQueryable<Sport> All()
        {
            return this.DbSet.Include(s => s.Events)
                                .ThenInclude(e => e.Matches)
                                    .ThenInclude(m => m.Markets)
                                        .ThenInclude(mk => mk.Odds);
        }

        public override Task<int> SaveChangesAsync()
        {
            HandleModifiedEntities();

            return base.SaveChangesAsync();
        }

        private void HandleModifiedEntities()
        {
            var modifiedEntities = Context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified && e.Entity is IActiveEntity)
                .ToList();

            foreach (var entityEntry in modifiedEntities)
            {
                HandleModifiedEntity(entityEntry.Entity as IActiveEntity);
            }
        }

        private void HandleModifiedEntity(IActiveEntity entity)
        {
            if (entity is Match match)
            {
                HandleModifiedMatch(match);
            }
            else if (entity is Odd odd)
            {
                HandleModifiedOdd(odd);
            }
            // Add handling for other entity types if needed
        }

        private void HandleModifiedMatch(Match match)
        {
            var originalMatch = Context.Entry(match).OriginalValues.ToObject() as Match;
            if (originalMatch != null && (match.StartDate != originalMatch.StartDate || match.MatchType != originalMatch.MatchType))
            {
                messageProducer.PublishChanges(match);
            }
        }

        private void HandleModifiedOdd(Odd odd)
        {
            var originalOdd = Context.Entry(odd).OriginalValues.ToObject() as Odd;
            if (originalOdd != null && odd.Value != originalOdd.Value)
            {
                messageProducer.PublishChanges(odd);
            }
        }
    }
}
