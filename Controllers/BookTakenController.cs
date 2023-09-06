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
    }
}