using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using CMS.MSSQL;
using CMS.Entities;
using Eurona.Common.DAL.Entities;

namespace Eurona.Common.DAL.MSSQL {
    [Serializable]
    public sealed class ZavozoveMistoStorage : MSSQLStorage<ZavozoveMisto> {
        private string entitySelect = @"SELECT * FROM tShpZavozoveMisto";
        public ZavozoveMistoStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static ZavozoveMisto GetError(DataRow record) {
            ZavozoveMisto entiry = new ZavozoveMisto();
            entiry.Id = Convert.ToInt32(record["ZavozoveMistoId"]);
            entiry.Kod = Convert.ToInt32(record["Kod"]);
            entiry.Stat = Convert.ToString(record["Stat"]);
            entiry.Mesto = Convert.ToString(record["Mesto"]);
            entiry.Psc = Convert.ToString(record["Psc"]);
            entiry.Popis = Convert.ToString(record["Popis"]);
            entiry.DatumACas = ConvertNullable.ToDateTime(record["DatumACas"]);
            entiry.DatumACas_Skryti = ConvertNullable.ToDateTime(record["DatumACas_Skryti"]);
            entiry.OsobniOdberVSidleSpolecnosti = Convert.ToBoolean(record["OsobniOdberVSidleSpolecnosti"]);
            entiry.OsobniOdberPovoleneCasy = Convert.ToString(record["OsobniOdberPovoleneCasy"]);
            entiry.OsobniOdberAdresaSidlaSpolecnosti = Convert.ToString(record["OsobniOdberAdresaSidlaSpolecnosti"]);
            return entiry;
        }

        public override List<ZavozoveMisto> Read(object criteria) {           
            if (criteria is ZavozoveMisto.ReadById) return LoadById((criteria as ZavozoveMisto.ReadById).Id);
            if (criteria is ZavozoveMisto.ReadBy) return LoadBy((criteria as ZavozoveMisto.ReadBy).OsobniOdberVSidleSpolecnosti);
            if (criteria is ZavozoveMisto.ReadByMesto) return LoadByMesto((criteria as ZavozoveMisto.ReadByMesto).Mesto);
            if (criteria is ZavozoveMisto.ReadJenAktualiByKod) return LoadJenAktualiByKod((criteria as ZavozoveMisto.ReadJenAktualiByKod).Kod);
            if (criteria is ZavozoveMisto.ReadJenAktualiByMesto) return LoadJenAktualiByMesto((criteria as ZavozoveMisto.ReadJenAktualiByMesto).Mesto);
            if (criteria is ZavozoveMisto.ReadOnlyMestoDistinctByStat) return LoadOnlyMestoDistinct((criteria as ZavozoveMisto.ReadOnlyMestoDistinctByStat).Stat);

            SqlParameter[] @params = null;
            string sql = entitySelect + " ORDER BY DatumACas DESC";
            List<ZavozoveMisto> list = new List<ZavozoveMisto>();

            using (SqlConnection connection = Connect()) {
                DataTable table = null;
                table = Query<DataTable>(connection, sql, @params);
                foreach (DataRow dr in table.Rows)
                    list.Add(GetError(dr));

            }
            return list;
        }

        public List<ZavozoveMisto> LoadBy(bool osobniOdberVSidleSpolecnosti) {
            List<ZavozoveMisto> list = new List<ZavozoveMisto>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE OsobniOdberVSidleSpolecnosti = @osobniOdberVSidleSpolecnosti";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@osobniOdberVSidleSpolecnosti", osobniOdberVSidleSpolecnosti));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetError(dr));
            }
            return list;
        }

        public List<ZavozoveMisto> LoadById(int id) {
            List<ZavozoveMisto> list = new List<ZavozoveMisto>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE ZavozoveMistoId = @Id";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@Id", id));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetError(dr));
            }
            return list;
        }

        public List<ZavozoveMisto> LoadOnlyMestoDistinct(string stat) {
            List<ZavozoveMisto> list = new List<ZavozoveMisto>();
            using (SqlConnection connection = Connect()) {
                string sql = @"SELECT DISTINCT Stat=@Stat,  Mesto, Kod, Psc, Popis, DatumACas=GETDATE(), DatumACas_Skryti=GETDATE(), ZavozoveMistoId=0, OsobniOdberPovoleneCasy=NULL, OsobniOdberVSidleSpolecnosti, OsobniOdberAdresaSidlaSpolecnosti=NULL
                FROM tShpZavozoveMisto
                WHERE ( DatumACas IS NULL OR DatumACas>GETDATE()) and ( DatumACas_Skryti IS NULL OR DatumACas_Skryti > GETDATE()) AND
                ( [Kod]=1 OR [Stat]=@Stat)
                ORDER BY OsobniOdberVSidleSpolecnosti, Mesto ASC";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@Stat", stat));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetError(dr));
            }
            return list;
        }
        public List<ZavozoveMisto> LoadByMesto(string mesto) {
            List<ZavozoveMisto> list = new List<ZavozoveMisto>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE Mesto = @Mesto";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@Mesto", mesto));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetError(dr));
            }
            return list;
        }
        public List<ZavozoveMisto> LoadJenAktualiByKod(int kod) {
            List<ZavozoveMisto> list = new List<ZavozoveMisto>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE Kod = @Kod AND (DatumACas_Skryti IS NULL OR DatumACas_Skryti > GETDATE())";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@Kod", kod));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetError(dr));
            }
            return list;
        }
        public List<ZavozoveMisto> LoadJenAktualiByMesto(string mesto) {
            List<ZavozoveMisto> list = new List<ZavozoveMisto>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE Mesto = @Mesto AND (DatumACas_Skryti IS NULL OR DatumACas_Skryti > GETDATE())";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@Mesto", mesto));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetError(dr));
            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        public override void Create(ZavozoveMisto entity) {
            using (SqlConnection connection = Connect()) {
                Exec(connection, @"INSERT INTO tShpZavozoveMisto (Stat, Mesto, Kod, Psc, Popis, DatumACas, DatumACas_Skryti, OsobniOdberVSidleSpolecnosti, OsobniOdberPovoleneCasy, OsobniOdberAdresaSidlaSpolecnosti) VALUES 
                    (@Stat, @Mesto, CASE WHEN @OsobniOdberVSidleSpolecnosti=1 THEN 1 ELSE CHECKSUM(@Mesto) END, @Psc, @Popis, @DatumACas, @DatumACas_Skryti, @OsobniOdberVSidleSpolecnosti, @OsobniOdberPovoleneCasy, @OsobniOdberAdresaSidlaSpolecnosti)",
                        new SqlParameter("@Stat", entity.Stat),
                        new SqlParameter("@Mesto", entity.Mesto),
                        new SqlParameter("@Psc", Null(entity.Psc)),
                        new SqlParameter("@Popis", Null(entity.Popis)),
                        new SqlParameter("@DatumACas", Null(entity.DatumACas)),
                        new SqlParameter("@DatumACas_Skryti", Null(entity.DatumACas_Skryti)),
                        new SqlParameter("@OsobniOdberVSidleSpolecnosti", Null(entity.OsobniOdberVSidleSpolecnosti)),
                        new SqlParameter("@OsobniOdberPovoleneCasy", Null(entity.OsobniOdberPovoleneCasy)),
                        new SqlParameter("@OsobniOdberAdresaSidlaSpolecnosti", Null(entity.OsobniOdberAdresaSidlaSpolecnosti))
                        );
            }
        }

        public override void Update(ZavozoveMisto entity) {
            using (SqlConnection connection = Connect()) {
                Exec(connection, "UPDATE tShpZavozoveMisto SET Stat=@Stat, Mesto=@Mesto, Kod=CASE WHEN @OsobniOdberVSidleSpolecnosti=1 THEN 1 ELSE CHECKSUM(@Mesto) END, Psc=@Psc, Popis=@Popis, DatumACas=@DatumACas, DatumACas_Skryti=@DatumACas_Skryti, OsobniOdberVSidleSpolecnosti=@OsobniOdberVSidleSpolecnosti, OsobniOdberPovoleneCasy=@OsobniOdberPovoleneCasy, OsobniOdberAdresaSidlaSpolecnosti=@OsobniOdberAdresaSidlaSpolecnosti WHERE ZavozoveMistoId=@Id",
                        new SqlParameter("@Stat", entity.Stat),
                        new SqlParameter("@Id", entity.Id),
                        new SqlParameter("@Mesto", entity.Mesto),
                        new SqlParameter("@Psc", Null(entity.Psc)),
                        new SqlParameter("@Popis", Null(entity.Popis)),
                        new SqlParameter("@DatumACas", Null(entity.DatumACas)),
                        new SqlParameter("@DatumACas_Skryti", Null(entity.DatumACas_Skryti)),
                        new SqlParameter("@OsobniOdberVSidleSpolecnosti", Null(entity.OsobniOdberVSidleSpolecnosti)),
                        new SqlParameter("@OsobniOdberPovoleneCasy", Null(entity.OsobniOdberPovoleneCasy)),
                        new SqlParameter("@OsobniOdberAdresaSidlaSpolecnosti", Null(entity.OsobniOdberAdresaSidlaSpolecnosti))
                        );
            }
        }

        public override void Delete(ZavozoveMisto entity) {
            using (SqlConnection connection = Connect()) {
                Exec(connection, "DELETE FROM tShpZavozoveMisto WHERE ZavozoveMistoId=@Id", new SqlParameter("@Id", entity.Id));
            }
        }
    }
}
