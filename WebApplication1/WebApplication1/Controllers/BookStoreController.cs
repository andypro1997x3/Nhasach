using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

using PagedList.Mvc;
using PagedList;

namespace WebApplication1.Controllers
{
    public class BookStoreController : Controller
    {
        dbQLBanSachDataContext db = new dbQLBanSachDataContext();
        
        private List<SACH> Laysachmoi(int count)
        {
            return db.SACHes.OrderByDescending(x => x.Ngaycapnhat).Take(count).ToList();
            
        }
        // GET: BookStore
        public ActionResult Index(int ? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);


            var sachmoi = Laysachmoi(15);
            return View(sachmoi.ToPagedList(pageNum,pageSize));
        }
		[HttpGet]
        public ActionResult Chude()
        {
            var chude = from cd in db.CHUDEs select cd;
            return PartialView(chude);
        }

        public ActionResult Nhaxuatban()
        {
            var nhaxb = from cd in db.NHAXUATBANs select cd;
            return PartialView(nhaxb);
        }

        public ActionResult SPtheochude(int id)
        {
            var sach = from s in db.SACHes where s.MaCD == id select s;
            return View(sach);
        }

        public ActionResult SPtheoNXB(int id)
        {
            var sach = from s in db.SACHes where s.MaNXB == id select s;
            return View(sach);
        }

        public ActionResult Details(int id)
        {
            var sach = from s in db.SACHes where s.Masach == id select s;
            return View(sach.Single());
        }
    }
}