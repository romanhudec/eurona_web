using System;
using System.Data.SqlClient;
using System.Text;

namespace CMS.MSSQL
{
	internal class MSSQLException : Exception
	{
		private string @sql = String.Empty;
		private SqlParameter[] @params = null;

		public MSSQLException(string sql)
			: base(sql)
		{
			this.sql = sql;
		}

		public MSSQLException(Exception inner, string sql)
			: base(sql, inner)
		{
			this.sql = sql;
		}

		public MSSQLException(string sql, params SqlParameter[] parameters)
			: base(sql)
		{
			this.sql = sql;
			this.@params = parameters;
		}

		public MSSQLException(Exception inner, string sql, params SqlParameter[] parameters)
			: base(sql, inner)
		{
			this.sql = sql;
			this.@params = parameters;
		}

		private string ParameterValue(object value)
		{
			if (value == null) return "null";
			if (value == DBNull.Value) return "NULL";
			return value.ToString();
		}

		private string ExtraMessage()
		{
			StringBuilder sb = new StringBuilder("SQL: ");
			sb.AppendLine(sql);
			if (@params != null && @params.Length > 0)
				foreach (SqlParameter op in @params)
					sb.AppendLine(String.Format("{0}={1}:{2}:{3}", op.ParameterName, ParameterValue(op.Value), op.TypeName, op.DbType));
			return sb.ToString();
		}

		public override string Message
		{
			get
			{
				return base.Message + Environment.NewLine + ExtraMessage();
			}
		}
	}
}
