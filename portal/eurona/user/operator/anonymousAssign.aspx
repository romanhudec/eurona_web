<%@ Page Title="Potvrzení požadavku na přiřazení nováčka" Language="C#" MasterPageFile="~/user/operator/admin.master" AutoEventWireup="true" CodeBehind="anonymousAssign.aspx.cs" Inherits="Eurona.User.Operator.AnonymousAssignPage" %>
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
	    function fnOnPageSubmit() {
	        blockUIProcessing('<%=Resources.Strings.Please_Wait%>');
        }
        
        /*
	    $(function () {
            var $allCheckbox = $('.rpCekajiciNovacci :checkbox');
        	var btnUlozitVybrane = document.getElementById('%=this.btnUlozitVybrane.ClientID %');
	        btnUlozitVybrane.disabled = true;
	        $allCheckbox.change(function () {
	            if ($allCheckbox.is(':checked')) {
	                btnUlozitVybrane.disabled = false;
	            }
	            else {
	                btnUlozitVybrane.disabled = true;
	            }
	        });
	    });*/
	</script>
	<h2>Potvrzení požadavku na přiřazení nováčka</h2>
	<div>
		<asp:CheckBox runat="server" ID="cbShowAll" Text="Zobrazit všechny (standardne se zobrazuje jen předchozí měsíc)." OnCheckedChanged="OnCheckAllCheckedChanged" AutoPostBack="true" />
	</div>
    <%--<asp:HiddenField runat="server" ID="hfRegistracniCislo" EnableViewState="true" />
	<asp:HiddenField runat="server" ID="hfJmenoSponzora" EnableViewState="true" />--%>
    <div style="margin:20px;" class="rpCekajiciNovacci">
        <asp:Repeater runat="server" ID="rpCekajiciNovacci">
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
                           <%-- <asp:CheckBox runat="server" ID="cbxVybrat" Text="Vybrat" CommandArgument='<%#Eval("Id") %>' />--%>
                        </div>
                    </td>
                </tr>
				<tr>
					<td colspan="4">
						<table>
							<tr>
								<td><asp:CheckBox Enabled="true" runat="server" ID="cbAnonymousOvereniSluzeb" Text="Ověření služeb" Checked='<%#Eval("AnonymousOvereniSluzeb") %>' CommandArgument='<%#Eval("Id") %>'/></td>
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
        <table width="100%" cellpadding="0", cellspacing="0">
            <tr>
                <td align="right">
                    <div>
                        <span>Ukládají se pouze záznamy, které mají zaškrtnuto "Ověření služeb"</span>
                    </div>
                    <asp:Button ID="btnUlozitVybrane" runat="server" Text="Uložit změny hromadně" CssClass="button" Enabled="false" OnClick="OnPotvrditPrijetiVybrane"/>  
                </td>
            </tr>
        </table>    
    </div>
</asp:Content>
