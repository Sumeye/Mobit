using Mobit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Mobit.Controllers
{
    public class HomeController : Controller
    {
        MobitDbEntities2 db = new MobitDbEntities2();
        // GET: Home
        public ActionResult Index()
        {
            var data = db.Products.ToList();
            return View(data);
        }

        public ActionResult Details(int? id)
        {
            Products data = db.Products.Where(x => x.ProductId == id).FirstOrDefault();
            db.Products.ToList();
            return View(data);
        }
        [HttpPost]
        public ActionResult Details(string receiverEmail, string message, int? id,string subject)
        {
            Products data = db.Products.Where(x => x.ProductId == id).FirstOrDefault();
            var baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("deneme987654123@gmail.com", "URL");
                    var receivereEmail = new MailAddress(receiverEmail, "Receiver");
                    var password = "deneme987654";
                    var body = message;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receivereEmail)
                    {
                        Body =  message + Environment.NewLine + baseUrl + "/Home/Details/" + data.ProductId,
                        Subject = subject,
                    })
                    {
                        smtp.Send(mess);
                    }
                    return new RedirectResult("~/Home/Details/"+data.ProductId);

                }
            }
            catch (Exception)
            {

                ViewBag.Error = "Mail Gönderilirken bir problem yaşandı";
            }
            return new RedirectResult("~/Home/Details/" + data.ProductId);

        }


    }
}