using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;

namespace Eurona.Common
{
		public class VocabularyUtilities
		{
				private List<Translation> dictionary;
				public VocabularyUtilities( string name )
				{
						if ( dictionary == null ) dictionary = Storage<Translation>.Read( new Translation.ReadByVocabulary { Vocabulary = name } );
				}

				public Translation Translate( string term )
				{
						Translation trans = dictionary.FirstOrDefault( x => x.Term == term );
						if ( trans == default( Translation ) ) return new Translation { Trans = String.Format( "!{0}:{1}", System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName, term ) };
						return trans;
				}
		}
}
