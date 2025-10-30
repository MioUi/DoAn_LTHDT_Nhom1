using System;

namespace DTO_QLTiemBanhNgot
{
    public class CBanhKem : CSanPham
    {
        private int _kichThuoc;
        public int KichThuoc
        {
            get { return _kichThuoc; }
            set
            {
                if (value < 18)
                    throw new ArgumentException("Kích thước bánh kem phải hợp lệ (từ 18cm trở lên).");
                _kichThuoc = value;
            }
        }

        public CBanhKem() : base() { }
        public CBanhKem(string masanpham, string tenbanh, DateTime ngaysanxuat, DateTime hansudung, double giaban, int kichthuoc) : base(masanpham, tenbanh, ngaysanxuat, hansudung, giaban)
        {
            KichThuoc = kichthuoc;
        }

        public override double TinhTienGiamGia()
        {
            if (KichThuoc >= 28)
                return GiaBan * 0.07;
            else
                return 0;
        }
    }
}
