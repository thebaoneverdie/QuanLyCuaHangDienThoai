using Nhom4_DoAnWeb_CHDT.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Nhom4_DoAnWeb_CHDT.Models
{
    public class Giohang
    {
        MyDataDataContext data = new MyDataDataContext();
        public int maDT { get; set; }
        [Display(Name = "Tên Điện Thoại")]
        public string tenDT { get; set; }
        [Display(Name = "Ảnh")]
        public string hinh { get; set; }
        [Display(Name = "Giá bán")]
        public Double giaban { get; set; }
        [Display(Name = "Giá nhập")]
        public Double gianhap { get; set; }
        [Display(Name = "Số lượng")]
        public int isoluong { get; set; }
        [Display(Name = "Thành tiền")]
        public Double dThanhtien
        {
            get { return isoluong * giaban; }
        }
        public Double dTienNhap
        {
            get { return isoluong * gianhap; }
        }
        public Giohang() {
            ListNV = data.tblNHANVIENs.ToList();
        }
        public Giohang(int id)
        {
            maDT = id;
            tblDIENTHOAI dt = data.tblDIENTHOAIs.Single(n => n.MADT == maDT);
            tenDT = dt.TENDT;
            hinh = dt.HINH;
            giaban = double.Parse(dt.GIABAN.ToString());
            gianhap = double.Parse(dt.GIAMUA.ToString());
            isoluong = 1;
        }
        public List<tblNHANVIEN> ListNV = new List<tblNHANVIEN>();
    }
}