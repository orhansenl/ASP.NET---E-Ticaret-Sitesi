using SENLStokTakip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SENLStokTakip.Controllers
{
    public class SepetController : Controller
    {
        // GET: Sepet
        StokTakipSistemiEntities db = new StokTakipSistemiEntities();

        public ActionResult Index(decimal?Tutar)
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var kullanici = db.Kullanici.FirstOrDefault(x => x.Email==kullaniciadi);
                var model = db.Sepet.Where(x => x.KullaniciID==kullanici.Id).ToList();
                var kid=db.Sepet.FirstOrDefault(x => x.KullaniciID==kullanici.Id);
                if (model!=null)
                {
                    if (kid==null)
                    {
                        ViewBag.Tutar="Sepetinizde ürün bulunmamaktadır.";
                    }
                    else if (kid!=null)
                    {
                        Tutar=db.Sepet.Where(x => x.KullaniciID==kid.KullaniciID).Sum(x => x.Urun.Fiyat*x.Adet);
                        ViewBag.Tutar="Toplam Tutar ="+ Tutar + "₺";
                        
                    }
                    return View(model);

                }
            }
            return HttpNotFound();
        }

        public ActionResult SepeteEkle(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var model = db.Kullanici.FirstOrDefault(x => x.Email==kullaniciadi);
                var u=db.Urun.Find(id);
                var sepet = db.Sepet.FirstOrDefault(x => x.KullaniciID == model.Id && x.UrunID==id);
                if (model!=null)
                {
                    if (sepet!=null)
                    {
                        sepet.Adet++;
                        sepet.Fiyat=u.Fiyat;
                        sepet.ToplamFiyat=u.Fiyat*sepet.Adet;
                        db.SaveChanges(); 
                        return RedirectToAction("Index");
                    }
                    var s = new Sepet
                    {
                        KullaniciID=model.Id,
                        UrunID=u.Id,
                        Adet=1,
                        Fiyat=u.Fiyat,
                        ToplamFiyat=u.Fiyat,
                        
                        Tarih=DateTime.Now

                    };
                    db.Sepet.Add(s);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                return View();
            }
            return HttpNotFound();
        }

        public ActionResult SepetCount(int?count)
        {
            if (User.Identity.IsAuthenticated)
            {
                var model = db.Kullanici.FirstOrDefault(x => x.Email==User.Identity.Name);
                count=db.Sepet.Where(x => x.KullaniciID==model.Id).Count();
                ViewBag.count=count;
                if (count==0)
                {
                    ViewBag.count="";
                }
                return PartialView();

            }
            return HttpNotFound();
        }

        public ActionResult arttir(int id)
        {
            var model = db.Sepet.Find(id);

            model.Adet++;
            model.ToplamFiyat=model.Adet*model.Fiyat;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult azalt(int id)
        {
            var model = db.Sepet.Find(id);
            if (model.Adet==1) 
            {
                db.Sepet.Remove(model);
                db.SaveChanges();
            }
            model.Adet--;
            model.ToplamFiyat=model.Adet*model.Fiyat;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public void AdetYaz(int id,int miktari)
        {
            var model = db.Sepet.Find(id);
            model.Adet=miktari;
            model.ToplamFiyat=model.Fiyat*model.Adet;
            db.SaveChanges();
        }

        public ActionResult Sil (int id)
        {
            var sil =db.Sepet.Find(id);
            db.Sepet.Remove(sil);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult HepsiniSil()
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var model = db.Kullanici.FirstOrDefault(x => x.Email==kullaniciadi);
                var sil = db.Sepet.Where(x => x.KullaniciID==model.Id);
                db.Sepet.RemoveRange(sil);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound();
            
        }
    }
}