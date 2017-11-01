<%@ Page Title="Lector Team" Language="C#" MasterPageFile="~/user/advisor/angel-team/page.master" AutoEventWireup="true" CodeBehind="lectorTeam.aspx.cs" Inherits="Eurona.User.Advisor.AngelTeam.LectorTeamPage" %>
<%@ Register Assembly="cms" Namespace="CMS.Utilities" TagPrefix="cmsUtilities" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
         #atmi_lectorTeam{background-image: url(../../../images/angel-menuitem-selected-bg.png);}
        .atp-lektori{margin-bottom:10px;}
        .atp-lektori .nadpis{color:#317cb4;}
        .atp-lektori .contact{padding:0px 10px 0px 10px;white-space:nowrap;}
        
        .angel-man-blue{width:50px;height:49px;background-image:url(../../../images/angel-man-blue.png);}
        .angel-man-yellow{width:50px;height:64px;background-image:url(../../../images/angel-man-yellow.png);}
        
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
                        <div class="atp-lektori">
							<table border="0" style="width:100%;" cellpadding="10">
								<tr>
									<td colspan="3" align="right">
										<div class="button" style="width:60px;">
											<a target="_blank" href='<%=aliasUtilities.Resolve("~/user/advisor/forumThreads.aspx") %>'>
												<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Forums_ForumContent %>"></asp:Literal>
											</a>
										</div>
									</td>
								</tr>
							   <%if (LogedAdvisor != null && (this.LogedAdvisor.AngelTeamManagerTyp == 2 || this.LogedAdvisor.AngelTeamManagerTyp == 5 || this.LogedAdvisor.AngelTeamManagerTyp == 6))
								{ %>
								<tr>
									<td colspan="3">
										<span class="nadpis">Angel lector</span>
									</td>
								</tr>
                                <tr>
                                    <td style="width:70px;"><img id="Img1" runat="server" src="~/images/angel-man-yellow-ws.png" width="70" height="72" alt=""/></td>
                                    <td>
										<div class="contact"><asp:Literal runat="server" ID="lblName"></asp:Literal></div>
										<div class="contact"><asp:Literal runat="server" ID="lblCity"></asp:Literal></div>
										<div class="contact"><asp:Literal runat="server" ID="lblPhone"></asp:Literal></div>
										<div class="contact"><asp:Literal runat="server" ID="lblEmail"></asp:Literal></div>
									</td>
                                    <td style="width:110px;" align="right"><div class="button" style="width:50px;"><a id="hlDiskuze" runat="server" target="_blank"><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, ATP_Diskuze %>"></asp:Literal></a></div></td>
                                </tr>
								<%} %>
								<asp:Repeater runat="server" ID="rpLektori" OnItemDataBound="OnItemDataBound">
									<HeaderTemplate>
										<tr>
											<td colspan="3">
												<span class="nadpis">ATP <asp:Literal runat="server" Text="<%$ Resources:EShopStrings, ATP_Lektori %>"></asp:Literal></span>

											</td>
										</tr>
									</HeaderTemplate>
									<ItemTemplate>
										<tr>
											<td style="width:70px;"><img runat="server" src="~/images/angel-man-blue-ws.png" width="70" height="72" alt=""/></td>
											<td>
												<div class="contact"><%#Eval( "Name" )%></div>
												<div class="contact"><%#Eval( "RegisteredAddress.City" )%></div>
												<div class="contact"><%#Eval("ContactPhone")%></div>
												<div class="contact"><%#Eval("ContactEmail")%></div>
											</td>
											<td style="width:110px;"align="right"><div class="button" style="width:50px;"><a id="A1" runat="server" href='<%#"~/user/advisor/angel-team/diskuze.aspx?id="+Eval("AccountId") %>'><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, ATP_Diskuze %>"></asp:Literal></a></div></td>
										</tr>
									</ItemTemplate>
									<FooterTemplate></FooterTemplate>
								</asp:Repeater> 
							</table>   
                        </div>
                    </td>
                    <td style="width:250px;" valign="top" align="right">
                        <img runat="server" src="~/images/angel-vip-lector.png" width="400" height="267" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
