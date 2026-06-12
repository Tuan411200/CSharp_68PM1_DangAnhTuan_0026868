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
    public partial class UC_QLSV : UserControl
    {
        DatabaseDataContext db = new DatabaseDataContext();

        public UC_QLSV()
        {
            InitializeComponent();
            this.Load += new System.EventHandler(this.UC_QLSV_Load);
        }

        private void UC_QLSV_Load(object sender, EventArgs e)
        {
            LoadDanhSachSinhVien();
            LoadComboBoxLop();
        }

        private void LoadDanhSachSinhVien()
        {
            try
            {
                var ds = db.SinhViens.ToList();
                dataGridView1.DataSource = ds;
                dataGridView1.Columns["LopHoc"].Visible = false;
                dataGridView1.Columns["MaSV"].HeaderText = "Mã SV";
                dataGridView1.Columns["HoTen"].HeaderText = "Họ Tên";
                dataGridView1.Columns["GioiTinh"].HeaderText = "Giới Tính";
                dataGridView1.Columns["NgaySinh"].HeaderText = "Ngày Sinh";
                dataGridView1.Columns["MaLop"].HeaderText = "Mã Lớp";
                dataGridView1.ReadOnly = true;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void LoadComboBoxLop()
        {
            try
            {
                var dsLop = db.LopHocs.ToList();

                cboLop.DataSource = dsLop;
                cboLop.DisplayMember = "MaLop";  
                cboLop.ValueMember = "MaLop";

            }
            catch (Exception ex)
            { MessageBox.Show("Lỗi tải danh sách lớp: " + ex.Message); }
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
               
                if (string.IsNullOrEmpty(txtMaSV.Text) || string.IsNullOrEmpty(txtHoTen.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ Mã SV và Họ Tên!", "Cảnh báo");
                    return;
                }

                
                string maMoi = txtMaSV.Text.Trim();
                var checkTrung = db.SinhViens.FirstOrDefault(x => x.MaSV == maMoi);
                if (checkTrung != null)
                {
                    MessageBox.Show("Mã sinh viên này đã tồn tại trong hệ thống!", "Lỗi trùng mã");
                    return;
                }

                
                SinhVien sv = new SinhVien();
                sv.MaSV = maMoi;
                sv.HoTen = txtHoTen.Text.Trim();
                sv.GioiTinh = cboGioiTinh.Text;      
                sv.NgaySinh = dtpNgaySinh.Value;
                var lopChon = cboLop.SelectedItem as LopHoc;
                if (lopChon == null) { MessageBox.Show("Vui lòng chọn lớp!"); return; }
                sv.MaLop = lopChon.MaLop;

                if (string.IsNullOrEmpty(sv.MaLop))
                {
                    MessageBox.Show("Vui lòng chọn lớp!");
                    return;
                }


                db.SinhViens.InsertOnSubmit(sv);
                db.SubmitChanges(); 

                MessageBox.Show("Thêm mới sinh viên thành công!", "Thông báo");

              
                LoadDanhSachSinhVien();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm dữ liệu: " + ex.Message, "Thông báo lỗi");
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            txtMaSV.Text = row.Cells["MaSV"].Value?.ToString();
            txtHoTen.Text = row.Cells["HoTen"].Value?.ToString();
            cboGioiTinh.Text = row.Cells["GioiTinh"].Value?.ToString();
            dtpNgaySinh.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);

            string maLop = row.Cells["MaLop"].Value?.ToString();
            foreach (LopHoc lop in cboLop.Items)
            {
                if (lop.MaLop == maLop) { cboLop.SelectedItem = lop; break; }
            }

            txtMaSV.ReadOnly = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}