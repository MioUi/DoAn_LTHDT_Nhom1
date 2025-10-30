using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DTO_QLTiemBanhNgot
{
    public class CHoaDon
    {
        private string _maHD;
        private DateTime _ngayMua;
        private Dictionary<CSanPham, int> _chiTietDonHang;

        public string MaHD
        {
            get { return _maHD; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Mã hóa đơn không được để trống.");
                _maHD = value;
            }
        }
        public DateTime NgayMua
        {
            get { return _ngayMua; }
            set
            {
                if (value.Date > DateTime.Now.Date)
                    throw new ArgumentException("Ngày mua (ngày lập hóa đơn) không được ở tương lai.");
                _ngayMua = value;
            }
        }
        public Dictionary<CSanPham, int> ChiTietDonHang
        {
            get { return _chiTietDonHang; }
        }
        public CHoaDon()
        {
            _chiTietDonHang = new Dictionary<CSanPham, int>();
            NgayMua = DateTime.Now;
        }

        public CHoaDon(string mahd, DateTime ngaymua, Dictionary<CSanPham, int> chitiethoadoncu)
        {
            MaHD = mahd;
            NgayMua = ngaymua;
            _chiTietDonHang = chitiethoadoncu;
        }
        public double TinhTienTroGiaMotSP(CSanPham obj)
        {
            double tienTroGiaSP = 0;
            if (obj is ITroGia troGiaObject)
            {
                tienTroGiaSP += troGiaObject.TinhTienTroGia();
            }
            return tienTroGiaSP;
        }
        public double TinhTongKhuyenMaiMotSP(CSanPham sp)
        {
            return sp.TinhTienGiamGia() + TinhTienTroGiaMotSP(sp);
        }
        public double TinhThanhTien()
        {
            double tongTien = 0;
            double tongTienBanhHandmade = 0;
            int soLuongBanhHandmade = 0;

            foreach (KeyValuePair<CSanPham, int> muc in _chiTietDonHang)
            {
                CSanPham obj = muc.Key;
                int soluong = muc.Value;

                double tongKhuyenMaiSP = TinhTongKhuyenMaiMotSP(obj);

                double giaSauKhuyenMai = obj.GiaBan - tongKhuyenMaiSP;
                if (giaSauKhuyenMai < 0) 
                    giaSauKhuyenMai = 0;

                double thanhTienCuaMuc = giaSauKhuyenMai * soluong;
                if (obj is CBanhHandmade)
                {
                    tongTienBanhHandmade += thanhTienCuaMuc;
                    soLuongBanhHandmade += soluong;
                }
                else
                    tongTien += thanhTienCuaMuc;
            }

            if (soLuongBanhHandmade >= 5)
                tongTienBanhHandmade *= 0.95;

            return tongTien + tongTienBanhHandmade;
        }
    }
}
