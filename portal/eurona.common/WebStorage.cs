using System.Configuration;
using System.Web;
using System.Web.SessionState;
using System;
using CMS;
using CMS.Entities;

namespace Eurona.Common
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

            //EShop
            if (typeof(T) == typeof(DAL.Entities.Uzavierka)) access = new DAL.MSSQL.UzavierkaStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(DAL.Entities.Cart)) access = new DAL.MSSQL.CartStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(DAL.Entities.CartProduct)) access = new DAL.MSSQL.CartProductStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(DAL.Entities.Order)) access = new DAL.MSSQL.OrderStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(DAL.Entities.Product)) access = new DAL.MSSQL.ProductStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(DAL.Entities.ProductRelation)) access = new DAL.MSSQL.ProductRelationStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(SHP.Entities.ProductComment)) access = new SHP.MSSQL.ProductCommentStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(CMS.Entities.Comment)) access = new CMS.MSSQL.CommentStorage(instanceId, account, connectionString) as IStorage<T>;

            if (typeof(T) == typeof(DAL.Entities.CenyProduktu)) access = new DAL.MSSQL.CenyProduktuStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(DAL.Entities.PiktogramyProduktu)) access = new DAL.MSSQL.PiktogramyProduktuStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(DAL.Entities.UcinkyProduktu)) access = new DAL.MSSQL.UcinkyProduktuStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(DAL.Entities.VlastnostiProduktu)) access = new DAL.MSSQL.VlastnostiProduktuStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(DAL.Entities.DokumentProduktuEmail)) access = new DAL.MSSQL.DokumentProduktuEmailStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(DAL.Entities.Organization)) access = new DAL.MSSQL.OrganizationStorage(instanceId, account, connectionString) as IStorage<T>;

            if (typeof(T) == typeof(DAL.Entities.BonusovyKredit)) access = new DAL.MSSQL.BonusovyKreditStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(DAL.Entities.BonusovyKreditUzivatele)) access = new DAL.MSSQL.BonusovyKreditUzivateleStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(DAL.Entities.BonusovyKreditLog)) access = new DAL.MSSQL.BonusovyKreditLogStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(DAL.Entities.AnonymniRegistrace)) access = new DAL.MSSQL.AnonymniRegistraceStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(DAL.Entities.Settings)) access = new DAL.MSSQL.SettingsStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(DAL.Entities.OrderSettings)) access = new DAL.MSSQL.OrderSettingsStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(DAL.Entities.LastOrderAddress)) access = new DAL.MSSQL.LastOrderAddressStorage(instanceId, account, connectionString) as IStorage<T>;
            //Reference

            if (session != null && account != null) session[GetType().ToString()] = access;

            return access;

        }

        private CMS.IStorage<T> GetCMSOverridedAccess()
        {
            //CMS override
            if (typeof(T) != typeof(CMS.Entities.UrlAlias))
                return null;

            IStorage<T> access = null;
            HttpSessionState session = HttpContext.Current.Session;
            Account account = session != null ? session["account"] as Account : null;

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            int instanceId = 0;
            if (!Int32.TryParse(ConfigurationManager.AppSettings["InstanceId"], out instanceId))
                throw new InvalidOperationException("In AppSettings must be defined InstanceId!");

            //CMS override
            if (typeof(T) == typeof(CMS.Entities.UrlAlias)) access = new DAL.MSSQL.UrlAliasStorage(instanceId, account, connectionString) as IStorage<T>;

            return access;
        }

        private CMS.IStorage<T> GetEshopOverridedAccess()
        {
            //ESHOP override
            if (typeof(T) != typeof(SHP.Entities.Cart) &&
                    typeof(T) != typeof(SHP.Entities.CartProduct) &&
                    typeof(T) != typeof(SHP.Entities.Order) &&
                    typeof(T) != typeof(SHP.Entities.Category) &&
                    typeof(T) != typeof(SHP.Entities.Classifiers.Currency))
                return null;

            IStorage<T> access = null;
            HttpSessionState session = HttpContext.Current.Session;
            Account account = session != null ? session["account"] as Account : null;

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            int instanceId = 0;
            if (!Int32.TryParse(ConfigurationManager.AppSettings["InstanceId"], out instanceId))
                throw new InvalidOperationException("In AppSettings must be defined InstanceId!");

            //ESHOP override
            if (typeof(T) == typeof(SHP.Entities.Category)) access = new Eurona.Common.DAL.MSSQL.CategoryStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(SHP.Entities.Cart)) access = new Eurona.Common.DAL.MSSQL.CartStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(SHP.Entities.CartProduct)) access = new Eurona.Common.DAL.MSSQL.CartProductStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(SHP.Entities.Order)) access = new Eurona.Common.DAL.MSSQL.OrderStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(SHP.Entities.Category)) access = new Eurona.Common.DAL.MSSQL.CategoryStorage(instanceId, account, connectionString) as IStorage<T>;

            if (typeof(T) == typeof(SHP.Entities.Classifiers.Currency)) access = new Eurona.Common.DAL.MSSQL.Classifiers.CurrencyStorage(instanceId, account, connectionString) as IStorage<T>;


            return access;
        }
    }
}
