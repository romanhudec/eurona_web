using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Eurona.DAL.Entities;
using CMS.MSSQL;

namespace Eurona.DAL.MSSQL
{
    internal sealed class ReklamniZasilkySouhlasStorage : MSSQLStorage<ReklamniZasilkySouhlas>
    {
        public ReklamniZasilkySouhlasStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString)
        {
        }

        private ReklamniZasilkySouhlas GetItem(DataRow record)
        {
            ReklamniZasilkySouhlas reklamniZasilkySouhlas = new ReklamniZasilkySouhlas();
            reklamniZasilkySouhlas.Id_zasilky = Convert.ToInt32(record["Id_zasilky"]);
            reklamniZasilkySouhlas.Id_odberatele = Convert.ToInt32(record["Id_odberatele"]);            
            reklamniZasilkySouhlas.Souhlas = Convert.ToBoolean(record["Souhlas"]);
            reklamniZasilkySouhlas.Datum_zmeny = Convert.ToDateTime(record["Datum_zmeny"]);
            return reklamniZasilkySouhlas;
        }

        public override List<ReklamniZasilkySouhlas> Read(object criteria)
        {
            List<ReklamniZasilkySouhlas> list = new List<ReklamniZasilkySouhlas>();
            if (criteria is ReklamniZasilkySouhlas.ReadByOdberatel)
            {
                using (SqlConnection connection = Connect())
                {
                    string sql = @"SELECT * from tReklamniZasilkySouhlas WHERE Id_zasilky=@Id_zasilky AND Id_odberatele=@Id_odberatele";

                    DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@Id_zasilky", (criteria as ReklamniZasilkySouhlas.ReadByOdberatel).Id_zasilky),
                        new SqlParameter("@Id_odberatele", (criteria as ReklamniZasilkySouhlas.ReadByOdberatel).Id_odberatele));
                    foreach (DataRow dr in table.Rows) list.Add(GetItem(dr));
                }
            }
            else
            {
                using (SqlConnection connection = Connect())
                {
                    string sql = @"SELECT * from tReklamniZasilkySouhlas";

                    DataTable table = Query<DataTable>(connection, sql);
                    foreach (DataRow dr in table.Rows) list.Add(GetItem(dr));
                }
            }
            return list;
        }

        public override int Count(object criteria)
        {
            throw new NotImplementedException();
        }


        public override void Create(ReklamniZasilkySouhlas entity)
        {
            using (SqlConnection connection = Connect())
            {
                string cmd = "INSERT INTO tReklamniZasilkySouhlas (Id_zasilky, Id_odberatele, Souhlas, Datum_zmeny) VALUES (@Id_zasilky, @Id_odberatele, @Souhlas, GETDATE())";
                Exec(connection, cmd, new SqlParameter("@Id_zasilky", entity.Id_zasilky), new SqlParameter("@Id_odberatele", entity.Id_odberatele), new SqlParameter("@Souhlas", entity.Souhlas));
            }
        }

        public override void Update(ReklamniZasilkySouhlas entity)
        {
            using (SqlConnection connection = Connect())
            {
                string cmd = "UPDATE tReklamniZasilkySouhlas SET Souhlas=@Souhlas, Datum_zmeny=GETDATE() WHERE Id_zasilky=@Id_zasilky AND Id_odberatele=@Id_odberatele";
                Exec(connection, cmd, new SqlParameter("@Id_zasilky", entity.Id_zasilky), new SqlParameter("@Id_odberatele", entity.Id_odberatele), new SqlParameter("@Souhlas", entity.Souhlas));
            }
        }

        public override void Delete(ReklamniZasilkySouhlas entity)
        {
            using (SqlConnection connection = Connect())
            {
                string cmd = "DELETE FROM tReklamniZasilkySouhlas WHERE Id_zasilky=@Id_zasilky AND Id_odberatele=@Id_odberatele";
                Exec(connection, cmd, new SqlParameter("@Id_zasilky", entity.Id_zasilky), new SqlParameter("@Id_odberatele", entity.Id_odberatele));
            }
        }

    }
}
