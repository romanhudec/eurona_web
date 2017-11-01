using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS
{
		public interface IStorage<T> where T: class, new()
		{
				void Create( T entity );
				int Count( object criteria );
				List<T> Read( object criteria );
				void Update( T entity );
				void Delete( T entity );

				R Execute<R>( R command ) where R: class, new();
		}
}
