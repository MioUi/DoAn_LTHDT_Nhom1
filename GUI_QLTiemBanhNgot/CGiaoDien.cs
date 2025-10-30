using BLL_QLTiemBanhNgot;
using DTO_QLTiemBanhNgot;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace GUI_QLTiemBanhNgot
{
    public class CGiaoDien
    {
        static string dinhDangNgay = "dd/MM/yyyy";

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            CQuanLy quanLy = new CQuanLy();

            bool dangChay = true;
            while (dangChay)
            {
                Console.Clear();
                Console.WriteLine(new string('=', 66));
                XuatTieuDeCanGiua($"CHƯƠNG TRÌNH QUẢN LÝ TIỆM BÁNH NGỌT", 66);
                Console.WriteLine(new string('=', 66));
                Console.WriteLine("1. Thêm một sản phẩm mới");
                Console.WriteLine("2. Xuất danh sách tất cả sản phẩm");
                Console.WriteLine("3. Xuất danh sách sản phẩm (theo loại sản phẩm)");
                Console.WriteLine("4. Tìm sản phẩm theo tên bánh");
                Console.WriteLine("5. Xuất sản phẩm đã mua (theo tên khách hàng)");
                Console.WriteLine("6. Xuất toàn bộ hoá đơn (theo tên khách hàng)");
                Console.WriteLine("7. Cập nhật giá bánh Handmade (tăng 3%)");
                Console.WriteLine("8. Xuất danh sách bánh có giá bán trên 150.000 VND");
                Console.WriteLine("9. In khách hàng đã mua nhiều hơn 3 sản phẩm");
                Console.WriteLine("10. In sản phẩm có ngày sản xuất trên 3 tháng");
                Console.WriteLine("0. Thoát chương trình");
                Console.Write("Vui lòng chọn chức năng: ");

                string luaChon = Console.ReadLine();

                switch (luaChon)
                {
                    case "1":
                        Console.Clear();
                        MenuThemSanPham(quanLy);
                        break;
                    case "2":
                        Console.Clear();
                        MenuXuatDanhSachSanPham(quanLy);
                        break;
                    case "3":
                        Console.Clear();
                        MenuXuatDanhSachSanPhamTheoLoai(quanLy);
                        break;
                    case "4":
                        Console.Clear();
                        MenuTimSanPhamTheoTen(quanLy);
                        break;
                    case "5":
                        Console.Clear();
                        MenuXuatSanPhamCuaKhachHang(quanLy);
                        break;
                    case "6":
                        Console.Clear();
                        MenuXuatHoaDonCuaKhachHang(quanLy);
                        break;
                    case "7":
                        Console.Clear();
                        MenuCapNhatGiaHandmade(quanLy);
                        break;
                    case "8":
                        Console.Clear();
                        MenuXuatBanhGiaTren150Ngan(quanLy);
                        break;
                    case "9":
                        Console.Clear();
                        MenuXuatKhachHangMuaNhieuHonBaSP(quanLy);
                        break;
                    case "10":
                        Console.Clear();
                        MenuXuatSanPhamSanXuatTrenBaThang(quanLy);
                        break;
                    case "0":
                        dangChay = false;
                        Console.WriteLine("Đang thoát chương trình...");
                        break;
                    default:
                        Console.WriteLine("LỖI: Lựa chọn không hợp lệ. Vui lòng chọn lại.");
                        break;
                }

                if (dangChay)
                {
                    Console.WriteLine("\nNhấn Enter để tiếp tục...");
                    Console.ReadLine();
                }
            }
        }
        static void MenuThemSanPham(CQuanLy quanly)
        {
            XuatTieuDeCanGiua("Thêm Sản Phẩm Mới", 66);
            Console.WriteLine("Chọn loại bánh:");
            Console.WriteLine("1. Bánh Ngọt");
            Console.WriteLine("2. Bánh Kem");
            Console.WriteLine("3. Bánh Handmade");
            Console.Write("Loại: ");
            string luachon = Console.ReadLine();

            string maTamThoi = string.Empty;

            while (true)
            {
                CSanPham sanPhamMoi = null;

                Console.Write("Tên bánh: ");
                string ten = NhapChuoiHopLe("Tên bánh");

                Console.Write($"Ngày sản xuất ({dinhDangNgay}): ");
                DateTime nsx = NhapNSXHopLe();
                Console.Write($"Hạn sử dụng ({dinhDangNgay}): ");
                DateTime hsd = NhapHSDHopLe(nsx);
                Console.Write("Giá bán: ");
                double gia = NhapSoThucHopLe("Giá bán");

                try
                {
                    switch (luachon)
                    {
                        case "1":
                            Console.Write("Cách bảo quản (Lạnh/Không lạnh): ");
                            string baoquan = NhapChuoiHopLe("Cách bảo quản");
                            sanPhamMoi = new CBanhNgot(maTamThoi, ten, nsx, hsd, gia, baoquan);
                            break;
                        case "2":
                            Console.Write("Kích thước (cm, vd: 18, 20...): ");
                            int kichthuoc = NhapSoNguyenHopLe("Kích thước");
                            sanPhamMoi = new CBanhKem(maTamThoi, ten, nsx, hsd, gia, kichthuoc);
                            break;
                        case "3":
                            Console.Write("Dòng bánh (Singapore, Đài Loan,...): ");
                            string dongbanh = NhapChuoiHopLe("Dòng bánh");
                            sanPhamMoi = new CBanhHandmade(maTamThoi, ten, nsx, hsd, gia, dongbanh);
                            break;
                        default:
                            Console.WriteLine("LỖI: Loại bánh không hợp lệ.");
                            return;
                    }

                    quanly.ThemSanPham(sanPhamMoi);
                    Console.WriteLine($"Đã thêm thành công sản phẩm: {sanPhamMoi.TenBanh} (Mã: {sanPhamMoi.MaSP})");
                    break;
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine($"\n--- LỖI THÊM SẢN PHẨM ---");
                    Console.WriteLine($"{ae.Message}");
                    Console.WriteLine("Vui lòng nhập lại thông tin sản phẩm.\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n--- LỖI KHÔNG XÁC ĐỊNH ---");
                    Console.WriteLine($"Lỗi: {ex.Message}");
                    Console.WriteLine("Vui lòng kiểm tra lại thông tin đã nhập.\n");
                }
            }
        }
        static void MenuXuatDanhSachSanPham(CQuanLy quanly)
        {
            List<CSanPham> dssp = quanly.LayTatCaSanPham();
            XuatTieuDeCanGiua("DANH SÁCH TẤT CẢ SẢN PHẨM", 66);
            XuatDanhSachPhanLoai(dssp);
        }
        static void MenuXuatDanhSachSanPhamTheoLoai(CQuanLy quanly)
        {
            XuatTieuDeCanGiua("XUẤT SẢN PHẨM THEO LOẠI", 66);
            Console.WriteLine("\nChọn loại bánh muốn xem:");
            Console.WriteLine("1. Bánh Ngọt");
            Console.WriteLine("2. Bánh Kem");
            Console.WriteLine("3. Bánh Handmade");
            Console.Write("Lựa chọn của bạn: ");
            string luaChonLoai = Console.ReadLine();

            List<CSanPham> dsKetQua = new List<CSanPham>();

            switch (luaChonLoai)
            {
                case "1":
                    dsKetQua = quanly.LayDanhSachBanhNgot().ToList<CSanPham>();
                    XuatTieuDeCanGiua("\nDANH SÁCH BÁNH NGỌT", 66);
                    break;
                case "2":
                    dsKetQua = quanly.LayDanhSachBanhKem().ToList<CSanPham>();
                    XuatTieuDeCanGiua("\nDANH SÁCH BÁNH KEM", 66);
                    break;
                case "3":
                    dsKetQua = quanly.LayDanhSachBanhHandmade().ToList<CSanPham>();
                    XuatTieuDeCanGiua("\nDANH SÁCH BÁNH HANDMADE", 66);
                    break;
                default:
                    Console.WriteLine("Lựa chọn loại bánh không hợp lệ.");
                    return;
            }
            XuatDanhSachPhanLoai(dsKetQua);
        }
        static void MenuTimSanPhamTheoTen(CQuanLy quanly)
        {
            XuatTieuDeCanGiua("TÌM KIẾM SẢN PHẨM (THEO TÊN SẢN PHẨM)", 66);

            Console.Write("\nNhập tên sản phẩm cần tìm: ");
            string tenTim = NhapChuoiHopLe("Tên sản phẩm");

            List<CSanPham> lstobj = quanly.TimSanPhamTheoTen(tenTim);

            XuatTieuDeCanGiua($"\nKẾT QUẢ TÌM KIẾM CHO: '{tenTim}'", 66);

            XuatDanhSachPhanLoai(lstobj);
        }
        static void MenuXuatSanPhamCuaKhachHang(CQuanLy quanly)
        {
            XuatTieuDeCanGiua($"TÌM LỊCH SỬ MUA HÀNG CỦA KHÁCH HÀNG", 66);

            Console.Write("\nNhập tên khách hàng cần xem lịch sử: ");
            string tenkh = NhapChuoiHopLe("Tên khách hàng");
            List<CKhachHang> lstobj = quanly.TimKhachHangTheoTen(tenkh);


            if (lstobj.Count == 0)
            {
                Console.WriteLine($"Không tìm thấy khách hàng {tenkh} trong danh sách.");
                return;
            }

            foreach (CKhachHang obj in lstobj)
            {
                XuatTieuDeCanGiua($"\nCÁC SẢN PHẨM ĐÃ MUA CỦA: {tenkh} | {obj.MaKH}", 66);

                HashSet<string> dsTenBanhDaMua = quanly.LaySanPhamDaMuaCuaKhachHang(obj);

                if (dsTenBanhDaMua.Count == 0)
                {
                    Console.WriteLine("Khách hàng này chưa mua sản phẩm nào.");
                    return;
                }

            foreach (string tenBanh in dsTenBanhDaMua)
                {
                    Console.WriteLine($"- {tenBanh}");
                }
            }
        }
        static void MenuXuatHoaDonCuaKhachHang(CQuanLy quanly)
        {
            XuatTieuDeCanGiua($"TÌM LỊCH SỬ MUA HÀNG CỦA KHÁCH HÀNG", 66);

            Console.Write("\nNhập tên khách hàng cần xem lịch sử: ");
            string tenkh = NhapChuoiHopLe("Tên khách hàng");

            List<CKhachHang> lstkh = quanly.TimKhachHangTheoTen(tenkh);

            if (lstkh.Count == 0)
            {
                Console.WriteLine($"Không tìm thấy khách hàng {tenkh} trong danh sách.");
                return;
            }

            foreach (CKhachHang kh in lstkh)
            {
                Console.WriteLine($"\n=============================================");
                Console.WriteLine($"LỊCH SỬ MUA HÀNG CỦA KHÁCH: {kh.TenKH} ({kh.MaKH})");
                Console.WriteLine($"=============================================");

                if (kh.LichSuMuaHang.Count == 0)
                {
                    Console.WriteLine("Khách hàng này chưa có lịch sử mua hàng.");
                    continue;
                }

                foreach (CHoaDon hd in kh.LichSuMuaHang)
                {
                    Console.WriteLine($"\n--- Hóa Đơn: {hd.MaHD} --- Ngày: {hd.NgayMua.ToShortDateString()} ---");

                    Console.WriteLine($"{"Tên Bánh",-20} | {"SL",3} | {"Đơn Giá",10} | {"Tổng Tiền",12} | {"Khuyến Mãi",12}");
                    Console.WriteLine(new string('-', 66));

                    double tongGiaTriDon = 0;
                    double tongKhuyenMai = 0;

                    foreach (KeyValuePair<CSanPham, int> muc in hd.ChiTietDonHang)
                    {
                        CSanPham sanpham = muc.Key;
                        int soluong = muc.Value;

                        double tongTienGoc = sanpham.GiaBan * soluong;

                        double tongKhuyenMaiDong = hd.TinhTongKhuyenMaiMotSP(sanpham) * soluong;

                        Console.WriteLine($"{sanpham.TenBanh,-20} | {soluong,3} | {sanpham.GiaBan,10} | {tongTienGoc,12} | {tongKhuyenMaiDong * -1,12}");

                        tongGiaTriDon += tongTienGoc;
                        tongKhuyenMai += tongKhuyenMaiDong;
                    }

                    Console.WriteLine(new string('-', 66));

                    Console.WriteLine($"Tổng giá trị đơn:   {tongGiaTriDon} VND");
                    double tongTienThanhToan = hd.TinhThanhTien();

                    Console.WriteLine($"Tổng tiền giảm:     {tongKhuyenMai} VND");
                    Console.WriteLine($"Tổng tiền thanh toán: {tongTienThanhToan} VND");
                }
            }   
        }
        static void MenuCapNhatGiaHandmade(CQuanLy quanly)
        {
            XuatTieuDeCanGiua("CẬP NHẬT GIÁ BÁNH HANDMADE (TĂNG 3%)", 66);

            List<CBanhHandmade> dsHandmade = quanly.LayDanhSachBanhHandmade();
            if (!dsHandmade.Any())
            {
                Console.WriteLine("Không có bánh handmade nào để cập nhật.");
                return;
            }

            XuatTieuDeCanGiua("DANH SÁCH TRƯỚC KHI CẬP NHẬT", 66);
            XuatDanhSachPhanLoai(dsHandmade.ToList<CSanPham>());

            quanly.CapNhatGiaBanhHandmade();

            XuatTieuDeCanGiua("DANH SÁCH SAU KHI CẬP NHẬT", 66);
            XuatDanhSachPhanLoai(quanly.LayDanhSachBanhHandmade().ToList<CSanPham>());

            Console.WriteLine("\nCập nhật hoàn tất.");
        }
        static void MenuXuatBanhGiaTren150Ngan(CQuanLy quanly)
        {
            List<CSanPham> dsTmp = quanly.LayBanhGiaTren150Ngan();
            XuatTieuDeCanGiua("DANH SÁCH BÁNH CÓ GIÁ TRÊN 150.000 VND", 66);
            XuatDanhSachPhanLoai(dsTmp);
        }
        static void MenuXuatKhachHangMuaNhieuHonBaSP(CQuanLy quanly)
        {
            List<CKhachHang> dsTmp = quanly.LayKhachHangMuaNhieuHonBaSP();

            XuatTieuDeCanGiua("\nKHÁCH HÀNG MUA TỔNG SỐ LƯỢNG TRÊN 3 SẢN PHẨM", 66);

            if (dsTmp.Count() == 0)
            {
                Console.WriteLine("Không có khách hàng nào thỏa mãn điều kiện.");
                return;
            }

            foreach (CKhachHang obj in dsTmp)
            {
                Console.WriteLine();
                obj.HienThiThongTin();
                Console.WriteLine($"Đã mua tổng cộng: {obj.TinhTongSoSanPhamDaMua()} sản phẩm.");
            }
        }
        static void MenuXuatSanPhamSanXuatTrenBaThang(CQuanLy quanly)
        {
            List<CSanPham> dsTmp = quanly.LaySanPhamSanXuatTrenBaThang();
            XuatTieuDeCanGiua("SẢN PHẨM SẢN XUẤT TRÊN 3 THÁNG", 66);

            XuatDanhSachPhanLoai(dsTmp);
        }
        //Hàm helper
        static void XuatDanhSachPhanLoai(List<CSanPham> dssp)
        {
            List<CBanhNgot> dsBanhNgot = dssp.OfType<CBanhNgot>().ToList();
            List<CBanhKem> dsBanhKem = dssp.OfType<CBanhKem>().ToList();
            List<CBanhHandmade> dsBanhHandmade = dssp.OfType<CBanhHandmade>().ToList();

            if (dssp.Count == 0)
            {
                Console.WriteLine("Không tìm thấy sản phẩm nào thỏa mãn điều kiện.");
                return;
            }

            if (dsBanhNgot.Any())
            {
                Console.WriteLine("\nLOẠI: BÁNH NGỌT");
                Console.WriteLine($"{"Mã SP",-7} | {"Tên Bánh",-25} | {"Giá Bán (VND)",15} | {"Cách Bảo Quản",-15}");
                Console.WriteLine(new string('-', 68));
                foreach (CBanhNgot obj in dsBanhNgot)
                {
                    Console.WriteLine(string.Format("{0, -7} | {1, -25} | {2, 15} | {3, -15}", obj.MaSP, obj.TenBanh, obj.GiaBan.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")), obj.CachBaoQuan));
                }
            }

            if (dsBanhKem.Any())
            {
                Console.WriteLine("\nLOẠI: BÁNH KEM");
                Console.WriteLine($"{"Mã SP",-7} | {"Tên Bánh",-25} | {"Giá Bán (VND)",15} | {"Kích Thước (cm)",-15}");
                Console.WriteLine(new string('-', 68));
                foreach (CBanhKem obj in dsBanhKem)
                {
                    Console.WriteLine(string.Format("{0, -7} | {1, -25} | {2, 15} | {3, -15}", obj.MaSP, obj.TenBanh, obj.GiaBan.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")), obj.KichThuoc));
                }
            }

            if (dsBanhHandmade.Any())
            {
                Console.WriteLine("\nLOẠI: BÁNH HANDMADE");
                Console.WriteLine($"{"Mã SP",-7} | {"Tên Bánh",-25} | {"Giá Bán (VND)",15} | {"Dòng Bánh",-15}");
                Console.WriteLine(new string('-', 68));
                foreach (CBanhHandmade obj in dsBanhHandmade)
                {
                    Console.WriteLine(string.Format("{0, -7} | {1, -25} | {2, 15} | {3, -15}", obj.MaSP, obj.TenBanh, obj.GiaBan.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")), obj.DongBanh));
                }
            }
        }
        static void XuatTieuDeCanGiua(string tieude, int chieurong)
        {
            int khoangcach = (chieurong - tieude.Length) / 2;
            string chuoikhoangcach = new string(' ', Math.Max(0, khoangcach)); 

            Console.WriteLine(chuoikhoangcach + tieude);
        }
        static string NhapChuoiHopLe(string tenThongTin)
        {
            string input;
            while (true)
            {
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine($"LỖI: {tenThongTin} không được để trống.");
                    Console.Write("Vui lòng nhập lại: ");
                }
                else
                    break;
            }
            return input;
        }
        static double NhapSoThucHopLe(string tenThongTin)
        {
            double ketqua;
            while (true)
            {
                string input = Console.ReadLine();
                if (double.TryParse(input, out ketqua))
                {
                    if (ketqua < 0)
                    { 
                        Console.WriteLine($"LỖI: {tenThongTin} phải >= 0.");
                        Console.Write("Vui lòng nhập lại: ");
                    }    
                    else
                        break;
                }
                else
                {
                    Console.Write("LỖI: Bạn nhập không phải là số.");
                    Console.Write("Vui lòng nhập lại: ");
                }    
            }
            return ketqua;
        }
        static int NhapSoNguyenHopLe(string tenThongTin)
        {
            int ketqua;
            while (true)
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out ketqua))
                {
                    if (ketqua < 0)
                    {
                        Console.WriteLine($"LỖI: {tenThongTin} phải >= 0.");
                        Console.Write("Vui lòng nhập lại: ");
                    }
                    else
                        break;
                }
                else
                {
                    Console.WriteLine("LỖI: Bạn nhập không phải là số nguyên.");
                    Console.Write("Vui lòng nhập lại: ");
                }
            }
            return ketqua;
        }
        static DateTime NhapNSXHopLe()
        {
            DateTime ketqua;
            while (true)
            {
                string input = Console.ReadLine();
                if (DateTime.TryParseExact(input, dinhDangNgay, CultureInfo.InvariantCulture, DateTimeStyles.None, out ketqua))
                {

                    if (ketqua.Date > DateTime.Now.Date)
                    {
                        Console.WriteLine("LỖI: Ngày sản xuất không được ở tương lai.");
                        Console.Write("Vui lòng nhập lại: ");
                        continue;
                    }
                    else
                        break;
                }
                else
                {
                    Console.WriteLine($"LỖI: Sai định dạng. Phải nhập đúng {dinhDangNgay}.");
                    Console.Write("Vui lòng nhập lại: ");
                }

            }
            return ketqua;
        }
        static DateTime NhapHSDHopLe(DateTime hsd)
        {
            DateTime ketqua;
            while (true)
            {
                string input = Console.ReadLine();
                if (DateTime.TryParseExact(input, dinhDangNgay, CultureInfo.InvariantCulture, DateTimeStyles.None, out ketqua))
                {

                    if (ketqua.Date <= hsd.Date)
                    {
                        Console.WriteLine("LỖI: Hạn sử dụng phải sau ngày sản xuất.");
                        Console.Write("Vui lòng nhập lại: ");
                        continue;
                    }
                    else
                        break;
                }
                else
                {   
                    Console.Write($"LỖI: Sai định dạng. Phải nhập đúng {dinhDangNgay}.");
                    Console.Write("Vui lòng nhập lại: ");
                }
            }
            return ketqua;
        }
    }
}