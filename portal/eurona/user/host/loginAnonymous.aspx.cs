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
using Eurona.Common.DAL.Entities;

namespace Eurona.User.Host
{
		public partial class LoginAnonymous: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						this.ddlAdvisor.EnableLoadOnDemand = false;

						if ( !IsPostBack )
						{
								this.ddlRegion.Items.Clear();
								List<ListItem> items = new Eurona.Common.Controls.UserManagement.OrganizationControl.Hepler().GetRegions();
								foreach ( ListItem item in items )
										this.ddlRegion.Items.Add( new RadComboBoxItem( item.Text, item.Value ) );

								RadComboBoxItem itemEmpty = new RadComboBoxItem( string.Empty, string.Empty );
								this.ddlRegion.Items.Insert( 0, itemEmpty );
						}
				}

				protected void OnDdlAdvisor_DataBound( object sender, EventArgs e )
				{
						//set the initial footer label
						( (Literal)ddlAdvisor.Footer.FindControl( "RadComboItemsCount" ) ).Text = Convert.ToString( ddlAdvisor.Items.Count );
				}

				protected void OnDdlAdvisor_ItemsRequested( object sender, RadComboBoxItemsRequestedEventArgs e )
				{
						//List<Organization> list = Storage<Organization>.Read();
						//this.ddlAdvisor.DataSource = list;
						//this.ddlAdvisor.DataBind();
						OnFindAdvisor( sender, null );
				}
				protected void OnDdlAdvisor_ItemDataBound( object sender, RadComboBoxItemEventArgs e )
				{
						//set the Text and Value property of every item
						//here you can set any other properties like Enabled, ToolTip, Visible, etc.
						e.Item.Text = ( (Organization)e.Item.DataItem ).Name;
						e.Item.Value = ( (Organization)e.Item.DataItem ).Code;
				}

				protected void OnFindAdvisor( object sender, EventArgs e )
				{
						if ( string.IsNullOrEmpty( this.txtAdvisorName.Text ) && string.IsNullOrEmpty( this.txtCity.Text ) && string.IsNullOrEmpty( this.ddlRegion.SelectedValue ) )
						{
								this.ddlAdvisor.DataSource = new List<Organization>();
								this.ddlAdvisor.DataBind();
								this.ddlAdvisor.Text = string.Empty;
								return;

						}
						string city = string.IsNullOrEmpty( txtCity.Text ) ? null : txtCity.Text;
						string advisorName = string.IsNullOrEmpty( txtAdvisorName.Text ) ? null : txtAdvisorName.Text;
						string regionCode = string.IsNullOrEmpty( ddlRegion.SelectedValue ) ? null : ddlRegion.SelectedValue;

						List<Organization> list = Storage<Organization>.Read( new Organization.ReadTOPForHost { City = city, Name = advisorName, RegionCode = regionCode } );
						this.ddlAdvisor.DataSource = list;
						this.ddlAdvisor.DataBind();

				}

				protected void OnLogin( object sender, EventArgs e )
				{
						if ( string.IsNullOrEmpty( this.ddlAdvisor.SelectedValue ) ) return;
						Session[HostSecurity.HostNameSessionName] = this.txtName.Text;
						Session[HostSecurity.HostAdvisorCodeSessionName] = this.ddlAdvisor.SelectedValue;

						//Update vyber TOP managera
						Organization advisor = Storage<Organization>.ReadFirst( new Organization.ReadByCode { Code = this.ddlAdvisor.SelectedValue } );
						if( advisor == null ) return;
						advisor.SelectedCount = advisor.SelectedCount.HasValue ? advisor.SelectedCount.Value + 1 : 1;
						Storage<Organization>.Update( advisor );

						AliasUtilities util = new AliasUtilities();
						string alias = util.Resolve( "~/user/host/default.aspx", this );
						if ( string.IsNullOrEmpty( alias ) ) Response.Redirect( ResolveUrl( "~/user/host/default.aspx" ) );
						else Response.Redirect( alias );
				}
		}
}
