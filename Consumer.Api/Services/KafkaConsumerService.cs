using Confluent.Kafka;
using Consumer.Api.Models;
using Consumer.Api.Services;
using Newtonsoft.Json;

public class KafkaConsumerService : IKafkaConsumerService
{
    private readonly ILogger<KafkaConsumerService> _logger;
    private readonly IConsumer<Ignore, string> _consumer;

    public KafkaConsumerService(ILogger<KafkaConsumerService> logger, IConsumer<Ignore, string> consumer)
    {
        _logger = logger;
        _consumer = consumer;
    }

    public async Task<T?> ConsumeAsync<T>(string topic, CancellationToken cancellationToken)
    {
        _consumer.Subscribe(topic);

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var response = _consumer.Consume(TimeSpan.FromSeconds(5));

                if (response == null)
                    continue;

                var message = response.Message.Value;
                _consumer.Commit(response);

                _logger.LogInformation($"Received message at {response.Topic}, Offset: {response.Offset}: {message}");
                if (response.Message.Value.Contains("Error"))
                {
                    var errorResponse = JsonConvert.DeserializeObject<CustomConsumerException>(message);
                    throw errorResponse!;
                }

                var deserialized = JsonConvert.DeserializeObject<T>(message);
                return deserialized;
            }
            catch (ConsumeException ex)
            {
                _logger.LogError($"Error occurred: {ex.Error.Reason}");
                throw;
            }
        }

        return default;
    }
}