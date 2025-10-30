using DTO_QLTiemBanhNgot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Xml;

namespace DAL_QLTiemBanhNgot
{
    public class CXmlData
    {
        private string dinhDangXML = "dd/MM/yyyy";

        public List<CSanPham> DocDanhSachSanPham(string duongdan)
        {
            List<CSanPham> dssp = new List<CSanPham>();
            XmlDocument doc = new XmlDocument();
            doc.Load(duongdan);

            XmlNodeList sanPhamNodes = doc.SelectNodes("/DanhSachSanPham/SanPham");

            foreach (XmlNode spNode in sanPhamNodes)
            {
                try
                {
                    string loaiBanh = spNode.Attributes["LoaiBanh"].Value;
                    CSanPham sp = null;

                    string ma = spNode["MaSanPham"].InnerText;
                    string ten = spNode["TenBanh"].InnerText;
                    double gia = double.Parse(spNode["GiaBan"].InnerText);
                    DateTime nsx = DateTime.ParseExact(spNode["NgaySanXuat"].InnerText, dinhDangXML, CultureInfo.InvariantCulture);
                    DateTime hsd = DateTime.ParseExact(spNode["HanSuDung"].InnerText, dinhDangXML, CultureInfo.InvariantCulture);

                    switch (loaiBanh)
                    {
                        case "CBanhNgot":
                            string cachBaoQuan = spNode["CachBaoQuan"].InnerText;
                            sp = new CBanhNgot(ma, ten, nsx, hsd, gia, cachBaoQuan);
                            break;
                        case "CBanhKem":
                            int kichThuoc = int.Parse(spNode["KichThuoc"].InnerText);
                            sp = new CBanhKem(ma, ten, nsx, hsd, gia, kichThuoc);
                            break;
                        case "CBanhHandmade":
                            string dongBanh = spNode["DongBanh"].InnerText;
                            sp = new CBanhHandmade(ma, ten, nsx, hsd, gia, dongBanh);
                            break;
                        default:
                            Console.WriteLine($"Lỗi: Loại bánh '{loaiBanh}' không được hỗ trợ.");
                            break;
                    }

                    if (sp != null)
                        dssp.Add(sp);
                }
                catch (NullReferenceException nre)
                {
                    Console.WriteLine($"Lỗi cấu trúc XML khi đọc một sản phẩm: {nre.Message}");
                }
                catch (FormatException fe)
                {
                    Console.WriteLine($"Lỗi định dạng dữ liệu khi đọc một sản phẩm: {fe.Message}");
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine($"Lỗi dữ liệu không hợp lệ khi đọc một sản phẩm: {ae.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi không xác định khi đọc một sản phẩm: {ex.Message}");
                }
            }
            return dssp;
        }

        public List<CKhachHang> DocDanhSachKhachHang(string duongdan, List<CSanPham> danhSachTatCaSanPham)
        {
            Dictionary<string, CSanPham> sanPhamDict = danhSachTatCaSanPham.ToDictionary(sp => sp.MaSP, StringComparer.OrdinalIgnoreCase);
            List<CKhachHang> dskh = new List<CKhachHang>();
            XmlDocument doc = new XmlDocument();
            doc.Load(duongdan);

            XmlNodeList khachHangNodes = doc.SelectNodes("/DanhSachKhachHang/KhachHang");

            foreach (XmlNode khNode in khachHangNodes)
            {
                string maKH = khNode["MaKH"].InnerText;
                string tenKH = khNode["TenKH"].InnerText;
                string sdt = khNode["SoDienThoai"].InnerText;
                CKhachHang kh = new CKhachHang(maKH, tenKH, sdt);

                XmlNode lichSuNode = khNode["LichSuMuaHang"];
                if (lichSuNode != null)
                {
                    XmlNodeList hoaDonNodes = lichSuNode.SelectNodes("HoaDon");
                    foreach (XmlNode hdNode in hoaDonNodes)
                    {
                        try
                        {
                            string maHD = hdNode["MaHD"].InnerText;
                            DateTime ngayMua = DateTime.ParseExact(hdNode["NgayMua"].InnerText, dinhDangXML, CultureInfo.InvariantCulture);
                            Dictionary<CSanPham, int> chiTietHoaDon = new Dictionary<CSanPham, int>();
                            XmlNodeList spMuaNodes = hdNode.SelectNodes("SanPham");
                            foreach (XmlNode spMuaNode in spMuaNodes)
                            {
                                string maSPMua = spMuaNode["MaSP"].InnerText;
                                int soLuongMua = int.Parse(spMuaNode["SoLuong"].InnerText);

                                CSanPham spThucTe;
                                if (sanPhamDict.TryGetValue(maSPMua, out spThucTe))
                                {
                                    chiTietHoaDon.Add(spThucTe, soLuongMua);
                                }
                                else
                                {
                                    Console.WriteLine($"Cảnh báo: Không tìm thấy sản phẩm có mã '{maSPMua}'... Bỏ qua.");
                                }
                            }
                            CHoaDon hoaDonMoi = new CHoaDon(maHD, ngayMua, chiTietHoaDon);

                            kh.ThemHoaDon(hoaDonMoi);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Lỗi khi đọc một hóa đơn của khách hàng {maKH}: {ex.Message}");
                        }
                    }
                }
                dskh.Add(kh);
            }
            return dskh;
        }

        public void LuuDanhSachSanPham(string duongdan, List<CSanPham> dssp)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("DanhSachSanPham");
            doc.AppendChild(root);

            foreach (CSanPham sp in dssp)
            {
                XmlElement spNode = doc.CreateElement("SanPham");
                spNode.SetAttribute("LoaiBanh", sp.GetType().Name);

                XmlElement maSP = doc.CreateElement("MaSanPham");
                maSP.InnerText = sp.MaSP; 
                spNode.AppendChild(maSP);

                XmlElement tenBanh = doc.CreateElement("TenBanh"); 
                tenBanh.InnerText = sp.TenBanh; 
                spNode.AppendChild(tenBanh);

                XmlElement nsx = doc.CreateElement("NgaySanXuat"); 
                nsx.InnerText = sp.NgaySanXuat.ToString(dinhDangXML); 
                spNode.AppendChild(nsx);

                XmlElement hsd = doc.CreateElement("HanSuDung"); 
                hsd.InnerText = sp.HanSuDung.ToString(dinhDangXML); 
                spNode.AppendChild(hsd);

                XmlElement giaBan = doc.CreateElement("GiaBan");
                giaBan.InnerText = sp.GiaBan.ToString(CultureInfo.InvariantCulture);
                spNode.AppendChild(giaBan);

                if (sp is CBanhNgot banhNgot)
                {
                    XmlElement cachBaoQuan = doc.CreateElement("CachBaoQuan"); 
                    cachBaoQuan.InnerText = banhNgot.CachBaoQuan; 
                    spNode.AppendChild(cachBaoQuan);
                }
                else if (sp is CBanhKem banhKem)
                {
                    XmlElement kichThuoc = doc.CreateElement("KichThuoc");
                    kichThuoc.InnerText = banhKem.KichThuoc.ToString(CultureInfo.InvariantCulture);
                    spNode.AppendChild(kichThuoc);
                }
                else if (sp is CBanhHandmade banhHandmade)
                {
                    XmlElement dongBanh = doc.CreateElement("DongBanh"); 
                    dongBanh.InnerText = banhHandmade.DongBanh;
                    spNode.AppendChild(dongBanh);
                }

                root.AppendChild(spNode);
            }
            doc.Save(duongdan);
        }

        public void LuuDanhSachKhachHang(string duongdan, List<CKhachHang> dskh)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("DanhSachKhachHang");
            doc.AppendChild(root);

            foreach (CKhachHang kh in dskh)
            {
                XmlElement khNode = doc.CreateElement("KhachHang");

                XmlElement maKH = doc.CreateElement("MaKH"); 
                maKH.InnerText = kh.MaKH; 
                khNode.AppendChild(maKH);

                XmlElement tenKH = doc.CreateElement("TenKH");
                tenKH.InnerText = kh.TenKH; 
                khNode.AppendChild(tenKH);

                XmlElement sdt = doc.CreateElement("SoDienThoai"); 
                sdt.InnerText = kh.SoDienThoai;
                khNode.AppendChild(sdt);

                XmlElement lichSuNode = doc.CreateElement("LichSuMuaHang");
                foreach (CHoaDon hd in kh.LichSuMuaHang)
                {
                    XmlElement hoaDonNode = doc.CreateElement("HoaDon");

                    XmlElement maHD = doc.CreateElement("MaHD"); 
                    maHD.InnerText = hd.MaHD; 
                    hoaDonNode.AppendChild(maHD);

                    XmlElement ngayMua = doc.CreateElement("NgayMua"); 
                    ngayMua.InnerText = hd.NgayMua.ToString(dinhDangXML); 
                    hoaDonNode.AppendChild(ngayMua);

                    foreach (KeyValuePair<CSanPham, int> muc in hd.ChiTietDonHang)
                    {
                        XmlElement spMuaNode = doc.CreateElement("SanPham");

                        XmlElement maSPMua = doc.CreateElement("MaSP"); 
                        maSPMua.InnerText = muc.Key.MaSP; 
                        spMuaNode.AppendChild(maSPMua);

                        XmlElement soLuongMua = doc.CreateElement("SoLuong"); 
                        soLuongMua.InnerText = muc.Value.ToString(); 
                        spMuaNode.AppendChild(soLuongMua);
                        hoaDonNode.AppendChild(spMuaNode);
                    }
                    lichSuNode.AppendChild(hoaDonNode);
                }
                khNode.AppendChild(lichSuNode);
                root.AppendChild(khNode);
            }
            doc.Save(duongdan);
        }
    }
}
