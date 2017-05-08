using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Gogol.Abstract;
using Gogol.Concrete;
using Moq;
using Ninject;


namespace Gogol.Infrastructure
{
    public class DependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public DependencyResolver(IKernel kernel)
        {
            this.kernel = kernel;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IBookRepository>().To<BooksRepository>();
            kernel.Bind<IFilters>().To<Filters>();
        }
    }
}