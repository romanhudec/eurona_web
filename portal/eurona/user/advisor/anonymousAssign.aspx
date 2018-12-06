<%@ Page Title="Potvrzení požadavku na přiřazení nováčka" Language="C#" MasterPageFile="~/user/advisor/page.master" AutoEventWireup="true" CodeBehind="anonymousAssign.aspx.cs" Inherits="Eurona.User.Advisor.AnonymousAssignPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script type="text/javascript">
	    function fnOnPageSubmit() {
	        blockUIProcessing('<%=Resources.Strings.Please_Wait%>');
	    }

        /*
	    $(function () {
	        var $allCheckbox = $('.rpCekajiciNovacci :checkbox');
	        var btnPotvrditVybrane = document.getElementById('%=this.btnPotvrditVybrane.ClientID %');
	        btnPotvrditVybrane.disabled = true;
	        $allCheckbox.change(function () {
	            if ($allCheckbox.is(':checked')) {
	                btnPotvrditVybrane.disabled = false;
	            }
	            else {
	                btnPotvrditVybrane.disabled = true;
	            }
	        });
	    });
        */

	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content_header" runat="server">
    <div class="content-header-container">
        <asp:Label CssClass="title" ID="lblTitle" runat="server" Text="Potvrzení požadavku na přiřazení nováčka" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
<%--    <asp:HiddenField runat="server" ID="hfRegistracniCislo" EnableViewState="true" />
	<asp:HiddenField runat="server" ID="hfJmenoSponzora" EnableViewState="true" />--%>
    <div style="margin:0px;" class="rpCekajiciNovacci">
        <asp:Repeater runat="server" ID="rpCekajiciNovacci" OnItemDataBound="OnItemDataBound">
            <HeaderTemplate>
				<table border="0" cellpadding="0" cellspacing="0" class="dataGrid">
				<tr>
					<td class="dataGrid_headerStyle"></td>
					<td class="dataGrid_headerStyle">P.č.</td>
					<td class="dataGrid_headerStyle">Reg.číslo</td>
					<td class="dataGrid_headerStyle">Datum registrace</td>
					<td class="dataGrid_headerStyle">Reg.č.sponzora</td>
					<td class="dataGrid_headerStyle">Jméno sponzora</td>
					<td class="dataGrid_headerStyle">Člen ATP</td>
					<td class="dataGrid_headerStyle">Jmeno člena ATP</td>
					<td class="dataGrid_headerStyle">Potvrzení</td>
				</tr>
			</HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td style="width:50px;" rowspan="2"><img runat="server" src="~/images/angel-man-red.png" width="50" height="49" alt=""/></td>
					<td rowspan="2"><span><%#Eval("BankContactId")%></span></td>
					<td><span><%#Eval("Code")%></span></td>
					<td><span><%#Eval("AnonymousCreatedAt")%></span></td>
					<td>
						<asp:TextBox id='txtCode' runat="server" title="Registrační číslo" Text='<%#Eval("AnonymousAssignToCode")%>' style="width:100px;" />
					</td>
					<td>
						<asp:TextBox id='txtCodeJmeno' runat="server" title="Jméno sponzora" Text='<%#Eval("AnonymousAssignToCode")%>' style="width:100px;" />
					</td>
					<td><span><%#Eval("AnonymousAssignByCode")%></span></td>
					<td><span><%#this.GetOrganizationNameByCode(Eval("AnonymousAssignByCode").ToString())%></span></td>
                    <td rowspan="2">
                        <div id='edit_<%#Eval("Id") %>' class="edit">
                            <asp:Button ID="btnPotvrdit" runat="server" Text="Potvrdit" CssClass="button" OnClick="OnPotvrditPrijeti" CommandArgument='<%#Eval("Id") %>'/>
                        </div>
                    </td>
                </tr>
				<tr>
					<td colspan="6">
						<table>
							<tr>
								<td><asp:CheckBox Enabled="false" runat="server" ID="cbAnonymousOvereniSluzeb" Text="Ověření služeb" Checked='<%#Eval("AnonymousOvereniSluzeb") %>' CommandArgument='<%#Eval("Id") %>'/></td>
								<td><asp:CheckBox Enabled="false" runat="server" ID="cbAnonymousZmenaNaJineRegistracniCislo" Text="Změna na jiné reg.číslo" Checked='<%#Eval("AnonymousZmenaNaJineRegistracniCislo") %>' />&nbsp;
								<asp:TextBox  Enabled="false" runat="server" ID="txtAnonymousZmenaNaJineRegistracniCisloText" Text='<%#Eval("AnonymousZmenaNaJineRegistracniCisloText")%>' style="width:100px;" /></td>
							</tr>
							<tr>
								<td><asp:CheckBox Enabled="false" runat="server" ID="cbAnonymousSouhlasStavajicihoPoradce" Text="Souhlas stávajícího poradce" Checked='<%#Eval("AnonymousSouhlasStavajicihoPoradce") %>' /></td>
								<td><asp:CheckBox Enabled="false" runat="server" ID="cbAnonymousSouhlasNavrzenehoPoradce" Text="Souhlas navrženého poradce" Checked='<%#Eval("AnonymousSouhlasNavrzenehoPoradce") %>' /></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td colspan="9">
						<span class="message"><%#Eval("AnonymousAssignStatus")%></span>
					</td>
				</tr>
				<tr>
					<td colspan="9"><hr /></td>
				</tr>
            </ItemTemplate>
            <FooterTemplate></table></FooterTemplate>
        </asp:Repeater>  
        <table width="100%" cellpadding="0", cellspacing="0">
            <tr>
                <td align="right">
                    <asp:Button ID="btnPotvrditVybrane" runat="server" Text="Potvrdit změny hromadně" CssClass="button" Enabled="false" OnClick="OnPotvrditPrijetiVybrane"/>  
                </td>
            </tr>
        </table>        
    </div>
</asp:Content>
