using ESportsFeed.Web.DTOs;

namespace ESportsFeed.Services.Data.Interfaces
{
    public interface IMatchService
    {
        Task<MatchDetailsDTO> GetMatchById(string id);
        List<MatchDTO> GetMatchesStartingIn24Hours();
    }
}