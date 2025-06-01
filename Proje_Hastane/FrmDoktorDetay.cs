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
    public partial class FrmDoktorDetay : Form
    {
        public FrmDoktorDetay()
        {
            InitializeComponent();
        }
        SqlBaglantı bgl=new SqlBaglantı();
        public string tc;
        private void FrmDoktorDetay_Load(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = true;
            LblTc.Text=tc.ToString();
            SqlCommand cmd = new SqlCommand("Select DoktorAd,DoktorSoyad From Tbl_Doktorlar where DoktorTc=@p1", bgl.baglanti());
            cmd.Parameters.AddWithValue("@p1", LblTc.Text);
            SqlDataReader dr1 = cmd.ExecuteReader();
            while (dr1.Read())
            {
                LblAdsoyad.Text = dr1[0] + " " + dr1[1];

            }
            bgl.baglanti().Close();
            //randevular
            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter("select * from Tbl_Randevu where RandevuDurum=1 and RandevuDoktor='"+LblAdsoyad.Text+"'",bgl.baglanti());
            da1.Fill(dt1);
            dataGridView1.DataSource = dt1;
            dataGridView1.ReadOnly = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 fr = new Form1();
            fr.Show();
            this.Hide();
        }

        private void BtnGüncelle_Click(object sender, EventArgs e)
        {
            FrmDoktorBilgiDüzenle fr=new FrmDoktorBilgiDüzenle();
            fr.tc=LblTc.Text;
            fr.Show();

        }

        private void BtnDuyuru_Click(object sender, EventArgs e)
        {
            FrmDuyurular frm = new FrmDuyurular();
            frm.Show();

        }

        private void BtnCıkıs_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen =dataGridView1.SelectedCells[0].RowIndex;
            RchSikayet.Text = dataGridView1.Rows[secilen].Cells[7].Value.ToString();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmDoktorBilgiDüzenle fr = new FrmDoktorBilgiDüzenle();
            fr.tc = LblTc.Text;
            fr.Show();
        }
    }
}
