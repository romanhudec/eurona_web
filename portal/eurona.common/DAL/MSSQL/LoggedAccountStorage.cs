using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eurona.Common.DAL.Entities;
using System.Data.SqlClient;
using System.Data;
using CMS.MSSQL;
using CMS.Entities;

namespace Eurona.Common.DAL.MSSQL
{
	public sealed class LoggedAccountStorage : MSSQLStorage<LoggedAccount>
	{
		private const int max_logged_minutes = 60;
		private const string entitySelect = @"SELECT AccountId, [Login], [Email], LoggedAt, TVD_Id, InstanceId, Code, Name, LoggedMinutes, Angel_team_clen, Angel_team_manager, Angel_team_manager_typ FROM vLoggedAccounts";
		public LoggedAccountStorage(int instanceId, Account account, string connectionString)
			: base(instanceId, account, connectionString)
		{
		}

		private static LoggedAccount GetLoggedAccount(DataRow record)
		{
			LoggedAccount entity = new LoggedAccount();
			entity.Id = Convert.ToInt32(record["AccountId"]);
			entity.AccountId = Convert.ToInt32(record["AccountId"]);
			entity.InstanceId = Convert.ToInt32(record["InstanceId"]);

			entity.LoggedAt = Convert.ToDateTime(record["LoggedAt"]);
			entity.TVD_Id = ConvertNullable.ToInt32(record["TVD_Id"]);
			entity.Code = Convert.ToString(record["Code"]);
			entity.Name = Convert.ToString(record["Name"]);
			entity.Email = Convert.ToString(record["Email"]);
			entity.LoggedMinutes = Convert.ToInt32(record["LoggedMinutes"]);
			entity.AngelTeamClen = Convert.ToBoolean(record["Angel_team_clen"]);
			entity.AngelTeamManager = Convert.ToBoolean(record["Angel_team_manager"]);

			return entity;
		}

		public override List<LoggedAccount> Read(object criteria)
		{
			if (criteria is LoggedAccount.ReadLogged) return LoadLogged(max_logged_minutes, criteria as LoggedAccount.ReadLogged);
			List<LoggedAccount> list = new List<LoggedAccount>();
			using (SqlConnection connection = Connect())
			{
				string sql = entitySelect;
				sql += " WHERE InstanceId = @InstanceId";
				DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@InstanceId", InstanceId));
				foreach (DataRow dr in table.Rows)
					list.Add(GetLoggedAccount(dr));
			}
			return list;
		}

		public override int Count(object criteria)
		{
			throw new NotImplementedException();
		}

		private List<LoggedAccount> LoadLogged(int minutes, LoggedAccount.ReadLogged by)
		{
			List<LoggedAccount> list = new List<LoggedAccount>();
			using (SqlConnection connection = Connect())
			{
				string sql = entitySelect;
				sql += @" WHERE InstanceId = @InstanceId AND ( @HasTVDId IS NULL OR ( @HasTVDId IS NOT NULL AND TVD_Id IS NOT NULL ) ) AND LoggedMinutes <= @LoggedMinutes AND (@AngelTeamClen IS NULL OR Angel_team_clen=@AngelTeamClen) AND 
					(@AngelTeamManager IS NULL OR Angel_team_manager=@AngelTeamManager) AND (@AngelTeamManagerTyp IS NULL OR Angel_team_manager_typ=@AngelTeamManagerTyp)";
				DataTable table = Query<DataTable>(connection, sql,
						new SqlParameter("@InstanceId", InstanceId),
						new SqlParameter("@LoggedMinutes", minutes),
						new SqlParameter("@AngelTeamClen", Null(by.AngelTeamClen)),
						new SqlParameter("@AngelTeamManager", Null(by.AngelTeamManager)),
						new SqlParameter("@AngelTeamManagerTyp", Null(by.AngelTeamManagerTyp)),
						new SqlParameter("@HasTVDId", Null(by.HasTVDId))
						);
				foreach (DataRow dr in table.Rows)
					list.Add(GetLoggedAccount(dr));
			}
			return list;
		}


		public override void Create(LoggedAccount entity)
		{
			using (SqlConnection connection = Connect())
			{
				string sql = @"
				IF NOT EXISTS(SELECT AccountId FROM tLoggedAccount WHERE AccountId=@AccountId)
				BEGIN
					INSERT INTO tLoggedAccount (AccountId, InstanceId, LoggedAt) VALUES (@AccountId, @InstanceId, GETDATE())
				END
				ELSE
				BEGIN
					IF EXISTS(SELECT AccountId FROM vLoggedAccounts WHERE AccountId=@AccountId AND LoggedMinutes > @MaxLoggedMinutes)
					BEGIN
						UPDATE tLoggedAccount SET LoggedAt=GETDATE() WHERE AccountId=@AccountId
					END
				END";

				Exec(connection, sql,
						new SqlParameter("@InstanceId", InstanceId),
						new SqlParameter("@AccountId", entity.AccountId),
						new SqlParameter("@MaxLoggedMinutes", max_logged_minutes));
			}

		}

		public override void Update(LoggedAccount entity)
		{
			throw new InvalidOperationException();
		}

		public override void Delete(LoggedAccount entity)
		{
			throw new InvalidOperationException();
		}
	}
}
