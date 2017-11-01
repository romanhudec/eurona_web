using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Controls;
using System.Web.UI.WebControls;
using System.Web.UI;
using AttributeEntity = SHP.Entities.Attribute;

namespace SHP.Controls.Attribute
{
		public class AttributeTypeValueControl : CmsControl
		{
				public AttributeTypeValueControl()
				{
				}

				public int AttributeId
				{
						get
						{
								object o = ViewState["AttributeId"];
								return o != null ? (int)Convert.ToInt32( o ) : 0;
						}
						set { ViewState["AttributeId"] = value; }
				}

				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						AttributeEntity attribute = Storage<AttributeEntity>.ReadFirst( new AttributeEntity.ReadById { AttributeId = this.AttributeId } );
						if ( attribute == null )
								return;

						Control ctrl = this.CreateInputControls( attribute );
						if ( ctrl != null )
								this.Controls.Add( ctrl );

				}

				private Control CreateInputControls( AttributeEntity attribute )
				{
						Control inpuControl = null;
						switch ( attribute.Type )
						{
								case SHP.Entities.AttributeType.Type.String:
								case SHP.Entities.AttributeType.Type.Int:
								case SHP.Entities.AttributeType.Type.Double:
										inpuControl = CreateInputTextBox( attribute );
										break;
								case SHP.Entities.AttributeType.Type.Boolean:
										inpuControl = CreateInputRadionButton( attribute ); 
										break;
								case SHP.Entities.AttributeType.Type.Picture:
								case SHP.Entities.AttributeType.Type.Attachment:
										inpuControl = CreateInpuFile( attribute ); 
										break;
								case SHP.Entities.AttributeType.Type.Choice:
								case SHP.Entities.AttributeType.Type.MultiChoice:
										inpuControl = CreateInpuDropDownList( attribute ); 
										break;	
								default:
										inpuControl = null;
										break;
						}
						return inpuControl;
				}

				#region Private Input Methods
				private Control CreateInputTextBox( AttributeEntity attribute )
				{
						return new TextBox();
				}
				private Control CreateInputRadionButton( AttributeEntity attribute )
				{
						return new RadioButton();
				}
				private Control CreateInpuFile( AttributeEntity attribute )
				{
						return new FileUpload();
				}
				private Control CreateInpuDropDownList( AttributeEntity attribute )
				{
						return new DropDownList();
				}
				#endregion
		}
}
