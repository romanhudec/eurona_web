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
	public sealed class AngelTeamViewsStorage : MSSQLStorage<AngelTeamViews>
	{
		private string entitySelect = "SELECT * FROM tAngelTeamViews";
		public AngelTeamViewsStorage(int instanceId, Account account, string connectionString)
			: base(instanceId, account, connectionString)
		{
		}

		private static AngelTeamViews GetAngelTeamViews(DataRow record)
		{
			AngelTeamViews entiry = new AngelTeamViews();
			entiry.AccountId = Convert.ToInt32(record["AccountId"]);
			entiry.ViewDate = Convert.ToDateTime(record["ViewDate"]);
			entiry.ViewCount = Convert.ToInt32(record["ViewCount"]);

			return entiry;
		}

		public override List<AngelTeamViews> Read(object criteria)
		{
			List<AngelTeamViews> list = new List<AngelTeamViews>();

			string sql = entitySelect;
			if (criteria is AngelTeamViews.ReadByAccount)
			{
				sql += " WHERE AccountId=@AccountId";
				using (SqlConnection connection = Connect())
				{
					DataTable table = null;
					table = Query<DataTable>(connection, sql, new SqlParameter("@AccountId", (criteria as AngelTeamViews.ReadByAccount).AccountId));
					foreach (DataRow dr in table.Rows)
						list.Add(GetAngelTeamViews(dr));

				}
				return list;
			}

			using (SqlConnection connection = Connect())
			{
				DataTable table = null;
				table = Query<DataTable>(connection, sql);
				foreach (DataRow dr in table.Rows)
					list.Add(GetAngelTeamViews(dr));

			}
			return list;
		}

		public override int Count(object criteria)
		{
			throw new NotImplementedException();
		}

		public override void Create(AngelTeamViews entity)
		{
			throw new NotImplementedException();
		}

		public override void Update(AngelTeamViews entity)
		{
			using (SqlConnection connection = Connect())
			{
				Exec(connection,
				@"
				IF NOT EXISTS( SELECT AccountId FROM tAngelTeamViews WHERE AccountId=@AccountId )
				BEGIN
					INSERT INTO tAngelTeamViews (AccountId, ViewDate, ViewCount) VALUES (@AccountId, GETDATE(), 1)
				END
				ELSE
				BEGIN
					IF EXISTS( SELECT AccountId FROM tAngelTeamViews WHERE AccountId=@AccountId AND DATEDIFF ( mi , ViewDate , GETDATE() ) <= 1 )
						UPDATE tAngelTeamViews SET ViewCount=ViewCount+1 WHERE AccountId=@AccountId
					ELSE
						UPDATE tAngelTeamViews SET ViewCount=1, ViewDate=GETDATE() WHERE AccountId=@AccountId
				END
				",
					new SqlParameter("@AccountId", entity.AccountId)
				);
			}
		}

		public override void Delete(AngelTeamViews entity)
		{
			throw new NotImplementedException();
		}
	}
}
