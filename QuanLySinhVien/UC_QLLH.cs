using QuanLySinhVien;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class UC_QLLH : UserControl
    {
        DatabaseDataContext db = new DatabaseDataContext();

        private int currentPage = 1;
        private int pageSize = 10;
        private List<LopHoc> danhSachLop = new List<LopHoc>();
        public UC_QLLH()
        {
            InitializeComponent();
            this.Load += new System.EventHandler(this.UC_QLLH_Load);
        }
        private void UC_QLLH_Load(object sender, EventArgs e)
        {
            LoadDanhSachLop();
        }
        private void LoadDanhSachLop(string keyword = "")
        {
            var query = db.LopHocs.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x =>
                    x.MaLop.Contains(keyword) ||
                    x.TenLop.Contains(keyword));
            }

            danhSachLop = query.ToList();

            int totalRecords = danhSachLop.Count;
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            if (totalPages == 0) totalPages = 1;
            if (currentPage > totalPages) currentPage = totalPages;

            var pageData = danhSachLop
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            dataGridView1.DataSource = pageData;

            // Ẩn cột navigation
            if (dataGridView1.Columns["SinhViens"] != null)
                dataGridView1.Columns["SinhViens"].Visible = false;

            // Cập nhật label phân trang
            label7.Text = $"Trang {currentPage}/{totalPages}  |  {totalRecords} bản ghi";
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadDanhSachLop(textBox5.Text.Trim());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaLop.Text) || string.IsNullOrEmpty(txtTenLop.Text))
            { MessageBox.Show("Nhập đầy đủ Mã Lớp và Tên Lớp!"); return; }

            if (db.LopHocs.Any(x => x.MaLop == txtMaLop.Text.Trim()))
            { MessageBox.Show("Mã lớp đã tồn tại!"); return; }

            // MaID là IDENTITY – không set thủ công
            var lop = new LopHoc
            {
                MaLop = txtMaLop.Text.Trim(),
                TenLop = txtTenLop.Text.Trim(),
                GhiChu = txtGhiChu.Text.Trim()
            };
            db.LopHocs.InsertOnSubmit(lop);
            db.SubmitChanges();
            MessageBox.Show("Thêm lớp thành công!");
            LamMoi();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaLop.Text))
            { MessageBox.Show("Vui lòng chọn lớp cần sửa!"); return; }

            // Tìm theo MaLop (business key), không dùng MaID để tránh lỗi parse
            var lop = db.LopHocs.FirstOrDefault(x => x.MaLop == txtMaLop.Text.Trim());
            if (lop == null) { MessageBox.Show("Không tìm thấy lớp!"); return; }

            lop.TenLop = txtTenLop.Text.Trim();
            lop.GhiChu = txtGhiChu.Text.Trim();
            db.SubmitChanges();
            MessageBox.Show("Cập nhật thành công!");
            LoadDanhSachLop(textBox5.Text.Trim());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaLop.Text))
            { MessageBox.Show("Vui lòng chọn lớp cần xóa!"); return; }

            var lop = db.LopHocs.FirstOrDefault(x => x.MaLop == txtMaLop.Text.Trim());
            if (lop == null) { MessageBox.Show("Không tìm thấy lớp!"); return; }

            // SinhViens là EntitySet<SinhVien> – kiểm tra có sinh viên không
            if (lop.SinhViens.Any())
            { MessageBox.Show("Lớp còn sinh viên, không thể xóa!"); return; }

            var confirm = MessageBox.Show($"Xóa lớp '{lop.TenLop}' ({lop.MaLop})?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.No) return;

            db.LopHocs.DeleteOnSubmit(lop);
            db.SubmitChanges();
            MessageBox.Show("Xóa lớp thành công!");
            LamMoi();
        }

        private void button4_Click(object sender, EventArgs e) => LamMoi();
        private void LamMoi()
        {
            txtMaID.Clear();
            txtMaLop.Clear();
            txtTenLop.Clear();
            txtGhiChu.Clear();
            textBox5.Clear();
            currentPage = 1;
            LoadDanhSachLop();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dataGridView1.Rows[e.RowIndex];

            // MaID là int IDENTITY – hiển thị read-only
            txtMaID.Text = row.Cells["MaID"]?.Value?.ToString();
            txtMaLop.Text = row.Cells["MaLop"]?.Value?.ToString();
            txtTenLop.Text = row.Cells["TenLop"]?.Value?.ToString();
            txtGhiChu.Text = row.Cells["GhiChu"]?.Value?.ToString();
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            currentPage = 1; LoadDanhSachLop(textBox5.Text.Trim());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--; LoadDanhSachLop(textBox5.Text.Trim());
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            int totalPages = Math.Max(1, (int)Math.Ceiling((double)danhSachLop.Count / pageSize));
            if (currentPage < totalPages)
            {
                currentPage++; LoadDanhSachLop(textBox5.Text.Trim());
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            currentPage = Math.Max(1, (int)Math.Ceiling((double)danhSachLop.Count / pageSize));
            LoadDanhSachLop(textBox5.Text.Trim());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaLop.Text))
            { MessageBox.Show("Vui lòng chọn lớp trước!"); return; }

            var form = new frm_dssv(txtMaLop.Text.Trim(), txtTenLop.Text.Trim());
            form.ShowDialog();
        }
    }
}