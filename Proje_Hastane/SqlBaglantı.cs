using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace Proje_Hastane
{
    internal class SqlBaglantı
    {
        public SqlConnection baglanti()
        {
            SqlConnection baglan = new SqlConnection("Data Source=Mamimonster\\SQLEXPRESS;Initial Catalog=HastaneProje;Integrated Security=True;Encrypt=False");
            baglan.Open();
            return baglan;
        }
    }
}
