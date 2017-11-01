using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eurona.DAL.Entities;
using System.Configuration;
using System.Data.SqlClient;
using Eurona.Common.DAL.Entities;
using System.Data;
using System.Web.UI;

namespace Eurona.Controls.UserManagement
{
	public class OrganizationControl : Eurona.Common.Controls.UserManagement.OrganizationControl
	{
		public OrganizationControl()
		{
			this.AutocompleteCityUrl = "~/getCityByState.ashx";
		}

		protected override bool SyncTVDUser(Organization organization)
		{
			return UpdateTVDUser(organization, !organization.TVD_Id.HasValue);
		}

		public bool UpdateTVDUser(Common.DAL.Entities.Organization organization, bool bCreate)
		{
			return OrganizationControl.UpdateTVDUser(organization, bCreate, btnSave);
		}

		public static bool SyncTVDUser(Organization organization, Control buttonControl)
		{
			return OrganizationControl.UpdateTVDUser(organization, !organization.TVD_Id.HasValue, buttonControl);
		}

		private static bool UpdateTVDUser(Common.DAL.Entities.Organization organization, bool bCreate, Control buttonControl)
		{
			Account account = Storage<Account>.ReadFirst(new Account.ReadById { AccountId = organization.AccountId.Value });

			Organization parentOrg = new Organization();
			if (organization.ParentId.HasValue) parentOrg = Storage<Organization>.ReadFirst(new Organization.ReadByTVDId { TVD_Id = organization.ParentId.Value });

			try
			{
				string connectionString = ConfigurationManager.ConnectionStrings["TVDConnectionString"].ConnectionString;
				CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(connectionString);
				using (SqlConnection connection = tvdStorage.Connect())
				{
					//Vystupne parametra
					//----------------------------------
					//Probehlo	bit	1=úspěch, 0=chyba		
					//Zprava	varchar(255)	text chyby		
					//Id_odberatele	int	prim. klíč		
					//Kod_odberatele	char(14)	reg. číslo	
					SqlParameter probehlo = new SqlParameter("@out_Probehlo", false);
					probehlo.Direction = ParameterDirection.Output;

					SqlParameter zprava = new SqlParameter("@out_Zprava", string.Empty);
					zprava.Direction = ParameterDirection.Output;
					zprava.SqlDbType = SqlDbType.VarChar;
					zprava.Size = 255;

					SqlParameter id_odberatele = new SqlParameter("@out_Id_odberatele", -1);
					id_odberatele.Direction = ParameterDirection.Output;

					SqlParameter kod_odberatele = new SqlParameter("@out_Kod_odberatele", string.Empty);
					kod_odberatele.Direction = ParameterDirection.Output;
					kod_odberatele.SqlDbType = SqlDbType.Char;
					kod_odberatele.Size = 14;
#if !__DEBUG_VERSION_WITHOUTTVD
					tvdStorage.ExecProc(connection, "esp_www_registrace",
							new SqlParameter("Nova_registrace", bCreate),
							new SqlParameter("Id_odberatele", DatabaseTools.Null(account.TVD_Id)),
							new SqlParameter("Stat_odberatele", DatabaseTools.Null(organization.RegisteredAddress.State)), // ????
							new SqlParameter("Kod_nadrizeneho", DatabaseTools.Null(parentOrg.Code)),
							new SqlParameter("Nazev_firmy", DatabaseTools.Null(organization.Name)),
							new SqlParameter("Nazev_firmy_radek", DatabaseTools.Null(organization.Name)),
							new SqlParameter("Ulice", DatabaseTools.Null(organization.RegisteredAddress.Street)),
							new SqlParameter("Psc", DatabaseTools.Null(organization.RegisteredAddress.Zip)),
							new SqlParameter("Misto", DatabaseTools.Null(organization.RegisteredAddress.City)),
							new SqlParameter("Stat", DatabaseTools.Null(organization.RegisteredAddress.State)), // ????
							new SqlParameter("Dor_nazev_firmy", DatabaseTools.Null(organization.Name)),
							new SqlParameter("Dor_nazev_firmy_radek", string.Empty),
							new SqlParameter("Dor_ulice", DatabaseTools.Null(organization.CorrespondenceAddress.Street)),
							new SqlParameter("Dor_psc", DatabaseTools.Null(organization.CorrespondenceAddress.Zip)),
							new SqlParameter("Dor_misto", DatabaseTools.Null(organization.CorrespondenceAddress.City)),
							new SqlParameter("Dor_stat", DatabaseTools.Null(organization.CorrespondenceAddress.State)), // ????
							new SqlParameter("Telefon", DatabaseTools.Null(organization.ContactPhone)),
							new SqlParameter("Mobil", DatabaseTools.Null(organization.ContactMobile)),
							new SqlParameter("E_mail", DatabaseTools.Null(organization.ContactEmail)),
							new SqlParameter("Cislo_op", DatabaseTools.Null(organization.ContactCardId)), // DOPLNIT
							new SqlParameter("Ico", DatabaseTools.Null(organization.Id1)),
							new SqlParameter("Dic", DatabaseTools.Null(organization.Id2)),
							new SqlParameter("P_f", DatabaseTools.Null(organization.PF)),// P alebo F ??
							new SqlParameter("Banka", DatabaseTools.Null(organization.BankContact.BankCode)),
							new SqlParameter("Cislo_uctu", DatabaseTools.Null(organization.BankContact.AccountNumber)),
							new SqlParameter("Login_www", DatabaseTools.Null(account.Login)),
							new SqlParameter("Datum_zahajeni", DatabaseTools.Null(DateTime.Now)),
							new SqlParameter("Platce_dph", DatabaseTools.Null(organization.VATPayment)),
							new SqlParameter("Statut", DatabaseTools.Null(organization.Statut)), //NRP nebo NRZ // ????
							new SqlParameter("Spec_symbol", DatabaseTools.Null(string.Empty)), // ????
							new SqlParameter("Kod_oblasti", DatabaseTools.Null(organization.RegionCode)), // ????
							new SqlParameter("Datum_narozeni", DatabaseTools.Null(organization.ContactBirthDay)), // DOPLNIT
							new SqlParameter("Telefon_prace", DatabaseTools.Null(organization.ContactWorkPhone)), // DOPLNIT

							new SqlParameter("Fax", DatabaseTools.Null(organization.FAX)), // DOPLNIT
							new SqlParameter("Icq", DatabaseTools.Null(organization.ICQ)), // DOPLNIT
							new SqlParameter("Skype", DatabaseTools.Null(organization.Skype)), // DOPLNIT

							new SqlParameter("Top_manager", DatabaseTools.Null(organization.TopManager)), // DOPLNIT

							probehlo, zprava, id_odberatele, kod_odberatele
							);

					//Vystupne parametra
					//----------------------------------
					//Probehlo	bit	1=úspěch, 0=chyba		
					//Zprava	varchar(255)	text chyby		
					//Id_odberatele	int	prim. klíč		
					//Kod_odberatele	char(14)	reg. číslo		
#else
					Random r = new Random( 1000 );
					probehlo.Value = true;
					zprava.Value = "";
					id_odberatele.Value = (( 999 ) * 1000) + r.Next();
					kod_odberatele.Value = "555-555555-" + r.Next().ToString();
#endif
					bool bSuccess = Convert.ToBoolean(probehlo.Value);
					string message = zprava.Value.ToString();
					if (bSuccess)
					{
						if (!bCreate) return true;

						int tvd_id = Convert.ToInt32(id_odberatele.Value);
						string code = kod_odberatele.Value.ToString();

						account.TVD_Id = tvd_id;
						Storage<Account>.Update(account);

						organization.Code = code;
						Storage<Organization>.Update(organization);
					}
					else
					{
						string js = string.Format("alert('Synchronizace se vzdáleným servrem (SAP) byla neúspěšná! Chyba: " + message + "');");
						buttonControl.Page.ClientScript.RegisterStartupScript(buttonControl.Page.GetType(), "TVDSyncOrganization", js, true);
						return false;
					}

				}
				return true;
			}
			catch (Exception ex)
			{
				CMS.EvenLog.WritoToEventLog(ex);

				string js = string.Format("alert('Synchronizace se vzdáleným servrem (SAP) byla neúspěšná!');");
				buttonControl.Page.ClientScript.RegisterStartupScript(buttonControl.Page.GetType(), "TVDSyncOrganization", js, true);
				return false;
			}
		}

		public static bool UpdateSponsorTVDUser(Common.DAL.Entities.Organization organization, Control buttonControl, out string message)
		{
			Account account = Storage<Account>.ReadFirst(new Account.ReadById { AccountId = organization.AccountId.Value });

			Organization parentOrg = new Organization();
			if (organization.ParentId.HasValue) parentOrg = Storage<Organization>.ReadFirst(new Organization.ReadByTVDId { TVD_Id = organization.ParentId.Value });

			try
			{
				string connectionString = ConfigurationManager.ConnectionStrings["TVDConnectionString"].ConnectionString;
				CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(connectionString);
				using (SqlConnection connection = tvdStorage.Connect())
				{
					//Vystupne parametra
					//----------------------------------
					//Probehlo	bit	1=úspěch, 0=chyba		
					//Zprava	varchar(255)	text chyby		
					//Id_odberatele	int	prim. klíč		
					//Kod_odberatele	char(14)	reg. číslo	
					SqlParameter probehlo = new SqlParameter("@out_Probehlo", false);
					probehlo.Direction = ParameterDirection.Output;

					SqlParameter zprava = new SqlParameter("@out_Zprava", string.Empty);
					zprava.Direction = ParameterDirection.Output;
					zprava.SqlDbType = SqlDbType.VarChar;
					zprava.Size = 255;

#if !__DEBUG_VERSION_WITHOUTTVD
					tvdStorage.ExecProc(connection, "esp_www_zmenasponzora",
							new SqlParameter("id_odberatele", DatabaseTools.Null(account.TVD_Id)),
							new SqlParameter("id_nadrizeneho", DatabaseTools.Null(parentOrg.TVD_Id)), // ????
							probehlo, zprava
							);

					//Vystupne parametra
					//----------------------------------
					//Probehlo	bit	1=úspěch, 0=chyba		
					//Zprava	varchar(255)	text chyby		
					//Id_odberatele	int	prim. klíč		
					//Kod_odberatele	char(14)	reg. číslo		
#else
					Random r = new Random( 1000 );
					probehlo.Value = true;
					zprava.Value = "";
					//id_odberatele.Value = (( 999 ) * 1000) + r.Next();
					//kod_odberatele.Value = "555-555555-" + r.Next().ToString();
#endif
					bool bSuccess = Convert.ToBoolean(probehlo.Value);
					message = zprava.Value.ToString();
					if (!bSuccess)
					{
						string js = string.Format("alert('Synchronizace se vzdáleným servrem (SAP) byla neúspěšná! Chyba: " + message + "');");
						buttonControl.Page.ClientScript.RegisterStartupScript(buttonControl.Page.GetType(), "UpdateSponsorTVDUser", js, true);
						return false;
					}

				}
				return true;
			}

			catch (Exception ex)
			{
				message = "Synchronizace se vzdáleným servrem (SAP) byla neúspěšná!";
				CMS.EvenLog.WritoToEventLog(ex);

				string js = string.Format("alert('Synchronizace se vzdáleným servrem (SAP) byla neúspěšná!');");
				buttonControl.Page.ClientScript.RegisterStartupScript(buttonControl.Page.GetType(), "UpdateSponsorTVDUser", js, true);
				return false;
			}
		}
		public static class DatabaseTools
		{
			public static object Null(object obj)
			{
				return Null(obj, DBNull.Value);
			}

			public static object Null(bool condition, object obj)
			{
				return Null(condition, obj, DBNull.Value);
			}

			public static object Null(object obj, object def)
			{
				return Null(obj != null, obj, def);
			}

			public static object Null(bool condition, object obj, object def)
			{
				return condition ? obj : def;
			}
		}
	}
}