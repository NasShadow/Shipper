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
using System.Windows.Forms;

namespace Giao_Diện_Shipper
{
    public partial class FormDoiLaiMatKhau : Form
    {
        NhanVienGiaoHang nhanviengiaohang = new NhanVienGiaoHang();
        static TaiKhoanBLL TKBLL = new TaiKhoanBLL();
        //Kết nối
        static SqlConnection conn = TKBLL.Load();
        public string email { get; set; } 
        public FormDoiLaiMatKhau(string email)
        {
            InitializeComponent();

            this.email = email;
        }
        //Form Load
        private void FormDoiLaiMatKhau_Load(object sender, EventArgs e)
        {
            TaoMa();
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

        //Ấn vào để đổi mật khẩu
        private void btn_dangnhap_Click(object sender, EventArgs e)
        {
            nhanviengiaohang.Ma_Nhan_VienGH = maNVGH;
            nhanviengiaohang.MatKhau = txt_Password.Text;
            nhanviengiaohang.XacNhanMatKhau = txt_CFPassword.Text;
            string get = TKBLL.CapNhat_MKThanhCong(nhanviengiaohang);

            switch (get)
            {
                case "requeid_botrong":
                    {
                        MessageBox.Show("VUI LÒNG NHẬP ĐẦY ĐỦ!!!", "THÔNG BÁO!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    }
                case "requeid_matkhau":
                    {
                        MessageBox.Show("MẬT KHẨU CẦN CÓ ÍT NHẤT 1 KÝ TỰ ĐẶC BIỆT!!!", "THÔNG BÁO!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    }
                case "requeid_short":
                    {
                        MessageBox.Show("MẬT KHẨU CỦA BẠN QUÁ NGẮN!!!", "THÔNG BÁO!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    }
                case "requeid_khongbang":
                    {
                        MessageBox.Show("MẬT KHẨU KHÔNG TRÙNG KHỚP NHAU!!!", "THÔNG BÁO!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    }
                default:
                    {
                        MessageBox.Show("CẬP NHẬT MẬT KHẨU THÀNH CÔNG!!!", "THÔNG BÁO!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    }
            }
        }


        //Hiển thị giao diện đăng nhập
        private void lbl_quenmk_Click(object sender, EventArgs e)
        {
            Hide();
            FormDangNhapShipper form = new FormDangNhapShipper();
            form.ShowDialog();
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pic_hien_Click(object sender, EventArgs e)
        {
            if (txt_Password.PasswordChar == '*')
            {
                txt_Password.PasswordChar = '\0';
                pic_an.Visible = true;
                pic_hien.Visible = false;
            }
        }

        private void pic_an_Click(object sender, EventArgs e)
        {
            if (txt_Password.PasswordChar == '\0')
            {
                txt_Password.PasswordChar = '*';
                pic_an.Visible = false;
                pic_hien.Visible = true;
            }
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
