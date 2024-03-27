using Nhom4_DoAnWeb_CHDT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Nhom4_DoAnWeb_CHDT.Controllers
{
    public class NhanVienController : Controller
    {
        // GET: NhanVien
        MyDataDataContext data = new MyDataDataContext();
        public ActionResult danh_sach()
        {
            var all_NV = from nv in data.tblNHANVIENs select nv;
            return View(all_NV);
        }
        //Get: NhanVien/Search
        public ActionResult tim_kiem(string searchName)
        {
            var search = data.tblNHANVIENs.Where(p => p.TENNV.ToLower().Contains(searchName.ToLower()));
            return View(search);
        }
        // GET: NhanVien/Details/5
        public ActionResult thong_tin(int id)
        {
            var D_nv = data.tblNHANVIENs.First(p => p.MANV == id);
            if(D_nv != null)
                return View(D_nv);
            return View("danh_sach");
        }

        // GET: NhanVien/Create
        public ActionResult them_moi()
        {
            return View();
        }

        // POST: NhanVien/Create
        [HttpPost]
        public ActionResult them_moi(FormCollection collection, tblNHANVIEN nv)
        {
            try
            {
                // TODO: Add insert logic here
                var C_tenNV = collection["TENNV"];
                var C_diachi = collection["DIACHI"];
                var C_dienthoai = collection["DIENTHOAI"];
                var C_email = collection["EMAIL"];
                var check = data.tblNHANVIENs.FirstOrDefault(p => p.EMAIL == C_email);
                if (check != null)
                {
                    ViewData["emailTonTai"] = "Email đã tồn tại!";
                    return this.them_moi();
                }
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
                if (string.IsNullOrEmpty(C_tenNV) || string.IsNullOrEmpty(C_diachi)
                    || string.IsNullOrEmpty(C_dienthoai) || string.IsNullOrEmpty(C_email))
                {
                    ViewData["Error"] = "Vui lòng điền đầy đủ thông tin!";
                    return this.them_moi();
                }
                else
                {
                    nv.TENNV = C_tenNV;
                    nv.DIACHI = C_diachi;
                    nv.DIENTHOAI = C_dienthoai;
                    nv.EMAIL = C_email;

                    data.tblNHANVIENs.InsertOnSubmit(nv);
                    data.SubmitChanges();
                    return RedirectToAction("danh_sach");
                }
                return this.them_moi();
            }
            catch
            {
                return View();
            }
        }

        // GET: NhanVien/Edit/5
        public ActionResult sua_thong_tin(int? id)
        {
            var E_nv = data.tblNHANVIENs.First(p => p.MANV == id);
            if (E_nv != null)
                return View(E_nv);
            return View("danh_sach");
        }

        // POST: NhanVien/Edit/5
        [HttpPost]
        public ActionResult sua_thong_tin(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var E_tenNV = collection["TENNV"];
                var E_diachi = collection["DIACHI"];
                var E_dienthoai = collection["DIENTHOAI"];
                var E_email = collection["EMAIL"];
                var nv = data.tblNHANVIENs.First(p => p.MANV == id);
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
                if (string.IsNullOrEmpty(E_tenNV) || string.IsNullOrEmpty(E_diachi)
                    || string.IsNullOrEmpty(E_dienthoai) || string.IsNullOrEmpty(E_email))
                {
                    ViewData["Error"] = "Vui lòng điền đầy đủ thông tin!";
                }
                else
                {
                    nv.TENNV = E_tenNV;
                    nv.DIACHI = E_diachi;
                    nv.DIENTHOAI = E_dienthoai;
                    nv.EMAIL = E_email;

                    UpdateModel(nv);
                    data.SubmitChanges();
                    return RedirectToAction("danh_sach");
                }
                return this.sua_thong_tin(id);
            }
            catch
            {
                return View();
            }
        }

        // GET: NhanVien/Delete/5
        public ActionResult xoa_nhan_vien(int? id)
        {
            var D_nv = data.tblNHANVIENs.First(p => p.MANV == id);
            if (D_nv != null)
                return View(D_nv);
            return View("danh_sach");
        }

        // POST: NhanVien/Delete/5
        [HttpPost]
        public ActionResult xoa_nhan_vien(int id, FormCollection collection)
        {
            var delNV = data.tblNHANVIENs.First(p => p.MANV == id);
            data.tblNHANVIENs.DeleteOnSubmit(delNV);
            data.SubmitChanges();
            return RedirectToAction("danh_sach");
        }
    }
}
