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
using System.Data.Common;

namespace Proje_Hastane
{
    public partial class FrmDoktorPan : Form
    {
        public FrmDoktorPan()
        {
            InitializeComponent();
        }
        SqlBaglantı bgl=new SqlBaglantı();
        private void DoktoYenile()
        {
            DataTable dt2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter("select * from Tbl_Doktorlar", bgl.baglanti());
            da2.Fill(dt2);
            dataGridView1.DataSource = dt2;
            bgl.baglanti().Close();
        }
        private void FrmDoktorPan_Load(object sender, EventArgs e)
        {
            DoktoYenile();
            //COMBOBOXA BRANS ÇEKME
            SqlCommand cmd = new SqlCommand("select * from Tbl_Brans",bgl.baglanti());
            DbDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                CmbBrans.Items.Add(dr[1]);
            }
            dataGridView1.ReadOnly = true;


        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmSekreterDetay fr=new FrmSekreterDetay();
            fr.Show();
            this.Close();
        }
        private void Temzilik()
        {
            TxtAd.Text = string.Empty;
            TxtSoyad.Text = string.Empty;
            TxtSifre.Text = string.Empty;
            CmbBrans.Text = string.Empty;
            MskTc.Text = string.Empty;  
        }

        private void BtnEkle_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtAd.Text) ||
                string.IsNullOrWhiteSpace(TxtSoyad.Text) ||
                string.IsNullOrWhiteSpace(CmbBrans.Text) ||
                string.IsNullOrWhiteSpace(MskTc.Text) ||
                string.IsNullOrWhiteSpace(TxtSifre.Text))
                {
                    MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (MskTc.Text.Length != 11 || !MskTc.Text.All(char.IsDigit))
                {
                    MessageBox.Show("TC Kimlik Numarası 11 haneli ve sadece rakamlardan oluşmalıdır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                SqlCommand tcKontrol = new SqlCommand("select count(*) from Tbl_Doktorlar where DoktorTc = @p1", bgl.baglanti());
                tcKontrol.Parameters.AddWithValue("@p1", MskTc.Text);
                int mevcutKayit = Convert.ToInt32(tcKontrol.ExecuteScalar());
                bgl.baglanti().Close();

                if (mevcutKayit > 0)
                {
                    MessageBox.Show("Bu TC kimlik numarası zaten kayıtlı. Lütfen farklı bir TC girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SqlCommand cmd = new SqlCommand("insert into Tbl_Doktorlar (DoktorAd,DoktorSoyad,DoktorBrans,DoktorTc,DoktorSifre)values(@p1,@p2,@p3,@p4,@p5)", bgl.baglanti());
                cmd.Parameters.AddWithValue("@p1", TxtAd.Text);
                cmd.Parameters.AddWithValue("@p2", TxtSoyad.Text);
                cmd.Parameters.AddWithValue("@p3", CmbBrans.Text);
                cmd.Parameters.AddWithValue("@p4", MskTc.Text);
                cmd.Parameters.AddWithValue("@p5", TxtSifre.Text);
                cmd.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Kayıt işlemi Tamamlandı.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DataTable dt2 = new DataTable();
                SqlDataAdapter da2 = new SqlDataAdapter("select * from Tbl_Doktorlar", bgl.baglanti());
                da2.Fill(dt2);
                dataGridView1.DataSource = dt2;
                bgl.baglanti().Close();
                Temzilik();

            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen=dataGridView1.SelectedCells[0].RowIndex;
            TxtAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            TxtSoyad.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            CmbBrans.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            MskTc.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();
            TxtSifre.Text = dataGridView1.Rows[secilen].Cells[5].Value.ToString();
            label7.Text= MskTc.Text;

        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtAd.Text) ||
                string.IsNullOrWhiteSpace(TxtSoyad.Text) ||
                string.IsNullOrWhiteSpace(MskTc.Text) ||
                string.IsNullOrWhiteSpace(TxtSifre.Text) ||
                string.IsNullOrWhiteSpace(CmbBrans.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // İşlemi durdur
            }
            try
            {
                SqlCommand cmd1 = new SqlCommand("delete from Tbl_Doktorlar where DoktorTc=@p1", bgl.baglanti());
                cmd1.Parameters.AddWithValue("@p1", MskTc.Text);
                cmd1.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Kayıt silme işlemi Tamamlandı.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DoktoYenile();
                Temzilik();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void BtnGüncelle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtAd.Text) ||
             string.IsNullOrWhiteSpace(TxtSoyad.Text) ||
             string.IsNullOrWhiteSpace(MskTc.Text) ||
             string.IsNullOrWhiteSpace(TxtSifre.Text) ||
             string.IsNullOrWhiteSpace(CmbBrans.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }
            if (MskTc.Text.Length != 11 || !MskTc.Text.All(char.IsDigit))
            {
                MessageBox.Show("TC Kimlik Numarası 11 haneli ve sadece rakamlardan oluşmalıdır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SqlCommand tcKontrol = new SqlCommand("select count(*) from Tbl_Doktorlar where DoktorTc = @p1 and DoktorTc != @p2", bgl.baglanti());
            tcKontrol.Parameters.AddWithValue("@p1", MskTc.Text); 
            tcKontrol.Parameters.AddWithValue("@p2", label7.Text); 
            int mevcutKayit = Convert.ToInt32(tcKontrol.ExecuteScalar());
            bgl.baglanti().Close();

            if (mevcutKayit > 0)
            {
                MessageBox.Show("Bu TC numarası zaten mevcut. Lütfen farklı bir TC numarası girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }
            try
            {
                SqlCommand cmd2 = new SqlCommand("Update Tbl_Doktorlar set DoktorAd=@p1,DoktorSoyad=@p2,DoktorBrans=@p3,DoktorTc=@p4,DoktorSifre=@p5 where DoktorTc=@p6 ", bgl.baglanti());
                cmd2.Parameters.AddWithValue("@p1", TxtAd.Text);
                cmd2.Parameters.AddWithValue("@p2", TxtSoyad.Text);
                cmd2.Parameters.AddWithValue("@p3", CmbBrans.Text);
                cmd2.Parameters.AddWithValue("@p4", MskTc.Text);
                cmd2.Parameters.AddWithValue("@p5", TxtSifre.Text);
                cmd2.Parameters.AddWithValue("@p6", label7.Text);
                cmd2.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Güncelleme işlemi Tamamlandı.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DoktoYenile();
                Temzilik();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
