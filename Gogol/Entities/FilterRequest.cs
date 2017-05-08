using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gogol.Entities
{
    public class FilterRequest
    {
        public List<string> authors { get; set; }
        public List<string> publishers { get; set; }
        public List<string> categories { get; set; }
    }
}