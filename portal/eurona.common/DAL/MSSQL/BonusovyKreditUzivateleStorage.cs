using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eurona.Common.DAL.Entities;
using System.Data.SqlClient;
using System.Data;
using CMS.MSSQL;
using CMS.Entities;

namespace Eurona.Common.DAL.MSSQL {
    [Serializable]
    public sealed class BonusovyKreditUzivateleStorage : MSSQLStorage<BonusovyKreditUzivatele> {
        private const string entitySelect = @"SELECT bk.BonusovyKreditUzivateleId, bk.AccountId, bk.BonusovyKreditId, bk.Datum, bk.PlatnostOd, bk.PlatnostDo, bk.Kod, bk.Hodnota, bk.Poznamka,
						bk.TVD_Id, bk.Typ, bk.InstanceId
						FROM vBonusoveKredityUzivatele bk";
        public BonusovyKreditUzivateleStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static BonusovyKreditUzivatele GetBonusovyKreditUzivatele(DataRow record) {
            BonusovyKreditUzivatele entity = new BonusovyKreditUzivatele();
            entity.Id = Convert.ToInt32(record["BonusovyKreditUzivateleId"]);
            entity.AccountId = Convert.ToInt32(record["AccountId"]);
            entity.TVD_Id = ConvertNullable.ToInt32(record["TVD_Id"]);
            entity.Typ = Convert.ToInt32(record["Typ"]);
            entity.BonusovyKreditId = Convert.ToInt32(record["BonusovyKreditId"]);
            entity.Datum = Convert.ToDateTime(record["Datum"]);
            entity.PlatnostOd = Convert.ToDateTime(record["PlatnostOd"]);
            entity.PlatnostDo = Convert.ToDateTime(record["PlatnostDo"]);
            entity.Kod = Convert.ToString(record["Kod"]);
            entity.Hodnota = Convert.ToDecimal(record["Hodnota"]);
            entity.Poznamka = Convert.ToString(record["Poznamka"]);

            return entity;
        }

        public override List<BonusovyKreditUzivatele> Read(object criteria) {
            if (criteria is BonusovyKreditUzivatele.ReadById) return LoadById(criteria as BonusovyKreditUzivatele.ReadById);
            if (criteria is BonusovyKreditUzivatele.ReadByAccount) return LoadByAccount(criteria as BonusovyKreditUzivatele.ReadByAccount);
            if (criteria is BonusovyKreditUzivatele.ReadByBonusovyKredit) return LoadByBonusovyKredit(criteria as BonusovyKreditUzivatele.ReadByBonusovyKredit);
            if (criteria is BonusovyKreditUzivatele.ReadByBonusovyKreditAndAccount) return LoadByBonusovyKreditAndAccount(criteria as BonusovyKreditUzivatele.ReadByBonusovyKreditAndAccount);
            if (criteria is BonusovyKreditUzivatele.ReadLast) return LoadLast(criteria as BonusovyKreditUzivatele.ReadLast);
            List<BonusovyKreditUzivatele> list = new List<BonusovyKreditUzivatele>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                DataTable table = Query<DataTable>(connection, sql);
                foreach (DataRow dr in table.Rows)
                    list.Add(GetBonusovyKreditUzivatele(dr));
            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        private List<BonusovyKreditUzivatele> LoadById(BonusovyKreditUzivatele.ReadById byBonusovyKreditUzivateleId) {
            List<BonusovyKreditUzivatele> list = new List<BonusovyKreditUzivatele>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE BonusovyKreditUzivateleId = @BonusovyKreditUzivateleId";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@BonusovyKreditUzivateleId", byBonusovyKreditUzivateleId.Id));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetBonusovyKreditUzivatele(dr));
            }
            return list;
        }

        private List<BonusovyKreditUzivatele> LoadByAccount(BonusovyKreditUzivatele.ReadByAccount by) {
            List<BonusovyKreditUzivatele> list = new List<BonusovyKreditUzivatele>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE AccountId = @AccountId AND InstanceId=@InstanceId";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@AccountId", by.AccountId), new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetBonusovyKreditUzivatele(dr));
            }
            return list;
        }

        private List<BonusovyKreditUzivatele> LoadByBonusovyKredit(BonusovyKreditUzivatele.ReadByBonusovyKredit by) {
            List<BonusovyKreditUzivatele> list = new List<BonusovyKreditUzivatele>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE BonusovyKreditId = @BonusovyKreditId AND InstanceId=@InstanceId";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@BonusovyKreditId", by.BonusovyKreditId), new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetBonusovyKreditUzivatele(dr));
            }
            return list;
        }

        private List<BonusovyKreditUzivatele> LoadByBonusovyKreditAndAccount(BonusovyKreditUzivatele.ReadByBonusovyKreditAndAccount by) {
            List<BonusovyKreditUzivatele> list = new List<BonusovyKreditUzivatele>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE BonusovyKreditId = @BonusovyKreditId AND AccountId = @AccountId AND InstanceId=@InstanceId";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@BonusovyKreditId", by.BonusovyKreditId), new SqlParameter("@AccountId", by.AccountId), new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetBonusovyKreditUzivatele(dr));
            }
            return list;
        }

        private List<BonusovyKreditUzivatele> LoadLast(BonusovyKreditUzivatele.ReadLast by) {
            List<BonusovyKreditUzivatele> list = new List<BonusovyKreditUzivatele>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE AccountId = @AccountId AND Typ=@Typ AND InstanceId=@InstanceId AND (@Kod IS NULL OR Kod=@Kod) ORDER BY Datum DESC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@AccountId", by.AccountId), new SqlParameter("@Typ", by.Typ), new SqlParameter("@Kod", Null(by.Kod)), new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetBonusovyKreditUzivatele(dr));
            }
            return list;
        }

        public override void Create(BonusovyKreditUzivatele entity) {
            using (SqlConnection connection = Connect()) {
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;

                ExecProc(connection, "pBonusovyKreditUzivateleCreate",
                        new SqlParameter("@AccountId", entity.AccountId),
                        new SqlParameter("@BonusovyKreditId", entity.BonusovyKreditId),
                        new SqlParameter("@Datum", entity.Datum),
                        new SqlParameter("@PlatnostOd", entity.PlatnostOd),
                        new SqlParameter("@PlatnostDo", entity.PlatnostDo),
                        new SqlParameter("@Kod", entity.Kod),
                        new SqlParameter("@Hodnota", entity.Hodnota),
                        new SqlParameter("@Poznamka", entity.Poznamka),
                        result);

                entity.Id = Convert.ToInt32(result.Value);
            }

        }

        public override void Update(BonusovyKreditUzivatele entity) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pBonusovyKreditUzivateleModify",
                        new SqlParameter("@BonusovyKreditUzivateleId", entity.Id),
                        new SqlParameter("@AccountId", entity.AccountId),
                        new SqlParameter("@BonusovyKreditId", entity.BonusovyKreditId),
                        new SqlParameter("@Datum", entity.Datum),
                        new SqlParameter("@PlatnostOd", entity.PlatnostOd),
                        new SqlParameter("@PlatnostDo", entity.PlatnostDo),
                        new SqlParameter("@Kod", entity.Kod),
                        new SqlParameter("@Hodnota", entity.Hodnota),
                        new SqlParameter("@Poznamka", entity.Poznamka)
                        );
            }
        }

        public override void Delete(BonusovyKreditUzivatele entity) {
            using (SqlConnection connection = Connect()) {
                SqlParameter id = new SqlParameter("@BonusovyKreditUzivateleId", entity.Id);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pBonusovyKreditUzivateleDelete", result, id);
            }
        }
    }
}
