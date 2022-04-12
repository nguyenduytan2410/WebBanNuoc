using mvcDangNhap.common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanNuoc.Models;

namespace WebBanNuoc.Controllers
{
    public class GiohangController : Controller
    {
        // GET: Giohang
        DBDrinkStoreDataContext data = new DBDrinkStoreDataContext();
        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang == null)
            {
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }
        public ActionResult ThemGioHang(int id, string strURL)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.Find(n => n.MaNuocngot == id);
            if (sanpham == null)
            {
                sanpham = new Giohang(id);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }
        private int TongSoluong()
        {
            int tsl = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                tsl = lstGiohang.Sum(n => n.iSoluong);
            }
            return tsl;
        }
        private int TongSoluongSanPham()
        {
            int tsl = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                tsl = lstGiohang.Count;
            }
            return tsl;
        }
        private double TongTien()
        {
            double tt = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                tt = lstGiohang.Sum(n => n.dThanhtien);
            }
            return tt;
        }
        public ActionResult GioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoluong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoluongSanPham();
            return View(lstGiohang);
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "*")
            {
                return RedirectToAction("DangNhap", "Nguoidung");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("ListNuoc", "Nuoc");
            }
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoluong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoluongSanPham();
            return View(lstGiohang);
        }
        public ActionResult DatHang(FormCollection collection)
        {
            DonDatHang dh = new DonDatHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            NuocNgot s = new NuocNgot();
            List<Giohang> gh = Laygiohang();
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);
            dh.MaKH = kh.MaKH;
            dh.Ngaydat = DateTime.Now;
            dh.Ngaygiao = DateTime.Parse(ngaygiao);
            dh.Tinhtranggiaohang = false;
            dh.Dathanhtoang = false;
            data.DonDatHangs.InsertOnSubmit(dh);
            data.SubmitChanges();
            int sl = 0;
            double tong = 0;
            foreach (var item in gh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.MaDonhang = dh.MaDonhang;
                ctdh.MaNuocngot = item.MaNuocngot;
                ctdh.Soluong = item.iSoluong;
                ctdh.Dongia = (decimal)item.giagiam;
                s = data.NuocNgots.Single(n => n.MaNuocngot == item.MaNuocngot);
                s.Soluongton -= ctdh.Soluong;
                sl += item.iSoluong;
                tong += item.dThanhtien;
                data.SubmitChanges();
                data.ChiTietDonHangs.InsertOnSubmit(ctdh);
            }
            string content = System.IO.File.ReadAllText(Server.MapPath("~/content/template/neworder.html"));
            content = content.Replace("{{CustomerName}}", dh.KhachHang.HoTen);
            content = content.Replace("{{Phone}}", dh.KhachHang.DienThoai);
            content = content.Replace("{{Email}}", dh.KhachHang.Email);
            content = content.Replace("{{Address}}", dh.KhachHang.Diachi);
            content = content.Replace("{{Total}}", tong.ToString("N0"));
            var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
            new MailHelper().SendMail(dh.KhachHang.Email, "Đơn hàng mới từ đại lý HHTTs", content);
            new MailHelper().SendMail(toEmail, "Đơn hàng", content);
            data.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XacnhanDonHang", "Giohang");
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoluong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoluongSanPham();
            return PartialView();
        }
        public ActionResult XoaGiohang(int id)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.MaNuocngot == id);
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.MaNuocngot == id);
                return RedirectToAction("GioHang");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapnhatGiohang(int id, FormCollection collection)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.MaNuocngot == id);
            if (sanpham != null)
            {
                sanpham.iSoluong = int.Parse(collection["txtSolg"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaTatCaGioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("GioHang");
        }
        public ActionResult Payment()
        {
            return View();
        }
        public ActionResult XacnhanDonHang()
        {
            return View();
        }
        public ActionResult XacNhanAdmin()
        {
            return View();
        }
        public ActionResult DaXacNhanAdmin()
        {
            return View();
        }
        public ActionResult TenDN()
        {
            return PartialView();
        }
    }
}