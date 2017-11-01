using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Web.UI;

namespace CMS.Utilities
{
		public static class RadGridUtilities
		{
				public static void Localize( RadGrid grid )
				{
						string locale = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower();
						if ( locale == "en" ) return;

						LocalizeMasterTableView( grid );
						LocalizeFilteringMenu( grid );
						LocalizePager( grid );
						LocalizeGroupingPanel( grid );

						grid.ClientSettings.ClientMessages.DragToGroupOrReorder = Resources.RadGrid.RadGrid_ClientSettings_ClientMessages_DragToGroupOrReorder;
						grid.ClientSettings.ClientMessages.DragToResize = Resources.RadGrid.RadGrid_ClientSettings_ClientMessages_DragToResize;
						grid.ClientSettings.ClientMessages.DropHereToReorder = Resources.RadGrid.RadGrid_ClientSettings_ClientMessages_DropHereToReorder;
				}

				private static void LocalizeMasterTableView( RadGrid grid )
				{
						grid.MasterTableView.NoMasterRecordsText = Resources.RadGrid.RadGrid_MasterTableView_NoMasterRecordsText;
						grid.MasterTableView.NoDetailRecordsText = Resources.RadGrid.RadGrid_MasterTableView_NoDetailRecordsText;

						grid.MasterTableView.PagerStyle.FirstPageToolTip = Resources.RadGrid.RadGrid_PagerStyle_FirstPageToolTip;
						grid.MasterTableView.PagerStyle.NextPageToolTip = Resources.RadGrid.RadGrid_PagerStyle_NextPageToolTip;
						grid.MasterTableView.PagerStyle.NextPagesToolTip = Resources.RadGrid.RadGrid_PagerStyle_NextPagesToolTip;
						grid.MasterTableView.PagerStyle.PrevPageToolTip = Resources.RadGrid.RadGrid_PagerStyle_PrevPageToolTip;
						grid.MasterTableView.PagerStyle.PrevPagesToolTip = Resources.RadGrid.RadGrid_PagerStyle_PrevPagesToolTip;
						grid.MasterTableView.PagerStyle.LastPageToolTip = Resources.RadGrid.RadGrid_PagerStyle_LastPageToolTip;
						grid.MasterTableView.PagerStyle.PageSizeLabelText = Resources.RadGrid.RadGrid_PagerStyle_PageSizeLabelText;
						grid.MasterTableView.PagerStyle.PagerTextFormat = Resources.RadGrid.RadGrid_PagerStyle_PagerTextFormat;
				}

				private static void LocalizeGroupingPanel( RadGrid grid )
				{
						grid.GroupPanel.Text = Resources.RadGrid.RadGrid_GroupPanel_Text;
						grid.GroupPanel.ToolTip = Resources.RadGrid.RadGrid_GroupPanel_ToolTip;
						grid.GroupingSettings.UnGroupTooltip = Resources.RadGrid.RadGrid_GroupingSettings_UnGroupTooltip;
						grid.GroupingSettings.UnGroupButtonTooltip = Resources.RadGrid.RadGrid_GroupingSettings_UnGroupButtonTooltip;
				}

				public static void LocalizePager( RadGrid grid )
				{
						grid.PagerStyle.FirstPageToolTip = Resources.RadGrid.RadGrid_PagerStyle_FirstPageToolTip;
						grid.PagerStyle.NextPageToolTip = Resources.RadGrid.RadGrid_PagerStyle_NextPageToolTip;
						grid.PagerStyle.NextPagesToolTip = Resources.RadGrid.RadGrid_PagerStyle_NextPagesToolTip;
						grid.PagerStyle.PrevPageToolTip = Resources.RadGrid.RadGrid_PagerStyle_PrevPageToolTip;
						grid.PagerStyle.PrevPagesToolTip = Resources.RadGrid.RadGrid_PagerStyle_PrevPagesToolTip;
						grid.PagerStyle.LastPageToolTip = Resources.RadGrid.RadGrid_PagerStyle_LastPageToolTip;
						grid.PagerStyle.PageSizeLabelText = Resources.RadGrid.RadGrid_PagerStyle_PageSizeLabelText;
						grid.PagerStyle.PagerTextFormat = Resources.RadGrid.RadGrid_PagerStyle_PagerTextFormat;
				}

				public static void LocalizeFilteringMenu( RadGrid grid )
				{
						GridFilterMenu menu = grid.FilterMenu;
						foreach ( RadMenuItem item in menu.Items )
						{
								string text = item.Text;
								switch ( text )
								{
										case "NoFilter": text = "Nefiltrovať"; break;
										case "Contains": text = "Obsahuje"; break;
										case "DoesNotContain": text = "Neobsahuje"; break;
										case "StartsWith": text = "Začína na"; break;
										case "EndsWith": text = "Končí s"; break;
										case "EqualTo": text = "Je rovné (=)"; break;
										case "NotEqualTo": text = "Nie je rovné"; break;
										case "GreaterThan": text = "Väčší ako (>)"; break;
										case "LessThan": text = "Menší ako (<)"; break;
										case "GreaterThanOrEqualTo": text = "Väčší alebo rovný (>=)"; break;
										case "LessThanOrEqualTo": text = "Menší alebo rovný (<=)"; break;
										case "Between": text = "Medzi"; break;
										case "NotBetween": text = "Nie je medzi"; break;
										case "IsEmpty": text = "Je prázndy"; break;
										case "NotIsEmpty": text = "Nie je prázdny"; break;
										case "IsNull": text = "Je žiadny"; break;
										case "NotIsNull": text = "Nie je žiadny"; break;
								}
								item.Text = text;
						}
				}
		}
}
