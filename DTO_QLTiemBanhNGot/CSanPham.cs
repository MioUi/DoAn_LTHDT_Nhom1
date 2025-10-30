using System;

namespace DTO_QLTiemBanhNgot
{
    public abstract class CSanPham
    {
        private string _maSP;
        private string _tenBanh;
        private DateTime _ngaySanXuat;
        private DateTime _hanSuDung;
        private double _giaBan;

        public string MaSP
        {
            get { return _maSP; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Mã sản phẩm không được để trống.");
                else
                    _maSP = value;
            }
        }
        public string TenBanh
        {
            get { return _tenBanh; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Tên bánh không được để trống.");
                else
                    _tenBanh = value;
            }
        }
        public DateTime NgaySanXuat
        {
            get { return _ngaySanXuat; }
            set
            {
                if (value.Date > DateTime.Now.Date)
                    throw new ArgumentException("Ngày sản xuất không được quá thời điểm hiện tại.");
                else
                    _ngaySanXuat = value;
            }
        }
        public DateTime HanSuDung
        {
            get { return _hanSuDung; }
            set
            {
                if (value.Date <= this.NgaySanXuat.Date)
                    throw new ArgumentException("Hạn sử dụng phải sau ngày sản xuất.");
                else
                    _hanSuDung = value;
            }
        }
        public double GiaBan
        {
            get { return _giaBan; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Giá bán phải là số dương.");
                else
                    _giaBan = value;
            }
        }

        public CSanPham() { }
        public CSanPham(string masp, string tenbanh, DateTime ngaysanxuat, DateTime hansudung, double giaban)
        {
            _maSP = masp;
            TenBanh = tenbanh;
            NgaySanXuat = ngaysanxuat;
            HanSuDung = hansudung;
            GiaBan = giaban;
        }

        public abstract double TinhTienGiamGia();
    }
}
