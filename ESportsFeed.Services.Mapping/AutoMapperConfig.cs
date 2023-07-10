using AutoMapper;
using ESportsFeed.Data.Models;
using ESportsFeed.Services.Models;
using ESportsFeed.Web.DTOs;

namespace ESportsFeed.Services.Mapping
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
                // Add your mapping configurations here
                CreateMap<SportModel, Sport>();
                CreateMap<EventModel, Event>();
                CreateMap<MatchModel, Match>();
                CreateMap<MarketModel, Market>();
                CreateMap<OddModel, Odd>();
                CreateMap<MatchType, MatchTypeModel>();
                CreateMap<Odd, OddDTO>();

            CreateMap<Match, MatchDetailsDTO>();
            CreateMap<Market, MarketDTO>();
        }
    }
}