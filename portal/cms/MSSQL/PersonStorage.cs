using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Data;
using System.Data.SqlClient;

namespace CMS.MSSQL {
    [Serializable]
    internal sealed class PersonStorage : MSSQLStorage<Person> {
        public PersonStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private Person GetPerson(DataRow record) {
            Person person = new Person();
            person.Id = Convert.ToInt32(record["PersonId"]);
            person.InstanceId = Convert.ToInt32(record["InstanceId"]);
            person.AccountId = ConvertNullable.ToInt32(record["AccountId"]);
            person.Title = Convert.ToString(record["Title"]);
            person.FirstName = Convert.ToString(record["FirstName"]);
            person.LastName = Convert.ToString(record["LastName"]);
            person.Email = Convert.ToString(record["Email"]);
            person.Notes = ConvertNullable.ToString(record["Notes"]);
            person.Mobile = ConvertNullable.ToString(record["Mobile"]);
            person.Phone = ConvertNullable.ToString(record["Phone"]);
            person.AddressHomeString = ConvertNullable.ToString(record["AddressHomeString"]);
            person.AddressTempString = ConvertNullable.ToString(record["AddressTempString"]);
            person.AddressHomeId = ConvertNullable.ToInt32(record["AddressHomeId"]);
            person.AddressTempId = ConvertNullable.ToInt32(record["AddressTempId"]);

            person.MakeDisplay();

            return person;
        }

        public override List<Person> Read(object criteria) {
            if (criteria is Person.ReadById) return LoadById((criteria as Person.ReadById).PersonId);
            if (criteria is Person.ReadByAccountId) return LoadByAccountId((criteria as Person.ReadByAccountId).AccountId);
            List<Person> persons = new List<Person>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
										SELECT PersonId = p.PersonId, p.InstanceId, AccountId = p.AccountId, Title = p.Title, FirstName = p.FirstName, LastName = p.LastName, Email = p.Email, 
												Notes = p.Notes, Phone = p.Phone, Mobile = p.Mobile,
												AddressHomeId = p.AddressHomeId, AddressTempId = p.AddressTempId,
												AddressHomeString = dbo.fFormatAddress(ha.Street, ha.Zip, ha.City ),
												AddressTempString = dbo.fFormatAddress(ta.Street, ta.Zip, ta.City )
										FROM vPersons p
										LEFT JOIN tAddress ha (NOLOCK) ON ha.AddressId = p.AddressHomeId
										LEFT JOIN tAddress ta (NOLOCK) ON ta.AddressId = p.AddressTempId
										WHERE p.InstanceId=@InstanceId";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows) persons.Add(GetPerson(dr));
            }
            return persons;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        private List<Person> LoadById(int id) {
            List<Person> persons = new List<Person>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
										SELECT PersonId = p.PersonId, p.InstanceId, AccountId = p.AccountId, Title = p.Title, FirstName = p.FirstName, LastName = p.LastName, Email = p.Email, 
												Notes = p.Notes, Phone = p.Phone, Mobile = p.Mobile,
												AddressHomeId = p.AddressHomeId, AddressTempId = p.AddressTempId,
												AddressHomeString = dbo.fFormatAddress(ha.Street, ha.Zip, ha.City ),
												AddressTempString = dbo.fFormatAddress(ta.Street, ta.Zip, ta.City )
										FROM vPersons p
										LEFT JOIN tAddress ha (NOLOCK) ON ha.AddressId = p.AddressHomeId
										LEFT JOIN tAddress ta (NOLOCK) ON ta.AddressId = p.AddressTempId
										WHERE p.PersonId = @PersonId";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@PersonId", id));
                if (table.Rows.Count < 1) return persons;
                DataRow row = table.Rows[0];
                Person person = GetPerson(row);
                persons.Add(person);
            }
            return persons;
        }

        private List<Person> LoadByAccountId(int accountId) {
            List<Person> persons = new List<Person>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
										SELECT PersonId = p.PersonId, p.InstanceId, AccountId = p.AccountId, Title = p.Title, FirstName = p.FirstName, LastName = p.LastName, Email = p.Email, 
												Notes = p.Notes, Phone = p.Phone, Mobile = p.Mobile,
												AddressHomeId = p.AddressHomeId, AddressTempId = p.AddressTempId,
												AddressHomeString = dbo.fFormatAddress(ha.Street, ha.Zip, ha.City ),
												AddressTempString = dbo.fFormatAddress(ta.Street, ta.Zip, ta.City )
										FROM vPersons p
										LEFT JOIN tAddress ha (NOLOCK) ON ha.AddressId = p.AddressHomeId
										LEFT JOIN tAddress ta (NOLOCK) ON ta.AddressId = p.AddressTempId
										WHERE p.AccountId = @AccountId AND p.InstanceId=@InstanceId";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@AccountId", accountId),
                        new SqlParameter("@InstanceId", InstanceId));
                if (table.Rows.Count < 1) return persons;
                DataRow row = table.Rows[0];
                Person person = GetPerson(row);
                persons.Add(person);
            }
            return persons;
        }

        public override void Create(Person person) {
            using (SqlConnection connection = Connect()) {
                SqlParameter historyAccount = new SqlParameter("@HistoryAccount", Null(Account != null ? AccountId : (int?)null));
                SqlParameter instanceId = new SqlParameter("@InstanceId", InstanceId);
                SqlParameter accountId = new SqlParameter("@AccountId", Null(person.AccountId));
                SqlParameter title = new SqlParameter("@Title", Null(person.Title));
                SqlParameter firstName = new SqlParameter("@FirstName", Null(person.FirstName));
                SqlParameter lastName = new SqlParameter("@LastName", Null(person.LastName));
                SqlParameter email = new SqlParameter("@Email", Null(person.Email));
                SqlParameter phone = new SqlParameter("@Phone", Null(person.Phone));
                SqlParameter mobile = new SqlParameter("@Mobile", Null(person.Mobile));
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;

                DataSet ds = QueryProc<DataSet>(connection, "pPersonCreate", result, historyAccount, instanceId, accountId,
                    title, firstName, lastName, email, phone, mobile);

                /*
                if (ds.Tables.Count != 3) {
                    //Log(new MSSQLException(
                }
                */

                DataTable dtAddresHome = ds.Tables[0];
                DataTable dtAddresTemp = ds.Tables[1];
                DataTable dtPerson = ds.Tables[2];

                person.AddressHomeId = Convert.ToInt32(dtAddresHome.Rows[0][0]);
                person.AddressTempId = Convert.ToInt32(dtAddresTemp.Rows[0][0]);
                person.Id = Convert.ToInt32(dtPerson.Rows[0][0]);
            }
        }

        public override void Update(Person person) {
            using (SqlConnection connection = Connect()) {

                SqlParameter historyAccount = new SqlParameter("@HistoryAccount", Null(Account != null ? AccountId : (int?)null));
                SqlParameter personId = new SqlParameter("@PersonId", person.Id);
                SqlParameter title = new SqlParameter("@Title", Null(person.Title));
                SqlParameter firstName = new SqlParameter("@FirstName", Null(person.FirstName));
                SqlParameter lastName = new SqlParameter("@LastName", Null(person.LastName));
                SqlParameter email = new SqlParameter("@Email", Null(person.Email));
                SqlParameter phone = new SqlParameter("@Phone", Null(person.Phone));
                SqlParameter mobile = new SqlParameter("@Mobile", Null(person.Mobile));
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pPersonModify", result, historyAccount,
                    personId, title, firstName, lastName, email, phone, mobile);
            }
        }


        public override void Delete(Person person) {
            using (SqlConnection connection = Connect()) {
                SqlParameter historyAccount = new SqlParameter("@HistoryAccount", AccountId);
                SqlParameter personId = new SqlParameter("@PersonId", person.Id);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pPersonDelete", result, historyAccount, personId);
            }
        }

    }
}
