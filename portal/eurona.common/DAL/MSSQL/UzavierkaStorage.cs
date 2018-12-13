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
    public sealed class UzavierkaStorage : MSSQLStorage<Uzavierka> {
        private string entitySelect = "SELECT UzavierkaId, Povolena, UzavierkaOd, UzavierkaDo, OperatorOrderOd, OperatorOrderDo, OperatorOrderDate FROM vShpUzavierka";
        public UzavierkaStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static Uzavierka GetUzavierka(DataRow record) {
            Uzavierka entiry = new Uzavierka();
            entiry.Id = Convert.ToInt32(record["UzavierkaId"]);
            entiry.Povolena = Convert.ToBoolean(record["Povolena"]);

            entiry.UzavierkaOd = ConvertNullable.ToDateTime(record["UzavierkaOd"]);
            entiry.UzavierkaDo = ConvertNullable.ToDateTime(record["UzavierkaDo"]);
            entiry.OperatorOrderOd = ConvertNullable.ToDateTime(record["OperatorOrderOd"]);
            entiry.OperatorOrderDo = ConvertNullable.ToDateTime(record["OperatorOrderDo"]);
            entiry.OperatorOrderDate = ConvertNullable.ToDateTime(record["OperatorOrderDate"]);

            return entiry;
        }

        public override List<Uzavierka> Read(object criteria) {
            List<Uzavierka> list = new List<Uzavierka>();

            using (SqlConnection connection = Connect()) {
                DataTable table = null;
                if (criteria is Uzavierka.ReadById) {
                    string sql = entitySelect + " WHERE UzavierkaId=@UzavierkaId";
                    table = Query<DataTable>(connection, sql,
                            new SqlParameter("@UzavierkaId", (criteria as Uzavierka.ReadById).UzavierkaId));

                    foreach (DataRow dr in table.Rows)
                        list.Add(GetUzavierka(dr));
                } else {

                    table = Query<DataTable>(connection, entitySelect);
                    foreach (DataRow dr in table.Rows)
                        list.Add(GetUzavierka(dr));
                }

            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        public override void Create(Uzavierka entity) {
            string sql = @"INSERT INTO tShpUzavierka (UzavierkaId, Povolena, UzavierkaOd, UzavierkaDo, OperatorOrderOd, OperatorOrderDo, OperatorOrderDate) 
            VALUES
            (@UzavierkaId, @Povolena, @UzavierkaOd, @UzavierkaDo, @OperatorOrderOd, @OperatorOrderDo, @OperatorOrderDate)";
            using (SqlConnection connection = Connect()) {
                Exec(connection, sql,
                        new SqlParameter("@UzavierkaId", entity.Id),
                        new SqlParameter("@Povolena", Null(entity.Povolena)),
                        new SqlParameter("@UzavierkaOd", Null(entity.UzavierkaOd)),
                        new SqlParameter("@UzavierkaDo", Null(entity.UzavierkaDo)),
                        new SqlParameter("@OperatorOrderOd", Null(entity.OperatorOrderOd)),
                        new SqlParameter("@OperatorOrderDo", Null(entity.OperatorOrderDo)),
                        new SqlParameter("@OperatorOrderDate", Null(entity.OperatorOrderDate))
                        );
            }
        }

        public override void Update(Uzavierka entity) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pShpUzavierkaModify",
                        new SqlParameter("@UzavierkaId", entity.Id),
                        new SqlParameter("@Povolena", Null(entity.Povolena)),
                        new SqlParameter("@UzavierkaOd", Null(entity.UzavierkaOd)),
                        new SqlParameter("@UzavierkaDo", Null(entity.UzavierkaDo)),
                        new SqlParameter("@OperatorOrderOd", Null(entity.OperatorOrderOd)),
                        new SqlParameter("@OperatorOrderDo", Null(entity.OperatorOrderDo)),
                        new SqlParameter("@OperatorOrderDate", Null(entity.OperatorOrderDate))
                        );
            }
        }

        public override void Delete(Uzavierka entity) {
            throw new NotImplementedException();
        }
    }
}
