using Confluent.Kafka;
using Newtonsoft.Json;

namespace Producer.Api.Services.KafkaProducerService
{
    public class KafkaProducerService : IKafkaProducerService
    {
        private readonly IProducer<Null, string> _producer;
        private readonly ILogger<KafkaProducerService> _logger;

        public KafkaProducerService(ILogger<KafkaProducerService> logger, IProducer<Null, string> producer)
        {
            _logger = logger;
            _producer = producer;
        }

        public async Task<string> SendMessageAsync<T>(string topic, T message, CancellationToken cancellationToken)
        {
            try
            {
                var serializedMessage = JsonConvert.SerializeObject(message, Formatting.Indented);
                var kafkaMessage = new Message<Null, string>
                {
                    Value = serializedMessage
                };

                var response = await _producer.ProduceAsync(topic, kafkaMessage, cancellationToken);
                _logger.LogInformation($"Delivered message to Topic {response.Topic}, " +
                    $"Offset: {response.Offset} {response.Value}");

                return serializedMessage;
            }

            catch (ProduceException<Null, string> ex)
            {
                _logger.LogError($"Delivery failed: \nCode: {ex.Error.Code} \nError: {ex.Error.Reason}");
                return ex.Message;
            }
            finally
            {
                _producer.Flush(cancellationToken);
            }
        }
    }
}
