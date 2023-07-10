using ESportsFeed.Data.Common.Repositories;
using ESportsFeed.Data.Models;
using ESportsFeed.Services.Data.Interfaces;
using ESportsFeed.Services.Messaging.Interfaces;

namespace ESportsFeed.Services.Data
{
    public class SynchronizationService : ISynchronizationService
    {
        private readonly ISportRepository sportRepository;
        private readonly IRepository<Event> eventRepository;
        private readonly IRepository<Match> matchRepository;
        private readonly IRepository<Market> marketRepository;
        private readonly IRepository<Odd> oddRepository;
        private readonly IMessageProducer messageProducer;

        public SynchronizationService(
            ISportRepository sportRepository,
            IRepository<Event> eventRepository,
            IRepository<Match> matchRepository,
            IRepository<Market> marketRepository,
            IRepository<Odd> oddRepository,
            IMessageProducer messageProducer)
        {
            this.sportRepository = sportRepository;
            this.eventRepository = eventRepository;
            this.matchRepository = matchRepository;
            this.marketRepository = marketRepository;
            this.oddRepository = oddRepository;

            this.messageProducer = messageProducer;
        }

        public async Task SynchronizeEntities(List<Sport> sports)
        {
            await SynchronizeSports(sports);

            await sportRepository.SaveChangesAsync();
        }

        public async Task SynchronizeSports(List<Sport> sports)
        {
            var existingSports = sportRepository.All().ToList();

            foreach (var sport in sports)
            {
                var existingSport = existingSports.FirstOrDefault(x => x.ID == sport.ID);
                if (existingSport == null)
                {
                    await sportRepository.AddAsync(sport);
                }
                else
                {
                    await SynchronizeEvents(sport.Events, existingSport.Events);
                    RemoveInactiveEntities(existingSport.Events, sport.Events, messageProducer);
                }
            }
        }

        private async Task SynchronizeEvents(List<Event> incomingEvents, List<Event> existingEvents)
        {
            foreach (var incomingEvent in incomingEvents)
            {
                var existingEvent = existingEvents.FirstOrDefault(e => e.ID == incomingEvent.ID);

                if (existingEvent == null)
                {
                    existingEvents.Add(incomingEvent);
                    await eventRepository.AddAsync(incomingEvent);
                }
                else
                {
                    await SynchronizeMatches(incomingEvent.Matches, existingEvent.Matches);
                    RemoveInactiveEntities(existingEvent.Matches, incomingEvent.Matches, messageProducer);
                }
            }
        }

        private async Task SynchronizeMatches(List<Match> incomingMatches, List<Match> existingMatches)
        {
            foreach (var incomingMatch in incomingMatches)
            {
                var existingMatch = existingMatches.FirstOrDefault(m => m.ID == incomingMatch.ID);

                if (existingMatch == null)
                {
                    existingMatches.Add(incomingMatch);
                    await matchRepository.AddAsync(incomingMatch);
                }
                else if (ShouldUpdateMatch(existingMatch, incomingMatch))
                {
                    // Update existing match with values from incoming match
                    existingMatch.MatchType = incomingMatch.MatchType;
                    existingMatch.StartDate = incomingMatch.StartDate;
                    matchRepository.Update(existingMatch);
                }
                else
                {
                    await SynchronizeMarkets(incomingMatch.Markets, existingMatch.Markets);
                    RemoveInactiveEntities(existingMatch.Markets, incomingMatch.Markets, messageProducer);
                }
            }
        }

        private async Task SynchronizeMarkets(List<Market> incomingMarkets, List<Market> existingMarkets)
        {
            foreach (var incomingMarket in incomingMarkets)
            {
                var existingMarket = existingMarkets.FirstOrDefault(m => m.ID == incomingMarket.ID);

                if (existingMarket == null)
                {
                    existingMarkets.Add(incomingMarket);
                    await marketRepository.AddAsync(incomingMarket);
                }
                else
                {
                    await SynchronizeOdds(incomingMarket.Odds, existingMarket.Odds);
                    RemoveInactiveEntities(existingMarket.Odds, incomingMarket.Odds, messageProducer);
                }
            }
        }

        private async Task SynchronizeOdds(List<Odd> incomingOdds, List<Odd> existingOdds)
        {
            foreach (var incomingOdd in incomingOdds)
            {
                var existingOdd = existingOdds.FirstOrDefault(o => o.ID == incomingOdd.ID);

                if (existingOdd == null)
                {
                    existingOdds.Add(incomingOdd);
                    await oddRepository.AddAsync(incomingOdd);
                }
                else if(ShouldUpdateOdd(existingOdd, incomingOdd))
                {
                    existingOdd.Value = incomingOdd.Value;
                    oddRepository.Update(existingOdd);
                }
            }
        }

        private void RemoveInactiveEntities<T>(List<T> existingEntities, List<T> incomingEntities, IMessageProducer messageProducer)
            where T : class, IActiveEntity
        {
            var inactiveEntities = existingEntities.Where(e => !incomingEntities.Any(ie => ie.ID == e.ID)).ToList();
            foreach (var entity in inactiveEntities)
            {
                entity.IsActive = false;
                messageProducer.PublishHiding(entity);
            }
        }

        private bool ShouldUpdateMatch(Match existingMatch, Match incomingMatch)
        {
            return existingMatch.MatchType != incomingMatch.MatchType || existingMatch.StartDate != incomingMatch.StartDate || existingMatch.IsActive == false;
        }

        private bool ShouldUpdateOdd(Odd existingOdd, Odd incomingOdd)
        {
            return existingOdd.Value != incomingOdd.Value || existingOdd.IsActive == false;
        }
    }
}