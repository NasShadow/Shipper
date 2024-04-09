using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DTO;

namespace Giao_Diện_Shipper
{
    public partial class FormDoiMatKhauProfile : Form
    {
        NhanVienGiaoHang nhanviengiaohang = new NhanVienGiaoHang();
        static TaiKhoanBLL TKBLL = new TaiKhoanBLL();

        //Kết nối
        static SqlConnection conn = TKBLL.Load();
        public string maNVGH { get; set; }
        public string email { get; set; }
        public FormDoiMatKhauProfile(string ma,string email)
        {
            InitializeComponent();
            this.maNVGH = ma;
            this.email = email;
        }

        //Form load
        private void FormDoiMatKhauProfile_Load(object sender, EventArgs e)
        {
            TaoMK();
            //Hàm random mã
            Random_OTP();

            //Hàm gửi mail
            Send_OTP_Email();
        }



        //Lấy mã Nhân viên giao hàng
        string MKHTai = "";
        private void TaoMK()
        {
            try
            {
                conn.Open();

                //Tạo vòng lặp và lấy dữ liệu từ csdl
                SqlCommand sqlCommand = new SqlCommand($"SELECT MatKhau FROM dbo.NhanVienGiaoHang WHERE Ma_Nhan_VienGH = N'{this.maNVGH}'", conn);
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MKHTai = "";
                    MKHTai += reader.GetString(0);
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


        //Button đổi mk
        private void btn_doimk_Click(object sender, EventArgs e)
        {
            if (txt_matkhauhientai.Text != MKHTai)
            {
                MessageBox.Show("MẬT KHẨU HIỆN TẠI CỦA BẠN ĐÃ SAI", "THÔNG BÁO!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (txt_otpemail.Text != otp_email)
            {
                MessageBox.Show("MÃ OTP CỦA BẠN ĐÃ SAI KHÔNG THỂ ĐỔI MK", "THÔNG BÁO!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            nhanviengiaohang.MatKhau = txt_matkhau.Text;
            nhanviengiaohang.XacNhanMatKhau = txt_xacnhanmatkhau.Text;
            nhanviengiaohang.Ma_Nhan_VienGH = maNVGH;

            string get = TKBLL.DoiMK_NVGiaoHang(nhanviengiaohang);

            switch(get)
            {
                case "requeid_botrong":
                    {
                        MessageBox.Show("VUI LÒNG KHÔNG BỎ TRỐNG THÔNG TIN!!!", "THÔNG BÁO!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        MessageBox.Show("MẬT KHẨU CỦA BẠN KHÔNG TRÙNG KHỚP!!!", "THÔNG BÁO!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    }
                default:
                    {
                        MessageBox.Show("ĐỔI MẬT KHẨU THÀNH CÔNG!!!", "THÔNG BÁO!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    }
            }

        }


        //Biến cục bộ chứa mã otp
        static string otp_email = "";

        //Hàm random code
        private void Random_OTP()
        {
            // Tạo một đối tượng Random để sinh số ngẫu nhiên
            Random random = new Random();

            // Tạo một chuỗi StringBuilder để xây dựng chuỗi ngẫu nhiên
            StringBuilder sb = new StringBuilder();

            // Tạo một mảng chứa tất cả các ký tự được cho phép
            string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            // Tạo một vòng lặp để thêm mỗi ký tự vào chuỗi ngẫu nhiên
            for (int i = 0; i < 6; i++)
            {
                // Lấy một ký tự ngẫu nhiên từ mảng characters
                char randomChar = characters[random.Next(characters.Length)];

                // Thêm ký tự ngẫu nhiên vào chuỗi
                sb.Append(randomChar);
            }

            otp_email = sb.ToString();
        }

        //Hàm gửi mã
        private void Send_OTP_Email()
        {
            string form, to, pass, content;
            form = "kienhttd00367@fpt.edu.vn";
            to = this.email;
            pass = "ldjw pdqo tbnj rftk";
            //Nội dung gửi mail
            content = $"Mã OTP để đổi mật khẩu tài khoản {this.email} là: " + otp_email;

            MailMessage mail = new MailMessage();
            mail.To.Add(to);
            mail.From = new MailAddress(form);
            //Tiêu đề cho nội dung gửi
            mail.Subject = "MÃ ĐỔI MẬT KHẢU TÀI KHOẢN TẠI SUNNY BOOK";
            mail.Body = content;

            // khởi tạo nó với địa chỉ của máy chủ SMTP. Trong trường hợp này, địa chỉ máy chủ SMTP là "smtp.gmail.com"
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            //Bật tính năng SSL để tạo một kết nối an toàn với máy chủ SMTP. SSL (Secure Sockets Layer) cung cấp một lớp bảo mật cho việc truyền thông tin giữa ứng dụng và máy chủ.
            smtp.EnableSsl = true;
            //Thiết lập cổng kết nối SMTP. Trong trường hợp này, sử dụng cổng 587, được sử dụng phổ biến cho kết nối SMTP bảo mật.
            smtp.Port = 587;
            //Thiết lập phương thức gửi email là qua mạng (Network).
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //Cung cấp thông tin xác thực cho máy chủ SMTP. Đối tượng NetworkCredential được tạo với tên đăng nhập (form) và mật khẩu (pass).
            smtp.Credentials = new NetworkCredential(form, pass);

            try
            {
                smtp.Send(mail);
                Console.WriteLine("Gửi Mail Thành Công!!!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }






















        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {
            
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            //Hàm random mã
            Random_OTP();

            //Hàm gửi mail
            Send_OTP_Email();
        }

        private void pic_an_Click(object sender, EventArgs e)
        {
            if (txt_matkhau.PasswordChar == '\0' && txt_xacnhanmatkhau.PasswordChar == '\0')
            {
                txt_matkhau.PasswordChar = '*';
                txt_xacnhanmatkhau.PasswordChar = '*';
                pic_an.Visible = false;
                pic_hien.Visible = true;
            }
        }

        private void pic_hien_Click(object sender, EventArgs e)
        {
            if (txt_matkhau.PasswordChar == '*' && txt_xacnhanmatkhau.PasswordChar == '*')
            {
                txt_matkhau.PasswordChar = '\0';
                txt_xacnhanmatkhau.PasswordChar = '\0';
                pic_an.Visible = true;
                pic_hien.Visible = false;
            }
        }
    }
}
