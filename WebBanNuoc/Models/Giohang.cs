using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanNuoc.Models
{
    public class Giohang
    {
       DBDrinkStoreDataContext data = new DBDrinkStoreDataContext();
        public int MaNuocngot { get; set; }
        [Display(Name = "Tên nước")]
        public string TenNuoc { get; set; }
        [Display(Name = "Ảnh bìa")]
        public string Anhbia { get; set; }
        [Display(Name = "Giá bán")]
        public Double giagiam { get; set; }
        [Display(Name = "Số lượng")]
        public int iSoluong { get; set; }    
        public int soluongton { get; set; }
        [Display(Name = "Thành tiền")]
        public Double dThanhtien
        {
            get { return iSoluong * giagiam; }
        }
        public Giohang(int id)
        {
            MaNuocngot = id;
            NuocNgot nuoc = data.NuocNgots.Single(n => n.MaNuocngot == MaNuocngot);
            TenNuoc = nuoc.TenNuoc;
            Anhbia = nuoc.Anhbia ;
            giagiam = double.Parse(nuoc.giagiam.ToString());
            iSoluong = 1;
            soluongton = int.Parse(nuoc.Soluongton.ToString());
        }
    }
}