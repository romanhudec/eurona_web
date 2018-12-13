using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL {
    [Serializable]
    internal sealed class NewsStorage : MSSQLStorage<News> {
        public NewsStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static News GetNews(DataRow record) {
            News news = new News();
            news.Id = Convert.ToInt32(record["NewsId"]);
            news.InstanceId = Convert.ToInt32(record["InstanceId"]);
            news.Locale = Convert.ToString(record["Locale"]);
            news.Date = ConvertNullable.ToDateTime(record["Date"]);
            news.Icon = Convert.ToString(record["Icon"]);
            news.Title = Convert.ToString(record["Title"]);
            news.Teaser = Convert.ToString(record["Teaser"]);
            news.Content = Convert.ToString(record["Content"]);
            news.UrlAliasId = ConvertNullable.ToInt32(record["UrlAliasId"]);
            news.Alias = Convert.ToString(record["Alias"]);
            return news;
        }

        public override List<News> Read(object criteria) {
            if (criteria is News.ReadById) return LoadById(criteria as News.ReadById);
            if (criteria is News.ReadLatest) return LoadLatest(criteria as News.ReadLatest);
            List<News> list = new List<News>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT	NewsId, InstanceId, Locale, [Date], Icon, Title, Teaser, Content, [UrlAliasId], Alias
								FROM vNews
								WHERE Locale = @Locale AND InstanceId=@InstanceId
								ORDER BY [Date] DESC, NewsId DESC";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@Locale", Locale),
                        new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetNews(dr));
            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        private List<News> LoadLatest(News.ReadLatest top) {
            List<News> list = new List<News>();
            using (SqlConnection connection = Connect()) {
                string sql = string.Format(@"
								SELECT TOP {0}	NewsId, InstanceId, Locale, [Date], Icon, Title, Teaser, Content, [UrlAliasId], Alias
								FROM vNews
								WHERE Locale = @Locale AND InstanceId=@InstanceId
								ORDER BY [Date] DESC, NewsId DESC", top.Count);
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@Locale", Locale),
                        new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetNews(dr));
            }
            return list;
        }

        private List<News> LoadById(News.ReadById byNewsId) {
            List<News> list = new List<News>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT	NewsId, InstanceId, Locale, [Date], Icon, Title, Teaser, Content, [UrlAliasId], Alias
								FROM vNews
								WHERE NewsId = @NewsId
								ORDER BY [Date] DESC, NewsId DESC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@NewsId", byNewsId.NewsId));
                foreach (DataRow dr in table.Rows)
                    list.Add(GetNews(dr));
            }
            return list;
        }

        public override void Create(News news) {
            using (SqlConnection connection = Connect()) {
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;

                ExecProc(connection, "pNewsCreate",
                        new SqlParameter("@HistoryAccount", AccountId),
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@Date", Null(news.Date)),
                        new SqlParameter("@Locale", news.Locale),
                        new SqlParameter("@Icon", news.Icon),
                        new SqlParameter("@Title", news.Title),
                        new SqlParameter("@Teaser", news.Teaser),
                        new SqlParameter("@Content", news.Content),
                        new SqlParameter("@ContentKeywords", Null(news.ContentKeywords, String.Empty)),
                        new SqlParameter("@UrlAliasId", news.UrlAliasId),
                        result);

                news.Id = Convert.ToInt32(result.Value);
            }
        }

        public override void Update(News news) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pNewsModify",
                        new SqlParameter("@HistoryAccount", AccountId),
                        new SqlParameter("@NewsId", news.Id),
                        new SqlParameter("@Date", Null(news.Date)),
                        new SqlParameter("@Icon", news.Icon),
                        new SqlParameter("@Title", news.Title),
                        new SqlParameter("@Teaser", news.Teaser),
                        new SqlParameter("@Content", news.Content),
                        new SqlParameter("@ContentKeywords", Null(news.ContentKeywords, String.Empty)),
                        new SqlParameter("@UrlAliasId", news.UrlAliasId)
                        );
            }
        }

        public override void Delete(News news) {
            using (SqlConnection connection = Connect()) {
                SqlParameter historyAccount = new SqlParameter("@HistoryAccount", AccountId);
                SqlParameter id = new SqlParameter("@NewsId", news.Id);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pNewsDelete", result, historyAccount, id);
            }
        }

    }
}
