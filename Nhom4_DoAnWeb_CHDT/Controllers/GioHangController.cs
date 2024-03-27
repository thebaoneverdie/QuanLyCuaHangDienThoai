using Nhom4_DoAnWeb_CHDT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Nhom4_DoAnWeb_CHDT.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        MyDataDataContext data = new MyDataDataContext();

        public PartialViewResult HienThiGioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = Tongtien();
            return PartialView("HienThiGioHang", lstGiohang);
        }
        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang == null)
            {
                lstGiohang = new List<Giohang>();
                Session["GioHang"] = lstGiohang;
            }
            return lstGiohang;
        }
        public ActionResult ThemGioHang(int id, string strUrl)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.Find(n => n.maDT == id);
            if (sanpham == null)
            {
                sanpham = new Giohang(id);
                lstGiohang.Add(sanpham);
                return Redirect(strUrl);
            }
            else
            {
                sanpham.isoluong++;
                return Redirect(strUrl);

            }
        }
        private int TongSoLuong()
        {
            int tsl = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                tsl = lstGiohang.Sum(n => n.isoluong);
            }
            return tsl;
        }
        private int TongSoLuongSanPham()
        {
            int tsl;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            tsl = (lstGiohang != null) ? lstGiohang.Count : 0;
            return tsl;
        }

        public string Tongtien()
        {
            double tt = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                tt = lstGiohang.Sum(n => n.dThanhtien);
            }
            return tt.ToString();
        }
        public string TongtienNhap()
        {
            double ttnp = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                ttnp = lstGiohang.Sum(n => n.dTienNhap);
            }
            return ttnp.ToString();
        }
        //GET: GioHang
        public ActionResult Giohang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = Tongtien();
            ViewBag.TongtienNhap = TongtienNhap();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(lstGiohang);
        }
        //GET: GiohangPartial
        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = Tongtien();
            ViewBag.TongtienNhap = TongtienNhap();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return PartialView();
        }
        // GET: GioHang/Delete/5
        public ActionResult XoaGioHang(int id)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.maDT == id);
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.maDT == id);
                return RedirectToAction("GioHang");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaTatCaGioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("GioHang");
        }
        public ActionResult CapnhatGiohang(int id, FormCollection collection)
        {
            tblTAIKHOAN tk = (tblTAIKHOAN)Session["TaiKhoan"];
            List<Giohang> lstGiohang = Laygiohang();
            var input_sl = int.Parse(collection["txtSoLg"].ToString());
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.maDT == id);
            var s = data.tblDIENTHOAIs.First(m => m.MADT == sanpham.maDT);
            if(tk != null && tk.role == "QT")
            {
                if (sanpham != null && (input_sl > 0))
                    sanpham.isoluong = input_sl;
            }
            else
            {
                if (sanpham != null && (input_sl <= s.SLTON && input_sl > 0))
                    sanpham.isoluong = input_sl;
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapnhatSoluongTon()
        {
            List<Giohang> lstGiohang = Laygiohang();
            foreach (Giohang c in lstGiohang)
            {
                var s = data.tblDIENTHOAIs.First(m => m.MADT == c.maDT);
                s.SLTON -= c.isoluong;
                UpdateModel(s);
            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("trang_chu", "KhoaiTei");
        }
        //Get: DatHang
        [HttpGet]
        public ActionResult ThongTinDatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("trang_chu", "KhoaiTei");
            }
            var list = data.tblNHACUNGCAPs.ToList();
            ViewBag.NHACUNGCAP = new SelectList(list, "MANCC", "TENNCC");
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = Tongtien();
            ViewBag.TongtienNhap = TongtienNhap();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(lstGiohang);
        }
        public ActionResult ThongTinDatHang(FormCollection collection)
        {
            tblHOADON hd = new tblHOADON();
            tblKHACHHANG kh = new tblKHACHHANG();
            tblDIENTHOAI dt = new tblDIENTHOAI();
            tblTAIKHOAN tk = (tblTAIKHOAN)Session["TaiKhoan"];
            tblNHACUNGCAP ncc = new tblNHACUNGCAP();
            List<Giohang> gh = Laygiohang();
            var list = data.tblNHACUNGCAPs.ToList();
          
            if (tk.role == "QT")
            {
                ViewBag.NHACUNGCAP = new SelectList(list, "MANCC", "TENNCC");
                var C_maNCC = collection["MANCC"];
                var find = data.tblNHACUNGCAPs.FirstOrDefault(p => p.MANCC == int.Parse(C_maNCC));
                Session["NCC"] = find;
                return RedirectToAction("NhapKho", "GioHang");
            }
            else
            {
                var hanbaohanh = String.Format("{0:MM/dd/yyyy}", collection["HANBAOHANH"]);
                string phuongThucThanhToan = collection["thanh-toan"];
                var C_tenKH = collection["TENKH"];
                var C_diachi = collection["DIACHI"];
                var C_dienthoai = collection["DIENTHOAI"];
                var C_email = collection["EMAIL"];
                var find = data.tblKHACHHANGs.FirstOrDefault(p => p.EMAIL == C_email && p.DIENTHOAI == C_dienthoai && p.DIACHI == C_diachi && p.TENKH == C_tenKH);
                if (find == null)
                {
                    if (!Regex.IsMatch(C_dienthoai, "^0\\d{9}$"))
                    {
                        ViewData["SDTKhongHopLe"] = "Số điện thoại không hợp lệ!";
                        return this.ThongTinDatHang();
                    }
                    if (!Regex.IsMatch(C_email, @"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$"))
                    {
                        ViewData["EmailKhongHopLe"] = "Địa chỉ email không hợp lệ!";
                        return this.ThongTinDatHang();
                    }
                    if (string.IsNullOrEmpty(C_tenKH) || string.IsNullOrEmpty(C_diachi)
                            || string.IsNullOrEmpty(C_dienthoai) || string.IsNullOrEmpty(C_email))
                    {
                        ViewData["Error"] = "Vui lòng điền đầy đủ thông tin!";
                        return this.ThongTinDatHang();
                    }
                    else
                    {
                        kh.MAKH = data.tblKHACHHANGs.Max(p => p.MAKH) + 1;
                        kh.TENKH = C_tenKH;
                        kh.DIACHI = C_diachi;
                        kh.DIENTHOAI = C_dienthoai;
                        kh.EMAIL = C_email;
                        data.tblKHACHHANGs.InsertOnSubmit(kh);
                        data.SubmitChanges();
                        Session["KhachHang"] = kh;
                    }
                }
                else
                {
                    Session["KhachHang"] = find;
                }
                return RedirectToAction("DatHang", "GioHang");
            }
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = Tongtien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(lstGiohang);
        }
        public ActionResult DatHang(FormCollection collection)
        {
            string thanhtoantienmat = collection["phuongThucThanhToan"];
            string thanhtoanmomo = collection["phuongThucThanhToan"];
            tblHOADON hd = new tblHOADON();
            tblKHACHHANG kh = (tblKHACHHANG)Session["KhachHang"];
            tblDIENTHOAI dt = new tblDIENTHOAI();
            Giohang nv = new Giohang();
            nv.ListNV = data.tblNHANVIENs.ToList();
            tblTAIKHOAN tk = (tblTAIKHOAN)Session["TaiKhoan"];
            List<Giohang> gh = Laygiohang();
            var hanbaohanh = String.Format("{0:MM/dd/yyyy}", collection["HANBAOHANH"]);
            var C_maNV = collection["MANV"];
            string phuongThucThanhToan = collection["thanh-toan"];
            hd.MAKH = kh.MAKH;
            hd.NGAYLAP = DateTime.Now;
            hd.HANBAOHANH = hd.NGAYLAP.AddYears(1);
            hd.TONGTG = decimal.Parse(Tongtien());
            if (tk.role == "NV")
                hd.MANV = int.Parse(C_maNV);
            else
                hd.MANV = 1;
            data.tblHOADONs.InsertOnSubmit(hd);
            data.SubmitChanges();
            foreach (var item in gh)
            {
                tblCTHD cthd = new tblCTHD();
                cthd.MAHD = hd.MAHD;
                cthd.MADT = item.maDT;
                cthd.SLMUA = item.isoluong;
                cthd.GIABAN = (decimal)item.giaban;
                if (thanhtoantienmat == "tien-mat")
                {
                    dt = data.tblDIENTHOAIs.Single(p => p.MADT == item.maDT);
                    if (dt.SLTON < cthd.SLMUA)
                    {

                        TempData["Error"] = "Sản phẩm không đủ hoặc hết hàng";
                        return RedirectToAction("DatHang", "GioHang");

                    }
                    else
                    {
                        dt.SLTON -= cthd.SLMUA;
                    }
                }
                        data.tblCTHDs.InsertOnSubmit(cthd);
                data.SubmitChanges();
            }
            if (thanhtoanmomo == "momo")
                return RedirectToAction("Payment", "Momo");
            Session["Giohang"] = null;
            return View("XacNhanThanhToan");
        }
        public ActionResult XacNhanThanhToan()
        {
            return View();
        }
        [HttpGet]
        public ActionResult NhapKho()
        {
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.TongtienNhap = TongtienNhap();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(lstGiohang);
        }
        public ActionResult NhapKho(FormCollection collection)
        {
            tblNHAPKHO nk = new tblNHAPKHO();
            var C_maNCC = collection["MANCC"];
            tblNHACUNGCAP ncc = (tblNHACUNGCAP)Session["NCC"];
            tblDIENTHOAI dt = new tblDIENTHOAI();
            List<Giohang> gh = Laygiohang();
            if (ncc != null)
            {
                nk.MANCC = ncc.MANCC;
                nk.NGAYNHAP = DateTime.Now;
                nk.TONGTG = decimal.Parse(TongtienNhap());
                nk.MANV = 1;
                data.tblNHAPKHOs.InsertOnSubmit(nk);
                data.SubmitChanges();
                foreach (var item in gh)
                {
                    tblCTPNHAPKHO ctnk = new tblCTPNHAPKHO();
                    ctnk.MAPNK = nk.MAPNK;
                    ctnk.MADT = item.maDT;
                    ctnk.SL = item.isoluong;
                    ctnk.GIANHAP = (decimal)item.gianhap;
                    dt = data.tblDIENTHOAIs.Single(p => p.MADT == item.maDT);
                    dt.SLTON += ctnk.SL;
                    data.tblCTPNHAPKHOs.InsertOnSubmit(ctnk);
                    data.SubmitChanges();
                }
            }
            Session["Giohang"] = null;
            return View("XacNhanThanhToan");
        }
        public ActionResult Clear()
        {
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("trang_chu", "Home");
        }
    }

}
