using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SachOnline.Models;
using PagedList;
using PagedList.Mvc;

namespace SachOnline.Controllers
{
    public class SachOnlineController : Controller
    {
        // GET: SachOnline
        QLBanSachEntities db = new QLBanSachEntities();

        /// <summary> /// LaySachMoi /// </summary>
        // <param name="count">int</param>
        /// <returns>List</returns>
        
        public ActionResult Index(string KeyWork, int page = 1, int pageSize = 6)
        {
            // Tổng số lượng sách
            int totalBooks = db.XEDAPs.Count();

            // Tính tổng số trang dựa trên số lượng sách và kích thước trang
            int totalPages = (int)Math.Ceiling((double)totalBooks / pageSize);

            // Đảm bảo rằng trang hiện tại nằm trong phạm vi hợp lệ
            page = Math.Max(1, Math.Min(totalPages, page));

            if(!string.IsNullOrEmpty(KeyWork))
            {
                var Danhsach1 = db.XEDAPs
               .OrderByDescending(item => item.NgayCapNhat)
               .Where(item => item.TenXe.Contains(KeyWork))
               .Skip((page - 1) * pageSize)
               .Take(pageSize)
               .ToList();


                int totalBooks1 = Danhsach1.Count();

                // Tính tổng số trang dựa trên số lượng sách và kích thước trang
                int totalPages1 = (int)Math.Ceiling((double)totalBooks1 / pageSize);

                // Đảm bảo rằng trang hiện tại nằm trong phạm vi hợp lệ
                page = Math.Max(1, Math.Min(totalPages1, page));
                // Đưa thông tin về phân trang vào ViewBag để sử dụng trong View
                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages1;

                return View(Danhsach1);
            }    
            // Lấy danh sách sách cho trang hiện tại
            var Danhsach = db.XEDAPs
                .OrderByDescending(item => item.NgayCapNhat)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Đưa thông tin về phân trang vào ViewBag để sử dụng trong View
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(Danhsach);
        }
        public ActionResult ChuDePartial()
        {
            var listChuDe = from cd in db.CHUDEs select cd; 
            return PartialView(listChuDe);
        }
        private List<XEDAP> LaySachBanNhieu(int count)
        {
            return db.XEDAPs.OrderByDescending(item => item.SoLuongBan).Take(count).ToList();
        }
        public ActionResult SachBanNhieuPartial()
        {
            var ds = LaySachBanNhieu(6);
            return PartialView(ds);
        }
        public ActionResult NavPartial()
        {
            return PartialView();
        }
        public ActionResult SachTheoChuDe(int id, int ? page)
        {
            ViewBag.MaCd = id;
            const int PageSize = 3;
            int NowPage = page ?? 1;

            var ds = from s in db.XEDAPs where s.MaCD == id select s;
            int ItemTotal = ds.Count();
            int SkipPage = (NowPage - 1) * PageSize;
            var pager = new Pager(ItemTotal, NowPage, PageSize);
            var ds1 = ds.OrderBy(item => item.MaCD).Skip(SkipPage).Take(PageSize);
            ViewBag.Pager = pager;

            return View(ds1);
        }
        public ActionResult SachTheoNhaXuatBan(int id, int ? page)
        {
            ViewBag.MaNXB = id;
            const int PageSize = 3;
            int NowPage = page ?? 1;
            var ds = from s in db.XEDAPs where s.MaNXB == id select s;
            int ItemTotal = ds.Count();
            int SkipPage = (NowPage - 1) * PageSize;
            var pager = new Pager(ItemTotal, NowPage, PageSize);
            var ds1 = ds.OrderBy(item => item.MaNXB).Skip(SkipPage).Take(PageSize);
            ViewBag.Pager = pager;
            return View(ds1);
        }
        public ActionResult NhaXuatBanPartial()
        {
            var listNhaXuatBan = from cd in db.NHAXUATBANs select cd; 
            return PartialView(listNhaXuatBan);
        }
        public ActionResult ChiTietSach(int id)
        {
            var sach = from s in db.XEDAPs where s.MaXe == id select s;
            return View(sach.Single());
        }
        public ActionResult LoginLogout()
        {
            return PartialView("LoginLogout");
        }
        public ActionResult TimKiem()
        {
            
            return View();
        }

        public List<XEDAP> SearchByKey(string key)
        {
            return db.XEDAPs.SqlQuery("Select * from SACH WHERE TenXe like N'%"+key+"%'").ToList();
        }
    }


}

 
