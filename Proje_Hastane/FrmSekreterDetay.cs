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
using Microsoft.SqlServer.Server;

namespace Proje_Hastane
{
    public partial class FrmSekreterDetay : Form
    {
        public FrmSekreterDetay()
        {
            InitializeComponent();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
        public string Tc;
        SqlBaglantı bgl=new SqlBaglantı();
        private void FrmSekreterDetay_Load(object sender, EventArgs e)
        {
            LblTc.Text = Tc;
            SqlCommand cmd = new SqlCommand("select SekreterAd , SekreterSoyad from Tbl_Sekreter where SekreterTc=@p1", bgl.baglanti());
            cmd.Parameters.AddWithValue("@p1", LblTc.Text);
            SqlDataReader dr1=cmd.ExecuteReader();
            while (dr1.Read())
            {
                LblAdsoyad.Text = dr1[0]+" "+dr1[1];

            } 
            bgl.baglanti().Close(); 
          
            //BRANŞLARI COMBOBOXA ÇEKME
            SqlCommand cmd1 = new SqlCommand("Select BransAd From Tbl_Brans", bgl.baglanti());

            SqlDataReader dr2 =cmd1.ExecuteReader(); 
            while (dr2.Read())
            {
                CmbBrans.Items.Add(dr2[0]);
            }
            bgl.baglanti().Close();
            //randevuları çekme 
            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter("select * from Tbl_Randevu", bgl.baglanti());
            da1.Fill(dt1);
            dataGridView3.DataSource = dt1;
            dataGridView3.ReadOnly = true;


        }
       

        private void RandevuYenile()
        {
            DataTable dt2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter("select * from Tbl_Randevu", bgl.baglanti());
            da2.Fill(dt2);
            dataGridView3.DataSource = dt2;
            bgl.baglanti().Close();
        }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MskTarih.Text) ||
                string.IsNullOrWhiteSpace(MskSaat.Text) ||
                string.IsNullOrWhiteSpace(CmbBrans.Text) ||
                string.IsNullOrWhiteSpace(CmbDoktor.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }
            DateTime tarih;
            if (!DateTime.TryParseExact(MskTarih.Text, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out tarih))
            {
                MessageBox.Show("Geçersiz tarih formatı. Lütfen tarihi 'GG.AA.YYYY' şeklinde girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime saat;
            if (!DateTime.TryParseExact(MskSaat.Text, "HH:mm", null, System.Globalization.DateTimeStyles.None, out saat))
            {
                MessageBox.Show("Geçersiz saat formatı. Lütfen saati 'SS:DD' şeklinde girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                SqlCommand cmd2 = new SqlCommand("insert into Tbl_Randevu (RandevuTarih,RandevuSaati,RandevuBrans,RandevuDoktor,RandevuDurum)values(@p1,@p2,@p3,@p4,@p5)", bgl.baglanti());
                cmd2.Parameters.AddWithValue("@p1", MskTarih.Text);
                cmd2.Parameters.AddWithValue("@p2", MskSaat.Text);
                cmd2.Parameters.AddWithValue("@p3", CmbBrans.Text);
                cmd2.Parameters.AddWithValue("@p4", CmbDoktor.Text);
                cmd2.Parameters.AddWithValue("@p5", ChkDurum.Checked);
                cmd2.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Randevu Oluşturuldu.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MskTarih.Text = string.Empty;
                MskSaat.Text = string.Empty;
                CmbBrans.Text = string.Empty;
                CmbDoktor.Text = string.Empty;
                TxtId.Text = string.Empty;
                RandevuYenile();

            }
            catch(Exception ex) 
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 fr = new Form1();
            fr.Show();
            this.Hide();
        }

        private void CmbBrans_SelectedIndexChanged(object sender, EventArgs e)
        {
            CmbDoktor.Items.Clear();
            CmbDoktor.Text = string.Empty;  
            SqlCommand cmd3 = new SqlCommand("Select DoktorAd,DoktorSoyad From Tbl_Doktorlar where DoktorBrans=@p1",bgl.baglanti());
            cmd3.Parameters.AddWithValue("@p1",CmbBrans.Text);
            SqlDataReader dr3 = cmd3.ExecuteReader();
            while (dr3.Read())
            {
                CmbDoktor.Items.Add(dr3[0]+" "+dr3[1]); 
            }
            bgl.baglanti().Close();
        }

        private void BtnOluştur_Click(object sender, EventArgs e)
        {

            SqlCommand cmd4 = new SqlCommand("insert into Tbl_Duyuru (DuyuruAd)values (@p1)", bgl.baglanti());
            cmd4.Parameters.AddWithValue("@p1",RchDuyuru.Text);
            cmd4.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Duyuru Oluşturuldu.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RchDuyuru.Text= string.Empty;   
        }

        private void BtnDoktorPan_Click(object sender, EventArgs e)
        {
            FrmDoktorPan drp = new FrmDoktorPan();
            drp.Show();
            
        }

        private void BtnBransPan_Click(object sender, EventArgs e)
        {
            FrmBranş brp=new FrmBranş();
            brp.Show();
            

        }

        

        private void dataGridView3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView3.SelectedCells[0].RowIndex;
            TxtId.Text=dataGridView3.Rows[secilen].Cells[0].Value.ToString();
            MskTarih.Text = dataGridView3.Rows[secilen].Cells[1].Value.ToString();
            MskSaat.Text = dataGridView3.Rows[secilen].Cells[2].Value.ToString();
            CmbBrans.Text = dataGridView3.Rows[secilen].Cells[3].Value.ToString();
            CmbDoktor.Text = dataGridView3.Rows[secilen].Cells[4].Value.ToString();
            MskTc.Text = dataGridView3.Rows[secilen].Cells[5].Value.ToString();
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtId.Text))
            {
                MessageBox.Show("Lütfen silmek istediğiniz kaydın ID bilgisini girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }
            try
            {
                SqlCommand cmd = new SqlCommand("delete from Tbl_Randevu where RandevuID=@p1", bgl.baglanti());
                cmd.Parameters.AddWithValue("@p1", TxtId.Text);
                cmd.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Kayıt silme işlemi Tamamlandı.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MskTarih.Text = string.Empty;
                MskSaat.Text = string.Empty;
                CmbBrans.Text = string.Empty;
                CmbDoktor.Text = string.Empty;
                TxtId.Text = string.Empty;
                RandevuYenile();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmDuyurular frm = new FrmDuyurular();
            frm.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmSekreterBilgiDüzenleme fr=new FrmSekreterBilgiDüzenleme();
            fr.tc = LblTc.Text;
            fr.Show();


        }

        private void button3_Click(object sender, EventArgs e)
        {
            FrmKayıtlıHastalar fr=new FrmKayıtlıHastalar();
            fr.Show();

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }
    }
}
