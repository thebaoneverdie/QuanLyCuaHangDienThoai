using Nhom4_DoAnWeb_CHDT.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.UI;

namespace Nhom4_DoAnWeb_CHDT.Controllers
{
    public class DienThoaiController : Controller
    {
        // GET: DienThoai
        MyDataDataContext data = new MyDataDataContext();

       
        public PartialViewResult HienThiDanhSachTheLoai()
        {
            List<tblLOAIDIENTHOAI> lstLoaiDienThoai = data.tblLOAIDIENTHOAIs.ToList();
           
            return PartialView("HienThiDanhSachTheLoai", lstLoaiDienThoai);
        }
        public ActionResult HienThiTheoLoaiDienThoai(int id, int? page)
        {
            if (page == null) page = 1;
            var dienThoaiTheoLoai = data.tblDIENTHOAIs
                                          .Where(d => d.MALOAI == id)
                                          .ToList();
            int pageSize = 12;
            int pageNum = page ?? 1;
            // Trả về view với danh sách điện thoại theo loại để hiển thị
            return View("danh_sach",dienThoaiTheoLoai.ToPagedList(pageNum, pageSize));
        }
        public ActionResult SapXepTang( int? page)
        {
            if (page == null) page = 1;
            var dienThoaiTheoLoai = data.tblDIENTHOAIs
                                      
                                      .OrderBy(d => d.GIABAN) // Sắp xếp tăng dần theo giá tiền
                                      .ToList();
            int pageSize = 12;
            int pageNum = page ?? 1;
            // Trả về view với danh sách điện thoại theo loại để hiển thị
            return View("danh_sach", dienThoaiTheoLoai.ToPagedList(pageNum, pageSize));
        }
        public ActionResult SapXepGiam(int? page)
        {
            if (page == null) page = 1;
            var dienThoaiTheoLoai = data.tblDIENTHOAIs

                                      .OrderByDescending(d => d.GIABAN) // Sắp xếp tăng dần theo giá tiền
                                      .ToList();
            int pageSize = 12;
            int pageNum = page ?? 1;
            // Trả về view với danh sách điện thoại theo loại để hiển thị
            return View("danh_sach", dienThoaiTheoLoai.ToPagedList(pageNum, pageSize));
        }


        public ActionResult danh_sach(int? page)
        {
            if (page == null) page = 1;
            var all_dt = (from s in data.tblDIENTHOAIs select s).Where(m => m.SLTON > 0).OrderBy(m => m.MALOAI);
            int pageSize = 12;
            int pageNum = page ?? 1;
            return View(all_dt.ToPagedList(pageNum, pageSize));
        }
        public ActionResult danh_sach_QT(int? page)
        {
            if (page == null) page = 1;
            var all_dt = (from s in data.tblDIENTHOAIs select s).Where(m => m.SLTON > 0).OrderBy(m => m.MALOAI);
            int pageSize = 8;
            int pageNum = page ?? 1;
            return View(all_dt.ToPagedList(pageNum, pageSize));
        }
        //Get: KhachHang/Search
        public ActionResult tim_kiem(string searchName, int? page)
        {
            if (page == null) page = 1;
            int pageSize = 12;
            int pageNum = page ?? 1;
            if (searchName == null)
                Session["searchDT"] = null;
            else
                Session["searchDT"] = searchName;
            var search = data.tblDIENTHOAIs.Where(p => p.TENDT.ToLower().Contains(searchName.ToLower()));
            return View(search.ToPagedList(pageNum, pageSize));
        }
        public ActionResult DienThoai(int? page, int maloai)
        {
            maloai = int.Parse(Request.QueryString["MALOAI"]);
            if (string.IsNullOrEmpty(maloai.ToString()))
            {
                maloai = 1;
            }
            if (page == null) page = 1;
            var all_dt = (from s in data.tblDIENTHOAIs select s).Where(m => m.SLTON > 0 && m.MALOAI == maloai);
            int pageSize = 8;
            int pageNum = page ?? 1;
            if (maloai == 1) ViewBag.Filter = "Apple";
            else if(maloai == 2) ViewBag.Filter = "SamSung";
            else if (maloai == 3) ViewBag.Filter = "Oppo";
            else if (maloai == 4) ViewBag.Filter = "Xiaomi";
            else if (maloai == 6) ViewBag.Filter = "Realmi";
            else if (maloai == 8) ViewBag.Filter = "Vivo";
            return View("DienThoai", all_dt.ToPagedList(pageNum, pageSize));

        }
        // GET: DienThoai/Details/5
        public ActionResult thong_tin(int id)
        {
            var D_dt = data.tblDIENTHOAIs.FirstOrDefault(p => p.MADT == id);
            if(D_dt != null)
                return View(D_dt);
            return View("danh_sach");
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
        // GET: DienThoai/Create
        public ActionResult them_moi()
        {
            return View();
        }

        // POST: DienThoai/Create
        [HttpPost]
        public ActionResult them_moi(FormCollection collection, tblDIENTHOAI dt)
        {
            try
            {
                // TODO: Add insert logic here
                var C_tenDT = collection["TENDT"];
                var C_hinh = collection["HINH"];
                var C_giaMua = collection["GIAMUA"];
                var C_giaBan = collection["GIABAN"];
                var C_moTa = collection["MOTA"];
                var C_sLTon = collection["SLTON"];
                var C_maLoai = collection["MALOAI"];

                if (string.IsNullOrEmpty(C_maLoai) || string.IsNullOrEmpty(C_tenDT)
                    || string.IsNullOrEmpty(C_hinh) || string.IsNullOrEmpty(C_giaMua)
                    || string.IsNullOrEmpty(C_giaBan) || string.IsNullOrEmpty(C_moTa)
                    || string.IsNullOrEmpty(C_sLTon))
                {
                    ViewData["Error"] = "Vui lòng điền đầy đủ thông tin!";
                    return this.them_moi();
                }
                else
                {
                    dt.MADT = int.Parse(data.tblDIENTHOAIs.Max(p => p.MADT).ToString()) + 1;
                    dt.TENDT = C_tenDT;
                    dt.HINH = C_hinh;
                    dt.GIAMUA = decimal.Parse(C_giaMua);
                    dt.GIABAN = decimal.Parse(C_giaBan);
                    dt.MOTA = C_moTa;
                    dt.SLTON = int.Parse(C_sLTon);
                    dt.MALOAI = int.Parse(C_maLoai);

                    data.tblDIENTHOAIs.InsertOnSubmit(dt);
                    data.SubmitChanges();
                    return RedirectToAction("danh_sach", "DienThoai");
                }
                return this.them_moi();
            }
            catch
            {
                return View();
            }
        }

        // GET: DienThoai/Edit/5
        public ActionResult sua_thong_tin(int? id)
        {
            var E_dt = data.tblDIENTHOAIs.FirstOrDefault(p => p.MADT == id);
            if(E_dt != null)
                return View(E_dt);
            return View("danh_sach");
        }

        // POST: DienThoai/Edit/5
        [HttpPost]
        public ActionResult sua_thong_tin(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var C_maLoai = collection["MALOAI"];
                var C_tenDT = collection["TENDT"];
                var C_hinh = collection["HINH"];
                var C_giaMua = collection["GIAMUA"];
                var C_giaBan = collection["GIABAN"];
                var C_moTa = collection["MOTA"];
                var C_sLTon = collection["SLTON"];
                var dt = data.tblDIENTHOAIs.First(p => p.MADT == id);
                if (string.IsNullOrEmpty(C_maLoai) || string.IsNullOrEmpty(C_tenDT)
                    || string.IsNullOrEmpty(C_hinh) || string.IsNullOrEmpty(C_giaMua)
                    || string.IsNullOrEmpty(C_giaBan) || string.IsNullOrEmpty(C_moTa)
                    || string.IsNullOrEmpty(C_sLTon))
                {
                    ViewData["Error"] = "Vui lòng điền đầy đủ thông tin!";
                    return this.sua_thong_tin(id);
                }
                else
                {
                    dt.MALOAI = int.Parse(C_maLoai);
                    dt.TENDT = C_tenDT;
                    dt.HINH = C_hinh;
                    dt.GIAMUA = decimal.Parse(C_giaMua);
                    dt.GIABAN = decimal.Parse(C_giaBan);
                    dt.MOTA = C_moTa;
                    dt.SLTON = int.Parse(C_sLTon);
                    UpdateModel(dt);
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

        // GET: DienThoai/Delete/5
        public ActionResult xoa_DT(int id)
        {
            var D_dt = data.tblDIENTHOAIs.FirstOrDefault(p => p.MADT == id);
            if(D_dt != null)
                return View(D_dt);
            return View("danh_sach");
        }

        // POST: DienThoai/Delete/5
        [HttpPost]
        public ActionResult xoa_DT(int id, FormCollection collection)
        {
            var delDT = data.tblDIENTHOAIs.First(p => p.MADT == id);
            data.tblDIENTHOAIs.DeleteOnSubmit(delDT);
            data.SubmitChanges();
            return RedirectToAction("danh_sach");
        }
        
    }
}