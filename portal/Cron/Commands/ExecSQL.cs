using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Mothiva.Cron.MSSQL;

namespace Mothiva.Cron.Commands
{
	public class ExecSQL : Mothiva.Cron.CronCommandBase
	{
		public ExecSQL(string entryName)
			: base(entryName)
		{
		}

		public override bool Exec(string command, Dictionary<string, string> parameters)
		{
			/*
			try {
				string connectionString = parameters["connectionString"];
				using (SqlConnection con = new SqlConnection(connectionString)) {
					con.Open();
					using (SqlCommand cmd = con.CreateCommand()) {
						cmd.CommandType = System.Data.CommandType.Text;
						cmd.CommandText = command;
						cmd.ExecuteNonQuery();
						return true;
					}
				}
			} catch (Exception ex) {
				base.WriteLogLine(ex);
				return false;
			}
			*/
			string cs = parameters["connectionString"];
			using (DbServer db = DbServer.Connect(cs)) {
				db.Exec(command);
				return true;
			}
		}
	}
}
