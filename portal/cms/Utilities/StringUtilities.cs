using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace CMS.Utilities
{
		public static class StringUtilities
		{
				public static string RemoveDiacritics( string stIn )
				{
						string stFormD = stIn.Normalize( NormalizationForm.FormD );
						StringBuilder sb = new StringBuilder();
						for ( int ich = 0; ich < stFormD.Length; ich++ )
						{
								UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory( stFormD[ich] );
								if ( uc != UnicodeCategory.NonSpacingMark )
								{ sb.Append( stFormD[ich] ); }
						}
						return ( sb.ToString().Normalize( NormalizationForm.FormC ) );
				}

		}
}
