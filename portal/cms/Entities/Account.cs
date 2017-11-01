using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities {
    public class Account : Entity {
        public class ReadById {
            public int AccountId { get; set; }
        }
        public class ReadByInstance {
            public int InstanceId { get; set; }
        }
        public class ReadByLogin {
            public string Login { get; set; }
        }

        public class ReadByEmail {
            public string Email { get; set; }
        }

        public class Verify {
            public Account Account { get; set; }
            public string VerifyCode { get; set; }
            public bool Result { get; set; }
        }

        public int InstanceId { get; set; }
        public string Locale { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Enabled { get; set; }

        public bool Verified { get; set; }
        public string VerifyCode { get; set; }
        public decimal Credit { get; set; }

        public bool Authenticate(string password) {
            return String.Compare(this.Password, password, true) == 0;
        }

        private List<Role> roles = new List<Role>();
        public List<Role> Roles {
            get { return roles; }
            set { roles = value; }
        }

        public string[] RoleArray {
            get {
                return roles.ConvertAll<string>(r => r.Name).ToArray();
            }
            set {
                roles.Clear();
                if (value != null && value.Length > 0)
                    foreach (String r in value)
                        roles.Add(new Role { Name = r });
            }
        }

        public string RoleString {
            get {
                StringBuilder sb = new StringBuilder();
                roles.ForEach(r => sb.AppendFormat("{0};", r.Name));
                return sb.ToString();
            }
            set {
                RoleArray = value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            }
        }
        /// <summary>
        /// Property na zobraznie rolestringu napr. v GridView
        /// </summary>
        public string RoleStringDisplay {
            get {
                return RoleString.Replace(";", "; ");
            }
        }

        public bool IsInRole(string role) {
            return this.RoleString.Contains(role);
        }

        public void AddToRoles(params string[] roles) {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.RoleString);

            foreach (string role in roles)
                sb.AppendFormat("{0};", role);

            this.RoleString = sb.ToString();
        }

        public void RemoveFromRole(string role) {
            StringBuilder sb = new StringBuilder();
            foreach (Role r in roles) {
                if (r.Name != role)
                    sb.AppendFormat("{0};", r.Name);
            }
            this.RoleString = sb.ToString();
        }

        // Format Display
        public string Display {
            get { return string.Format("{0} ({1})", this.Login, this.Email); }
        }
    }
}
