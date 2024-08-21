using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
namespace Otomasyon_LeftSoft
{
    class SqlBaglantisi
    {
        public OleDbConnection sqlbaglan()
        {
            // Sql Bağlantı Sınıfı //
            OleDbConnection baglan = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=LeftSoft.mdb");
            baglan.Open();
            return baglan;
        }
    }
}
