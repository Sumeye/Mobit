using Mobit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mobit.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public ActionResult Login(Admin item)
        {
            using (MobitDbEntities2 db = new MobitDbEntities2())
            {
                var find = (from a in db.Admin
                            where a.UserName.ToLower().Trim() == item.UserName.ToLower().Trim() &&
                            a.Password.ToLower().Trim() == item.Password.ToLower().Trim()
                            select a
                            ).FirstOrDefault();

                if (find != null)
                {
                   
                    Session["AdminId"] = find.AdminId;
                    Session["UserName"] = find.UserName;
                    
                    return RedirectToAction("List", "Product");
                }
                else
                {
                    return View(item);
                }
            }
        }

    }
}