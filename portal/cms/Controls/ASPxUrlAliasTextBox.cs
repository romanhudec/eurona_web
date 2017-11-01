using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Resources;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using UrlAliasPrefixEntity = CMS.Entities.Classifiers.UrlAliasPrefix;
using CMS.Entities.Classifiers;

namespace CMS.Controls
{
		/// <summary>
		/// Autor: Ing. Roman Hudec
		/// </summary>
		[ToolboxData( "<{0}:ASPxUrlAliasTextBox runat=server></{0}:ASPxUrlAliasTextBox>" )]
		[ControlValueProperty( "Text" ), ValidationProperty( "Text" ), ParseChildren( true, "Text" )]
		public class ASPxUrlAliasTextBox: UserControl
		{
				public delegate void UrlAliasPrefixEventHandler( string prefix, out string newPrefix );
				public event UrlAliasPrefixEventHandler OnGetUrlAliasPrefix;

				private TextBox txtAlias = null;

				#region Public properties
				public string Text
				{
						get { return this.txtAlias.Text; }
						set { this.txtAlias.Text = value; }
				}

				public string FieldID { get; set; }
				public Unit Width { get; set; }
				public bool AutoCompletteAlias { get; set; }

				/// <summary>
				/// ID Url Alis prefixu, ktory sa ma doplnat pre samotny alias.
				/// </summary>
				public int? UrlAliasPrefixId
				{
						get
						{
								object o = ViewState["UrlAliasPrefixId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["UrlAliasPrefixId"] = value; }
				}

				#endregion

				public ASPxUrlAliasTextBox()
				{
						this.txtAlias = new TextBox();
						this.txtAlias.ID = "txtAlias";
				}

				#region Overridet methods

				public bool Enabled
				{
						get { return this.txtAlias.Enabled; }
						set { this.txtAlias.Enabled = value; }
				}

				protected override void CreateChildControls()
				{
						this.txtAlias.Width = this.Width;
						this.Controls.Add( this.txtAlias );

						if ( this.AutoCompletteAlias )
						{
								ClientScriptManager csm = Page.ClientScript;
								string js = @"
								var elm = document.getElementById('" + this.FieldID + @"');
								var aliasPrefix = '" + this.GetAliasPrefix() + @"';
								elm.onblur = function () { 
										createUrlAlias( aliasPrefix, elm.value, '" + this.txtAlias.ClientID + @"' );
								}
								";
								csm.RegisterStartupScript( GetType(), "uac_" + this.txtAlias.ClientID, js, true );

								string jsFunction =
								@"function createUrlAlias( prefix, fieldValue, aliasElmId ){
										var aliasElm = document.getElementById(aliasElmId);

										var str = makeAnsiUrl( fieldValue.toLowerCase() );
										str = str.replace(/\,/g, '-');
										str = str.replace(/\;/g, '-');
										str = str.replace(/\+/g, '-');
										str = str.replace(/\_/g, '-');
										str = str.replace(/\./g, '-');
										str = str.replace(/\:/g, '-');
										str = str.replace(/\#/g, '-');
										str = str.replace(/\</g, '-');
										str = str.replace(/\>/g, '-');
										str = str.replace(/\?/g, '');
										str = str.replace(/\""/g, '');
										str = str.replace(/\&/g, '-');
										
										str = str.replace(/\----/g, '-');
										str = str.replace(/\---/g, '-');
										str = str.replace(/\--/g, '-');

										if( prefix.length != 0 ){
												str = makeAnsiUrl( prefix.toLowerCase() ) + '/' + str;
										}
										str = '/'+str;

										aliasElm.value = str;
								}

								function makeAnsiUrl(s) {
										var s;

										var convertMap =
														{
																'á': 'a', 'Á': 'A', 'ä': 'a', 'í': 'i', 'Í': 'i',
																'ó': 'o', 'Ó': 'O', 'ô': 'o', 'é': 'e', 'ě': 'e',
																'É': 'E', 'ú': 'u', 'Ú': 'U', 'ů': 'u', 'Ů': 'U',
																'ľ': 'l', 'Ľ': 'L', 'ĺ': 'l', 'Ĺ': 'L', 'š': 's',
																'Š': 's', 'č': 'c', 'Č': 'C', 'ť': 't', 'Ť': 'T',
																'ž': 'z', 'Ž': 'Z', 'ř': 'r', 'Ř': 'R', 'ý': 'y',
																'Ý': 'Y', 'ň': 'n', 'Ň': 'N', 'ď': 'd', 'ö': 'o',
																'ŕ': 'r' , ' ': '-'
														}

										for (var i in convertMap) {
												s = s.replace(new RegExp(i, 'g'), convertMap[i]);
										}
										return s;
								}
								";
								csm.RegisterStartupScript( GetType(), "urlAliasControl", jsFunction, true );
						}
				}
				#endregion

				private string GetAliasPrefix()
				{
						if ( !this.UrlAliasPrefixId.HasValue )
								return string.Empty;

						string aliasPrefix = string.Empty;
						UrlAliasPrefixEntity urlAliasPrefix = Storage<UrlAliasPrefixEntity>.ReadFirst( new ClassifierBase.ReadById { Id = this.UrlAliasPrefixId.Value } );
						if ( urlAliasPrefix != null )
								aliasPrefix = urlAliasPrefix.Name;

						//Event na moznost upravy prefixu!
						if ( OnGetUrlAliasPrefix != null )
								OnGetUrlAliasPrefix( aliasPrefix, out aliasPrefix );

						return aliasPrefix;
				}
				/// <summary>
				/// Vráti normalizovany URL alias z textboxu. Ak je textbox prázdny 
				/// vytvori URL Alias z alternativneho aliasu a znormalizuje ho.
				/// </summary>
				public string GetUrlAlias( string alternateAlias )
				{
						//Format alternate url alias
						if ( alternateAlias.StartsWith( "~" ) ) alternateAlias = alternateAlias.Remove( 0, 1 );
						if ( alternateAlias.StartsWith( "/" ) ) alternateAlias = alternateAlias.Remove( 0, 1 );

						string aliasPrefix = this.GetAliasPrefix();
						if ( !string.IsNullOrEmpty( aliasPrefix ) )
								alternateAlias = string.Format( "{0}/{1}", aliasPrefix, alternateAlias );
						alternateAlias = "~/" + alternateAlias;

						//Format url alias
						string alias = this.txtAlias.Text;
						if ( string.IsNullOrEmpty( alias ) ) alias = alternateAlias;

						//Normalizacia urlAliasu
						if ( alias.StartsWith( "~" ) ) alias = alias.Remove( 0, 1 );
						if ( alias.StartsWith( "/" ) ) alias = alias.Remove( 0, 1 );
						alias = "~/" + alias;

						return alias;
				}

		}
}
