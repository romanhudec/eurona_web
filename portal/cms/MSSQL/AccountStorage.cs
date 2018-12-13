using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL {
    [Serializable]
    internal sealed class AccountStorage : MSSQLStorage<Account> {
        public AccountStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private Account GetAccount(DataRow record) {
            Account account = new Account();
            account.Id = Convert.ToInt32(record["AccountId"]);
            account.InstanceId = Convert.ToInt32(record["InstanceId"]);
            account.Email = Convert.ToString(record["Email"]);
            account.Login = Convert.ToString(record["Login"]);
            account.Password = Convert.ToString(record["Password"]);
            account.Enabled = Convert.ToBoolean(record["Enabled"]);
            account.Locale = Convert.ToString(record["Locale"]);
            account.RoleString = Convert.ToString(record["Roles"]);
            account.Verified = Convert.ToBoolean(record["Verified"]);
            account.VerifyCode = Convert.ToString(record["VerifyCode"]);
            account.Credit = Convert.ToDecimal(record["Credit"]);
            return account;
        }

        public override List<Account> Read(object criteria) {
            if (criteria is Account.ReadById) return LoadById((criteria as Account.ReadById).AccountId);
            if (criteria is Account.ReadByEmail) return LoadByEmail((criteria as Account.ReadByEmail).Email);
            if (criteria is Account.ReadByLogin) return LoadByLogin((criteria as Account.ReadByLogin).Login);
            List<Account> accounts = new List<Account>();
            using (SqlConnection connection = Connect()) {
                //Nacitava zoznam vsetkych okrem system
                string sql = @"
										SELECT AccountId, InstanceId, Email, Login, Password, Locale, Verified, VerifyCode, Enabled, Roles, Credit
										FROM vAccounts WHERE AccountId > 1 AND InstanceId=@InstanceId";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows) accounts.Add(GetAccount(dr));
            }
            return accounts;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        private List<Account> LoadByLogin(string login) {
            List<Account> accounts = new List<Account>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
										SELECT AccountId, InstanceId, Email, Login, Password, Locale, Enabled, Verified, VerifyCode, Roles, Credit
										FROM vAccounts
										WHERE Login = @Login AND InstanceId=@InstanceId";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@Login", login), new SqlParameter("@InstanceId", InstanceId));
                if (table.Rows.Count > 0)
                    accounts.Add(GetAccount(table.Rows[0]));
            }
            return accounts;
        }

        private List<Account> LoadByEmail(string email) {
            List<Account> accounts = new List<Account>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
										SELECT AccountId, InstanceId, Email, Login, Password, Locale, Enabled, Verified, VerifyCode, Roles, Credit
										FROM vAccounts
										WHERE Email = @Email AND InstanceId=@InstanceId";
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
										SELECT AccountId, InstanceId, Email, Login, Password, Locale, Enabled, Verified, VerifyCode, Roles, Credit
										FROM vAccounts
										WHERE AccountId = @AccountId";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@AccountId", id));
                if (table.Rows.Count > 0)
                    accounts.Add(GetAccount(table.Rows[0]));
            }
            return accounts;
        }

        public override void Create(Account account) {
            using (SqlConnection connection = Connect()) {
                SqlParameter historyAccount = new SqlParameter("@HistoryAccount", Null(Account != null ? AccountId : (int?)1));
                SqlParameter instanceId = new SqlParameter("@InstanceId", InstanceId);
                SqlParameter email = new SqlParameter("@Email", Null(account.Email));
                SqlParameter login = new SqlParameter("@Login", account.Login);
                SqlParameter password = new SqlParameter("@Password", account.Password);
                SqlParameter enabled = new SqlParameter("@Enabled", account.Enabled);
                SqlParameter roles = new SqlParameter("@Roles", account.RoleString);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pAccountCreate", result, historyAccount, instanceId,
                    email, login, password, enabled, roles);
                account.Id = Convert.ToInt32(result.Value);
            }
        }

        public override void Update(Account account) {
            using (SqlConnection connection = Connect()) {
                SqlParameter historyAccount = new SqlParameter("@HistoryAccount", Null(Account != null ? AccountId : (int?)null));
                SqlParameter accountId = new SqlParameter("@AccountId", account.Id);
                SqlParameter email = new SqlParameter("@Email", Null(account.Email));
                SqlParameter login = new SqlParameter("@Login", account.Login);
                SqlParameter password = new SqlParameter("@Password", account.Password);
                SqlParameter enabled = new SqlParameter("@Enabled", account.Enabled);
                SqlParameter locale = new SqlParameter("@Locale", account.Locale);
                SqlParameter roles = new SqlParameter("@Roles", account.RoleString);
                SqlParameter verified = new SqlParameter("@Verified", account.Verified);
                SqlParameter verifyCode = new SqlParameter("@VerifyCode", account.VerifyCode);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pAccountModify", result, historyAccount, verified, verifyCode,
                    accountId, email, login, password, enabled, locale, roles);
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
