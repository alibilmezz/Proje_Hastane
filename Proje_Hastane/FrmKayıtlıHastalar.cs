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
    public partial class FrmKayıtlıHastalar : Form
    {
        public FrmKayıtlıHastalar()
        {
            InitializeComponent();
        }
        SqlBaglantı bgl=new SqlBaglantı();

        private void FrmKayıtlıHastalar_Load(object sender, EventArgs e)
        {
            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter("select * from Tbl_Hasta", bgl.baglanti());
            da1.Fill(dt1);
            dataGridView1.DataSource = dt1;
            dataGridView1.ReadOnly = true;

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            TxtID.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            TxtAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            MskTC.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtID.Text) ||
                string.IsNullOrWhiteSpace(TxtAd.Text) ||
                string.IsNullOrWhiteSpace(MskTC.Text) ||
                string.IsNullOrWhiteSpace(RchDuyuru.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }

            try
            {
                
                SqlCommand cmd = new SqlCommand("INSERT INTO Tbl_HastaMesaj (HastaID, HastaDuyuru) VALUES (@p1, @p2)", bgl.baglanti());
                cmd.Parameters.AddWithValue("@p1", TxtID.Text); 
                cmd.Parameters.AddWithValue("@p2", RchDuyuru.Text); 
                cmd.ExecuteNonQuery();
                bgl.baglanti().Close();

                MessageBox.Show("Duyuru başarıyla oluşturuldu.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);

                
                RchDuyuru.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
