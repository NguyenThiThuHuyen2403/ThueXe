using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SachOnline.Models
{
    public class GioHang
    {
        QLBanSachEntities db = new QLBanSachEntities();
        public int iMaXe { get; set; }
        public string sTenXe { get; set; }
        public string sAnhBia { get; set; }
        public int dDonGia { get; set; }
        public int iSoLuong { get; set; }
        public int iSoNgayThue { get; set; }
        public float dThanhTien
        {
            get { return iSoLuong * dDonGia * iSoNgayThue; }
        }
        public GioHang(int ms)
        {
            iMaXe = ms;
            XEDAP s = db.XEDAPs.Single(n => n.MaXe == iMaXe);
            sTenXe = s.TenXe;
            sAnhBia = s.HinhMinhHoa;
            dDonGia = (int)s.DonGia;
            iSoLuong = 1;
            iSoNgayThue = 1;
        }
    }
}