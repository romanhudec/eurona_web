using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eurona.DAL.Entities;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Eurona.Common.DAL.Entities;

namespace Eurona.Controls
{
	public class AngelTeamHelper
	{

		/// <summary>
		/// Vrati aktualny pocet EuronaStars.
		/// </summary>
		/// 
		public static int GetPocetEuronaStars(Account account)
		{
			int pocetEuronaStars = 0;
			if (!account.TVD_Id.HasValue) return pocetEuronaStars;

			string connectionStringTVD = ConfigurationManager.ConnectionStrings["TVDConnectionString"].ConnectionString;
			CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(connectionStringTVD);
			using (SqlConnection connection = tvdStorage.Connect())
			{
				string sql = @"SELECT PocetEuronaStars = ISNULL(SUM(Hvezdy_celkem),0) FROM provize_aktualni_angelteam 
				WHERE Id_odberatele = @idOdberatele";
				DataTable dt = tvdStorage.Query(connection, sql,
						new SqlParameter("@idOdberatele", account.TVD_Id));

				if (dt.Rows.Count != 0)
				{
					pocetEuronaStars = Convert.ToInt32(dt.Rows[0]["PocetEuronaStars"]);
				}
			}

			return pocetEuronaStars;
		}

	}
}