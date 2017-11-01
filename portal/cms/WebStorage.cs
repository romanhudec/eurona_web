using System.Configuration;
using System.Web;
using System.Web.SessionState;
using CMS.Entities;
using CMS.MSSQL;
using System;

namespace CMS {
    public class WebStorage<T> where T : class, new() {
        public virtual IStorage<T> Access() {
            IStorage<T> access = null;
            HttpSessionState session = HttpContext.Current.Session;

            if (session != null && session[GetType().ToString()] is IStorage<T>)
                access = session[GetType().ToString()] as IStorage<T>;
            if (access != null) return access;

            Account account = session != null ? session["account"] as Account : null;

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            int instanceId = 0;
            if (!Int32.TryParse(ConfigurationManager.AppSettings["InstanceId"], out instanceId))
                throw new InvalidOperationException("In AppSettings must be defined InstanceId!");


            //Upgrade
            if (typeof(T) == typeof(CMS.Entities.Upgrade)) access = new UpgradeStorage(instanceId, account, connectionString) as IStorage<T>;

            //SupportedLocale
            if (typeof(T) == typeof(CMS.Entities.Classifiers.SupportedLocale)) access = new MSSQL.Classifiers.SupportedLocaleStorage(instanceId, account, connectionString) as IStorage<T>;

            if (typeof(T) == typeof(CMS.Entities.Account)) access = new AccountStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(CMS.Entities.AccountCredit)) access = new AccountCreditStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(CMS.Entities.Role)) access = new RoleStorage(instanceId, account, connectionString) as IStorage<T>;

            //Profile
            if (typeof(T) == typeof(Profile)) access = new ProfileStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(AccountProfile)) access = new AccountProfileStorage(instanceId, account, connectionString) as IStorage<T>;

            if (typeof(T) == typeof(CMS.Entities.UrlAlias)) access = new UrlAliasStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(CMS.Entities.Classifiers.UrlAliasPrefix)) access = new CMS.MSSQL.Classifiers.UrlAliasPrefixStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(CMS.Entities.Page)) access = new PageStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(CMS.Entities.MasterPage)) access = new MasterPageStorage(instanceId, account, connectionString) as IStorage<T>;

            if (typeof(T) == typeof(CMS.Entities.Menu)) access = new MenuStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(CMS.Entities.NavigationMenu)) access = new NavigationMenuStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(CMS.Entities.NavigationMenuItem)) access = new NavigationMenuItemStorage(instanceId, account, connectionString) as IStorage<T>;

            if (typeof(T) == typeof(CMS.Entities.Person)) access = new PersonStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(CMS.Entities.Organization)) access = new OrganizationStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(CMS.Entities.BankContact)) access = new BankContactStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(CMS.Entities.Address)) access = new AddressStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(CMS.Entities.Poll)) access = new PollStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(CMS.Entities.PollOption)) access = new PollOptionStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(CMS.Entities.PollAnswer)) access = new PollAnswerStorage(instanceId, account, connectionString) as IStorage<T>;

            if (typeof(T) == typeof(CMS.Entities.Tag)) access = new TagStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(CMS.Entities.Comment)) access = new CommentStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(CMS.Entities.AccountVote)) access = new AccountVoteStorage(instanceId, account, connectionString) as IStorage<T>;

            if (typeof(T) == typeof(CMS.Entities.News)) access = new NewsStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(CMS.Entities.Newsletter)) access = new NewsletterStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(CMS.Entities.FAQ)) access = new FAQStorage(instanceId, account, connectionString) as IStorage<T>;

            //SearchEngine
            if (typeof(T) == typeof(CMS.Entities.SearchEngineBase)) access = new SearchEngineStorage(instanceId, account, connectionString) as IStorage<T>;

            //Services
            if (typeof(T) == typeof(CMS.Entities.Classifiers.PaidService)) access = new MSSQL.Classifiers.PaidServiceStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(CMS.Entities.ProvidedService)) access = new ProvidedServiceStorage(instanceId, account, connectionString) as IStorage<T>;

            //International Phone Number Format
            if (typeof(T) == typeof(CMS.Entities.IPNF)) access = new IPNFStorage(instanceId, account, connectionString) as IStorage<T>;

            // Vocabulary & Translation
            if (typeof(T) == typeof(CMS.Entities.Vocabulary)) access = new VocabularyStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(CMS.Entities.Translation)) access = new TranslationStorage(instanceId, account, connectionString) as IStorage<T>;

            //Articles
            if (typeof(T) == typeof(CMS.Entities.Classifiers.ArticleCategory)) access = new CMS.MSSQL.Classifiers.ArticleCategoryStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(Article)) access = new ArticleStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(ArticleTag)) access = new ArticleTagStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(ArticleComment)) access = new ArticleCommentStorage(instanceId, account, connectionString) as IStorage<T>;

            //Blogs
            if (typeof(T) == typeof(Blog)) access = new BlogStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(BlogTag)) access = new BlogTagStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(BlogComment)) access = new BlogCommentStorage(instanceId, account, connectionString) as IStorage<T>;

            //ImageGallery
            if (typeof(T) == typeof(ImageGallery)) access = new ImageGalleryStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(ImageGalleryTag)) access = new ImageGalleryTagStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(ImageGalleryComment)) access = new ImageGalleryCommentStorage(instanceId, account, connectionString) as IStorage<T>;

            if (typeof(T) == typeof(ImageGalleryItem)) access = new ImageGalleryItemStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(ImageGalleryItemComment)) access = new ImageGalleryItemCommentStorage(instanceId, account, connectionString) as IStorage<T>;

            //Forum
            if (typeof(T) == typeof(ForumThread)) access = new ForumThreadStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(Forum)) access = new ForumStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(ForumPost)) access = new ForumPostStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(ForumTracking)) access = new ForumTrackingStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(ForumFavorites)) access = new ForumFavoritesStorage(instanceId, account, connectionString) as IStorage<T>;
            if (typeof(T) == typeof(ForumPostAttachment)) access = new ForumPostAttachmentStorage(instanceId, account, connectionString) as IStorage<T>;

            if (typeof(T) == typeof(EmailLog)) access = new EmailLogStorage(instanceId, account, connectionString) as IStorage<T>;
            

            // pozor! AccountStorage je specificky, musi fungovat aj vtedy ked Account (ako param v ctor) je null!
            // preto to session napcham iba take storage, ktorym uz je jasny Account
            // inac povedane, AccountStorage sa vytvori prvy raz Account=null, a druhy raz uz s vyplenym account...
            if (session != null && account != null) session[GetType().ToString()] = access;

            return access;
        }
    }
}

