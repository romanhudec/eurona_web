﻿<%@ Page Title="Potvrzení požadavku na přiřazení nováčka" Language="C#" MasterPageFile="~/user/operator/admin.master" AutoEventWireup="true" CodeBehind="anonymousAssign.aspx.cs" Inherits="Eurona.User.Operator.AnonymousAssignPage" %>
<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="A2" href="~/admin/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal2" runat="server" Text="Potvrzení požadavku na přiřazení nováčka" />
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
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
	<h2>Potvrzení požadavku na přiřazení nováčka</h2>
	<div>
		<asp:CheckBox runat="server" ID="cbShowAll" Text="Zobrazit všechny (standardne se zobrazuje jen předchozí měsíc)." OnCheckedChanged="OnCheckAllCheckedChanged" AutoPostBack="true" />
	</div>
    <asp:HiddenField runat="server" ID="hfRegistracniCislo" EnableViewState="true" />
	<asp:HiddenField runat="server" ID="hfJmenoSponzora" EnableViewState="true" />
    <div style="margin:20px;">
        <asp:Repeater runat="server" ID="rpCekajiciNovacci" OnItemDataBound="OnItemDataBound">
            <HeaderTemplate>
				<table border="0" cellpadding="0" cellspacing="0" class="dataGrid">
				<tr>
					<td class="dataGrid_headerStyle"></td>
					<td class="dataGrid_headerStyle">P.č.</td>
					<td class="dataGrid_headerStyle">Reg.číslo</td>
					<td class="dataGrid_headerStyle">Datum registrace</td>
					<td class="dataGrid_headerStyle">Člen ATP</td>
					<td class="dataGrid_headerStyle">Jmeno člena ATP</td>
					<td class="dataGrid_headerStyle">Uložení</td>
				</tr>
			</HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td style="width:50px;" rowspan="2"><img runat="server" src="~/images/angel-man-red.png" width="50" height="49" alt=""/></td>
					<td rowspan="2"><span><%#Eval("BankContactId")%></span></td>
					<td><span><%#Eval("Code")%></span></td>
					<td><span><%#Eval("AnonymousCreatedAt")%></span></td>
					<td><span><%#Eval("AnonymousAssignByCode")%></span></td>
					<td><span><%#this.GetOrganizationNameByCode(Eval("AnonymousAssignByCode").ToString())%></span></td>
                    <td rowspan="2">
                        <div id='edit_<%#Eval("Id") %>' class="edit">
                            <asp:Button ID="btnUlzit" runat="server" Text="Uložit" CssClass="button" OnClick="OnUlozit" CommandArgument='<%#Eval("Id") %>'/>
                        </div>
                    </td>
                </tr>
				<tr>
					<td colspan="4">
						<table>
							<tr>
								<td><asp:CheckBox Enabled="true" runat="server" ID="cbAnonymousOvereniSluzeb" Text="Ověření služeb" Checked='<%#Eval("AnonymousOvereniSluzeb") %>' /></td>
								<td><asp:CheckBox Enabled="true" runat="server" ID="cbAnonymousZmenaNaJineRegistracniCislo" Text="Změna na jiné reg.číslo" Checked='<%#Eval("AnonymousZmenaNaJineRegistracniCislo") %>' OnCheckedChanged="OnZmenaNaJineRegCislo" AutoPostBack="true" />&nbsp;
								<asp:TextBox  Enabled='<%#Convert.ToBoolean(Eval("AnonymousZmenaNaJineRegistracniCislo")) %>' runat="server" ID="txtAnonymousZmenaNaJineRegistracniCisloText" Text='<%#Eval("AnonymousZmenaNaJineRegistracniCisloText")%>' style="width:100px;" /></td>
							</tr>
							<tr>
								<td><asp:CheckBox Enabled="true" runat="server" ID="cbAnonymousSouhlasStavajicihoPoradce" Text="Souhlas stávajícího poradce" Checked='<%#Eval("AnonymousSouhlasStavajicihoPoradce") %>' /></td>
								<td><asp:CheckBox Enabled="true" runat="server" ID="cbAnonymousSouhlasNavrzenehoPoradce" Text="Souhlas navrženého poradce" Checked='<%#Eval("AnonymousSouhlasNavrzenehoPoradce") %>' /></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td colspan="7">
						<span class="message"><%#Eval("AnonymousAssignStatus")%></span>
					</td>
				</tr>
				<tr>
					<td colspan="7"><hr /></td>
				</tr>
            </ItemTemplate>
            <FooterTemplate></table></FooterTemplate>
        </asp:Repeater>    
    </div>
</asp:Content>
