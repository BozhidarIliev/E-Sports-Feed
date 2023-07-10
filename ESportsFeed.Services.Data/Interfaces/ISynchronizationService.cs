using ESportsFeed.Data.Common.Repositories;
using ESportsFeed.Data.Models;

namespace ESportsFeed.Services.Data.Interfaces
{
    public interface ISynchronizationService
    {
        Task SynchronizeEntities(List<Sport> sports);
    }
}