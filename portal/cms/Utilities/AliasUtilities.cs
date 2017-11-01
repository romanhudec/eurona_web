using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.UI;
using Entity = CMS.Entities.Entity;
using IUrlAliasEntity = CMS.Entities.IUrlAliasEntity;
using UrlAliasEntity = CMS.Entities.UrlAlias;

namespace CMS.Utilities
{
	public class AliasUtilities : Control
	{
		public AliasUtilities()
		{
		}

		/// <summary>
		/// Vráti URL Alias pre danú URL adresu ak existuje.
		/// Ak URL Alias pre danu URL neexiszuje vráti povodnu URL.
		/// </summary>
		public string Resolve(string url)
		{
			string alias = GetAlias(url);
			return alias.StartsWith("~") ? Page.ResolveUrl(alias) : alias;

			//CMS.Entities.UrlAlias alias = Storage<CMS.Entities.UrlAlias>.ReadFirst( new CMS.Entities.UrlAlias.ReadByUrl { Url = url } );
			//if ( alias == null ) return Page.ResolveUrl( url );
			//return Page.ResolveUrl( alias.Alias );
		}

		/// <summary>
		/// Vráti URL Alias pre danú URL adresu ak existuje.
		/// Ak URL Alias pre danu URL neexiszuje vráti povodnu URL.
		/// </summary>
		public string Resolve(string url, Page page)
		{
			string alias = GetAlias(url);
			return alias.StartsWith("~") ? page.ResolveUrl(alias) : alias;

			//CMS.Entities.UrlAlias alias = Storage<CMS.Entities.UrlAlias>.ReadFirst( new CMS.Entities.UrlAlias.ReadByUrl { Url = url } );
			//if ( alias == null ) return page.ResolveUrl( url );
			//return page.ResolveUrl( alias.Alias );
		}

		public string GetAlias(string url)
		{
			string validUrl = url.StartsWith("~") ? url : "~" + url;
			CMS.Entities.UrlAlias alias = Storage<CMS.Entities.UrlAlias>.ReadFirst(new CMS.Entities.UrlAlias.ReadByUrl { Url = validUrl });
			if (alias == null) return url;
			return alias.Alias;
		}

		public string GetAliasAsRoot(string url)
		{
			string validUrl = url.StartsWith("~/") ? url : "~/" + url;
			CMS.Entities.UrlAlias alias = Storage<CMS.Entities.UrlAlias>.ReadFirst(new CMS.Entities.UrlAlias.ReadByUrl { Url = validUrl });
			if (alias == null) return url;
			url = alias.Alias;
			return url.StartsWith("~/")
				? url.Substring(2) /* odstran ~/ lebo root */
				: url;
		}

		/// <summary>
		/// Metóda vytvori URL Alias podľa vstupných parametrov.
		/// </summary>
		internal static bool CreateUrlAlias<T>(System.Web.UI.Page page, string displayUrl, string aliasName, string alias, T entity)
				where T : Entity, IUrlAliasEntity, new()
		{
			return CreateUrlAlias<T>(page, displayUrl, aliasName, alias, entity, Storage<T>.Instance);
		}

		private static bool GetCharForAlias(ref char ch)
		{
			if (Char.IsLetterOrDigit(ch) || ch == '-' || ch == '_' || ch == ' ')
			{
				if (ch == ' ') ch = '-';
				return true;
			}
			return false;
		}

		public static string GetAliasString(string name)
		{
			string alias = Utilities.StringUtilities.RemoveDiacritics(name);
			alias = alias.ToLower().Trim();

			string newAlias = string.Empty;
			foreach (char ch in alias)
			{
				char ch2 = ch;
				if (GetCharForAlias(ref ch2)) newAlias += ch2;
			}

			// este by sa zislo nahradit duplicity. napr '--' za '-'
			while (newAlias.Contains("--"))
				newAlias = newAlias.Replace("--", "-");

			return newAlias;
		}

		/// <summary>
		/// Metóda vytvori URL Alias podľa vstupných parametrov.
		/// </summary>
		public static bool CreateUrlAlias<T>(System.Web.UI.Page page, string displayUrl, string aliasName, string alias, T entity, IStorage<T> storage)
				where T : Entity, IUrlAliasEntity, new()
		{
			if (!string.IsNullOrEmpty(displayUrl))
			{
				UrlAliasEntity urlAliasExists = Storage<UrlAliasEntity>.ReadFirst(new UrlAliasEntity.ReadByAlias { Alias = alias });
				if (!entity.UrlAliasId.HasValue)
				{
					//Kontrola existencie URL Aliasu
					if (urlAliasExists != null)
					{
						page.ClientScript.RegisterStartupScript(page.GetType(), "alert", "alert('" + Resources.Controls.AdminUrlAliasesControl_UrlAliasExist_Message + "');", true);
						return false;
					}

					UrlAliasEntity urlAlias = new UrlAliasEntity();
					urlAlias.Alias = alias;
					urlAlias.Url = string.Format(displayUrl, entity.Id);
					urlAlias.Name = aliasName;
					Storage<UrlAliasEntity>.Create(urlAlias);

					entity.UrlAliasId = urlAlias.Id;
					storage.Update(entity);
				}
				else
				{
					//Update aliasu, ak dojde ku zmene nazvu galerie, musi sa zmenit aj url.
					UrlAliasEntity urlAlias = Storage<UrlAliasEntity>.ReadFirst(new UrlAliasEntity.ReadById { UrlAliasId = entity.UrlAliasId.Value });
					urlAlias.Alias = alias;
					urlAlias.Url = string.Format(displayUrl, entity.Id);
					urlAlias.Name = aliasName;

					//Kontrola existencie URL Aliasu
					if (urlAliasExists != null && urlAliasExists.Id != urlAlias.Id)
					{
						page.ClientScript.RegisterStartupScript(page.GetType(), "alert", "alert('" + Resources.Controls.AdminUrlAliasesControl_UrlAliasExist_Message + "');", true);
						return false;
					}

					Storage<UrlAliasEntity>.Update(urlAlias);
					return true;
				}
			}
			return true;
		}
		/// <summary>
		/// Metóda vztvori URL Alias podľa vstupných parametrov.
		/// </summary>
		public static bool CreateUrlAlias(System.Web.UI.Page page, string url, string name, string alias)
		{
			if (!string.IsNullOrEmpty(url))
			{
				UrlAliasEntity urlAliasExists = Storage<UrlAliasEntity>.ReadFirst(new UrlAliasEntity.ReadByUrl { Url = url });
				if (urlAliasExists != null)
				{
					urlAliasExists.Alias = alias;
					urlAliasExists.Url = url;
					urlAliasExists.Name = name;
					Storage<UrlAliasEntity>.Update(urlAliasExists);
				}
				else
				{
					urlAliasExists = Storage<UrlAliasEntity>.ReadFirst(new UrlAliasEntity.ReadByAlias { Alias = alias });
					if (urlAliasExists != null)
					{
						page.ClientScript.RegisterStartupScript(page.GetType(), "alert", "alert('" + Resources.Controls.AdminUrlAliasesControl_UrlAliasExist_Message + "');", true);
						return false;
					}
					UrlAliasEntity urlAlias = new UrlAliasEntity();
					urlAlias.Alias = alias;
					urlAlias.Url = url;
					urlAlias.Name = name;
					Storage<UrlAliasEntity>.Create(urlAlias);
				}
			}
			return true;
		}
	}
}
