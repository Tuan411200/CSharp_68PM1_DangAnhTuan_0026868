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
                listView1.View = View.Details;
                listView1.GridLines = true;       
                listView1.FullRowSelect = true;  

               
                listView1.Columns.Clear();
                listView1.Items.Clear();

                
                listView1.Columns.Add("Mã SV", 90);
                listView1.Columns.Add("Họ Tên", 150);
                listView1.Columns.Add("Giới Tính", 80);
                listView1.Columns.Add("Ngày Sinh", 110);
                listView1.Columns.Add("Mã Lớp", 90);

               
                var ds = db.SinhViens.ToList();

                
                foreach (var sv in ds)
                {
                    
                    ListViewItem item = new ListViewItem(sv.MaSV);

                   
                    item.SubItems.Add(sv.HoTen);
                    item.SubItems.Add(sv.GioiTinh);
                    item.SubItems.Add(sv.NgaySinh.ToString("dd/MM/yyyy")); 
                    item.SubItems.Add(sv.MaLop);

                    
                    listView1.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sinh viên: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadComboBoxLop()
        {
            try
            {
                var dsLop = db.LopHocs.ToList();

            }
            catch { }
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
                sv.MaLop = cboLop.SelectedValue?.ToString() ?? "CNTT01"; 

                
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
        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }
}