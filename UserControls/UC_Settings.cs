using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace Giao_Diện_Shipper.UserControls
{
    public partial class UC_Settings : UserControl
    {

        NhanVienGiaoHang nhanviengiaohang = new NhanVienGiaoHang();
        static TaiKhoanBLL TKBLL = new TaiKhoanBLL();

        //Kết nối
        static SqlConnection conn = TKBLL.Load();
        public string email { get;set; }
        public UC_Settings(string email)
        {
            InitializeComponent();

            this.email = email;
        }

        private void Load1()
        {
            cbo_gioitinh.Items.Clear();
            cbo_gioitinh.Items.Add("Nam");
            cbo_gioitinh.Items.Add("Nữ");

            txt_manhanvien.Enabled = false;
            txt_tennhanvien.Enabled = false;
            txt_email.Enabled = false;
            txt_matkhau.Enabled = false;
        }


        string maNVGH = "";

        //Lấy mã Nhân viên giao hàng
        private void TaoMa()
        {
            try
            {
                conn.Open();

                //Tạo vòng lặp và lấy dữ liệu từ csdl
                SqlCommand sqlCommand = new SqlCommand($"SELECT Ma_Nhan_VienGH FROM dbo.NhanVienGiaoHang WHERE Email = N'{this.email}'", conn);
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    maNVGH = "";
                    maNVGH += reader.GetString(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void Load_Data()
        {
            try
            {
                using (SqlConnection conn = TKBLL.Load())
                {
                    conn.Open();
                    SqlCommand sqlCommand = new SqlCommand($"SELECT * FROM dbo.NhanVienGiaoHang WHERE Email = N'{this.email}'", conn);
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {


                        while (reader.Read())
                        {
                            // Lấy giá trị từ cột "Ma_loai_hang"
                            string MaNhanVien = reader["Ma_Nhan_VienGH"].ToString();
                            string TenNhanVien = reader["Ten_NV_GH"].ToString();
                            string Email = reader["Email"].ToString();
                            string MatKhau = reader["MatKhau"].ToString();
                            string GioiTinh = reader["Gioi_Tinh"].ToString();
                            string DiaChi = reader["DiaChi"].ToString();

                            //Check Gtinh
                            if (GioiTinh == "True")
                            {
                                GioiTinh = "Nam";
                            }
                            else if (GioiTinh == "False")
                            {
                                GioiTinh = "Nữ";
                            }

                            txt_manhanvien.Text = MaNhanVien;
                            txt_tennhanvien.Text = TenNhanVien;
                            txt_email.Text = Email;
                            txt_matkhau.Text = MatKhau;
                            cbo_gioitinh.Text = GioiTinh;
                            txt_diachi.Text = DiaChi;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }



        //Cập nhật thông tin tài khoản
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            nhanviengiaohang.DiaChi = txt_diachi.Text;
            nhanviengiaohang.Gioi_Tinh = cbo_gioitinh.Text;
            nhanviengiaohang.Ma_Nhan_VienGH = maNVGH;
            if (cbo_gioitinh.Text == "Nam")
            {
                nhanviengiaohang.Gioi_Tinh = "1";
            }
            else if(cbo_gioitinh.Text == "Nữ")
            {
                nhanviengiaohang.Gioi_Tinh = "0";
            }

            string get = TKBLL.CapNhat_ProFile(nhanviengiaohang);

            switch (get)
            {
                case "requeid_botrong":
                    {
                        MessageBox.Show("VUI LÒNG KHÔNG ĐƯỢC BỎ TRỐNG!!!", "THÔNG BÁO!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    }
                default:
                    {
                        MessageBox.Show("CẬP NHẬT THÔNG TIN THÀNH CÔNG!!!", "THÔNG BÁO!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        UC_Settings_Load(null,null);
                        break;
                    }
            }

        }

        //LoadData
        private void UC_Settings_Load(object sender, EventArgs e)
        {
            TaoMa();
            Load1();
            Load_Data();
        }

        private void btn_doimk_Click(object sender, EventArgs e)
        {
            FormDoiMatKhauProfile formDoiMatKhauProfile= new FormDoiMatKhauProfile(maNVGH, this.email);
            formDoiMatKhauProfile.ShowDialog();
        }

        private void cbo_gioitinh_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
