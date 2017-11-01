using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Pump
{
		public sealed class CsvFileDataStorage: FileDataStorage
		{
				private string separator = string.Empty;
				private bool firstLineIsColumnNames = true;

				public CsvFileDataStorage( Stream fileStream, string fileName, string separator, bool firstLineIsColumnNames ) :
						base( fileStream, fileName )
				{
						this.separator = separator;
						this.firstLineIsColumnNames = firstLineIsColumnNames;
				}

				public override DataTable LoadData()
				{
						TextReader tr = null;
						DataTable dt = null;

						try
						{
								tr = new StreamReader( (Stream)this.DataSource, System.Text.Encoding.Default );
								string line = tr.ReadLine();
								line = NormalizeLine( line );

								string[] rowData = line.Split( separator.ToCharArray() );
								dt = CreateEmptyDataTable( rowData, firstLineIsColumnNames );

								if ( firstLineIsColumnNames )
								{
										line = tr.ReadLine();
										line = NormalizeLine( line );
								}

								while ( line != null )
								{
										rowData = line.Split( separator.ToCharArray() );
										if ( dt.Columns.Count < rowData.Length )
												AddUnknownColumn( dt );

										dt.Rows.Add( rowData );

										line = tr.ReadLine();
										if ( line != null )
												line = NormalizeLine( line );

								}
						}
						catch ( Exception ex )
						{
								throw new FileDataStorageException( string.Format( "Load csv file '{0}; into failed!", this.FileName ), ex );
						}
						finally
						{
								if ( tr != null )
										tr.Close();
						}

						return dt;
				}

				private string NormalizeLine( string line )
				{
						return line.Replace( "\"", "" );
				}
		}
}
