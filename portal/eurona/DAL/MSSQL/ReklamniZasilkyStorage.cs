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
    internal sealed class ReklamniZasilkyStorage : MSSQLStorage<ReklamniZasilky>
    {
        public ReklamniZasilkyStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString)
        {
        }

        private ReklamniZasilky GetItem(DataRow record)
        {
            ReklamniZasilky reklamniZasilky = new ReklamniZasilky();
            reklamniZasilky.Id = Convert.ToInt32(record["Id_zasilky"]);
            reklamniZasilky.Popis = record["Popis"].ToString();
            reklamniZasilky.Default_souhlas = Convert.ToBoolean(record["Default_souhlas"]);
            return reklamniZasilky;
        }

        public override List<ReklamniZasilky> Read(object criteria)
        {
            List<ReklamniZasilky> list = new List<ReklamniZasilky>();
            using (SqlConnection connection = Connect())
            {
                string sql = @"SELECT * from tReklamniZasilky";

                DataTable table = Query<DataTable>(connection, sql);
                foreach (DataRow dr in table.Rows) list.Add(GetItem(dr));
            }
            return list;
        }

        public override int Count(object criteria)
        {
            throw new NotImplementedException();
        }


        public override void Create(ReklamniZasilky entity)
        {
            using (SqlConnection connection = Connect())
            {
                string cmd = "INSERT INTO tReklamniZasilky (Id_zasilky, Popis, Default_souhlas) VALUES (@Id, @Popis, @DefaultSouhlas)";
                Exec(connection, cmd, new SqlParameter("@Id", entity.Id), new SqlParameter("@Popis", entity.Popis), new SqlParameter("@Popis", entity.Default_souhlas));
            }
        }

        public override void Update(ReklamniZasilky entity)
        {
            using (SqlConnection connection = Connect())
            {
                string cmd = "UPDATE tReklamniZasilky SET Popis=@Popis, Default_souhlas=@Default_souhlas WHERE Id_zasilky=@Id";
                Exec(connection, cmd, new SqlParameter("@Id", entity.Id), new SqlParameter("@Popis", entity.Popis), new SqlParameter("@Popis", entity.Default_souhlas));
            }
        }

        public override void Delete(ReklamniZasilky entity)
        {
            using (SqlConnection connection = Connect())
            {
                string cmd = "DELETE FROM tReklamniZasilky WHERE Id_zasilky=@Id";
                Exec(connection, cmd, new SqlParameter("@Id", entity.Id), new SqlParameter("@Popis", entity.Popis), new SqlParameter("@Popis", entity.Default_souhlas));
            }
        }

    }
}
