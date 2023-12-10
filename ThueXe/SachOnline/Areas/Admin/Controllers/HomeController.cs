using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SachOnline.Models;
using SachOnline.App_Start;

namespace SachOnline.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        QLBanSachEntities db = new QLBanSachEntities();
        // GET: Admin/Home
        [AdminAuthorize()]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            var User = f["Username"];
            var Password = f["Password"];
            ADMIN ad = db.ADMINs.FirstOrDefault(item => item.TenDNAdmin == User && item.MatKhauAdmin == Password);
            if (ad != null)
            {
                Session["Admin"] = ad;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
       
    }
}