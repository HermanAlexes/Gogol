using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Gogol.Abstract;
using Gogol.Models;
using Gogol.Entities;
using System.IO;
using Newtonsoft.Json;

namespace Gogol.Controllers
{
    public class HomeController : Controller
    {
        private IBookRepository _repository;
        private IFilters _filters;

        public HomeController(IBookRepository bookRepository, IFilters filters)
        {
            _repository = bookRepository;
            _filters = filters;
        }

        public ActionResult Index()
        {
            BooksListViewModel model = new BooksListViewModel
            {
                // Books = _repository.Books
            };

            return View(model);
        }

        [HttpGet]
        public JsonResult GetBooks()
        {
            var books = _repository.Books;

            return Json(books, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFilteredBooks()
        {
            Stream req = Request.InputStream;
            req.Seek(0, SeekOrigin.Begin);
            string filersJson = new StreamReader(req).ReadToEnd();

            var filters = JsonConvert.DeserializeObject<FilterRequest>(filersJson);

            var books = _repository.GetFilteredBooks(filters);

            return Json(books, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAuthorFilters()
        {
            var filters = _filters.Authors;

            return Json(filters, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPublisherFilters()
        {
            var filters = _filters.Publishers;

            return Json(filters, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCategoryFilters()
        {
            var filters = _filters.Categories;

            return Json(filters, JsonRequestBehavior.AllowGet);
        }

    }
}