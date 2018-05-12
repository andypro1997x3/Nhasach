using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class GioHangController : Controller
    {
        dbQLBanSachDataContext data = new dbQLBanSachDataContext();

        public List<GioHang> Laygiohang()
        {
            List<GioHang> lstGiohang = Session["Giohang"] as List<GioHang>;
            if(lstGiohang==null)
            {
                lstGiohang = new List<GioHang>();
                Session["GioHang"] = lstGiohang;
            }
            return lstGiohang;
        }

        public ActionResult ThemGiohang(int iMasach,string strURL)
        {
            List<GioHang> lstGioHang = Laygiohang();
            GioHang sanpham = lstGioHang.Find(n => n.iMasach == iMasach);
            if(sanpham==null)
            {
                sanpham = new GioHang(iMasach);
                lstGioHang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }

        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if(lstGiohang!=null)
            {
                iTongSoLuong = lstGiohang.Sum(n => n.iSoluong);
            }
            return iTongSoLuong;
        }

        private double TongTien()
        {
            double iTongTien = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if(lstGiohang!=null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhtien);

            }
            return iTongTien;
        }

        public ActionResult GioHang()
        {
            List<GioHang> lstGiohang = Laygiohang();
            if(lstGiohang.Count==0)
            {
                return RedirectToAction("Index", "BookStore");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }
        // GET: GioHang
		public ActionResult GiohangPartial()
		{
			ViewBag.Tongsoluong = TongSoLuong();
			ViewBag.Tongtien = TongTien();
			return PartialView();
		}
		public ActionResult XoaGiohang(int iMaSp)
		{
			List<GioHang> lstGiohang = Laygiohang();
			GioHang sanpham = lstGiohang.SingleOrDefault(n => n.iMasach == iMaSp);
			if(sanpham != null)
			{
				lstGiohang.RemoveAll(n => n.iMasach == iMaSp);
				return RedirectToAction("GioHang");
			}
			if (lstGiohang.Count == 0)
			{
				return RedirectToAction("Index", "BookStore");
			}
			return RedirectToAction("GioHang");
		}
		public ActionResult CapnhatGiohang(int iMasp, FormCollection f )

		{
			List<GioHang> lstGiohang = Laygiohang();
			GioHang sanpham = lstGiohang.SingleOrDefault(n => n.iMasach == iMasp);
			if (sanpham != null)
			{
				sanpham.iSoluong = int.Parse(f["txtSoluong"].ToString());
			}
			return RedirectToAction("Giohang");
		}
		[HttpGet]
		public ActionResult DatHang()
		{
			if(Session["Taikhoan"] ==null || Session["Taikhoan"].ToString()=="")
			{
				return RedirectToAction("Dangnhap", "nguoidung");
			}
			if (Session ["Giohang"] == null)
			{
				return RedirectToAction("Index", "BookStore");
			}
			List<GioHang> lstGiohang = Laygiohang();
			ViewBag.Tongsoluong = TongSoLuong();
			ViewBag.Tongtien = TongTien();
			return View(lstGiohang);
		}
		[HttpPost]
		public ActionResult DatHang(FormCollection collection)
		{
			DONDATHANG ddh = new DONDATHANG();
			KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
			List<GioHang> gh = Laygiohang();
			ddh.MaKH = kh.MaKH;
			ddh.NgayDH = DateTime.Now;
			var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
			ddh.Ngaygiaohang = DateTime.Parse(ngaygiao);
			ddh.HTGiaohang = false;
			ddh.HTThanhtoan = false;
			data.DONDATHANGs.InsertOnSubmit(ddh);
			data.SubmitChanges();
			foreach (var item in gh)
			{
				CTDATHANG ctdh = new CTDATHANG();
				ctdh.SoDH = ddh.SoDH;
				ctdh.Masach = item.iMasach;
				ctdh.Soluong = item.iSoluong;
				ctdh.Dongia = (decimal)item.dDonggia;
				data.CTDATHANGs.InsertOnSubmit(ctdh);
			}
			data.SubmitChanges();
			Session["GioHang"] = null;
			return RedirectToAction("Xacnhandonhang", "GioHang");
		}
		public ActionResult Xacnhandonhang()
		{
			return View();
		}
		public ActionResult Index()
        {
            return View();
        }
    }
}