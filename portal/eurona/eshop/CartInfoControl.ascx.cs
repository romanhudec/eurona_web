﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CartEntity = Eurona.Common.DAL.Entities.Cart;
using ShpCultureUtilities = SHP.Utilities.CultureUtilities;
using CMS.Utilities;
using Eurona.Common.Controls.Cart;

namespace Eurona.Eshop {
    public partial class CartInfoControl : System.Web.UI.UserControl {
        private CartEntity cart = null;
        protected void Page_Load(object sender, EventArgs e) {
            this.UpdateControl(false);
        }

        public void UpdateControl(bool renew) {
            if (renew) this.cart = null;
            decimal price = this.CartEntity != null && this.CartEntity.PriceTotalWVAT.HasValue ? this.CartEntity.PriceTotalWVAT.Value : 0m;
            int count = this.CartEntity != null ? this.CartEntity.CartProductsCount : 0;

            this.txtPrice.Text = ShpCultureUtilities.CurrencyInfo.ToString(price, this.Session);
            this.txtCount.Text = count.ToString();

            //Check na URL Alias
            if (Security.IsLogged(false) && !Security.IsInRole(Eurona.Common.DAL.Entities.Role.ANONYMOUSADVISOR)) {
                AliasUtilities aliasUtils = new AliasUtilities();
                string url = aliasUtils.Resolve("~/user/advisor/cart.aspx", this.Page);
                this.hlCart.NavigateUrl = url;
                this.hlImage.NavigateUrl = url;
                this.hlkPokladne.HRef = Page.ResolveUrl(url);
                return;
            }

            //ANONYMOUS_CART je nastaveny pokial sa jedna o objednavku tvorenu z anonymneho kosika
            if (!Security.IsLogged(false) || (Security.IsLogged(false) && Security.IsInRole(Eurona.Common.DAL.Entities.Role.ANONYMOUSADVISOR))) {
                AliasUtilities aliasUtils = new AliasUtilities();
                //string url = aliasUtils.Resolve( "~/login.aspx?login", this.Page );
                string url = aliasUtils.Resolve("~/user/anonymous/default.aspx", this.Page);
                if (Security.IsLogged(false))
                    url = aliasUtils.Resolve("~/user/anonymous/cart.aspx", this.Page);

                this.hlCart.NavigateUrl = url;
                this.hlImage.NavigateUrl = url;
                this.hlkPokladne.HRef = Page.ResolveUrl(url);
            }
        }

        public CartEntity CartEntity {
            get {
                if (this.cart != null) return this.cart;
                this.cart = EuronaCartHelper.GetAccountCart(this.Page);
                return this.cart;
            }
        }
    }
}