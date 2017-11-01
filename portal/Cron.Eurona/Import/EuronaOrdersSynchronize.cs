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
	/// Synchronizacia obrazkou ciselnikou z TVD
	/// </summary>
	public class EuronaOrdersSynchronize : Synchronize
	{
		private int instanceId = 0;
		public EuronaOrdersSynchronize(int instanceId, MSSQLStorage srcSqlStorage, MSSQLStorage dstSqlStorage)
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
			using (SqlConnection connection = this.DestinationDataStorage.Connect())
			{
				try
				{
					int rowIndex = 0;
					try
					{
						string sql = string.Empty;

						#region TVD->EURONA
						DataTable td = EuronaTVDDAL.GetTVDFinalOrders(this.SourceDataStorage, this.InstanceId);
						foreach (DataRow row in td.Rows)
						{
							int orderId = Convert.ToInt32(row["orderId"]);
							int orderStatusCode = Convert.ToInt32(row["StavK2"]);
							string kodMeny = GetString(row["Kod_meny"]);
							string sdeleni_pro_poradce_html = GetString(row["sdeleni_pro_poradce_html"]);

							try
							{
								EuronaDAL.Order.SyncEuronaFinalOrder(this.DestinationDataStorage, orderId, orderStatusCode, kodMeny, sdeleni_pro_poradce_html);
								OnItemProccessed(rowIndex, rowsCount, string.Format("Proccessing order '{0}' : ok", orderId));
							}
							catch (Exception ex)
							{
								string errorMessage = string.Format("Proccessing Orders : failed!", orderId);
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
						#endregion

						#region TVD->EURONA ->MENA, sdeleni_pro_poradce_html
						td = EuronaTVDDAL.GetTVDFakturyOrders(this.SourceDataStorage, this.InstanceId);
						foreach (DataRow row in td.Rows)
						{
							int orderId = Convert.ToInt32(row["orderId"]);
							string kodMeny = GetString(row["Kod_meny"]);
							string sdeleni_pro_poradce_html = GetString(row["sdeleni_pro_poradce_html"]);

							try
							{
								EuronaDAL.Order.SyncEuronaFakturyOrder(this.DestinationDataStorage, orderId, kodMeny, sdeleni_pro_poradce_html);
								OnItemProccessed(rowIndex, rowsCount, string.Format("Proccessing order '{0}' : ok", orderId));
							}
							catch (Exception ex)
							{
								string errorMessage = string.Format("Proccessing Orders : failed!", orderId);
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
						#endregion
					}
					catch (Exception ex)
					{
						string errorMessage = "Proccessing orders : failed!";
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
