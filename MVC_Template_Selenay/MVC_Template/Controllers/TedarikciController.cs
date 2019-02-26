using MVC_Template.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Template.Controllers
{
    public class TedarikciController : Controller
    {
        NorthwindContext ctx = new NorthwindContext();
        // GET: Tedarikci
        public ActionResult Index()
        {
            List<Supplier>ktg=ctx.Suppliers.ToList();
            return View(ktg);
        }

        public ActionResult TedarikciEkle()
        {

            return View();
        }
        [HttpPost]
        public ActionResult TedarikciEkle(Supplier ted)
        {
            ctx.Suppliers.Add(ted);
            ctx.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult Guncelle(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            Supplier supplier = ctx.Suppliers.Find(id);

            if (supplier == null)
                return HttpNotFound();

            return View(supplier);
        }
        [HttpPost]
        public ActionResult Guncelle(Supplier ted)
        {
            Supplier supplier = ctx.Suppliers.Find(ted.SupplierID);

            if (supplier == null)
                return HttpNotFound();

            supplier.CompanyName = ted.CompanyName;
            supplier.ContactName = ted.ContactName;
            supplier.ContactTitle = ted.ContactTitle;
            
            ctx.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public void Sil(int id)
        {
            Supplier s = ctx.Suppliers.FirstOrDefault(x => x.SupplierID == id);
            ctx.Suppliers.Remove(s);
            ctx.SaveChanges();
        }

    }
}