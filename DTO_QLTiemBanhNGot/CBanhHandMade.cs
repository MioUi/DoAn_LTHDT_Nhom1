using System;

namespace DTO_QLTiemBanhNgot
{
    public class CBanhHandmade : CSanPham, ITroGia
    {
        private string _dongBanh;
        public string DongBanh
        {
            get { return _dongBanh; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Dòng bánh không được để trống.");
                _dongBanh = value;
            }
        }

        public CBanhHandmade() : base() { }
        public CBanhHandmade(string masanpham, string tenbanh, DateTime ngaysanxuat, DateTime hansudung, double giaban, string dongbanh) : base(masanpham, tenbanh, ngaysanxuat, hansudung, giaban)
        {
            DongBanh = dongbanh;
        }

        public override void HienThiThongTin()
        {
            base.HienThiThongTin();
            Console.WriteLine($"Dòng bánh: {DongBanh}");
        }

        public double TinhTienTroGia()
        {
            return 2000;
        }
        public override double TinhTienGiamGia()
        {
            return 0;
        }
    }
}
