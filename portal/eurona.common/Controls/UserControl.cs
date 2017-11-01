using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eurona.Common.Controls
{
    public class UserControl : System.Web.UI.UserControl
    {
        /// <summary>
        /// Vráti stránku, z ktorej bola otvorená aktuálna stránka.
        /// </summary>
        private string returnUrl = null;
        public string ReturnUrl
        {
            get
            {
                if (returnUrl != null) return returnUrl;

                if (this.Request["ReturnUrl"] == null)
                    return null;

                returnUrl = this.Request["ReturnUrl"];
                returnUrl = returnUrl.Replace(",", "&ReturnUrl=");
                string query = Server.UrlDecode(this.Request.QueryString.ToString());
                int index = query.IndexOf(returnUrl);
                if (index == -1) return returnUrl;

                returnUrl = query.Substring(index);
                return returnUrl;
            }

        }

        /// <summary>
        /// Vráti retazec, ktorý sa použije ako parameter v query.
        /// </summary>
        protected string BuildReturnUrlQueryParam()
        {
            //return string.Format( "ReturnUrl={0}", this.Request.RawUrl );

            CMS.Utilities.AliasUtilities aliasUtils = new CMS.Utilities.AliasUtilities();
            return string.Format("ReturnUrl={0}", aliasUtils.Resolve(this.Request.RawUrl, this.Page));
        }

        protected string ConfigValue(string key)
        {
            return CMS.Utilities.ConfigUtilities.ConfigValue(key, Page);
        }

        protected string ConfigValue(string key, bool resolve)
        {
            if (resolve)
                return CMS.Utilities.ConfigUtilities.ConfigValue(key, Page);
            else
                return CMS.Utilities.ConfigUtilities.ConfigValue(key);
        }
    }
}
