using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Linq;
using System.Linq.Dynamic;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters;
using System.Xml.Linq;
using System.Web.UI;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using System.Collections;

namespace Eurona.Common.Controls {
    public class Tweet11 {
        public string Id { get; set; }
        public string Text { get; set; }
        public Author11 Author { get; set; }

        public static List<Tweet11> GetTwetts(TweetStream11 ts) {
            if (ts.Tweets == null || ts.Tweets.Count == 0) ts.Refresh();
            return ts.Tweets;
        }
    }

    public class Author11 {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class TweetStream11 {
        private string m_refreshUri;
        List<Tweet11> m_tweets;
        public TweetStream11(string queryUri) {
            m_refreshUri = queryUri;
            m_tweets = new List<Tweet11>();
        }
        public List<Tweet11> Tweets {
            get {
                return m_tweets;
            }
        }

        public void Refresh() {
            try {
                string json = DownloadJSON(m_refreshUri);
                if (json == null) return;
                m_tweets = new List<Tweet11>();
                var jsonTweets = new JavaScriptSerializer().DeserializeObject(json) as ICollection;
                foreach (var tweetObject in jsonTweets) {
                    Dictionary<string, object> val = (Dictionary<string, object>)tweetObject;

                    Tweet11 tweet = new Tweet11();
                    tweet.Id = val["id"].ToString();
                    tweet.Text = val["text"].ToString();
                    tweet.Author = new Author11();
                    //tweet.Author.Name = val["name"].ToString();
                    //tweet.Author.Url = val["url"].ToString();
                    m_tweets.Add(tweet);
                }
            } catch (Exception ex) {
                CMS.EvenLog.WritoToEventLog(ex);
                m_tweets = new List<Tweet11>();
            }

        }

        private string DownloadJSON(string Url) {
            try {
                string[] urls = Url.Split('=');
                if (urls.Length != 2) return null;

                // oauth application keys
                var oauth_token = "1224310933-MvVDlmt7LIHQJA7ToHGtB8DiagEU1vxWtgzLgYk";
                var oauth_token_secret = "ay9xNTpmp9l8iIA46wobSJKZ1nrk9dJ25IFCA9dbk";
                var oauth_consumer_key = "lhTcvaH2sgGusH7U548Wbw";
                var oauth_consumer_secret = "nByrVjckeWs3XBquLEH7n5wazE6vJGE7zetEMmTMpw";

                // oauth implementation details
                var oauth_version = "1.0";
                var oauth_signature_method = "HMAC-SHA1";

                // unique request details
                var oauth_nonce = Convert.ToBase64String(
                    new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
                var timeSpan = DateTime.UtcNow
                    - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                var oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();

                // message api details
                //var status = "Updating status via REST API if this works";
                var resource_url = "https://api.twitter.com/1.1/statuses/user_timeline.json";
                var screen_name = urls[1];// "euronabycerny";//"updateme";
                // create oauth signature
                var baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
                                "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}&screen_name={6}";

                var baseString = string.Format(baseFormat,
                                            oauth_consumer_key,
                                            oauth_nonce,
                                            oauth_signature_method,
                                            oauth_timestamp,
                                            oauth_token,
                                            oauth_version,
                                             Uri.EscapeDataString(screen_name)
                                            );

                baseString = string.Concat("GET&", Uri.EscapeDataString(resource_url), "&", Uri.EscapeDataString(baseString));

                var compositeKey = string.Concat(Uri.EscapeDataString(oauth_consumer_secret),
                                        "&", Uri.EscapeDataString(oauth_token_secret));

                string oauth_signature;
                using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey))) {
                    oauth_signature = Convert.ToBase64String(
                        hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
                }

                // create the request header
                var headerFormat = "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
                                   "oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
                                   "oauth_token=\"{4}\", oauth_signature=\"{5}\", " +
                                   "oauth_version=\"{6}\"";

                var authHeader = string.Format(headerFormat,
                                        Uri.EscapeDataString(oauth_nonce),
                                        Uri.EscapeDataString(oauth_signature_method),
                                        Uri.EscapeDataString(oauth_timestamp),
                                        Uri.EscapeDataString(oauth_consumer_key),
                                        Uri.EscapeDataString(oauth_token),
                                        Uri.EscapeDataString(oauth_signature),
                                        Uri.EscapeDataString(oauth_version)
                                );


                // make the request

                ServicePointManager.Expect100Continue = false;

                var postBody = "screen_name=" + Uri.EscapeDataString(screen_name);//
                resource_url += "?" + postBody;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resource_url);
                request.Headers.Add("Authorization", authHeader);
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";


                WebResponse response = request.GetResponse();
                string responseData = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return responseData;
            } catch// ( Exception ex ) 
            {
                //CMS.EvenLog.WritoToEventLog( ex );
                return null;
            }
        }
    }

    public class TwitterControl11 : CMS.Controls.CmsControl {
        protected override void CreateChildControls() {
            base.CreateChildControls();

            string url = ConfigValue("EURONA:TwitterUrl");
            if (System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower() == "pl")
                url = ConfigValue("EURONA:TwitterUrl:pl");
            if (System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower() == "sk")
                url = ConfigValue("EURONA:TwitterUrl:sk");

            if (this.Session[url] != null && this.Session[url].ToString().Length != 0) {
                this.Controls.Add(new LiteralControl(this.Session[url].ToString()));
                return;
            }

            TweetStream11 ts = new TweetStream11(url);
            List<Tweet11> list = Tweet11.GetTwetts(ts);

            StringBuilder sbTweetString = new StringBuilder();

            foreach (Tweet11 t in list) {
                sbTweetString.Append("&nbsp;&nbsp;&laquo;" + t.Text + "&laquo;&nbsp;&nbsp;");
            }

            this.Session[url] = sbTweetString.ToString();
            this.Controls.Add(new LiteralControl(sbTweetString.ToString()));

        }
    }
    public class TwitterControlAdvisor11 : CMS.Controls.CmsControl {
        protected override void CreateChildControls() {
            base.CreateChildControls();

            string url = ConfigValue("EURONA:TwitterKacelarUrl");
            if (System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower() == "pl")
                url = ConfigValue("EURONA:TwitterKacelarUrl:pl");
            if (System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower() == "sk")
                url = ConfigValue("EURONA:TwitterKacelarUrl:sk");

            if (this.Session[url] != null && this.Session[url].ToString().Length != 0) {
                this.Controls.Add(new LiteralControl(this.Session[url].ToString()));
                return;
            }

            TweetStream11 ts = new TweetStream11(url);
            List<Tweet11> list = Tweet11.GetTwetts(ts);

            StringBuilder sbTweetString = new StringBuilder();

            foreach (Tweet11 t in list) {
                sbTweetString.Append("&nbsp;&nbsp;&laquo;" + t.Text + "&laquo;&nbsp;&nbsp;");
            }

            this.Session[url] = sbTweetString.ToString();
            this.Controls.Add(new LiteralControl(sbTweetString.ToString()));
        }
    }

    public class CernyForLifeTwitterControl11 : CMS.Controls.CmsControl {
        protected override void CreateChildControls() {
            base.CreateChildControls();

            string url = ConfigValue("CERNYFORLIFE:TwitterUrl");
            if (System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower() == "pl")
                url = ConfigValue("CERNYFORLIFE:TwitterUrl:pl");
            if (System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower() == "sk")
                url = ConfigValue("CERNYFORLIFE:TwitterUrl:sk");

            if (this.Session[url] != null && this.Session[url].ToString().Length != 0) {
                this.Controls.Add(new LiteralControl(this.Session[url].ToString()));
                return;
            }

            TweetStream11 ts = new TweetStream11(url);
            List<Tweet11> list = Tweet11.GetTwetts(ts);

            StringBuilder sbTweetString = new StringBuilder();

            foreach (Tweet11 t in list) {
                sbTweetString.Append("&nbsp;&nbsp;&laquo;" + t.Text + "&laquo;&nbsp;&nbsp;");
            }

            this.Session[url] = sbTweetString.ToString();
            this.Controls.Add(new LiteralControl(sbTweetString.ToString()));

        }
    }
    public class CernyForLifeTwitterControlAdvisor11 : CMS.Controls.CmsControl {
        protected override void CreateChildControls() {
            base.CreateChildControls();

            string url = ConfigValue("CERNYFORLIFE:TwitterKacelarUrl");
            if (System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower() == "pl")
                url = ConfigValue("CERNYFORLIFE:TwitterKacelarUrl:pl");
            if (System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower() == "sk")
                url = ConfigValue("CERNYFORLIFE:TwitterKacelarUrl:sk");

            if (this.Session[url] != null && this.Session[url].ToString().Length != 0) {
                this.Controls.Add(new LiteralControl(this.Session[url].ToString()));
                return;
            }

            TweetStream11 ts = new TweetStream11(url);
            List<Tweet11> list = Tweet11.GetTwetts(ts);

            StringBuilder sbTweetString = new StringBuilder();

            foreach (Tweet11 t in list) {
                sbTweetString.Append("&nbsp;&nbsp;&laquo;" + t.Text + "&laquo;&nbsp;&nbsp;");
            }

            this.Session[url] = sbTweetString.ToString();
            this.Controls.Add(new LiteralControl(sbTweetString.ToString()));
        }
    }
}


