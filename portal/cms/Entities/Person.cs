using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities {
    [Serializable]
    public class Person : Entity {
        public class ReadById {
            public int PersonId { get; set; }
        }

        public class ReadByAccountId {
            public int AccountId { get; set; }
        }

        public int InstanceId { get; set; }
        //FK Account
        public int? AccountId { get; set; }
        private Account account = null;
        public Account Account {
            get {
                if (!this.AccountId.HasValue) return null;

                if (account != null) return account;
                account = Storage<Account>.ReadFirst(new Account.ReadById { AccountId = this.AccountId.Value });
                return account;
            }
        }


        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Notes { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }

        //FK Home address
        public int? AddressHomeId { get; set; }
        public string AddressHomeString { get; set; }
        private Address addressHome = null;
        public Address AddressHome {
            get {
                if (!this.AddressHomeId.HasValue) return null;

                if (addressHome != null) return addressHome;
                addressHome = Storage<Address>.ReadFirst(new Address.ReadById { AddressId = this.AddressHomeId.Value });
                return addressHome;
            }
        }

        //FK Temp address
        public int? AddressTempId { get; set; }
        public string AddressTempString { get; set; }
        private Address addressTemp = null;
        public Address AddressTemp {
            get {
                if (!this.AddressTempId.HasValue) return null;

                if (addressTemp != null) return addressTemp;
                addressTemp = Storage<Address>.ReadFirst(new Address.ReadById { AddressId = this.AddressTempId.Value });
                return addressTemp;
            }
        }

        // Format Display
        public string Display { get; set; }
        public void MakeDisplay() {
            StringBuilder sb = new StringBuilder();
            if (!String.IsNullOrEmpty(Title)) sb.AppendFormat("{0}. ", Title);
            if (!String.IsNullOrEmpty(FirstName)) sb.AppendFormat("{0} ", FirstName);
            if (!String.IsNullOrEmpty(LastName)) sb.Append(LastName);
            Display = sb.ToString();
        }

    }
}
