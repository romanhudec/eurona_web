using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities
{
	public class ForumThread : Entity, IUrlAliasEntity
	{
		public ForumThread()
		{
			this.Locked = false;
			this.Alias = string.Empty;
			this.VisibleForRole = string.Empty;
			this.EditableForRole = string.Empty;
		}

		public class ReadById
		{
			public int ForumThreadId { get; set; }
		}
		public class ReadByObjectId
		{
			public int ObjectId { get; set; }
		}

		public int InstanceId { get; set; }
		public int? ObjectId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Icon { get; set; }
		public string Locale { get; set; }
		public bool Locked { get; set; }
		public string VisibleForRole { get; set; }
		public string EditableForRole { get; set; }
		public int ForumsCount { get; set; }
		public int ForumPostCount { get; set; }

		#region IUrlAliasEntity Members
		public int? UrlAliasId { get; set; }
		public string Alias { get; set; }
		#endregion
	}
}
