using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gogol;
using Gogol.Abstract;
using Gogol.Concrete;
using Gogol.Controllers;
using Moq;
using Gogol.Entities;

namespace Gogol.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void CheckBooksIsNotEmpty()
        {
            throw new NotImplementedException();
        }
        
        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
           Mock<IBookRepository> repo = new Mock<IBookRepository>();

           AdminController target = new AdminController(repo.Object);

           ActionResult result = target.Edit(new BookDto {Name = "asda"});
        }

        [TestMethod]
        public void Can_Delete_Bool_ById()
        {
           Mock<IBookRepository> repo = new Mock<IBookRepository>();
           
           //TODO: Finish delete service...-
        }
    }
}
