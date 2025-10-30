using System;

namespace DTO_QLTiemBanhNgot
{
    public class CBanhNgot : CSanPham, ITroGia
    {
        private string _cachBaoQuan;
        public string CachBaoQuan
        {
            get { return _cachBaoQuan; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Cách bảo quản không được để trống.");
                _cachBaoQuan = value;
            }
        }

        public CBanhNgot() : base() { }
        public CBanhNgot(string masanpham, string tenbanh, DateTime ngaysanxuat, DateTime hansudung, double giaban, string cachbaoquan) : base(masanpham, tenbanh, ngaysanxuat, hansudung, giaban)
        {
            CachBaoQuan = cachbaoquan;
        }

        public double TinhTienTroGia()
        {
            return 1000;
        }
        public override double TinhTienGiamGia()
        {
            if ((HanSuDung.Date - DateTime.Now.Date).TotalDays <= 2)
                return GiaBan * 0.30;
            else
                return 0;
        }
    }
}
