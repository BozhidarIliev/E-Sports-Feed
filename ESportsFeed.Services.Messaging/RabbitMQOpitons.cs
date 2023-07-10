public class RabbitMQOptions
{
    public string UserName { get; set; }

    public string Password { get; set; }

    public string HostName { get; set; }

    public string VHostName { get; set; }

    public int Port { get; set; } = 5672;

    public string Scheme { get; set; } = "amqps";
}