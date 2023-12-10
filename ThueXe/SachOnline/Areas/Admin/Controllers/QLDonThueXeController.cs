using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using SachOnline.Models;
using SachOnline.App_Start;

namespace SachOnline.Areas.Admin.Controllers
{
    public class QLDonThueXeController : Controller
    {
        // GET: Admin/QLDonThueXe
        QLBanSachEntities db = new QLBanSachEntities();
        [AdminAuthorize()]
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 7;
            return View(db.DONDATHANGs.ToList().OrderBy(n => n.SoDH).ToPagedList(iPageNum, iPageSize));
        }
        [AdminAuthorize()]
        //Thêm chủ đề start

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection f)
        {
            DONDATHANG cd = new DONDATHANG();
            cd.SoDH = int.Parse(f["sSoDon"]);
            cd.MaKH = int.Parse(f["sMaKH"]);
            cd.NgayDH = DateTime.Parse(f["sNgayDat"]);
            cd.TriGia = int.Parse(f["sTriGia"]);
            cd.NgayGiaoHang = DateTime.Parse(f["sNgayMuonXe"]);
            cd.TenNguoiNhan = f["sTenKH"];
            cd.DiaChiNhan = f["sDiaChi"];
            cd.DienThoaiNhan = f["sSDT"];
            cd.HTThanhToan = f["sHinhThucThanhToan"];
            cd.SoNgayThue = int.Parse(f["SoNgayThue"]);
            cd.SoTienPhat = int.Parse(f["SoTienPhat"]);
            cd.TrangThai = bool.Parse(f["TrangThai"]);
            cd.NgayTraXe = DateTime.Parse(f["NgayTraXe"]);
            db.DONDATHANGs.Add(cd);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //Thêm chủ đề end


        //Sửa start
        public ActionResult Edit(int id)
        {
            DONDATHANG cd = db.DONDATHANGs.FirstOrDefault(item => item.SoDH == id);
            return View(cd);
        }
        [HttpPost]
        public ActionResult Edit(DONDATHANG cd1)
        {
            
            DONDATHANG cd = db.DONDATHANGs.FirstOrDefault(item => item.SoDH == cd1.SoDH);
       
            cd.MaKH = cd1.MaKH;
            cd.NgayDH = cd1.NgayDH;
            cd.TriGia = cd1.TriGia;
            cd.NgayGiaoHang = cd1.NgayGiaoHang;
            cd.TenNguoiNhan = cd1.TenNguoiNhan;
            cd.DiaChiNhan = cd1.DiaChiNhan;
            cd.DienThoaiNhan = cd1.DienThoaiNhan;
            cd.HTThanhToan = cd1.HTThanhToan;
            cd.SoNgayThue = cd1.SoNgayThue;
            cd.SoTienPhat = cd1.SoTienPhat;
            cd.TrangThai = cd1.TrangThai;
            cd.NgayTraXe = cd1.NgayTraXe;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //Sửa end

        public ActionResult Delete(int id)
        {
            DONDATHANG cd = db.DONDATHANGs.FirstOrDefault(item => item.SoDH == id);
            return View(cd);
        }
        [HttpPost]
        public ActionResult Delete(FormCollection f)
        {
            int id = int.Parse(f["iSoDon"]);
            DONDATHANG cd = db.DONDATHANGs.FirstOrDefault(item => item.SoDH == id);
          
           
            db.DONDATHANGs.Remove(cd);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Details(int id)
        {
            DONDATHANG cd = db.DONDATHANGs.FirstOrDefault(item => item.SoDH == id);
            return View(cd);
        }
    }
}