using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gogol.Abstract;
using Gogol.Entities;

namespace Gogol.Models
{
    public class AdminListViewModel
    {
        public IBookRepository BookRepository { get; set; }
    }
}