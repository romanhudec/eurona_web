<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HomeFlashRotatorControl.ascx.cs" Inherits="Eurona.Controls.HomeFlashRotatorControl" %>
<script type="text/javascript">
    dojo.require("dojox.widget.AutoRotator");
    dojo.require("dojox.widget.rotator.Controller");
    dojo.require("dojox.widget.rotator.Fade");
</script>
<link href="../styles/homeflashrotatorcontrol.css" rel="stylesheet" type="text/css" />
<div>
    <asp:Image runat="server" style="width:920px;height:240px;" ID="ImageMap2" ImageUrl="~/images/banner_homepage.png" />
<%-- 
    <asp:ImageMap style="width:920px;height:240px;" ID="ImageMap2" ImageUrl="~/images/banner_homepage.png" HotSpotMode="Navigate"  runat="server">
        <asp:RectangleHotSpot Top="101" Bottom="185" Left="307" Right="391" AlternateText="Eurona TV" NavigateUrl ="/eshop/" />
        <asp:RectangleHotSpot Top="150" Bottom="187" Left="648" Right="854" AlternateText="Eurona na Facebooku" NavigateUrl ="http://www.facebook.com/euronabycerny" />
    </asp:ImageMap>

   <div id="flash-banner" >
        <div class="banner" style="margin:auto;">
            <div dojoType='dojox.widget.AutoRotator' class='rotator' id='bannerRotator' jsId='bannerRotator' transition='dojox.widget.rotator.fade' duration='3000' random='false' cycles='7'>
            <%=RenderFlash() %>
            </div>
        </div>
        <div class='switch' dojoType='dojox.widget.rotator.Controller' commands='#' rotator='bannerRotator'></div>
    </div>--%>
</div>