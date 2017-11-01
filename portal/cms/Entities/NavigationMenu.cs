using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities
{
		public class NavigationMenu : Entity
		{
				public class ReadById
				{
						public int NavigationMenuId { get; set; }
				}
				public class ReadByAlias
				{
						public int UrlAliasId { get; set; }
				}
				public class ReadForCurrentAccount
				{
						public string Code { get; set; }
				}

				public class ReadByMenuId
				{
						public int MenuId { get; set; }
				}

				public int InstanceId { get; set; }
				public int MenuId { get; set; }
				public string Name { get; set; }
				public string Locale { get; set; }
				public int? Order { get; set; }
				public string Icon { get; set; }

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

				//FK UrlAlias
				public int? UrlAliasId { get; set; }
				public string Alias { get; set; }
		}
}
