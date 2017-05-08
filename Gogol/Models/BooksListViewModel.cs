using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gogol.Entities;

namespace Gogol.Models
{
    public class BooksListViewModel
    {
        public IEnumerable<BookDto> Books { get; set; }
    }
}