<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeBehind="nastaveniAnonymniRegistrace.aspx.cs" Inherits="Eurona.Admin.EuronaNastaveniAnonymniRegistrace" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="server">
    <cms:RoundPanel ID="rp" runat="server" CssClass="roundPanel" Width="100%">
		<div>
			<h2>Nastavení nových registrací</h2>
            <hr />
			<span>Nové registrace pod EURONA (procent):&nbsp;</span><asp:TextBox runat="server" ID="txtEuronaReistraceProcent"></asp:TextBox>
		</div>
        <h2>Nastavení nezařazených nováčků</h2>
        <hr />
        <table>
            <tr>
                <td colspan="6">
                    <h3>Zobrazit nováčky v seznamu na zařazení</h3>
                </td>
            </tr>
            <tr>
                <td colspan="6"><asp:CheckBox runat="server" ID="cbNeomezene" AutoPostBack="true" OnCheckedChanged="OnNeomezeneChecked" Text="Zobrazit v seznamu neomezene dlouho." /></td>
            </tr>
            <tr runat="server" id="trObdobi">
                <td colspan="6">
                    <div>
		            Interval od&nbsp;
                    <asp:DropDownList runat="server" ID="ddlZobrazitVSeznamuLimitFrom"></asp:DropDownList>&nbsp;
                    Hodin : <asp:TextBox runat="server" ID="txtZobrazitVSeznamuLimitFromHodin" Width="40px"></asp:TextBox>
                    Minut : <asp:TextBox runat="server" ID="txtZobrazitVSeznamuLimitFromMinut" Width="40px"></asp:TextBox>
                    &nbsp;&nbsp;do&nbsp;
                    <asp:DropDownList runat="server" ID="ddlZobrazitVSeznamuLimitTo"></asp:DropDownList>
                    Hodin : <asp:TextBox runat="server" ID="txtZobrazitVSeznamuLimitToHodin" Width="40px"></asp:TextBox>
                    Minut : <asp:TextBox runat="server" ID="txtZobrazitVSeznamuLimitToMinut" Width="40px"></asp:TextBox>
                    <asp:Button runat="server" Text="Přidat" ID="Button1" OnClick="OnPridatZobrazitVSeznamuLimit" />
		            <telerik:RadGrid AutoGenerateColumns="False" ID="gridViewZobrazitVSeznamuLimit" AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" runat="server" OnItemCommand="OnZobrazitVSeznamuLimitRowCommand">
			            <PagerStyle Mode="NextPrevAndNumeric" />
			            <GroupingSettings CaseSensitive="false" />
			            <MasterTableView TableLayout="Fixed" PageSize="50" DataKeyNames="Id">
				            <Columns>
					            <telerik:GridBoundColumn HeaderText="Interval" DataField="DisplayString" UniqueName="DisplayString"
						            SortExpression="DisplayString" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
					            <telerik:GridTemplateColumn ShowFilterIcon="false" AllowFiltering="false" ItemStyle-HorizontalAlign="Right">
						            <ItemTemplate>
							            <asp:LinkButton ID="LinkButton1" runat="server" Text="Odebrat" CommandArgument='<%#Eval("DisplayString") %>' CommandName="DELETE_ITEM" OnClientClick="return window.confirm('Skutečne si přejete odebrat tohoto poradce?');"></asp:LinkButton>
						            </ItemTemplate>
					            </telerik:GridTemplateColumn>
				            </Columns>
			            </MasterTableView>
		            </telerik:RadGrid>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <h3>Maximální počet přijatých nováčků</h3>
                </td>
            </tr>
            <tr>
                <td>Počet:</td>
                <td colspan="5"><asp:TextBox runat="server" ID="txtMaxPocetPrijetychNovacku"></asp:TextBox></td>
            </tr>
        </table>
		<div>
			<h2>Uživatelé, kteří mají právo potvrzovat nezařazené nováčky</h2>
            <hr />
		</div>
		Registrační číslo poradce : <asp:TextBox runat="server" ID="txtCode"></asp:TextBox>&nbsp;<asp:Button runat="server" Text="Přidat" ID="btnAdd" OnClick="OnPridatAnonymousManager" />
		<telerik:RadGrid AutoGenerateColumns="False" ID="gridView" AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" runat="server" OnItemCommand="OnRowCommand">
			<PagerStyle Mode="NextPrevAndNumeric" />
			<GroupingSettings CaseSensitive="false" />
			<MasterTableView TableLayout="Fixed" PageSize="50" DataKeyNames="Id">
				<Columns>
					<telerik:GridHyperLinkColumn HeaderText="Reg. číslo" DataTextField="Code" DataNavigateUrlFields="AccountId"  UniqueName="Code" ItemStyle-Wrap="false" DataNavigateUrlFormatString="~/user/operator/user.aspx?id={0}&ReturnUrl=/user/operator/anonymousAccounts.aspx"
						SortExpression="Code" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
					<telerik:GridBoundColumn HeaderText="Jméno" DataField="Name" UniqueName="Name"
						SortExpression="Name" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
					<telerik:GridHyperLinkColumn HeaderText="Email" DataTextField="ContactEmail" UniqueName="ContactEmail"  DataNavigateUrlFields="ContactEmail" DataNavigateUrlFormatString="mailto:{0}"
						SortExpression="ContactEmail" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
					<telerik:GridTemplateColumn ShowFilterIcon="false" AllowFiltering="false" ItemStyle-HorizontalAlign="Right">
						<ItemTemplate>
							<asp:LinkButton runat="server" Text="Odebrat" CommandArgument='<%#Eval("Id") %>' CommandName="DELETE_ITEM" OnClientClick="return window.confirm('Skutečne si přejete odebrat tohoto poradce?');"></asp:LinkButton>
						</ItemTemplate>
					</telerik:GridTemplateColumn>
				</Columns>
			</MasterTableView>
		</telerik:RadGrid>
        <div>
            <table>
            <tr>
                <td>
                    <asp:Button runat="server" ID="btnSave" Text="Uložit" OnClick="OnSave" />
                    <asp:Button runat="server" ID="btnCancel" Text="Zrušit" OnClick="OnCancel" CausesValidation="false" />
                </td>
            </tr>
            </table>
        </div>
    </cms:RoundPanel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
</asp:Content>
