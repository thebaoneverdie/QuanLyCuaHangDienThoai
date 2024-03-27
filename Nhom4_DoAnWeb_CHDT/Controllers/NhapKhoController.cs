using Nhom4_DoAnWeb_CHDT.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Nhom4_DoAnWeb_CHDT.Controllers
{
    public class NhapKhoController : Controller
    {
        // GET: NhapKho
        MyDataDataContext data = new MyDataDataContext();

        public ActionResult danh_sach_NK(int? page)
        {
            var all_nk = (from s in data.tblNHAPKHOs select s).OrderBy(m => m.MAPNK);
            int pageSize = 12;
            int pageNum = page ?? 1;
            return View(all_nk.ToPagedList(pageNum, pageSize));
        }
        // GET: NhapKho/Details/5
        public ActionResult thong_tin(int id)
        {
            var find = data.tblNHAPKHOs.First(p => p.MAPNK == id);
            if (find != null)
            {
                var ctnk = data.tblCTPNHAPKHOs.Where(p => p.MAPNK == id).ToList();
                ViewBag.cthd = ctnk;
                return View(find);
            }
            return View("Index", "Home");
        }

        public ActionResult xoa_NK(int id)
        {
            var find = data.tblNHAPKHOs.FirstOrDefault(p => p.MAPNK == id);
            if (find != null)
                return View(find);
            return View("Index", "Home");
        }

        // POST: HoaDon/Delete/5
        [HttpPost]
        public ActionResult xoa_NK(int id, FormCollection collection)
        {
            var delNK = data.tblNHAPKHOs.FirstOrDefault(p => p.MAPNK == id);
            if (delNK != null)
            {
                var ctnk = data.tblCTPNHAPKHOs.Where(p => p.MAPNK == id).ToList();
                foreach (var item in ctnk)
                {
                    var dt = data.tblDIENTHOAIs.FirstOrDefault(p => p.MADT == item.MADT);
                    if (dt != null)
                    {
                        dt.SLTON -= item.SL;
                    }
                }
                ViewBag.ctnk = ctnk;
                data.tblCTPNHAPKHOs.DeleteAllOnSubmit(ctnk);
                data.SubmitChanges();
            }
            data.tblNHAPKHOs.DeleteOnSubmit(delNK);
            data.SubmitChanges();
            return RedirectToAction("danh_sach");
        }
    }
}
