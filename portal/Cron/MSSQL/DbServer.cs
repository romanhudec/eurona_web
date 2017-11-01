using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Mothiva.Cron.MSSQL
{
	public class DbServer : IDisposable
	{
		private SqlConnection connection;

		private DbServer(SqlConnection c)
		{
			connection = c;
		}

		public static DbServer Connect()
		{
			return Connect(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
		}

		public static DbServer Connect(string connectionString)
		{
			SqlConnection connection = new SqlConnection(connectionString);
			connection.Open();
			return new DbServer(connection);
		}

		private DbException Log(DbException ex)
		{
			return ex;
		}

		public object Null(object obj)
		{
			return Null(obj, DBNull.Value);
		}

		public object Null(bool condition, object obj)
		{
			return Null(condition, obj, DBNull.Value);
		}

		public object Null(object obj, object def)
		{
			return Null(obj != null, obj, def);
		}

		public object Null(bool condition, object obj, object def)
		{
			return condition ? obj : def;
		}

		public object NullOrEmpty(string str, object def)
		{
			return String.IsNullOrEmpty(str) ? def : str;
		}

		public object NullOrEmpty(string str)
		{
			return NullOrEmpty(str, DBNull.Value);
		}

		public D Query<D>(string sql) where D : new()
		{
			try {
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
				throw Log(new DbException(ex, sql));
			}
		}

		public D Query<D>(CommandType commandType, string sql, params SqlParameter[] parameters)
			where D : new()
		{
			try {
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
				throw Log(new DbException(ex, sql, parameters));
			}
		}

		public D Query<D>(string sql, params SqlParameter[] parameters)
			where D : new()
		{
			return Query<D>(CommandType.Text, sql, parameters);
		}

		public D QueryProc<D>(string storedProc, params SqlParameter[] parameters)
			where D : new()
		{
			return Query<D>(CommandType.StoredProcedure, storedProc, parameters);
		}

		public int Exec(CommandType commandType, string sql)
		{
			try {
				using (SqlCommand cmd = new SqlCommand(sql, connection)) {
					cmd.CommandType = commandType;
					return cmd.ExecuteNonQuery();
				}
			} catch (Exception ex) {
				throw Log(new DbException(ex, sql));
			}
		}

		public int Exec(CommandType commandType, string sql, params SqlParameter[] parameters)
		{
			try {
				using (SqlCommand cmd = new SqlCommand(sql, connection)) {
					cmd.CommandType = commandType;
					foreach (SqlParameter op in parameters) cmd.Parameters.Add(op);
					return cmd.ExecuteNonQuery();
				}
			} catch (Exception ex) {
				throw Log(new DbException(ex, sql, parameters));
			}
		}

		public int Exec(string sql)
		{
			return Exec(CommandType.Text, sql);
		}

		public int Exec(string sqlCommand, params SqlParameter[] parameters)
		{
			return Exec(CommandType.Text, sqlCommand, parameters);
		}

		public int ExecProc(string sql)
		{
			return Exec(CommandType.StoredProcedure, sql);
		}

		public int ExecProc(string storedProc, params SqlParameter[] parameters)
		{
			return Exec(CommandType.StoredProcedure, storedProc, parameters);
		}

		#region IDisposable Members

		public void Dispose()
		{
			if (connection.State != ConnectionState.Closed)
				connection.Close();
			connection.Dispose();
		}

		#endregion
	}
}
