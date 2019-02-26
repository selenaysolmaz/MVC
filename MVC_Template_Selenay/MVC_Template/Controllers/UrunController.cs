using MVC_Template.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Template.Controllers
{
    public class UrunController : Controller
    {
        NorthwindContext ctx = new NorthwindContext();
        // GET: Urun
        public ActionResult Index()
        {
            List<Product> prd = ctx.Products.ToList();
            return View(prd);
        }

        public ActionResult UrunEkle()
        {
            ViewBag.Kategoriler = ctx.Categories.ToList();
            ViewBag.Tedarikciler = ctx.Suppliers.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult UrunEkle(Product prd)
        {
            ctx.Products.Add(prd);
            ctx.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult UrunSorSil(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            Product prd = ctx.Products.FirstOrDefault(x => x.ProductID == id);
            if (prd == null)
                return HttpNotFound();
            return View(prd);
        }
        [HttpPost]
        public ActionResult UrunSorSil(Product p)
        {
            Product prd = ctx.Products.FirstOrDefault(x => x.ProductID == p.ProductID);
            ctx.Products.Remove(prd);
            ctx.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult UrunSil(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            Product product = ctx.Products.Find(id);

            if (product == null)
                return HttpNotFound();

            ctx.Products.Remove(product);
            ctx.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Guncelle(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            Product product = ctx.Products.Find(id);

            if (product == null)
                return HttpNotFound();

            ViewBag.Kategoriler = ctx.Categories.ToList();
            ViewBag.Tedarikciler = ctx.Suppliers.ToList();

            return View(product);
        }
        [HttpPost]
        //silme işlemi için ayrı bir sayfaya yönlendirilir.
        //
        public ActionResult Guncelle(Product prd)
        {
            Product product = ctx.Products.Find(prd.ProductID);
            if (product == null)
                return HttpNotFound();
            product.ProductName = prd.ProductName;
            product.UnitPrice = prd.UnitPrice;
            product.UnitsInStock = prd.UnitsInStock;
            product.CategoryID = prd.CategoryID;
            product.SupplierID = prd.SupplierID;

            ctx.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}