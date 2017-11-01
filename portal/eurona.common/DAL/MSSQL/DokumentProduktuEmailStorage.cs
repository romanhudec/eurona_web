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
    public sealed class DokumentProduktuEmailStorage : MSSQLStorage<DokumentProduktuEmail> {
        private string entitySelect = "SELECT * FROM tShpDokumentProduktuEmail";
        public DokumentProduktuEmailStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static DokumentProduktuEmail GetVlastnostiProduktu(DataRow record) {
            DokumentProduktuEmail entiry = new DokumentProduktuEmail();
            entiry.ProductId = Convert.ToInt32(record["ProductId"]);
            entiry.Email = Convert.ToString(record["Email"]);
            entiry.Info = Convert.ToString(record["Info"]);
            entiry.Timestamp = Convert.ToDateTime(record["Timestamp"]);

            return entiry;
        }

        public override List<DokumentProduktuEmail> Read(object criteria) {
            List<DokumentProduktuEmail> list = new List<DokumentProduktuEmail>();

            using (SqlConnection connection = Connect()) {
                DataTable table = null;
                if (criteria is DokumentProduktuEmail.ReadByProduct) {
                    string sql = entitySelect + " WHERE ProductId=@ProductId";
                    table = Query<DataTable>(connection, sql,
                            new SqlParameter("@ProductId", (criteria as DokumentProduktuEmail.ReadByProduct).ProductId));

                    foreach (DataRow dr in table.Rows)
                        list.Add(GetVlastnostiProduktu(dr));
                } else if (criteria is DokumentProduktuEmail.ReadByDate) {
                    string sql = entitySelect + " WHERE (Timestamp>=@DateFrom OR @DateFrom IS NULL) AND (Timestamp<=@DateTo OR @DateTo IS NULL)";
                    table = Query<DataTable>(connection, sql,
                            new SqlParameter("@DateFrom", Null((criteria as DokumentProduktuEmail.ReadByDate).DateFrom)),
                            new SqlParameter("@DateTo", Null((criteria as DokumentProduktuEmail.ReadByDate).DateTo)));

                    foreach (DataRow dr in table.Rows)
                        list.Add(GetVlastnostiProduktu(dr));
                } else {

                    table = Query<DataTable>(connection, entitySelect);
                    foreach (DataRow dr in table.Rows)
                        list.Add(GetVlastnostiProduktu(dr));
                }

            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        public override void Create(DokumentProduktuEmail entity) {
            using (SqlConnection connection = Connect()) {
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;

                Exec(connection, "INSERT INTO tShpDokumentProduktuEmail (ProductId, Email, Info, Timestamp) VALUES (@ProductId, @Email, @Info, GETDATE())",
                        new SqlParameter("@ProductId", entity.ProductId),
                        new SqlParameter("@Email", entity.Email),
                        new SqlParameter("@Info", entity.Info)
                        );
            }

        }

        public override void Update(DokumentProduktuEmail entity) {
            throw new NotImplementedException();
        }

        public override void Delete(DokumentProduktuEmail entity) {
            throw new NotImplementedException();
        }
    }
}
