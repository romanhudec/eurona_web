using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Eurona.DAL.Entities
{
	public class Account : CMS.Entities.Account
	{
		public string InstanceName
		{
			get
			{
				switch (this.InstanceId)
				{
					case 1:
						return "Eurona";
					case 2:
						return "Intenza";
					default:
						return string.Empty;
				}
			}
		}

		public class ReadByLoginAndInstance
		{
			public string Login { get; set; }
			public int InstanceId { get; set; }
		}

		public int? TVD_Id { get; set; }
		public bool CanAccessIntensa { get; set; }
		public DateTime Created { get; set; }
        public bool MustChangeAccountPassword{ get; set; }
        public DateTime? PasswordChanged { get; set; }

		private AccountExt accountExtension = null;
		public AccountExt AccountExtension
		{
			get
			{
				if (accountExtension != null) return accountExtension;
				accountExtension = Storage<AccountExt>.ReadFirst(new AccountExt.ReadByAccount { AccountId = this.Id });
				return accountExtension;
			}
			set { this.accountExtension = value; }
		}

        public bool SingleUserCookieLinkEnabled { get; set; }

		/*
		public class CmdGetAccountRoles
		{
				public int AccountId { get; set; }
				public string Result { get; set; }
		}

		private List<Role> roles = new List<Role>();
		public new List<Role> Roles
		{
				get 
				{
						EnsureRoles();
						return roles; 
				}
				set { roles = value; }
		}

		public new string[] RoleArray
		{
				get
				{
						return roles.ConvertAll<string>( r => r.Name ).ToArray();
				}
				set
				{
						roles.Clear();
						if ( value != null && value.Length > 0 )
								foreach ( String r in value )
										roles.Add( new Role { Name = r } );
				}
		}


		private string roleString = null;
		public new string RoleString
		{
				get
				{
						if ( roleString != null )
						{
								StringBuilder sb = new StringBuilder();
								roles.ForEach( r => sb.AppendFormat( "{0};", r.Name ) );
								return sb.ToString();
						}
						else
						{
								EnsureRoles();
								return roleString;
						}
				}
				set
				{
						RoleArray = value.Split( new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries );
				}
		}

		/// <summary>
		/// Methoda potiahne RoleString
		/// </summary>
		private void EnsureRoles()
		{
				if ( this.roles.Count == 0 )
				{
						Account.CmdGetAccountRoles cmd = new Account.CmdGetAccountRoles();
						cmd.AccountId = this.Id;
						cmd = Storage<Account>.Execute( cmd );
						this.roleString = cmd.Result;
						this.RoleArray = roleString.Split( new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries );
				}
		}

		/// <summary>
		/// Property na zobraznie rolestringu napr. v GridView
		/// </summary>
		public new string RoleStringDisplay
		{
				get
				{
						return RoleString.Replace( ";", "; " );
				}
		}

		public new bool IsInRole( string role )
		{
				return this.RoleString.Contains( role );
		}

		public new void AddToRoles( params string[] roles )
		{
				StringBuilder sb = new StringBuilder();
				sb.Append( this.RoleString );

				foreach ( string role in roles )
						sb.AppendFormat( "{0};", role );

				this.RoleString = sb.ToString();
		}

		public new void RemoveFromRole( string role )
		{
				StringBuilder sb = new StringBuilder();
				foreach ( Role r in roles )
				{
						if ( r.Name != role )
								sb.AppendFormat( "{0};", r.Name );
				}
				this.RoleString = sb.ToString();
		}
		*/
	}
}
