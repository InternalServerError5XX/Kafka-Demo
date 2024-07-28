namespace Producer.Api.Services.KafkaProducerService
{
    public interface IKafkaProducerService
    {
        Task<string> SendMessageAsync<T>(string topic, T message, CancellationToken cancellationToken);
    }
}
