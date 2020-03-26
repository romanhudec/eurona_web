using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CMS.Entities;
using CMS.MSSQL;
using SHP.Entities.Classifiers;

namespace SHP.MSSQL.Classifiers {
    [Serializable]
    public sealed class ShipmentStorage : MSSQLStorage<Shipment> {
        public ShipmentStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static Shipment GetShipment(DataRow record) {
            Shipment shipment = new Shipment();
            shipment.Id = Convert.ToInt32(record["ShipmentId"]);
            shipment.InstanceId = Convert.ToInt32(record["InstanceId"]);
            shipment.Name = Convert.ToString(record["Name"]);
            shipment.Code = Convert.ToString(record["Code"]);
            shipment.Icon = Convert.ToString(record["Icon"]);
            shipment.Locale = Convert.ToString(record["Locale"]);
            shipment.Notes = Convert.ToString(record["Notes"]);
            shipment.Price = ConvertNullable.ToDecimal(record["Price"]);
            shipment.VATId = ConvertNullable.ToInt32(record["VATId"]);
            shipment.PriceWVAT = ConvertNullable.ToDecimal(record["PriceWVAT"]);
            shipment.VAT = ConvertNullable.ToDecimal(record["VAT"]);
            shipment.Order = ConvertNullable.ToInt32(record["Order"]);
            shipment.Hide = record["Hide"] == DBNull.Value ? false : Convert.ToBoolean(record["Hide"]);
            shipment.PlatbaDobirkou = Convert.ToBoolean(record["PlatbaDobirkou"]);
            shipment.PlatbaKartou = Convert.ToBoolean(record["PlatbaKartou"]);

            return shipment;
        }

        public override List<Shipment> Read(object criteria) {
            if (criteria is Shipment.ReadById) return LoadById(criteria as Shipment.ReadById);
            if (criteria is Shipment.ReadByCode) return LoadByCode(criteria as Shipment.ReadByCode);
            if (criteria is Shipment.ReadDefault) return LoadDefault();
            if (criteria is Shipment.Read4AllLocales) return Load4AllLocales();
            List<Shipment> list = new List<Shipment>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT ShipmentId, InstanceId, [Name], [Code], [Icon], [Locale], [Notes], [Price], [VATId], [PriceWVAT], [VAT], [Order], [Hide], [PlatbaDobirkou], [PlatbaKartou]
								FROM vShpShipments
								WHERE Locale = @Locale AND InstanceId=@InstanceId
								ORDER BY [Order] ASC, [Code] DESC";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@Locale", Locale), new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetShipment(dr));
            }
            return list;
        }

        public List<Shipment> Load4AllLocales() {
            List<Shipment> list = new List<Shipment>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT ShipmentId, InstanceId, [Name], [Code], [Icon], [Locale], [Notes], [Price], [VATId], [PriceWVAT], [VAT], [Order], [Hide], [PlatbaDobirkou], [PlatbaKartou]
								FROM vShpShipments
								WHERE InstanceId=@InstanceId
								ORDER BY  [Order] ASC, [Default] DESC";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetShipment(dr));
            }
            return list;
        }


        public List<Shipment> LoadDefault() {
            List<Shipment> list = new List<Shipment>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT ShipmentId, InstanceId, [Name], [Code], [Icon], [Locale], [Notes], [Price], [VATId], [PriceWVAT], [VAT], [Order], [Hide], [PlatbaDobirkou], [PlatbaKartou]
								FROM vShpShipments
								WHERE Locale = @Locale AND InstanceId=@InstanceId AND [Default]=1
								ORDER BY  [Order] ASC, [Default] DESC";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@Locale", Locale), new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetShipment(dr));
            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        private List<Shipment> LoadById(Shipment.ReadById by) {
            List<Shipment> list = new List<Shipment>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT ShipmentId, InstanceId, [Name], [Code], [Icon], [Locale], [Notes], [Price], [VATId], [PriceWVAT], [VAT], [Order], [Hide], [PlatbaDobirkou], [PlatbaKartou]
								FROM vShpShipments
								WHERE ShipmentId = @ShipmentId
								ORDER BY [Order] ASC, [Code] DESC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@ShipmentId", by.Id));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetShipment(dr));
            }
            return list;
        }


        private List<Shipment> LoadByCode(Shipment.ReadByCode by) {
            List<Shipment> list = new List<Shipment>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT ShipmentId, InstanceId, [Name], [Code], [Icon], [Locale], [Notes], [Price], [VATId], [PriceWVAT], [VAT], [Order], [Hide], [PlatbaDobirkou], [PlatbaKartou]
								FROM vShpShipments
								WHERE Code = @Code AND Locale=@Locale AND InstanceId=@InstanceId
								ORDER BY [Order] ASC, [Code] DESC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@Code", by.Code), new SqlParameter("@Locale", Locale), new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetShipment(dr));
            }
            return list;
        }

        public override void Create(Shipment shipment) {
        }

        public override void Update(Shipment shipment) {
            using (SqlConnection connection = Connect()) {
                Exec(connection, "UPDATE cShpShipment SET [Order]=@Order, [Hide]=@Hide, [PlatbaKartou]=@PlatbaKartou, [PlatbaDobirkou]=@PlatbaDobirkou WHERE ShipmentId=@Id", 
                    new SqlParameter("@Id", shipment.Id),
                    new SqlParameter("@Order", Null(shipment.Order)),
                    new SqlParameter("@Hide", Null(shipment.Hide)),
                    new SqlParameter("@PlatbaKartou", Null(shipment.PlatbaKartou)),
                    new SqlParameter("@PlatbaDobirkou", Null(shipment.PlatbaDobirkou)));
            }
        }

        public override void Delete(Shipment shipment) {
            using (SqlConnection connection = Connect()) {
                SqlParameter historyAccount = new SqlParameter("@HistoryAccount", AccountId);
                SqlParameter addressId = new SqlParameter("@ShipmentId", shipment.Id);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pShpShipmentDelete", result, historyAccount, addressId);
            }
        }
    }
}
