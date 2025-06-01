using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proje_Hastane
{
    public partial class FrmBranş : Form
    {
        public FrmBranş()
        {
            InitializeComponent();
        }
        SqlBaglantı bgl=new SqlBaglantı();
        private void BranşYenile()
        {
            DataTable dt2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter("select * from Tbl_Brans", bgl.baglanti());
            da2.Fill(dt2);
            dataGridView1.DataSource = dt2;
            bgl.baglanti().Close();
        }
       
        private void FrmBranş_Load(object sender, EventArgs e)
        {
          BranşYenile();
            dataGridView1.ReadOnly = true;


        }

        private void BtnEkle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtBransAd.Text))
            {
                MessageBox.Show("Lütfen branş adını girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }

            
            SqlCommand bransKontrol = new SqlCommand("select count(*) from Tbl_Brans where BransAd = @p1", bgl.baglanti());
            bransKontrol.Parameters.AddWithValue("@p1", TxtBransAd.Text); 
            int mevcutKayit = Convert.ToInt32(bransKontrol.ExecuteScalar());
            bgl.baglanti().Close();

            if (mevcutKayit > 0)
            {
                MessageBox.Show("Bu branş adı zaten mevcut. Lütfen farklı bir branş adı girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }
            try
            {
                SqlCommand cmd = new SqlCommand("insert into Tbl_Brans (BransAd)values(@p1)", bgl.baglanti());
                cmd.Parameters.AddWithValue("@p1", TxtBransAd.Text);
                cmd.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Kayıt işlemi Tamamlandı.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                BranşYenile();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           


        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmSekreterDetay fr = new FrmSekreterDetay();
            fr.Show();
            this.Close();
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            SqlCommand cmd1 = new SqlCommand("delete from Tbl_Brans where BransID=@p1", bgl.baglanti());
            cmd1.Parameters.AddWithValue("@p1", TxtBransId.Text);
            cmd1.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Kayıt silme işlemi Tamamlandı.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            BranşYenile();
           
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            TxtBransId.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            TxtBransAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
        }

        private void BtnGüncelle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtBransAd.Text))
            {
                MessageBox.Show("Lütfen branş adı alanını doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SqlCommand bransKontrol = new SqlCommand("select count(*) from Tbl_Brans where BransAd = @p1 and BransId != @p2", bgl.baglanti());
            bransKontrol.Parameters.AddWithValue("@p1", TxtBransAd.Text); 
            bransKontrol.Parameters.AddWithValue("@p2", TxtBransId.Text); 
            int mevcutKayit = Convert.ToInt32(bransKontrol.ExecuteScalar());
            bgl.baglanti().Close();

            if (mevcutKayit > 0)
            {
                MessageBox.Show("Bu branş adı zaten mevcut. Lütfen farklı bir branş adı girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }

            try
            {
                SqlCommand cmd2 = new SqlCommand("Update Tbl_Brans set BransAd=@p1", bgl.baglanti());
                cmd2.Parameters.AddWithValue("@p1", TxtBransAd.Text);
                cmd2.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Kayıt güncelleme işlemi Tamamlandı.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TxtBransAd.Text = string.Empty;
                TxtBransId.Text = string.Empty;
                BranşYenile();

            }
           
            catch (Exception ex)
{
               
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
