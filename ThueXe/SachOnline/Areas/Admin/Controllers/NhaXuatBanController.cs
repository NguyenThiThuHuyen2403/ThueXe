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
    public class NhaXuatBanController : Controller
    {
        // GET: Admin/NhaXuatBan
        QLBanSachEntities db = new QLBanSachEntities();
        [AdminAuthorize()]
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 7;
            return View(db.NHAXUATBANs.ToList().OrderBy(n => n.MaNXB).ToPagedList(iPageNum, iPageSize));
        }
        [AdminAuthorize()]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection f)
        {
            var TenNxb = f["sNameNxb"];
            var DiaChi = f["sDiaChi"];
            var DienThoai = f["sDienThoai"];
            NHAXUATBAN nbx = new NHAXUATBAN();

            nbx.TenNXB = TenNxb;
            nbx.DiaChi = DiaChi;
            nbx.DienThoai = DienThoai;
            db.NHAXUATBANs.Add(nbx);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            NHAXUATBAN nbx = db.NHAXUATBANs.FirstOrDefault(item => item.MaNXB == id);
            return View(nbx);
        }
        [HttpPost]
        public ActionResult Edit(FormCollection f)
        {
            int id = int.Parse(f["iMaNXB"]);
            var TenNxb = f["sNameNxb"];
            var DiaChi = f["sDiaChi"];
            var DienThoai = f["sDienThoai"];
            NHAXUATBAN nxb = db.NHAXUATBANs.FirstOrDefault(item => item.MaNXB == id);
            nxb.TenNXB = TenNxb;
            nxb.DiaChi = DiaChi;
            nxb.DienThoai = DienThoai;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            NHAXUATBAN nbx = db.NHAXUATBANs.FirstOrDefault(item => item.MaNXB == id);
            return View(nbx);
        }

        public ActionResult Delete(int id)
        {
            NHAXUATBAN nbx = db.NHAXUATBANs.FirstOrDefault(item => item.MaNXB == id);
            //db.NHAXUATBANs.Remove(nbx);
            return View(nbx);
        }
        [HttpPost]
        public ActionResult Delete(FormCollection f)
        {
            int id = int.Parse(f["iMaNXB"]);
            NHAXUATBAN nbx = db.NHAXUATBANs.FirstOrDefault(item => item.MaNXB == id);
            db.NHAXUATBANs.Remove(nbx);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}