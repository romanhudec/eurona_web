<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="Eurona.Admin.BKDefault" Codebehind="bkdefault.aspx.cs" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<%@ Register src="Controls/BKDetailVyrobkuControl.ascx" tagname="BKDetailVyrobkuControl" tagprefix="uc1" %>
<%@ Register src="Controls/BKOdeslatPriteliControl.ascx" tagname="BKOdeslatPriteliControl" tagprefix="uc2" %>
<%@ Register src="Controls/BKFacebookControl.ascx" tagname="BKFacebookControl" tagprefix="uc3" %>
<%@ Register src="Controls/BKHodnoceniVyrobkuControl.ascx" tagname="BKHodnoceniVyrobkuControl" tagprefix="uc4" %>

<%@ Register src="Controls/BKOnlineObjednavkaControl.ascx" tagname="BKOnlineObjednavkaControl" tagprefix="uc5" %>

<%@ Register src="Controls/BKRegistracePodrizenehoControl.ascx" tagname="BKRegistracePodrizenehoControl" tagprefix="uc6" %>
<%@ Register src="Controls/BKShareAkcniNabidkyFacebookControl.ascx" tagname="BKShareAkcniNabidkyFacebookControl" tagprefix="uc7" %>
<%@ Register src="Controls/BKShareSpecialniNabidkyFacebookControl.ascx" tagname="BKShareSpecialniNabidkyFacebookControl" tagprefix="uc8" %>
<%@ Register src="Controls/BKMaximalniPocetZaMesicControl.ascx" tagname="BKMaximalniPocetZaMesicControl" tagprefix="uc9" %>

<asp:Content ID="content" ContentPlaceHolderID="content" Runat="Server">
    
    <style type="text/css">
        .bkItem{margin:5px;}
    </style>
    <cms:RoundPanel ID="rp" runat="server" CssClass="roundPanel" Width="100%">
        <div style="margin-bottom:10px;">
            <h2><asp:Literal ID="Literal1" runat="server" Text="Nastavení bonusových kreditů" /></h2>
        </div>
        <div class="bkItem">
            <h3>Odeslání online objednávky</h3>
            <uc5:BKOnlineObjednavkaControl ID="BKOnlineObjednavkaControl1" runat="server" />
        </div>
        <div class="bkItem">
            <hr />        
            <h3>Odeslaní emailu s odkazem na výrobek pomocí "Poslat příteli"</h3> 
            <uc2:BKOdeslatPriteliControl ID="BKOdeslatPriteliControl1" runat="server" />
        </div>
        <div class="bkItem">
            <hr />        
            <h3>Odeslaní odkazu na zeď Facebook pomocí tlačítka "Facebook"</h3> 
            <uc3:BKFacebookControl ID="BKFacebookControl1" runat="server" />
        </div>
        <div class="bkItem">
            <hr />
            <h3>Hodnocení produktu</h3>  
            <uc4:BKHodnoceniVyrobkuControl ID="BKHodnoceniVyrobkuControl1" runat="server" />
        </div>
        <div class="bkItem">
            <hr />        
            <h3>Vstup na detail výrobku</h3>   
            <uc1:BKDetailVyrobkuControl ID="BKDetailVyrobkuControl2" runat="server" />
        </div>
    
        <div class="bkItem">
            <hr />        
            <h3>Registraci podřízeného s první objednávkou za min. 1500,- Kč</h3>   
            <uc6:BKRegistracePodrizenehoControl ID="BKRegistracePodrizenehoControl" runat="server" />
        </div>
        <div class="bkItem">
            <hr />        
            <h3>Sdílení akčních nabídek na "Facebook"</h3>   
            <uc7:BKShareAkcniNabidkyFacebookControl ID="BKShareAkcniNabidkyFacebookControl" runat="server" />
        </div>
        <div class="bkItem">
            <hr />        
            <h3>Sdílení speciálních nabídek na "Facebook"</h3>   
            <uc8:BKShareSpecialniNabidkyFacebookControl ID="BKShareSpecialniNabidkyFacebookControl" runat="server" />
        </div>
        <div class="bkItem">
            <hr />        
            <h3>Maximální počet BK za měsíc</h3>   
            <uc9:BKMaximalniPocetZaMesicControl ID="BKMaximalniPocetZaMesicControl" runat="server" />
        </div>
    </cms:RoundPanel>
</asp:Content>

