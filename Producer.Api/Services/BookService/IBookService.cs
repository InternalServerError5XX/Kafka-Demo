using Producer.Api.Models;

namespace Producer.Api.Services.BookService
{
    public interface IBookService
    {
        IEnumerable<Book> GetBooks();
        Book GetBook(int id);
        Book AddBook(BookDto bookDto);
        Book UpdateBook(int id, BookDto bookDto);
        bool DeleteBook(int id);
    }
}
