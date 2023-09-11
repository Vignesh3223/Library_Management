using Library.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Library.Controllers
{
    public class UserController : Controller
    {
        Library_ManagementEntities libentities = new Library_ManagementEntities();

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(User user)
        {
            Validate_User_Result roleUser = libentities.Validate_User(user.Email, user.Password).FirstOrDefault();
            string message = string.Empty;
            switch (roleUser.UserId.Value)
            {
                case -1:
                    message = "Email / Password is incorrect.";
                    break;
                case -2:
                    message = "Account has not been activated.";
                    break;
                default:
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, user.Email, DateTime.Now, DateTime.Now.AddMinutes(30), false, roleUser.Roles, FormsAuthentication.FormsCookiePath);
                    string hash = FormsAuthentication.Encrypt(ticket);
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);
                    if (ticket.IsPersistent)
                    {
                        cookie.Expires = ticket.Expiration;
                    }
                    Response.Cookies.Add(cookie);
                    return RedirectToAction("Index", "Home");
            }
            ViewBag.Message = message;
            TempData["userID"] = roleUser.UserId;
            return View(user);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(HttpPostedFileBase Avatar, [Bind(Include = "UserId,Username,RoleId,Password,Email")] User newUser)
        {
            if (ModelState.IsValid)
            {
                byte[] pic;
                using (var reader = new BinaryReader(Avatar.InputStream))
                {
                    pic = reader.ReadBytes(Avatar.ContentLength);
                }
                newUser.Avatar = pic;
                libentities.Users.Add(newUser);
                libentities.SaveChanges();
                return RedirectToAction("Login");
            }
            return View();
        }

        [Authorize(Roles = "Librarian")]
        public ActionResult Index()
        {
            List<User> user = libentities.Users.ToList();
            return View(user);
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            User user = libentities.Users.Find(id);
            return View(user);
        }

        public ActionResult Edit(int? id)
        {
            User user = libentities.Users.Find(id);
            List<Role> role = libentities.Roles.ToList();
            ViewBag.Roles = new SelectList(role, "RoleId", "RoleName");
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase Avatar, [Bind(Include = "UserId,Username,RoleId,Password,Email")] User user)
        {
            if (ModelState.IsValid)
            {
                byte[] edpic;
                using (var reader = new BinaryReader(Avatar.InputStream))
                {
                    edpic = reader.ReadBytes(Avatar.ContentLength);
                }
                user.Avatar = edpic;
                libentities.Entry(user).State = System.Data.Entity.EntityState.Modified;
                libentities.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult Delete(int? id)
        {
            User user = libentities.Users.Find(id);
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = libentities.Users.Find(id);
            libentities.Users.Remove(user);
            libentities.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}