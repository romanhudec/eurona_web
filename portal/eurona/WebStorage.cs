using System;
using System.Configuration;
using System.Web;
using System.Web.SessionState;
using CMS;
using Eurona.DAL.Entities;

namespace Eurona
{

	/// <summary>
	/// Summary description for WebStorage
	/// </summary>
	public class WebStorage<T> : CMS.WebStorage<T> where T : class, new()
	{
		public override CMS.IStorage<T> Access()
		{
			IStorage<T> access = null;
			access = GetCMSOverridedAccess();
			if (access == null) access = GetEshopOverridedAccess();
			if (access == null) access = base.Access();
			if (access != null) return access;

			HttpSessionState session = HttpContext.Current.Session;

			if (session != null && session[GetType().ToString()] is IStorage<T>)
				access = session[GetType().ToString()] as IStorage<T>;
			if (access != null) return access;

			Account account = session != null ? session["account"] as Account : null;

			string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
			int instanceId = 0;
			if (!Int32.TryParse(ConfigurationManager.AppSettings["InstanceId"], out instanceId))
				throw new InvalidOperationException("In AppSettings must be defined InstanceId!");

            if (typeof(T) == typeof(AdvisorPage)) access = new Eurona.DAL.MSSQL.AdvisorPageStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(ReklamniZasilky)) access = new Eurona.DAL.MSSQL.ReklamniZasilkyStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(ReklamniZasilkySouhlas)) access = new Eurona.DAL.MSSQL.ReklamniZasilkySouhlasStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(SingleUserCookieLinkActivity)) access = new Eurona.DAL.MSSQL.SingleUserCookieLinkActivityStorage(instanceId, account, connectionString) as IStorage<T>;
			//Prepisane CMS Entity
			if (typeof(T) == typeof(MimoradnaNabidka)) access = new Eurona.DAL.MSSQL.MimoradnaNabidkaStorage(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(AdvisorAccount)) access = new Eurona.DAL.MSSQL.AdvisorAccountStorage(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(Account)) access = new Eurona.DAL.MSSQL.AccountStorage(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(AccountExt)) access = new Eurona.DAL.MSSQL.AccountExtStorage(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(Role)) access = new Eurona.DAL.MSSQL.RoleStorage(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(HostPerson)) access = new Eurona.DAL.MSSQL.HostPersonStorage(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(Order)) access = new Eurona.DAL.MSSQL.OrderStorage(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(OrderFastView)) access = new Eurona.DAL.MSSQL.OrderFastViewStorage(instanceId, account, connectionString) as IStorage<T>;

			//Prepisane SHP Entity
			if (typeof(T) == typeof(Eurona.Common.DAL.Entities.Error)) access = new Eurona.Common.DAL.MSSQL.ErrorStorage(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(Eurona.Common.DAL.Entities.Uzavierka)) access = new Eurona.Common.DAL.MSSQL.UzavierkaStorage(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(Eurona.Common.DAL.Entities.Product)) access = new Eurona.Common.DAL.MSSQL.ProductStorage(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(Eurona.Common.DAL.Entities.Cart)) access = new Eurona.Common.DAL.MSSQL.CartStorage(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(Eurona.Common.DAL.Entities.ProductRelation)) access = new Eurona.Common.DAL.MSSQL.ProductRelationStorage(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(Eurona.Common.DAL.Entities.CartProduct)) access = new Eurona.Common.DAL.MSSQL.CartProductStorage(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(Eurona.Common.DAL.Entities.Order)) access = new Eurona.Common.DAL.MSSQL.OrderStorage(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(Eurona.Common.DAL.Entities.BonusovyKredit)) access = new Eurona.Common.DAL.MSSQL.BonusovyKreditStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(Eurona.Common.DAL.Entities.BonusovyKreditLog)) access = new Eurona.Common.DAL.MSSQL.BonusovyKreditLogStorage(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(Eurona.Common.DAL.Entities.BonusovyKreditUzivatele)) access = new Eurona.Common.DAL.MSSQL.BonusovyKreditUzivateleStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(Eurona.Common.DAL.Entities.DokumentProduktuEmail)) access = new Eurona.Common.DAL.MSSQL.DokumentProduktuEmailStorage(instanceId, account, connectionString) as IStorage<T>;

			//Referencie
			//if (typeof(T) == typeof(CMS.Entities.ForumThread)) access = new CMS.MSSQL.(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(SHP.Entities.Order)) access = new SHP.MSSQL.OrderStorage(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(SHP.Entities.Address)) access = new SHP.MSSQL.AddressStorage(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(SHP.Entities.ProductRelation)) access = new SHP.MSSQL.ProductRelationStorage(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(SHP.Entities.ProductReviews)) access = new SHP.MSSQL.ProductReviewsStorage(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(SHP.Entities.ProductHighlights)) access = new SHP.MSSQL.ProductHighlightsStorage(instanceId, account, connectionString) as IStorage<T>;

			if (typeof(T) == typeof(SHP.Entities.Classifiers.Highlight)) access = new SHP.MSSQL.Classifiers.HighlightStorage(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(SHP.Entities.Classifiers.Payment)) access = new SHP.MSSQL.Classifiers.PaymentStorage(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(SHP.Entities.Classifiers.Shipment)) access = new SHP.MSSQL.Classifiers.ShipmentStorage(instanceId, account, connectionString) as IStorage<T>;

			if (typeof(T) == typeof(Eurona.Common.DAL.Entities.AngelTeamSettings)) access = new Eurona.Common.DAL.MSSQL.AngelTeamSettingsStorage(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(Eurona.Common.DAL.Entities.AngelTeamViews)) access = new Eurona.Common.DAL.MSSQL.AngelTeamViewsStorage(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(Eurona.Common.DAL.Entities.Organization)) access = new Eurona.Common.DAL.MSSQL.OrganizationStorage(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(Eurona.Common.DAL.Entities.AnonymniRegistrace)) access = new Eurona.Common.DAL.MSSQL.AnonymniRegistraceStorage(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(Eurona.Common.DAL.Entities.AngelTeam)) access = new Eurona.Common.DAL.MSSQL.AngelTeamStorage(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(Eurona.Common.DAL.Entities.LoggedAccount)) access = new Eurona.Common.DAL.MSSQL.LoggedAccountStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(Eurona.Common.DAL.Entities.Settings)) access = new Eurona.Common.DAL.MSSQL.SettingsStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(Eurona.Common.DAL.Entities.OrderSettings)) access = new Eurona.Common.DAL.MSSQL.OrderSettingsStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(Eurona.Common.DAL.Entities.LastOrderAddress)) access = new Eurona.Common.DAL.MSSQL.LastOrderAddressStorage(instanceId, account, connectionString) as IStorage<T>;
			if (session != null && account != null) session[GetType().ToString()] = access;

			return access;

		}

		/// <summary>
		/// Prepisane CMS Storage
		/// </summary>
		/// <returns></returns>
		private CMS.IStorage<T> GetCMSOverridedAccess()
		{
			//CMS override
			if (typeof(T) != typeof(CMS.Entities.Account) &&
					typeof(T) != typeof(CMS.Entities.Role) &&
					typeof(T) != typeof(CMS.Entities.Classifiers.UrlAliasPrefix))
				return null;

			IStorage<T> access = null;
			HttpSessionState session = HttpContext.Current.Session;
			Account account = session != null ? session["account"] as Account : null;

			string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
			int instanceId = 0;
			if (!Int32.TryParse(ConfigurationManager.AppSettings["InstanceId"], out instanceId))
				throw new InvalidOperationException("In AppSettings must be defined InstanceId!");

			//CMS override
			if (typeof(T) == typeof(CMS.Entities.Account)) access = new Eurona.DAL.MSSQL.AccountStorage(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(CMS.Entities.Role)) access = new Eurona.DAL.MSSQL.RoleStorage(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(CMS.Entities.Classifiers.UrlAliasPrefix)) access = new Eurona.DAL.MSSQL.Classifiers.UrlAliasPrefixStorage(instanceId, account, connectionString) as IStorage<T>;


			return access;
		}

		/// <summary>
		/// Prepisane SHP Storage
		/// </summary>
		private CMS.IStorage<T> GetEshopOverridedAccess()
		{
			if (
					typeof(T) != typeof(SHP.Entities.Category) &&
					typeof(T) != typeof(SHP.Entities.CartProduct) &&

					typeof(T) != typeof(SHP.Entities.Classifiers.Currency) &&
					typeof(T) != typeof(SHP.Entities.Classifiers.OrderStatus)
				) return null;

			IStorage<T> access = null;
			HttpSessionState session = HttpContext.Current.Session;
			Account account = session != null ? session["account"] as Account : null;

			string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
			int instanceId = 0;
			if (!Int32.TryParse(ConfigurationManager.AppSettings["InstanceId"], out instanceId))
				throw new InvalidOperationException("In AppSettings must be defined InstanceId!");

			//ESHOP override

			if (typeof(T) == typeof(SHP.Entities.Classifiers.Currency)) access = new Eurona.DAL.MSSQL.Classifiers.CurrencyStorage(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(SHP.Entities.Classifiers.OrderStatus)) access = new Eurona.DAL.MSSQL.Classifiers.OrderStatusStorage(instanceId, account, connectionString) as IStorage<T>;

			if (typeof(T) == typeof(SHP.Entities.Category)) access = new Eurona.Common.DAL.MSSQL.CategoryStorage(instanceId, account, connectionString) as IStorage<T>;
			if (typeof(T) == typeof(SHP.Entities.CartProduct)) access = new Eurona.Common.DAL.MSSQL.CartProductStorage(instanceId, account, connectionString) as IStorage<T>;

			return access;
		}
	}
}
