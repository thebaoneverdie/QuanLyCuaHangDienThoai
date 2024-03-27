using Nhom4_DoAnWeb_CHDT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nhom4_DoAnWeb_CHDT.Controllers
{
    public class LoaiDTController : Controller
    {
        // GET: LoaiDT
        MyDataDataContext data = new MyDataDataContext();
        public ActionResult danh_sach()
        {
            var all_LoaiDT = from lDT in data.tblLOAIDIENTHOAIs select lDT;
            return View(all_LoaiDT);
        }
        //Get: LoaiDT/Search

        public ActionResult tim_kiem(string searchName)
        {
            var search = data.tblLOAIDIENTHOAIs.Where(p => p.TENLOAI.ToLower().Contains(searchName.ToLower()));
            return View(search);
        }
        // GET: LoaiDT/Details/5
        public ActionResult thong_tin(int id)
        {
            var D_LoaiDT = data.tblLOAIDIENTHOAIs.First(p => p.MALOAI == id);
            if(D_LoaiDT != null)
                return View(D_LoaiDT);
            return HttpNotFound();
        }

        // GET: LoaiDT/Create
        public ActionResult them_moi()
        {
            return View();
        }

        // POST: LoaiDT/Create
        [HttpPost]
        public ActionResult them_moi(FormCollection collection, tblLOAIDIENTHOAI lDT)
        {
            try
            {
                // TODO: Add insert logic here
                var ten = collection["TENLOAI"];
                if (string.IsNullOrEmpty(ten))
                {
                    ViewData["Error"] = "Vui lòng điền đầy đủ thông tin!";
                    return this.them_moi();
                }
                else
                {
                    lDT.TENLOAI = ten;
                    data.tblLOAIDIENTHOAIs.InsertOnSubmit(lDT);
                    data.SubmitChanges();
                    return RedirectToAction("danh_sach");
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: LoaiDT/Edit/5
        public ActionResult sua_thong_tin(int id)
        {
            var search = data.tblLOAIDIENTHOAIs.First(p => p.MALOAI == id);
            if(search != null)
                return View(search);
            return View("danh_sach");
        }

        // POST: LoaiDT/Edit/5
        [HttpPost]
        public ActionResult sua_thong_tin(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var loaiDT = data.tblLOAIDIENTHOAIs.First(p => p.MALOAI == id);
                var tenloai = collection["TENLOAI"];
                if (string.IsNullOrEmpty(tenloai))
                {
                    ViewData["Error"] = "Vui lòng điền đầy đủ thông tin!";
                    return this.sua_thong_tin(id);
                }
                else
                {
                    loaiDT.TENLOAI = tenloai;
                    UpdateModel(loaiDT);
                    data.SubmitChanges();
                    return RedirectToAction("danh_sach");
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: LoaiDT/Delete/5
        public ActionResult xoa_LoaiDT(int id)
        {
            var search = data.tblLOAIDIENTHOAIs.First(p => p.MALOAI == id);
            if (search != null)
                return View(search);
            return View("danh_sach");
        }

        // POST: LoaiDT/Delete/5
        [HttpPost]
        public ActionResult xoa_LoaiDT(int id, FormCollection collection)
        {
            var delLDT = data.tblLOAIDIENTHOAIs.First(p => p.MALOAI == id);
            data.tblLOAIDIENTHOAIs.DeleteOnSubmit(delLDT);
            data.SubmitChanges();
            return RedirectToAction("danh_sach");
        }
    }
}
