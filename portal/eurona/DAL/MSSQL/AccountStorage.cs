using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eurona.DAL.Entities;
using System.Data.SqlClient;
using System.Data;

namespace Eurona.DAL.MSSQL {
    [Serializable]
    internal sealed class AccountStorage : CMS.MSSQL.MSSQLStorage<Account> {
        public AccountStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private Account GetAccount(DataRow record) {
            Account account = new Account();
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
            account.MustChangeAccountPassword = Convert.ToBoolean(record["MustChangeAccount"]);
            account.PasswordChanged = ConvertNullable.ToDateTime(record["PasswordChanged"]);
            account.SingleUserCookieLinkEnabled = Convert.ToBoolean(record["SingleUserCookieLinkEnabled"]);
            return account;
        }

        public override List<Account> Read(object criteria) {
            if (criteria is Account.ReadById) return LoadById((criteria as Account.ReadById).AccountId);
            if (criteria is Account.ReadByEmail) return LoadByEmail((criteria as Account.ReadByEmail).Email);
            if (criteria is Account.ReadByLogin) return LoadByLogin((criteria as Account.ReadByLogin).Login);
            if (criteria is Account.ReadByEmailVerifyCode) return LoadByEmailVerifyCode((criteria as Account.ReadByEmailVerifyCode).EmailVerifyCode);
            if (criteria is Account.ReadByEmailToVerify) return LoadByEmailToVerify((criteria as Account.ReadByEmailToVerify).EmailToVerify, (criteria as Account.ReadByEmailToVerify).OnlyEmailVerified);
            if (criteria is Account.ReadByLoginAndInstance) return LoadByLoginAndInstance((criteria as Account.ReadByLoginAndInstance).Login, (criteria as Account.ReadByLoginAndInstance).InstanceId);
            List<Account> accounts = new List<Account>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
				SELECT AccountId, TVD_Id, Created, InstanceId, Email, Login, Password, Locale, Verified, VerifyCode, 
                EmailVerifyCode, EmailToVerify, EmailVerifyStatus, EmailVerified, EmailBeforeVerify, LoginBeforeVerify,
                Enabled, Credit, CanAccessIntensa, Roles, MustChangeAccount, PasswordChanged, SingleUserCookieLinkEnabled
				FROM vAccounts WHERE AccountId > 1 AND ( InstanceId=@InstanceId OR InstanceId=0 ) ";

                if (((Eurona.DAL.Entities.Account)Account).IsInRole(Role.OPERATOR)) {
                    sql = @"
					SELECT AccountId, TVD_Id, Created, InstanceId, Email, Login, Password, Locale, Verified, VerifyCode, 
                    EmailVerifyCode, EmailToVerify, EmailVerifyStatus, EmailVerified, EmailBeforeVerify, LoginBeforeVerify,                    
                    Enabled, Credit, CanAccessIntensa, Roles, MustChangeAccount, PasswordChanged, SingleUserCookieLinkEnabled
					FROM vAccounts WHERE AccountId > 1 AND ( InstanceId=@InstanceId OR InstanceId=0 ) AND  Roles LIKE'%' + @Role +'%'";
                }
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@InstanceId", InstanceId), new SqlParameter("@Role", Role.ADVISOR));
                foreach (DataRow dr in table.Rows) accounts.Add(GetAccount(dr));
            }
            return accounts;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        private List<Account> LoadByEmailVerifyCode(string emailVerifyCode) {
            List<Account> accounts = new List<Account>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
				SELECT AccountId, TVD_Id, Created, InstanceId, Email, Login, Password, Locale, Enabled, Verified, VerifyCode, 
                EmailVerifyCode, EmailToVerify, EmailVerifyStatus, EmailVerified, EmailBeforeVerify, LoginBeforeVerify,         
                Credit, CanAccessIntensa, Roles, MustChangeAccount, PasswordChanged, SingleUserCookieLinkEnabled
				FROM vAccounts
				WHERE EmailVerifyCode = @EmailVerifyCode";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@EmailVerifyCode", emailVerifyCode));
                if (table.Rows.Count > 0)
                    accounts.Add(GetAccount(table.Rows[0]));
            }
            return accounts;
        }

        private List<Account> LoadByEmailToVerify(string emailToVerify, bool onlyEmailVerified) {
            List<Account> accounts = new List<Account>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
				SELECT AccountId, TVD_Id, Created, InstanceId, Email, Login, Password, Locale, Enabled, Verified, VerifyCode, 
                EmailVerifyCode, EmailToVerify, EmailVerifyStatus, EmailVerified, EmailBeforeVerify, LoginBeforeVerify,         
                Credit, CanAccessIntensa, Roles, MustChangeAccount, PasswordChanged, SingleUserCookieLinkEnabled
				FROM vAccounts
				WHERE EmailToVerify = @EmailToVerify AND 
                (@OnlyEmailVerified = 0 OR (@OnlyEmailVerified = 1 AND EmailVerified IS NOT NULL) )";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@EmailToVerify", emailToVerify), new SqlParameter("@OnlyEmailVerified", onlyEmailVerified));
                if (table.Rows.Count > 0)
                    accounts.Add(GetAccount(table.Rows[0]));
            }
            return accounts;
        } 

        private List<Account> LoadByLogin(string login) {
            login = NormalizeSQLParameterValue(login);
            List<Account> accounts = new List<Account>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
				SELECT AccountId, TVD_Id, Created, InstanceId, Email, Login, Password, Locale, Enabled, Verified, VerifyCode, 
                EmailVerifyCode, EmailToVerify, EmailVerifyStatus, EmailVerified, EmailBeforeVerify, LoginBeforeVerify,         
                Credit, CanAccessIntensa, Roles, MustChangeAccount, PasswordChanged, SingleUserCookieLinkEnabled
				FROM vAccounts
				WHERE Login = @Login --AND ( InstanceId=@InstanceId OR InstanceId=0 )";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@Login", login), new SqlParameter("@InstanceId", InstanceId));
                if (table.Rows.Count > 0)
                    accounts.Add(GetAccount(table.Rows[0]));
            }
            return accounts;
        }

        private List<Account> LoadByEmail(string email) {
            email = NormalizeSQLParameterValue(email);
            List<Account> accounts = new List<Account>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
				SELECT AccountId, TVD_Id, Created, InstanceId, Email, Login, Password, Locale, Enabled, Verified, VerifyCode, 
                EmailVerifyCode, EmailToVerify, EmailVerifyStatus, EmailVerified, EmailBeforeVerify, LoginBeforeVerify,         
                Credit, CanAccessIntensa, Roles, MustChangeAccount, PasswordChanged, SingleUserCookieLinkEnabled
				FROM vAccounts
				WHERE Email = @Email AND ( InstanceId=@InstanceId OR InstanceId=0 )";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@Email", email), new SqlParameter("@InstanceId", InstanceId));
                if (table.Rows.Count > 0)
                    accounts.Add(GetAccount(table.Rows[0]));
            }
            return accounts;
        }

        private List<Account> LoadById(int id) {
            List<Account> accounts = new List<Account>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
				SELECT AccountId, TVD_Id, Created, InstanceId, Email, Login, Password, Locale, Enabled, Verified, 
                EmailVerifyCode, EmailToVerify, EmailVerifyStatus, EmailVerified, EmailBeforeVerify, LoginBeforeVerify,         
                VerifyCode, Credit, CanAccessIntensa, Roles, MustChangeAccount, PasswordChanged, SingleUserCookieLinkEnabled
				FROM vAccounts
				WHERE AccountId = @AccountId";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@AccountId", id));
                if (table.Rows.Count > 0)
                    accounts.Add(GetAccount(table.Rows[0]));
            }
            return accounts;
        }
        private List<Account> LoadByLoginAndInstance(string login, int instanceId) {
            login = NormalizeSQLParameterValue(login);
            List<Account> accounts = new List<Account>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
				SELECT AccountId, TVD_Id, Created, InstanceId, Email, Login, Password, Locale, Enabled, Verified, VerifyCode, 
                EmailVerifyCode, EmailToVerify, EmailVerifyStatus, EmailVerified, EmailBeforeVerify, LoginBeforeVerify,         
                Credit, CanAccessIntensa, Roles, MustChangeAccount, PasswordChanged, SingleUserCookieLinkEnabled
				FROM vAccounts
				WHERE Login = @Login AND InstanceId=@InstanceId";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@Login", login), new SqlParameter("@InstanceId", instanceId));
                if (table.Rows.Count > 0)
                    accounts.Add(GetAccount(table.Rows[0]));
            }
            return accounts;
        }
        public override void Create(Account account) {
            if (string.IsNullOrEmpty(account.Login))
                account.Login = account.Email;
            if (string.IsNullOrEmpty(account.Login))
                account.Login = Guid.NewGuid().ToString();

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
                SqlParameter mustChangeAccount = new SqlParameter("@MustChangeAccount", account.MustChangeAccountPassword);
                SqlParameter passwordChanged = new SqlParameter("@PasswordChanged", account.PasswordChanged);
                SqlParameter singleUserCookieLinkEnabled = new SqlParameter("@SingleUserCookieLinkEnabled", account.SingleUserCookieLinkEnabled);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pAccountCreate", result, historyAccount, instanceId, tvdId, accessIntensa,
                    email, login, password, enabled, roles, mustChangeAccount, passwordChanged, singleUserCookieLinkEnabled);
                account.Id = Convert.ToInt32(result.Value);
            }
        }

        public override void Update(Account account) {
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
                SqlParameter mustChangeAccount = new SqlParameter("@MustChangeAccount", account.MustChangeAccountPassword);
                SqlParameter passwordChanged = new SqlParameter("@PasswordChanged", account.PasswordChanged);
                SqlParameter verified = new SqlParameter("@Verified", account.Verified);
                SqlParameter verifyCode = new SqlParameter("@VerifyCode", account.VerifyCode);
                SqlParameter emailVerifyCode = new SqlParameter("@EmailVerifyCode", account.EmailVerifyCode);
                SqlParameter emailToVerify = new SqlParameter("@EmailToVerify", account.EmailToVerify);
                SqlParameter emailVerifyStatus = new SqlParameter("@EmailVerifyStatus", account.EmailVerifyStatus);
                SqlParameter emailVerified = new SqlParameter("@EmailVerified", account.EmailVerified);
        
                SqlParameter singleUserCookieLinkEnabled = new SqlParameter("@SingleUserCookieLinkEnabled", account.SingleUserCookieLinkEnabled);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pAccountModify", result, historyAccount, verified, verifyCode, emailVerifyCode, emailToVerify, emailVerifyStatus, emailVerified,
                    accountId, tvdId, accessIntensa, email, login, password, enabled, locale, roles, mustChangeAccount, passwordChanged, singleUserCookieLinkEnabled);
            }
        }

        public override void Delete(Account account) {
            using (SqlConnection connection = Connect()) {
                SqlParameter historyAccount = new SqlParameter("@HistoryAccount", Null(Account != null ? AccountId : (int?)null));
                SqlParameter accountId = new SqlParameter("@AccountId", account.Id);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pAccountDelete", result, historyAccount, accountId);
            }
        }

        public override R Execute<R>(R command) {
            if (command is Account.Verify) {
                Account.Verify verify = command as Account.Verify;

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
