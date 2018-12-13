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
    public sealed class CenyProduktuStorage : MSSQLStorage<CenyProduktu> {
        private string entitySelect = "SELECT ProductId, CurrencyId, CurrencySymbol, CurrencyCode, Locale, Cena, BeznaCena, DynamickaSleva, StatickaSleva, CenaBK FROM vShpCenyProduktu WHERE Locale=@Locale ";
        public CenyProduktuStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static CenyProduktu GetCenyProduktu(DataRow record) {
            CenyProduktu entiry = new CenyProduktu();
            entiry.Id = Convert.ToInt32(record["ProductId"]);
            entiry.CurrencyId = ConvertNullable.ToInt32(record["CurrencyId"]);
            entiry.CurrencySymbol = Convert.ToString(record["CurrencySymbol"]);
            entiry.CurrencyCode = Convert.ToString(record["CurrencyCode"]);
            entiry.Locale = Convert.ToString(record["Locale"]);
            entiry.Cena = Convert.ToDecimal(record["Cena"]);
            entiry.BeznaCena = Convert.ToDecimal(record["BeznaCena"]);
            entiry.DynamickaSleva = ConvertNullable.ToBool(record["DynamickaSleva"]);
            entiry.StatickaSleva = ConvertNullable.ToDecimal(record["StatickaSleva"]);
            entiry.CenaBK = ConvertNullable.ToDecimal(record["CenaBK"]);

            return entiry;
        }

        public override List<CenyProduktu> Read(object criteria) {
            List<CenyProduktu> list = new List<CenyProduktu>();

            using (SqlConnection connection = Connect()) {
                DataTable table = null;
                if (criteria is CenyProduktu.ReadByProduct) {
                    string sql = entitySelect + " AND ProductId=@ProductId";
                    table = Query<DataTable>(connection, sql,
                            new SqlParameter("@Locale", Locale),
                            new SqlParameter("@ProductId", (criteria as CenyProduktu.ReadByProduct).ProductId));

                    foreach (DataRow dr in table.Rows)
                        list.Add(GetCenyProduktu(dr));
                } else {

                    table = Query<DataTable>(connection, entitySelect, new SqlParameter("@Locale", Locale));
                    foreach (DataRow dr in table.Rows)
                        list.Add(GetCenyProduktu(dr));
                }

            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        public override void Create(CenyProduktu entity) {
            throw new NotImplementedException();
        }

        public override void Update(CenyProduktu entity) {
            throw new NotImplementedException();
        }

        public override void Delete(CenyProduktu entity) {
            throw new NotImplementedException();
        }
    }
}
