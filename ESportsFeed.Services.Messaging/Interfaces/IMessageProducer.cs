namespace ESportsFeed.Services.Messaging.Interfaces
{
    public interface IMessageProducer
    {
        void PublishHiding<T>(T entity);
        void PublishChanges<T>(T entity);
    }
}