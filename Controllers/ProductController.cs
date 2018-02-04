using Mobit.App_Classes;
using Mobit.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Mobit.Controllers
{
    public class ProductController : Controller
    {
        MobitDbEntities2 db = new MobitDbEntities2();
        Products pro = new Products();



        #region Listeleme işlemi !!!
        public ActionResult List()
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var data = db.Products.ToList();
            return View(data);
        }
        #endregion
        #region EKLEME İŞLEMİ !!!
        [HttpGet]
        public ActionResult Ekle()
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Ekle(Products urun)
        {
            if (urun.ImageFile != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(urun.ImageFile.FileName);
                string extension = Path.GetExtension(urun.ImageFile.FileName);
                fileName = fileName+ extension;
                urun.ImagePath = "~/Content/ProductImage/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Content/ProductImage/"), fileName);
                urun.ImageFile.SaveAs(fileName);
                using (MobitDbEntities2 db = new MobitDbEntities2())
                {
                    pro.ProductName = urun.ProductName;
                    pro.Description = urun.Description;
                    db.Products.Add(urun);
                    db.SaveChanges();
                }
            }
            ModelState.Clear();
            return RedirectToAction("List");

        }

        #endregion
        #region Silme İşlemi !!!
        public ActionResult Delete(int? id)
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            Products deleted = db.Products.Where(x => x.ProductId == id).FirstOrDefault();
            db.Products.Remove(deleted);
            db.SaveChanges();
            return RedirectToAction("List");
        }
        #endregion
        #region Guncelleme İşlemi!!!
        public ActionResult Update(int? id)
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products updated = db.Products.Find(id);
            if (updated == null)
            {
                return HttpNotFound();
            }
            return View(updated);
        }
        [HttpPost]
        public ActionResult Update(Products urun, int? id)
        {
            using (MobitDbEntities2 db = new MobitDbEntities2())
            {
                var a = db.Products.Find(urun.ProductId);
                if (urun.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(urun.ImageFile.FileName);
                    string extension = Path.GetExtension(urun.ImageFile.FileName);
                    fileName = fileName+extension;
                    urun.ImagePath = "~/Content/ProductImage/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Content/ProductImage/"), fileName);
                    urun.ImageFile.SaveAs(fileName);
                }
                db.Entry(a).CurrentValues.SetValues(urun);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
        #endregion




        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Login");
        }

    }
}
