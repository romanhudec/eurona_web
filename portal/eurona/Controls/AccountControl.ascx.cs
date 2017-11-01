using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Utilities;
using Eurona.DAL.Entities;
using System.Data;
using Telerik.Web.UI;

namespace Eurona.Controls
{
		public partial class AccountControl: Eurona.Common.Controls.UserControl
		{
				private Account account = null;
				protected void Page_Load( object sender, EventArgs e )
				{
						if ( string.IsNullOrEmpty( Request["id"] ) ) return;

						this.account = Storage<Account>.ReadFirst( new Account.ReadById { AccountId = Convert.ToInt32( Request["id"] ) } );
						//this.advisorRow.Visible = false;
						//if ( this.account.IsInRole( Role.HOST ) )this.advisorRow.Visible = true;

						//this.ddlAdvisor.EnableLoadOnDemand = false;
						//List<AdvisorPerson> list = Storage<AdvisorPerson>.Read();
						//this.ddlAdvisor.DataSource = list;
						if ( !IsPostBack )
						{
								this.txtLogin.Text = this.account.Login;
								this.txtEmail.Text = this.account.Email;

								//if ( this.account.AccountExtension != null && this.account.AccountExtension.AdvisorId.HasValue )
								//    this.ddlAdvisor.SelectedValue = this.account.AccountExtension.AdvisorId.Value.ToString();
								//this.ddlAdvisor.DataBind();
						}
				}

				//protected void OnDdlAdvisor_DataBound( object sender, EventArgs e )
				//{
				//    //set the initial footer label
				//    ( (Literal)ddlAdvisor.Footer.FindControl( "RadComboItemsCount" ) ).Text = Convert.ToString( ddlAdvisor.Items.Count );
				//}

				//protected void OnDdlAdvisor_ItemsRequested( object sender, RadComboBoxItemsRequestedEventArgs e )
				//{
				//    List<AdvisorPerson> list = Storage<AdvisorPerson>.Read();
				//    this.ddlAdvisor.DataSource = list;
				//    this.ddlAdvisor.DataBind();
				//}
				//protected void OnDdlAdvisor_ItemDataBound( object sender, RadComboBoxItemEventArgs e )
				//{
				//    //set the Text and Value property of every item
				//    //here you can set any other properties like Enabled, ToolTip, Visible, etc.
				//    e.Item.Text = ( (AdvisorPerson)e.Item.DataItem ).Display;
				//    e.Item.Value = ( (AdvisorPerson)e.Item.DataItem ).AccountId.ToString();
				//}

				protected void OnSaveClick( object sender, EventArgs e )
				{
						this.account.Email = this.txtEmail.Text;
						Storage<Account>.Update( this.account );

						//int advisorId = 0;
						//Int32.TryParse( this.ddlAdvisor.SelectedValue, out advisorId );
						//if ( advisorId == 0 ) return;
						
						//if ( this.account.AccountExtension == null )
						//{
						//    this.account.AccountExtension = new AccountExt();
						//    this.account.AccountExtension.AccountId = this.account.Id;
						//    this.account.AccountExtension.AdvisorId = advisorId;
						//    Storage<AccountExt>.Create( this.account.AccountExtension );
						//}else
						//{
						//    this.account.AccountExtension.AdvisorId = advisorId;
						//    Storage<AccountExt>.Update( this.account.AccountExtension );
						//}

						if ( string.IsNullOrEmpty( this.ReturnUrl ) )
								return;

						Response.Redirect( this.ReturnUrl );
				}

				protected void OnCancelClick( object sender, EventArgs e )
				{
						if ( string.IsNullOrEmpty( this.ReturnUrl ) )
								return;

						Response.Redirect( this.ReturnUrl );
				}

		}
}