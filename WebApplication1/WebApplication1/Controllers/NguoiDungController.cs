using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class NguoiDungController : Controller
    {
        // GET: NguoiDung
        dbQLBanSachDataContext db = new dbQLBanSachDataContext();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]

        public ActionResult DangKy(FormCollection collection,KHACHHANG kh)
        {
            var hoten = collection["HotenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var matkhaunhaplai = collection["Matkhaunhaplai"];
            var diachi = collection["Diachi"];
            var email = collection["Email"];
            var dienthoai = collection["Dienthoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);
            
            if(String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi 1"] = "Họ tên khách hàng không được để trống";
            }
            else if(String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi 2"] = "Phải nhập tên đăng nhập ";
            }
            else if(String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi 3"] = "Phải nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["Loi 4"] = "Phải nhập lại mật khẩu";
            }

            else if(String.IsNullOrEmpty(email))
            {
                ViewData["Loi 5"] = "Email không được bỏ trống";
            }
            else if(String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi 6"] = "Phải nhập điện thoại";
            }
            else
            {
                kh.HoTenKH = hoten;
                kh.TenDN = tendn;
                kh.Matkhau = matkhau;
                kh.DiachiKH = diachi;
                kh.Email = email;
                kh.Ngaysinh = DateTime.Parse(ngaysinh);
                kh.DienthoaiKH = dienthoai;

                
                db.KHACHHANGs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("Dangnhap");
                
            }
            return this.DangKy();
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            if (string.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }

            else if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.TenDN == tendn && n.Matkhau == matkhau);
                if (kh != null)
                {
					
                    Session["Taikhoan"] = kh;
					return RedirectToAction("Giohang", "GioHang");	
				}
                else ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu k đúng";
            }
            return View();
        }

    }
}