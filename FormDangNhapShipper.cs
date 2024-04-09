using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Giao_Diện_Shipper
{
    public partial class FormDangNhapShipper : Form
    {
        NhanVienGiaoHang nhanviengiaohang = new NhanVienGiaoHang();
        TaiKhoanBLL TKBLL = new TaiKhoanBLL();

        public FormDangNhapShipper()
        {
            InitializeComponent();
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {

        }

        //Nút Đăng Nhập Form Shipper
        private void guna2Button1_Click(object sender, EventArgs e)
        {

            nhanviengiaohang.Email = txt_username.Text;
            nhanviengiaohang.MatKhau = txt_Password.Text;

            string check = TKBLL.Check_LoginShipper(nhanviengiaohang);

            switch (check)
            {
                case "requeid_botrong":
                    {
                        MessageBox.Show("VUI LÒNG NHẬP ĐẦY ĐỦ!!!", "THÔNG BÁO!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                case "Tài khoản mật khẩu đã sai!":
                    {
                        MessageBox.Show("TÀI KHOẢN HOẶC MẬT KHẨU CỦA BẠN ĐÃ SAI!!!", "THÔNG BÁO!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                case "false":
                    {
                        MessageBox.Show("TÀI KHOẢN CỦA BẠN ĐÃ BỊ KHÓA!!!", "THÔNG BÁO!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                /*case "Shipper":
                    {
                        if (nhanviengiaohang.MatKhau == "123456789ABCD@")
                        {
                            Hide();
                            FormDoiLaiMatKhau form1 = new FormDoiLaiMatKhau(txt_username.Text);
                            form1.ShowDialog();
                            return;
                        }
                        MessageBox.Show("ĐĂNG NHẬP THÀNH CÔNG!!!", "THÔNG BÁO!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Hide();
                        Form1 form = new Form1(txt_username.Text);
                        form.ShowDialog();
                        return;
                    }*/
                default:
                    {
                        if (nhanviengiaohang.MatKhau == "123456789ABCD@")
                        {
                            Hide();
                            FormDoiLaiMatKhau form1 = new FormDoiLaiMatKhau(txt_username.Text);
                            form1.ShowDialog();
                            return;
                        }
                        MessageBox.Show("ĐĂNG NHẬP THÀNH CÔNG!!!", "THÔNG BÁO!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Hide();
                        Form1 form = new Form1(txt_username.Text);
                        form.ShowDialog();
                        return;
                    }
            }



        }

        private void lbl_quenmk_Click(object sender, EventArgs e)
        {
            Hide();
            FormTimKiemMatKhau form = new FormTimKiemMatKhau();
            form.ShowDialog();
        }

        private void FormDangNhapShipper_Load(object sender, EventArgs e)
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
    }
}
