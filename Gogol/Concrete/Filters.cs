using Gogol.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gogol.Concrete
{
    public class Filters : IFilters
    {
        private GogolStoreEntities _entitiesContext;

        public IEnumerable<string> Authors
        {
            get
            {
                using (_entitiesContext = new GogolStoreEntities())
                {
                    return _entitiesContext.Authors.Select(x => x.Name).ToList();
                }
            }
        }

        public IEnumerable<string> Publishers
        {
            get
            {
                using (_entitiesContext = new GogolStoreEntities())
                {
                    return _entitiesContext.Publishers.Select(x => x.Name).ToList();
                }
            }
        }

        public IEnumerable<string> Categories
        {
            get
            {
                using (_entitiesContext = new GogolStoreEntities())
                {
                    return _entitiesContext.Categories.Select(x => x.Name).ToList();
                }
            }
        }
    }
}