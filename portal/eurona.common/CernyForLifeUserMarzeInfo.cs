using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Diagnostics;
using System.ComponentModel;
using CartEntity = Eurona.Common.DAL.Entities.Cart;

namespace Eurona.Common
{
		/// <summary>
		/// Treida popisujuca informacie INTENSA pouzivatela
		/// </summary>
		///

		public class CernyForLifeUserMarzeInfo
		{
				/*
				private const int sessionExpirationMinutes = 10;

				private DateTime datum;
				private int tvdId = 0;
				private System.Web.UI.Page page;
				private bool logUserMarzeInfo = false;
				public CernyForLifeUserMarzeInfo( System.Web.UI.Page page, int tvdId )
				{
						this.tvdId = tvdId;
						this.datum = Eurona.Common.Application.CurrentOrderDate;
						this.page = page;

						string logUserMarzeString = ConfigurationManager.AppSettings["LOG_UserMarzeInfo"];
						if ( !string.IsNullOrEmpty( logUserMarzeString ) )
						{
								if ( !Boolean.TryParse( logUserMarzeString, out logUserMarzeInfo ) )
										logUserMarzeInfo = true;
						}
				}

				public static decimal MinimalniPovolenaMarze { get { return 20m; } }
				public static decimal MaximalniPovolenaMarze { get { return 30m; } }
				 
				/// <summary>
				/// Metoda naplni data, informaciami z eurosap a zaktualizuje ceny INTENSA produktov v kosiku
				/// </summary>
				public bool GetMarzeInfoFromEurosap( int? cartId, out string errorMessage )
				{
						if ( this.page.Session["EUROSAP_LastAktualization"] != null )
								this.LastAktualization = Convert.ToDateTime( this.page.Session["EUROSAP_LastAktualization"] );

						//Ak doslo k zmen TVD_Id je potrebne vykonat prepocet nanovo
						if ( this.page.Session["EUROSAP_TVDId"] == null || Convert.ToInt32( this.page.Session["EUROSAP_TVDId"] ) != tvdId )
						{
								this.LastAktualization = DateTime.MinValue;
						}

						bool hasSessionValue = false;
						bool recalculate = false;
						TimeSpan tsSessionExpiration = ( DateTime.Now - this.LastAktualization );
						if ( tsSessionExpiration.Days != 0 || tsSessionExpiration.Hours != 0 || tsSessionExpiration.Minutes >= sessionExpirationMinutes )
								recalculate = true;

						if ( !recalculate )
						{
								//Zistim session z EUROSAP
								if ( this.page.Session["EUROSAP_TVDId"] != null &&
										this.page.Session["EUROSAP_MarzeEurona"] != null &&
										this.page.Session["EUROSAP_MarzeIntensa"] != null &&
										this.page.Session["EUROSAP_ChybiBodu"] != null &&
										this.page.Session["EUROSAP_ChybiRegistraci"] != null )
								{
										hasSessionValue = true;

										this.MarzeEurona = Convert.ToDecimal( this.page.Session["EUROSAP_MarzeEurona"] );
										this.MarzeIntensa = Convert.ToDecimal( this.page.Session["EUROSAP_MarzeIntensa"] );
										this.ChybiBodu = Convert.ToInt32( this.page.Session["EUROSAP_ChybiBodu"] );
										this.ChybiRegistraci = Convert.ToInt32( this.page.Session["EUROSAP_ChybiRegistraci"] );

										//Logovanie
										string logMessage = string.Format( "TVD_Id={0}", this.tvdId );
										logMessage += string.Format( "\nEUROSAP->MarzeEurona={0}, MarzeIntensa={1}, ChybiBodu={2}, ChybiRegistraci={3}",
												this.MarzeEurona, this.MarzeIntensa, this.ChybiBodu, this.ChybiRegistraci );

										//Update podla kosiku eurona
										UpdateByIntensaCart( cartId );

										//Logovanie
										logMessage += string.Format( "\nEURONA->MarzeEurona={0}, MarzeIntensa={1}, ChybiBodu={2}, ChybiRegistraci={3}",
												this.MarzeEurona, this.MarzeIntensa, this.ChybiBodu, this.ChybiRegistraci );

										if ( cartId.HasValue )
										{
												CartEntity cart = Storage<CartEntity>.ReadFirst( new CartEntity.ReadById { CartId = cartId.Value } );
												if ( cart != null ) logMessage += string.Format( "\nCartId={0},Discount={1}", cart.Id, cart.Discount );
										}

										if( logUserMarzeInfo ) CMS.EvenLog.WritoToEventLog( logMessage, EventLogEntryType.Information );

										errorMessage = string.Empty;
										return true;
								}
						}

						//Ak je nutne hodnoty nacitat zo SAP
						if ( recalculate || hasSessionValue == false )
						{
								errorMessage = string.Empty;//"Synchronizace se vzdáleným servrem (SAP) byla neúspěšná!";
								bool bSuccess = false;

								//Procedura esp_www_marze:
								//@out_Probehlo bit OUTPUT, -- 1=úspěch, 0=selhání
								//@out_Zprava nvarchar(255) OUTPUT, -- text OK nebo chybová zpráva
								//@out_Marze_Eurona numeric(5,2) OUTPUT, -- hodnota marže výrobků Eurona (20,25,30)
								//@out_Marze_Intensa numeric(5,2) OUTPUT, -- hodnota marže výrobků Intensa (20,25,30)
								//@out_Chybi_bodu int OUTPUT, -- kolik bodů v akt. měsíci zbývá pro dosažení marže Intensa stejné jako Eurona
								//@out_Chybi_registraci int OUTPUT, -- kolik registrací s min.obj. v akt. měsíci zbývá pro dosažení marže Intensa stejné jako Eurona

								//@Id_odberatele int, -- id odběratele
								//@Datum datetime -- aktuální datum (možno datovat zpětně, třeba při podpoře zadávání objednávek do min. měsíce...)

								SqlParameter probehlo = new SqlParameter( "@out_Probehlo", false );
								probehlo.Direction = ParameterDirection.Output;

								SqlParameter zprava = new SqlParameter( "@out_Zprava", string.Empty );
								zprava.Direction = ParameterDirection.Output;
								zprava.SqlDbType = SqlDbType.VarChar;
								zprava.Size = 255;

								SqlParameter marze_Eurona = new SqlParameter( "@out_Marze_Eurona", 0m );
								marze_Eurona.Direction = ParameterDirection.Output;
								marze_Eurona.SqlDbType = SqlDbType.Decimal;

								SqlParameter marze_Intensa = new SqlParameter( "@out_Marze_Intensa", 0m );
								marze_Intensa.Direction = ParameterDirection.Output;
								marze_Intensa.SqlDbType = SqlDbType.Decimal;

								SqlParameter chybi_bodu = new SqlParameter( "@out_Chybi_bodu", -1 );
								chybi_bodu.Direction = ParameterDirection.Output;

								SqlParameter chybi_registraci = new SqlParameter( "@out_Chybi_registraci", -1 );
								chybi_registraci.Direction = ParameterDirection.Output;

								try
								{
#if !__DEBUG_VERSION_WITHOUTTVD
										string connectionString = ConfigurationManager.ConnectionStrings["TVDConnectionString"].ConnectionString;
										CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage( connectionString );
										using ( SqlConnection connection = tvdStorage.Connect() )
										{
												tvdStorage.ExecProc( connection, "esp_www_marze",
														new SqlParameter( "Id_odberatele", this.tvdId ),
														new SqlParameter( "Datum", this.datum ),
														probehlo, zprava, marze_Eurona, marze_Intensa, chybi_bodu, chybi_registraci );
										}
#else
										probehlo.Value = 1;
										zprava.Value = string.Empty;
										marze_Eurona.Value = 25m;
										marze_Intensa.Value = 0m;
										chybi_bodu.Value = 100;
										chybi_registraci.Value = 0;
#endif
								}
								catch ( Exception ex )
								{
										CMS.EvenLog.WritoToEventLog( ex );
										if ( zprava.Value != null ) errorMessage = zprava.Value.ToString();
										else errorMessage = "Synchronizace se vzdáleným servrem (SAP) byla neúspěšná!";

										return false;
								}

								//===============================================================================
								//Vystupne parametra
								//===============================================================================
								//Probehlo	bit	1=úspěch, 0=chyba		
								//Zprava	varchar(255)	text chyby		
								bSuccess = Convert.ToBoolean( probehlo.Value );
								if ( zprava != null && zprava.Value != null )
								{
										int instanceId = 0;
										Int32.TryParse( ConfigurationManager.AppSettings["InstanceId"], out instanceId );
										Eurona.Common.Application.InstanceType instnaceType = (Eurona.Common.Application.InstanceType)instanceId;

										string msg = string.Format( "{0}:INTENSA User -> esp_www_marze(Id_odberatele:{1}) = {2}", instnaceType.ToString(), this.tvdId, zprava.Value.ToString() );
										if ( logUserMarzeInfo ) CMS.EvenLog.WritoToEventLog( msg, EventLogEntryType.Information );
								}

								if ( bSuccess )
								{
										//Naplnenie session z EUROSAP
										this.page.Session["EUROSAP_MarzeEurona"] = Convert.ToDecimal( marze_Eurona.Value );
										this.page.Session["EUROSAP_MarzeIntensa"] = Convert.ToDecimal( marze_Intensa.Value );
										this.page.Session["EUROSAP_ChybiBodu"] = Convert.ToInt32( chybi_bodu.Value );
										this.page.Session["EUROSAP_ChybiRegistraci"] = Convert.ToInt32( chybi_registraci.Value );
										this.page.Session["EUROSAP_LastAktualization"] = DateTime.Now;
										this.page.Session["EUROSAP_TVDId"] = this.tvdId;
										this.LastAktualization = DateTime.Now;

										//Naplnenie property
										this.MarzeEurona = Convert.ToDecimal( marze_Eurona.Value );
										this.MarzeIntensa = Convert.ToDecimal( marze_Intensa.Value );
										this.ChybiBodu = Convert.ToInt32( chybi_bodu.Value );
										this.ChybiRegistraci = Convert.ToInt32( chybi_registraci.Value );

										//Logovanie
										string logMessage = string.Format( "TVD_Id={0}", this.tvdId );
										logMessage += string.Format( "\nEUROSAP->MarzeEurona={0}, MarzeIntensa={1}, ChybiBodu={2}, ChybiRegistraci={3}",
												this.MarzeEurona, this.MarzeIntensa, this.ChybiBodu, this.ChybiRegistraci );

										//Update podla kosiku eurona
										//Update nastavi propoerty this.MarzeEurona, ... na aktualne hodnoty.
										//!!! V Session ostavaju povodne hodnoty z EUROSAP
										UpdateByIntensaCart( cartId );

										//Logovanie
										logMessage += string.Format( "\nEURONA->MarzeEurona={0}, MarzeIntensa={1}, ChybiBodu={2}, ChybiRegistraci={3}",
												this.MarzeEurona, this.MarzeIntensa, this.ChybiBodu, this.ChybiRegistraci );

										if ( cartId.HasValue )
										{
												CartEntity cart = Storage<CartEntity>.ReadFirst( new CartEntity.ReadById { CartId = cartId.Value } );
												if ( cart != null ) logMessage += string.Format( "\nCartId={0},Discount={1}", cart.Id, cart.Discount );
										}
										if ( logUserMarzeInfo )  CMS.EvenLog.WritoToEventLog( logMessage, EventLogEntryType.Information );
								}

								if ( zprava.Value != null ) errorMessage = zprava.Value.ToString();
								return bSuccess;
						}

						errorMessage = "";
						return false;
				}
				/// <summary>
				/// Metoda upravi UserMargin Info podla aktualnych INTENSA produktov v kosiku
				/// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
				/// V Session musia ostat povodne hodnoty z EUROSAP !!!!!!!
				/// </summary>
				/// 
				private void UpdateByIntensaCart( int? cartId )
				{
						if ( !cartId.HasValue ) return;

						//pShpRecalculateIntensaCart
						//@CartId INT,
						//@MarzeEurona DECIMAL(19,2) = NULL,
						//@MarzeIntensa DECIMAL(19,2) = NULL,
						//@ChibiBodu INT = NULL,
						//@ChibiRegistraci INT = NULL,
						//@RecalculateCartProducts BIT = 0,
						//@out_MarzeEurona DECIMAL(19,2) = 0 OUTPUT,
						//@out_MarzeIntensa DECIMAL(19,2) = 0 OUTPUT,
						//@out_ChibiBodu INT = 0  OUTPUT,
						//@out_ChibiRegistraci INT = 0  OUTPUT

						SqlParameter marze_Eurona = new SqlParameter( "@out_MarzeEurona", 0m );
						marze_Eurona.Direction = ParameterDirection.Output;
						marze_Eurona.SqlDbType = SqlDbType.Decimal;

						SqlParameter marze_Intensa = new SqlParameter( "@out_MarzeIntensa", 0m );
						marze_Intensa.Direction = ParameterDirection.Output;
						marze_Intensa.SqlDbType = SqlDbType.Decimal;

						SqlParameter chybi_bodu = new SqlParameter( "@out_ChibiBodu", -1 );
						chybi_bodu.Direction = ParameterDirection.Output;

						SqlParameter chybi_registraci = new SqlParameter( "@out_ChibiRegistraci", -1 );
						chybi_registraci.Direction = ParameterDirection.Output;

						try
						{
								string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
								CMS.Pump.MSSQLStorage storage = new CMS.Pump.MSSQLStorage( connectionString );
								using ( SqlConnection connection = storage.Connect() )
								{
										storage.ExecProc( connection, "pShpRecalculateEuronaCart",
												new SqlParameter( "CartId", cartId.Value ),
												new SqlParameter( "MarzeEurona", this.MarzeEurona ),
												new SqlParameter( "MarzeIntensa", this.MarzeIntensa ),
												new SqlParameter( "ChibiBodu", this.ChybiBodu ),
												new SqlParameter( "ChibiRegistraci", this.ChybiRegistraci ),
												new SqlParameter( "RecalculateCartProducts", 1 ),
												marze_Eurona, marze_Intensa, chybi_bodu, chybi_registraci );
								}

								this.MarzeEurona = Convert.ToDecimal( marze_Eurona.Value );
								this.MarzeIntensa = Convert.ToDecimal( marze_Intensa.Value );
								this.ChybiBodu = Convert.ToInt32( chybi_bodu.Value );
								this.ChybiRegistraci = Convert.ToInt32( chybi_registraci.Value );
						}
						catch ( Exception ex )
						{
								CMS.EvenLog.WritoToEventLog( ex );
								throw ex;
						}
				}
				/// <summary>
				///  hodnota marže výrobků Eurona (20,25,30)
				/// </summary>
				[DefaultValue( 0 )]
				public decimal MarzeEurona { get; set; }
				/// <summary>
				/// hodnota marže výrobků Intensa (20,25,30)
				/// </summary>
				[DefaultValue( 0 )]
				public decimal MarzeIntensa { get; set; }
				/// <summary>
				/// kolik bodů v akt. měsíci zbývá pro dosažení marže Intensa stejné jako Eurona
				/// </summary>
				[DefaultValue( 0 )]
				public int ChybiBodu { get; set; }

				/// <summary>
				/// kolik registrací s min.obj. v akt. měsíci zbývá pro dosažení marže Intensa stejné jako Eurona
				/// </summary>
				[DefaultValue( 0 )]
				public int ChybiRegistraci { get; set; }

				/// <summary>
				/// Datum poslednej aktualizacie z EUROSAP
				/// </summary>
				public DateTime LastAktualization { get; set; }
				*/
		}
}