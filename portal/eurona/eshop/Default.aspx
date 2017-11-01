<%@ Page Language="C#" Title="<%$ Resources:Strings, Navigation_Products %>" MasterPageFile="~/eshop/default.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Eurona.EShop.Default" %>
<%@ Register Assembly="shp" Namespace="SHP.Controls.Category" TagPrefix="shpCategory" %>
<%@ Register src="ProductsControl.ascx" tagname="ProductsControl" tagprefix="uc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <shpCategory:CategoryControl runat="server" ID="categoryControl" />
    <style type="text/css">
        .products-content{background-color:transparent!important;padding-top:10px;text-align:center;}
        /*
        #flash-banner{width:994px;height:417px;overflow:hidden; position:relative;z-index:0;}
        #flash-banner .banner{width:994px;height:417px;margin:0 auto;display:block!important}
        #flash-banner .rotator{width:994px;height:417px;margin:0 auto;}
        #flash-banner .rotator .pane{width:994px;height:417px;margin:auto;position:relative!important;vertical-align:middle;}
        #flash-banner .switch{display:none;}
        #flash-banner .switch ul{margin:0;padding:0;}
        #flash-banner .switch li{list-style:none;margin:0;padding:0; display:inline-block;}

        #flash-banner .switch .dojoxRotatorNumber{display:none;}
        #flash-banner .switch .dojoxRotatorNumber a{margin:10px 40px 10px 40px;display:block;width:40px;height:40px;position:relative;z-index:0; text-decoration:none;}
        #flash-banner .switch .dojoxRotatorNumber span{ line-height:40px;color:#ffffff;font-weight:bold;}
        #flash-banner .switch .dojoxRotatorSelected a{}   
        */
        #flash-banner{overflow:hidden; background-color:#F0F9FE;}
        #flash-banner .banner{width:994px;margin:0 auto;display:block!important;height:417px;position:relative;z-index:0;}
        #flash-banner .rotator{width:994px;margin:0 auto;display:block!important;}
         #flash-banner .rotator .pane{width:994px;height:417px;margin:auto;position:relative!important;vertical-align:middle;}
        
        #flash-banner .switch{display:block;margin:0px auto; width:100%;position:relative; background-color:#F0F9FE;}
        #flash-banner .switch ul{margin:0;padding:0;}
        #flash-banner .switch li{list-style:none;margin:0;padding:0; display:inline-block;}
        #flash-banner .switch .dojoxRotatorNumber a{display:block;margin:10px 3px 10px 3px;width:20px;height:20px;background:url(../images/rotator-switch.png) no-repeat 0 0;position:relative;z-index:0; text-decoration:none;}
        #flash-banner .switch .dojoxRotatorNumber span{ display:none;}
        #flash-banner .switch .dojoxRotatorSelected a{background-image:url(../images/rotator-switch-selected.png); background-repeat:no-repeat;}               
    </style>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
   <div class="exclusiv-flash">
        <script type="text/javascript">
            dojo.require("dojox.widget.AutoRotator");
            dojo.require("dojox.widget.rotator.Controller");
            dojo.require("dojox.widget.rotator.Fade");
        </script>
        <div>
            <div id="flash-banner" >
                <div class="banner" style="margin:auto;">
                    <div dojoType='dojox.widget.AutoRotator' class='rotator' id='bannerRotator' jsId='bannerRotator' transition='dojox.widget.rotator.fade' duration='5000' random='false' cycles='12'>
                    <%=RenderFlash() %>
                    </div>
                </div>
                <div class='switch' dojoType='dojox.widget.rotator.Controller' commands='#' rotator='bannerRotator'></div>
            </div>
        </div>
   </div>
</asp:Content>