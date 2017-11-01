using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Pump;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Drawing;

namespace Cron.Eurona.Import
{
	/// <summary>
	/// Synchronizacia Bonusovych kreditov z TVD
	/// </summary>
	public class CernyForLifeBonusoveKredityUzivateleSynchronize : Synchronize
	{
		private int instanceId = 0;
		public CernyForLifeBonusoveKredityUzivateleSynchronize(int instanceId, MSSQLStorage srcSqlStorage, MSSQLStorage dstSqlStorage)
			: base(srcSqlStorage, dstSqlStorage)
		{
			this.instanceId = instanceId;
		}

		public int InstanceId { get { return this.instanceId; } }

		public override void Synchronize()
		{
			int addedItems = 0;
			int updatedItems = 0;
			int errorItems = 0;
			int ignoredItems = 0;

			int rowsCount = 0;
			using (SqlConnection connection = this.SourceDataStorage.Connect())
			{
				try
				{
					int rowIndex = 0;
					try
					{
						string sql = string.Empty;
						//TVD->CL
						DataTable td = EuronaDAL.BonusoveKredityUzivatele.GetBonusoveKredityUzivatele(this.SourceDataStorage, this.InstanceId);
						foreach (DataRow row in td.Rows)
						{
							int yyyymm = Convert.ToInt32(row["YYYYMM"]);
							int kredit = Convert.ToInt32(row["Kredit"]);
							int idOdberatele = Convert.ToInt32(row["TVD_Id"]);

							try
							{
								sql = @"
								IF EXISTS(SELECT RRRRMM, Id_odberatele  FROM CL_www_bonuskredit_naroky WHERE RRRRMM = @yyyymm AND Id_odberatele = @idOdberatele ) BEGIN
										UPDATE CL_www_bonuskredit_naroky WITH (ROWLOCK) SET Narok=@kredit WHERE RRRRMM = @yyyymm AND Id_odberatele = @idOdberatele
								END
								ELSE
								BEGIN
										INSERT INTO CL_www_bonuskredit_naroky WITH (ROWLOCK) (RRRRMM, Id_odberatele, Narok) VALUES (@yyyymm, @idOdberatele, @kredit)
								END";
								using (SqlConnection connectionDst = this.DestinationDataStorage.Connect())
								{
									this.DestinationDataStorage.Exec(connectionDst, sql,
											new SqlParameter("@yyyymm", yyyymm),
											new SqlParameter("@kredit", kredit),
											new SqlParameter("@idOdberatele", idOdberatele));
								}
								OnItemProccessed(rowIndex, rowsCount, string.Format("Proccessing bonusove kredity TVD_Id={0} and YYYYMM={1} : ok", idOdberatele, yyyymm));
							}
							catch (Exception ex)
							{
								string errorMessage = string.Format("Proccessing bonusove kredity TVD_Id={0} and YYYYMM={1} failed!", idOdberatele, yyyymm);
								StringBuilder sbMessage = new StringBuilder();
								sbMessage.Append(errorMessage);
								sbMessage.AppendLine(ex.Message);
								if (ex.InnerException != null) sbMessage.AppendLine(ex.InnerException.Message);
								sbMessage.AppendLine(ex.StackTrace);

								OnError(errorMessage, ex);
#if !_OFFLINE_DEBUG
								SendEmail(errorMessage, sbMessage.ToString());
#endif
							}
						}

					}
					catch (Exception ex)
					{
						string errorMessage = "Proccessing accounts : failed!";
						StringBuilder sbMessage = new StringBuilder();
						sbMessage.Append(errorMessage);
						sbMessage.AppendLine(ex.Message);
						if (ex.InnerException != null) sbMessage.AppendLine(ex.InnerException.Message);
						sbMessage.AppendLine(ex.StackTrace);

						OnError(errorMessage, ex);
#if !_OFFLINE_DEBUG
						SendEmail(errorMessage, sbMessage.ToString());
#endif
						errorItems++;
					}
					finally
					{
						rowIndex++;
					}

				}
				finally
				{
					OnFinish(rowsCount, addedItems, updatedItems, errorItems, ignoredItems);
				}
			}
		}

		private string GetString(object obj)
		{
			if (obj == null) return null;
			return obj.ToString().Trim();
		}

	}
}
