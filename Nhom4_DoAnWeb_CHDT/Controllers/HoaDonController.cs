using System;
using PagedList;
using Nhom4_DoAnWeb_CHDT.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace Nhom4_DoAnWeb_CHDT.Controllers
{
    public class HoaDonController : Controller
    {
        // GET: HoaDon
        MyDataDataContext data = new MyDataDataContext();
        public ActionResult nhap_TTHD()
        {
            return View();
        }
        [HttpPost]
        public ActionResult nhap_TTHD(FormCollection collection)
        {
            tblKHACHHANG kh = new tblKHACHHANG();
            tblTAIKHOAN tk = (tblTAIKHOAN)Session["TaiKhoan"];

            if(tk.role == "KH")
            { 
                var C_tenKH = collection["TENKH"];
                var C_diachi = collection["DIACHI"];
                var C_dienthoai = collection["DIENTHOAI"];
                var C_email = collection["EMAIL"];
                if (!Regex.IsMatch(C_dienthoai, "^0\\d{9}$"))
                {
                    ViewData["SDTKhongHopLe"] = "Số điện thoại không hợp lệ!";
                    return this.nhap_TTHD();
                }
                if (!Regex.IsMatch(C_email, @"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$"))
                {
                    ViewData["EmailKhongHopLe"] = "Địa chỉ email không hợp lệ!";
                    return this.nhap_TTHD();
                }
                if (string.IsNullOrEmpty(C_tenKH) || string.IsNullOrEmpty(C_diachi)
                    || string.IsNullOrEmpty(C_dienthoai) || string.IsNullOrEmpty(C_email))
                {
                    ViewData["Error"] = "Vui lòng điền đầy đủ thông tin!";
                    return this.nhap_TTHD();
                }
                else
                {
                    var find_KH = data.tblKHACHHANGs.FirstOrDefault(p => p.EMAIL == C_email && p.DIENTHOAI == C_dienthoai && p.DIACHI == C_diachi
                                                        && p.TENKH == C_tenKH);
                    if (find_KH != null)
                    {
                        Session["KhachHang"] = find_KH;
                        return RedirectToAction("danh_sach_HD");
                    }
                }
            }
            if (tk.role == "NV")
            {
                var C_tenNV = collection["TENNV"];
                var C_diachi = collection["DIACHI"];
                var C_dienthoai = collection["DIENTHOAI"];
                var C_email = collection["EMAIL"];
                if (!Regex.IsMatch(C_dienthoai, "^0\\d{9}$"))
                {
                    ViewData["SDTKhongHopLe"] = "Số điện thoại không hợp lệ!";
                    return this.nhap_TTHD();
                }
                if (!Regex.IsMatch(C_email, @"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$"))
                {
                    ViewData["EmailKhongHopLe"] = "Địa chỉ email không hợp lệ!";
                    return this.nhap_TTHD();
                }
                if (string.IsNullOrEmpty(C_tenNV) || string.IsNullOrEmpty(C_diachi)
                    || string.IsNullOrEmpty(C_dienthoai) || string.IsNullOrEmpty(C_email))
                {
                    ViewData["Error"] = "Vui lòng điền đầy đủ thông tin!";
                    return this.nhap_TTHD();
                }
                else
                {
                    var find_NV = data.tblNHANVIENs.FirstOrDefault(p => p.EMAIL == C_email && p.DIENTHOAI == C_dienthoai
                                                        && p.DIACHI == C_diachi && p.TENNV == C_tenNV);
                    if (find_NV != null)
                    {
                        Session["NhanVien"] = find_NV;
                        return RedirectToAction("danh_sach_HD");
                    }
                }
            }
            return View("trang_chu", "KhoaiTei");
        }
        public ActionResult danh_sach_HD(int? page)
        {
            if (page == null) page = 1;
            tblKHACHHANG kh = (tblKHACHHANG)Session["KhachHang"];
            if (kh != null)
            {
                var all_hd = (from s in data.tblHOADONs select s).Where(m => m.MAKH == kh.MAKH).OrderBy(m => m.MAHD);
                int pageSize = 12;
                int pageNum = page ?? 1;
                return View(all_hd.ToPagedList(pageNum, pageSize));
            }
            tblNHANVIEN nv = (tblNHANVIEN)Session["NhanVien"];
            if (nv != null)
            {
                var all_hd = (from s in data.tblHOADONs select s).Where(m => m.MANV == nv.MANV).OrderBy(m => m.MAHD);
                int pageSize = 12;
                int pageNum = page ?? 1;
                return View(all_hd.ToPagedList(pageNum, pageSize));
            }
            return View("trang_chu", "KhoaiTei");
        }
        public ActionResult danh_sach_HDQT(int? page)
        {
                var all_hd = (from s in data.tblHOADONs select s).OrderBy(m => m.MANV);
                int pageSize = 12;
                int pageNum = page ?? 1;
                return View(all_hd.ToPagedList(pageNum, pageSize));
        }
        // GET: HoaDon/Details/5
        public ActionResult thong_tin(int id)
        {
            var find = data.tblHOADONs.First(p => p.MAHD == id);
            if(find != null)
            {
                var cthd = data.tblCTHDs.Where(p => p.MAHD == id).ToList();
                ViewBag.cthd = cthd;
                return View(find);
            }
            return View("trang_chu", "KhoaiTei");
        }

        // GET: HoaDon/Create
        
        /*
        // GET: HoaDon/Delete/5
        public ActionResult xoa_HD(int id)
        {
            var find = data.tblHOADONs.FirstOrDefault(p => p.MAHD == id);
            if (find != null && find.NGAYLAP == DateTime.Now)
                return View(find);
            else
            {
                ViewData["thongbao"] = "Xin lỗi không thể xóa hóa đơn trong quá khứ";
                return this.xoa_HD(id);
            }
        }

        // POST: HoaDon/Delete/5
        [HttpPost]
        public ActionResult xoa_HD(int id, FormCollection collection)
        {
            var delHD = data.tblHOADONs.FirstOrDefault(p => p.MAHD == id);
            if(delHD != null)
            {
                var cthd = data.tblCTHDs.Where(p => p.MAHD == id).ToList();
                foreach (var item in cthd)
                {
                    var dt = data.tblDIENTHOAIs.FirstOrDefault(p => p.MADT == item.MADT);
                    if (dt != null)
                    {
                        dt.SLTON += item.SLMUA;
                    }
                }
                ViewBag.cthd = cthd;
                data.tblCTHDs.DeleteAllOnSubmit(cthd);
                data.SubmitChanges();
            }
            data.tblHOADONs.DeleteOnSubmit(delHD);
            data.SubmitChanges();
            return RedirectToAction("danh_sach_HDQTs");
        }
        */
        public ActionResult ThongKe()
        {
            var startDate = Convert.ToDateTime(Request.QueryString.Get("start-date"));
            var endDate = Convert.ToDateTime(Request.QueryString.Get("end-date"));
            var totalRevenue = data.tblHOADONs
                .Where(i => i.NGAYLAP >= startDate && i.NGAYLAP <= endDate)
                .Sum(i => i.TONGTG);
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            var hoadon = data.tblHOADONs.Where(i => i.NGAYLAP >= startDate && i.NGAYLAP <= endDate);
            return View(hoadon);
        }


    }
}
