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
    public class UserAdminController : Controller
    {
        QLBanSachEntities db = new QLBanSachEntities();
        // GET: Admin/UserAdmin
        [AdminAuthorize()]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(string TenDN, string Password)
        {
            if (String.IsNullOrEmpty(TenDN))
            {
                ViewData["err1"] = "Tên đăng nhập không được để trống";
            }
            else if (String.IsNullOrEmpty(Password))
            {
                ViewData["err1"] = "Mật khẩu không được để trống";
            }
            else
            {
                ADMIN kh = db.ADMINs.SingleOrDefault(item => item.TenDNAdmin == TenDN && item.MatKhauAdmin == Password);
                if (kh != null)
                {
                    ViewBag.thongbao = "Đăng nhập thành công";
                    Session["Admin"] = kh;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.thongbao = "Sai tên tk hoặc mật khẩu";
                }
            }
            return View();
        }
    }
}