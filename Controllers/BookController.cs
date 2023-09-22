using Library.Models;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace Library.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        Library_ManagementEntities1 libentities = new Library_ManagementEntities1();

        public ActionResult BookGenres(int? c)
        {
            List<Book_Genre> bookgenre = libentities.Book_Genre.ToList();
            return View(libentities.Book_Genre.ToList().ToPagedList(c ?? 1,6));
        }

        //[Route("Index/{GenreId}")]
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
            ViewBag.ReturnUrl = Request.UrlReferrer;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase Picture, [Bind(Include = "BookName,Author,GenreId,Available")] Book book)
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
            ViewBag.ReturnUrl = Request.UrlReferrer;
            return View(book);
        }

        public ActionResult Edit(int? id)
        {
            Book book = libentities.Books.Find(id);
            List<Book_Genre> genre = libentities.Book_Genre.ToList();
            ViewBag.Book_Genre = new SelectList(genre, "GenreId", "Genre");
            ViewBag.ReturnUrl = Request.UrlReferrer;
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase Picture, [Bind(Include = "BookId,BookName,Author,GenreId,Available")] Book book)
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
            ViewBag.ReturnUrl = Request.UrlReferrer;
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
            TempData["userid"] = Session["userID"];
            ViewBag.ReturnUrl = Request.UrlReferrer;
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TakeBook( [Bind(Include = "TakeId,UserId,Username,Email,BookId,BookName,TakenDate,ReturnDate,IsReturned")] Book_Taken booktaken )
        {
            int? userId = TempData["userid"] as int?;
            string username = Session["name"] as string;
            string email = Session["email"] as string;
            int? BookId = TempData["BookID"] as int?;
            string Bookname = TempData["Bookname"] as string;
            byte[] Pic = TempData["Picture"] as byte[];

            if (ModelState.IsValid)
            {
                booktaken.UserId = userId;
                booktaken.Username = username;
                booktaken.Email = email;
                booktaken.BookId = BookId;
                booktaken.BookName = Bookname;
                booktaken.TakenDate = DateTime.Now;
                booktaken.ReturnDate = DateTime.Now.AddDays(14);
                booktaken.Picture = Pic;
                booktaken.IsReturned = false;
                libentities.Book_Taken.Add(booktaken);
                libentities.SaveChanges();
                return RedirectToAction("Index1", "BookTaken");
            }
            return View();
        }

        public class SearchResult
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Available { get; set; }
            public string Author { get; set; }
            public byte[] Image { get; set; }
        }

        public ActionResult SearchBook(string name)
        {
            var searchResults = new List<Object>();

            var booksAndAuthors = libentities.Books
                .Where(b => b.BookName.Contains(name) || b.Author.Contains(name))
                .Select(b => new { Id = b.BookId, Name = b.BookName, Available = b.Available, Author = b.Author, Image = b.Picture }).ToList();

            searchResults.AddRange(booksAndAuthors);

            if (searchResults.Any())
            {
                if (User.IsInRole("Librarian"))
                {
                    return View("Search1", searchResults);
                }
                else
                {
                    return View("Search2", searchResults);
                }
            }
            else
            {
                return RedirectToAction("BookGenres");
            }
        }

        public ActionResult Search1()
        {
            return View();
        }

        public ActionResult Search2()
        {
            return View();
        }
    }
}