using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;

namespace SHP.Entities
{
		public class Address: Entity
		{
				public class ReadById
				{
						public int AddressId { get; set; }
				}

				public int InstanceId { get; set; }
				public string FirstName { get; set; }
				public string LastName { get; set; }
				public string Organization { get; set; }
				public string Id1 { get; set; }
				public string Id2 { get; set; }
				public string Id3 { get; set; }
				public string Phone { get; set; }
				public string Email { get; set; }

				public string Street { get; set; }
				public string Zip { get; set; }
				public string City { get; set; }
				public string State { get; set; }
				public string Notes { get; set; }

				// Format Display
				public string Display { get; set; }
				public void MakeDisplay()
				{
						StringBuilder sb = new StringBuilder();
						if ( !String.IsNullOrEmpty( Organization ) ) sb.AppendFormat( "{0} ", Organization );
						if ( !String.IsNullOrEmpty( FirstName ) ) sb.AppendFormat( "{0} ", FirstName );
						if ( !String.IsNullOrEmpty( LastName ) ) sb.AppendFormat(  "{0} ", LastName );
						if ( !String.IsNullOrEmpty( Street ) ) sb.AppendFormat( "{0} ", Street );
						if ( !String.IsNullOrEmpty( City ) ) sb.AppendFormat( "{0} ", City );
						if ( !String.IsNullOrEmpty( Zip ) ) sb.AppendFormat( "{0} ", Zip );
						Display = sb.ToString();
				}
		}
}
