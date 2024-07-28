namespace Consumer.Api.Services
{
    public interface IKafkaConsumerService
    {
        Task<T?> ConsumeAsync<T>(string topic, CancellationToken cancellationToken);
    }
}
