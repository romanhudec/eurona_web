<%@ Page Title="Potvrzení požadavku na přiřazení nováčka" Language="C#" MasterPageFile="~/user/advisor/page.master" AutoEventWireup="true" CodeBehind="anonymousAssign.aspx.cs" Inherits="Eurona.User.Advisor.AnonymousAssignPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script type="text/javascript">
		function validatePrijemNovacka(id) {
			var elmCode = document.getElementById('code_' + id.toString());
			var elmJmeno = document.getElementById('code_jmeno_' + id.toString());
			if (elmCode.value.length == 0 && elmJmeno.value.length == 0) return false;

			var elmHfCode = document.getElementById('<%=this.hfRegistracniCislo.ClientID %>');
			elmHfCode.value = elmCode.value;

			var elmHfJmeno = document.getElementById('<%=this.hfJmenoSponzora.ClientID %>');
			elmHfJmeno.value = elmJmeno.value;
			return true;
		}
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
    <asp:HiddenField runat="server" ID="hfRegistracniCislo" EnableViewState="true" />
	<asp:HiddenField runat="server" ID="hfJmenoSponzora" EnableViewState="true" />
    <div style="margin:0px;">
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
						<input type="text" id='code_<%#Eval("Id") %>' title="Registrační číslo" value='<%#Eval("AnonymousAssignToCode")%>' style="width:100px;" />
					</td>
					<td>
						<input type="text" id='code_jmeno_<%#Eval("Id") %>' title="Jméno sponzora" value='<%#Eval("AnonymousAssignToCode")%>' style="width:100px;" />
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
								<td><asp:CheckBox Enabled="false" runat="server" ID="cbAnonymousOvereniSluzeb" Text="Ověření služeb" Checked='<%#Eval("AnonymousOvereniSluzeb") %>' /></td>
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
    </div>
</asp:Content>
