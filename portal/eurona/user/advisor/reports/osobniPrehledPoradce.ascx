<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="osobniPrehledPoradce.ascx.cs" Inherits="Eurona.user.advisor.reports.osobniPrehledPoradce" %>
<style>
.reporttable
	{
		text-align: left;
		margin: 20px;
		border: solid 2px #000000;
		border-collapse: collapse;
		background-color: white;
	}

	.reporttable TH
	{
		color: #635fa6;
		background-color: #e6f4fc;
		text-align: center;
		font-weight: bold;
	}

	.reporttable TD
	{
		border: solid 1px #000000;
		color: #000000;
		background-color: #ffffff;
		font-weight: normal;
	}
.stonetable
	{
		margin: 20px;
		border-collapse: collapse;
		background-color: white;
	}

	.stonetable TH
	{
		padding: 6px;
		color: #635fa6;
		background-color: #e6f4fc;
		text-align: center;
		font-weight: bold;
	}

	.stonetable_firstrow
	{
		background-color: #94bfea;
		height: 10px;
	}
	
	.stonetable_TD
	{
        margin: 0 auto;
		padding: 10px;
		color: #001b98;
		background-color: #eff1fa;
		background-image: url(../../../images/reports/stonetable.jpg);
		text-align: center!important;
        vertical-align:top;
		font-weight: normal;
	}
	

	.credittable
	{
		text-align: left;
		margin: 20px;
		border-collapse: collapse;
		background-image: url(../../../images/reports/credittable.jpg);
	}
	
	.exporttoexcel{display:none;}
	.print{display:none;}
</style>
<asp:FormView ID="formActivity" runat="server"  Width="100%" RowStyle-HorizontalAlign="Center">
	<ItemTemplate>			
		<table border="0">
		<tr>
		<td valign="top">
			<table class="reporttable" border="0" cellpadding="6" cellspacing="0">
			<tr><th colspan="2"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Reports, OsobniAktivita %>"></asp:Literal></td></tr>
			<tr>
				<td>BO vl.</td>
				<td align="right"><%# Eval( "Body_vlastni" )%></td>
			</tr>
			<tr>
				<td>OO vl.</td>
				<td align="right"><%# Eval("Objem_vlastni", "{0:0.00}")%></td>
			</tr>
			<tr>
				<td>BO os. skupiny</td>
				<td align="right"><%# Eval("Body_os")%></td>
			</tr>
			<tr>
				<td>OO os. skupiny (v Kč)</td>
				<td align="right"><%# Eval( "Objem_os", "{0:0.00}" )%></td>
			</tr>
			<tr>
				<td>BO skupiny</td>
				<td align="right"><%# Eval( "Body_celkem" )%></td>
			</tr>
			<tr>
				<td>OO skupiny (v Kč)</td>
				<td align="right"><%# Eval( "Objem_celkem", "{0:0.00}" )%></td>
			</tr>
			<tr>
				<td><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Reports, VyplacenaProvizeVMene %>"></asp:Literal>(v Kč<%--<%# Eurona.User.Advisor.Reports.ReportHelper.PriceCurrency( Eval( "Provize_vyplata_kod_meny" ) )%>--%>)</td>
				<td align="right"><%# Eval( "Provize_vyplata", "{0:0.00}" )%></td>
			</tr>
			<tr>
				<td><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Reports, ProcentualniHladina %>"></asp:Literal></td>
				<td align="right"><%# Eval("Hladina")%></td>
			</tr>					
			</table>
		</td>
		<td valign="top">
			<table class="reporttable" border="0" cellpadding="6" cellspacing="0">
			<tr><th colspan="2">aktivita</td></tr>
			<tr>
				<td><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Reports, ProvizeVAktualnimMesiciZVLObjemu %>"></asp:Literal></td>
				<td align="right"><%# Eval( "Provize_vlastni", "{0:0.00}" )%></td>
			</tr>
			<tr>
				<td><asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:Reports, ProvizeVAktualnimMesiciZeSkupiny %>"></asp:Literal></td>
				<td align="right"><%# Eval( "Provize_skupina", "{0:0.00}" )%></td>
			</tr>
			<tr>
				<td><asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:Reports, BodyDoDosazeniDalsiUrovne %>"></asp:Literal></td>
				<td align="right"><%# Eurona.User.Advisor.Reports.ReportHelper.RestPoints( Eval( "Body_celkem" ) )%></td>
			</tr>
			<tr>
				<td><asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:Reports, DalsiUroven %>"></asp:Literal></td>
				<td align="right"><%# Eurona.User.Advisor.Reports.ReportHelper.NextLevel( Eval( "Body_celkem" ) )%></td>
			</tr>
			</table>
		</td>
		</tr>
		</table>
	
		<br />
		<h3><asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:Reports, VaseDiamantovaCesta %>"></asp:Literal></h3>

		<table>
		<tr>
		<td valign="top">
		    <table class="reporttable" border="0" cellpadding="6" cellspacing="0">
		    <tr><th colspan="3">bonusy</td></tr>
		    <tr>
			    <td>
			        <img id="Img1" src="~/images/reports/drahokam_eurona.gif" runat="server" />
			    </td>
			    <td>eurona bonus</td>
			    <td align="right"><%# Eval( "Bonus1", "{0:0.00}" )%>&nbsp;<%# Eurona.User.Advisor.Reports.ReportHelper.PriceCurrency( Eval( "Kod_meny" ) )%></td>
		    </tr>
		    <tr>
			    <td>
			        <img id="Img3" src="~/images/reports/drahokam_rubin.gif" runat="server" />
			    </td>
			    <td>rubínový bonus</td>
			    <td align="right"><%# Eval( "Bonus2", "{0:0.00}" )%>&nbsp;<%# Eurona.User.Advisor.Reports.ReportHelper.PriceCurrency( Eval( "Kod_meny" ) )%></td>
		    </tr>
		    <tr>
			    <td>
			        <img id="Img5" src="~/images/reports/drahokam_safir.gif" runat="server" />
			    </td>
			    <td>safírový bonus</td>
			    <td align="right"><%# Eval( "Bonus3", "{0:0.00}" )%>&nbsp;<%# Eurona.User.Advisor.Reports.ReportHelper.PriceCurrency( Eval( "Kod_meny" ) )%></td>
		    </tr>
		    <tr>
			    <td>
			        <img id="Img15" src="~/images/reports/drahokam_briliant.gif" runat="server" />
			    </td>
			    <td>briliantový bonus</td>
			    <td align="right"><%# Eval( "Bonus4", "{0:0.00}" )%>&nbsp;<%# Eurona.User.Advisor.Reports.ReportHelper.PriceCurrency( Eval( "Kod_meny" ) )%></td>
		    </tr>
		    <tr>
				<td></td>
			    <td><asp:Literal ID="Literal9" runat="server" Text="<%$ Resources:Reports, SoucetBonusu %>"></asp:Literal></td>
			    <td align="right"><%# Eurona.User.Advisor.Reports.ReportHelper.SumCredit( Eval( "Bonus1" ), Eval( "Bonus2" ), Eval( "Bonus3" ), Eval( "Bonus4" ) )%>&nbsp;<%# Eurona.User.Advisor.Reports.ReportHelper.PriceCurrency( Eval( "Kod_meny" ) )%></td>
		    </tr>
		    </table>				
		</td>
		<td valign="top">
		    <table class="stonetable" cellpadding="6" cellspacing="0" style="width:450px;">
		    <tr>
				<td colspan="7" class="stonetable_firstrow"></td>
		    </tr>
		    <tr>
				<th colspan="7"><asp:Literal ID="Literal10" runat="server" Text="<%$ Resources:Reports, AktualnyNarokBonusu %>"></asp:Literal></th>
		    </tr>
	        <tr>
                <td class="stonetable_TD" align="center">
				    <b style="text-align:center;">EB</b>
                     <div style="padding-top:10px;">
                        <img id="Img7" src="~/images/reports/animace_eurona.gif" runat="server" visible='<%# Eurona.User.Advisor.Reports.ReportHelper.GreaterThanZero(Eval("Bonus1")) %>' title="eurona bonus" />
                        <img id="Img8" src="~/images/reports/animace_seda.gif" runat="server" visible='<%# !Eurona.User.Advisor.Reports.ReportHelper.GreaterThanZero(Eval("Bonus1")) %>' title="eurona bonus" />
                    </div>
                </td>
                <td class="stonetable_TD">
				    <b>RB</b>
                    <div style="padding-top:10px;">
                        <img id="Img9" src="~/images/reports/animace_rubin.gif" runat="server" visible='<%# Eurona.User.Advisor.Reports.ReportHelper.ShowBonusRubin(3, Eval("BonusRubinHladina")) %>' title="rubínový bonus" />
                        <img id="Img10" src="~/images/reports/animace_seda.gif" runat="server" visible='<%# !Eurona.User.Advisor.Reports.ReportHelper.ShowBonusRubin(3, Eval("BonusRubinHladina")) %>' title="rubínový bonus" />
                    </div>
                </td>
                <td class="stonetable_TD">
				    <b>SB</b>
                     <div style="padding-top:10px;">
                        <img id="Img11" src="~/images/reports/animace_safir.gif" runat="server" visible='<%# Eurona.User.Advisor.Reports.ReportHelper.ShowBonusRubin(4, Eval("BonusRubinHladina")) %>' title="safírový bonus" />
                        <img id="Img12" src="~/images/reports/animace_seda.gif" runat="server" visible='<%# !Eurona.User.Advisor.Reports.ReportHelper.ShowBonusRubin(4, Eval("BonusRubinHladina")) %>' title="safírový bonus" />
                    </div>
                </td>
                <td class="stonetable_TD">
				    <b>BB</b>
                    <div style="padding-top:10px;">
                        <img id="Img13" src="~/images/reports/animace_briliant.gif" runat="server" visible='<%# Eurona.User.Advisor.Reports.ReportHelper.ShowBonusRubin(5, Eval("BonusRubinHladina")) %>' title="briliantový bonus" />
                        <img id="Img14" src="~/images/reports/animace_seda.gif" runat="server" visible='<%# !Eurona.User.Advisor.Reports.ReportHelper.ShowBonusRubin(5, Eval("BonusRubinHladina")) %>' title="briliantový bonus" />
                    </div>
                </td>
                <td class="stonetable_TD">
				    <b>2RB</b>
                     <div>
                        <img id="Img6" src="~/images/reports/animace_2rubin.gif" runat="server" visible='<%# Eurona.User.Advisor.Reports.ReportHelper.ShowBonusRubin(6, Eval("BonusRubinHladina")) %>' title="rubínový bonus" />
                        <img id="Img16" src="~/images/reports/animace_2seda.gif" runat="server" visible='<%# !Eurona.User.Advisor.Reports.ReportHelper.ShowBonusRubin(6, Eval("BonusRubinHladina")) %>' title="rubínový bonus" />
                    </div>
                </td>
                <td class="stonetable_TD">
				    <b style="text-align:center;">2SB</b>
                     <div>
                        <img id="Img17" src="~/images/reports/animace_2safir.gif" runat="server" visible='<%# Eurona.User.Advisor.Reports.ReportHelper.ShowBonusRubin(7, Eval("BonusRubinHladina")) %>' title="safírový bonus" />
                        <img id="Img18" src="~/images/reports/animace_2seda.gif" runat="server" visible='<%# !Eurona.User.Advisor.Reports.ReportHelper.ShowBonusRubin(7, Eval("BonusRubinHladina")) %>' title="safírový bonus" />
                    </div>
                </td>
                <td class="stonetable_TD" align="center">
				   <b style="text-align:center;">2BB</b>
                    <div>
                        <img id="Img19" src="~/images/reports/animace_2briliant.gif" runat="server" visible='<%# Eurona.User.Advisor.Reports.ReportHelper.ShowBonusRubin(8, Eval("BonusRubinHladina")) %>' title="briliantový bonus" />
                        <img id="Img20" src="~/images/reports/animace_2seda.gif" runat="server" visible='<%# !Eurona.User.Advisor.Reports.ReportHelper.ShowBonusRubin(8, Eval("BonusRubinHladina")) %>' title="briliantový bonus" />
                    </div>
                </td>
	        </tr>
	        <tr>
                <td class="stonetable_TD">
                </td>
                <td class="stonetable_TD">
                </td>
                <td class="stonetable_TD">
                </td>
                <td class="stonetable_TD">
                </td>
                <td class="stonetable_TD">
                </td>
                <td class="stonetable_TD">
                </td>
                <td class="stonetable_TD">
                </td>
	        </tr>
		    </table>
		</td>
		</tr>
		</table>	
		
		<table>
		<tr>
		<td valign="top">
		    <table class="credittable_">
		    <tr>
		    <td style="display:none;">
			    <table class="reporttable" border="0" cellpadding="3" cellspacing="0">
			    <tr>
                    <th colspan="3">kredity</th>
                </tr>
			    <tr>
				    <td><asp:Literal ID="Literal11" runat="server" Text="<%$ Resources:Reports, KreditZiskanyVMesici %>"></asp:Literal></td>
				    <td align="right"><%# Eval("ecredit") %></td>
				    <td rowspan="5"><img src="~/images/reports/pocket.jpg" runat="server" /></td>
			    </tr>
			    <tr>
				    <td><asp:Literal ID="Literal12" runat="server" Text="<%$ Resources:Reports, AktualniZbyvajiciKreditProCerpani %>"></asp:Literal></td>
				    <td align="right"><%# Eval( "Narok_eurokredit" )%></td>
			    </tr>
    			
			    <tr>
				    <td>Zisk EURO</td>
				    <td align="right">
                        <%# Eurona.User.Advisor.Reports.ReportHelper.ActualCredit( Eval( "Objem_vlastni" ), Eval( "Kod_meny" ) )%>
					    <asp:Repeater ID="Repeater3" runat="server" DataSourceID="sqlRestMarginPrice" >
						    <ItemTemplate>
							    <%# Eurona.User.Advisor.Reports.ReportHelper.ActualCredit( Eval( "cena_mj_katalogova" ), Eval( "Kod_meny" ) )%>
						    </ItemTemplate>
					    </asp:Repeater>
				    </td>
			    </tr>
			    <tr>
				    <td><asp:Literal ID="Literal13" runat="server" Text="<%$ Resources:Reports, ProZiskaniDalsihoEuraZbyvaObjednatZa %>"></asp:Literal></td>
                     <%--   <%# Eurona.User.Advisor.Reports.ReportHelper.RestMarginCredit( Eval( "Objem_vlastni" ), Eval( "Kod_meny" ) )%>
						<%# Eurona.User.Advisor.Reports.ReportHelper.PriceCurrency( Eval( "Kod_meny" ) )%>			--%>		
				    <td align="right">
					    <asp:Repeater ID="Repeater4" runat="server" DataSourceID="sqlRestMarginPrice" >
						    <ItemTemplate>
							    <%# Eurona.User.Advisor.Reports.ReportHelper.RestMarginCredit( Eval( "cena_mj_katalogova" ), Eval( "Kod_meny" ) )%>
							    <%# Eurona.User.Advisor.Reports.ReportHelper.PriceCurrency( Eval( "Kod_meny" ) )%>						
						    </ItemTemplate>
					    </asp:Repeater>
				    </td>
			    </tr>		
 
			    </table>
			</td>
			   
			<td>
				<%--<img id="Img2" src="~/images/reports/eurocredit.gif" runat="server" />
				<br />--%>
				<table class="reporttable" border="0" cellpadding="6" cellspacing="0">
				<tr>
					<td><asp:Literal ID="Literal14" runat="server" Text="<%$ Resources:Reports, CelkemPoradcuVeSkupine %>"></asp:Literal></td>
					<td align="right">
						<asp:Repeater ID="repTotalGroup" runat="server" DataSourceID="sqlTotalGroup" >
							<ItemTemplate>
								<%# Eval("usercount")%>
							</ItemTemplate>
						</asp:Repeater>
					</td>
				</tr>
				<tr>
					<td><asp:Literal ID="Literal15" runat="server" Text="<%$ Resources:Reports, CelkemPoradcuVOsobniSkupine %>"></asp:Literal></td>
					<td align="right">
						<asp:Repeater ID="repBonusGroup" runat="server" DataSourceID="sqlBonusGroup" >
							<ItemTemplate>
								<%# Eval("usercount")%>
							</ItemTemplate>
						</asp:Repeater>
					</td>
				</tr>
				<tr>
					<td><asp:Literal ID="Literal16" runat="server" Text="<%$ Resources:Reports, CelkemPoradcuV1Linii %>"></asp:Literal></td>
					<td align="right">
						<asp:Repeater ID="repDirectGroup" runat="server" DataSourceID="sqlDirectGroup" >
							<ItemTemplate>
								<%# Eval("usercount")%>
							</ItemTemplate>
						</asp:Repeater>
					</td>
				</tr>
				</table>
			</td>
			   
			</tr>
			</table>
		</td>
		</tr>
		</table>
		
		<table>
		<tr>
		<td>
			<table class="reporttable" border="0" cellpadding="6" cellspacing="0">
			<tr>
			<th colspan="2"><asp:Literal ID="Literal17" runat="server" Text="<%$ Resources:Reports, Marze %>"></asp:Literal></th>
			</tr>
			<tr>
				<td><asp:Literal ID="Literal18" runat="server" Text="<%$ Resources:Reports, MarzeNaTentoMesic %>"></asp:Literal></td>
				<td align="right"><%# Eval( "Marze_platna", "{0:0.00}" )%></td>
			</tr>
			<tr>
				<td><asp:Literal ID="Literal19" runat="server" Text="<%$ Resources:Reports, PredbeznyNarokNaMarziProPristiMesic %>"></asp:Literal></td>
				<td align="right"><%# Eval( "Marze_nasledujici", "{0:0.00}" )%></td>
			</tr>
			<tr>
				<td><asp:Literal ID="Literal20" runat="server" Text="<%$ Resources:Reports, VasNevycerpanyNarokNaSlevuNRZ %>"></asp:Literal></td>
				<td align="right"><%# Eval( "Eurokredit_vlastni", "{0:0.00}" )%></td>
			</tr>
			<tr>
				<td><asp:Literal ID="Literal21" runat="server" Text="<%$ Resources:Reports, VasNevycerpanyNarokNaSlevuKeKonciMinulehoMesice %>"></asp:Literal></td>
				<td align="right"><%# Eval( "Narok_sleva_nrz", "{0:0.00}" )%></td>
			</tr>
			<tr>
				<td><asp:Literal ID="Literal22" runat="server" Text="<%$ Resources:Reports, ProDosazeni25MarzeZbyvaObjednat %>"></asp:Literal></td>
				<td align="right">
					<asp:Repeater ID="Repeater1" runat="server" DataSourceID="sqlRestMarginPrice" >
						<ItemTemplate>
                            <%# Eurona.User.Advisor.Reports.ReportHelper.RestMarginBody( 100, Eval( "objem_pro_marzi" ))%>					
						</ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblEmptyData" runat="server" Visible='<%# ((Repeater)Container.NamingContainer).Items.Count == 0 %>' Text="100"  ForeColor="Black"/>
                        </FooterTemplate>
						<SeparatorTemplate><br /></SeparatorTemplate>
					</asp:Repeater>
				</td>
			</tr>		
			<tr>
				<td><asp:Literal ID="Literal23" runat="server" Text="<%$ Resources:Reports, ProDosazeni30MarzeZbyvaObjednat %>"></asp:Literal></td>
				<td align="right">
					<asp:Repeater ID="Repeater2" runat="server" DataSourceID="sqlRestMarginPrice" >
						<ItemTemplate>
                            <%# Eurona.User.Advisor.Reports.ReportHelper.RestMarginBody( 200, Eval( "objem_pro_marzi" ))%>		
						</ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblEmptyData" runat="server" Visible='<%# ((Repeater)Container.NamingContainer).Items.Count == 0 %>' Text="200" ForeColor="Black" />
                        </FooterTemplate>
						<SeparatorTemplate><br /></SeparatorTemplate>
					</asp:Repeater>
				</td>
			</tr>
			</table>
		</td>
		</tr>
		</table>		
		
		<table>
		<tr>
		<td>
			<table class="reporttable" border="0" cellpadding="6" cellspacing="0">
			<tr><th colspan="2"><asp:Literal ID="Literal24" runat="server" Text="<%$ Resources:Reports, Advisor %>"></asp:Literal></th></tr>
			<tr>
				<td><asp:Literal ID="Literal25" runat="server" Text="<%$ Resources:Reports, KodPoradce %>"></asp:Literal></td>
				<td><%# Eval("Kod_odberatele") %></td>
			</tr>
			<tr>
				<td><asp:Literal ID="Literal26" runat="server" Text="<%$ Resources:Reports, Jmeno %>"></asp:Literal></td>
				<td><%# Eval( "Nazev_firmy" )%></td>
			</tr>
			<tr>
				<td>adresa</td>
				<td><%# Eval( "Adresa" )%></td>
			</tr>
			<tr>
				<td>email</td>
				<td><a href='<%# "mailto:" + Eval("E_mail")%>'><%# Eval( "E_mail" )%></a></td>
			</tr>
			<tr>
				<td><asp:Literal ID="Literal29" runat="server" Text="<%$ Resources:Reports, MobilniTelefon %>"></asp:Literal></td>
				<td><%# Eval( "Mobil" )%></td>
			</tr>
			</table>
		</td>
		</tr>
		</table>
			
	</ItemTemplate>
</asp:FormView>
<asp:SqlDataSource ID="sqlDirectGroup" runat="server"
	SelectCommand="SELECT usercount=COUNT(*) FROM odberatele where Cislo_nadrizeneho = @Id_odberatele AND Stav_odberatele!='Z'" SelectCommandType="Text">
	<SelectParameters>
		<asp:Parameter Name="Id_odberatele" Type="Int32" />
	</SelectParameters>
</asp:SqlDataSource>
<%--<asp:SqlDataSource ID="sqlBonusGroup" runat="server"
	SelectCommand="SELECT usercount = COUNT(*) FROM provize_aktualni
    WHERE Hladina < 21 AND Id_odberatele IN (SELECT Id_Odberatele FROM fGetOdberateleStrom(@Id_odberatele, DATEPART(YEAR, GETDATE())*100 +  DATEPART(MONTH, GETDATE())))" SelectCommandType="Text">
	<SelectParameters>
		<asp:Parameter Name="Id_odberatele" Type="Int32" />
	</SelectParameters>
</asp:SqlDataSource>--%>


<asp:SqlDataSource ID="sqlBonusGroup" runat="server"
	SelectCommand="SELECT usercount = Count(f.Id_Odberatele) FROM fGetOdberateleStrom(@Id_odberatele, DATEPART(YEAR, GETDATE())*100 +  DATEPART(MONTH, GETDATE())) f
INNER JOIN provize_aktualni p ON p.Id_odberatele = f.Id_Odberatele
INNER JOIN odberatele o  ON o.Id_odberatele = p.Id_odberatele
INNER JOIN odberatele op  ON op.Id_odberatele = o.Cislo_nadrizeneho
LEFT JOIN odberatele oTop  ON oTop.Id_odberatele = p.Id_topmanagera
LEFT JOIN uspesny_start us ON us.Id_odberatele = p.Id_odberatele AND us.RRRRMM = p.RRRRMM
WHERE oTop.Id_odberatele = @Id_odberatele and o.Stav_odberatele!='Z'" SelectCommandType="Text">
	<SelectParameters>
		<asp:Parameter Name="Id_odberatele" Type="Int32" />
	</SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="sqlTotalGroup" runat="server" 
	SelectCommand="SELECT usercount = COUNT(*) FROM provize_aktualni
    WHERE Id_odberatele IN (SELECT Id_Odberatele FROM fGetOdberateleStrom(@Id_odberatele, DATEPART(YEAR, GETDATE())*100 +  DATEPART(MONTH, GETDATE())))" SelectCommandType="Text">
	<SelectParameters>
		<asp:Parameter Name="Id_odberatele" Type="Int32" />
	</SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="sqlRestMarginPrice" runat="server"
	SelectCommand="SELECT objem_pro_marzi = SUM( fr.zapocet_mj_body_marze*fr.mnozstvi), cena_mj_katalogova=SUM(fr.cena_mj_katalogova*fr.mnozstvi), f.kod_meny from www_faktury f
        INNER JOIN www_faktury_radky fr ON fr.id_prepoctu = f.id_prepoctu
        WHERE f.cislo_objednavky_eurosap IS NOT NULL 
        AND f.cislo_objednavky_eurosap IN (select Cislo_objednavky from provize_aktualni_objednavky )
        AND f.cislo_objednavky_eurosap NOT IN (select id_objednavky from objednavkyfaktury where StavK2=0 ) -- objednavka nesmie byt stornovana
        AND YEAR(f.datum_vystaveni)=@rok AND MONTH(f.datum_vystaveni)=@mesic
        AND id_odberatele = @Id_odberatele
        GROUP BY f.kod_meny " SelectCommandType="Text">
	<SelectParameters>
		<asp:Parameter Name="Id_odberatele" Type="Int32" />
        <asp:Parameter Name="rok" Type="Int32" />
        <asp:Parameter Name="mesic" Type="Int32" />
	</SelectParameters>
</asp:SqlDataSource>
