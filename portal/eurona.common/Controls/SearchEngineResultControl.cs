using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using SHP.Entities;
using System.IO;

namespace Eurona.Common.Controls.SearchEngine
{
		public class SearchEngineResultControl: CMS.Controls.SearchEngine.SearchEngineResultControl
		{
				protected override List<CMS.Entities.SearchEngineBase> GetResultData( string keywords )
				{
						List<CMS.Entities.SearchEngineBase> list = base.GetResultData( keywords );
						List<CMS.Entities.SearchEngineBase> listProducts = Storage<CMS.Entities.SearchEngineBase>.Read( new ShpSearchEngineEntity.SearchProducts { Keywords = keywords } );

						//Fill dataTable 
						string storagePathFormat = string.Format( "{0}{1}/_t/", ConfigValue( "SHP:ImageGallery:Product:StoragePath" ), "{0}" );
						string productImagesPathFormat = Server.MapPath( storagePathFormat );

						foreach ( CMS.Entities.SearchEngineBase item in listProducts )
						{
								string path = string.Format( productImagesPathFormat, item.Id );
								if ( !Directory.Exists( path ) )
										continue;

								DirectoryInfo di = new DirectoryInfo( path );
								FileInfo[] fileInfos = di.GetFiles( "*.*" );
								if ( fileInfos.Length == 0 ) continue;

								string url = string.Format( storagePathFormat, item.Id ) + fileInfos[0].Name;
								item.ImageUrl = url;
						}

						list.AddRange( listProducts );
						return list;
				}
		}
}
