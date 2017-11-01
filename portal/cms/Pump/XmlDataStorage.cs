using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace CMS.Pump
{
		public abstract class XmlDataStorage: DataStorage
		{
				public XmlDataStorage( object dataSource )
						: base( dataSource )
				{
				}

				public virtual XDocument LoadData() 
				{
						return null;
				}
		}

		public class XmlFileDataStorage: XmlDataStorage
		{
				public XmlFileDataStorage( string fileName )
						: base( fileName )
				{
				}

				public override XDocument LoadData()
				{
						XDocument xmlDoc = XDocument.Load( this.DataSource.ToString() );
						return xmlDoc;

				}

				public string FileName
				{
						get { return this.DataSource.ToString(); }
				}
		}

		public class XmlStreamDataStorage: XmlDataStorage
		{
				public XmlStreamDataStorage( Stream stream )
						: base( stream )
				{
				}

				public override XDocument LoadData()
				{
						XmlTextReader sr = new XmlTextReader( (Stream)this.DataSource );
						XDocument xmlDoc = XDocument.Load( sr );

						return xmlDoc;
				}
		}
}
