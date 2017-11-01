<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HomeFlashControl.ascx.cs" Inherits="Eurona.Controls.HomeFlashControl" %>
<div class="homeFlashControl">

<script src="../javascripts/homeflashcontrol.js" type="text/javascript"></script>

<noscript>
	<object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=10,0,0,0" width="1024" height="768" id="homeFlash" align="middle">
	<param name="allowScriptAccess" value="sameDomain" />
    <param name="wmode" value="transparent"> 
	<param name="allowFullScreen" value="false" />
	<param name="movie" value="homeFlash.swf" />
    <param name="quality" value="high" />
    <%string muteCookieName =  "EURONA:Mute";

		string value = Request.Cookies[muteCookieName] != null ? Request.Cookies[muteCookieName].Value : null;
        if ( !string.IsNullOrEmpty( value ) && value == "true" )
        {
            %><param name="mute" value="1" /><%
        }
    %>
    <param name="bgcolor" value="#ffffff" />	
    <embed src="homeFlash.swf" quality="high" bgcolor="#ffffff" width="1024" height="768" name="homeFlash" align="middle" allowScriptAccess="sameDomain" allowFullScreen="false" type="application/x-shockwave-flash" pluginspage="http://www.adobe.com/go/getflashplayer_cz" />
	</object>
</noscript>
</div>