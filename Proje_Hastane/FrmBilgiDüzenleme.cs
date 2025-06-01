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
using System.Reflection.Emit;

namespace Proje_Hastane
{
    public partial class FrmBilgiDüzenleme : Form
    {
        public FrmBilgiDüzenleme()
        {
            InitializeComponent();
        }
        public string ıd;
        SqlBaglantı baglantı = new SqlBaglantı();
        private void FrmBilgiDüzenleme_Load(object sender, EventArgs e)
        {
            textBox1.Text= ıd;
            SqlCommand cmd = new SqlCommand("select * from Tbl_Hasta where HastaID=@p1", baglantı.baglanti());
            cmd.Parameters.AddWithValue("@p1",textBox1.Text); 
            SqlDataReader dr= cmd.ExecuteReader();
            while (dr.Read())
            {
                TxtAd.Text = dr[1].ToString();
                TxtSoyad.Text = dr[2].ToString();
                MskTc2.Text = dr[3].ToString();
                MskTelno.Text = dr[4].ToString();
                TxtSifre2.Text = dr[5].ToString();
                CmbCinsiyet.Text = dr[6].ToString();
            }
            baglantı.baglanti().Close();
            
        }

        private void BtnKayıt_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtAd.Text) ||
                string.IsNullOrWhiteSpace(TxtSoyad.Text) ||
                string.IsNullOrWhiteSpace(MskTelno.Text) ||
                string.IsNullOrWhiteSpace(TxtSifre2.Text) ||
                string.IsNullOrWhiteSpace(MskTc2.Text) ||
                string.IsNullOrWhiteSpace(CmbCinsiyet.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }
            if (MskTc2.Text.Length != 11 || !MskTc2.Text.All(char.IsDigit))
            {
                MessageBox.Show("TC Kimlik Numarası 11 haneli ve sadece rakamlardan oluşmalıdır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SqlCommand tcKontrol = new SqlCommand("select count(*) from Tbl_Hasta where HastaTc = @p1", baglantı.baglanti());
            tcKontrol.Parameters.AddWithValue("@p1", MskTc2.Text);
            int mevcutKayit = Convert.ToInt32(tcKontrol.ExecuteScalar());
            baglantı.baglanti().Close();

            
            string telno = new string(MskTelno.Text.Where(char.IsDigit).ToArray());
            if (telno.Length != 10 || !telno.StartsWith("5"))
            {
                MessageBox.Show("Telefon numarası 10 haneli, sadece rakamlardan oluşmalı ve '5' ile başlamalıdır.",
                                "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SqlCommand telKontrol = new SqlCommand("select count(*) from Tbl_Hasta where HastaTelefon = @p1", baglantı.baglanti());
            telKontrol.Parameters.AddWithValue("@p1", MskTelno.Text);
            int mevcutTelefon = Convert.ToInt32(telKontrol.ExecuteScalar());
            baglantı.baglanti().Close();

            if (mevcutTelefon > 0)
            {
                MessageBox.Show("Bu telefon numarası zaten kayıtlı. Lütfen farklı bir telefon numarası girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                SqlCommand komutgüncelle = new SqlCommand("Update Tbl_Hasta Set HastaAd=@a1,HastaSoyad=@a2,HastaTelefon=@a3,HastaSifre=@a4,HastaCinsiyet=@a5,HastaTc=@a6 where HastaID=@p1 ", baglantı.baglanti());
                komutgüncelle.Parameters.AddWithValue("@a1", TxtAd.Text);
                komutgüncelle.Parameters.AddWithValue("@a2", TxtSoyad.Text);
                komutgüncelle.Parameters.AddWithValue("@a3", MskTelno.Text);
                komutgüncelle.Parameters.AddWithValue("@a4", TxtSifre2.Text);
                komutgüncelle.Parameters.AddWithValue("@a5", CmbCinsiyet.Text);
                komutgüncelle.Parameters.AddWithValue("@a6", MskTc2.Text);
                komutgüncelle.Parameters.AddWithValue("@p1", textBox1.Text);

                komutgüncelle.ExecuteNonQuery();
                baglantı.baglanti().Close();
                MessageBox.Show("Bilgi Güncellendi.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }


        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
