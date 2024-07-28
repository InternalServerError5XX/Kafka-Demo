using Newtonsoft.Json;

namespace Consumer.Api.Models
{
    public class CustomConsumerException : Exception
    {
        [JsonProperty("error")]
        public string CustomMessage { get; }

        [JsonProperty("stackTrace")]
        public string CustomStackTrace { get; }

        public CustomConsumerException(string customMessage, string customStackTrace) : base(customMessage)
        {
            CustomMessage = customMessage;
            CustomStackTrace = customStackTrace;
        }
    }
}
