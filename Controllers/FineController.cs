using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class FineController : Controller
    {
        Library_ManagementEntities libentities = new Library_ManagementEntities();
        public ActionResult Index()
        {
            List<Fine> fine = libentities.Fines.ToList();
            return View(fine);
        }
        public ActionResult Index1()
        {
            List<Fine> fine = libentities.Fines.ToList();
            return View(fine);
        }

        public class OrderModel
        {
            public string orderId { get; set; }
            public string razorpayKey { get; set; }
            public int amount { get; set; }
            public string currency { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public int fineId { get; set; }
        }


        [HttpPost]
        public ActionResult CreateOrder(Fine _fndata)
        {
            Random randomObj = new Random();
            string transactionId = randomObj.Next(10000000, 100000000).ToString();
            Razorpay.Api.RazorpayClient payClient = new Razorpay.Api.RazorpayClient("rzp_test_0ISuvn0T3DxvYV", "8xvT8EUGg3p056yzibjM4BvK");
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", _fndata.FineAmount * 100);
            options.Add("receipt", transactionId);
            options.Add("currency", "INR");
            options.Add("payment_capture", "0");
            Razorpay.Api.Order orderResponse = payClient.Order.Create(options);
            string orderId = orderResponse["id"].ToString();

            OrderModel order = new OrderModel
            {
                orderId = orderResponse.Attributes["id"],
                razorpayKey = "rzp_test_0ISuvn0T3DxvYV",
                amount = (int)_fndata.FineAmount * 100,
                currency = "INR",
                name = _fndata.Username,
                email = _fndata.Email,
                fineId = _fndata.FineId
            };

            return View("Payment", order);
        }

        [HttpPost]
        public ActionResult Payment(int fineid)
        {
            string paymentId = Request.Params["rzp_paymentid"];
            string orderId = Request.Params["rzp_orderid"];
            Razorpay.Api.RazorpayClient payClient = new Razorpay.Api.RazorpayClient("rzp_test_0ISuvn0T3DxvYV", "8xvT8EUGg3p056yzibjM4BvK");
            Razorpay.Api.Payment payment = payClient.Payment.Fetch(paymentId);
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", payment.Attributes["amount"]);
            Razorpay.Api.Payment paymentCaptured = payment.Capture(options);
            string amt = paymentCaptured.Attributes["amount"];
            if (paymentCaptured.Attributes["status"] == "captured")
            {
                Fine fine = libentities.Fines.Find(fineid);
                fine.IsPaid = true;
                libentities.SaveChanges();
                return RedirectToAction("Success");
            }
            else
            {
                return RedirectToAction("Failure");
            }
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Failed()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            Fine fine = libentities.Fines.Find(id);
            return View(fine);
        }
    }
}