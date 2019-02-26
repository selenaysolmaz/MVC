using MVC_Template.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Template.Controllers
{
    public class CategoryController : Controller
    {
        NorthwindContext ctx = new NorthwindContext();

        // GET: Category
        public ActionResult Index()
        {
            List<Category> ktg = ctx.Categories.ToList();
            return View(ktg);
        }

        // GET
        public ActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Ekle([Bind(Include = "CategoryName, Description")] Category ktg, HttpPostedFileBase Picture)
        {
            if (Picture == null)
                return View();

            ktg.Picture = ConvertToBytes(Picture);

            if (ModelState.IsValid)
            {
                ctx.Categories.Add(ktg);
                ctx.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        // Category Picture nesnesi Database'de byte[] şeklinde tutulduğu için seçilen resmi byte[]'e çevrilmesini sağlayan method.
        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes(image.ContentLength);
            byte[] bytes = new byte[imageBytes.Length + 78];
            Array.Copy(imageBytes, 0, bytes, 78, imageBytes.Length);
            return bytes;
        }

        //sil için gete gerek yok çünkü ayrı bir sayfa göstermiyoruz.eger ayr sayfa acmak isteseydik get de yazardık.
        //sil için 3 farklı yontem kullanabiliriz
        //1-silme işlemi için ayrı bir sayfaya yönlendirilir. orada soru sorulur evet ise silinir. hayır ise silinmez.
        //2-sil butonuna basılınca browserın kendi ayar kutusu vardır o kullanılabilir. bu kutuda kayıt silinsinmi diye sorulur. tamam denilirse silinir. hayırsa silinmez. bu işlem asenkron bir işlemdir.buradaki sıkınıtı browswe tarafındaki alert kutusu 3-4 kez cıkınca bir daha gösterme checkbox ı eklenir ve check edilirse birdaha silme işlemi yapılamaz.
        //3-buda asenkron bir yontemdir.modal kutusu kullanılır. bizim calısmamozda templatin hazır modal kutularından birini sececeğiz.sil butınu tıklanınca silme için modal kutucuk acılır ve silme işlemş orda yapılır.
        //2. yontem için ajaks kulanmak gerek
        [HttpPost]
        public void Sil(int id)
        {
            Category k = ctx.Categories.FirstOrDefault(x => x.CategoryID == id);
            ctx.Categories.Remove(k);
            ctx.SaveChanges();
        }

        public ActionResult Guncelle(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            Category category = ctx.Categories.Find(id);

            if (category == null)
                return HttpNotFound();

            return View(category);
        }
        [HttpPost]
        public ActionResult Guncelle([Bind(Include = "CategoryID, CategoryName, Description")] Category ktg, HttpPostedFileBase Picture)
        {
            
            if (ModelState.IsValid)
            {

                Category k= ctx.Categories.Find(ktg.CategoryID);
                
                k.CategoryName = ktg.CategoryName;
                k.Description = ktg.Description;

                if (Picture != null)
                {
                    k.Picture = ConvertToBytes(Picture);
                }
               // ctx.Entry(k).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}