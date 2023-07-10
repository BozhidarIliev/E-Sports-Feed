using AutoMapper;
using ESportsFeed.Services.Data.Interfaces;
using ESportsFeed.Services.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Xml.Serialization;

namespace ESportsFeed.Services.Data
{
    public class FeedBackgroundService : BackgroundService
    {
        private readonly IConfiguration configuration;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly IMapper mapper;
        private readonly PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(120));
        private string xmlFeedUrl;


        public FeedBackgroundService(IConfiguration configuration, IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            this.configuration = configuration;
            this.scopeFactory = scopeFactory;
            this.mapper = mapper;
            xmlFeedUrl = configuration.GetSection("xmlFeedUrl").Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (await timer.WaitForNextTickAsync(stoppingToken)
                && !stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await FetchFeed();
                }
                catch (Exception e)
                {
                    
                    throw;
                }
            }
        }

        private async Task FetchFeed()
        {
            using var scope = scopeFactory.CreateScope();
            {
                var synchronizationService = scope.ServiceProvider.GetRequiredService<ISynchronizationService>();

                using (var httpClient = new HttpClient())
                {
                    var xmlData = await httpClient.GetStringAsync(xmlFeedUrl);

                    using (var stringReader = new StringReader(xmlData))
                    {
                        var serializer = new XmlSerializer(typeof(XmlSportsWrapper));
                        var xmlSports = (XmlSportsWrapper)serializer.Deserialize(stringReader);

                        var sports = mapper.Map<List<ESportsFeed.Data.Models.Sport>>(xmlSports.Sport);
                         
                        await synchronizationService.SynchronizeEntities(sports);
                    }
                }
            }
        }
    }
}