using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using CMS.Entities;

[assembly: WebResource( "CMS.Controls.Translations.vocabulary.js", "application/x-javascript" )]
namespace CMS.Controls.Translations
{
		public class Vocabulary: Control
		{
				private List<Translation> dictionary;
				private bool isEdit = true;

				public string Name { get; set; }

				public Translation Translate( string term )
				{
						EnsureChildControls();
						Translation trans = dictionary.FirstOrDefault( x => x.Term == term );
						if ( trans == default( Translation ) )
								return new Translation { Trans = String.Format( "!{0}:{1}", System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName, term ) };
						return trans;
				}

				public string CssClass { get; set; }
				public bool IsEdit
				{
						get { return this.isEdit; }
						set { this.isEdit = value; }
				}

				/// <summary>
				/// Vráti priamo HTML text.
				/// </summary>
				public string this[string term]
				{
						get
						{
								Translation trans = Translate( term );
								if ( this.IsEdit && CanManageControl() )
								{
										string requestUrl = Utilities.ConfigUtilities.ConfigValue( "CMS:Volcabulary:AjaxOperationsUrl" );
										requestUrl = Page.ResolveUrl( string.IsNullOrEmpty( requestUrl ) ? string.Empty : string.Format( "{0}?id={1}", requestUrl, trans.Id ) );
										string texts = string.Format( "{0};{1}", Resources.Vocabulary.Title, Resources.Vocabulary.Update );
										string memberName = string.Format( "oVocabulary{0}", trans.Id );
										string clientId = string.Format( "{0}_{1}", this.ClientID, trans.Id );

										string script = string.Format( "<script type='text/javascript'>var {0} = new Vocabulary('{1}', '{2}', '{3}', '{4}');</script>",
												memberName, clientId, this.CssClass, texts, requestUrl );

										return script + string.Format( @"<span id='{0}' class='{1}'
										onclick={2}.showEditControl() onmouseover={2}.showEditControl() onmouseout={2}.hideEditControl() >
										{3}</span>", clientId, this.CssClass, memberName, trans.Trans );
								}
								return trans.Trans;
						}
				}

				protected override void CreateChildControls()
				{
						base.CreateChildControls();
						if ( dictionary == null )
								dictionary = Storage<Translation>.Read( new Translation.ReadByVocabulary { Vocabulary = Name } );

						if ( CanManageControl() )
						{
								ClientScriptManager cs = Page.ClientScript;
								Type csType = this.GetType();
								string scriptKey = "vocabulary-js";

								string url = cs.GetWebResourceUrl( csType, "CMS.Controls.Translations.vocabulary.js" );
								cs.RegisterClientScriptInclude( csType, scriptKey, url );
						}
				}

				/// <summary>
				/// Vrtáti true ak je možne v okamihu zobrazenie spravovat vocabulary control.
				/// Teda či sa ma zobraziť v režime s editorom.
				/// </summary>
				public virtual bool CanManageControl()
				{
						string roleString = Utilities.ConfigUtilities.ConfigValue( "CMS:ContentEditor:Roles" );
						string[] roles = roleString.Split( new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries );

						foreach ( string role in roles )
								if ( Security.IsInRole( role ) ) return true;

						return false;
				}
		}
}
