using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UzavierkaEntity = Eurona.Common.DAL.Entities.Uzavierka;

namespace Eurona.User.Anonymous
{
		public partial class CountDownControl: Eurona.Common.Controls.UserControl
		{
				private UzavierkaEntity uzavierka = null;
				//protected void Page_Load( object sender, EventArgs e )
				//{
				//}

				public string CssClass { get; set; }

				protected override void CreateChildControls()
				{
						base.CreateChildControls();
						
						this.divContainer.Attributes.Add( "class", this.CssClass );
						this.divContainer.Style.Add( "margin-bottom", "10px" );
						this.lblDnes.Text = string.Format( "Dnes je {0}", DateTime.Now.ToShortDateString() );

						if ( Eurona.Common.Application.EuronaUzavierka.IsUzavierka4Advisor() )
						{
								//this.Visible = false;
                            this.lblZbyvaInfo.Text = string.Format("Probíhá uzávěrka. Vytvářet objednávky bude možné : {0}", Eurona.Common.Application.EuronaUzavierka.GeUzavierka4AdvisorTo());
								return;
						}

						this.uzavierka = Storage<UzavierkaEntity>.ReadFirst( new UzavierkaEntity.ReadById { UzavierkaId = (int)UzavierkaEntity.UzavierkaId.Eurona } );
						if ( this.uzavierka == null || this.uzavierka.Povolena == false || this.uzavierka.UzavierkaOd.HasValue == false )
						{
								this.Visible = false;
								return;
						}

						if ( this.uzavierka.UzavierkaOd.Value.Year != DateTime.Now.Year || this.uzavierka.UzavierkaOd.Value.Month != DateTime.Now.Month )
						{
								this.Visible = false;
								return;
						}

						this.lblUzavierkaInfo.Text = string.Format( "Uzávěrka tohoto měsíce je {0} v {1:0#}:{2:0#} hod",
								this.uzavierka.UzavierkaOd.Value.ToShortDateString(),
								this.uzavierka.UzavierkaOd.Value.Hour,
								this.uzavierka.UzavierkaOd.Value.Minute );

						this.lblZbyvaInfo.Text = "Pro Vaši objednávku zbývá :";


						this.RegisterStartupCountDownScript( "cnt_container" );
				}

				private void RegisterStartupCountDownScript( string containerId )
				{
						ClientScriptManager cs = this.Page.ClientScript;
						Type cstype = this.Page.GetType();

						if ( !cs.IsStartupScriptRegistered( cstype, "CountDownManager" ) )
						{

								int seconds = (int)( this.uzavierka.UzavierkaOd.Value - DateTime.Now ).TotalSeconds;
								if ( seconds < 0 ) seconds = 0;

								string locale = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower();
								string countDownConstructors = string.Format( "cdm.add(new CountDown('{0}', {1}, '{2}'));\n", containerId, seconds, locale );

								string js = @"<script type='text/javascript'> 
								var cdm = new CountDownManager();
								var loadBase = window.onload;
								window.onload = function() { " +
										countDownConstructors
										+ @"cdm.start();
										if (loadBase != null) loadBase();
								}
								</script>";
								cs.RegisterStartupScript( cstype, "CountDownManager", js );
						}
				}
		}
}