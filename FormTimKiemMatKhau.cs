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
using BLL;

namespace Giao_Diện_Shipper
{
    public partial class FormTimKiemMatKhau : Form
    {
        NhanVienGiaoHang nhanviengiaohang = new NhanVienGiaoHang();
        TaiKhoanBLL TKBLL = new TaiKhoanBLL();


        public FormTimKiemMatKhau()
        {
            InitializeComponent();

            txt_Password.Enabled = false;
        }

        //Tìm kiếm
        private void btn_dangnhap_Click(object sender, EventArgs e)
        {
            nhanviengiaohang.Email = txt_username.Text;
            string get = TKBLL.Find_PassAdmin(nhanviengiaohang);

            switch (get)
            {
                case "requeid_botrong":
                    {
                        MessageBox.Show("VUI LÒNG NHẬP EMAIL!!!", "THÔNG BÁO!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    }
                case "Email của bạn không tồn tại!":
                    {
                        MessageBox.Show("EMAIL KHÔNG TỒN TẠI!!!", "THÔNG BÁO!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    }
                default:
                    {
                        MessageBox.Show("TÌM KIẾM THÀNH CÔNG", "THÔNG BÁO!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txt_Password.Text = TKBLL.Find_PassAdmin(nhanviengiaohang);
                        break;
                    }
            }
        }

        private void lbl_quenmk_Click(object sender, EventArgs e)
        {
            Hide();
            FormDangNhapShipper form = new FormDangNhapShipper();
            form.ShowDialog();
        }
    }
}
