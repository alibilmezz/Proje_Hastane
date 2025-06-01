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
    public partial class FrmHastaUye : Form
    {
        public FrmHastaUye()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        SqlBaglantı bgl=new SqlBaglantı();
        private void BtnKayıt_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(TxtAd.Text) ||
                string.IsNullOrWhiteSpace(TxtSoyad.Text) ||
                string.IsNullOrWhiteSpace(MskTc2.Text) ||
                string.IsNullOrWhiteSpace(MskTelno.Text) ||
                string.IsNullOrWhiteSpace(TxtSifre2.Text) ||
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
            SqlCommand tcKontrol = new SqlCommand("select count(*) from Tbl_Hasta where HastaTc = @p1", bgl.baglanti());
            tcKontrol.Parameters.AddWithValue("@p1", MskTc2.Text); 
            int mevcutKayit = Convert.ToInt32(tcKontrol.ExecuteScalar());
            bgl.baglanti().Close();

            if (mevcutKayit > 0)
            {
                MessageBox.Show("Bu TC numarası zaten kayıtlı. Lütfen farklı bir TC numarası girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }
            string telno = new string(MskTelno.Text.Where(char.IsDigit).ToArray());
            if (telno.Length != 10 || !telno.StartsWith("5"))
            {
                MessageBox.Show("Telefon numarası 10 haneli, sadece rakamlardan oluşmalı ve '5' ile başlamalıdır.",
                                "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SqlCommand telKontrol = new SqlCommand("select count(*) from Tbl_Hasta where HastaTelefon = @p1", bgl.baglanti());
            telKontrol.Parameters.AddWithValue("@p1", MskTelno.Text);
            int mevcutTelefon = Convert.ToInt32(telKontrol.ExecuteScalar());
            bgl.baglanti().Close();

            if (mevcutTelefon > 0)
            {
                MessageBox.Show("Bu telefon numarası zaten kayıtlı. Lütfen farklı bir telefon numarası girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try

            {
                SqlCommand cmd = new SqlCommand("insert into Tbl_Hasta (HastaAd,HastaSoyad,HastaTc,HastaTelefon,HastaSifre,HastaCinsiyet)values(@p1,@p2,@p3,@p4,@p5,@p6)", bgl.baglanti());
                cmd.Parameters.AddWithValue("@p1", TxtAd.Text);
                cmd.Parameters.AddWithValue("@p2", TxtSoyad.Text);
                cmd.Parameters.AddWithValue("@p3", MskTc2.Text);
                cmd.Parameters.AddWithValue("@p4", MskTelno.Text);
                cmd.Parameters.AddWithValue("@p5", TxtSifre2.Text);
                cmd.Parameters.AddWithValue("@p6", CmbCinsiyet.Text);
                cmd.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Kaydınız Gerçekleşmiştir Şifreniz: " + TxtSifre2.Text, ("Bilgilendirme"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();

            }
            catch (Exception ex)
            {
              
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }






        }

        private void FrmHastaUye_Load(object sender, EventArgs e)
        {

        }
    }
}
