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
    public sealed class BonusovyKreditStorage : MSSQLStorage<BonusovyKredit> {
        private const string entitySelect = @"SELECT BonusovyKreditId, InstanceId, Typ, HodnotaOd, HodnotaDo, HodnotaOdSK, HodnotaDoSK, HodnotaOdPL, HodnotaDoPL, Kredit, Aktivni
						FROM vBonusoveKredity bk";
        public BonusovyKreditStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static BonusovyKredit GetBonusovyKredit(DataRow record) {
            BonusovyKredit entity = new BonusovyKredit();
            entity.Id = Convert.ToInt32(record["BonusovyKreditId"]);
            entity.InstanceId = Convert.ToInt32(record["InstanceId"]);
            entity.Typ = Convert.ToInt32(record["Typ"]);
            entity.HodnotaOd = ConvertNullable.ToDecimal(record["HodnotaOd"]);
            entity.HodnotaDo = ConvertNullable.ToDecimal(record["HodnotaDo"]);
            entity.HodnotaOdSK = ConvertNullable.ToDecimal(record["HodnotaOdSK"]);
            entity.HodnotaDoSK = ConvertNullable.ToDecimal(record["HodnotaDoSK"]);
            entity.HodnotaOdPL = ConvertNullable.ToDecimal(record["HodnotaOdPL"]);
            entity.HodnotaDoPL = ConvertNullable.ToDecimal(record["HodnotaDoPL"]);
            entity.Kredit = Convert.ToDecimal(record["Kredit"]);
            entity.Aktivni = Convert.ToBoolean(record["Aktivni"]);

            return entity;
        }

        public override List<BonusovyKredit> Read(object criteria) {
            if (criteria is BonusovyKredit.ReadById) return LoadById(criteria as BonusovyKredit.ReadById);
            if (criteria is BonusovyKredit.ReadByTyp) return LoadByTyp(criteria as BonusovyKredit.ReadByTyp);
            List<BonusovyKredit> list = new List<BonusovyKredit>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE InstanceId = @InstanceId";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetBonusovyKredit(dr));
            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        private List<BonusovyKredit> LoadById(BonusovyKredit.ReadById byBonusovyKreditId) {
            List<BonusovyKredit> list = new List<BonusovyKredit>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE BonusovyKreditId = @BonusovyKreditId";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@BonusovyKreditId", byBonusovyKreditId.Id));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetBonusovyKredit(dr));
            }
            return list;
        }

        private List<BonusovyKredit> LoadByTyp(BonusovyKredit.ReadByTyp by) {
            List<BonusovyKredit> list = new List<BonusovyKredit>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                sql += " WHERE Typ = @Typ AND InstanceId = @InstanceId";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@Typ", by.Typ),
                        new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetBonusovyKredit(dr));
            }
            return list;
        }

        public override void Create(BonusovyKredit entity) {
            using (SqlConnection connection = Connect()) {
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;

                ExecProc(connection, "pBonusovyKreditCreate",
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@Typ", entity.Typ),
                        new SqlParameter("@HodnotaOd", Null(entity.HodnotaOd)),
                        new SqlParameter("@HodnotaDo", Null(entity.HodnotaDo)),
                        new SqlParameter("@HodnotaOdSK", Null(entity.HodnotaOdSK)),
                        new SqlParameter("@HodnotaDoSK", Null(entity.HodnotaDoSK)),
                        new SqlParameter("@HodnotaOdPL", Null(entity.HodnotaOdPL)),
                        new SqlParameter("@HodnotaDoPL", Null(entity.HodnotaDoPL)),
                        new SqlParameter("@Kredit", entity.Kredit),
                        new SqlParameter("@Aktivni", entity.Aktivni),
                        result);

                entity.Id = Convert.ToInt32(result.Value);
            }

        }

        public override void Update(BonusovyKredit entity) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pBonusovyKreditModify",
                        new SqlParameter("@BonusovyKreditId", entity.Id),
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@Typ", entity.Typ),
                        new SqlParameter("@HodnotaOd", Null(entity.HodnotaOd)),
                        new SqlParameter("@HodnotaDo", Null(entity.HodnotaDo)),
                        new SqlParameter("@HodnotaOdSK", Null(entity.HodnotaOdSK)),
                        new SqlParameter("@HodnotaDoSK", Null(entity.HodnotaDoSK)),
                        new SqlParameter("@HodnotaOdPL", Null(entity.HodnotaOdPL)),
                        new SqlParameter("@HodnotaDoPL", Null(entity.HodnotaDoPL)),
                        new SqlParameter("@Kredit", entity.Kredit),
                        new SqlParameter("@Aktivni", entity.Aktivni)
                        );
            }
        }

        public override void Delete(BonusovyKredit entity) {
            using (SqlConnection connection = Connect()) {
                SqlParameter id = new SqlParameter("@BonusovyKreditId", entity.Id);
                SqlParameter instance = new SqlParameter("@InstanceId", InstanceId);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pBonusovyKreditDelete", result, instance, id);
            }
        }
    }
}
