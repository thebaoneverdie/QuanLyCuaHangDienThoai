using Nhom4_DoAnWeb_CHDT.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Nhom4_DoAnWeb_CHDT.Controllers
{
    public class KhachHangController : Controller
    {
        // GET: KhachHang
        MyDataDataContext data = new MyDataDataContext();
        public ActionResult danh_sach(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 12;
            int pageNum = page ?? 1;
            var all_KH = from kh in data.tblKHACHHANGs select kh;
            return View(all_KH.ToPagedList(pageNum, pageSize));
        }
        public ActionResult danh_sach_QT(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 12;
            int pageNum = page ?? 1;
            var all_KH = from kh in data.tblKHACHHANGs select kh;
            return View(all_KH.ToPagedList(pageNum, pageSize));
        }
        //Get: KhachHang/Search
        public ActionResult tim_kiem(string searchName)
        {
            var search = data.tblKHACHHANGs.Where(p => p.TENKH.ToLower().Contains(searchName.ToLower()));
            return View(search);
        }
        // GET: KhachHang/Details/5
        public ActionResult thong_tin(int id)
        {
            var D_kh = data.tblKHACHHANGs.First(p => p.MAKH == id);
            if (D_kh != null)
                return View(D_kh);
            return View("danh_sach");
        }

        // GET: KhachHang/Create
        public ActionResult them_moi()
        {
            return View();
        }

        // POST: KhachHang/Create
        [HttpPost]
        public ActionResult them_moi(FormCollection collection, tblKHACHHANG kh)
        {
            try
            {
                // TODO: Add insert logic here
                var C_tenKH = collection["TENKH"];
                var C_diachi = collection["DIACHI"];
                var C_dienthoai = collection["DIENTHOAI"];
                var C_email = collection["EMAIL"];
                var find = data.tblKHACHHANGs.FirstOrDefault(p => p.TENKH == C_tenKH && p.DIENTHOAI == C_dienthoai && p.EMAIL == C_email);
                if (!Regex.IsMatch(C_dienthoai, "^0\\d{9}$"))
                {
                    ViewData["SDTKhongHopLe"] = "Số điện thoại không hợp lệ!";
                    return this.them_moi();
                }
                if (!Regex.IsMatch(C_email, @"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$"))
                {
                    ViewData["EmailKhongHopLe"] = "Địa chỉ email không hợp lệ!";
                    return this.them_moi();
                }
                if (string.IsNullOrEmpty(C_tenKH) || string.IsNullOrEmpty(C_diachi)
                    || string.IsNullOrEmpty(C_dienthoai) || string.IsNullOrEmpty(C_email))
                {
                    ViewData["Error"] = "Vui lòng điền đầy đủ thông tin!";
                    return this.them_moi();
                }
                else
                {
                    if(find == null)
                    {
                        kh.TENKH = C_tenKH;
                        kh.DIACHI = C_diachi;
                        kh.DIENTHOAI = C_dienthoai;
                        kh.EMAIL = C_email;

                        data.tblKHACHHANGs.InsertOnSubmit(kh);
                        data.SubmitChanges();
                    }
                    else
                    {
                        return View("thong_tin",find);
                    }
                    return RedirectToAction("danh_sach");
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: KhachHang/Edit/5
        public ActionResult sua_thong_tin(int? id)
        {
            var E_kh = data.tblKHACHHANGs.First(p => p.MAKH == id);
            if (E_kh != null)
                return View(E_kh);
            return View("danh_sach");
        }

        // POST: KhachHang/Edit/5
        [HttpPost]
        public ActionResult sua_thong_tin(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var E_tenKH = collection["TENKH"];
                var E_diachi = collection["DIACHI"];
                var E_dienthoai = collection["DIENTHOAI"];
                var E_email = collection["EMAIL"];
                var kh = data.tblKHACHHANGs.First(p => p.MAKH == id);
                if (!Regex.IsMatch(E_dienthoai, "^0\\d{9}$"))
                {
                    ViewData["SDTKhongHopLe"] = "Số điện thoại không hợp lệ!";
                    return this.sua_thong_tin(id);
                }
                if (!Regex.IsMatch(E_email, @"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$"))
                {
                    ViewData["EmailKhongHopLe"] = "Địa chỉ email không hợp lệ!";
                    return this.sua_thong_tin(id);
                }
                if (string.IsNullOrEmpty(E_tenKH) || string.IsNullOrEmpty(E_diachi)
                    || string.IsNullOrEmpty(E_dienthoai) || string.IsNullOrEmpty(E_email))
                {
                    ViewData["Error"] = "Vui lòng điền đầy đủ thông tin!";
                    return this.sua_thong_tin(id);

                }
                else
                {
                    kh.TENKH = E_tenKH;
                    kh.DIACHI = E_diachi;
                    kh.DIENTHOAI = E_dienthoai;
                    kh.EMAIL = E_email;

                    UpdateModel(kh);
                    data.SubmitChanges();
                    return RedirectToAction("danh_sach");
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: KhachHang/Delete/5
        public ActionResult xoa_KH(int? id)
        {
            var D_kh = data.tblKHACHHANGs.First(p => p.MAKH == id);
            if (D_kh != null)
                return View(D_kh);
            return View("danh_sach");
        }

        // POST: KhachHang/Delete/5
        [HttpPost]
        public ActionResult xoa_KH(int id, FormCollection collection)
        {
            var delKH = data.tblKHACHHANGs.First(p => p.MAKH == id);
            data.tblKHACHHANGs.DeleteOnSubmit(delKH);
            data.SubmitChanges();
            return RedirectToAction("danh_sach");
        }
    }
}
