using ESportsFeed.Data.Models;
using ESportsFeed.Services.Messaging;
using ESportsFeed.Services.Messaging.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Security.Principal;
using System.Text;

public class RabbitMQProducer : IMessageProducer, IDisposable
{
    private readonly ConnectionFactory connectionFactory;
    private readonly IConnection connection;
    private readonly IModel channel;
    private const string HidingQueueName = "hiding_queue";
    private const string MatchChangesQueueName = "match_changes_queue";
    private const string OddChangesQueueName = "odd_changes_queue";

    public RabbitMQProducer(ConnectionFactory connectionFactory)
    {
        this.connectionFactory = connectionFactory;
        connection = this.connectionFactory.CreateConnection();
        channel = connection.CreateModel();

        DeclareQueue(channel, HidingQueueName);
        DeclareQueue(channel, MatchChangesQueueName);
        DeclareQueue(channel, OddChangesQueueName);
    }

    private void DeclareQueue(IModel channel, string queueName)
    {
        channel.QueueDeclare(queue: queueName,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
        arguments: null);
    }

    public void PublishHiding<T>(T entity) 
    {
        var queueName = HidingQueueName;

        var hidingMessage = new HidingMessage
        {
            EntityId = (entity as IActiveEntity).ID,
            EntityType = entity.GetType().Name,
            DateTime = DateTime.Now
        };
        var jsonMessage = JsonConvert.SerializeObject(hidingMessage);
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
    }

    public void PublishChanges<T>(T entity)
    {
        var queueName = GetQueueNameForChangesMessage(entity);

        var message = GetChangesMessage(entity);
        var jsonMessage = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
    }

    private string GetQueueNameForChangesMessage<T>(T entity)
    {
        if (entity is Match)
            return MatchChangesQueueName;
        if (entity is Odd)
            return OddChangesQueueName;

        throw new ArgumentException("Invalid changes message entity type");
    }

    private object GetChangesMessage<T>(T entity)
    {
        if (entity is Match)
        {
            var match = entity as Match;
            var changesMessage = new MatchChangesMessage
            {
                MatchId = match.ID,
                MatchType = match.MatchType.ToString(),
                StartDate = match.StartDate
            };
            return changesMessage;
        }
        if (entity is Odd)
        {
            var odd = entity as Odd;
            var changesMessage = new OddChangesMessage
            {
                OddId = odd.ID,
                Value = odd.Value
            };
            return changesMessage;
        }

        throw new ArgumentException("Invalid changes message entity type");
    }
    public void Dispose()
    {
        channel?.Dispose();
        connection?.Dispose();
    }
}
