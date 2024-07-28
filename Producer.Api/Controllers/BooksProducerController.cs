using Microsoft.AspNetCore.Mvc;
using Producer.Api.Models;
using Producer.Api.Services.BookService;
using Producer.Api.Services.KafkaProducerService;

namespace Producer.Api.Controllers
{
    [Route("api/books-producer")]
    [ApiController]
    [TypeFilter(typeof(ProducerExceptionFilter))]
    public class BooksProducerController(IBookService bookService, IKafkaProducerService producerService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var response = bookService.GetBooks();
            var message = await producerService.SendMessageAsync("books-topic", response, default);

            return Ok(message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var response = bookService.GetBook(id);
            var message = await producerService.SendMessageAsync("books-topic", response, default);

            return Ok(message);
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(BookDto bookDto)
        {
            var response = bookService.AddBook(bookDto);
            var message = await producerService.SendMessageAsync("books-topic", response, default);

            return Ok(message);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateBook(int id, BookDto bookDto)
        {
            var response = bookService.UpdateBook(id, bookDto);
            var message = await producerService.SendMessageAsync("books-topic", response, default);

            return Ok(message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var response = bookService.DeleteBook(id);
            var message = await producerService.SendMessageAsync("books-topic", response, default);

            return Ok(message);
        }
    }
}
