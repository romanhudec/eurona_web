using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace QueueIT.KnownUserV3.SDK {
    class HttpContextProvider : IHttpContextProvider {

        public HttpContextProvider() {
            this.HttpRequest = new HttpRequest();
            this.HttpResponse = new HttpResponse();
        }

        public IHttpRequest HttpRequest { get; internal set; }
        public IHttpResponse HttpResponse { get; internal set; }

        public static IHttpContextProvider Instance = new HttpContextProvider();
    }

    class HttpRequest : IHttpRequest {

        public HttpRequest() {
            this.UserAgent = HttpContext.Current.Request.UserAgent;
            this.Headers = HttpContext.Current.Request.Headers;
            this.Url = HttpContext.Current.Request.Url;
            this.UserHostAddress = HttpContext.Current.Request.UserHostAddress;
        }
        public string UserAgent { get; internal set; }
        public NameValueCollection Headers { get; internal set; }
        public Uri Url { get; internal set; }
        public string UserHostAddress { get; internal set; }

        public string GetCookieValue(string cookieKey) {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieKey];
            string cookieValue = null;
            if (cookie != null) cookieValue = cookie.Value;
            if (cookieValue == null)
                return null;
            return HttpUtility.UrlDecode(cookieValue);
        }
    }

    class HttpResponse : IHttpResponse {
        public void SetCookie(string cookieName, string cookieValue, string domain, DateTime expiration) {
            if (HttpContext.Current.Response.
                Cookies.AllKeys.Any(key => key == KnownUser.QueueITDebugKey)) {
                HttpContext.Current.Response.Cookies.Remove(KnownUser.QueueITDebugKey);
            }
            var cookie = new HttpCookie(cookieName, Uri.EscapeDataString(cookieValue));
            if (!string.IsNullOrEmpty(domain)) {
                cookie.Domain = domain;
            }
            cookie.HttpOnly = false;
            cookie.Expires = expiration;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}
