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
    public class ChuDeController : Controller
    {
        // GET: Admin/ChuDe
        QLBanSachEntities db = new QLBanSachEntities();
        [AdminAuthorize()]
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 7;
            return View(db.CHUDEs.ToList().OrderBy(n => n.MaCD).ToPagedList(iPageNum, iPageSize));
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
            CHUDE cd = new CHUDE();
            cd.TenChuDe = f["sNameCd"];
            db.CHUDEs.Add(cd);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //Thêm chủ đề end


        //Sửa start
        public ActionResult Edit(int id)
        {
            CHUDE cd = db.CHUDEs.FirstOrDefault(item => item.MaCD == id);
            return View(cd);
        }
        [HttpPost]
        public ActionResult Edit(FormCollection f)
        {
            int id = int.Parse(f["iMaCD"]);
            CHUDE cd = db.CHUDEs.FirstOrDefault(item => item.MaCD == id);
            var NameCd = f["sNameCd"];
            if (NameCd == null)
            {
                ViewBag.ThongBao = "Tên chủ đề không được để trống";
                return View(cd);
            }
            cd.TenChuDe = NameCd;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //Sửa end

        public ActionResult Delete(int id)
        {
            CHUDE cd = db.CHUDEs.FirstOrDefault(item => item.MaCD == id);
            return View(cd);
        }
        [HttpPost]
        public ActionResult Delete(FormCollection f)
        {
            int id = int.Parse(f["iMaCD"]);
            CHUDE cd = db.CHUDEs.FirstOrDefault(item => item.MaCD == id);
            db.CHUDEs.Remove(cd);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Details(int id)
        {
            CHUDE cd = db.CHUDEs.FirstOrDefault(item => item.MaCD == id);
            return View(cd);
        }
    }
}