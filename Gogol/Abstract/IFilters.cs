using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gogol.Abstract
{
    public interface IFilters
    {
        IEnumerable<string> Authors { get; }
        IEnumerable<string> Publishers { get; }
        IEnumerable<string> Categories { get; }
    }
}
