using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL {
    [Serializable]
    public class SearchEngineStorage : MSSQLStorage<SearchEngineBase> {
        public SearchEngineStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static SearchEngineBase GetResult(DataRow record) {
            SearchEngineBase result = new SearchEngineBase();
            result.Id = Convert.ToInt32(record["Id"]);
            result.Title = Convert.ToString(record["Title"]);
            result.Content = Convert.ToString(record["Content"]);
            result.UrlAlias = Convert.ToString(record["UrlAlias"]);
            result.ImageUrl = Convert.ToString(record["ImageUrl"]);
            result.RoleId = ConvertNullable.ToInt32(record["RoleId"]);
            result.RoleString = Convert.ToString(record["RoleString"]);

            return result;
        }

        public override List<SearchEngineBase> Read(object criteria) {
            List<SearchEngineBase> list = new List<SearchEngineBase>();

            if ((criteria is CmsSearchEngineEntity.ISearch)) {
                CmsSearchEngineEntity.ISearch search = criteria as CmsSearchEngineEntity.ISearch;
                using (SqlConnection connection = Connect()) {
                    string searchProcedure = string.Empty;
                    if (search is CmsSearchEngineEntity.SearchPages)
                        searchProcedure = "pSearchPages";
                    else if (search is CmsSearchEngineEntity.SearchArticles)
                        searchProcedure = "pSearchArticles";
                    else if (search is CmsSearchEngineEntity.SearchBlogs)
                        searchProcedure = "pSearchBlogs";
                    else if (search is CmsSearchEngineEntity.SearchNews)
                        searchProcedure = "pSearchNews";
                    else if (search is CmsSearchEngineEntity.SearchImageGalleries)
                        searchProcedure = "pSearchImageGalleries";
                    if (search is CmsSearchEngineEntity.SearchForums)
                        searchProcedure = "pSearchForums";
                    if (search is CmsSearchEngineEntity.SearchForumPosts)
                        searchProcedure = "pSearchForumPosts";
                    if (!string.IsNullOrEmpty(searchProcedure)) {
                        DataTable table = QueryProc<DataTable>(connection, searchProcedure,
                                new SqlParameter("@Keywords", search.Keywords),
                                new SqlParameter("@Locale", Locale),
                                new SqlParameter("@InstanceId", InstanceId));
                        foreach (DataRow dr in table.Rows)
                            list.Add(GetResult(dr));
                    }
                }
            } else if ((criteria is CmsSearchEngineEntity.ICommentSearch)) {
                CmsSearchEngineEntity.ICommentSearch search = criteria as CmsSearchEngineEntity.ICommentSearch;
                using (SqlConnection connection = Connect()) {
                    string searchProcedure = string.Empty;
                    if (search is CmsSearchEngineEntity.SearchArticleComments)
                        searchProcedure = "pSearchArticleComments";
                    else if (search is CmsSearchEngineEntity.SearchBlogComments)
                        searchProcedure = "pSearchBlogComments";
                    else if (search is CmsSearchEngineEntity.SearchImageGalleryComments)
                        searchProcedure = "pSearchImageGalleryComments";
                    else if (search is CmsSearchEngineEntity.SearchImageGalleryItemComments)
                        searchProcedure = "pSearchImageGalleryItemComments";

                    if (!string.IsNullOrEmpty(searchProcedure)) {
                        DataTable table = QueryProc<DataTable>(connection, searchProcedure,
                                new SqlParameter("@Keywords", search.Keywords),
                                new SqlParameter("@Locale", Locale),
                                new SqlParameter("@InstanceId", InstanceId),
                                new SqlParameter("@CommentAliasPostFix", search.CommentAliasPostFix));
                        foreach (DataRow dr in table.Rows)
                            list.Add(GetResult(dr));
                    }
                }
            } else
                throw new InvalidCastException("Criteria mus implement interface CmsSearchEngineEntity.ISearch OR CmsSearchEngineEntity.ICommentSearch");

            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }
        public override void Create(SearchEngineBase entity) {
            throw new NotImplementedException();
        }
        public override void Delete(SearchEngineBase entity) {
            throw new NotImplementedException();
        }
        public override void Update(SearchEngineBase entity) {
            throw new NotImplementedException();
        }

    }
}
