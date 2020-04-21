<%@ Page Title="CHECK IN! V.I.P. Zone" Language="C#" MasterPageFile="~/user/advisor/angel-team/page.master" AutoEventWireup="true" CodeBehind="vip.aspx.cs" Inherits="Eurona.User.Advisor.AngelTeam.VIPPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #atmi_vip{background-image: url(../../../images/angel-menuitem-selected-bg.png);}
        .cekajici-novacci{margin-bottom:10px;}
        .cekajici-novacci .nadpis{color:#317cb4;}
        .cekajici-novacci .edit{ display:inline-block;}
        .cekajici-novacci .mesto{padding:0px 10px 0px 10px;white-space:nowrap;}
        
        .prijati-novacci{}
        .prijati-novacci .nadpis{color:#317cb4;}
        .prijati-novacci .mesto{padding:0px 10px 0px 10px;white-space:nowrap;}
        
        .angel-man-blue{width:70px;height:72px;background-image:url(../../../images/angel-man-blue.png);}
        .angel-man-yellow{width:70px;height:72px;background-image:url(../../../images/angel-man-yellow.png);}
        .angel-man-yellow-ws{width:70px;height:72px;background-image:url(../../../images/angel-man-yellow-ws.png);}
        .angel-man-blue-ws{width:70px;height:72px;background-image:url(../../../images/angel-man-blue-ws.png);}
        .angel-man-red-ws{width:70px;height:72px;background-image:url(../../../images/angel-man-red-ws.png);}
        .angel-man-red-wsh{width:70px;height:72px;background-image:url(../../../images/angel-man-red-wsh.png);}
        
        .tooltip{font-style:italic;color:#DADAb4;}
    </style>
    <script type="text/javascript">
        function hideTooltip(id) {
            var elm = document.getElementById('tooltip_' + id.toString());
            elm.style.display = 'none';

            var elmCode = document.getElementById('code_' + id.toString());
            elmCode.focus();
        }

        function showPrijemEdit(id) {
            var elm = document.getElementById('edit_' + id.toString());
            elm.style.display = 'block';
        }

        function validatePrijemNovacka(id) {
            var elm = document.getElementById('code_' + id.toString());
            if (elm.value.length == 0) return false;

            var elmHf = document.getElementById('<%=this.hfRegistracniCislo.ClientID %>');
            elmHf.value = elm.value;
            return true;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <asp:HiddenField runat="server" ID="hfRegistracniCislo" EnableViewState="true" />
    <div style="margin:20px;">
        <div>
            <table border="0" width="100%">
                <tr>
                    <td valign="top">
                        <div class="cekajici-novacci">
                            <span class="nadpis"><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, ATP_CekajiciNovacci %>"></asp:Literal></span>
                            <asp:Repeater runat="server" ID="rpCekajiciNovacci" OnItemDataBound="OnItemDataBound">
                                <HeaderTemplate><table border="0" style="width:100%;"></HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td style="width:70px;"><img runat="server" src="~/images/angel-man-red.png" width="70" height="72" alt=""/></td>
                                        <td><span class="mesto"><%#Eval( "RegisteredAddressCity" )%></span></td>
                                        <td style="width:110px;"><div class="blue-button" onclick='showPrijemEdit(<%#Eval("Id") %>)' style="width:100px;"><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, ATP_PrijmoutNovacka %>"></asp:Literal></div></td>
                                        <td style="width:100%;">
                                            <div id='edit_<%#Eval("Id") %>' class="edit" style="display:none;">
                                                <input type="text" id='code_<%#Eval("Id") %>' title="Registrační číslo" onclick='hideTooltip(<%#Eval("Id") %>)' />
                                                <div id='tooltip_<%#Eval("Id") %>' class="tooltip" style="position:absolute;margin-top:-20px;margin-left:5px;"onclick='hideTooltip(<%#Eval("Id") %>)'><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, ATP_RegistracniCislo %>"></asp:Literal></div>
                                                <asp:Button ID="btnOk" runat="server" Text="Ok" CssClass="button" OnClick="OnPrijmoutNovacka" CommandArgument='<%#Eval("Id") %>'/>
                                            </div>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate></table></FooterTemplate>
                            </asp:Repeater>    
                        </div>
                        <div class="prijati-novacci">
                            <span class="nadpis"><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, ATP_JizPrijatiDnesniNovacci %>"></asp:Literal></span>
                            <asp:Repeater runat="server" ID="rpPrijatiNovacci">
                                <HeaderTemplate><table border="0" style="width:100%;"></HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td style="width:50px;"><img id="Img1" runat="server" src="~/images/angel-man-green.png" width="50" height="49"/></td>
                                        <td><span class="mesto"><%#Eval( "RegisteredAddressCity" )%></span></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate></table></FooterTemplate>
                            </asp:Repeater>  
                        </div>
                        
                    </td>
                    <td style="width:250px;" valign="top" align="right">
                        <img runat="server" src="~/images/angel-vip.png" width="250" height="279" />
                    </td>
                </tr>
            </table>
        </div>
        <div style="border-top:2px solid #C10076;">
            <div style="margin:auto;width:100%;text-align:center;padding-top:10px;">
                <span style="color:#317cb4;font-size:14px;width:auto;">V.I.P ANGEL TEAM PROFESSIONAL</span>
            </div>
            <div>
				<table width="100%">
					<tr>
						<td align="center">
							<table>
								<tr>
									<td><div class='angel-man-yellow-ws'></div></td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td align="center">
							<table>
								<tr>
									<td><div class='angel-man-blue-ws'></div></td>
									<td><div class='angel-man-blue-ws'></div></td>
									<td><div class='angel-man-blue-ws'></div></td>
									<td><div class='angel-man-blue-ws'></div></td>
									<td><div class='angel-man-blue-ws'></div></td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td align="center">
							<table>
								<tr>
									<td><div class='angel-man-red-ws'></div></td>
									<td><div class='angel-man-red-ws'></div></td>
									<td><div class='angel-man-red-ws'></div></td>
									<td><div class='angel-man-red-ws'></div></td>
									<td><div class='angel-man-red-wsh'></div></td>
									<td><div class='angel-man-red-ws'></div></td>
									<td><div class='angel-man-red-ws'></div></td>
									<td><div class='angel-man-red-ws'></div></td>
									<td><div class='angel-man-red-ws'></div></td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td align="center">
							<table width="100%" border="0">
								<tr>
									<asp:Repeater runat="server" ID="rpPrihlaseniClenVIP">
										<ItemTemplate>
											<td align="center" valign="bottom"><div class='angel-man-blue'></div></td>
										</ItemTemplate>
									</asp:Repeater>
									<asp:Repeater runat="server" ID="rpPrihlaseniManagerVIP">
										<ItemTemplate>
											<td align="center" valign="bottom"><div class='angel-man-yellow'></div></td>
										</ItemTemplate>
									</asp:Repeater>
								</tr>
							</table>
						</td>
					</tr>
				</table>

            </div>
        </div>
    </div>
</asp:Content>
