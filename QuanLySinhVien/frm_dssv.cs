using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLySinhVien
{
    public partial class frm_dssv : Form
    {
        DatabaseDataContext db = new DatabaseDataContext();
        private string maLop;
        public frm_dssv(string maLop, string tenLop)
        {
            InitializeComponent();
            this.maLop = maLop;
            this.Text = $"Danh sách sinh viên - {tenLop} ({maLop})";
            LoadDanhSachSV();
        }

        private void frm_dssv_Load(object sender, EventArgs e)
        {

        }
        private void LoadDanhSachSV()
        {
            var ds = db.SinhViens
        .Where(sv => sv.MaLop == maLop)
        .ToList()  
        .Select(sv => new
        {
            sv.MaSV,
            sv.HoTen,
            sv.GioiTinh,
            NgaySinh = sv.NgaySinh.ToString("dd/MM/yyyy")  
        })
        .ToList();

            dataGridView1.DataSource = ds;
        }
    }
}
