using System.Web;
using CMS.Controls.Page;

namespace CMS.Utilities
{
		public static class DefaultContentProcessor
		{
				public static string Process( PageControl sender, string content )
				{
						HttpContext context = HttpContext.Current;
						HttpRequest request = context.Request;
						return content
							.Replace( "${LOCALE}", System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName )
							.Replace( "${ROOT}", ServerUtilities.Root( request ) )
							.Replace( "${IDENTITY}", context.User.Identity.Name )
						;
				}
		}
}
