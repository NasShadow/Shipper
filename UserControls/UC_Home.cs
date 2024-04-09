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
using BLL;
using DTO;

namespace Giao_Diện_Shipper.UserControls
{
    public partial class UC_Home : UserControl
    {


        DonHang donhang = new DonHang();
        static TaiKhoanBLL TKBLL = new TaiKhoanBLL();

        //Kết nối
        static SqlConnection conn = TKBLL.Load();


        public string email { get;set; }
        public UC_Home( string email )
        {
            InitializeComponent();
            this.email = email;
        }

        private void UC_Home_Load(object sender, EventArgs e)
        {
            Value_DF();
            Showdatagridview_LoaiHang();
            TaoMa();
            btn_nhandon.Enabled = false;
        }


        DataTable dt = new DataTable("DonHang");
        private void Showdatagridview_LoaiHang()
        {
            try
            {
                dt.Clear();
                //Sẽ load lên datagridview
                string query = "SELECT Ma_Don_Hang N'Mã_Đơn_Hàng', Thanh_Tien N'Thành_Tiền', Dia_Chi N'Địa_Chỉ', Ten_Khach_Hang N'Tên_Khách_Hàng' FROM dbo.DonHang\r\nJOIN dbo.KhachHang ON KhachHang.Ma_Khach_Hang = DonHang.Ma_Khach_Hang\r\nWHERE Ma_Nhan_Vien IS NOT NULL AND Trang_Thai_Don IS NULL AND Ma_Nhan_VienGH IS NULL";
                SqlDataAdapter adapter = new SqlDataAdapter(query, TKBLL.Load());
                adapter.Fill(dt);
                dgv_donhang.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "THÔNG BÁO!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Khi ấn từ lưới
        private void dgv_donhang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = dgv_donhang.CurrentRow.Index;

            txt_madonhang.Text = dgv_donhang.Rows[i].Cells[0].Value.ToString();
            txt_diachi.Text = dgv_donhang.Rows[i].Cells[2].Value.ToString();

            btn_nhandon.Enabled = true;
        }


        private void Value_DF()
        {
            txt_diachi.Enabled = false;
            txt_madonhang.Enabled = false;
        }

        private void guna2ContextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

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
        //Nút nhận đơn hàng
        private void btn_nhandon_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("BẠN MUỐN NHẬN ĐƠN HÀNG NÀY CHỨ???", "THÔNG BÁO!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                donhang.Ma_Don_Hang = txt_madonhang.Text;
                donhang.Ma_Nhan_VienGH = maNVGH;


                string get = TKBLL.CapNhat_TrangThaiDonHang(donhang);

                switch (get)
                {
                    default:
                        {
                            MessageBox.Show("NHẬN ĐƠN HÀNG THÀNH CÔNG!!!", "THÔNG BÁO!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            UC_Home_Load(null,null);
                            break;
                        }
                }

                return;
            }
            else if (result == DialogResult.No)
            {
                return;
            }
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
    }
}
