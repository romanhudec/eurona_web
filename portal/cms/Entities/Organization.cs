using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities {
    [Serializable]
    public class Organization : Entity {
        public class ReadById {
            public int OrganizationId { get; set; }
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

        public string Id1 { get; set; }
        public string Id2 { get; set; }
        public string Id3 { get; set; }

        public string Name { get; set; }
        public string Notes { get; set; }
        public string Web { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string ContactMobile { get; set; }

        //FK ContactPerson
        public int? ContactPersonId { get; set; }
        public string ContactPersonString { get; set; }
        private Person contactPerson = null;
        public Person ContactPerson {
            get {
                if (!this.ContactPersonId.HasValue) return null;

                if (contactPerson != null) return contactPerson;
                contactPerson = Storage<Person>.ReadFirst(new Person.ReadById { PersonId = this.ContactPersonId.Value });
                return contactPerson;
            }
        }

        //FK BankContact
        public int? BankContactId { get; set; }
        public string BankContactString {
            get {
                if (this.BankContact == null) return string.Empty;
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(this.BankContact.AccountNumber))
                    sb.Append(this.BankContact.AccountNumber);

                if (!string.IsNullOrEmpty(this.BankContact.BankCode) && sb.Length != 0)
                    sb.Append("/" + this.BankContact.AccountNumber);

                return sb.ToString();
            }
        }
        private BankContact bankContact = null;
        public BankContact BankContact {
            get {
                if (!this.BankContactId.HasValue) return null;

                if (bankContact != null) return bankContact;
                bankContact = Storage<BankContact>.ReadFirst(new BankContact.ReadById { BankContactId = this.BankContactId.Value });
                return bankContact;
            }
        }

        //FK RegisteredAddress
        public int? RegisteredAddressId { get; set; }
        public string RegisteredAddressString { get; set; }
        private Address registeredAddress = null;
        public Address RegisteredAddress {
            get {
                if (!this.RegisteredAddressId.HasValue) return null;

                if (registeredAddress != null) return registeredAddress;
                registeredAddress = Storage<Address>.ReadFirst(new Address.ReadById { AddressId = this.RegisteredAddressId.Value });
                return registeredAddress;
            }
        }

        //FK CorrespondenceAddress
        public int? CorrespondenceAddressId { get; set; }
        public string CorrespondenceAddressString { get; set; }
        private Address correspondenceAddress = null;
        public Address CorrespondenceAddress {
            get {
                if (!this.CorrespondenceAddressId.HasValue) return null;

                if (correspondenceAddress != null) return correspondenceAddress;
                correspondenceAddress = Storage<Address>.ReadFirst(new Address.ReadById { AddressId = this.CorrespondenceAddressId.Value });
                return correspondenceAddress;
            }
        }

        //FK CorrespondenceAddress
        public int? InvoicingAddressId { get; set; }
        public string InvoicingAddressString { get; set; }
        private Address invoicingAddress = null;
        public Address InvoicingAddress {
            get {
                if (!this.InvoicingAddressId.HasValue) return null;

                if (invoicingAddress != null) return invoicingAddress;
                invoicingAddress = Storage<Address>.ReadFirst(new Address.ReadById { AddressId = this.InvoicingAddressId.Value });
                return invoicingAddress;
            }
        }

    }
}

