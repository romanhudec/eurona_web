using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Controls;
using System.Web.UI.WebControls;
using CurrenyEntity = SHP.Entities.Classifiers.Currency;

namespace SHP.Controls.Settings
{
		public class CurrencyChoiceControl: CmsControl
		{
				private DropDownList ddlCurrency = null;
				//private Button btnChoiceCurrency = null;
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						this.ddlCurrency = new DropDownList();
						this.ddlCurrency.ID = "ddlCurrency";
						this.ddlCurrency.DataSource = Storage<CurrenyEntity>.Read();
						this.ddlCurrency.DataValueField = "Id";
						this.ddlCurrency.DataTextField = "Name";
						this.ddlCurrency.AutoPostBack = true;
						this.ddlCurrency.SelectedIndexChanged += OnCurrencyChanged;

						this.Controls.Add( this.ddlCurrency );

						if ( !IsPostBack )
						{
								this.ddlCurrency.DataBind();
								if ( Session["SHP:Currency:Id"] != null )
										this.ddlCurrency.SelectedValue = Session["SHP:Currency:Id"].ToString();
								else
								{
										CurrenyEntity ce = Storage<CurrenyEntity>.ReadFirst( new CurrenyEntity.ReadByRate { Rate = 1 } );
										if ( ce != null )
												this.ddlCurrency.SelectedValue = ce.Id.ToString();
								}
						}
				}

				void OnCurrencyChanged( object sender, EventArgs e )
				{
						if ( string.IsNullOrEmpty( this.ddlCurrency.SelectedValue ) ) return;
						CurrenyEntity ce = Storage<CurrenyEntity>.ReadFirst( new CurrenyEntity.ReadById { Id = Convert.ToInt32( this.ddlCurrency.SelectedValue ) } );
						Session["SHP:Currency:Id"] = ce.Id;
						Session["SHP:Currency:Rate"] = ce.Rate;
						Session["SHP:Currency:Symbol"] = ce.Symbol;

						Response.Redirect( Request.RawUrl );
				}
		}
}
