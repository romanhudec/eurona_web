using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using CMS.MSSQL;
using SHP.Entities;

namespace SHP.MSSQL {
    [Serializable]
    public sealed class ProductHighlightsStorage : MSSQLStorage<ProductHighlights> {
        public ProductHighlightsStorage(int instanceId, CMS.Entities.Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static ProductHighlights GetProductHighlights(DataRow record) {
            ProductHighlights highlight = new ProductHighlights();
            highlight.Id = Convert.ToInt32(record["ProductHighlightsId"]);
            highlight.InstanceId = Convert.ToInt32(record["InstanceId"]);
            highlight.ProductId = Convert.ToInt32(record["ProductId"]);
            highlight.HighlightId = Convert.ToInt32(record["HighlightId"]);
            highlight.Name = Convert.ToString(record["Name"]);
            highlight.Code = Convert.ToString(record["Code"]);
            highlight.Icon = Convert.ToString(record["Icon"]);
            highlight.Notes = Convert.ToString(record["Notes"]);

            return highlight;
        }


        public override List<ProductHighlights> Read(object criteria) {
            if (criteria is ProductHighlights.ReadById) return LoadById(criteria as ProductHighlights.ReadById);
            if (criteria is ProductHighlights.ReadByProduct) return LoadByProduct(criteria as ProductHighlights.ReadByProduct);
            List<ProductHighlights> list = new List<ProductHighlights>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT ProductHighlightsId, ProductId, HighlightId, InstanceId, Icon, Name, Code, Notes
								FROM vShpProductHighlights
								WHERE InstanceId=@InstanceId
								ORDER BY Name ASC";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetProductHighlights(dr));
            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        private List<ProductHighlights> LoadById(ProductHighlights.ReadById by) {
            List<ProductHighlights> list = new List<ProductHighlights>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT ProductHighlightsId, ProductId, HighlightId, InstanceId, Icon, Name, Code, Notes
								FROM vShpProductHighlights 
								WHERE ProductHighlightsId = @ProductHighlightsId
								ORDER BY Name ASC";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@ProductHighlightsId", Null(by.ProductHighlightsId)));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetProductHighlights(dr));
            }
            return list;
        }

        private List<ProductHighlights> LoadByProduct(ProductHighlights.ReadByProduct by) {
            List<ProductHighlights> list = new List<ProductHighlights>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT ProductHighlightsId, ProductId, HighlightId, InstanceId, Icon, Name, Code, Notes
								FROM vShpProductHighlights 
								WHERE InstanceId=@InstanceId AND ProductId = ISNULL(@ProductId, ProductId ) AND HighlightId = ISNULL(@HighlightId, HighlightId )
								ORDER BY Name ASC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@ProductId", Null(by.ProductId)),
                        new SqlParameter("@HighlightId", Null(by.HighlightId)),
                        new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetProductHighlights(dr));
            }
            return list;
        }

        public override void Create(ProductHighlights highlight) {
            using (SqlConnection connection = Connect()) {
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;

                ExecProc(connection, "pShpProductHighlightsCreate",
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@ProductId", highlight.ProductId),
                        new SqlParameter("@HighlightId", highlight.HighlightId),
                        result);

                highlight.Id = Convert.ToInt32(result.Value); ;
            }
        }

        public override void Update(ProductHighlights entity) {
            throw new NotImplementedException();
        }

        public override void Delete(ProductHighlights highlight) {
            using (SqlConnection connection = Connect()) {
                SqlParameter id = new SqlParameter("@ProductHighlightsId", highlight.Id);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pShpProductHighlightsDelete", result, id);
            }
        }

    }
}
