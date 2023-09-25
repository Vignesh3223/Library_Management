using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using Library.Models;
using System.IO;

namespace Library.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Mailer(string Name,string Email)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("VIGNESH","20bsca150vigneshr@skacas.ac.in"));
            message.To.Add(new MailboxAddress(Name, Email));
            message.Subject = "FEEDBACK";
            var body = new BodyBuilder { TextBody = "Thank you for your feedback...." };
            message.Body = body.ToMessageBody();
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("20bsca150vigneshr@skacas.ac.in", "welcome123");
                client.Send(message);
                client.Disconnect(true);
            } 
            return RedirectToAction("Index");
        }
    }
}