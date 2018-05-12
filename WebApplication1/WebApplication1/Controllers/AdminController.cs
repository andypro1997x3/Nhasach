using System.Linq;
using System.Web.Mvc;
using WebApplication1.Models;
using PagedList.Mvc;
using PagedList;
using System.IO;
using System.Web;
using System.Data.Linq;

namespace WebApplication1.Controllers
{
    public class AdminController : Controller
    {
        // GET: Login
        dbQLBanSachDataContext db = new dbQLBanSachDataContext();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Sach(int ? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.SACHes.ToList().OrderBy(n=>n.Masach).ToPagedList(pageNumber,pageSize));
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];

            if(string.IsNullOrEmpty(tendn))
            { ViewData["loi1"] = "Phai nhap ten dang nhap"; }

            else if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["loi2"] = "Phai nhap mat khau";
            }
            else
            {
                Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    Session["admin"] = ad;
                    return RedirectToAction("Index", "Admin");

                }
                else ViewBag.Thongbao = "Ten dang nhap hoac mat khau khong dung";
            }
            return View();
        }
        [HttpGet]
        public ActionResult ThemmoiSach()
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "Tenchude");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiSach(SACH sach,HttpPostedFileBase fileupload)
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "Tenchude");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");

            //var fileName = Path.GetFileName(fileupload.FileName);
            //var path = Path.Combine(Server.MapPath("~/img"), fileName);
            if(fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }


            else
            {
                if(ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);

                    var path = Path.Combine(Server.MapPath("~/img"), fileName);

                    if(System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    sach.Hinhminhhoa = fileName;

                    db.SACHes.InsertOnSubmit(sach);
                    db.SubmitChanges();
                }
            }
            //if(System.IO.File.Exists(path))
            //{
            //    ViewBag.Thongbao = "Hình ảnh đã tồn tại";
            //}
            //else
            //{
            //    fileupload.SaveAs(path);
            //}

           
            return RedirectToAction("Sach");
        }
        

        public ActionResult Chitietsach(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if(sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        [HttpGet]
        public ActionResult Xoasach(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if(sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        [HttpPost,ActionName("Xoasach")]

        public ActionResult Xacnhanxoa(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if(sach==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.SACHes.DeleteOnSubmit(sach);
            db.SubmitChanges();
            return RedirectToAction("Sach");
        }
        [HttpGet]
        public ActionResult Suasach(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            ViewBag.Mota = sach.Mota;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "Tenchude");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");

            return View(sach);
        }

        [HttpPost]

        [ValidateInput(false)]

        public ActionResult Suasach(SACH sach, HttpPostedFileBase fileupload)
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "Tenchude");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            SACH s = db.SACHes.ToList().Find(n => n.Masach == sach.Masach);
            //var fileName = Path.GetFileName(fileupload.FileName);
            //var path = Path.Combine(Server.MapPath("~/img"), fileName);
            if (ModelState.IsValid)
            {
                if (fileupload != null)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/img"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "hinh da ton tai";

                    }
                    else
                    {
                        fileupload.SaveAs(path);
                        s.Hinhminhhoa = fileName;
                    }
                }
                s.Masach = sach.Masach;
                s.Tensach = sach.Tensach;
                s.Donvitinh = sach.Donvitinh;
                s.Dongia = sach.Dongia;
                s.Mota = sach.Mota;
                s.MaCD = sach.MaCD;
                s.MaNXB = sach.MaNXB;
                s.Ngaycapnhat = sach.Ngaycapnhat;
                s.Soluongban = sach.Soluongban;
                s.solanxem = sach.solanxem;
                s.moi = sach.moi;
                db.SubmitChanges();
            }



            return RedirectToAction("Sach");
        

        }
		public ActionResult NXB(int? page)
		{
			int pageNumber = (page ?? 1);
			int pageSize = 7;
			return View(db.NHAXUATBANs.ToList().OrderBy(n => n.MaNXB).ToPagedList(pageNumber, pageSize));
		}
		public ActionResult Sanpham(int? page)
		{
			int pageNumber = (page ?? 1);
			int pageSize = 7;
			return View(db.CHUDEs.ToList().OrderBy(n => n.MaCD).ToPagedList(pageNumber, pageSize));
		}
	}
}