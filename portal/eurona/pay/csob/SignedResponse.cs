using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Eurona.pay.csob {
    public abstract class SignedResponse {
        public virtual string getData2VerifyResponse() {
            throw new NotImplementedException("Please implement body getData2VerifyResponse!");
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