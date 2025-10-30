using System;
using System.Collections.Generic;
using System.Linq;

namespace DTO_QLTiemBanhNgot
{
    public class CKhachHang
    {
        private string _maKH;
        private string _tenKH;
        private string _soDienThoai;
        private List<CHoaDon> _lichSuMuaHang;

        public string MaKH
        {
            get { return _maKH; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Mã khách hàng không được để trống.");
                }
                _maKH = value;
            }
        }
        public string TenKH
        {
            get { return _tenKH; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Tên khách hàng không được để trống.");
                _tenKH = value;
            }
        }
        public string SoDienThoai
        {
            get { return _soDienThoai; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Số điện thoại không được để trống.");
                _soDienThoai = value;
            }
        }
        public List<CHoaDon> LichSuMuaHang
        {
            get { return _lichSuMuaHang; }
        }

        public CKhachHang()
        {
            _lichSuMuaHang = new List<CHoaDon>();
        }
        public CKhachHang(string makh, string tenkh, string sodienthoai)
        {
            MaKH = makh;
            TenKH = tenkh;
            SoDienThoai = sodienthoai;

            _lichSuMuaHang = new List<CHoaDon>();
        }

        public int TinhTongSoSanPhamDaMua()
        {
            int tongSoLuong = 0;
            foreach (CHoaDon hd in _lichSuMuaHang)
                tongSoLuong += hd.ChiTietDonHang.Values.Sum();
            return tongSoLuong; 
        }
        public void ThemHoaDon(CHoaDon hoadon)
        {
            if (hoadon != null)
                _lichSuMuaHang.Add(hoadon);
        }
    }
}
    