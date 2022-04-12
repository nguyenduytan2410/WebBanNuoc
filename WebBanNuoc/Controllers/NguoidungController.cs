using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebBanNuoc.Models;

namespace WebBanNuoc.Controllers
{
    public class NguoidungController : Controller
    {
        // GET: Nguoidung
        DBDrinkStoreDataContext data = new DBDrinkStoreDataContext();
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangky(FormCollection collection, KhachHang kh)
        {
            var HoTen = collection["HoTen"];
            var TaiKhoan = collection["TaiKhoan"];
            var MatKhau = collection["MatKhau"];
            var MatKhauXacNhan = collection["MatKhauXacNhan"];
            var Email = collection["Email"];
            var Diachi = collection["Diachi"];
            /* var GT = collection["GioiTinh"];*/
            var DienThoai = collection["DienThoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["NgaySinh"]);
            if (String.IsNullOrEmpty(MatKhauXacNhan))
            {
                ViewData["NhapMKXN"] = "Phải nhập mật khẩu xác nhận!";
            }
            else if (String.IsNullOrEmpty(HoTen))
                ViewData["NhapHoten"] = "Phải nhập đủ họ tên";
            else if (String.IsNullOrEmpty(TaiKhoan))
                ViewData["NhapTK"] = "Phải nhập tài khoản";
            else if (String.IsNullOrEmpty(MatKhau))
                ViewData["nhapMK"] = "Phải nhập mật khẩu";
            else if (MatKhau.Length < 8)
                ViewData["dodaiMK"] = "mật khẩu phải nhiều hơn 8 ký tự";
            else if (String.IsNullOrEmpty(Diachi))
                ViewData["nhapDC"] = "Phải nhập địa chỉ";
            else if (String.IsNullOrEmpty(Email))
                ViewData["nhapEmail"] = "Phải nhập Email";
            else if (String.IsNullOrEmpty(DienThoai))
                ViewData["nhapDT"] = "Phải nhập số điện thoại";
            else if (String.IsNullOrEmpty(DienThoai))
                ViewData["nhapGT"] = "Phải nhập GT";
            else if (String.IsNullOrEmpty(MatKhau))
                ViewData["nhapMK"] = "Phải nhập mật khẩu";
            else if (String.IsNullOrEmpty(ngaysinh))
                ViewData["nhapNS"] = "Phải nhập ngày sinh";
            else
            {
                if (!MatKhau.Equals(MatKhauXacNhan))
                {
                    ViewData["MatKhauGiongNhau"] = "Mật khẩu và mật khẩu xác nhận phải giống nhau";
                }
                else
                {
                    kh.HoTen = HoTen;
                    kh.TaiKhoan = TaiKhoan;
                    kh.MatKhau = MatKhau;
                    kh.Email = Email;
                    kh.Diachi = Diachi;
                    kh.DienThoai = DienThoai;
                    kh.NgaySinh = DateTime.Parse(ngaysinh);
                    data.KhachHangs.InsertOnSubmit(kh);
                    data.SubmitChanges();
                    return RedirectToAction("DangNhap");
                }
            }
            return this.Dangky();
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var TaiKhoan = collection["TaiKhoan"];
            var MatKhau = collection["MatKhau"];
            KhachHang kh = data.KhachHangs.SingleOrDefault(n => n.TaiKhoan == TaiKhoan && n.MatKhau == MatKhau);
            if (kh != null)
            {
                ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                Session["Taikhoan"] = kh;
            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return RedirectToAction("IndexTC", "Home");
        }
        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("DangNhap");
        }
        public ActionResult profile(int id)
        {
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            kh.MaKH = id;
            var E_Pro = data.KhachHangs.First(m => m.MaKH == id);
            return View(E_Pro);
        }
        [HttpPost]
        public ActionResult profile(int id, FormCollection collection)
        {
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            kh.MaKH = id;
            var E_Pro = data.KhachHangs.First(m => m.MaKH == id);

            var hoten = collection["HoTen"];
            var tendangnhap = collection["TaiKhoan"];
            var matkhau = collection["MatKhau"];
            var MatkhauXacNhan = collection["MatKhauXacNhan"];
            var email = collection["Email"];
            var diachi = collection["DiaChi"];
            var dienthoai = collection["DienThoai"];
            var gioitinh = collection["GioiTinh"];
            var ngaysinh = String.Format("{0:dd/MM/yyyy}", collection["ngaysinh"]);

            E_Pro.MaKH = id;
            if (string.IsNullOrEmpty(hoten))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {

                kh.TaiKhoan = tendangnhap;
                kh.MatKhau = matkhau;
                kh.Email = email;
                kh.Diachi = diachi;
                kh.DienThoai = dienthoai;
                kh.NgaySinh = DateTime.Parse(ngaysinh);
                kh.MaKH = Convert.ToInt32(collection["id"]);
                kh.GioiTinh = gioitinh;
                UpdateModel(E_Pro);
                data.SubmitChanges();
                return RedirectToAction("Index", "Home");
            }
            return this.profile(id);
        }

    }
}