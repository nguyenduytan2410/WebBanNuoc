using System.Web.Mvc;
using WebBanNuoc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace WebBanNuoc.Controllers
{
    public class NuocController : Controller
    {
        DBDrinkStoreDataContext data = new DBDrinkStoreDataContext();
        // GET: Nuoc
        public ActionResult ListNuoc(string searchString)
        {
            KhachHang kh = (KhachHang)Session["TaiKhoan"];  
            var all_nuoc = from ss in data.NuocNgots select ss;
            if (!String.IsNullOrEmpty(searchString))
            {
                all_nuoc = all_nuoc.Where(s => s.TenNuoc.Contains(searchString));
                return View(all_nuoc);
            }
            if (Session["TaiKhoan"] == null)
            {
                return RedirectToAction("XacNhanAdmin", "GioHang");
            }
            else
            {
                if (kh.Admin == 1)
                {

                    return View(all_nuoc);
                }
                else
                {
                    return RedirectToAction("IndexTC", "Home");
                }
            }
        }
        public ActionResult Detail(int id)
        {
            var D_nuoc = data.NuocNgots.Where(m => m.MaNuocngot == id).First();
            return View(D_nuoc);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, NuocNgot s)
        {
            var E_TenNuoc = collection["TenNuoc"];
            var E_Anhbia = collection["Anhbia"];
          
            var E_Mota= collection["Mota"];
            var E_GiaBan= Convert.ToDecimal(collection["GiaBan"]);
            var E_Ngaycapnhat = Convert.ToDateTime(collection["ngaycapnhat"]);
            var E_Soluongton = Convert.ToInt32(collection["soluongton"]);
            if (string.IsNullOrEmpty(E_TenNuoc))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                s.TenNuoc =E_TenNuoc.ToString();
                s.Anhbia = E_Anhbia.ToString();
                s.Mota = E_Mota.ToString();
                s.giagiam = E_GiaBan;
                s.giamgia= 1.0;
                s.GiaBan = E_GiaBan;
                s.Ngaycapnhat = E_Ngaycapnhat;
                s.Soluongton = E_Soluongton;
                data.NuocNgots.InsertOnSubmit(s);
                data.SubmitChanges();
                return RedirectToAction("ListNuoc");
            }
            return this.Create();
        }
        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
            return "/Content/images/" + file.FileName;
        }
        public ActionResult Edit(int id)
        {
            var E_nuoc = data.NuocNgots.First(m => m.MaNuocngot == id);
            return View(E_nuoc);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var E_nuoc = data.NuocNgots.First(m => m.MaNuocngot == id);
            var E_TenNuoc = collection["TenNuoc"];
            var E_Anhbia = collection["Anhbia"];
     
            var E_Mota = collection["Mota"];
            var E_GiaBan = Convert.ToDecimal(collection["GiaBan"]);
            var E_Ngaycapnhat = Convert.ToDateTime(collection["ngaycapnhat"]);
            var E_Soluongton = Convert.ToInt32(collection["soluongton"]);
            var E_Discount = Convert.ToDouble(collection["giamgia"]);
            var E_DisPrice = Convert.ToDecimal(collection["GiaBan"]) * Convert.ToDecimal(collection["giamgia"]);
            E_nuoc.MaNuocngot = id;
            if (string.IsNullOrEmpty(E_TenNuoc))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                E_nuoc.TenNuoc = E_TenNuoc;
                E_nuoc.Anhbia = E_Anhbia;            
                E_nuoc.GiaBan = E_GiaBan;
                E_nuoc.Mota = E_Mota;
                E_nuoc.giamgia = E_Discount;
                E_nuoc.giagiam = E_DisPrice;
                E_nuoc.Ngaycapnhat = E_Ngaycapnhat;
                E_nuoc.Soluongton = E_Soluongton;
                UpdateModel(E_nuoc);
                data.SubmitChanges();
                return RedirectToAction("ListNuoc");
            }
            return this.Edit(id);
        }
        public ActionResult Delete(int id)
        {
            var D_nuoc = data.NuocNgots.First(m => m.MaNuocngot == id);
            return View(D_nuoc);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_nuoc = data.NuocNgots.Where(m => m.MaNuocngot == id).First();
            data.NuocNgots.DeleteOnSubmit(D_nuoc);
            data.SubmitChanges();
            return RedirectToAction("ListNuoc");
        }
        public ActionResult LoaiSanPham()
        {
            var loaisanpham = from cdsp in data.ChuDes select cdsp; 
            return PartialView(loaisanpham);
        }
        public ActionResult SPTheoLoai(int id)
        {
            var sptl = from tl in data.NuocNgots where  tl.MaChuDe == id select tl;
            return PartialView(sptl);
        }
        public ActionResult ThuongHieu()
        {
            var thuonghieu = from th in data.NhaXuatBans select th;
            return PartialView(thuonghieu);
        }
        public ActionResult SPTheoTH(string id)
        {
            var sptth = from tl in data.NuocNgots where tl.MaNXB == id select tl;
            return PartialView(sptth);
        }
        public ActionResult Map()
        {
            return View();
        }
    }
}