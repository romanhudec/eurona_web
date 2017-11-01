using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities
{
		public class ForumPost: Entity
		{
				public ForumPost()
				{
				}

				public class ReadById
				{
						public int ForumPostId { get; set; }
				}
				public class ReadByForumId
				{
						public int ForumId { get; set; }
				}
				public class IncrementVoteCommand
				{
						public int AccountId { get; set; }
						public int ObjectTypeId { get; set; }
						public int ForumPostId { get; set; }
						public int Rating { get; set; }
				}

				public int ForumId { get; set; }
				public int InstanceId { get; set; }
				public int? ParentId { get; set; }
				public int AccountId { get; set; }
				public string IPAddress{ get; set; }
				public DateTime Date{ get; set; }
				public string Title{ get; set; }
				public string Content{ get; set; }

				public string AccountName { get; set; }
				public int Votes { get; set; }
				public int TotalRating { get; set; }
				public double RatingResult { get; set; }

				public static AccountVote.ObjectType AccountVoteType { get { return AccountVote.ObjectType.ForumPost; } }
				
				//FK
				private List<ForumPostAttachment> attachments = null;
				public List<ForumPostAttachment> Attachments
				{
						get
						{
								if ( this.attachments != null ) return this.attachments;
								this.attachments = Storage<ForumPostAttachment>.Read( new ForumPostAttachment.ReadBy { ForumPostId = this.Id } );
								return this.attachments;
						}
				}
		}
}
