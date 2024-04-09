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
    public partial class UC_Inbox : UserControl
    {
        DonHang donhang = new DonHang();
        static TaiKhoanBLL TKBLL = new TaiKhoanBLL();

        //Kết nối
        static SqlConnection conn = TKBLL.Load();

        public string email { get; set; }
        public UC_Inbox(string email)
        {
            InitializeComponent();
            this.email = email;

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        //FormLoad
        private void UC_Inbox_Load(object sender, EventArgs e)
        {
            TaoMa();
            Showdatagridview_LoaiHang();
            Load1();
        }

        private void Load1()
        {
            btn_Huydon.Enabled = false;
            txt_madonhang.Enabled = false;
            txt_diachi.Enabled = false;
        }

        //Ấn vào lưới
        private void dgv_donhang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = dgv_donhang.CurrentRow.Index;

            txt_madonhang.Text = dgv_donhang.Rows[i].Cells[0].Value.ToString();
            txt_diachi.Text = dgv_donhang.Rows[i].Cells[2].Value.ToString();

            btn_Huydon.Enabled = true;
        }

        //HIển thị datagridview
        DataTable dt = new DataTable("DonHang");
        private void Showdatagridview_LoaiHang()
        {
            try
            {
                dt.Clear();
                //Sẽ load lên datagridview
                string query = $"SELECT dbo.DonHang.Ma_Don_Hang N'Mã_Đơn_Hàng', Thanh_Tien N'Thành_Tiền', Dia_Chi N'Địa_Chỉ', Ten_Khach_Hang N'Tên_Khách_Hàng', IIF(Trang_Thai_Don=0, N'Đang Giao Hàng', N'Hoàn Thành Đơn') AS N'Trạng_Thái_Đơn', IIF(Phuong_thuc_thanh_toan = 1, N'Thanh_Toán_Online', N'Thanh_Toán_Trực_Tiếp') N'Phương_Thức_Thanh_Toán' FROM dbo.DonHang \r\nJOIN dbo.KhachHang ON KhachHang.Ma_Khach_Hang = DonHang.Ma_Khach_Hang \r\nJOIN dbo.PhuongThucThanhToan ON PhuongThucThanhToan.Ma_Don_Hang = DonHang.Ma_Don_Hang\r\nWHERE Ma_Nhan_Vien IS NOT NULL AND Trang_Thai_Don = 0 AND Ma_Nhan_VienGH = N'{maNVGH}'";
                SqlDataAdapter adapter = new SqlDataAdapter(query, TKBLL.Load());
                adapter.Fill(dt);
                dgv_donhang.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "THÔNG BÁO!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Lấy mã Nhân viên giao hàng
        string maNVGH = "";

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
        //Tìm kiếm đơn
        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataView dv = dt.DefaultView;
                // Access Text property of txt_search
                dv.RowFilter = string.Format("Mã_Đơn_Hàng LIKE '%{0}%'", txt_search.Text);
                dgv_donhang.DataSource = dv.ToTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "THÔNG BÁO!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Huydon_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("BẠN MUỐN HỦY ĐƠN HÀNG NÀY CHỨ???", "THÔNG BÁO!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                donhang.Ma_Don_Hang = txt_madonhang.Text;

                string get = TKBLL.Huy_DonHang(donhang);

                switch (get)
                {
                    default:
                        {
                            MessageBox.Show("HỦY ĐƠN HÀNG THÀNH CÔNG!!!", "THÔNG BÁO!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            UC_Inbox_Load(null, null);
                            break;
                        }
                }






                return;
            }
            else if (result == DialogResult.No)
            {

            }
        }
    }
}
