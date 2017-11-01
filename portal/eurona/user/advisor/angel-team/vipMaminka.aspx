<%@ Page Title="V.I.P.maminka" Language="C#" MasterPageFile="~/user/advisor/angel-team/page.master" AutoEventWireup="true" CodeBehind="vipMaminka.aspx.cs" Inherits="Eurona.User.Advisor.AngelTeam.VIPMaminkaPage" %>
<%@ Register Assembly="cms" Namespace="CMS.Utilities" TagPrefix="cmsUtilities" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
         #atmi_vipMaminka{background-image: url(../../../images/angel-menuitem-selected-bg.png);}
        .vip-maminky{margin-bottom:10px;}
        .vip-maminky .nadpis{color:#317cb4;}
        .vip-maminky .contact{padding:0px 10px 0px 10px;white-space:nowrap;}
                
        .tooltip{font-style:italic;color:#DADAb4;}
        .button a{color:#fff;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
	<cmsUtilities:AliasUtilities ID="aliasUtilities" runat="server" ></cmsUtilities:AliasUtilities>
    <div style="margin:20px;">
        <div>
            <table border="0" width="100%">
                <tr>
                    <td valign="top">
                        <div class="vip-maminky">
							<table border="0" style="width:100%;" cellpadding="10">
								<tr>
									<td colspan="3" align="right">
										<div class="button" style="width:60px;">
											<a target="_blank" href='<%=aliasUtilities.Resolve("~/user/advisor/forumThreads.aspx") %>'>
												<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Forums_ForumContent %>"></asp:Literal>
											</a>
										</div>
									</td>
								</tr>
								<%if (LogedAdvisor != null && this.LogedAdvisor.AngelTeamManagerTyp == 4){ %>
								<tr>
									<td colspan="3">
										<span class="nadpis">V.I.P.maminka / lector</span> 
									</td>
								</tr>
                                <tr>
                                    <td style="width:70px;"><img id="Img1" runat="server" src="~/images/angel-man-red-wsh.png" width="70" height="72" alt=""/></td>
                                    <td>
										<div class="contact"><asp:Literal runat="server" ID="lblName"></asp:Literal></div>
										<div class="contact"><asp:Literal runat="server" ID="lblCity"></asp:Literal></div>
										<div class="contact"><asp:Literal runat="server" ID="lblPhone"></asp:Literal></div>
										<div class="contact"><asp:Literal runat="server" ID="lblEmail"></asp:Literal></div>
									</td>
                                    <td style="width:110px;" align="right"><div class="button" style="width:50px;"><a id="hlDiskuze" runat="server" target="_blank"><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, ATP_Diskuze %>"></asp:Literal></a></div></td>
                                </tr>
								<%} %>
								<asp:Repeater runat="server" ID="rpMaminky" OnItemDataBound="OnItemDataBound">
									<HeaderTemplate>
										<tr>
											<td colspan="3">
												<span class="nadpis">V.I.P.maminky</span>
											</td>
										</tr>
									</HeaderTemplate>
									<ItemTemplate>
										<tr>
											<td style="width:70px;"><img id="Img1" runat="server" src="~/images/angel-man-red-ws.png" width="70" height="72" alt=""/></td>
											<td>
												<div class="contact"><%#Eval( "Name" )%></div>
												<div class="contact"><%#Eval( "RegisteredAddress.City" )%></div>
												<div class="contact"><%#Eval("ContactPhone")%></div>
												<div class="contact"><%#Eval("ContactEmail")%></div>
											</td>
											<td style="width:110px;" align="right"><div class="button" style="width:50px;"><a runat="server" href='<%#"~/user/advisor/angel-team/diskuze.aspx?id="+Eval("AccountId") %>'><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, ATP_Diskuze %>"></asp:Literal></a></div></td>
										</tr>
									</ItemTemplate>
									<FooterTemplate></FooterTemplate>
								</asp:Repeater>
							</table>
                        </div>
                    </td>
                    <td style="width:250px;" valign="top" align="right">
                        <img runat="server" src="~/images/angel-vip-maminka.png" width="400" height="273" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
