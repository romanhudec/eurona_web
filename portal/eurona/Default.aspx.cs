using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProductsEntity = Eurona.Common.DAL.Entities.Product;
using Telerik.Web.UI;
using System.IO;
using System.Configuration;

namespace Eurona
{
	public partial class DefaultPage : WebPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			this.Title = this.genericPage.Title;
			ProductsEntity.ReadByFilter filter = null;
			filter = new Eurona.Common.DAL.Entities.Product.ReadByFilter { Novinka = true };
            
			//List<ProductsEntity> list = Storage<ProductsEntity>.Read( filter );
			//this.radRotatorNews.RotatorType = RotatorType.Buttons;
			//this.radRotatorNews.WrapFrames = true;
			//this.radRotatorNews.DataSource = list;
			//this.radRotatorNews.DataBind();

			//SetupMuteButton();

			if (Request.QueryString.ToString().Contains("login"))
			{
				(this.Master as PageMasterPage).HideAkcniNabidky();
                (this.Master as PageMasterPage).HideNovinky();
				(this.Master as PageMasterPage).HideVanoce();
				(this.Master as PageMasterPage).HideCart();
			}
		}

		protected override void CreateChildControls()
		{
			base.CreateChildControls();
		}
		/*
		protected void OnMute(object sender, EventArgs e)
		{
			string muteCookieName = "EURONA:Mute";

			string value = Request.Cookies[muteCookieName] != null ? Request.Cookies[muteCookieName].Value : null;
			if (string.IsNullOrEmpty(value))
			{
				HttpCookie c = new HttpCookie(muteCookieName);
				c.Value = "true";
				c.Expires = DateTime.Now.AddYears(1);
				Response.Cookies.Add(c);
				this.btnMute.ImageUrl = "~/images/mute1.png";
			}
			else
			{
				Response.Cookies.Remove(muteCookieName);
				HttpCookie c = new HttpCookie(muteCookieName);
				c.Value = value == "true" ? "false" : "true";
				c.Expires = DateTime.Now.AddYears(1);
				Response.Cookies.Add(c);

				if (c.Value == "true") this.btnMute.ImageUrl = "~/images/mute1.png";
				else this.btnMute.ImageUrl = "~/images/mute0.png";
			}

		}
		*/
		/*
		protected void SetupMuteButton()
		{
			string muteCookieName = "EURONA:Mute";
			string value = Request.Cookies[muteCookieName] != null ? Request.Cookies[muteCookieName].Value : null;
			if (!string.IsNullOrEmpty(value) && value == "true")
				this.btnMute.ImageUrl = "~/images/mute1.png";
			else
				this.btnMute.ImageUrl = "~/images/mute0.png";
		}
		*/
		public string GetImageSrc(int productId)
		{
			string noImageImg = this.ResolveUrl("~/images/noimage.png");

			string storageUrl = CMS.Utilities.ConfigUtilities.ConfigValue("SHP:ImageGallery:Product:StoragePath", this.Page);

			string storagePath = string.Format("{0}{1}/", storageUrl, productId);
			string productImagesPath = this.Server.MapPath(storagePath);

			if (!Directory.Exists(productImagesPath))
				return noImageImg;

			DirectoryInfo di = new DirectoryInfo(productImagesPath);
			FileInfo[] fileInfos = di.GetFiles("*.*");

			if (fileInfos.Length == 0)
				return noImageImg;

			string urlThumbnail = storagePath + "_t/" + fileInfos[0].Name;
			return this.ResolveUrl(urlThumbnail);
		}
	}
}
