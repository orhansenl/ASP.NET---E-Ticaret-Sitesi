using PagedList;
using SENLStokTakip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SENLStokTakip.Controllers
{
    public class TumSatislarController : Controller
    {
        // GET: TumSatislar
        StokTakipSistemiEntities db = new StokTakipSistemiEntities();
        [Authorize(Roles = "A")]

        public ActionResult Index(int sayfa=1)
        {
            return View(db.Satislar.ToList().ToPagedList(sayfa,50));
        }
    }
}