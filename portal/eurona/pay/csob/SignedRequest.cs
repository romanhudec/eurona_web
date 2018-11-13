using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eurona.pay.csob {
    public abstract class SignedRequest {
       public virtual string getData2Sign() {
           throw new NotImplementedException("Please implement getData2Sign in your class!");
        }

        protected string getStringValue(string value) {
            if (value == null) return "";
            return value;
        }
        protected string getIntValue(int value) {
            return value.ToString();
        }
        protected string getIntValue(Nullable<Int32> value) {
            if (value == null) return "";
            return value.Value.ToString();
        }
        protected string getBoolValue(bool value) {
            return value ? "true" : "false";
        }
    }
}