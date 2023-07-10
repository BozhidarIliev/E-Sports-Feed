using Microsoft.EntityFrameworkCore;
using ESportsFeed.Data.Common.Repositories;
using ESportsFeed.Data.Models;
using ESportsFeed.Services.Messaging.Interfaces;
using System.Security.Cryptography.X509Certificates;

namespace ESportsFeed.Data.Repositories
{
    public class MatchRepository : EfRepository<Match>, IMatchRepository
    {
        private readonly IMessageProducer messageProducer;

        public MatchRepository(ApplicationDbContext context, IMessageProducer messageProducer)
            : base(context, messageProducer)
        {
            this.messageProducer = messageProducer;
        }

        public override IQueryable<Match> All()
        {
            return this.DbSet.Include(m => m.Markets).ThenInclude(mk => mk.Odds);
        }

        public override async Task<Match> GetByIdAsync(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await DbSet.Include(x => x.Markets)
                                .ThenInclude(m => m.Odds)
                                .FirstOrDefaultAsync(x => x.ID == id);
        }
    }
}
