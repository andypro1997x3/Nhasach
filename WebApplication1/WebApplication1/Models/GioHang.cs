using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class GioHang
    {
        dbQLBanSachDataContext data = new dbQLBanSachDataContext();
        public int iMasach { get; set; }
        public string sTensach { get; set; }
        public string sAnhbia { get; set; }
        public Double dDonggia { get; set; }
        public int iSoluong { get; set; }
        public Double dThanhtien { get { return iSoluong * dDonggia; } }

        public GioHang(int Masach)
        {
            iMasach = Masach;
            SACH sach = data.SACHes.Single(n => n.Masach == iMasach);
            sTensach = sach.Tensach;
            sAnhbia = sach.Hinhminhhoa;
            dDonggia = double.Parse(sach.Dongia.ToString());
            iSoluong = 1;

        }
    }
}