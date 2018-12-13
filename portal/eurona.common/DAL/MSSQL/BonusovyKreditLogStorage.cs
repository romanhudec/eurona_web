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
    public sealed class BonusovyKreditLogStorage : MSSQLStorage<BonusovyKreditLog> {
        private const string entitySelect = @"SELECT * FROM tBonusovyKreditLog";
        public BonusovyKreditLogStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static BonusovyKreditLog GetBonusovyKreditLog(DataRow record) {
            BonusovyKreditLog entity = new BonusovyKreditLog();
            entity.Id = Convert.ToInt32(record["Id"]);
            entity.BonusovyKreditTyp = ConvertNullable.ToInt32(record["BonusovyKreditTyp"]);
            entity.AccountId = Convert.ToInt32(record["AccountId"]);
            entity.Message = Convert.ToString(record["Message"]);
            entity.Status = Convert.ToString(record["Status"]);
            entity.Timestamp = Convert.ToDateTime(record["Timestamp"]);
            return entity;
        }

        public override List<BonusovyKreditLog> Read(object criteria) {
            List<BonusovyKreditLog> list = new List<BonusovyKreditLog>();
            using (SqlConnection connection = Connect()) {
                string sql = entitySelect;
                DataTable table = Query<DataTable>(connection, sql);
                foreach (DataRow dr in table.Rows)
                    list.Add(GetBonusovyKreditLog(dr));
            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }


        public override void Create(BonusovyKreditLog entity) {
            try {
                string sql = @"INSERT INTO tBonusovyKreditLog (AccountId, BonusovyKreditTyp, Message, Status, Timestamp) 
                 VALUES (@AccountId, @BonusovyKreditTyp, @Message, @Status, GETDATE())";
                using (SqlConnection connection = Connect()) {
                    Exec(connection, sql,
                            new SqlParameter("@AccountId", Null(entity.AccountId)),
                            new SqlParameter("@BonusovyKreditTyp", Null(entity.BonusovyKreditTyp)),
                            new SqlParameter("@Message", Null(entity.Message)),
                            new SqlParameter("@Status", Null(entity.Status))
                            );
                }
            } catch (Exception ex) {
                CMS.EvenLog.WritoToEventLog(ex);
            }
        }

        public override void Update(BonusovyKreditLog entity) {
        }

        public override void Delete(BonusovyKreditLog entity) {
        }
    }
}
