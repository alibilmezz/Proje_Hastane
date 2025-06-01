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
    public partial class FrmHastaDetay : Form
    {
        public FrmHastaDetay()
        {
            InitializeComponent();
        }
        public string tc;
        SqlBaglantı bgl=new SqlBaglantı();

        private void fileSystemWatcher1_Changed(object sender, System.IO.FileSystemEventArgs e)
        {

        }

        private void FrmHastaDetay_Load(object sender, EventArgs e)
        {
            //AD SOYAD ÇEKME
            LblTc.Text= tc;
            SqlCommand cmd = new SqlCommand("select HastaAd,HastaSoyad,HastaID From Tbl_Hasta where HastaTc=@p1", bgl.baglanti());
            cmd.Parameters.AddWithValue("@p1",LblTc.Text);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                LblAdsoyad.Text = dr[0]+" "+dr[1];
                LblID.Text = dr[2]+"";
            }
            bgl.baglanti().Close();

            //RANDEVU GEÇMİŞİ
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select * from Tbl_Randevu where HastaTC=" + tc, bgl.baglanti());
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            //BRANŞLARI ÇEKME
            SqlCommand cmd2 = new SqlCommand("select BransAd from Tbl_Brans ",bgl.baglanti());
            SqlDataReader dr2 = cmd2.ExecuteReader();
            while (dr2.Read())
            {
                CmbBrans.Items.Add(dr2[0]);
            }
            bgl.baglanti().Close();
            dataGridView1.ReadOnly = true;
            dataGridView2.ReadOnly = true;



        }

        private void CmbBrans_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DOKTORLARI ÇEKME
            CmbDoktor.Items.Clear();
            CmbDoktor.Text = string.Empty;

            SqlCommand cmd3 = new SqlCommand("select DoktorAd,DoktorSoyad from Tbl_Doktorlar where DoktorBrans=@p2", bgl.baglanti());
            cmd3.Parameters.AddWithValue("@p2", CmbBrans.Text);
            SqlDataReader dr3 = cmd3.ExecuteReader();
            while (dr3.Read())
            {
                CmbDoktor.Items.Add(dr3[0] + " " + dr3[1]);
            }
            bgl.baglanti().Close();

        }

        private void CmbDoktor_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select * from Tbl_Randevu where RandevuBrans='" + CmbBrans.Text + "'" + "and RandevuDoktor='" + CmbDoktor.Text +"'and RandevuDurum=0",bgl.baglanti());   
            da.Fill(dt);
            dataGridView2.DataSource = dt;
        }

        private void LnkBilgidüzenle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmBilgiDüzenleme fr =new FrmBilgiDüzenleme();
            fr.ıd =LblID.Text;
            fr.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 fr = new Form1();
            fr.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView2.SelectedCells[0].RowIndex;  
            textBox1.Text=dataGridView2.Rows[secilen].Cells[0].Value.ToString();    
        }

        private void BtnRandevual_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LblTc.Text) ||
                string.IsNullOrWhiteSpace(RchSikayet.Text) ||
                string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }

            try
            {
                SqlCommand cmd = new SqlCommand("Update Tbl_Randevu set RandevuDurum=1,HastaTc=@p1,HastaSikayet=@p2 where RandevuId=@p3", bgl.baglanti());
                cmd.Parameters.AddWithValue("@p1", LblTc.Text);
                cmd.Parameters.AddWithValue("@p2", RchSikayet.Text);
                cmd.Parameters.AddWithValue("@p3", textBox1.Text);
                cmd.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Randevu Oluşturuldu.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Text = string.Empty;
                CmbBrans.Text = string.Empty;
                CmbDoktor.Text = string.Empty;
                RchSikayet.Text = string.Empty;
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void BtnCıkıs_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnMesaj_Click(object sender, EventArgs e)
        {
            FrmHastaDuy fr = new FrmHastaDuy();
            fr.ID=LblID.Text;   
            fr.Show();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
    }
}
