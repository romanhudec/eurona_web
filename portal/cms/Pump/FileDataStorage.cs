using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Pump
{
		public abstract class FileDataStorage: DataStorage
		{
				private string fileName = string.Empty;
				public FileDataStorage( Stream fileStream, string fileName )
						: base( fileStream )
				{
						this.fileName = fileName;
				}

				public virtual DataTable LoadData()
				{
						return null;
				}

				public string FileName
				{
						get { return fileName; }
				}

				/// <summary>
				/// Vytvorí prázdnu DataTable zo stlpcami.
				/// </summary>
				protected virtual DataTable CreateEmptyDataTable( string[] colunNames, bool isColumnNames )
				{
						DataTable dt = new DataTable();
						int colNameIndex = 1;
						foreach ( string columnName in colunNames )
						{
								dt.Columns.Add( isColumnNames ? columnName : string.Format( "Column{0}", colNameIndex ), typeof( string ) );
								colNameIndex++;
						}

						return dt;
				}

				protected virtual void AddUnknownColumn( DataTable dt )
				{
						dt.Columns.Add( string.Format( "Unknown{0}", dt.Columns.Count + 1 ) );
				}
		}

		public class FileDataStorageException: Exception
		{
				public FileDataStorageException( string message )
						: base( message )
				{
				}

				public FileDataStorageException( string message, Exception innerException )
						: base( message, innerException )
				{
				}
		}
}
