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
    public partial class FrmDoktorBilgiDüzenle : Form
    {
        public FrmDoktorBilgiDüzenle()
        {
            InitializeComponent();
        }
        public string tc;
        SqlBaglantı bgl=new SqlBaglantı();
        private void FrmDoktorBilgiDüzenle_Load(object sender, EventArgs e)
        {
            MskTc2.Text = tc;
            SqlCommand cmd = new SqlCommand("select * from Tbl_Doktorlar where DoktorTc=@p1",bgl.baglanti());
            cmd.Parameters.AddWithValue("@p1", MskTc2.Text);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                TxtAd.Text = dr[1].ToString();
                TxtSoyad.Text = dr[2].ToString();              
                TxtSifre2.Text = dr[5].ToString();
                CmbBrans.Text = dr[3].ToString();
            }
            bgl.baglanti().Close();
            //branşları çekme
            SqlCommand cmd2 = new SqlCommand("select BransAd from Tbl_Brans", bgl.baglanti());
            SqlDataReader dr2 = cmd2.ExecuteReader();
            while (dr2.Read())
            {
                CmbBrans.Items.Add(dr2[0]);
            }
            bgl.baglanti().Close();

        }

        private void BtnGüncelle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtAd.Text) ||
            string.IsNullOrWhiteSpace(TxtSoyad.Text) ||
            string.IsNullOrWhiteSpace(MskTc2.Text) ||
            string.IsNullOrWhiteSpace(TxtSifre2.Text) ||
            string.IsNullOrWhiteSpace(CmbBrans.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }
            if (MskTc2.Text.Length != 11 || !MskTc2.Text.All(char.IsDigit))
            {
                MessageBox.Show("TC Kimlik Numarası 11 haneli ve sadece rakamlardan oluşmalıdır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SqlCommand tcKontrol = new SqlCommand("select count(*) from Tbl_Doktorlar where DoktorTc = @p1 and DoktorTc != @p2", bgl.baglanti());
            tcKontrol.Parameters.AddWithValue("@p1", MskTc2.Text); 
            tcKontrol.Parameters.AddWithValue("@p2", tc);
            int mevcutKayit = Convert.ToInt32(tcKontrol.ExecuteScalar());
            bgl.baglanti().Close();

            if (mevcutKayit > 0)
            {
                MessageBox.Show("Bu TC kimlik numarası zaten başka bir doktora ait. Lütfen farklı bir TC girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }
            try
            {
                SqlCommand komutgüncelle = new SqlCommand("Update Tbl_Doktorlar Set DoktorAd=@a1,DoktorSoyad=@a2,DoktorTc=@a3,DoktorSifre=@a4,DoktorBrans=@a5 where DoktorTc=@p1", bgl.baglanti());
                komutgüncelle.Parameters.AddWithValue("@p1", tc);
                komutgüncelle.Parameters.AddWithValue("@a1", TxtAd.Text);
                komutgüncelle.Parameters.AddWithValue("@a2", TxtSoyad.Text);
                komutgüncelle.Parameters.AddWithValue("@a3", MskTc2.Text);
                komutgüncelle.Parameters.AddWithValue("@a4", TxtSifre2.Text);
                komutgüncelle.Parameters.AddWithValue("@a5", CmbBrans.Text);
                komutgüncelle.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Bilgi Güncellendi.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();

            }
            catch(Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
