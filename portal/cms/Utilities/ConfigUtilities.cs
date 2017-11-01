using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web.UI;

namespace CMS.Utilities
{
		public static class ConfigUtilities
		{
				public static string ConfigValue( string key, Page page )
				{
						string value = ConfigValue( key );
						if ( value.StartsWith( "~" ) ) value = page.ResolveUrl( value );
						return value;
				}

				public static string ConfigValue( string key )
				{
						string value = ConfigurationManager.AppSettings[key];
						if ( String.IsNullOrEmpty( value ) ) throw new Exception( String.Format( "Key '{0}' not found! Check your web.config file.", key ) );
						return value;
				}

				/// <summary>
				/// Metóda vráti virtualnu cestu na Icon storage path pre danz typ entity
				/// </summary>
				public static string GetEntityIconStoragePath( Type entityType )
				{
						string storagePath = ConfigValue( "CMS:EntityIcon:StoragePath" );
						string storageDirectoty = storagePath + string.Format( "{0}/", entityType.Name );
						return storageDirectoty;
				}
		}
}
