using Nhom4_DoAnWeb_CHDT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
namespace Nhom4_DoAnWeb_CHDT.Controllers
{
    public class KhoaiTeiController : Controller
    {
        MyDataDataContext data = new MyDataDataContext();
        public ActionResult trang_chu()
        {
            var all_sach = (from s in data.tblDIENTHOAIs select s).Where(m => m.SLTON > 0).OrderBy(m => m.MALOAI);
            return View(all_sach);
        }
        
        public ActionResult gioi_thieu()
        {
            return View();
        }

        public ActionResult dich_vu_sua_chua()
        {
            return View();
        }
        
    }
}