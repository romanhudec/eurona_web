using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.DAL.Entities;
using ProductsEntity = Eurona.Common.DAL.Entities.Product;
using Telerik.Web.UI;
using System.IO;
using OrderEntity = Eurona.DAL.Entities.Order;
using Eurona.Common.Controls.Cart;
using Eurona.Common;
using Eurona.Controls;
using OrganizationEntity = Eurona.Common.DAL.Entities.Organization;

namespace Eurona.User.Advisor {
    public partial class DefaultPage : WebPage {
        int waitingOrdersToAccept = 0;

        protected void Page_Load(object sender, EventArgs e) {
            if (!Security.Account.IsInRole(Role.ADVISOR) && !Security.Account.IsInRole(Role.ADMINISTRATOR))
                return;

            Eurona.Common.Application.BrowserInfo bi = Eurona.Common.Application.GetBrowserVersion(this.Request);
            if (bi.Validate() == false) {
                string js = string.Format("alert('{0}');", "Používáte starou verzi prohlížeče a nekteré části portálu se Vám nemusí správne zobrazit. Prosím aktualizujte svůj prohlížeč na nejnovější verzi.");
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "BrowserInfoJS", js, true);
            }

            if (this.LogedAdvisor != null && (this.LogedAdvisor.AngelTeamClen || this.LogedAdvisor.AngelTeamManager))
                this.trPrehledSkupinyATP.Visible = true;
            /*
            ProductsEntity.ReadByFilter filter = null;
            filter = new Eurona.Common.DAL.Entities.Product.ReadByFilter { TOPProducts = true, MaxCount = 10 };
            List<ProductsEntity> list = Storage<ProductsEntity>.Read(filter);
             * */
            ProductsEntity.ReadVamiNejviceNakupovane filter = new ProductsEntity.ReadVamiNejviceNakupovane();
            List<ProductsEntity> list = Storage<ProductsEntity>.Read(filter);
            this.radRotatorNews.RotatorType = RotatorType.Buttons;
            this.radRotatorNews.WrapFrames = true;
            this.radRotatorNews.DataSource = list;
            this.radRotatorNews.DataBind();

            // Nacitanie poctu objednavok cakajucich na akceptaciu pridruzenia
            OrderEntity.ReadByFilter filterOrder = new OrderEntity.ReadByFilter();
            filterOrder.AssociationAccountId = Security.Account.Id;
            filterOrder.AssociationRequestStatus = (int)OrderEntity.AssociationStatus.WaitingToAccept;
            List<OrderEntity> listOrders = Storage<OrderEntity>.Read(filterOrder);
            waitingOrdersToAccept = listOrders.Count;

            DateTime date = DateTime.Now.AddMonths(-1);
            decimal platnychNaTentoMesic = BonusovyKreditUzivateleHelper.GetPlatnyKreditCelkem(Security.Account, date.Year, date.Month);
            decimal cerpanoTentoMesic = BonusovyKreditUzivateleHelper.GetCerpaniKredituCelkem(Security.Account, date.Year, date.Month);
            this.lblStavBK.InnerText = (platnychNaTentoMesic - cerpanoTentoMesic).ToString("F0");


            //Rozblikanie headeru pri nevybavenych ziadostiach o pridruzenie
            if (waitingOrdersToAccept != 0)
                this.ClientScript.RegisterStartupScript(this.Page.GetType(), "jsBlink", "blink('tdOrdersToAssociate','divOrdersToAssociate');", true);

            //Mimoradna nabidka
            /*
            this.tableMimoradnaNabidka.Visible = false;
            MimoradnaNabidka mn = Storage<MimoradnaNabidka>.ReadFirst();
            if (mn != null) {
                this.lblMimoradnaNabidkaTitle.Text = mn.Title;
                this.imgMimoradnaNabidka.Src = Page.ResolveUrl(mn.Icon);
                this.tableMimoradnaNabidka.Visible = true;
                this.btnMimoradnaNabidka.CommandArgument = mn.Alias;
            }
             * */

            if (Page.Request.RawUrl.Contains("?verified")) {
                if (Security.Account.IsInRole(Eurona.DAL.Entities.Role.ANONYMOUSADVISOR)) {
                    Response.Redirect("~/user/anonymous/cart.aspx");
                }
            }
        }

        public OrganizationEntity LogedAdvisor {
            get { return (this.Master as PageMasterPage).LogedAdvisor; }
        }

        public string GetImageSrc(int productId) {
            string noImageUrl = this.ResolveUrl("~/images/noimage.png");

            string storageUrl = CMS.Utilities.ConfigUtilities.ConfigValue("SHP:ImageGallery:Product:StoragePath", this.Page);

            string storagePath = string.Format("{0}{1}/", storageUrl, productId);
            string productImagesPath = this.Server.MapPath(storagePath);

            if (!Directory.Exists(productImagesPath))
                return noImageUrl;

            DirectoryInfo di = new DirectoryInfo(productImagesPath);
            FileInfo[] fileInfos = di.GetFiles("*.*");

            if (fileInfos.Length == 0)
                return noImageUrl;

            //Sort files by name
            Comparison<FileInfo> comparison = new Comparison<FileInfo>(delegate(FileInfo a, FileInfo b) {
                return String.Compare(a.Name, b.Name);
            });
            Array.Sort(fileInfos, comparison);

            string urlThumbnail = storagePath + "_t/" + fileInfos[0].Name;
            return this.ResolveUrl(urlThumbnail);
        }

        //protected void OnMimoradnaNabidkaClick(object sender, EventArgs e) {
        //    if (string.IsNullOrEmpty(this.btnMimoradnaNabidka.CommandArgument)) return;
        //    Response.Redirect(Page.ResolveUrl(this.btnMimoradnaNabidka.CommandArgument));
        //}
        protected void OnOrdersToAssociate(object sender, EventArgs e) {
            Page.Response.Redirect(Page.ResolveUrl("~/user/advisor/orderstoassociate.aspx"));
        }

        protected void OnAddCart(object sender, EventArgs e) {
            Button btn = (sender as Button);
            if (string.IsNullOrEmpty(btn.CommandArgument))
                return;
            int quantity = 1;
            int productId = 0;

            //TextBox txtQuantity = (TextBox)btn.Parent.Controls[2];
            //if ( !Int32.TryParse( txtQuantity.Text, out quantity ) ) quantity = 1;

            Int32.TryParse(btn.CommandArgument, out productId);

            ProductsEntity product = Storage<ProductsEntity>.ReadFirst(new ProductsEntity.ReadById { ProductId = productId });
            if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(product.Code, product, quantity, this))
                return;
            if (!EuronaCartHelper.AddProductToCart(this.Page, productId, quantity, this))
                return;

            //Alert s informaciou o pridani do nakupneho kosika
            string js = string.Format("alert('{0}');window.location.href=window.location.href+'{1}nocache='+(new Date()).getSeconds();",
                 string.Format(SHP.Resources.Controls.AdminProductControl_ProductWasAddedToCart_Message, btn.CommandName, quantity),
                 this.Request.RawUrl.Contains("?") ? "&" : "?");
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "addProductToCart", js, true);
        }

    }
}