using Consumer.Api.Models;
using Consumer.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Producer.Api;
using System.Text;

namespace Consumer.Api.Controllers
{
    [Route("api/books")]
    [ApiController]
    [TypeFilter(typeof(ConsumerExceptionFilter))]
    public class BookConsumerController(IKafkaConsumerService consumerService, HttpClient httpClient) : ControllerBase
    {
        private static readonly Uri baseUrl = new Uri("https://localhost:7076/api/books-producer");

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            await httpClient.GetAsync(baseUrl); 
            var response = await consumerService.ConsumeAsync<IEnumerable<ConsumerBook>>("books-topic", default);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            await httpClient.GetAsync($"{baseUrl}/{id}");
            var response = await consumerService.ConsumeAsync<ConsumerBook>("books-topic", default);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(ConsumerBookDto bookDto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(bookDto), Encoding.UTF8, "application/json");
            await httpClient.PostAsync(baseUrl, content);
            var response = await consumerService.ConsumeAsync<ConsumerBook>("books-topic", default);

            return Ok(response);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateBook(int id, ConsumerBookDto bookDto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(bookDto), Encoding.UTF8, "application/json");
            await httpClient.PatchAsync($"{baseUrl}/{id}", content);
            var response = await consumerService.ConsumeAsync<ConsumerBook>("books-topic", default);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            await httpClient.DeleteAsync($"{baseUrl}/{id}");
            var response = await consumerService.ConsumeAsync<string>("books-topic", default);

            return Ok(response);
        }
    }
}
