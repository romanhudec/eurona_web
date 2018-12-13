using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL {
    [Serializable]
    internal sealed class ImageGalleryStorage : MSSQLStorage<ImageGallery> {
        public ImageGalleryStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static ImageGallery GetImageGallery(DataRow record) {
            ImageGallery imageGallery = new ImageGallery();
            imageGallery.Id = Convert.ToInt32(record["ImageGalleryId"]);
            imageGallery.InstanceId = Convert.ToInt32(record["InstanceId"]);
            imageGallery.Date = Convert.ToDateTime(record["Date"]);
            imageGallery.EnableComments = NullableDBToBool(record["EnableComments"]);
            imageGallery.EnableVotes = NullableDBToBool(record["EnableVotes"]);
            imageGallery.Name = Convert.ToString(record["Name"]);
            imageGallery.Description = Convert.ToString(record["Description"]);
            imageGallery.RoleId = ConvertNullable.ToInt32(record["RoleId"]);
            imageGallery.Visible = NullableDBToBool(record["Visible"]);
            imageGallery.UrlAliasId = ConvertNullable.ToInt32(record["UrlAliasId"]);
            imageGallery.Alias = Convert.ToString(record["Alias"]);
            imageGallery.ItemsCount = Convert.ToInt32(record["ItemsCount"]);
            imageGallery.CommentsCount = Convert.ToInt32(record["CommentsCount"]);
            imageGallery.ViewCount = Convert.ToInt32(record["ViewCount"]);

            return imageGallery;
        }

        private static bool NullableDBToBool(object dbValue) {
            if (dbValue == DBNull.Value) return false;
            return Convert.ToInt32(dbValue) == 1;
        }

        public override List<ImageGallery> Read(object criteria) {
            if (criteria is ImageGallery.ReadById) return LoadById(criteria as ImageGallery.ReadById);
            if (criteria is ImageGallery.ReadForCurrentAccount) return LoadForCurrentAccount(criteria as ImageGallery.ReadForCurrentAccount);
            List<ImageGallery> imageGallerys = new List<ImageGallery>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT ImageGalleryId, InstanceId, RoleId, Name, Description, Date, EnableComments, EnableVotes, Visible, [UrlAliasId], Alias, ViewCount, ItemsCount, CommentsCount
								FROM vImageGalleries WHERE InstanceId=@InstanceId
								ORDER BY Date DESC";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    imageGallerys.Add(GetImageGallery(dr));
            }
            return imageGallerys;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        private List<ImageGallery> LoadById(ImageGallery.ReadById byImageGalleryId) {
            List<ImageGallery> imageGallerys = new List<ImageGallery>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT ImageGalleryId, InstanceId, RoleId, Name, Description, Date, EnableComments, EnableVotes, Visible, [UrlAliasId], Alias, ViewCount, ItemsCount, CommentsCount
								FROM vImageGalleries
								WHERE ImageGalleryId = @ImageGalleryId
								ORDER BY Date DESC";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@ImageGalleryId", byImageGalleryId.ImageGalleryId));
                foreach (DataRow dr in table.Rows)
                    imageGallerys.Add(GetImageGallery(dr));
            }
            return imageGallerys;
        }

        private List<ImageGallery> LoadForCurrentAccount(ImageGallery.ReadForCurrentAccount by) {
            List<ImageGallery> imageGallerys = new List<ImageGallery>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								DECLARE @AdministratorRoleId INT
								SELECT @AdministratorRoleId = RoleId FROM vRoles WHERE Name = @AdministratorRoleName

								IF @LogedAccountId IS NULL
										SELECT ImageGalleryId, InstanceId, RoleId, Name, Description, Date, EnableComments, EnableVotes, Visible, [UrlAliasId], Alias, ViewCount, ItemsCount, CommentsCount
										FROM vImageGalleries 
										WHERE InstanceId=@InstanceId AND RoleId IS NULL AND (@TagId IS NULL OR ImageGalleryId IN ( SELECT ImageGalleryId FROM vImageGalleryTags WHERE TagId = @TagId )) AND
												Visible = 1
										ORDER BY Date DESC
								ELSE
										SELECT DISTINCT ImageGalleryId, g.InstanceId, g.RoleId, Name, Description, Date, EnableComments, EnableVotes, Visible, [UrlAliasId], Alias, ViewCount, ItemsCount, CommentsCount
										FROM vImageGalleries g
										INNER JOIN vAccountRoles ar (NOLOCK) ON (ar.RoleId = ISNULL(g.RoleId, ar.RoleId) OR ar.RoleId = @AdministratorRoleId) AND ar.AccountId = @LogedAccountId
										WHERE g.InstanceId=@InstanceId AND (@TagId IS NULL OR ImageGalleryId IN ( SELECT ImageGalleryId FROM vImageGalleryTags WHERE TagId = @TagId )) AND
												Visible = 1
										ORDER BY Date DESC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@TagId", Null(by.TagId)),
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@LogedAccountId", Account != null ? (object)Account.Id : (object)DBNull.Value),
                        new SqlParameter("@AdministratorRoleName", Role.ADMINISTRATOR));

                foreach (DataRow dr in table.Rows)
                    imageGallerys.Add(GetImageGallery(dr));
            }
            return imageGallerys;
        }

        private void DeleteImageGalleryTags(ImageGallery imageGallery) {
            using (SqlConnection connection = Connect()) {
                string sql = @"DELETE FROM tImageGalleryTag WHERE ImageGalleryId=@ImageGalleryId";
                SqlParameter imageGalleryId = new SqlParameter("@ImageGalleryId", imageGallery.Id);
                Exec(connection, sql, imageGalleryId);
            }
        }

        private void UpdateImageGalleryTags(ImageGallery imageGallery) {
            //Nacitanie povodnych hodnot
            List<ImageGalleryTag> tags = imageGallery.ImageGalleryTags;

            //Vymazanie starych hodnot
            DeleteImageGalleryTags(imageGallery);

            using (SqlConnection connection = Connect()) {
                foreach (ImageGalleryTag at in tags) {
                    at.ImageGalleryId = imageGallery.Id;
                    Storage<ImageGalleryTag>.Create(at);
                }
            }
        }

        public override void Create(ImageGallery imageGallery) {
            using (SqlConnection connection = Connect()) {
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;

                ExecProc(connection, "pImageGalleryCreate",
                        new SqlParameter("@HistoryAccount", AccountId),
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@EnableComments", imageGallery.EnableComments),
                        new SqlParameter("@EnableVotes", imageGallery.EnableVotes),
                        new SqlParameter("@Name", imageGallery.Name),
                        new SqlParameter("@Description", Null(imageGallery.Description)),
                        new SqlParameter("@Date", Null(imageGallery.Date)),
                        new SqlParameter("@RoleId", imageGallery.RoleId),
                        new SqlParameter("@UrlAliasId", imageGallery.UrlAliasId),
                        new SqlParameter("@Visible", imageGallery.Visible),
                        result);

                imageGallery.Id = Convert.ToInt32(result.Value);
            }

            UpdateImageGalleryTags(imageGallery);
        }

        public override void Update(ImageGallery imageGallery) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pImageGalleryModify",
                        new SqlParameter("@HistoryAccount", Null(Account != null ? AccountId : (int?)null)),
                        new SqlParameter("@ImageGalleryId", imageGallery.Id),
                        new SqlParameter("@EnableComments", imageGallery.EnableComments),
                        new SqlParameter("@EnableVotes", imageGallery.EnableVotes),
                        new SqlParameter("@Name", imageGallery.Name),
                        new SqlParameter("@Description", Null(imageGallery.Description)),
                        new SqlParameter("@Date", Null(imageGallery.Date)),
                        new SqlParameter("@RoleId", imageGallery.RoleId),
                        new SqlParameter("@UrlAliasId", imageGallery.UrlAliasId),
                        new SqlParameter("@Visible", imageGallery.Visible));
            }

            UpdateImageGalleryTags(imageGallery);
        }

        public override void Delete(ImageGallery imageGallery) {
            DeleteImageGalleryTags(imageGallery);

            using (SqlConnection connection = Connect()) {
                SqlParameter historyAccount = new SqlParameter("@HistoryAccount", AccountId);
                SqlParameter imageGalleryId = new SqlParameter("@ImageGalleryId", imageGallery.Id);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pImageGalleryDelete", result, historyAccount, imageGalleryId);
            }
        }

        public override R Execute<R>(R command) {
            Type t = typeof(R);
            if (t == typeof(ImageGallery.IncrementViewCountCommand))
                return IncrementViewCount(command as ImageGallery.IncrementViewCountCommand) as R;

            return base.Execute<R>(command);
        }

        private ImageGallery.IncrementViewCountCommand IncrementViewCount(ImageGallery.IncrementViewCountCommand cmd) {
            using (SqlConnection connection = Connect()) {
                SqlParameter imageGalleryId = new SqlParameter("@ImageGalleryId", cmd.ImageGalleryId);
                ExecProc(connection, "pImageGalleryIncrementViewCount", imageGalleryId);
            }

            return cmd;
        }
    }
}
