using System.Configuration;
using System.Web;
using System.Web.SessionState;
using CMS;
using SHP.Entities;
using SHP.Entities.Classifiers;
using SHP.MSSQL;
using SHP.MSSQL.Classifiers;
using CMSAccount = CMS.Entities.Account;

namespace SHP
{
		/// <summary>
		/// Summary description for WebStorage
		/// </summary>
		public class WebStorage<T>: CMS.WebStorage<T> where T: class, new()
		{
				public override CMS.IStorage<T> Access()
				{
						HttpSessionState session = HttpContext.Current.Session;

						IStorage<T> access = base.Access();
						if ( access != null )
						{
								//CMS Override 
								if ( typeof( T ) != typeof( CMS.Entities.SearchEngineBase ) )
										return access;

								access = null;
								if ( session != null ) session[GetType().ToString()] = null;
						}


						if ( session != null && session[GetType().ToString()] is IStorage<T> )
								access = session[GetType().ToString()] as IStorage<T>;
						if ( access != null ) return access;

						CMSAccount account = session != null ? session["account"] as CMSAccount : null;

						string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

						int instanceId = 0;
						if ( !System.Int32.TryParse( ConfigurationManager.AppSettings["InstanceId"], out instanceId ) )
								throw new System.InvalidOperationException( "In AppSettings must be defined InstanceId!" );

						//Overrides from CMS
						if ( typeof( T ) == typeof( SHP.Entities.UrlAlias ) ) access = new SHP.MSSQL.UrlAliasStorage( instanceId, account, connectionString ) as IStorage<T>;

						//SearchEngine
						if ( typeof( T ) == typeof( CMS.Entities.SearchEngineBase ) ) access = new SearchEngineStorage( instanceId, account, connectionString ) as IStorage<T>;

						//Classifiers
						if ( typeof( T ) == typeof( VAT ) ) access = new VATStorage( instanceId, account, connectionString ) as IStorage<T>;
						if ( typeof( T ) == typeof( Currency ) ) access = new CurrencyStorage( instanceId, account, connectionString ) as IStorage<T>;
						if ( typeof( T ) == typeof( Highlight ) ) access = new HighlightStorage( instanceId, account, connectionString ) as IStorage<T>;
						if ( typeof( T ) == typeof( OrderStatus ) ) access = new OrderStatusStorage( instanceId, account, connectionString ) as IStorage<T>;
						if ( typeof( T ) == typeof( Shipment ) ) access = new ShipmentStorage( instanceId, account, connectionString ) as IStorage<T>;
						if ( typeof( T ) == typeof( Payment ) ) access = new PaymentStorage( instanceId, account, connectionString ) as IStorage<T>;

						//Entities
						if ( typeof( T ) == typeof( Address ) ) access = new AddressStorage( instanceId, account, connectionString ) as IStorage<T>;
						if ( typeof( T ) == typeof( Category ) ) access = new CategoryStorage( instanceId, account, connectionString ) as IStorage<T>;
						if ( typeof( T ) == typeof( Attribute ) ) access = new AttributeStorage( instanceId, account, connectionString ) as IStorage<T>;
						if ( typeof( T ) == typeof( Product ) ) access = new ProductStorage( instanceId, account, connectionString ) as IStorage<T>;
						if ( typeof( T ) == typeof( ProductRelation ) ) access = new ProductRelationStorage( instanceId, account, connectionString ) as IStorage<T>;
						if ( typeof( T ) == typeof( ProductReviews ) ) access = new ProductReviewsStorage( instanceId, account, connectionString ) as IStorage<T>;
						if ( typeof( T ) == typeof( ProductHighlights ) ) access = new ProductHighlightsStorage( instanceId, account, connectionString ) as IStorage<T>;
						if ( typeof( T ) == typeof( ProductCategories ) ) access = new ProductCategoriesStorage( instanceId, account, connectionString ) as IStorage<T>;
						if ( typeof( T ) == typeof( ProductComment ) ) access = new ProductCommentStorage( instanceId, account, connectionString ) as IStorage<T>;
						if ( typeof( T ) == typeof( Cart ) ) access = new CartStorage( instanceId, account, connectionString ) as IStorage<T>;
						if ( typeof( T ) == typeof( CartProduct ) ) access = new CartProductStorage( instanceId, account, connectionString ) as IStorage<T>;
						if ( typeof( T ) == typeof( Order ) ) access = new OrderStorage( instanceId, account, connectionString ) as IStorage<T>;

						if ( session != null && account != null ) session[GetType().ToString()] = access;

						return access;

				}
		}
}
