using SENLStokTakip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SENLStokTakip.Controllers
{
    public class IstatistikController : Controller
    {
        // GET: Istatistik
        StokTakipSistemiEntities db = new StokTakipSistemiEntities();

        public ActionResult Index()
        {
            var satis = db.Satislar.Count();
            ViewBag.satis=satis;
            var urun = db.Urun.Count();
            ViewBag.urun=urun;
            var kategori = db.Kategori.Count();
            ViewBag.kategori=kategori;
            var sepet = db.Sepet.Count();
            ViewBag.sepet=sepet;
            var kullanici = db.Kullanici.Count();
            ViewBag.kullanici=kullanici;
            return View();
        }
    }
}