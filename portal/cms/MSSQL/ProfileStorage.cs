using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data.SqlClient;
using System.Data;

namespace CMS.MSSQL
{
		internal sealed class ProfileStorage: MSSQLStorage<Profile>
		{
				public ProfileStorage( int instanceId, Account account, string connectionString )
						: base( instanceId, account, connectionString )
				{
				}

				private static Profile GetProfile( DataRow record )
				{
						Profile profile = new Profile();
						profile.Id = Convert.ToInt32( record["ProfileId"] );
						profile.InstanceId = Convert.ToInt32( record["InstanceId"] );
						profile.Name = Convert.ToString( record["Name"] );
						profile.Type = Convert.ToInt32( record["Type"] );
						profile.Description = Convert.ToString( record["Description"] );
						
						return profile;
				}

				public override List<Profile> Read( object criteria )
				{
						if ( criteria is Profile.ReadById ) return LoadById( criteria as Profile.ReadById );
						List<Profile> profiles = new List<Profile>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT ProfileId, InstanceId, Name, Type, Description
								FROM vProfiles WHERE InstanceId=@InstanceId";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@InstanceId", InstanceId ) );
								foreach ( DataRow dr in table.Rows )
										profiles.Add( GetProfile( dr ) );
						}
						return profiles;
				}

				public override int Count( object criteria )
				{
						throw new NotImplementedException();
				}

				private List<Profile> LoadById( Profile.ReadById byProfileId )
				{
						List<Profile> profiles = new List<Profile>();
						using ( SqlConnection connection = Connect() )
						{
								string sql = @"
								SELECT ProfileId, InstanceId, Name, Type, Description
								FROM vProfiles
								WHERE ProfileId = @ProfileId";
								DataTable table = Query<DataTable>( connection, sql, new SqlParameter( "@ProfileId", byProfileId.ProfileId ) );
								foreach ( DataRow dr in table.Rows )
										profiles.Add( GetProfile( dr ) );
						}
						return profiles;
				}

	
				public override void Create( Profile profile )
				{
						throw new NotImplementedException();
				}

				public override void Update( Profile profile )
				{
						throw new NotImplementedException();
				}

				public override void Delete( Profile profile )
				{
						throw new NotImplementedException();
				}
		}
}
