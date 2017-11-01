using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using CMS.MSSQL;
using CMS.Entities;
using Eurona.Common.DAL.Entities;

namespace Eurona.Common.DAL.MSSQL
{
	public sealed class AngelTeamSettingsStorage : MSSQLStorage<AngelTeamSettings>
	{
		private string entitySelect = "SELECT * FROM tAngelTeamSettings";
		public AngelTeamSettingsStorage(int instanceId, Account account, string connectionString)
			: base(instanceId, account, connectionString)
		{
		}

		private static AngelTeamSettings GetAngelTeamSettings(DataRow record)
		{
			AngelTeamSettings entiry = new AngelTeamSettings();
			entiry.DisableATP = Convert.ToBoolean(record["DisableATP"]);
			entiry.MaxViewPerMinute = Convert.ToInt32(record["MaxViewPerMinute"]);
			entiry.BlockATPHours = Convert.ToInt32(record["BlockATPHours"]);

			return entiry;
		}

		public override List<AngelTeamSettings> Read(object criteria)
		{
			List<AngelTeamSettings> list = new List<AngelTeamSettings>();

			using (SqlConnection connection = Connect())
			{
				DataTable table = null;
				table = Query<DataTable>(connection, entitySelect);
				foreach (DataRow dr in table.Rows)
					list.Add(GetAngelTeamSettings(dr));

			}
			return list;
		}

		public override int Count(object criteria)
		{
			throw new NotImplementedException();
		}

		public override void Create(AngelTeamSettings entity)
		{
			using (SqlConnection connection = Connect())
			{
				Exec(connection,
				@"INSERT INTO tAngelTeamSettings (DisableATP, MaxViewPerMinute, BlockATPHours ) VALUES (@DisableATP, @MaxViewPerMinute, @BlockATPHours )",
					new SqlParameter("@DisableATP", entity.DisableATP),
					new SqlParameter("@MaxViewPerMinute", entity.MaxViewPerMinute),
					new SqlParameter("@BlockATPHours", entity.BlockATPHours)
				);
			}
		}

		public override void Update(AngelTeamSettings entity)
		{
			using (SqlConnection connection = Connect())
			{
				Exec(connection, 
				@"UPDATE tAngelTeamSettings SET DisableATP=@DisableATP, MaxViewPerMinute=@MaxViewPerMinute, BlockATPHours=@BlockATPHours",
					new SqlParameter("@DisableATP", entity.DisableATP),
					new SqlParameter("@MaxViewPerMinute", entity.MaxViewPerMinute),
					new SqlParameter("@BlockATPHours", entity.BlockATPHours)
				);
			}
		}

		public override void Delete(AngelTeamSettings entity)
		{
			throw new NotImplementedException();
		}
	}
}
