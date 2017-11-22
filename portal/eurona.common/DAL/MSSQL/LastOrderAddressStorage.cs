using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SHP.Entities;
using CMS.MSSQL;
using Eurona.Common.DAL.Entities;

namespace Eurona.Common.DAL.MSSQL {
    public sealed class LastOrderAddressStorage : MSSQLStorage<LastOrderAddress> {
        public LastOrderAddressStorage(int instanceId, CMS.Entities.Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static LastOrderAddress GetAddress(DataRow record) {
            LastOrderAddress address = new LastOrderAddress();
            address.Id = Convert.ToInt32(record["AddressId"]);
            address.AccountId = Convert.ToInt32(record["AccountId"]);

            address.FirstName = Convert.ToString(record["FirstName"]);
            address.LastName = Convert.ToString(record["LastName"]);
            address.Organization = Convert.ToString(record["Organization"]);
            address.Id1 = Convert.ToString(record["Id1"]);
            address.Id2 = Convert.ToString(record["Id2"]);
            address.Id3 = Convert.ToString(record["Id3"]);
            address.Phone = Convert.ToString(record["Phone"]);
            address.Email = Convert.ToString(record["Email"]);
            address.Street = Convert.ToString(record["Street"]);
            address.Zip = Convert.ToString(record["Zip"]);
            address.City = Convert.ToString(record["City"]);
            address.State = Convert.ToString(record["State"]);

            address.Notes = Convert.ToString(record["Notes"]);

            address.MakeDisplay();

            return address;
        }

        public override List<LastOrderAddress> Read(object criteria) {
            if (criteria is LastOrderAddress.ReadById) return LoadById(criteria as LastOrderAddress.ReadById);
            if (criteria is LastOrderAddress.ReadByAccountId) return LoadById(criteria as LastOrderAddress.ReadByAccountId);
            List<LastOrderAddress> addesses = new List<LastOrderAddress>();          
            return addesses;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        private List<LastOrderAddress> LoadById(LastOrderAddress.ReadById byAddressId) {
            List<LastOrderAddress> addesses = new List<LastOrderAddress>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
					SELECT AddressId, AccountId ,FirstName ,LastName ,Organization ,Id1 ,Id2 ,Id3 ,City ,Street ,Zip,
							State ,Phone ,Email ,Notes
					FROM tShpLastOrderAddress
					WHERE AddressId = @AddressId";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@AddressId", byAddressId.AddressId));
                foreach (DataRow dr in table.Rows)
                    addesses.Add(GetAddress(dr));
            }
            return addesses;
        }
        private List<LastOrderAddress> LoadById(LastOrderAddress.ReadByAccountId byAccountId) {
            List<LastOrderAddress> addesses = new List<LastOrderAddress>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT AddressId, AccountId ,FirstName ,LastName ,Organization ,Id1 ,Id2 ,Id3 ,City ,Street ,Zip,
										State ,Phone ,Email ,Notes
								FROM tShpLastOrderAddress
								WHERE AccountId = @AccountId";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@AccountId", byAccountId.AccountId));
                foreach (DataRow dr in table.Rows)
                    addesses.Add(GetAddress(dr));
            }
            return addesses;
        }

        public override void Create(LastOrderAddress address) {
            using (SqlConnection connection = Connect()) {
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;

                string sql = @"INSERT INTO tShpLastOrderAddress (AccountId ,FirstName ,LastName ,Organization ,Id1 ,Id2 ,Id3 ,City ,Street ,Zip, State ,Phone ,Email ,Notes)
                             VALUES( @AccountId ,@FirstName ,@LastName ,@Organization ,@Id1 ,@Id2 ,@Id3 ,@City ,@Street ,@Zip, @State ,@Phone ,@Email ,@Notes)";
                Exec(connection, sql,
                        new SqlParameter("@AccountId", address.AccountId),
                        new SqlParameter("@FirstName", Null(address.FirstName)),
                        new SqlParameter("@LastName", Null(address.LastName)),
                        new SqlParameter("@Organization", Null(address.Organization)),
                        new SqlParameter("@Id1", Null(address.Id1)),
                        new SqlParameter("@Id2", Null(address.Id2)),
                        new SqlParameter("@Id3", Null(address.Id3)),
                        new SqlParameter("@City", Null(address.City)),
                        new SqlParameter("@Street", Null(address.Street)),
                        new SqlParameter("@Zip", Null(address.Zip)),
                        new SqlParameter("@State", Null(address.State)),
                        new SqlParameter("@Phone", Null(address.Phone)),
                        new SqlParameter("@Email", Null(address.Email)),
                        new SqlParameter("@Notes", Null(address.Notes)));
            }
        }

        public override void Update(LastOrderAddress address) {
            using (SqlConnection connection = Connect()) {
                string sql = @"UPDATE tShpLastOrderAddress SET AccountId=@AccountId ,FirstName=@FirstName  ,LastName=@LastName ,Organization=@Organization ,
                Id1=@Id1 ,Id2=@Id2 ,Id3=@Id3 ,City=@City ,Street=@Street ,Zip=@Zip, State=@State ,Phone=@Phone ,Email=@Email ,Notes=@Notes 
                WHERE AddressId=@AddressId";
                Exec(connection, sql,
                        new SqlParameter("@AddressId", address.Id),
                        new SqlParameter("@AccountId", address.AccountId),
                        new SqlParameter("@FirstName", Null(address.FirstName)),
                        new SqlParameter("@LastName", Null(address.LastName)),
                        new SqlParameter("@Organization", Null(address.Organization)),
                        new SqlParameter("@Id1", Null(address.Id1)),
                        new SqlParameter("@Id2", Null(address.Id2)),
                        new SqlParameter("@Id3", Null(address.Id3)),
                        new SqlParameter("@City", Null(address.City)),
                        new SqlParameter("@Street", Null(address.Street)),
                        new SqlParameter("@Zip", Null(address.Zip)),
                        new SqlParameter("@State", Null(address.State)),
                        new SqlParameter("@Phone", Null(address.Phone)),
                        new SqlParameter("@Email", Null(address.Email)),
                        new SqlParameter("@Notes", Null(address.Notes)));
            }
        }

        public override void Delete(LastOrderAddress address) {
            using (SqlConnection connection = Connect()) {
                SqlParameter addressId = new SqlParameter("@AddressId", address.Id);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "DELETE FROM tShpLastOrderAddress WHERE AddressId=@AddressId", result, addressId);
            }
        }

    }
}
