using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gogol.Abstract;
using Gogol.Concrete;
using Gogol.Entities;
using Gogol.Models;

namespace Gogol.Controllers
{
    public class AdminController : Controller
    {
        private IBookRepository _bookRepository;

        public AdminController(IBookRepository repository)
        {
            _bookRepository = repository;
        }

        // GET: Admin   
        public ActionResult Index()
        {
            return View(new AdminListViewModel { BookRepository = _bookRepository });
        }

        public ViewResult Edit(int Id)
        {
            return View(_bookRepository.Books.First(r => r.Id == Id));
        }

        [HttpPost]
        public ActionResult Edit(BookDto book)
        {
            try
            {
                if (book.Id == 0)
                {
                    book.Id = null;
                }

                _bookRepository.SaveBook(book);
                TempData["message"] = string.Format("{0} has been saved", book.Name);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


        }

        [HttpPost]
        public ActionResult Delete(int bookId)
        {
            try
            {
                BookDto book = _bookRepository.DeleteBookById(bookId);
                TempData["message"] = string.Format("{0} has been removed", book.Name);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


        }

        public ViewResult Create()
        {
            return View("Edit", new BookDto());
        }

    }
}