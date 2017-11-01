using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities
{
		public class Menu: Entity
		{
				public class ReadById
				{
						public int MenuId { get; set; }
				}

				public class ReadForCurrentAccount
				{
				}

				public int InstanceId { get; set; }
				public string Name { get; set; }
				public string Code { get; set; }
				public string Locale { get; set; }

				//FK Role
				public int? RoleId { get; set; }
				private Role role = null;
				public Role Role
				{
						get
						{
								if ( !this.RoleId.HasValue ) return null;

								if ( role != null ) return role;
								role = Storage<Role>.ReadFirst( new Role.ReadById { RoleId = this.RoleId.Value } );
								return role;
						}
				}

				public string RoleName
				{
						get { return Role != null ? role.Name : String.Empty; }
				}
			
		}
}
