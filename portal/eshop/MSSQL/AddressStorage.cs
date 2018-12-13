using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SHP.Entities;
using CMS.MSSQL;

namespace SHP.MSSQL {
    [Serializable]
    public sealed class AddressStorage : MSSQLStorage<Address> {
        public AddressStorage(int instanceId, CMS.Entities.Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static Address GetAddress(DataRow record) {
            Address address = new Address();
            address.Id = Convert.ToInt32(record["AddressId"]);
            address.InstanceId = Convert.ToInt32(record["InstanceId"]);

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

        public override List<Address> Read(object criteria) {
            if (criteria is Address.ReadById) return LoadById(criteria as Address.ReadById);
            List<Address> addesses = new List<Address>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT AddressId, InstanceId ,FirstName ,LastName ,Organization ,Id1 ,Id2 ,Id3 ,City ,Street ,Zip,
										State ,Phone ,Email ,Notes
								FROM vShpAddresses WHERE InstanceId=@InstanceId";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows)
                    addesses.Add(GetAddress(dr));
            }
            return addesses;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        private List<Address> LoadById(Address.ReadById byAddressId) {
            List<Address> addesses = new List<Address>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
								SELECT AddressId, InstanceId ,FirstName ,LastName ,Organization ,Id1 ,Id2 ,Id3 ,City ,Street ,Zip,
										State ,Phone ,Email ,Notes
								FROM vShpAddresses
								WHERE AddressId = @AddressId";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@AddressId", byAddressId.AddressId));
                foreach (DataRow dr in table.Rows)
                    addesses.Add(GetAddress(dr));
            }
            return addesses;
        }

        public override void Create(Address address) {
            using (SqlConnection connection = Connect()) {
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;

                ExecProc(connection, "pShpAddressCreate",
                        new SqlParameter("@HistoryAccount", AccountId),
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@FirstName", address.FirstName),
                        new SqlParameter("@LastName", address.LastName),
                        new SqlParameter("@Organization", address.Organization),
                        new SqlParameter("@Id1", address.Id1),
                        new SqlParameter("@Id2", address.Id2),
                        new SqlParameter("@Id3", address.Id3),
                        new SqlParameter("@City", address.City),
                        new SqlParameter("@Street", address.Street),
                        new SqlParameter("@Zip", address.Zip),
                        new SqlParameter("@State", address.State),
                        new SqlParameter("@Phone", address.Phone),
                        new SqlParameter("@Email", address.Email),
                        new SqlParameter("@Notes", address.Notes),
                        result);

                address.Id = Convert.ToInt32(result.Value);
            }
        }

        public override void Update(Address address) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pShpAddressModify",
                    new SqlParameter("@HistoryAccount", Null(Account != null ? AccountId : (int?)null)),
                        new SqlParameter("@AddressId", address.Id),
                        new SqlParameter("@FirstName", address.FirstName),
                        new SqlParameter("@LastName", address.LastName),
                        new SqlParameter("@Organization", address.Organization),
                        new SqlParameter("@Id1", address.Id1),
                        new SqlParameter("@Id2", address.Id2),
                        new SqlParameter("@Id3", address.Id3),
                        new SqlParameter("@City", address.City),
                        new SqlParameter("@Street", address.Street),
                        new SqlParameter("@Zip", address.Zip),
                        new SqlParameter("@State", address.State),
                        new SqlParameter("@Phone", address.Phone),
                        new SqlParameter("@Email", address.Email),
                        new SqlParameter("@Notes", address.Notes));
            }
        }

        public override void Delete(Address address) {
            using (SqlConnection connection = Connect()) {
                SqlParameter historyAccount = new SqlParameter("@HistoryAccount", AccountId);
                SqlParameter addressId = new SqlParameter("@AddressId", address.Id);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pShpAddressDelete", result, historyAccount, addressId);
            }
        }

    }
}
