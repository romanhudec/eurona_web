using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eurona.DAL.Entities;
using System.Data.SqlClient;
using System.Data;

namespace Eurona.DAL.MSSQL {
    [Serializable]
    public sealed class AdvisorAccountStorage : CMS.MSSQL.MSSQLStorage<AdvisorAccount> {
        public AdvisorAccountStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private AdvisorAccount GetAccount(DataRow record) {
            AdvisorAccount account = new AdvisorAccount();
            account.Id = Convert.ToInt32(record["AccountId"]);
            account.Created = Convert.ToDateTime(record["Created"]);
            account.TVD_Id = ConvertNullable.ToInt32(record["TVD_Id"]);
            account.InstanceId = Convert.ToInt32(record["InstanceId"]);
            account.Email = Convert.ToString(record["Email"]);
            account.Login = Convert.ToString(record["Login"]);
            account.Password = Convert.ToString(record["Password"]);
            account.Enabled = Convert.ToBoolean(record["Enabled"]);
            account.Locale = Convert.ToString(record["Locale"]);
            account.RoleString = Convert.ToString(record["Roles"]);
            account.Verified = Convert.ToBoolean(record["Verified"]);
            account.VerifyCode = Convert.ToString(record["VerifyCode"]);
            account.EmailVerifyCode = Convert.ToString(record["EmailVerifyCode"]);
            account.EmailToVerify = Convert.ToString(record["EmailToVerify"]);
            account.EmailVerifyStatus = ConvertNullable.ToInt32(record["EmailVerifyStatus"]);
            account.EmailVerified = ConvertNullable.ToDateTime(record["EmailVerified"]);
            account.EmailBeforeVerify = Convert.ToString(record["EmailBeforeVerify"]);
            account.LoginBeforeVerify = Convert.ToString(record["LoginBeforeVerify"]);
            account.Credit = Convert.ToDecimal(record["Credit"]);
            account.CanAccessIntensa = Convert.ToBoolean(record["CanAccessIntensa"]);

            account.AdvisorCode = Convert.ToString(record["AdvisorCode"]);

            account.Name = Convert.ToString(record["Name"]);
            account.Phone = Convert.ToString(record["Phone"]);
            account.Mobile = Convert.ToString(record["Mobile"]);
            account.RegisteredAddress = Convert.ToString(record["RegisteredAddress"]);
            account.CorrespondenceAddress = Convert.ToString(record["CorrespondenceAddress"]);

            account.ZasilaniTiskovin = Convert.ToBoolean(record["ZasilaniTiskovin"]);
            account.ZasilaniNewsletter = Convert.ToBoolean(record["ZasilaniNewsletter"]);
            account.ZasilaniKatalogu = Convert.ToBoolean(record["ZasilaniKatalogu"]);

            return account;
        }

        public override List<AdvisorAccount> Read(object criteria) {
            if (criteria is AdvisorAccount.ReadById) return LoadById((criteria as AdvisorAccount.ReadById).AccountId);
            if (criteria is AdvisorAccount.ReadByEmail) return LoadByEmail((criteria as AdvisorAccount.ReadByEmail).Email);
            if (criteria is AdvisorAccount.ReadByLogin) return LoadByLogin((criteria as AdvisorAccount.ReadByLogin).Login);
            if (criteria is AdvisorAccount.ReadNewsletter) return LoadNewsletter(criteria as AdvisorAccount.ReadNewsletter);
            if (criteria is AdvisorAccount.ReadByFilter) return LoadByFilter((criteria as AdvisorAccount.ReadByFilter));
            if (criteria is AdvisorAccount.ReadDisabled) return LoadDisabled();
            if (criteria is AdvisorAccount.ReadEnabled) return LoadEnabled();
            List<AdvisorAccount> accounts = new List<AdvisorAccount>();
            using (SqlConnection connection = Connect()) {
                string sql =
                        sql = @"
				SELECT AccountId, TVD_Id, Created, InstanceId, Email, Login, Password, Locale, Verified, VerifyCode, Enabled, Credit, CanAccessIntensa, Roles,
                EmailVerifyCode, EmailToVerify, EmailVerifyStatus, EmailVerified, EmailBeforeVerify, LoginBeforeVerify, 
				AdvisorCode, Name, Phone, Mobile, RegisteredAddress, CorrespondenceAddress, ZasilaniTiskovin,ZasilaniNewsletter, ZasilaniKatalogu
				FROM vAdvisorAccounts WHERE AccountId > 1 AND ( InstanceId=@InstanceId OR InstanceId=0 ) AND  Roles LIKE'%' + @Role +'%'";

                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@InstanceId", InstanceId), new SqlParameter("@Role", Role.ADVISOR));
                foreach (DataRow dr in table.Rows) accounts.Add(GetAccount(dr));
            }
            return accounts;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        private List<AdvisorAccount> LoadNewsletter(AdvisorAccount.ReadNewsletter by) {
            List<AdvisorAccount> accounts = new List<AdvisorAccount>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
				SELECT AccountId, TVD_Id, Created, InstanceId, Email, Login, Password, Locale, Enabled, Verified, VerifyCode, Credit, CanAccessIntensa, Roles, 
                EmailVerifyCode, EmailToVerify, EmailVerifyStatus, EmailVerified, EmailBeforeVerify, LoginBeforeVerify,
				AdvisorCode, Name, Phone, Mobile, RegisteredAddress, CorrespondenceAddress, ZasilaniTiskovin,ZasilaniNewsletter, ZasilaniKatalogu
				FROM vAdvisorAccounts
				WHERE (ZasilaniNewsletter = 1 OR ZasilaniTiskovin = 1 OR ZasilaniKatalogu = 1) AND ( InstanceId=@InstanceId OR InstanceId=0 )";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows) accounts.Add(GetAccount(dr));
            }
            return accounts;
        }

        private List<AdvisorAccount> LoadByFilter(AdvisorAccount.ReadByFilter by) {
            List<AdvisorAccount> accounts = new List<AdvisorAccount>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
				SELECT AccountId, TVD_Id, Created, InstanceId, Email, Login, Password, Locale, Enabled, Verified, VerifyCode, Credit, CanAccessIntensa, Roles, 
                EmailVerifyCode, EmailToVerify, EmailVerifyStatus, EmailVerified, EmailBeforeVerify, LoginBeforeVerify,
				AdvisorCode, Name, Phone, Mobile, RegisteredAddress, CorrespondenceAddress, ZasilaniTiskovin,ZasilaniNewsletter, ZasilaniKatalogu
				FROM vAdvisorAccounts
				WHERE
                    (@RegistrationDate IS NULL OR Created=@RegistrationDate) AND  
                    (@AdvisorCode IS NULL OR AdvisorCode LIKE '%'+@AdvisorCode+'%') AND 
                    (@Login IS NULL OR Login LIKE '%'+@Login+'%') AND 
                    (@Email IS NULL OR Email LIKE '%'+@Email+'%') AND 
                    ( InstanceId=@InstanceId OR InstanceId=0 )";
                DataTable table = Query<DataTable>(connection, sql,
                    new SqlParameter("@RegistrationDate", Null(by.RegistrationDate)),
                    new SqlParameter("@AdvisorCode", Null(by.AdvisorCode)),
                    new SqlParameter("@Login", Null(by.Login)),
                    new SqlParameter("@Email", Null(by.Email)),
                    new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    accounts.Add(GetAccount(dr));
            }
            return accounts;
        }

        private List<AdvisorAccount> LoadByLogin(string login) {
            List<AdvisorAccount> accounts = new List<AdvisorAccount>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
				SELECT AccountId, TVD_Id, Created, InstanceId, Email, Login, Password, Locale, Enabled, Verified, VerifyCode, Credit, CanAccessIntensa, Roles, 
                EmailVerifyCode, EmailToVerify, EmailVerifyStatus, EmailVerified, EmailBeforeVerify, LoginBeforeVerify,
				AdvisorCode, Name, Phone, Mobile, RegisteredAddress, CorrespondenceAddress, ZasilaniTiskovin,ZasilaniNewsletter, ZasilaniKatalogu
				FROM vAdvisorAccounts
				WHERE Login = @Login AND ( InstanceId=@InstanceId OR InstanceId=0 )";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@Login", login), new SqlParameter("@InstanceId", InstanceId));
                if (table.Rows.Count > 0)
                    accounts.Add(GetAccount(table.Rows[0]));
            }
            return accounts;
        }

        private List<AdvisorAccount> LoadByEmail(string email) {
            List<AdvisorAccount> accounts = new List<AdvisorAccount>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
				SELECT AccountId, TVD_Id, Created, InstanceId, Email, Login, Password, Locale, Enabled, Verified, VerifyCode, Credit, CanAccessIntensa, Roles, 
                EmailVerifyCode, EmailToVerify, EmailVerifyStatus, EmailVerified, EmailBeforeVerify, LoginBeforeVerify,
				AdvisorCode, Name, Phone, Mobile, RegisteredAddress, CorrespondenceAddress, ZasilaniTiskovin,ZasilaniNewsletter, ZasilaniKatalogu
				FROM vAdvisorAccounts
				WHERE Email = @Email AND ( InstanceId=@InstanceId OR InstanceId=0 )";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@Email", email), new SqlParameter("@InstanceId", InstanceId));
                if (table.Rows.Count > 0)
                    accounts.Add(GetAccount(table.Rows[0]));
            }
            return accounts;
        }

        private List<AdvisorAccount> LoadById(int id) {
            List<AdvisorAccount> accounts = new List<AdvisorAccount>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
				SELECT AccountId, TVD_Id, Created, InstanceId, Email, Login, Password, Locale, Enabled, Verified, VerifyCode, Credit, CanAccessIntensa, Roles, 
                EmailVerifyCode, EmailToVerify, EmailVerifyStatus, EmailVerified, EmailBeforeVerify, LoginBeforeVerify,
				AdvisorCode, Name, Phone, Mobile, RegisteredAddress, CorrespondenceAddress, ZasilaniTiskovin,ZasilaniNewsletter, ZasilaniKatalogu
				FROM vAdvisorAccounts
				WHERE AccountId = @AccountId";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@AccountId", id));
                if (table.Rows.Count > 0)
                    accounts.Add(GetAccount(table.Rows[0]));
            }
            return accounts;
        }

        private List<AdvisorAccount> LoadDisabled() {
            List<AdvisorAccount> accounts = new List<AdvisorAccount>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
				SELECT AccountId, TVD_Id, Created, InstanceId, Email, Login, Password, Locale, Enabled, Verified, VerifyCode, Credit, CanAccessIntensa, Roles, 
                EmailVerifyCode, EmailToVerify, EmailVerifyStatus, EmailVerified, EmailBeforeVerify, LoginBeforeVerify,
				AdvisorCode, Name, Phone, Mobile, RegisteredAddress, CorrespondenceAddress, ZasilaniTiskovin,ZasilaniNewsletter, ZasilaniKatalogu
				FROM vAdvisorAccounts
				WHERE Enabled=0 AND ( InstanceId=@InstanceId OR InstanceId=0 )";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows) accounts.Add(GetAccount(dr));
            }
            return accounts;
        }

        private List<AdvisorAccount> LoadEnabled() {
            List<AdvisorAccount> accounts = new List<AdvisorAccount>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
				SELECT AccountId, TVD_Id, Created, InstanceId, Email, Login, Password, Locale, Enabled, Verified, VerifyCode, Credit, CanAccessIntensa, Roles, 
                EmailVerifyCode, EmailToVerify, EmailVerifyStatus, EmailVerified, EmailBeforeVerify, LoginBeforeVerify,
				AdvisorCode, Name, Phone, Mobile, RegisteredAddress, CorrespondenceAddress, ZasilaniTiskovin,ZasilaniNewsletter, ZasilaniKatalogu
				FROM vAdvisorAccounts
				WHERE Enabled=1 AND ( InstanceId=@InstanceId OR InstanceId=0 )";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows) accounts.Add(GetAccount(dr));
            }
            return accounts;
        }
        public override void Create(AdvisorAccount account) {
            using (SqlConnection connection = Connect()) {
                SqlParameter historyAccount = new SqlParameter("@HistoryAccount", Null(Account != null ? AccountId : (int?)1));
                SqlParameter instanceId = new SqlParameter("@InstanceId", InstanceId);
                SqlParameter tvdId = new SqlParameter("@TVD_Id", Null(account.TVD_Id));
                SqlParameter accessIntensa = new SqlParameter("@CanAccessIntensa", Null(account.CanAccessIntensa));
                SqlParameter email = new SqlParameter("@Email", Null(account.Email));
                SqlParameter login = new SqlParameter("@Login", account.Login);
                SqlParameter password = new SqlParameter("@Password", account.Password);
                SqlParameter enabled = new SqlParameter("@Enabled", account.Enabled);
                SqlParameter roles = new SqlParameter("@Roles", account.RoleString);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pAccountCreate", result, historyAccount, instanceId, tvdId, accessIntensa,
                    email, login, password, enabled, roles);
                account.Id = Convert.ToInt32(result.Value);
            }
        }

        public override void Update(AdvisorAccount account) {
            using (SqlConnection connection = Connect()) {
                SqlParameter historyAccount = new SqlParameter("@HistoryAccount", Null(Account != null ? AccountId : (int?)null));
                SqlParameter accountId = new SqlParameter("@AccountId", account.Id);
                SqlParameter tvdId = new SqlParameter("@TVD_Id", Null(account.TVD_Id));
                SqlParameter accessIntensa = new SqlParameter("@CanAccessIntensa", Null(account.CanAccessIntensa));
                SqlParameter email = new SqlParameter("@Email", Null(account.Email));
                SqlParameter login = new SqlParameter("@Login", account.Login);
                SqlParameter password = new SqlParameter("@Password", account.Password);
                SqlParameter enabled = new SqlParameter("@Enabled", account.Enabled);
                SqlParameter locale = new SqlParameter("@Locale", account.Locale);
                SqlParameter roles = new SqlParameter("@Roles", account.RoleString);
                SqlParameter verified = new SqlParameter("@Verified", account.Verified);
                SqlParameter verifyCode = new SqlParameter("@VerifyCode", account.VerifyCode);
                SqlParameter emailVerifyCode = new SqlParameter("@EmailVerifyCode", account.EmailVerifyCode);
                SqlParameter emailToVerify = new SqlParameter("@EmailToVerify", account.EmailToVerify);
                SqlParameter emailVerifyStatus = new SqlParameter("@EmailVerifyStatus", account.EmailVerifyStatus);
                SqlParameter emailVerified = new SqlParameter("@EmailVerified", account.EmailVerified);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pAccountModify", result, historyAccount, verified, verifyCode, emailVerifyCode, emailToVerify, emailVerifyStatus, emailVerified,
                    accountId, tvdId, accessIntensa, email, login, password, enabled, locale, roles);
            }
        }

        public override void Delete(AdvisorAccount account) {
            using (SqlConnection connection = Connect()) {
                SqlParameter historyAccount = new SqlParameter("@HistoryAccount", Null(Account != null ? AccountId : (int?)null));
                SqlParameter accountId = new SqlParameter("@AccountId", account.Id);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pAccountDelete", result, historyAccount, accountId);
            }
        }

        public override R Execute<R>(R command) {
            if (command is AdvisorAccount.Verify) {
                AdvisorAccount.Verify verify = command as AdvisorAccount.Verify;

                using (SqlConnection connection = Connect()) {
                    SqlParameter historyAccount = new SqlParameter("@HistoryAccount", Null(Account != null ? AccountId : (int?)null));
                    SqlParameter accountId = new SqlParameter("@AccountId", verify.Account.Id);
                    SqlParameter verifyCode = new SqlParameter("@VerifyCode", verify.VerifyCode);
                    SqlParameter result = new SqlParameter("@Result", false);
                    result.Direction = ParameterDirection.Output;
                    ExecProc(connection, "pAccountVerify", result, historyAccount, accountId, verifyCode);
                    verify.Result = result.Value == DBNull.Value ? false : Convert.ToBoolean(result.Value);
                    return verify as R;
                }
            }
            return base.Execute<R>(command);
        }
    }
}
