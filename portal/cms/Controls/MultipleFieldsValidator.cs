/* 
Author: Adam Tibi
Date: 11 March 2006
Email: aztibi@gmail.com
*/

using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Permissions;

[assembly: WebResource( "CMS.Controls.MultipleFieldsValidator.js", "application/x-javascript" )]
namespace CMS.Controls
{

		//[AspNetHostingPermission( SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal )]
		//[AspNetHostingPermission( SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal )]
		[ToolboxData( @"<{0}:MultipleFieldsValidator runat=server></{0}:MultipleFieldsValidator>" )]
		public class MultipleFieldsValidator: BaseValidator
		{

				#region Overriden Methods

				protected override void AddAttributesToRender( System.Web.UI.HtmlTextWriter writer )
				{

						// Setting SetFocusOnError before calling the base AddAttributesToRender because
						// the AddAttributesToRender is going to check for "SetFocusOnError" value
						base.SetFocusOnError = false; // Because we have many fields to check!

						base.AddAttributesToRender( writer );

						if ( this.RenderUplevel )
						{
								string clientID = this.ClientID;

								Page.ClientScript.RegisterExpandoAttribute( clientID, "evaluationfunction", "MultipleFieldsValidatorEvaluateIsValid" );
								Page.ClientScript.RegisterExpandoAttribute( clientID, "controlstovalidate", this.GenerateClientSideControlsToValidate() );
								Page.ClientScript.RegisterExpandoAttribute( clientID, "condition", PropertyConverter.EnumToString( typeof( Conditions ), Condition ) );
						}
				}

				protected override bool ControlPropertiesValid()
				{

						if ( this.ControlsToValidate.Trim().Length == 0 )
						{
								throw new HttpException( string.Format( "The ControlsToValidate property of {0} cannot be blank.", this.ID ) );
						}

						string[] controlToValidateIDs = this.GetControlsToValidateIDs();
						if ( controlToValidateIDs.Length < 2 )
						{
								throw new HttpException( string.Format( "The ControlsToValidate property of {0} has less than two IDs.", this.ID ) );
						}

						foreach ( string controlToValidateID in controlToValidateIDs )
						{
								base.CheckControlValidationProperty( controlToValidateID, "ControlsToValidate" );
						}

						return true;
				}

				protected override bool EvaluateIsValid()
				{

						string[] controlToValidateIDs = this.GetControlsToValidateIDs();

						switch ( Condition )
						{
								case Conditions.OR:
										foreach ( string controlToValidateID in controlToValidateIDs )
										{
												string controlToValidateValue = base.GetControlValidationValue( controlToValidateID );
												controlToValidateValue = controlToValidateValue == null ? string.Empty : controlToValidateValue.Trim();
												if ( controlToValidateValue != string.Empty )
												{
														return true;
												}
										}
										return false;
								case Conditions.XOR:
										// Taking the initial values
										bool previousResult = false;
										bool passedFirstElement = false;
										// if one of the compared values is not like the other, then they are not all the same,
										// then they satisfy the XOR condition
										foreach ( string controlToValidateID in controlToValidateIDs )
										{
												string controlToValidateValue = base.GetControlValidationValue( controlToValidateID );
												controlToValidateValue = controlToValidateValue == null ? string.Empty : controlToValidateValue.Trim();
												if ( !passedFirstElement )
												{
														previousResult = controlToValidateValue != string.Empty;
														passedFirstElement = true;
														continue;
												}
												bool currentResult = controlToValidateValue != string.Empty;
												if ( previousResult != currentResult )
												{
														return true;
												}
												previousResult = currentResult;
										}
										return false;
								case Conditions.AND:
										foreach ( string controlToValidateID in controlToValidateIDs )
										{
												string controlToValidateValue = base.GetControlValidationValue( controlToValidateID );
												controlToValidateValue = controlToValidateValue == null ? string.Empty : controlToValidateValue.Trim();
												if ( controlToValidateValue == string.Empty )
												{
														return false;
												}
										}
										return true;
								default:
										// This line shouldn't be reached
										throw new Exception( "End of validation has been reached without a result!" );
						}
				}

				protected override void OnPreRender( EventArgs e )
				{
						base.OnPreRender( e );
						if ( base.RenderUplevel )
						{

								ClientScriptManager cs = this.Page.ClientScript;
								Type cstype = this.GetType();
								string urlInclude = cs.GetWebResourceUrl( cstype, "CMS.Controls.MultipleFieldsValidator.js" );
								cs.RegisterClientScriptInclude( cstype, "MultipleFieldsValidatorJs", urlInclude );
						}
				}

				#endregion

				#region Helper Methods

				private string[] GetControlsToValidateIDs()
				{
						string controlsToValidate = this.ControlsToValidate.Replace( " ", "" );
						string[] controlToValidateIDs;
						try
						{
								controlToValidateIDs = controlsToValidate.Split( ',' );
						}
						catch ( ArgumentOutOfRangeException ex )
						{
								throw new FormatException( string.Format( "The ControlsToValidate property of {0} is not well-formatted.", this.ID ), ex );
						}
						return controlToValidateIDs;
				}

				private string GenerateClientSideControlsToValidate()
				{
						string[] controlToValidateIDs = this.GetControlsToValidateIDs();
						string controlToValidateIDTrimmed;
						string controlRenderIDs = string.Empty;
						foreach ( string controlToValidateID in controlToValidateIDs )
						{
								controlToValidateIDTrimmed = controlToValidateID.Trim();
								if ( controlToValidateIDTrimmed == string.Empty )
								{
										throw new FormatException( string.Format( "The ControlsToValidate property of {0} is not well-formatted.", this.ID ) );
								}
								controlRenderIDs += "," + base.GetControlRenderID( controlToValidateIDTrimmed );
						}
						controlRenderIDs = controlRenderIDs.Remove( 0, 1 ); // Removing the first ","
						return controlRenderIDs;
				}

				#endregion

				#region Properties

				[Browsable( false )]
				[EditorBrowsable( EditorBrowsableState.Never )]
				public new bool SetFocusOnError
				{
						get
						{
								return false;
						}
						set
						{
								throw new NotSupportedException( "SetFocusOnError is not supported because you have multiple controls to validate" );
						}
				}

				[Browsable( false )]
				[EditorBrowsable( EditorBrowsableState.Never )]
				public new string ControlToValidate
				{
						get
						{
								return string.Empty;
						}
						set
						{
								throw new NotSupportedException( "ControlToValidate is not supported because you have multiple controls to validate" );
						}
				}

				/// <summary>
				/// Comma separated list of control IDs that you want to check
				/// </summary>
				[Browsable( true )]
				[Category( "Behavior" )]
				[Themeable( false )]
				[DefaultValue( "" )]
				[Description( "Comma separated list of control IDs that you want to check" )]
				public string ControlsToValidate
				{
						get
						{
								return (string)( ViewState["ControlsToValidate"] ?? string.Empty );
						}
						set
						{
								ViewState["ControlsToValidate"] = value;
						}
				}

				/// <summary>
				/// The condition used to compare the value of the fields, 
				/// e.g. 'OR', will return true if at least one field is valid
				/// </summary>
				[Browsable( true )]
				[Themeable( false )]
				[Category( "Behavior" )]
				[DefaultValue( Conditions.OR )]
				[Description( "The condition used to compare the value of the fields, e.g. 'OR', will return true if at least one field is valid" )]
				public Conditions Condition
				{
						get
						{
								return (Conditions)( ViewState["Condition"] ?? Conditions.OR );
						}
						set
						{
								ViewState["Condition"] = value;
						}
				}

				#endregion

				#region Enum

				public enum Conditions
				{
						/// <summary>
						/// OR condition
						/// </summary>
						OR,
						/// <summary>
						/// XOR Condition
						/// </summary>
						XOR,
						/// <summary>
						/// AND Condition
						/// </summary>
						AND
				}
				#endregion
		}

}
