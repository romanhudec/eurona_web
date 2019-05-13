using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Eurona.DAL.Entities {
    [Serializable]
    public class Account : CMS.Entities.Account {

        public enum EmailVerifyStatusCode :int  {
            NONE = 1,
            EMAIL_SEND = 2,
            DATA_VALIDATED = 3,
            VERIFIED = 0
        }

        public string InstanceName {
            get {
                switch (this.InstanceId) {
                    case 1:
                        return "Eurona";
                    case 2:
                        return "Intenza";
                    default:
                        return string.Empty;
                }
            }
        }

        public class ReadByLoginAndInstance {
            public string Login { get; set; }
            public int InstanceId { get; set; }
        }

        public int? TVD_Id { get; set; }
        public bool CanAccessIntensa { get; set; }
        public DateTime Created { get; set; }
        public bool MustChangeAccountPassword { get; set; }
        public DateTime? PasswordChanged { get; set; }

        private AccountExt accountExtension = null;
        public AccountExt AccountExtension {
            get {
                if (accountExtension != null) return accountExtension;
                accountExtension = Storage<AccountExt>.ReadFirst(new AccountExt.ReadByAccount { AccountId = this.Id });
                return accountExtension;
            }
            set { this.accountExtension = value; }
        }

        public bool SingleUserCookieLinkEnabled { get; set; }

        //Email verification
        public string EmailVerifyCode { get; set; }
        public string EmailToVerify { get; set; }
        public int? EmailVerifyStatus { get; set; }
        public string LoginBeforeVerify{ get; set; }
        public string EmailBeforeVerify { get; set; }

        public class ReadByEmailVerifyCode {
            public string EmailVerifyCode { get; set; }
        }

        public class ReadByEmailToVerify {
            public string EmailToVerify { get; set; }
            public bool OnlyEmailVerified { get; set; }
        }
    }
}
