using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Proje_Hastane
{
    public partial class FrmSekreterGiris : Form
    {
        public FrmSekreterGiris()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TxtSifre.UseSystemPasswordChar = !TxtSifre.UseSystemPasswordChar;

        }

        private void button2_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 fr = new Form1();
            fr.Show();
            this.Hide();

        }
        SqlBaglantı bgl=new SqlBaglantı();  
        private void BtnGiris_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("Select * from Tbl_Sekreter where SekreterTc=@p1 and SekreterSifre=@p2", bgl.baglanti());
            cmd.Parameters.AddWithValue("@p1", MskTc.Text);
            cmd.Parameters.AddWithValue("@p2",TxtSifre.Text);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                FrmSekreterDetay fr = new FrmSekreterDetay();fr.Tc = MskTc.Text; fr.Show();
                this.Hide();

            }
            else
            {
                MessageBox.Show("Girdiğiniz Şifre Veya Tc Hatalı.lütfen Tekrar Deneyiniz..", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            bgl.baglanti().Close(); 
        }

        private void FrmSekreterGiris_Load(object sender, EventArgs e)
        {

        }
    }
}
