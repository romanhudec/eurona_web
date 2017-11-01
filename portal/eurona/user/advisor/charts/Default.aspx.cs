using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Telerik.Charting;
using System.Drawing;

namespace Eurona.User.Advisor.Charts
{
		public partial class Default: ChartPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						// set the plot area gradient background fill
						int obdobiOd = DateTime.Now.Year * 100;

						DataTable dt = null;
						CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage( base.ConnectionString );
						using ( SqlConnection connection = tvdStorage.Connect() )
						{
								string sql = @"SELECT p.RRRRMM,p.Id_odberatele,p.Id_nadrizeneho,p.Objem_vlastni,p.Objem_celkem,p.Objem_os,p.Body_vlastni,p.Body_os,p.Body_celkem, p.Hladina,
										p.Hladina,p.Vnoreni,p.Odpustit_hranici,p.Bonus1,p.Bonus2,p.Bonus3,p.Bonus4,p.Statut,p.Provize_vlastni,p.Provize_skupina,p.Provize_leader,p.Provize_celk_pred_kracenim,
										p.Provize_celk_po_kraceni,p.Provize_celk_po_kraceni_mena,p.Provize_celk_po_kraceni_kod_meny,p.Provize_mimo ,p.Provize_mimo_mena ,p.Provize_mimo_kod_meny ,p.Provize_vyplata,
										p.Provize_vyplata_mena ,p.Provize_vyplata_kod_meny ,p.Stav ,p.Marze_platna ,p.Marze_mena ,p.Marze_kod_meny ,p.Marze_nasledujici ,p.Novy ,p.Pocet_novych ,p.Pocet_novych_s_objednavkou,
										p.Mesicu_bez_objednavky ,p.Poradi ,p.Eurokredit_vlastni ,p.Eurokredit_registrace ,p.Eurokredit_vyber
										FROM provize_finalni p
										WHERE p.RRRRMM > @RRRRMM AND  p.Id_odberatele=@Id_odberatele";
								//Clear data
								dt = tvdStorage.Query( connection, sql,
										new SqlParameter( "@RRRRMM", obdobiOd ),
										new SqlParameter( "@Id_odberatele", this.LogedAdvisor.TVD_Id ) );
						}

						if ( dt != null )
						{
								#region Vyvoj celkove provize
								chartCelekoveProzize.ChartTitle.TextBlock.Text = Resources.Reports.Chart_VyvojCelkoveProvize;
								chartCelekoveProzize.Legend.Visible = false;

								ChartSeries chartSeries = new ChartSeries();
								chartSeries.Appearance.LineSeriesAppearance.Color = Color.Red;
								chartSeries.Appearance.FillStyle.MainColor = Color.Red;
								chartSeries.Appearance.FillStyle.SecondColor = Color.OrangeRed;
								chartSeries.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
								chartSeries.Type = ChartSeriesType.Line;
								chartSeries.Name = string.Empty;
								chartSeries.Appearance.ShowLabelConnectors = false;
								chartSeries.Appearance.ShowLabels = false;

								// visually enhance the data points
								chartSeries.Appearance.PointMark.Dimensions.Width = 5;
								chartSeries.Appearance.PointMark.Dimensions.Height = 5;
								chartSeries.Appearance.PointMark.FillStyle.MainColor = System.Drawing.Color.Red;
								chartSeries.Appearance.PointMark.FillStyle.SecondColor = System.Drawing.Color.Red;
								chartSeries.Appearance.PointMark.Visible = true;

								foreach ( DataRow dr in dt.Rows )
								{
										ChartSeriesItem item = new ChartSeriesItem();
										item.Label.Visible = false;
										item.XValue = Convert.ToDouble( dr["RRRRMM"] ) - obdobiOd;
										item.YValue = Convert.ToDouble( dr["Provize_skupina"] );
										chartSeries.AddItem( item );
								}
								chartCelekoveProzize.Series.Add( chartSeries );
								chartSeries.PlotArea.XAxis.AxisLabel.Visible = true;
								chartSeries.PlotArea.XAxis.AxisLabel.TextBlock.Text =  Resources.Reports.Mesic_Column.ToLower();
								chartCelekoveProzize.DataBind();
								#endregion

								#region Provizni hladiny
								chartProvizniHladiny.ChartTitle.TextBlock.Text = string.Format( Resources.Reports.Chart_PostupVKariere, (int)( obdobiOd / 100 ) );
								chartProvizniHladiny.Legend.Visible = false;

								ChartSeries csPh = new ChartSeries();
								csPh.Appearance.LineSeriesAppearance.Color = Color.Red;
								csPh.Appearance.FillStyle.MainColor = Color.Red;
								csPh.Appearance.FillStyle.SecondColor = Color.OrangeRed;
								csPh.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
								csPh.Type = ChartSeriesType.Line;
								csPh.Name = string.Empty;
								csPh.Appearance.ShowLabelConnectors = false;
								csPh.Appearance.ShowLabels = false;

								// visually enhance the data points
								csPh.Appearance.PointMark.Dimensions.Width = 5;
								csPh.Appearance.PointMark.Dimensions.Height = 5;
								csPh.Appearance.PointMark.FillStyle.MainColor = System.Drawing.Color.Red;
								csPh.Appearance.PointMark.FillStyle.SecondColor = System.Drawing.Color.Red;
								csPh.Appearance.PointMark.Visible = true;

								foreach ( DataRow dr in dt.Rows )
								{
										ChartSeriesItem item = new ChartSeriesItem();
										item.Label.Visible = false;
										item.XValue = Convert.ToDouble( dr["RRRRMM"] ) - obdobiOd;
										item.YValue = Convert.ToDouble( dr["Hladina"] );
										csPh.AddItem( item );
								}
								chartProvizniHladiny.Series.Add( csPh );
								csPh.PlotArea.XAxis.AxisLabel.Visible = true;
								csPh.PlotArea.XAxis.AxisLabel.TextBlock.Text =  Resources.Reports.Mesic_Column.ToLower();
								csPh.PlotArea.YAxis.AddRange( 0, 21, 3 );
								csPh.PlotArea.YAxis.AutoScale = false;
								chartProvizniHladiny.DataBind();
								#endregion

								#region Vývoj obratu osobních objednávek
								cartVlastniObjednavky.ChartTitle.TextBlock.Text = string.Format( Resources.Reports.Chart_VyvojOsobnichObjednavek, (int)( obdobiOd / 100 ) );
								cartVlastniObjednavky.Legend.Visible = false;

								ChartSeries csVooo = new ChartSeries();
								csVooo.Appearance.LineSeriesAppearance.Color = Color.Red;
								csVooo.Appearance.FillStyle.MainColor = Color.Red;
								csVooo.Appearance.FillStyle.SecondColor = Color.OrangeRed;
								csVooo.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
								csVooo.Type = ChartSeriesType.Bar;
								csVooo.Name = string.Empty;
								csVooo.Appearance.ShowLabelConnectors = false;
								csVooo.Appearance.ShowLabels = false;

								// visually enhance the data points
								csVooo.Appearance.PointMark.Dimensions.Width = 5;
								csVooo.Appearance.PointMark.Dimensions.Height = 5;
								csVooo.Appearance.PointMark.FillStyle.MainColor = System.Drawing.Color.Red;
								csVooo.Appearance.PointMark.FillStyle.SecondColor = System.Drawing.Color.Red;
								csVooo.Appearance.PointMark.Visible = true;

								foreach ( DataRow dr in dt.Rows )
								{
										ChartSeriesItem item = new ChartSeriesItem();
										item.Label.Visible = false;
										item.XValue = Convert.ToDouble( dr["RRRRMM"] ) - obdobiOd;
										item.YValue = Convert.ToDouble( dr["Objem_vlastni"] );
										csVooo.AddItem( item );
								}
								cartVlastniObjednavky.Series.Add( csVooo );
								csVooo.PlotArea.XAxis.AxisLabel.Visible = true;
								csVooo.PlotArea.XAxis.AxisLabel.TextBlock.Text =  Resources.Reports.Mesic_Column.ToLower();
								cartVlastniObjednavky.DataBind();
								#endregion

								#region Vývoj obratu skupiny
								cartObratSkupiny.ChartTitle.TextBlock.Text = string.Format( Resources.Reports.Chart_VyvojObratuSkupiny, (int)( obdobiOd / 100 ) );
								cartObratSkupiny.Legend.Visible = false;

								ChartSeries csVos = new ChartSeries();
								csVos.Appearance.LineSeriesAppearance.Color = Color.Orange;
								csVos.Appearance.FillStyle.MainColor = Color.Orange;
								csVos.Appearance.FillStyle.SecondColor = Color.Orange;
								csVos.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
								csVos.Type = ChartSeriesType.Line;

								csVos.Name = string.Empty;
								csVos.Appearance.ShowLabelConnectors = false;
								csVos.Appearance.ShowLabels = false;

								// visually enhance the data points
								csVos.Appearance.PointMark.Dimensions.Width = 5;
								csVos.Appearance.PointMark.Dimensions.Height = 5;
								csVos.Appearance.PointMark.FillStyle.MainColor = System.Drawing.Color.Orange;
								csVos.Appearance.PointMark.FillStyle.SecondColor = System.Drawing.Color.Orange;
								csVos.Appearance.PointMark.Visible = true;

								foreach ( DataRow dr in dt.Rows )
								{
										ChartSeriesItem item = new ChartSeriesItem();
										item.Label.Visible = false;
										item.XValue = Convert.ToDouble( dr["RRRRMM"] ) - obdobiOd;
										item.YValue = Convert.ToDouble( dr["Objem_celkem"] );
										csVos.AddItem( item );
								}
								cartObratSkupiny.Series.Add( csVos );
								csVos.PlotArea.XAxis.AxisLabel.Visible = true;
								csVos.PlotArea.XAxis.AxisLabel.TextBlock.Text =  Resources.Reports.Mesic_Column.ToLower();
								cartObratSkupiny.DataBind();
								#endregion

								#region Vývoj skupinových bodů
								cartVyvojSkupinBodu.ChartTitle.TextBlock.Text = string.Format( Resources.Reports.Chart_VyvojSkupinovychBodu, (int)( obdobiOd / 100 ) );
								cartVyvojSkupinBodu.Legend.Visible = false;

								ChartSeries csVsb = new ChartSeries();
								csVsb.Appearance.FillStyle.MainColor = Color.Orange;
								csVsb.Appearance.FillStyle.SecondColor = Color.Yellow;
								csVsb.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;

								csVsb.Type = ChartSeriesType.Bar;
								csVsb.Name = string.Empty;
								csVsb.Appearance.ShowLabelConnectors = false;
								csVsb.Appearance.ShowLabels = false;

								// visually enhance the data points
								csVsb.Appearance.PointMark.Dimensions.Width = 5;
								csVsb.Appearance.PointMark.Dimensions.Height = 5;
								csVsb.Appearance.PointMark.FillStyle.MainColor = System.Drawing.Color.Orange;
								csVsb.Appearance.PointMark.FillStyle.SecondColor = System.Drawing.Color.Orange;
								csVsb.Appearance.PointMark.Visible = false;
								foreach ( DataRow dr in dt.Rows )
								{
										ChartSeriesItem item = new ChartSeriesItem();
										item.Label.Visible = false;
										item.XValue = Convert.ToDouble( dr["RRRRMM"] ) - obdobiOd;
										item.YValue = Convert.ToDouble( dr["Body_celkem"] );
										csVsb.AddItem( item );
								}
								cartVyvojSkupinBodu.Series.Add( csVsb );
								csVsb.PlotArea.XAxis.AxisLabel.Visible = true;
								csVsb.PlotArea.XAxis.AxisLabel.TextBlock.Text =  Resources.Reports.Mesic_Column.ToLower();
								cartVyvojSkupinBodu.DataBind();
								#endregion

								#region Vývoj přidelených leaderských bonusů
								cartLeaderBonus.ChartTitle.TextBlock.Text = string.Format( Resources.Reports.Chart_VyvojPridelenychLeaderskychBosusu, (int)( obdobiOd / 100 ) );
								cartLeaderBonus.Legend.Visible = false;

								ChartSeries csVplb = new ChartSeries();
								csVplb.Appearance.FillStyle.MainColor = Color.Orange;
								csVplb.Appearance.FillStyle.SecondColor = Color.Yellow;
								csVplb.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;

								csVplb.Type = ChartSeriesType.Bar;
								csVplb.Name = string.Empty;
								csVplb.Appearance.ShowLabelConnectors = false;
								csVplb.Appearance.ShowLabels = false;

								// visually enhance the data points
								csVplb.Appearance.PointMark.Dimensions.Width = 5;
								csVplb.Appearance.PointMark.Dimensions.Height = 5;
								csVplb.Appearance.PointMark.FillStyle.MainColor = System.Drawing.Color.Red;
								csVplb.Appearance.PointMark.Visible = false;

								foreach ( DataRow dr in dt.Rows )
								{
										ChartSeriesItem item = new ChartSeriesItem();
										item.Label.Visible = false;
										item.XValue = Convert.ToDouble( dr["RRRRMM"] ) - obdobiOd;
										item.YValue = Convert.ToDouble( dr["Provize_leader"] );
										csVplb.AddItem( item );
								}
								cartLeaderBonus.Series.Add( csVplb );
								csVplb.PlotArea.XAxis.AxisLabel.Visible = true;
								csVplb.PlotArea.XAxis.AxisLabel.TextBlock.Text =  Resources.Reports.Mesic_Column.ToLower();
								cartLeaderBonus.DataBind();
								#endregion

								#region Vývoj vlastních nových registrací
								cartVyvojVlastnichNovychRegistraci.ChartTitle.TextBlock.Text = string.Format( Resources.Reports.Chart_VyvojVlastnichNovychRegistraci, (int)( obdobiOd / 100 ) );
								cartVyvojVlastnichNovychRegistraci.Legend.Visible = false;

								ChartSeries csVvnr = new ChartSeries();
								csVvnr.Appearance.FillStyle.MainColor = Color.Green;
								csVvnr.Appearance.FillStyle.SecondColor = Color.LightGreen;
								csVvnr.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;

								csVvnr.Type = ChartSeriesType.Bar;
								csVvnr.Name = string.Empty;
								csVvnr.Appearance.ShowLabelConnectors = false;
								csVvnr.Appearance.ShowLabels = false;

								// visually enhance the data points
								csVvnr.Appearance.PointMark.Dimensions.Width = 5;
								csVvnr.Appearance.PointMark.Dimensions.Height = 5;
								csVvnr.Appearance.PointMark.FillStyle.MainColor = System.Drawing.Color.Red;
								csVvnr.Appearance.PointMark.Visible = false;

								foreach ( DataRow dr in dt.Rows )
								{
										ChartSeriesItem item = new ChartSeriesItem();
										item.Label.Visible = false;
										item.XValue = Convert.ToDouble( dr["RRRRMM"] ) - obdobiOd;
										item.YValue = Convert.ToDouble( dr["Pocet_novych"] );
										csVvnr.AddItem( item );
								}
								cartVyvojVlastnichNovychRegistraci.Series.Add( csVvnr );

								csVvnr.PlotArea.XAxis.AxisLabel.Visible = true;
								csVvnr.PlotArea.XAxis.AxisLabel.TextBlock.Text =  Resources.Reports.Mesic_Column.ToLower();

								cartVyvojVlastnichNovychRegistraci.DataBind();
								#endregion
						}

						#region Vývoj celkových nových registrací skupiny
						DataTable dtNoveRegistaceCelkem = null; 
						using ( SqlConnection connection = tvdStorage.Connect() )
						{
								string sql = @"SELECT p.RRRRMM, Pocet_novych = SUM( p.Pocet_novych )
										FROM provize_finalni p
										WHERE p.RRRRMM > @RRRRMM AND  ( p.Id_odberatele=@Id_odberatele OR p.Id_nadrizeneho=@Id_odberatele )
										GROUP BY p.RRRRMM";
								//Clear data
								dtNoveRegistaceCelkem = tvdStorage.Query( connection, sql,
										new SqlParameter( "@RRRRMM", obdobiOd ),
										new SqlParameter( "@Id_odberatele", this.LogedAdvisor.TVD_Id ) );
						}
						if ( dtNoveRegistaceCelkem != null )
						{
								#region Vývoj celkových nových registrací skupiny
								cartVyvojNovychRegistraciSkupiny.ChartTitle.TextBlock.Text = string.Format( Resources.Reports.Chart_VyvojNovychRegistraciSkupiny, (int)( obdobiOd / 100 ) );
								cartVyvojNovychRegistraciSkupiny.Legend.Visible = false;

								ChartSeries csVcnrs = new ChartSeries();
								csVcnrs.Appearance.FillStyle.MainColor = Color.Blue;
								csVcnrs.Appearance.FillStyle.SecondColor = Color.LightSkyBlue;
								csVcnrs.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;

								csVcnrs.Type = ChartSeriesType.Bar;
								csVcnrs.Name = string.Empty;
								csVcnrs.Appearance.ShowLabelConnectors = false;
								csVcnrs.Appearance.ShowLabels = false;

								// visually enhance the data points
								csVcnrs.Appearance.PointMark.Dimensions.Width = 5;
								csVcnrs.Appearance.PointMark.Dimensions.Height = 5;
								csVcnrs.Appearance.PointMark.FillStyle.MainColor = System.Drawing.Color.Red;
								csVcnrs.Appearance.PointMark.Visible = false;

								double maxX = 0;
								foreach ( DataRow dr in dtNoveRegistaceCelkem.Rows )
								{
										ChartSeriesItem item = new ChartSeriesItem();
										item.Label.Visible = false;
										item.XValue = Convert.ToDouble( dr["RRRRMM"] ) - obdobiOd;
										item.YValue = Convert.ToDouble( dr["Pocet_novych"] );
										csVcnrs.AddItem( item );

										Math.Max( maxX, item.XValue + 1 );
								}
								cartVyvojNovychRegistraciSkupiny.PlotArea.XAxis.MaxValue = maxX;
								cartVyvojNovychRegistraciSkupiny.Series.Add( csVcnrs );

								csVcnrs.PlotArea.XAxis.AxisLabel.Visible = true;
								csVcnrs.PlotArea.XAxis.AxisLabel.TextBlock.Text = Resources.Reports.Mesic_Column.ToLower();

								cartVyvojNovychRegistraciSkupiny.DataBind();
								#endregion
						}
						#endregion
				}
		}
}