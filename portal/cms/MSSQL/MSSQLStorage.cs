using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data;
using System.Data.SqlClient;

namespace CMS.MSSQL {
    [Serializable]
    public abstract class MSSQLStorage<T> : IStorage<T> where T : class, new() {
        private Account account;
        private string connectionString;
        private int instanceId;

        public MSSQLStorage(int instanceId, Account account, string connectionString) {
            this.instanceId = instanceId;
            this.account = account;
            this.connectionString = connectionString;
        }

        protected Account Account {
            get { return this.account; }
        }

        protected int AccountId {
            get { return account.Id; }
        }

        protected string Locale {
            get {
                if (account != null) return account.Locale;
                return System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower();
            }
        }

        protected int InstanceId {
            get { return instanceId; }
        }

        protected string ConnectionString {
            get { return connectionString; }
        }

        public string NormalizeSQLParameterValue(string value) {
            value = value.Replace('%', ' ').Replace('<', ' ').Replace('>', ' ').Replace('=', ' ').Replace('\'', ' ');
            return value.Trim();
        }

        protected SqlConnection Connect() {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        private MSSQLException Log(MSSQLException ex) {
            try {
                using (SqlConnection connection = Connect()) {
                    string sql = "INSERT INTO [tSysError] ([AccountId], [Category], [Error], [DateTime]) VALUES (@AccountId, 'DataLayer', @Error, GETDATE())";
                    SqlParameter[] parameters = new SqlParameter[] {
						new SqlParameter("@AccountId", AccountId),
						new SqlParameter("@Error", ex.ToString()),
					};
                    using (SqlCommand cmd = new SqlCommand(sql, connection)) {
                        cmd.CommandType = CommandType.Text;
                        foreach (SqlParameter op in parameters)
                            cmd.Parameters.Add(op);
                        cmd.ExecuteNonQuery();
                    }
                }
            } catch { //(Exception ex2) {
                //System.Diagnostics.Debug.Write(ex2);
            } finally {
            }
            return ex;
        }

        protected object Null(object obj) {
            return Null(obj, DBNull.Value);
        }

        protected object Null(bool condition, object obj) {
            return Null(condition, obj, DBNull.Value);
        }

        protected object Null(object obj, object def) {
            return Null(obj != null, obj, def);
        }

        protected object Null(bool condition, object obj, object def) {
            return condition ? obj : def;
        }

        private static Dictionary<string, string[]> similarChars = null;
        private static Dictionary<string, string[]> SimilarChars {
            get {
                if (similarChars != null) return similarChars;
                similarChars = new Dictionary<string, string[]>();
                similarChars.Add("#", new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" });
                similarChars.Add("a", new string[] { "a", "á", "ä" });
                similarChars.Add("c", new string[] { "c", "č", "ć" });
                similarChars.Add("d", new string[] { "d", "ď" });
                similarChars.Add("e", new string[] { "e", "é", "ě" });
                similarChars.Add("i", new string[] { "i", "í" });
                similarChars.Add("l", new string[] { "l", "ĺ", "ľ" });
                similarChars.Add("n", new string[] { "n", "ň", "ń" });
                similarChars.Add("o", new string[] { "o", "ó", "ô" });
                similarChars.Add("r", new string[] { "r", "ŕ", "ř" });
                similarChars.Add("s", new string[] { "s", "š", "ś" });
                similarChars.Add("t", new string[] { "t", "ť" });
                similarChars.Add("u", new string[] { "u", "ú", "ů" });
                similarChars.Add("y", new string[] { "y", "ý" });
                similarChars.Add("z", new string[] { "z", "ž", "ź" });
                return similarChars;
            }
        }

        protected string QuickFilter(string column, string filter) {
            // String.IsNullOrEmpty(filter) || 
            if (filter == "*") return null;

            StringBuilder sb = new StringBuilder(" (");

            if (SimilarChars.ContainsKey(filter)) {
                string[] similars = SimilarChars[filter];
                foreach (string similar in similars)
                    sb.AppendFormat(" OR {0} LIKE '{1}%'", column, similar);
                sb = sb.Remove(2, 4);
            } else {
                sb.AppendFormat("{0} LIKE '{1}%'", column, filter);
            }
            sb.Append(") ");

            return sb.ToString();
        }

        private int Exec(SqlConnection connection, CommandType commandType, string sql) {
            try {
                if (commandType != CommandType.StoredProcedure) {
                    sql =
                    @"SET QUOTED_IDENTIFIER ON;
                    SET ANSI_NULLS ON;
                    SET ARITHABORT ON;            
                    " + sql;
                }
                using (SqlCommand cmd = new SqlCommand(sql, connection)) {
                    cmd.CommandType = commandType;
                    return cmd.ExecuteNonQuery();
                }
            } catch (Exception ex) {
                throw Log(new MSSQLException(ex, sql));
            }
        }

        private int Exec(SqlConnection connection, CommandType commandType, string sql, params SqlParameter[] parameters) {
            try {
                if (commandType != CommandType.StoredProcedure) {
                    sql =
                    @"SET QUOTED_IDENTIFIER ON;
                    SET ANSI_NULLS ON;
                    SET ARITHABORT ON;            
                    " + sql;
                }
                using (SqlCommand cmd = new SqlCommand(sql, connection)) {
                    cmd.CommandType = commandType;
                    foreach (SqlParameter op in parameters) cmd.Parameters.Add(op);
                    return cmd.ExecuteNonQuery();
                }
            } catch (Exception ex) {
                throw Log(new MSSQLException(ex, sql, parameters));
            }
        }

        protected int Exec(SqlConnection connection, string sql) {
            return Exec(connection, CommandType.Text, sql);
        }

        protected int Exec(SqlConnection connection, string sqlCommand, params SqlParameter[] parameters) {
            return Exec(connection, CommandType.Text, sqlCommand, parameters);
        }

        protected int ExecProc(SqlConnection connection, string sql) {
            return Exec(connection, CommandType.StoredProcedure, sql);
        }

        protected int ExecProc(SqlConnection connection, string storedProc, params SqlParameter[] parameters) {
            return Exec(connection, CommandType.StoredProcedure, storedProc, parameters);
        }

        protected object Scalar(SqlConnection connection, string sql) {
            try {
                sql =
                @"SET QUOTED_IDENTIFIER ON;
                SET ANSI_NULLS ON;
                SET ARITHABORT ON;            
                " + sql;
                using (SqlCommand cmd = new SqlCommand(sql, connection)) {
                    cmd.CommandType = CommandType.Text;
                    return cmd.ExecuteScalar();
                }
            } catch (Exception ex) {
                throw Log(new MSSQLException(ex, sql));
            }
        }

        protected object Scalar(SqlConnection connection, string sql, params SqlParameter[] parameters) {
            try {
                sql =
                @"SET QUOTED_IDENTIFIER ON;
                SET ANSI_NULLS ON;
                SET ARITHABORT ON;            
                " + sql;
                using (SqlCommand cmd = new SqlCommand(sql, connection)) {
                    cmd.CommandType = CommandType.Text;
                    foreach (SqlParameter op in parameters)
                        cmd.Parameters.Add(op);
                    return cmd.ExecuteScalar();
                }
            } catch (Exception ex) {
                throw Log(new MSSQLException(ex, sql));
            }
        }

        protected D Query<D>(SqlConnection connection, string sql)
            where D : new() {
            try {
                sql =
                @"SET QUOTED_IDENTIFIER ON;
                SET ANSI_NULLS ON;
                SET ARITHABORT ON;            
                " + sql;
                using (SqlCommand cmd = new SqlCommand(sql, connection)) {
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd)) {
                        D d = new D();
                        if (d is DataTable)
                            da.Fill(d as DataTable);
                        else if (d is DataSet)
                            da.Fill(d as DataSet);
                        else throw new InvalidCastException();
                        return d;
                    }
                }
            } catch (Exception ex) {
                throw Log(new MSSQLException(ex, sql));
            }
        }

        private D Query<D>(SqlConnection connection, CommandType commandType, string sql, params SqlParameter[] parameters)
            where D : new() {
            try {
                if (commandType != CommandType.StoredProcedure) {
                    sql =
                    @"SET QUOTED_IDENTIFIER ON;
                    SET ANSI_NULLS ON;
                    SET ARITHABORT ON;            
                    " + sql;
                }
                using (SqlCommand cmd = new SqlCommand(sql, connection)) {
                    cmd.CommandType = commandType;
                    if (parameters != null && parameters.Length > 0)
                        foreach (SqlParameter op in parameters)
                            cmd.Parameters.Add(op);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd)) {
                        D d = new D();
                        if (d is DataTable)
                            da.Fill(d as DataTable);
                        else if (d is DataSet)
                            da.Fill(d as DataSet);
                        else throw new InvalidCastException();
                        return d;
                    }
                }
            } catch (Exception ex) {
                throw Log(new MSSQLException(ex, sql, parameters));
            }
        }

        protected D Query<D>(SqlConnection connection, string sql, params SqlParameter[] parameters)
            where D : new() {
            return Query<D>(connection, CommandType.Text, sql, parameters);
        }

        protected D QueryProc<D>(SqlConnection connection, string storedProc, params SqlParameter[] parameters)
            where D : new() {
            return Query<D>(connection, CommandType.StoredProcedure, storedProc, parameters);
        }

        public abstract void Create(T entity);
        public abstract int Count(object criteria);
        public abstract List<T> Read(object criteria);
        public abstract void Update(T entity);
        public abstract void Delete(T entity);

        public virtual R Execute<R>(R command) where R : class, new() {
            return default(R);
        }
    }
}
