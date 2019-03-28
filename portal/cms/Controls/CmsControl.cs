using System.Web.UI;
using System;
using System.Web;
using System.Text;
using System.Configuration;
using System.Web.UI.WebControls;
using CMS.Utilities;

namespace CMS.Controls
{
    public class CmsControl : UserControl
    {
        public string CssClass { get; set; }
        public Unit Width { get; set; }
        public Unit Height { get; set; }

        protected T GetState<T>(string key, T @default)
        {
            object value = ViewState[key];
            return value != null ? (T)value : @default;
        }

        protected T GetState<T>(string key)
        {
            return GetState<T>(key, default(T));
        }

        protected void SetState<T>(string key, T value)
        {
            ViewState[key] = value;
        }

        protected T GetSession<T>(string key, T @default)
        {
            object value = Session[key];
            return value != null ? (T)value : @default;
        }

        protected T GetSession<T>(string key)
        {
            return GetSession<T>(key, default(T));
        }

        protected void SetSession<T>(string key, T value)
        {
            Session[key] = value;
        }

        public bool IsEditing
        {
            get { return GetState<bool>("IsEditing"); }
            set { SetState<bool>("IsEditing", value); }
        }

        protected bool IsPostBackControl(string control)
        {
            string tmp = this.ClientID + this.ClientIDSeparator + control;
            tmp = tmp.Replace('_', '$');
            return Page.Request.Form[tmp] != null;
        }

        protected bool IsPostBackControl(Control control)
        {
            return IsPostBackControl(control.ID);
        }

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
        public string BuildReturnUrlQueryParam()
        {
            //return string.Format( "ReturnUrl={0}", this.Request.RawUrl );

            AliasUtilities aliasUtils = new AliasUtilities();
            return string.Format("ReturnUrl={0}", aliasUtils.Resolve(this.Request.RawUrl, this.Page));
        }

        protected string ConfigValue(string key)
        {
            return Utilities.ConfigUtilities.ConfigValue(key, Page);
        }

        protected string ConfigValue(string key, bool resolve)
        {
            if (resolve)
                return Utilities.ConfigUtilities.ConfigValue(key, Page);
            else
                return Utilities.ConfigUtilities.ConfigValue(key);
        }
        #region UI Helper methods
        /// <summary>
        /// Vytvori validacny control.
        /// </summary>
        protected RequiredFieldValidator CreateRequiredFieldValidatorControl(string controlToValidateId)
        {
            return CreateRequiredFieldValidatorControl(controlToValidateId, "*");
        }
        protected RequiredFieldValidator CreateRequiredFieldValidatorControl(string controlToValidateId, string message) {
            RequiredFieldValidator rv = new RequiredFieldValidator();
            rv.ID = string.Format("rv_{0}", controlToValidateId);
            rv.InitialValue = "";
            rv.ControlToValidate = controlToValidateId;
            rv.Display = ValidatorDisplay.Static;
            rv.ErrorMessage = message;
            rv.ForeColor = System.Drawing.Color.Empty;
            rv.CssClass = "ms-formvalidation";
            rv.SetFocusOnError = true;
            rv.EnableClientScript = true;
            //rv.Controls.Add( new LiteralControl( string.Format( "<img src='{0}' alt='chyba'>", Page.ResolveUrl( "~/images/Cancel.png" ) ) ) );
            return rv;
        }
        protected RegularExpressionValidator CreatePasswordValidatorControl(string controlToValidateId) {
            return CreatePasswordValidatorControl(controlToValidateId, "!");
        }
        protected RegularExpressionValidator CreatePasswordValidatorControl(string controlToValidateId, String message) {

            //PasswordPolicy
            RegularExpressionValidator rv = new RegularExpressionValidator();
            rv.ID = string.Format("rve_{0}", controlToValidateId);
            rv.ControlToValidate = controlToValidateId;
            rv.Display = ValidatorDisplay.Static;
            rv.ErrorMessage = message;
            rv.ForeColor = System.Drawing.Color.Empty;
            rv.CssClass = "ms-formvalidation";
            rv.SetFocusOnError = true;
            rv.EnableClientScript = true;
            rv.ValidationExpression = @"^(?=.*[A-Z])(?=.*\d)(?!.*(.)\1\1)[a-zA-Z0-9@]{6,}$";
            return rv;
        }

        protected RegularExpressionValidator CreateEmailValidatorControl(string controlToValidateId)
        {
            RegularExpressionValidator rv = new RegularExpressionValidator();
            rv.ID = string.Format("rve_{0}", controlToValidateId);
            rv.ControlToValidate = controlToValidateId;
            rv.Display = ValidatorDisplay.Static;
            rv.ErrorMessage = "!";
            rv.ForeColor = System.Drawing.Color.Empty;
            rv.CssClass = "ms-formvalidation";
            rv.SetFocusOnError = true;
            rv.EnableClientScript = true;
            rv.ValidationExpression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            return rv;
        }

        protected RegularExpressionValidator CreateNumberValidatorControl(string controlToValidateId)
        {
            RegularExpressionValidator rv = new RegularExpressionValidator();
            rv.ID = string.Format("rvn_{0}", controlToValidateId);
            rv.ControlToValidate = controlToValidateId;
            rv.Display = ValidatorDisplay.Static;
            rv.ErrorMessage = "!";
            rv.ForeColor = System.Drawing.Color.Empty;
            rv.CssClass = "ms-formvalidation";
            rv.SetFocusOnError = true;
            rv.EnableClientScript = true;
            rv.ValidationExpression = @"^[-]?[0-9]+$";
            return rv;
        }

        protected RegularExpressionValidator CreatePhoneValidatorControl(string controlToValidateId)
        {
            RegularExpressionValidator rv = new RegularExpressionValidator();
            rv.ID = string.Format("rpn_{0}", controlToValidateId);
            rv.ControlToValidate = controlToValidateId;
            rv.Display = ValidatorDisplay.Static;
            rv.ErrorMessage = "!";
            rv.ForeColor = System.Drawing.Color.Empty;
            rv.CssClass = "ms-formvalidation";
            rv.SetFocusOnError = true;
            rv.EnableClientScript = true;
            rv.ValidationExpression = @"^[0-9\s]{0,15}$";
            return rv;
        }

        protected RangeValidator CreateDoubleValidatorControl(string controlToValidateId)
        {
            RangeValidator rv = new RangeValidator();
            rv.ID = string.Format("rvr_{0}", controlToValidateId);
            rv.ControlToValidate = controlToValidateId;
            rv.Display = ValidatorDisplay.Static;
            rv.ErrorMessage = "!";
            rv.ForeColor = System.Drawing.Color.Empty;
            rv.CssClass = "ms-formvalidation";
            rv.SetFocusOnError = true;
            rv.EnableClientScript = true;
            rv.MinimumValue = "-99999999";
            rv.MaximumValue = "99999999";
            rv.Type = ValidationDataType.Double;
            return rv;
        }

        protected RegularExpressionValidator CreateTimeValidatorControl(string controlToValidateId)
        {
            RegularExpressionValidator rv = new RegularExpressionValidator();
            rv.ID = string.Format("rvt_{0}", controlToValidateId);
            rv.ControlToValidate = controlToValidateId;
            rv.Display = ValidatorDisplay.Static;
            rv.ErrorMessage = "!";
            rv.ForeColor = System.Drawing.Color.Empty;
            rv.CssClass = "ms-formvalidation";
            rv.SetFocusOnError = true;
            rv.EnableClientScript = true;
            rv.ValidationExpression = @"^((0?[1-9]|1[012])(:[0-5]\d){0,2})$|^([01]\d|2[0-3])(:[0-5]\d){0,2}$";
            return rv;
        }

        protected RegularExpressionValidator CreateRegularExpressionValidatorControl(RegExpValidator rex)
        {
            RegularExpressionValidator rv = new RegularExpressionValidator();
            rv.ID = string.Format("rvt_{0}", rex.ControlToValidate);
            rv.ControlToValidate = rex.ControlToValidate;
            rv.Display = ValidatorDisplay.Static;
            rv.ErrorMessage = rex.ErrorMessage;
            rv.ForeColor = System.Drawing.Color.Empty;
            rv.CssClass = rex.CssClass;
            rv.SetFocusOnError = rex.SetFocusOnError;
            rv.EnableClientScript = rex.EnableClientScript;
            rv.ValidationExpression = rex.ValidationExpression;
            return rv;
        }
        #endregion

        [Serializable]
        public class RegExpValidator
        {
            public string ControlToValidate { get; set; }
            public string ErrorMessage { get; set; }
            public string CssClass { get; set; }
            public bool SetFocusOnError { get; set; }
            public bool EnableClientScript { get; set; }
            public string ValidationExpression { get; set; }
        }
    }
}
