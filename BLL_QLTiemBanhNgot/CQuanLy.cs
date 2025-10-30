using DAL_QLTiemBanhNgot;
using DTO_QLTiemBanhNgot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL_QLTiemBanhNgot
{
    public class CQuanLy
    {
        private List<CSanPham> _danhSachSanPham;
        private List<CKhachHang> _danhSachKhachHang;
        private CXmlData _xuLyFile;

        public CQuanLy()
        {
            _danhSachSanPham = new List<CSanPham>();
            _danhSachKhachHang = new List<CKhachHang>();
            _xuLyFile = new CXmlData();
            DocDuLieu();
        }

        private void DocDuLieu()
        {
            try
            {
                _danhSachSanPham = _xuLyFile.DocDanhSachSanPham(@"../../../Data/Banh.xml");
                _danhSachKhachHang = _xuLyFile.DocDanhSachKhachHang(@"../../../Data/KhachHang.xml", _danhSachSanPham).ToList();
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                Console.WriteLine("LỖI: Không tìm thấy Data.");
                Console.WriteLine("Chương trình sẽ tiếp tục với danh sách rỗng.");
            }
            catch (System.IO.FileNotFoundException fnfEx)
            {
                Console.WriteLine($"LỖI: Không tìm thấy file dữ liệu '{fnfEx.FileName}'.");
                Console.WriteLine("Chương trình sẽ tiếp tục với danh sách rỗng.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LỖI NGHIÊM TRỌNG KHI ĐỌC FILE: {ex.Message}");
                Console.WriteLine("Chương trình sẽ tiếp tục với danh sách rỗng.");
            }
        }
        private void LuuDanhSachSanPham()
        {
            try
            {
                _xuLyFile.LuuDanhSachSanPham(@"../../../Data/Banh.xml", _danhSachSanPham);
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                Console.WriteLine(@"LỖI LƯU FILE SP: Không tìm thấy thư mục ""Data"" để lưu.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LỖI KHI LƯU FILE SP: {ex.Message}");
            }
        }
        private string TaoMaSanPhamMoi(Type loaiBanh)
        {
            string maLonNhat = string.Empty;
            string maLoai = string.Empty;
            int soHienTai = 0;

            if (loaiBanh == typeof(CBanhNgot))
            {
                maLoai = "BN";
                maLonNhat = _danhSachSanPham.OfType<CBanhNgot>().Select(sp => sp.MaSP).DefaultIfEmpty("BN00").Max();
            }
            else if (loaiBanh == typeof(CBanhKem))
            {
                maLoai = "BK";
                maLonNhat = _danhSachSanPham.OfType<CBanhKem>().Select(sp => sp.MaSP).DefaultIfEmpty("BK00").Max();
            }
            else if (loaiBanh == typeof(CBanhHandmade))
            {
                maLoai = "BH";
                maLonNhat = _danhSachSanPham.OfType<CBanhHandmade>().Select(sp => sp.MaSP).DefaultIfEmpty("BH00").Max();
            }

            soHienTai = int.Parse(maLonNhat.Substring(2));
            int soTiepTheo = soHienTai + 1;
            return maLoai + soTiepTheo.ToString("D2");
        }
        public void ThemSanPham(CSanPham sanphammoi)
        {
            if (sanphammoi == null)
                throw new ArgumentNullException("Thông tin sản phẩm không được rỗng.");

            bool tenDaTonTai = _danhSachSanPham.Any(sp => sp.GetType() == sanphammoi.GetType() && sp.TenBanh.Equals(sanphammoi.TenBanh, StringComparison.OrdinalIgnoreCase));


            if (tenDaTonTai)
                throw new ArgumentException($"Lỗi: Tên bánh '{sanphammoi.TenBanh}' đã tồn tại cho loại bánh này.");

            string maSPMoi = TaoMaSanPhamMoi(sanphammoi.GetType());
            sanphammoi.MaSP = maSPMoi;

            _danhSachSanPham.Add(sanphammoi);
            LuuDanhSachSanPham();
        }
        public List<CSanPham> LayTatCaSanPham()
        {
            return _danhSachSanPham;
        }
        public List<CSanPham> TimSanPhamTheoTen(string ten)
        {
            return _danhSachSanPham.Where(sp => string.Equals(sp.TenBanh, ten, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        public List<CKhachHang> TimKhachHangTheoTen(string tenkh)
        {
            return _danhSachKhachHang.Where(kh => string.Equals(kh.TenKH, tenkh, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        public HashSet<string> LaySanPhamDaMuaCuaKhachHang(CKhachHang khachHang)
        {
            HashSet<string> lsttmp = new HashSet<string>();

            foreach (CHoaDon hd in khachHang.LichSuMuaHang)
            {
                foreach (CSanPham sp in hd.ChiTietDonHang.Keys)
                {
                    lsttmp.Add(sp.TenBanh);
                }
            }

            return lsttmp;
        }
        public List<CHoaDon> LayDanhSachHoaDon(CKhachHang khachHang)
        {
            List<CHoaDon> lsttmp = new List<CHoaDon>();

            foreach (CHoaDon hd in khachHang.LichSuMuaHang)
            {
                string hoadon = hd.MaHD;
                DateTime ngaymua = hd.NgayMua;

                Dictionary<CSanPham, int> chiTiet = hd.ChiTietDonHang;

                CHoaDon hoadonMoi = new CHoaDon(hoadon, ngaymua, chiTiet);

                lsttmp.Add(hoadonMoi);
            }

            return lsttmp;
        }
        public void CapNhatGiaBanhHandmade()
        {
            List<CBanhHandmade> dsBanhHandmade = _danhSachSanPham.OfType<CBanhHandmade>().ToList();

            foreach (CBanhHandmade sp in dsBanhHandmade)
            {
                
                sp.GiaBan = Math.Round(sp.GiaBan * 1.03);
            }

            LuuDanhSachSanPham();
        }
        public List<CBanhNgot> LayDanhSachBanhNgot()
        {
            return _danhSachSanPham.OfType<CBanhNgot>().ToList();
        }
        public List<CBanhKem> LayDanhSachBanhKem()
        {
            return _danhSachSanPham.OfType<CBanhKem>().ToList();
        }
        public List<CBanhHandmade> LayDanhSachBanhHandmade()
        {
            return _danhSachSanPham.OfType<CBanhHandmade>().ToList();
        }
        public List<CSanPham> LayBanhGiaTren150Ngan()
        {
            return _danhSachSanPham.Where(sp => sp.GiaBan > 150000).ToList();
        }
        public List<CKhachHang> LayKhachHangMuaNhieuHonBaSP()
        {
            return _danhSachKhachHang.Where(kh => kh.TinhTongSoSanPhamDaMua() > 3).ToList();
        }
        public List<CSanPham> LaySanPhamSanXuatTrenBaThang()
        {
            DateTime baThangTruoc = DateTime.Now.AddMonths(-3);
            return _danhSachSanPham.Where(sp => sp.NgaySanXuat < baThangTruoc).ToList();
        }
    }
}

