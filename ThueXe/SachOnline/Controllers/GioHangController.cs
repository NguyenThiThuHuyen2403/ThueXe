using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayPal.Api;
using SachOnline.Models;

namespace SachOnline.Controllers
{
    public class GioHangController : Controller
    {
        QLBanSachEntities db = new QLBanSachEntities();
        // GET: GioHang
        public ActionResult Index()
        {
            return View();
        }
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        public ActionResult ThemGioHang(int ms, string url)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.Find(n => n.iMaXe == ms);
            if (sp == null)
            {
                sp = new GioHang(ms);
                lstGioHang.Add(sp);
            }
            else
            {
                sp.iSoLuong++;
            }
            return Redirect(url);
        }
        private int TongSoLuong()
        {
            int sum = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                sum = lstGioHang.Sum(n => n.iSoLuong);
            }
            return sum;
        }
        private int TongSoNgayThue()
        {
            int sum = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                sum = lstGioHang.Sum(n => n.iSoNgayThue);
            }
            return sum;
        }
        public float TongTien()
        {
            float sum = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                sum = lstGioHang.Sum(n => n.dThanhTien);
            }
            return sum;
        }
        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "SachOnline");
            }
            ViewBag.TongTien = TongTien();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongSoNgayThue = TongSoNgayThue();
            return View(lstGioHang);
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.TongTien = TongTien();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongSoNgayThue = TongSoNgayThue();
            return PartialView();
        }
        public ActionResult XoaSPKhoiGioHang(int iMaXe)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.FirstOrDefault(item => item.iMaXe == iMaXe);
            if (sp != null)
            {
                lstGioHang.RemoveAll(item => item.iMaXe == iMaXe);
                if (lstGioHang.Count == 0)
                {
                    RedirectToAction("Index", "SachOnline");
                }
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapNhatGioHang(int iMaXe, FormCollection f)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.SingleOrDefault(item => item.iMaXe == iMaXe);
            if (sp != null)
            {
                sp.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
                sp.iSoNgayThue = int.Parse(f["txtSoNgayThue"].ToString());
            }

            return RedirectToAction("GioHang");
        }

        public ActionResult XoaGioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("Index", "SachOnline");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return Redirect("~/User/DangNhap?id=2");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "SachOnline");
            }
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            ViewBag.TongSoNgayThue = TongSoNgayThue();
            return View(lstGioHang);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection f)
        {
            int Htt = int.Parse(f["HTThanhToan"]);
            if(Htt==2)
            {
                DateTime NG =DateTime.Parse(String.Format("{0:MM/dd/yyyy}", f["NgayGiao"]));
                Session["NgayGiao"] = NG;
                return RedirectToAction("PaymentWithPaypal");
            }    
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
            List<GioHang> lstGioHang = LayGioHang();
            ddh.MaKH = kh.MaKH;
            ddh.TenNguoiNhan = kh.HoTenKH;
            ddh.DiaChiNhan = kh.DiaChiKH;
            ddh.TriGia = TongTien();
            ddh.DienThoaiNhan = kh.DienThoaiKH;
            ddh.NgayDH = DateTime.Now;
            var NgayGiao = String.Format("{0:MM/dd/yyyy}", f["NgayGiao"]);
            ddh.NgayGiaoHang = DateTime.Parse(NgayGiao);
            ddh.HTThanhToan = "COD";
            ddh.SoNgayThue = TongSoNgayThue();
            db.DONDATHANGs.Add(ddh);
            db.SaveChanges();
            foreach (var item in lstGioHang)
            {
                CTDATHANG ctdh = new CTDATHANG();
                ctdh.SoDH = ddh.SoDH;
                ctdh.MaXe = item.iMaXe;
                ctdh.SoLuong = item.iSoLuong;
                ctdh.DonGia = (decimal)item.dDonGia;
                db.CTDATHANGs.Add(ctdh);
            }
            db.SaveChanges();

            var strSanPham = "";
            var thanhtien = decimal.Zero;
            var TongTienn = decimal.Zero;
           
            foreach (var sp in lstGioHang)
            {
                strSanPham += "<tr>";
                strSanPham += "<td>" + sp.sTenXe + "</td>";
                strSanPham += "<td>" + sp.iSoLuong + "</td>";
                strSanPham += "<td>" + sp.iSoNgayThue + "</td>";
                strSanPham += "<td>" + SachOnline.Common.Common.FormatNumber(sp.dDonGia, 0) + "</td>";
                strSanPham += "</tr>";
                thanhtien += sp.iSoLuong * sp.dDonGia *sp.iSoNgayThue;
            }
            TongTienn = thanhtien;
            string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));
            //var TongSoLuongg = TongSoLuong();
            contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
            contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
            contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", kh.HoTenKH);
            contentCustomer = contentCustomer.Replace("{{Phone}}", kh.DienThoaiKH);
            contentCustomer = contentCustomer.Replace("{{Email}}", kh.Email);
            contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", kh.DiaChiKH);
            contentCustomer = contentCustomer.Replace("{{TongSoLuong}}", SachOnline.Common.Common.FormatNumber(TongSoLuong(), 0));
            contentCustomer = contentCustomer.Replace("{{TongSoNgayThue}}", SachOnline.Common.Common.FormatNumber(TongSoNgayThue(), 0));
            contentCustomer = contentCustomer.Replace("{{ThanhTien}}", SachOnline.Common.Common.FormatNumber(thanhtien, 0));
            contentCustomer = contentCustomer.Replace("{{TongTien}}", SachOnline.Common.Common.FormatNumber(TongTienn, 0));
            contentCustomer = contentCustomer.Replace("{{HTThanhToan}}", ddh.HTThanhToan);
            SachOnline.Common.Common.SendMail("XeDapOnline", "Đơn hàng #" + ddh.SoDH, contentCustomer.ToString(), kh.Email);

            
            Session["GioHang"] = null;

            return RedirectToAction("XacNhanDonHang", "GioHang");
        }
        public ActionResult XacNhanDonHang()
        {

            return View();
        }

        public ActionResult FailureView()
        {
            return View();
        }
      
        //Paypal
        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/GioHang/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
            List<GioHang> lstGioHang = LayGioHang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDH = DateTime.Now;
            ddh.TenNguoiNhan = kh.HoTenKH;
            ddh.DiaChiNhan = kh.DiaChiKH;
            ddh.TriGia = TongTien();
            ddh.DienThoaiNhan = kh.DienThoaiKH;
            DateTime NG =(DateTime)Session["NgayGiao"];
            if(NG!=null)
            {
                ddh.NgayGiaoHang = NG;
                Session["NgayGiao"] = null;
            }

            ddh.HTThanhToan = "Thanh toán bằng Paypal";
            ddh.SoNgayThue = TongSoNgayThue();
            db.DONDATHANGs.Add(ddh);
            db.SaveChanges();
            foreach (var item in lstGioHang)
            {
                CTDATHANG ctdh = new CTDATHANG();
                ctdh.SoDH = ddh.SoDH;
                ctdh.MaXe = item.iMaXe;
                ctdh.SoLuong = item.iSoLuong;
                ctdh.DonGia = (decimal)item.dDonGia;
                db.CTDATHANGs.Add(ctdh);
            }
            db.SaveChanges();

            var strSanPham = "";
            var thanhtien = decimal.Zero;
            var TongTienn = decimal.Zero;
            foreach (var sp in lstGioHang)
            {
                strSanPham += "<tr>";
                strSanPham += "<td>" + sp.sTenXe + "</td>";
                strSanPham += "<td>" + sp.iSoLuong + "</td>";
                strSanPham += "<td>" + sp.iSoNgayThue + "</td>";
                strSanPham += "<td>" + SachOnline.Common.Common.FormatNumber(sp.dDonGia, 0) + "</td>";
                strSanPham += "</tr>";
                thanhtien += sp.iSoLuong * sp.dDonGia * sp.iSoNgayThue;
            }
            TongTienn = thanhtien;
            string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));

            contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
            contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", kh.HoTenKH);
            contentCustomer = contentCustomer.Replace("{{Phone}}", kh.DienThoaiKH);
            contentCustomer = contentCustomer.Replace("{{Email}}", kh.Email);
            contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", kh.DiaChiKH);
            contentCustomer = contentCustomer.Replace("{{TongSoLuong}}", SachOnline.Common.Common.FormatNumber(TongSoLuong(), 0));
            contentCustomer = contentCustomer.Replace("{{TongSoNgayThue}}", SachOnline.Common.Common.FormatNumber(TongSoNgayThue(), 0));
            contentCustomer = contentCustomer.Replace("{{ThanhTien}}", SachOnline.Common.Common.FormatNumber(thanhtien, 0));
            contentCustomer = contentCustomer.Replace("{{TongTien}}", SachOnline.Common.Common.FormatNumber(TongTienn, 0));
            contentCustomer = contentCustomer.Replace("{{HTThanhToan}}", ddh.HTThanhToan);
            SachOnline.Common.Common.SendMail("XeDapOnline", "Đơn hàng #" + ddh.SoDH, contentCustomer.ToString(), kh.Email);


            Session["GioHang"] = null;
            //on successful payment, show success page to user.  
            return RedirectToAction("XacNhanDonHang", "GioHang");
        }
        private PayPal.Api.Payment payment;
        //Thực hiện cộng tiền, trừ tiền
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        //Tính tổng tiền
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            List<GioHang> lstGioHang = LayGioHang();
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            foreach (var item in lstGioHang)
            {
                itemList.items.Add(new Item()
                {
                    name = item.sTenXe,
                    currency = "USD",
                    price = item.dThanhTien.ToString(),
                    quantity = item.iSoLuong.ToString(),
                    sku = item.iMaXe.ToString()
                });
            }
            //Adding Item Details like name, currency, price etc  
            
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = TongTien().ToString()
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = TongTien().ToString(), // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            var paypalOrderId = DateTime.Now.Ticks;
            transactionList.Add(new Transaction()
            {
                description = $"Invoice #{paypalOrderId}",
                invoice_number = paypalOrderId.ToString(), //Generate an Invoice No    
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }

    }
}