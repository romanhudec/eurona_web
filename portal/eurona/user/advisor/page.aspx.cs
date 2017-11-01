using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS;
using PageEntity = CMS.Entities.Page;
using CMS.Entities;

namespace Eurona.User.Advisor
{
		public partial class GenericPage: WebPage
		{
				private PageEntity pageEntity;
				private PageEntity PageEntity
				{
						get
						{
								if ( pageEntity != null ) return pageEntity;
								string sid = Request["id"];
								if ( !String.IsNullOrEmpty( sid ) )
								{
										int id = -1;
										if ( Int32.TryParse( sid, out id ) )
												pageEntity = Storage<PageEntity>.ReadFirst( new PageEntity.ReadById { PageId = id } );
								}
								else
								{
										string name = Server.UrlDecode( Request["name"] );
										if ( string.IsNullOrEmpty( name ) )
										{
												CMS.EvenLog.WritoToEventLog( string.Format( "Page has not query parameter 'name'!{0}. Request:{1}", this.Request.UrlReferrer, this.Request.RawUrl ), System.Diagnostics.EventLogEntryType.Error );
												return null;
										}
										pageEntity = Storage<PageEntity>.ReadFirst( new PageEntity.ReadByName { Name = name } );
								}
								return pageEntity;
						}
				}

				protected void Page_PreInit( object sender, EventArgs e )
				{
						if ( this.PageEntity != null ){
								this.Page.MasterPageFile = this.PageEntity.MasterPage.Url;
						}
				}

				protected void Page_Load( object sender, EventArgs e )
				{
						if ( this.PageEntity != null )
						{
								this.Title = this.PageEntity.Title;
								genericPage.PageName = this.PageEntity.Name;
								aThisPage.Text = this.PageEntity.Title;
								this.lblTitle.Text = this.PageEntity.Title;
						}
				}
		}
}
