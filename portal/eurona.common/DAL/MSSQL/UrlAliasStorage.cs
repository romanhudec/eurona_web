using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;
using CMS.MSSQL;

namespace Eurona.Common.DAL.MSSQL
{
    public class UrlAliasStorage : MSSQLStorage<UrlAlias>
    {
        public UrlAliasStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString)
        {
        }

        public static UrlAlias GetUrlAlias(DataRow record)
        {
            UrlAlias urlAlias = new UrlAlias();
            urlAlias.Id = Convert.ToInt32(record["UrlAliasId"]);
            urlAlias.InstanceId = Convert.ToInt32(record["InstanceId"]);
            urlAlias.Url = Convert.ToString(record["Url"]);
            urlAlias.Alias = Convert.ToString(record["Alias"]);
            urlAlias.Name = Convert.ToString(record["Name"]);
            urlAlias.Locale = Convert.ToString(record["Locale"]);
            return urlAlias;
        }

        public override List<UrlAlias> Read(object criteria)
        {
            List<UrlAlias> list = new List<UrlAlias>();
            using (SqlConnection connection = Connect())
            {
                string sql = @"
				SELECT a.UrlAliasId, a.InstanceId, a.Url, a.Alias, a.Locale, a.[Name]
				FROM vUrlAliases a";
                SqlParameter[] @params = null;
                if (criteria is UrlAlias.ReadById)
                {
                    @params = new SqlParameter[] { new SqlParameter("@UrlAliasId", (criteria as UrlAlias.ReadById).UrlAliasId) };
                    sql += " WHERE a.UrlAliasId = @UrlAliasId";
                }
                else if (criteria is UrlAlias.ReadByUrl)
                {
                    @params = new SqlParameter[] { 
					new SqlParameter("@Url", (criteria as UrlAlias.ReadByUrl).Url), 
					new SqlParameter("@Locale", Locale ),
					new SqlParameter("@InstanceId", InstanceId )};
                    sql += " WHERE a.Locale = @Locale AND ( a.InstanceId=0 OR a.InstanceId=@InstanceId ) AND a.Url = @Url";
                }
                else if (criteria is UrlAlias.ReadByAlias)
                {
                    @params = new SqlParameter[] { 
					new SqlParameter("@Alias", (criteria as UrlAlias.ReadByAlias).Alias), 
					new SqlParameter("@Locale", Locale ),
					new SqlParameter("@InstanceId", InstanceId )};
                    sql += " WHERE a.Locale = @Locale AND ( a.InstanceId=0 OR a.InstanceId=@InstanceId ) AND a.Alias = @Alias";
                }
                else if (criteria is UrlAlias.ReadByLocaleAlias)
                {
                    @params = new SqlParameter[] { 
					new SqlParameter("@Alias", (criteria as UrlAlias.ReadByLocaleAlias).Alias), 
					new SqlParameter("@Locale", Null((criteria as UrlAlias.ReadByLocaleAlias).Locale) ),
                    new SqlParameter("@IgnoreInstance", Null((criteria as UrlAlias.ReadByLocaleAlias).IgnoreInstance) ),
					new SqlParameter("@InstanceId", InstanceId )};
                    sql += " WHERE (@Locale IS NULL OR a.Locale = @Locale) AND ( @IgnoreInstance=1 OR ( a.InstanceId=0 OR a.InstanceId=@InstanceId) ) AND a.Alias = @Alias";
                }
                else
                {
                    if (criteria is UrlAlias.ReadByAliasType.Pages)
                    {
                        @params = new SqlParameter[] { 
						new SqlParameter( "@Locale", Locale ), 
						new SqlParameter("@InstanceId", InstanceId )};
                        sql += " INNER JOIN vPages x ON x.UrlAliasId = a.UrlAliasId";
                        sql += " WHERE a.Locale = @Locale AND ( a.InstanceId=0 OR a.InstanceId=@InstanceId )";
                    }
                    else if (criteria is UrlAlias.ReadByAliasType.Articles)
                    {
                        @params = new SqlParameter[] { 
						new SqlParameter( "@Locale", Locale ), 
						new SqlParameter("@InstanceId", InstanceId )};
                        sql += " INNER JOIN vArticles x ON x.UrlAliasId = a.UrlAliasId";
                        sql += " WHERE a.Locale = @Locale AND ( a.InstanceId=0 OR a.InstanceId=@InstanceId )";
                    }
                    else if (criteria is UrlAlias.ReadByAliasType.Blogs)
                    {
                        @params = new SqlParameter[] { 
						new SqlParameter( "@Locale", Locale ), 
						new SqlParameter("@InstanceId", InstanceId )};
                        sql += " INNER JOIN vBlogs x ON x.UrlAliasId = a.UrlAliasId";
                        sql += " WHERE a.Locale = @Locale AND ( a.InstanceId=0 OR a.InstanceId=@InstanceId )";
                    }
                    else if (criteria is UrlAlias.ReadByAliasType.ImageGalleries)
                    {
                        @params = new SqlParameter[] { 
						new SqlParameter( "@Locale", Locale ), 
						new SqlParameter("@InstanceId", InstanceId )};
                        sql += " INNER JOIN vImageGalleries x ON x.UrlAliasId = a.UrlAliasId";
                        sql += " WHERE a.Locale = @Locale AND ( a.InstanceId=0 OR a.InstanceId=@InstanceId )";
                    }
                    else if (criteria is UrlAlias.ReadByAliasType.News)
                    {
                        @params = new SqlParameter[] { 
						new SqlParameter( "@Locale", Locale ), 
						new SqlParameter("@InstanceId", InstanceId )};
                        sql += " INNER JOIN vNews x ON x.UrlAliasId = a.UrlAliasId";
                        sql += " WHERE a.Locale = @Locale AND ( a.InstanceId=0 OR a.InstanceId=@InstanceId )";
                    }
                    else if (criteria is UrlAlias.ReadByAliasType.Custom)
                    {
                        @params = new SqlParameter[] { new SqlParameter("@Locale", Locale), new SqlParameter("@InstanceId", InstanceId) };
                        sql += " LEFT JOIN vPages p ON p.UrlAliasId = a.UrlAliasId";
                        sql += " LEFT JOIN vArticles art ON art.UrlAliasId = a.UrlAliasId";
                        sql += " LEFT JOIN vBlogs b ON b.UrlAliasId = a.UrlAliasId";
                        sql += " LEFT JOIN vImageGalleries i ON i.UrlAliasId = a.UrlAliasId";
                        sql += " LEFT JOIN vNews n ON n.UrlAliasId = a.UrlAliasId";
                        sql += @" WHERE a.Locale = @Locale AND ( a.InstanceId=0 OR a.InstanceId=@InstanceId ) AND 
						p.UrlAliasId IS NULL AND art.UrlAliasId IS NULL AND b.UrlAliasId IS NULL AND i.UrlAliasId IS NULL AND n.UrlAliasId IS NULL ";
                    }
                    else
                    {
                        @params = new SqlParameter[] { new SqlParameter("@Locale", Locale), new SqlParameter("@InstanceId", InstanceId) };
                        sql += " WHERE Locale = @Locale AND ( a.InstanceId=0 OR a.InstanceId=@InstanceId )";
                    }
                }
                DataTable table = Query<DataTable>(connection, sql, @params);
                foreach (DataRow dr in table.Rows)
                    list.Add(GetUrlAlias(dr));
            }
            return list;
        }

        public override int Count(object criteria)
        {
            throw new NotImplementedException();
        }

        public override void Create(UrlAlias urlAlias)
        {
            using (SqlConnection connection = Connect())
            {
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;

                ExecProc(connection, "pUrlAliasCreate",
                    new SqlParameter("@InstanceId", InstanceId),
                    new SqlParameter("@Url", urlAlias.Url),
                    new SqlParameter("@Alias", urlAlias.Alias),
                    new SqlParameter("@Locale", String.IsNullOrEmpty(urlAlias.Locale) ? Locale : urlAlias.Locale),
                    new SqlParameter("@Name", urlAlias.Name),
                    result);

                urlAlias.Id = Convert.ToInt32(result.Value);

            }
        }

        public override void Update(UrlAlias urlAlias)
        {
            using (SqlConnection connection = Connect())
            {
                ExecProc(connection, "pUrlAliasModify",
                    new SqlParameter("@Url", urlAlias.Url),
                    new SqlParameter("@UrlAliasId", urlAlias.Id),
                    new SqlParameter("@Name", urlAlias.Name),
                    new SqlParameter("@Alias", urlAlias.Alias));
            }
        }

        public override void Delete(UrlAlias urlAlias)
        {
            using (SqlConnection connection = Connect())
            {
                SqlParameter urlAliasId = new SqlParameter("@UrlAliasId", urlAlias.Id);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pUrlAliasDelete", result, urlAliasId);
            }
        }

    }
}
