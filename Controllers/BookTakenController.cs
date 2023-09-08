using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    [Authorize]
    public class BookTakenController : Controller
    {
        Library_ManagementEntities libentities = new Library_ManagementEntities();

        public ActionResult Index()
        {
            List<Book_Taken> booktaken = libentities.Book_Taken.ToList();
            return View(booktaken);
        }

        public ActionResult Index1()
        {
            List<Book_Taken> booktaken = libentities.Book_Taken.ToList();
            return View(booktaken);
        }

        public ActionResult ReturnBook(int? id)
        {
            Book_Taken booktaken = libentities.Book_Taken.Find(id);
            return View(booktaken);
        }


        [HttpPost, ActionName("ReturnBook")]
        [ValidateAntiForgeryToken]
        public ActionResult ReturnBookConfirmed(int id)
        {
            Book_Taken booktaken = libentities.Book_Taken.Find(id);
            libentities.Book_Taken.Remove(booktaken);
            libentities.SaveChanges();
            return RedirectToAction("Index1");
        }
    }
}