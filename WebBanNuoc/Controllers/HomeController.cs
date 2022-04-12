using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanNuoc.Models;

namespace WebBanNuoc.Controllers
{
    public class HomeController : Controller
    {
        DBDrinkStoreDataContext data = new DBDrinkStoreDataContext();
        public ActionResult IndexTC(string searchString, int ? page)
        {
            ViewBag.Tongsoluong = TongSoLuongSanPham();
            ViewBag.Keyword = searchString;
            if (page == null) page = 1;
            var all_nuoc = (from s in data.NuocNgots select s).OrderBy(m => m.MaNuocngot);
            if (!string.IsNullOrEmpty(searchString)) all_nuoc = (IOrderedQueryable<NuocNgot>)all_nuoc.Where(a => a.TenNuoc.Contains(searchString));
            int pageSize = 6;
            int pageNum = page ?? 1;
            return View(all_nuoc.ToPagedList(pageNum, pageSize));
        }
        private int TongSoLuongSanPham()
        {
            int tsl = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                tsl = lstGiohang.Count;
            }
            return tsl;
        }
        public JsonResult CheckUsername(string userdata)
        {
            System.Threading.Thread.Sleep(200);
            var SearchData = data.KhachHangs.Where(x => x.TaiKhoan == userdata).SingleOrDefault();

            if (SearchData != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }
        }
        public JsonResult CheckEmail(string userdata)
        {
            System.Threading.Thread.Sleep(200);
            var SearchData = data.KhachHangs.Where(x => x.Email == userdata).SingleOrDefault();

            if (SearchData != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }
        }

        public JsonResult CheckPhone(string userdata)
        {
            System.Threading.Thread.Sleep(200);
            var SearchData = data.KhachHangs.Where(x => x.DienThoai == userdata).SingleOrDefault();

            if (SearchData != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}