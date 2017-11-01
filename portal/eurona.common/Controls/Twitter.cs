using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Linq;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters;
using System.Xml.Linq;
using System.Web.UI;

namespace Eurona.Common.Controls
{
    public class Tweet
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public Author Author { get; set; }

        public static List<Tweet> GetTwetts(TweetStream ts)
        {
            if (ts.Tweets == null || ts.Tweets.Count == 0) ts.Refresh();
            return ts.Tweets;
        }
    }

    public class Author
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class TweetStream
    {
        private string m_refreshUri;
        List<Tweet> m_tweets;
        public TweetStream(string queryUri)
        {
            m_refreshUri = queryUri;
            m_tweets = new List<Tweet>();
        }
        public List<Tweet> Tweets
        {
            get
            {
                return m_tweets;
            }
        }

        public void Refresh()
        {
            try
            {
                //XDocument feed = XDocument.Load( m_refreshUri );
                string xmlData = DownloadXML(m_refreshUri);
                if (xmlData == null)
                {
                    m_tweets = new List<Tweet>();
                    return;
                }
                XDocument feed = XDocument.Parse(xmlData);

                if (feed != null)
                {
                    m_tweets = (from tweet in feed.Descendants("status")
                                select new Tweet
                                {
                                    Text = (string)tweet.Element("text"),
                                    //Published = DateTime.Parse( (string)tweet.Element( "created_at" ) ),
                                    Id = (string)tweet.Element("id"),
                                    Author = (from author in tweet.Descendants("user")
                                              select new Author
                                              {
                                                  Name = (string)author.Element("name"),
                                                  Url = (string)author.Element("url"),
                                              }).First(),
                                }).ToList<Tweet>();
                }
            }
            catch (Exception ex)
            {
                CMS.EvenLog.WritoToEventLog(ex);
                m_tweets = new List<Tweet>();
            }

        }

        private string DownloadXML(string Url)
        {
            try
            {
                WebRequest request = WebRequest.Create(Url);
                WebResponse res = request.GetResponse();
                string text;
                using (StreamReader reader = new StreamReader(res.GetResponseStream()))
                {
                    text = reader.ReadToEnd();
                }
                return text;
            }
            catch// ( Exception ex ) 
            {
                //CMS.EvenLog.WritoToEventLog( ex );
                return null;
            }
        }
    }

    public class TwitterControl : CMS.Controls.CmsControl
    {
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            string url = ConfigValue("EURONA:TwitterUrl");
            if (System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower() == "pl")
                url = ConfigValue("EURONA:TwitterUrl:pl");
            if (System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower() == "sk")
                url = ConfigValue("EURONA:TwitterUrl:sk");

            if (this.Session[url] != null && this.Session[url].ToString().Length != 0)
            {
                this.Controls.Add(new LiteralControl(this.Session[url].ToString()));
                return;
            }

            TweetStream ts = new TweetStream(url);
            List<Tweet> list = Tweet.GetTwetts(ts);

            StringBuilder sbTweetString = new StringBuilder();

            foreach (Tweet t in list)
            {
                sbTweetString.Append("&nbsp;&nbsp;&laquo;" + t.Text + "&laquo;&nbsp;&nbsp;");
            }

            this.Session[url] = sbTweetString.ToString();
            this.Controls.Add(new LiteralControl(sbTweetString.ToString()));

        }
    }
    public class TwitterControlAdvisor : CMS.Controls.CmsControl
    {
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            string url = ConfigValue("EURONA:TwitterKacelarUrl");
            if (System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower() == "pl")
                url = ConfigValue("EURONA:TwitterKacelarUrl:pl");
            if (System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower() == "sk")
                url = ConfigValue("EURONA:TwitterKacelarUrl:sk");

            if (this.Session[url] != null && this.Session[url].ToString().Length != 0)
            {
                this.Controls.Add(new LiteralControl(this.Session[url].ToString()));
                return;
            }

            TweetStream ts = new TweetStream(url);
            List<Tweet> list = Tweet.GetTwetts(ts);

            StringBuilder sbTweetString = new StringBuilder();

            foreach (Tweet t in list)
            {
                sbTweetString.Append("&nbsp;&nbsp;&laquo;" + t.Text + "&laquo;&nbsp;&nbsp;");
            }

            this.Session[url] = sbTweetString.ToString();
            this.Controls.Add(new LiteralControl(sbTweetString.ToString()));
        }
    }

    public class CernyForLifeTwitterControl : CMS.Controls.CmsControl
    {
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            string url = ConfigValue("CERNYFORLIFE:TwitterUrl");
            if (System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower() == "pl")
                url = ConfigValue("CERNYFORLIFE:TwitterUrl:pl");
            if (System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower() == "sk")
                url = ConfigValue("CERNYFORLIFE:TwitterUrl:sk");

            if (this.Session[url] != null && this.Session[url].ToString().Length != 0)
            {
                this.Controls.Add(new LiteralControl(this.Session[url].ToString()));
                return;
            }

            TweetStream ts = new TweetStream(url);
            List<Tweet> list = Tweet.GetTwetts(ts);

            StringBuilder sbTweetString = new StringBuilder();

            foreach (Tweet t in list)
            {
                sbTweetString.Append("&nbsp;&nbsp;&laquo;" + t.Text + "&laquo;&nbsp;&nbsp;");
            }

            this.Session[url] = sbTweetString.ToString();
            this.Controls.Add(new LiteralControl(sbTweetString.ToString()));

        }
    }
    public class CernyForLifeTwitterControlAdvisor : CMS.Controls.CmsControl
    {
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            string url = ConfigValue("CERNYFORLIFE:TwitterKacelarUrl");
            if (System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower() == "pl")
                url = ConfigValue("CERNYFORLIFE:TwitterKacelarUrl:pl");
            if (System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower() == "sk")
                url = ConfigValue("CERNYFORLIFE:TwitterKacelarUrl:sk");

            if (this.Session[url] != null && this.Session[url].ToString().Length != 0)
            {
                this.Controls.Add(new LiteralControl(this.Session[url].ToString()));
                return;
            }

            TweetStream ts = new TweetStream(url);
            List<Tweet> list = Tweet.GetTwetts(ts);

            StringBuilder sbTweetString = new StringBuilder();

            foreach (Tweet t in list)
            {
                sbTweetString.Append("&nbsp;&nbsp;&laquo;" + t.Text + "&laquo;&nbsp;&nbsp;");
            }

            this.Session[url] = sbTweetString.ToString();
            this.Controls.Add(new LiteralControl(sbTweetString.ToString()));
        }
    }
}


