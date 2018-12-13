using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eurona.DAL.Entities;
using System.Data.SqlClient;
using System.Data;
using CMS.MSSQL;

namespace Eurona.DAL.MSSQL {
    [Serializable]
    internal sealed class MimoradnaNabidkaStorage : MSSQLStorage<MimoradnaNabidka> {
        public MimoradnaNabidkaStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static MimoradnaNabidka GetMimoradnaNabidka(DataRow record) {
            MimoradnaNabidka entity = new MimoradnaNabidka();
            entity.Id = Convert.ToInt32(record["MimoradnaNabidkaId"]);
            entity.InstanceId = Convert.ToInt32(record["InstanceId"]);
            entity.Locale = Convert.ToString(record["Locale"]);
            entity.Date = ConvertNullable.ToDateTime(record["Date"]);
            entity.Icon = Convert.ToString(record["Icon"]);
            entity.Title = Convert.ToString(record["Title"]);
            entity.Teaser = Convert.ToString(record["Teaser"]);
            entity.Content = Convert.ToString(record["Content"]);
            entity.UrlAliasId = ConvertNullable.ToInt32(record["UrlAliasId"]);
            entity.Alias = Convert.ToString(record["Alias"]);
            return entity;
        }

        public override List<MimoradnaNabidka> Read(object criteria) {
            if (criteria is MimoradnaNabidka.ReadById) return LoadById(criteria as MimoradnaNabidka.ReadById);
            List<MimoradnaNabidka> list = new List<MimoradnaNabidka>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
				SELECT n.[MimoradnaNabidkaId], n.InstanceId, n.[Locale], n.[Date], n.[Icon], n.[Title], n.[Teaser], n.[Content],
					n.UrlAliasId, alias.Alias, alias.Url
				FROM tMimoradnaNabidka n LEFT JOIN tUrlAlias alias ON alias.UrlAliasId = n.UrlAliasId
				WHERE n.Locale = @Locale AND n.InstanceId=@InstanceId
				ORDER BY [Date] DESC, MimoradnaNabidkaId DESC";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@Locale", Locale),
                        new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetMimoradnaNabidka(dr));
            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        private List<MimoradnaNabidka> LoadById(MimoradnaNabidka.ReadById byMimoradnaNabidkaId) {
            List<MimoradnaNabidka> list = new List<MimoradnaNabidka>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
				SELECT n.[MimoradnaNabidkaId], n.InstanceId, n.[Locale], n.[Date], n.[Icon], n.[Title], n.[Teaser], n.[Content],
					n.UrlAliasId, alias.Alias, alias.Url
				FROM tMimoradnaNabidka n LEFT JOIN tUrlAlias alias ON alias.UrlAliasId = n.UrlAliasId
				WHERE MimoradnaNabidkaId = @MimoradnaNabidkaId
				ORDER BY [Date] DESC, MimoradnaNabidkaId DESC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@MimoradnaNabidkaId", byMimoradnaNabidkaId.MimoradnaNabidkaId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetMimoradnaNabidka(dr));
            }
            return list;
        }

        public override void Create(MimoradnaNabidka entity) {
            using (SqlConnection connection = Connect()) {
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;

                string sql = @"
				INSERT INTO tMimoradnaNabidka ( InstanceId, Locale, [Date], Icon, Title, Teaser, Content, UrlAliasId) 
				VALUES ( @InstanceId, @Locale, @Date, @Icon, @Title, @Teaser, @Content, @UrlAliasId)
				
				SET @Result = SCOPE_IDENTITY()
				SELECT MimoradnaNabidkaId = @Result";

                Exec(connection, sql,
                new SqlParameter("@InstanceId", InstanceId),
                new SqlParameter("@Date", Null(entity.Date)),
                new SqlParameter("@Locale", entity.Locale),
                new SqlParameter("@Icon", entity.Icon),
                new SqlParameter("@Title", entity.Title),
                new SqlParameter("@Teaser", entity.Teaser),
                new SqlParameter("@Content", entity.Content),
                new SqlParameter("@UrlAliasId", Null(entity.UrlAliasId)),
                result);

                entity.Id = Convert.ToInt32(result.Value);
            }
        }

        public override void Update(MimoradnaNabidka entity) {
            using (SqlConnection connection = Connect()) {
                string sql = @"UPDATE tMimoradnaNabidka	
					SET	[Date] = @Date, Icon = @Icon, Title = @Title, Teaser = @Teaser, Content = @Content, UrlAliasId=@UrlAliasId
					WHERE MimoradnaNabidkaId = @MimoradnaNabidkaId";
                Exec(connection, sql,
                        new SqlParameter("@MimoradnaNabidkaId", entity.Id),
                        new SqlParameter("@Date", Null(entity.Date)),
                        new SqlParameter("@Icon", entity.Icon),
                        new SqlParameter("@Title", entity.Title),
                        new SqlParameter("@Teaser", entity.Teaser),
                        new SqlParameter("@Content", entity.Content),
                        new SqlParameter("@UrlAliasId", entity.UrlAliasId)
                        );
            }
        }

        public override void Delete(MimoradnaNabidka entity) {
            using (SqlConnection connection = Connect()) {
                SqlParameter id = new SqlParameter("@MimoradnaNabidkaId", entity.Id);
                SqlParameter urlAliasId = new SqlParameter("@UrlAliasId", entity.UrlAliasId);

                string sql = @"
				DELETE FROM tMimoradnaNabidka WHERE MimoradnaNabidkaId = @MimoradnaNabidkaId
				DELETE FROM tUrlAlias WHERE UrlAliasId = @UrlAliasId
				";

                Exec(connection, sql, id, urlAliasId);
            }
        }

    }
}
