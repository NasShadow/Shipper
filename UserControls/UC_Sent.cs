using BLL;
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
    public partial class UC_Sent : UserControl
    {
        static TaiKhoanBLL TKBLL = new TaiKhoanBLL();

        //Kết nối
        static SqlConnection conn = TKBLL.Load();

        public string email { get;set; }
        public UC_Sent(string email)
        {
            InitializeComponent();
            this.email = email;
        }
        //fORM LOAD
        private void UC_Sent_Load(object sender, EventArgs e)
        {
            TaoMa();
            Showdatagridview_LoaiHang();
        }

        DataTable dt = new DataTable("DonHang");
        private void Showdatagridview_LoaiHang()
        {
            try
            {
                dt.Clear();
                //Sẽ load lên datagridview
                string query = $"SELECT Ma_Don_Hang N'Mã_Đơn_Hàng', Thanh_Tien N'Thành_Tiền', Dia_Chi N'Địa_Chỉ', Ten_Khach_Hang N'Tên_Khách_Hàng', IIF(Trang_Thai_Don=0, N'Đang Giao Hàng', N'Hoàn Thành Đơn') AS N'Trạng_Thái_Đơn' FROM dbo.DonHang\r\nJOIN dbo.KhachHang ON KhachHang.Ma_Khach_Hang = DonHang.Ma_Khach_Hang\r\nWHERE Ma_Nhan_Vien IS NOT NULL AND Trang_Thai_Don = 1 AND Ma_Nhan_VienGH = N'{maNVGH}'";
                SqlDataAdapter adapter = new SqlDataAdapter(query, TKBLL.Load());
                adapter.Fill(dt);
                dgv_donhang.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "THÔNG BÁO!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void dgv_donhang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
