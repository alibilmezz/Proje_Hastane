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
    public partial class FrmHastaDuy : Form
    {
        public FrmHastaDuy()
        {
            InitializeComponent();
        }
        public string ID    ;

        SqlBaglantı bgl=new SqlBaglantı();
        private void FrmHastaDuy_Load(object sender, EventArgs e)
        {
            label2.Text = ID;
            SqlCommand cmd = new SqlCommand("SELECT HastaDuyuru FROM Tbl_HastaMesaj WHERE HastaID = @p1", bgl.baglanti());
            cmd.Parameters.AddWithValue("@p1", label2.Text); 

            SqlDataAdapter da = new SqlDataAdapter(cmd); 
            DataTable dt = new DataTable();
            dataGridView1.ReadOnly = true;

            try
            {
                da.Fill(dt); 
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                bgl.baglanti().Close(); 
            }
        }
    }
}
