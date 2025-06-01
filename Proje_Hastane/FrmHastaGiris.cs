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
    public partial class FrmHastaGiris : Form
    {
        public FrmHastaGiris()
        {
            InitializeComponent();
        }
        SqlBaglantı bgl=new SqlBaglantı();
        
        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("select * from Tbl_Hasta where HastaTc=@p1 and HastaSifre=@p2",bgl.baglanti());
            cmd.Parameters.AddWithValue("@p1",MskTc.Text);
            cmd.Parameters.AddWithValue("@p2",TxtSifre.Text);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                FrmHastaDetay frm = new FrmHastaDetay();
                frm.tc = MskTc.Text;
                frm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Girdiğiniz Şifre Veya Tc Hatalı.lütfen Tekrar Deneyiniz..", "Uyarı",MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            bgl.baglanti().Close(); 
            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           FrmHastaUye frm = new FrmHastaUye();
            frm.Show();

        }

        private void TxtSifre_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            TxtSifre.UseSystemPasswordChar = !TxtSifre.UseSystemPasswordChar;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 fr = new Form1();
            fr.Show();
            this.Hide();
        }

        private void FrmHastaGiris_Load(object sender, EventArgs e)
        {

        }
    }
}
