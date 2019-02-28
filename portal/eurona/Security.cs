using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using Eurona.DAL.Entities;

namespace Eurona {
    public static class Security {
        public static string COOKIE_ROLES = "mothiva.cms.Eurona";

        public static void RedirectToLoginPage(HttpResponse response) {
            FormsAuthentication.RedirectToLoginPage();
            response.End();
        }

        private static void RedirectFromLoginPage(Account account) {
            FormsAuthentication.RedirectFromLoginPage(account.Login, true);
        }

        public static void Login(Account account) {
            Login(account, true);
        }

        public static void Login(Account account, bool redirect) {
            HttpContext context = HttpContext.Current;
            HttpSessionState session = context.Session;
            HttpResponse response = context.Response;
            session["account"] = account;
            session["accountId"] = account.Id;
            FormsAuthentication.SetAuthCookie(account.Login, true);

            /*
            //PasswordPolicy
            //Zmena hesla kazdych 12 mesiacov
            if (account.PasswordChanged == null || account.PasswordChanged.Value.AddMonths(12) <= DateTime.Now) {
                account.MustChangeAccountPassword = true;
                Storage<Account>.Update(account);
            }
             * */

            if (redirect) RedirectFromLoginPage(account);
        }

        public static void Logout() {
            HttpContext context = HttpContext.Current;
            HttpSessionState session = context.Session;
            HttpResponse response = context.Response;
            if (session != null) session.Clear();
            FormsAuthentication.SignOut();
            response.Cookies[COOKIE_ROLES].Value = "";
            FormsAuthentication.RedirectToLoginPage();
        }

        public static void LogoutWithoutRedirect() {
            HttpContext context = HttpContext.Current;
            HttpSessionState session = context.Session;
            HttpResponse response = context.Response;
            if (session != null) session.Clear();
            FormsAuthentication.SignOut();
            response.Cookies[COOKIE_ROLES].Value = "";
        }

        public static bool IsLogged(bool redirect) {
            HttpContext context = HttpContext.Current;
            HttpResponse response = context.Response;
            HttpSessionState session = context.Session;
            if (session == null) return false;
            Account account = session["account"] as Account;
            if (account == null && context.Request.IsAuthenticated) {
                string login = context.User.Identity.Name;
                account = Storage<Account>.ReadFirst(new Account.ReadByLogin { Login = login });
                if (account != null) {
                    session["account"] = account;
                    session["accountId"] = account.Id;
                }
            }
            if (account == null) {
                if (redirect) RedirectToLoginPage(response);
                return false;
            }

            return true;
        }

        public static Account Account {
            get {
                HttpContext context = HttpContext.Current;
                HttpSessionState session = context.Session;
                HttpResponse response = context.Response;
                if (session == null) {
                    RedirectToLoginPage(response);
                    return null;
                }
                Account account = null;
                if (session["account"] == null || session["accountId"] == null) {
                    //account = AccountODS.GetAccountByLogin(context.User.Identity.Name);
                    account = Storage<Account>.ReadFirst(new Account.ReadByLogin { Login = context.User.Identity.Name });
                    if (account == null) {
                        RedirectToLoginPage(response);
                        return null;
                    }
                    session["account"] = account;
                    session["accountId"] = account.Id;
                }
                if (session["account"] == null || session["accountId"] == null) {
                    RedirectToLoginPage(response);
                    return null;
                }
                account = session["account"] as Account;
                if (account == null && context.Request.IsAuthenticated) {
                    string login = context.User.Identity.Name;
                    account = Storage<Account>.ReadFirst(new Account.ReadByLogin { Login = login });
                    if (account != null) {
                        session["account"] = account;
                        session["accountId"] = account.Id;
                    }
                }
                if (account == null) {
                    RedirectToLoginPage(response);
                    return null;
                }
                return account;
            }
        }

        public static bool IsInRole(string role) {
            if (Security.IsLogged(false) == false) return false;

            Account account = Account;
            if (account == null) return false;
            //HttpContext context = HttpContext.Current;
            //return context.User.IsInRole( role );
            return account.IsInRole(role);
        }

        public static void Error(string message) {
            HttpContext context = HttpContext.Current;
            HttpResponse response = context.Response;
            response.Clear();
            response.Write(message);
            response.End();
        }

        /// <summary>
        /// Update login time for logged user
        /// </summary>
        public static void UpdateLoginTime() {
            if (!Security.IsLogged(false)) return;
            Account account = Security.Account;
            if (account == null) return;

            Eurona.Common.DAL.Entities.LoggedAccount la = new Common.DAL.Entities.LoggedAccount();
            la.InstanceId = account.InstanceId;
            la.AccountId = account.Id;
            Storage<Eurona.Common.DAL.Entities.LoggedAccount>.Create(la);
        }
    }

}
