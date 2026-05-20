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
    public partial class frm_login : Form
    {
        public frm_login()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
            if (username == "0026868@st.huce.edu.vn" && password == "12345")
            {
                MessageBox.Show("Đăng nhập thành công!");
                frm_main frm = new frm_main();
                frm.Show();

                this.Hide();
            }
            else
            {
                MessageBox.Show("Đăng nhập thất bại!");
            }
        }
    }
}
