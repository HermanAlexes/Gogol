using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gogol.Entities;

namespace Gogol.Abstract
{
    public interface IBookRepository
    {
        IEnumerable<BookDto> Books { get; }

        void SaveBook(BookDto book);

        BookDto DeleteBookById(int bookId);

        IEnumerable<BookDto> GetFilteredBooks(FilterRequest filters);
    }
}
