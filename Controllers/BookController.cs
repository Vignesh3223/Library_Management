using Library.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class BookController : Controller
    {
        Library_ManagementEntities libentities = new Library_ManagementEntities();

        public ActionResult BookGenres()
        {
            List<Book_Genre> bookgenre = libentities.Book_Genre.ToList();
            return View(bookgenre);
        }

        public ActionResult Index(int? GenreId)
        {
            List<Book> book = libentities.Books.Where(bk => bk.GenreId == GenreId).ToList();
            return View(book);
        }
        public ActionResult Index1(int? GenreId)
        {
            List<Book> book = libentities.Books.Where(bk => bk.GenreId == GenreId).ToList();
            return View(book);
        }
        public ActionResult Create()
        {
            List<Book_Genre> genre = libentities.Book_Genre.ToList();
            ViewBag.Book_Genre = new SelectList(genre, "GenreId", "Genre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase Picture, [Bind(Include = "BookName,Author,GenreId")] Book book)
        {
            if (ModelState.IsValid)
            {
                byte[] cover;

                using (var reader = new BinaryReader(Picture.InputStream))
                {
                    cover = reader.ReadBytes(Picture.ContentLength);
                }
                book.Picture = cover;
                libentities.Books.Add(book);
                libentities.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            Book book = libentities.Books.Find(id);
            return View(book);
        }

        public ActionResult Edit(int? id)
        {
            Book book = libentities.Books.Find(id);
            List<Book_Genre> genre = libentities.Book_Genre.ToList();
            ViewBag.Book_Genre = new SelectList(genre, "GenreId", "Genre");
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase Picture, [Bind(Include = "BookId,BookName,Author,GenreId")] Book book)
        {
            if (ModelState.IsValid)
            {
                byte[] cover;

                using (var reader = new BinaryReader(Picture.InputStream))
                {
                    cover = reader.ReadBytes(Picture.ContentLength);
                }
                book.Picture = cover;
                libentities.Entry(book).State = System.Data.Entity.EntityState.Modified;
                libentities.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Delete(int? id)
        {
            Book book = libentities.Books.Find(id);
            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = libentities.Books.Find(id);
            libentities.Books.Remove(book);
            libentities.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult TakeBook(int? id)
        {
            Book book = libentities.Books.Find(id);
            TempData["BookID"] = book.BookId;
            TempData["Bookname"] = book.BookName;
            TempData["Picture"] = book.Picture;
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TakeBook( [Bind(Include = "TakeId,UserId,Username,BookId,BookName,TakenDate,ReturnDate")] Book_Taken booktaken )
        {
            int? userId = TempData["userID"] as int?;
            int? BookId = TempData["BookID"] as int?;
            string Bookname = TempData["Bookname"] as string;
            byte[] Pic = TempData["Picture"] as byte[];

            if (ModelState.IsValid)
            {
                booktaken.UserId = userId;
                booktaken.Username = User.Identity.Name;
                booktaken.BookId = BookId;
                booktaken.BookName = Bookname;
                booktaken.TakenDate = DateTime.Now;
                booktaken.ReturnDate = DateTime.Now.AddDays(12);
                booktaken.Picture = Pic;
                libentities.Book_Taken.Add(booktaken);
                libentities.SaveChanges();
                return RedirectToAction("Index1", "BookTaken");
            }
            return View();
        }
    }
}