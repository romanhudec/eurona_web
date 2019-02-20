<%@ Page Title="" Language="C#" MasterPageFile="~/user/anonymous/page.master" AutoEventWireup="true" CodeBehind="register.aspx.cs" Inherits="Eurona.User.Anonymous.RegisterPage" %>
<%@ Register Assembly="cms" Namespace="CMS.Utilities" TagPrefix="cmsUtilities" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls.Page" TagPrefix="cmsPage" %>
<%@ Register assembly="cms" namespace="CMS.Controls" tagprefix="cms" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script type="text/javascript" src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>

    <style type="text/css">
        .ms-formvalidation{color:#ff0000;}
        #rn_register{background-image:url(../../images/anonymous-register-navigation-item-selected.png);}
        #rn_register a{color:#36AFE2;}
        
        .required{color:#C10076;font-weight:bold;font-size:20px;vertical-align:middle;}
        .validator{margin-left:1px; display:none;}
        .validator div{background-color:#E07B82;width:198px;height:15px;padding:3px 0px 1px 2px;color:#fff;}
        .validator span{color:#fff;}
    </style>
    <script language="javascript" type="text/javascript">

    	function AcceptTermsAndConditions(checkbox) {
    		var btnAccept = document.getElementById("<%=btnContinue.ClientID%>");
    		if (btnAccept == null) return;

    		btnAccept.disabled = !checkbox.checked;
    	}

    	function OnStejnaJakoFakturacni(checkbox) {
    		if (checkbox.checked) {
				document.getElementById("<%=txtAdresaDodaci_Mesto.ClientID%>").value = document.getElementById("<%=txtMesto.ClientID%>").value;
				document.getElementById("<%=txtAdresaDodaci_PSC.ClientID%>").value = document.getElementById("<%=txtPsc.ClientID%>").value;
				document.getElementById("<%=txtAdresaDodaci_Ulice.ClientID%>").value = document.getElementById("<%=txtUlice.ClientID%>").value;
    		}
		}

		function hideElm(elmName) {
			var elm = document.getElementById(elmName);
			if (elm == null) return;
			elm.style.display = 'none';
		}

		function validate(controlId, validatorId) {
			var control = document.getElementById(controlId);
			if (control == null) return true;

			var validator = document.getElementById(validatorId);
			if (validator == null) return true;
			validator.style.display = 'none';

			if (control.value == '') {
				validator.style.display = 'block';
				//control.focus();
				return false;
			}

			return true;
		}

		function validateRegExp(controlId, validatorId, expr) {
			var control = document.getElementById(controlId);
			if (control == null) return true;

			var validator = document.getElementById(validatorId);
			if (validator == null) return true;
			validator.style.display = 'none';

			var patt = new RegExp(expr);
			if (patt.test(control.value) == false) {
				validator.style.display = 'block';
				control.focus();
				return false;
			}

			return true;
		}
		function updateASPxValidators() {
		    var isAllValid = true;
		    for (var i = 0; i < Page_Validators.length; i++) {
		        var val = Page_Validators[i];
		        var ctrl = document.getElementById(val.controltovalidate);
		        if (ctrl != null && ctrl.style != null) {
		            if (!val.isvalid) {
		                ctrl.style.background = '#EFB6E6';//'#FFAAAA';
		                isAllValid = false;
		            }
		            else
		                ctrl.style.backgroundColor = '';
		        }
		    }
		}
		function onSave() {
			if (validate('<%=txtHesloProHosta.ClientID %>', 'validatorHesloProHosta') == false) return false;
			if (validateRegExp('<%=txtHesloProHosta.ClientID %>', 'validatorHesloProHostaRegExp', '^[0-9]{3}-[0-9]{10}$') == false) return false;

			if (validate('<%=txtEmail.ClientID %>', 'validatorEmail') == false) return false;
			if (validate('<%=txtName.ClientID %>', 'validatorName') == false) return false;
			if (validate('<%=txtUlice.ClientID %>', 'validatorUlice') == false) return false;
			if (validate('<%=txtMesto.ClientID %>', 'validatorMesto') == false) return false;
			if (validate('<%=txtPsc.ClientID %>', 'validatorPSC') == false) return false;
			if (validate('<%=txtMobil.ClientID %>', 'validatorMobil') == false) return false;
			if (validate('<%=txtAdresaDodaci_Ulice.ClientID %>', 'validatorAdresaDodaci_Ulice') == false) return false;
			if (validate('<%=txtAdresaDodaci_Mesto.ClientID %>', 'validatorAdresaDodaci_Mesto') == false) return false;
			if (validate('<%=txtAdresaDodaci_PSC.ClientID %>', 'validatorAdresaDodaci_PSC') == false) return false;

		    //if (validate('<perc=txtLogin.ClientID perc>', 'validatorLogin') == false) return false;
		    if (validate('<%=txtPassword.ClientID %>', 'validatorPassword') == false) return false;
            
		    if (validateRegExp('<%=txtMobil.ClientID %>', 'validatorMobil', '^[0-9]{9}$') == false) {
		        alert('<asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_IvalidMobile%>" />');
		        return false;
		    }

			if (validate('<%=txtConfirmPassword.ClientID %>', 'validatorConfirmPassword') == false) return false;
			return true;
		}

		function confirmNemamHeloProHostaCheck() {
			var result = confirm('OPRAVDU jste neobdržel/a  od svého poradce Eurona  heslo pro hosta, které se vyplňuje v úvodu formuláře? Přejete si pokračovat?');
			if (!result) {
				var elm = document.getElementById('<%=rbHesloProHostaANO.ClientID %>');
				elm.checked = 1;
				return false;
			}
			return true;
		}

        function validatePwd() {
            var btnContinue = document.getElementById('<%=btnContinue.ClientID%>');
            var elmErrorMessage = document.getElementById('<%=lblValidatorTextPwd.ClientID%>');
            var elmErrorMessageRepeat = document.getElementById('<%=lblValidatorTextPwdRepeat.ClientID%>');
            var elm = document.getElementById('<%=txtPassword.ClientID%>');
            var elmRepeat = document.getElementById('<%=txtConfirmPassword.ClientID%>');

            var result = validatePasswordAndRepeatPassword(elm, elmRepeat, elmErrorMessage, elmErrorMessageRepeat);
            if (result == false) {
                btnContinue.disabled = true;
            } else {
                onSave();
            }
        }
        
		$(function () {
			$(".tbCityAutocomplette").autocomplete({
				source: function (request, response) {
					$.ajax({
						url: "<%=Page.ResolveUrl("~/getCityByState.ashx")%>?mesto=" + request.term + "&stat=" + document.getElementById('<%=ddlStat.ClientID%>').value,
						data: request.term,
						dataType: "json",
						type: "POST",
						contentType: "application/json; charset=utf-8",
						dataFilter: function (data) { return data; },
						success: function (data) {
							response($.map(data.d, function (item) {
								return { label:item.Name + ' ,' + item.Psc, value:item.Name, psc:item.Psc }
							}))
						},
						error: function (XMLHttpRequest, textStatus, errorThrown) {
							alert(textStatus);
						}
					});
				},
			    html: true, // optional (jquery.ui.autocomplete.html.js required)
			    // optional (if other layers overlap autocomplete list)
			    open: function (event, ui) {
			        $('.ui-autocomplete').css('z-index', 1000);
			    },
				select: function (event, ui) {
					document.getElementById('<%=txtMesto.ClientID%>').value = ui.item.value.trim();
				    document.getElementById('<%=txtPsc.ClientID%>').value = ui.item.psc.trim();
				},
				minLength: 2
			});
		});
        $(function () {
            $(".tbCityAutocomplette2").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "<%=Page.ResolveUrl("~/getCityByState.ashx")%>?mesto=" + request.term + "&stat=" + document.getElementById('<%=ddlStat.ClientID%>').value,
					    data: request.term,
					    dataType: "json",
					    type: "POST",
					    contentType: "application/json; charset=utf-8",
					    dataFilter: function (data) { return data; },
					    success: function (data) {
					        response($.map(data.d, function (item) {
					            return { label: item.Name + ' ,' + item.Psc, value: item.Name, psc: item.Psc }
					        }))
					    },
					    error: function (XMLHttpRequest, textStatus, errorThrown) {
					        alert(textStatus);
					    }
					});
                },
                html: true, // optional (jquery.ui.autocomplete.html.js required)
                // optional (if other layers overlap autocomplete list)
                open: function (event, ui) {
                    $('.ui-autocomplete').css('z-index', 1000);
                },
			    select: function (event, ui) {
			        document.getElementById('<%=txtAdresaDodaci_Mesto.ClientID%>').value = ui.item.value.trim();
				    document.getElementById('<%=txtAdresaDodaci_PSC.ClientID%>').value = ui.item.psc.trim();
				},
			    minLength: 2
			});
        });
 
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content_header" runat="server">
    vyplnení údajů pro registraci
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <div style="margin:0px 30px 20px 30px;">
    <table border="0" width="100%">
        <tr>
            <td valign="top" colspan="2" style="padding-top:20px;padding-bottom:20px;">
                <table border="0" width="100%">
					<tr>
						<td></td>
						<td>
							<div class="validator" id='validatorHesloProHostaRegExp' onclick="hideElm('validatorHesloProHostaRegExp');" style="width:100%;">
								<div style="margin-left:0px;width:100%;">
									<asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_Litujeme %>"></asp:Literal>
								</div>
							</div> 
						</td>
					</tr>
                    <tr>
                        <td style="width:150px;"><asp:Label ID="Label1" runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_HesloProHosta %>" Font-Bold="true" ToolTip="<%$ Resources:EShopStrings, Anonymous_Register_HesloProHosta_Tooltip %>"></asp:Label></td>
                        <td>
                            <div>
                                <asp:RadioButton runat="server" ID="rbHesloProHostaANO" Font-Bold="true" GroupName="HesloProHostaGroup" Text="<%$ Resources:EShopStrings, Anonymous_Register_HesloProHosta_AnoText %>" OnCheckedChanged="OnRadioButtonHesloProHostaChecked" AutoPostBack="true" ToolTip="<%$ Resources:EShopStrings, Anonymous_Register_HesloProHosta_AnoText_Tooltip %>" />
                                &nbsp;&nbsp;
                                <span id='spanHesloProHosta' runat="server">
                                    <span class="required">*</span>
                                    <div style="display:inline-block;">
                                        <asp:TextBox runat="server" ID="txtHesloProHosta" ToolTip="Pokud Vám Váš obchodní specialista, poradce Eurona předal heslo pro hosta, vyplňte ho zde a pokračujte v objednávce." onclick="hideElm('validatorHesloProHosta');" Width="120"></asp:TextBox>
                                        <div class="validator" id='validatorHesloProHosta' onclick="hideElm('validatorHesloProHosta');" style="width:120px;">
                                            <div style="position:absolute;margin-top:-20px;margin-left:0px;width:118px;">
                                                <asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_ProsimVyplnte %>"></asp:Literal>
                                            </div>
                                        </div>
                                    </div>
									<span style="font-style:italic;"><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_ZadejteHesloVeTvaru %>"></asp:Literal></span>
                                </span>
                            </div>
                            <div>
                               <asp:RadioButton runat="server" ID="rbHesloProHostaNE" Font-Bold="true" GroupName="HesloProHostaGroup" Text="<%$ Resources:EShopStrings, Anonymous_Register_HesloProHosta_NeText %>" onclick="confirmNemamHeloProHostaCheck()" OnCheckedChanged="OnRadioButtonHesloProHostaChecked" AutoPostBack="true"/>
                            </div>
                        </td>
                    </tr>
					<tr><td colspan="2"><div style="height:30px;"></div></td></tr>
                    <tr>
                        <td style="width:130px;">
                            <span class="required">*</span>
                            <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Strings, RegisterControl_EmailLabel %>" />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtEmail" CausesValidation="True" Width="200px" onclick="hideElm('validatorEmail');"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="emailValidator" runat="server" ControlToValidate="txtEmail"
                                ErrorMessage="!" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$">
                                <div class="validator" id='validatorEmail1' onclick="hideElm('validatorEmail1');" style="display:block;">
                                    <div style="position:absolute;margin-top:-20px;">
                                        <asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_VyplntePovinnouPolozku %>"></asp:Literal>            
                                    </div>
                                </div> 
                            </asp:RegularExpressionValidator>
                            <div class="validator" id='validatorEmail' onclick="hideElm('validatorEmail');">
                                <div style="position:absolute;margin-top:-20px;">
                                    <asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_VyplntePovinnouPolozku %>"></asp:Literal>            
                                </div>
                            </div> 
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="required">*</span>
                            <asp:Literal ID="Literal5" runat="server" Text="Stát : " />
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlStat" Width="200px" AutoPostBack="true"></asp:DropDownList>
                        </td>                        
                    </tr>
                    <tr>
                        <td>
                            <span class="required">*</span>
                            <asp:Literal ID="Literal6" runat="server" Text="Region : " />
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlRegion" Width="200px"></asp:DropDownList>
                        </td>                        
                    </tr>
                    <tr>
                        <td>
                            <span class="required">*</span>
                            <asp:Literal ID="Literal7" runat="server" Text="PF : " />
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlPF" Width="200px"></asp:DropDownList>
                        </td>                        
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal8" runat="server" Text="Předmět činnosti : " />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtPredmetCinnosti" Width="200px"></asp:TextBox>
                        </td>                        
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                        </td>
                    </tr>
                </table>                
            </td>
        </tr>
        <tr><td colspan="2"></td></tr>
        <tr>
            <td><b><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_FakturacniAdresa %>"></asp:Literal></b></td>
			<td rowspan="2" valign="top">
				<table>
					<tr>
						<td></td>
						<td><b><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_DodaciAdresa %>"></asp:Literal></b></td>
					</tr>
					<tr>
						<td valign="top">
							<table>
								<tr>
									<td><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_DatumNarozeni %>"></asp:Literal></td>
									<td>
										<cms:ASPxDatePicker runat="server" ID="dtpDatumNarozeni" Width="90px"></cms:ASPxDatePicker>
									</td>
								</tr>
								<tr>
									<td><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_ICO %>"></asp:Literal></td>
									<td>
										<asp:TextBox runat="server" ID="txtICO" Width="185px"></asp:TextBox>
									</td>
								</tr>
								<tr>
									<td><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_DIC %>"></asp:Literal></td>
									<td>
										<asp:TextBox runat="server" ID="txtDIC" Width="185px"></asp:TextBox>
									</td>
								</tr>
								<tr>
									<td><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_PlatceDPH %>"></asp:Literal></td>
									<td>
										<asp:CheckBox runat="server" ID="cbPlatceDPH" />
									</td>
								</tr>
								<tr>
									<td><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_BankovniUcet %>"></asp:Literal></td>
									<td>
										<asp:TextBox runat="server" ID="txtBankovniUcet" Width="185px"></asp:TextBox>
									</td>
								</tr>
							</table>
						</td>
						<td valign="top">
							<asp:CheckBox runat="server" ID="cbStejnaJakoFakturacni" Text="<%$ Resources:EShopStrings, Anonymous_Register_StejnaJakoFakturacni %>" onclick="OnStejnaJakoFakturacni(this)" />
							<div id="divDodaciAdresa">
								<table>
									<tr>
										<td colspan="2"></td>
									</tr>
									<tr>
										<td colspan="2"><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_JinaVyplnte %>"></asp:Literal></td>
									</tr>
									<tr>
										<td><span class="required">*</span><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_Ulice %>"></asp:Literal></td>
										<td>
											<asp:TextBox runat="server" ID="txtAdresaDodaci_Ulice" Width="200px" onclick="hideElm('validatorAdresaDodaci_Ulice');"></asp:TextBox>
											<div class="validator" id='validatorAdresaDodaci_Ulice' onclick="hideElm('validatorAdresaDodaci_Ulice');">
												<div style="position:absolute;margin-top:-20px;">
													<asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_VyplntePovinnouPolozku %>"></asp:Literal>         
												</div>
											</div>
										</td>
									</tr>
									<tr>
										<td><span class="required">*</span><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_Mesto %>"></asp:Literal></td>
										<td>
                                            <div class="ui-widget">
								                <asp:TextBox CssClass="tbCityAutocomplette2" runat="server" ID="txtAdresaDodaci_Mesto" Width="200px" onclick="hideElm('validatorAdresaDodaci_Mesto');"></asp:TextBox>
							                </div>
											
											<div class="validator" id='validatorAdresaDodaci_Mesto' onclick="hideElm('validatorAdresaDodaci_Mesto');">
												<div style="position:absolute;margin-top:-20px;">
													<asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_VyplntePovinnouPolozku %>"></asp:Literal>       
												</div>
											</div>
										</td>
									</tr>
									<tr>
										<td><span class="required">*</span><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_PSC %>"></asp:Literal></td>
										<td>
											<asp:TextBox runat="server" ID="txtAdresaDodaci_PSC" Width="40px" onclick="hideElm('validatorAdresaDodaci_PSC');"></asp:TextBox>
                                            <div style='margin-left:3px; width:150px; display:inline-flex;vertical-align:middle;word-wrap:break-word;' class='address_notes_desription'>
                                                <asp:Literal ID="Literal13" runat="server" Text="<%$ Resources:EShopStrings, OrderControl_PSC_Hint %>"></asp:Literal>
                                            </div>
											<div class="validator" id='validatorAdresaDodaci_PSC' onclick="hideElm('validatorAdresaDodaci_PSC');">
												<div style="position:absolute;margin-top:-20px;">
													<asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_VyplntePovinnouPolozku %>"></asp:Literal>           
												</div>
											</div>
										</td>
									</tr>
								</table>
							</div>
						</td>
					</tr>
				</table>
				<div>
					<a runat="server" id="linkVasePrilezitosti" target="_blank">
						<div>
							<cmsPage:PageControl ID="PageControl2" IsEditing="true" runat="server" CssEditorToolBar="contentEditorToolbar" CssEditorContent="contentEditorContent" NewUrl="" 
							ManageUrl="" NotFoundUrlFormat="~/notFound.aspx?page={0}" PageName="anonymous-register-banner1-content" PopUpEditorUrlFormat="~/admin/contentEditor.aspx?id={0}" />
						</div>
					</a>
				</div>
			</td>
        </tr>
        <tr>
            <td valign="top">
                <table>
                    <tr>
                        <td style="white-space:nowrap;">
                            <span class="required">*</span>
							<asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_JmenoAPrijmeni %>"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtName" Width="200px" onclick="hideElm('validatorName');"></asp:TextBox>
                            <div class="validator" id='validatorName' onclick="hideElm('validatorName');">
                                <div style="position:absolute;margin-top:-20px;">
                                    <asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_VyplntePovinnouPolozku %>"></asp:Literal>               
                                </div>
                            </div> 
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="required">*</span>
							<asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_Ulice %>"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtUlice" Width="200px" onclick="hideElm('validatorUlice');"></asp:TextBox>
                            <div class="validator" id='validatorUlice' onclick="hideElm('validatorUlice');">
                                <div style="position:absolute;margin-top:-20px;">
                                    <asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_VyplntePovinnouPolozku %>"></asp:Literal>             
                                </div>
                            </div> 
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="required">*</span>
							<asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_Mesto %>"></asp:Literal>
                        </td>
                        <td>
							<div class="ui-widget">
								<asp:TextBox CssClass="tbCityAutocomplette" runat="server" ID="txtMesto" Width="200px" onclick="hideElm('validatorMesto');"></asp:TextBox>
							</div>
                            <div class="validator" id='validatorMesto' onclick="hideElm('validatorMesto');">
                                <div style="position:absolute;margin-top:-20px;">
                                    <asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_VyplntePovinnouPolozku %>"></asp:Literal>             
                                </div>
                            </div> 
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="required">*</span>
							<asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_PSC %>"></asp:Literal>
                        </td>
                        <td valign="middle">
                            <asp:TextBox runat="server" ID="txtPsc" Width="40px" onclick="hideElm('validatorPSC');"></asp:TextBox>
                            <div style='margin-left:3px; width:150px; display:inline-flex;vertical-align:middle;word-wrap:break-word;' class='address_notes_desription'>
                                <asp:Literal ID="Literal12" runat="server" Text="<%$ Resources:EShopStrings, OrderControl_PSC_Hint %>"></asp:Literal>
                            </div>
							<%--<span style="font-size:8px;"><asp:Literal runat="server" Text="<%$Resources:Strings, ZipDescription %>"></asp:Literal></span>--%>
                            <div class="validator" id='validatorPSC' onclick="hideElm('validatorPSC');">
                                <div style="position:absolute;margin-top:-20px;">
                                    <asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_VyplntePovinnouPolozku %>"></asp:Literal>            
                                </div>
                            </div>
                        </td>
                    </tr>
                    <%--<tr>
                        <td>
                            <span class="required">*</span>
							<asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_Kraj %>"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtKraj" Width="200px" onclick="hideElm('validatorKraj');"></asp:TextBox>
                            <div class="validator" id='validatorKraj' onclick="hideElm('validatorKraj');">
                                <div style="position:absolute;margin-top:-20px;">
                                    <asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_VyplntePovinnouPolozku %>"></asp:Literal>              
                                </div>
                            </div>
                        </td>
                    </tr>--%>
                    <tr>
                        <td>
							<asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_Telefon %>"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtTelefon" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="required">*</span>
                            Mobil : 
                        </td>
                        <td>
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="2">
                                        <div>
                                            <span class='address_notes_desription'>Telefonní číslo ve tvaru 123456789</span>                                
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:TextBox Enabled="false" ReadOnly="true" runat="server" ID="lblMobilPrefix" Width="30px" >+420</asp:TextBox>&nbsp;</td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtMobil" Width="164px" onblur="return updateASPxValidators()" onclick="hideElm('validatorMobil');" />                                        
                                        <div class="validator" id='validatorMobil' onclick="hideElm('validatorMobil');">
                                            <div style="position:absolute;margin-top:-20px;width:168px;">
                                                <asp:Literal ID="Literal11" runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_VyplntePovinnouPolozku %>"></asp:Literal>                
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>                            
                        </td>
                    </tr> 
                    <tr>
                        <td></td>
                        <td>
                            <div style="width:200px;">
                                <asp:RegularExpressionValidator ID="mobilValidator" runat="server" Display="Static" ControlToValidate="txtMobil" ErrorMessage="<%$ Resources:EShopStrings, Anonymous_Register_IvalidMobile %>" EnableClientScript="true" SetFocusOnError="true" ValidationExpression="^([0-9]{9,9})$" />                            
                            </div>
                        </td>
                    </tr>           
                </table>
            </td>
		</tr>
		<tr>
			<td colspan="2">
                <table>
                    <tr>
                        <td>
                            <table border="0" style="border:2px solid #c10076;margin:10px 0px 10px 0px;padding:10px 0px 10px 0px;">

<%--                                <tr>
                                    <td style="width:130px;">
                                        <span class="required">*</span>
                                        <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, RegisterControl_LoginLabel %>" />
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtLogin" Width="200px" onclick="hideElm('validatorLogin');"></asp:TextBox>
                                        <div class="validator" id='validatorLogin' onclick="hideElm('validatorLogin');">
                                            <div style="position:absolute;margin-top:-20px;">
                                                <asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_VyplntePovinnouPolozku %>"></asp:Literal>
                                            </div>
                                        </div>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td>
                                        <span class="required">*</span>
                                        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, RegisterControl_PasswordLabel %>" />
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" TextMode="Password" ID="txtPassword" Width="250px" oninput="validatePwd();" onclick="hideElm('validatorPassword');"></asp:TextBox>
                                        <div class="validator" id='validatorPassword' onclick="hideElm('validatorPassword');">
                                            <div style="position:absolute;margin-top:-20px;">
                                                <asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_VyplntePovinnouPolozku %>"></asp:Literal>
                                            </div>
                                        </div>    
                                        <div class="validation-message">
                                            <asp:Label runat="server" ID="lblValidatorTextPwd" Width="250px" Text="<%$ Resources:Strings, EmailVerifyControl_PasswordErrorMessage %>" ForeColor="#EA008A" style="display:none;"></asp:Label>
                                        </div>                                                                           
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="required">*</span>
                                        <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, RegisterControl_PasswordConfirmLabel %>" />
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" TextMode="Password" ID="txtConfirmPassword" oninput="validatePwd();" Width="250px" CausesValidation="True" onclick="hideElm('validatorConfirmPassword');"></asp:TextBox>
                                        <div class="validator" id='validatorConfirmPassword' onclick="hideElm('validatorConfirmPassword');">
                                            <div style="position:absolute;margin-top:-20px;">
												<asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_VyplntePovinnouPolozku %>"></asp:Literal>
                                            </div>
                                        </div> 
                                        <div class="validation-message">
                                            <asp:Label runat="server" ID="lblValidatorTextPwdRepeat"  Width="250px" Text="<%$ Resources:Strings, EmailVerifyControl_PasswordRepeatErrorMessage %>" ForeColor="#EA008A" style="display:none;"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <span style="color:#c10076;font-size:16px;"><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_VasePrihlasovaciUdaje %>"></asp:Literal>&nbsp;<br /><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_ProVasiDalsiObjednavku %>"></asp:Literal></span>                            
                        </td>
                    </tr>
                </table>
			</td>
		</tr>
        <tr>
            <td colspan="2">
                <div style="padding:10px;">
                    <asp:Literal ID="Literal9" runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_ReklamniZasilky_Header %>"></asp:Literal>
                </div>
                <div>
                    <asp:Repeater runat="server" ID="rpReklamniZasilky" OnItemDataBound="ReklamniZasilkyOnItemDataBound">
                        <HeaderTemplate>
			                <table border="0" cellpadding="0" cellspacing="0" class="dataGrid" style="width:auto!important;">
			                <tr>
				                <td class="dataGrid_headerStyle">Reklamní zásilka</td>
				                <td class="dataGrid_headerStyle">Souhlas</td>
			                </tr>
		                </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
				                <td><span><%#Eval("Popis")%></span></td>
				                <td><asp:CheckBox Enabled="true" runat="server" ID="cbReklamniZasilkySouhlas" Text="" CommandArgument='<%#Eval("Id") %>' /></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate></table></FooterTemplate>
                    </asp:Repeater> 
                    <cmsPage:PageControl ID="genericPage" IsEditing="false" runat="server" CssEditorToolBar="contentEditorToolbar" CssEditorContent="contentEditorContent" NewUrl="~/admin/page.aspx"
                    ManageUrl="~/admin/pages.aspx" NotFoundUrlFormat="~/notFound.aspx?page={0}" PageName="anonymous-registration-content" PopUpEditorUrlFormat="~/admin/contentEditor.aspx?id={0}" />
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <span class="required">*</span> <asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Register_PovinneUdaje %>"></asp:Literal>
                <div>
                    <%--<asp:CheckBox ID="cbAcceptTerms" runat="server" Text="Souhlas se " onclick="AcceptTermsAndConditions(this)" />&nbsp;--%>
                    <asp:CheckBox ID="cbAcceptTerms" runat="server" />&nbsp; <span class="required">*</span>Souhlas se&nbsp; 
                    <asp:HyperLink ID="hlSmluvniPodminky" runat="server" NavigateUrl="" Text="<%$ Resources:EShopStrings, Anonymous_Register_SmluvnimiPodminkami %>" Target="_blank" ></asp:HyperLink>&nbsp;a&nbsp;
                    <asp:HyperLink ID="hlObchodniPodminky" runat="server" NavigateUrl="" Text="<%$ Resources:EShopStrings, Anonymous_Register_ObchodnimiPodminkami %>" Target="_blank" ></asp:HyperLink>
                </div>
                <asp:Button runat="server" ID="btnContinue"  Enabled="true" CssClass="button" Text="<%$ Resources:Strings, RegisterControl_ContinueButton %>" OnClick="OnContinueClick" />
            </td>
        </tr>
    </table>
    </div>
    <%--UTILITIES--%>
    <cmsUtilities:AliasUtilities ID="aliasUtilities" runat="server" ></cmsUtilities:AliasUtilities>  
</asp:Content>
