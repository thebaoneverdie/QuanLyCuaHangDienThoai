using Nhom4_DoAnWeb_CHDT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Nhom4_DoAnWeb_CHDT.Controllers
{
    public class NCCController : Controller
    {
        // GET: NCC
        MyDataDataContext data = new MyDataDataContext();
        public ActionResult danh_sach()
        {
            var all_NCC = from ncc in data.tblNHACUNGCAPs select ncc;
            return View(all_NCC);
        }
        //Get: LoaiDT/Search
        public ActionResult tim_kiem(string searchName)
        {
            var search = data.tblNHACUNGCAPs.Where(p => p.TENNCC.ToLower().Contains(searchName.ToLower()));
            return View(search);
        }
        // GET: NCC/Details/5
        public ActionResult thong_tin(int id)
        {
            var D_nhacungcap = data.tblNHACUNGCAPs.First(m => m.MANCC == id);
            if(D_nhacungcap != null)
                return View(D_nhacungcap);
            return View("danh_sach");
        }
        // GET: NCC/Create
        public ActionResult them_moi()
        {
            return View();
        }
        
        // POST: NCC/Create
        [HttpPost]
        public ActionResult them_moi(FormCollection collection, tblNHACUNGCAP ncc)
        {
            try
            {
                var C_tenNCC = collection["TENNCC"];
                var C_diachi = collection["DIACHI"];
                var C_dienthoai = collection["DIENTHOAI"];
                var C_email = collection["EMAIL"];
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
                if (string.IsNullOrEmpty(C_tenNCC) || string.IsNullOrEmpty(C_diachi)
                    || string.IsNullOrEmpty(C_dienthoai) || string.IsNullOrEmpty(C_email))
                {
                    ViewData["Error"] = "Vui lòng điền đầy đủ thông tin!";
                    return this.them_moi();
                }
                else
                {
                    ncc.TENNCC = C_tenNCC;
                    ncc.DIACHI = C_diachi;
                    ncc.DIENTHOAI = C_dienthoai;
                    ncc.EMAIL = C_email;

                    data.tblNHACUNGCAPs.InsertOnSubmit(ncc);
                    data.SubmitChanges();
                    return RedirectToAction("danh_sach");
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: NCC/Edit/5
        public ActionResult sua_thong_tin(int? id)
        {
            var ncc = data.tblNHACUNGCAPs.First(m => m.MANCC == id);
            if(ncc != null)
                return View(ncc);
            return View("danh_sach");
        }

        // POST: NCC/Edit/5
        [HttpPost]
        public ActionResult sua_thong_tin(int id, FormCollection collection)
        {
            try
            {
                var ncc = data.tblNHACUNGCAPs.First(p => p.MANCC == id);
                var E_tenncc = collection["TENNCC"];
                var E_diachi = collection["DIACHI"];
                var E_dienthoai = collection["DIENTHOAI"];
                var E_email = collection["EMAIL"];
                if (!Regex.IsMatch(E_dienthoai, "^0\\d{9}$"))
                {
                    ViewData["SDTKhongHopLe"] = "Số điện thoại không hợp lệ!";
                    return this.them_moi();
                }
                if (!Regex.IsMatch(E_email, @"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$"))
                {
                    ViewData["EmailKhongHopLe"] = "Địa chỉ email không hợp lệ!";
                    return this.them_moi();
                }
                if (string.IsNullOrEmpty(E_tenncc) || string.IsNullOrEmpty(E_diachi)
                    || string.IsNullOrEmpty(E_dienthoai) || string.IsNullOrEmpty(E_email))
                {
                    ViewData["Error"] = "Vui lòng điền đầy đủ thông tin!";
                    return this.sua_thong_tin(id);
                }
                else
                {
                    ncc.TENNCC = E_tenncc;
                    ncc.DIACHI = E_diachi;
                    ncc.DIENTHOAI = E_dienthoai;
                    ncc.EMAIL = E_email;

                    UpdateModel(ncc);
                    data.SubmitChanges();
                    return RedirectToAction("danh_sach");
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: NCC/Delete/5
        public ActionResult xoa_NCC(int? id)
        {
            var search = data.tblNHACUNGCAPs.First(p => p.MANCC == id);
            if (search != null)
                return View(search);
            return View("danh_sach");
        }

        // POST: NCC/Delete/5
        [HttpPost]
        public ActionResult xoa_NCC(int id, FormCollection collection)
        {
            var delNCC = data.tblNHACUNGCAPs.First(p => p.MANCC == id);
            data.tblNHACUNGCAPs.DeleteOnSubmit(delNCC);
            data.SubmitChanges();
            return RedirectToAction("danh_sach");
        }
    }
}
