<%@ Page Title="<%$ Resources:Strings, Navigation_SearchResult %>" Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" Inherits="Eurona.SearchResultPage" Codebehind="search.aspx.cs" %>

<%@ Register assembly="cms" namespace="CMS.Controls.SearchEngine" tagprefix="cmsSearchEngine" %>
<%@ Register assembly="shp" namespace="SHP.Controls.SearchEngine" tagprefix="shpSearchEngine" %>

<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
	<shpSearchEngine:SearchEngineResultControl ID="searchEngineResultControl" CssClass="searchResultControl" runat="server" Width="100%">
	<%--<SearcheTemplate>
	    <div class="seacrh_title">
			<%# (Container.DataItem as CMS.Entities.XX).Title %>
	    </div>
	</SearcheTemplate>--%>
	</shpSearchEngine:SearchEngineResultControl>
</asp:Content>

