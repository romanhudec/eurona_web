using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using SettingsEntity = Eurona.Common.DAL.Entities.Settings;
using AccountEntity = Eurona.DAL.Entities.Account;
using SingleUserCookieLinkActivityEntity = Eurona.DAL.Entities.SingleUserCookieLinkActivity;
using Eurona.Common.DAL.Entities;

namespace Eurona {
    public static class CookiesUtils {
        public static string COOKIE_SINGLEUSERCOOKIESLINK = "mothiva.cms.Eurona.SingleUserCookieLink";
        public static string COOKIE_SINGLEUSERCOOKIESLINK_QUERY_PARAMETER = "sucl";

        public static HttpCookie GetCookie(Page page, string name) {
            if (page.Request.Cookies.Count == 0 ) return null;
            HttpCookie cookie = page.Request.Cookies.Get(name);
            return cookie;
        }

        public static void CreateCookie(Page page, string name, string value, int expirationDurationDays) {
            HttpCookie StudentCookies = new HttpCookie(name);
            StudentCookies.Value = value;
            StudentCookies.Expires = DateTime.Now.AddDays(expirationDurationDays);
            page.Response.Cookies.Add(StudentCookies);
        }

        public static void ProcessSingleUserCookiesLink(Page page) {
            string cookieParam = page.Request.QueryString.Get(COOKIE_SINGLEUSERCOOKIESLINK_QUERY_PARAMETER);
            if (string.IsNullOrEmpty(cookieParam)) return;

            string strValue = CMS.Utilities.Cryptographer.Decrypt(cookieParam);
            int accountId = 0;
            Int32.TryParse(strValue, out accountId);

            if (accountId == 0) return;

            HttpCookie cookie = GetCookie(page, COOKIE_SINGLEUSERCOOKIESLINK);
            if (cookie == null) {
                int expirationDays = 30;
                SettingsEntity settingsAccountLinkCookiesLimit = Storage<SettingsEntity>.ReadFirst(new SettingsEntity.ReadByCode { Code = "ACCOUNT_LINK_COOKIES_LIMIT" });
                if( settingsAccountLinkCookiesLimit != null ){
                    Int32.TryParse(settingsAccountLinkCookiesLimit.Value, out expirationDays );
                }
                CreateCookie(page, COOKIE_SINGLEUSERCOOKIESLINK, accountId.ToString(), expirationDays);
            }

            SingleUserCookieLinkActivityEntity activity = new SingleUserCookieLinkActivityEntity();
            activity.CookieAccountId = accountId;
            activity.Url = page.Request.Url.AbsoluteUri;
            activity.UrlTimestamp = DateTime.Now;
            activity.IPAddress = Utilities.GetUserIP(page.Request);
            Storage<SingleUserCookieLinkActivityEntity>.Create(activity);
        }

        public static AccountEntity GetSingleUserCookiesLink(Page page) {
            HttpCookie singleUserCookieLink = CookiesUtils.GetCookie(page, CookiesUtils.COOKIE_SINGLEUSERCOOKIESLINK);
            if (singleUserCookieLink == null) return null;
            string strValue = singleUserCookieLink.Value;
            int accountId = 0;
            Int32.TryParse(strValue, out accountId);
            if (accountId == 0) return null;
            AccountEntity account = Storage<AccountEntity>.ReadFirst(new AccountEntity.ReadById { AccountId = accountId });
            return account;
        }

        public static bool IsSingleUserCoocieLinkEnabled() {
            SettingsEntity settingsAccountLinkCookiesEnabled = Storage<SettingsEntity>.ReadFirst(new SettingsEntity.ReadByCode { Code = "ACCOUNT_LINK_COOKIES_ENABLED" });
            bool enabled = true;
            Boolean.TryParse(settingsAccountLinkCookiesEnabled.Value, out enabled);
            return enabled;
        }
    }
}