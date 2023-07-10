using AutoMapper;
using ESportsFeed.Data.Common.Repositories;
using ESportsFeed.Data.Models;
using ESportsFeed.Services.Data.Interfaces;
using ESportsFeed.Web.DTOs;

namespace ESportsFeed.Services.Data
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository matchRepository;
        private readonly IMapper mapper;

        public MatchService(IMatchRepository matchRepository, IMapper mapper)
        {
            this.matchRepository = matchRepository;
            this.mapper = mapper;
        }

        public List<MatchDTO> GetMatchesStartingIn24Hours()
        {
            var next24Hours = DateTime.UtcNow.AddHours(24);

            var matches = matchRepository.All()
                .Where(m => m.StartDate <= next24Hours)
                .ToList();

            if (matches == null || matches.Count == 0)
            {
                throw new Exception("There are no matches starting in the next 24 hours");
            }

            var matchDTOs = matches.Select(m => new MatchDTO
            {
                Name = m.Name,
                StartDate = m.StartDate,
                ActivePreviewMarkets = GetActivePreviewMarkets(m.Markets)
            }).ToList();

            return matchDTOs;
        }

        public async Task<MatchDetailsDTO> GetMatchById(string id)
        {
            var match = await matchRepository.GetByIdAsync(id);

            if (match == null)
            {
                throw new ArgumentException("Match not found", nameof(id));
            }

            var activeMarketsDTO = match.Markets
                .Select(m => new MarketDTO
                {
                    MarketName = m.Name,
                    ActiveOdds = GetActiveOdds(m.Odds),
                    InactiveOdds = GetInactiveOdds(m.Odds),
                })
                .ToList();

            var inactiveMarkets = match.Markets.Where(x => x.IsActive == false).ToList();
            
            var inactiveMarketsDTO = mapper.Map<List<MarketDTO>>(inactiveMarkets);

            MatchDetailsDTO matchDetails = new MatchDetailsDTO
            {
                Name = match.Name,
                StartDate = match.StartDate,
                ActiveMarkets = activeMarketsDTO,
                InactiveMarkets = inactiveMarketsDTO
            };

            return matchDetails;
        }

        private List<PreviewMarketDTO> GetActivePreviewMarkets(List<Market> markets)
        {
            return markets
                .Where(b => IsPreviewMarket(b.Name))
                .Select(b => new PreviewMarketDTO
                {
                    Name = b.Name,
                    ActiveOdds = GetPreviewOdds(b.Odds),
                })
                .ToList();
        }

        private List<OddDTO> GetActiveOdds(List<Odd> odds)
        {
            var activeOdds = odds.Where(o => o.IsActive).ToList();

            return mapper.Map<List<OddDTO>>(activeOdds);
        }

        private List<OddDTO> GetInactiveOdds(List<Odd> odds)
        {
            var activeOdds = odds.Where(o => o.IsActive == false).ToList();

            return mapper.Map<List<OddDTO>>(activeOdds);
        }

        private List<OddDTO> GetPreviewOdds(List<Odd> odds)
        {
            var activeOdds = GetActiveOdds(odds);

            // If there is no SpecialBetValue, return all active odds.
            if (activeOdds.All(o => o.SpecialBetValue == 0))
            {
                return activeOdds;
            }

            // Group the active odds by SpecialBetValue and return only the first group.
            var groupedOdds = odds.GroupBy(o => o.SpecialBetValue);

            if (groupedOdds != null && groupedOdds.Count() != 0)
            {
                return mapper.Map<List<OddDTO>>(groupedOdds.FirstOrDefault().ToList());
            }

            return new List<OddDTO>();
        }

        private bool IsPreviewMarket(string betName)
        {
            return betName.Equals("Match Winner", StringComparison.OrdinalIgnoreCase)
                || betName.Equals("Map Advantage", StringComparison.OrdinalIgnoreCase)
                || betName.Equals("Total Maps Played", StringComparison.OrdinalIgnoreCase);
        }
    }
}
