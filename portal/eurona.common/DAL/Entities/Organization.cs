using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eurona.Common.DAL.Entities
{
	/*
	ATP typ manažera = 1 - ATP manažer zlatý
	ATP typ manažera = 2 - ATP lector *
	ATP typ manažera = 3 - VIP maminka
	ATP typ manažera = 4 – VIP maminka/lector
	ATP typ manažera = 5 - ATP lector **
	ATP typ manažera = 6 - ATP lector ***
	ATP typ manažera = 7 - ATP manažer zlatý/lector
	 */
	public enum ATPManagerTyp : int
	{
		None = 0,
		Zlaty = 1,
		Lector1 = 2,
		Maminka = 3,
		MaminkaLector = 4,
		Lector2 = 5,
		Lector3 = 6,
		ZlatyLector = 7,
	}
	public class Organization : CMS.Entities.Organization
	{
		public const string EURONA_CODE = "420-5495234474";

        public static string GetLocaleByRegistrationCode(string code) {
            string locale = "cs";
            if (code.StartsWith("420")) {
                locale = "cs";
            } else if (code.StartsWith("421")) {
                locale = "sk";
            } else {
                locale = "pl";
            }
            return locale;
        }

		public DateTime Created { get; set; }
		/// <summary>
		/// TVD_Id accountu nadriadeneho
		/// </summary>
		public int? ParentId { get; set; }
		public string Code { get; set; }
		public bool VATPayment { get; set; }
		public int TopManager { get; set; }

		public string FAX { get; set; }
		public string Skype { get; set; }
		public string ICQ { get; set; }
		public DateTime? ContactBirthDay { get; set; }
		public string ContactCardId { get; set; }
		public string ContactWorkPhone { get; set; }
		public string PF { get; set; }
		public string RegionCode { get; set; }
		public int? SelectedCount { get; set; }

		/// <summary>
		/// Marza pouzivatela
		/// </summary>
		public decimal? UserMargin { get; set; }
		/// <summary>
		/// Obmedzeny pristup
		/// </summary>
		public int RestrictedAccess { get; set; }

		/// <summary>
		/// NRZ, NRP
		/// </summary>
		public string Statut { get; set; }

		/// <summary>
		/// TVD_Id accountu
		/// </summary>
		public int? TVD_Id { get; set; }

		public bool AnonymousRegistration { get; set; }
		public int? AnonymousAssignBy { get; set; }
        public DateTime? AnonymousTempAssignAt { get; set; }
		public DateTime? AnonymousAssignAt { get; set; }
		public string AnonymousAssignToCode { get; set; }

		public DateTime? AnonymousCreatedAt { get; set; }
		public string AnonymousAssignStatus{ get; set; }
		public string AnonymousAssignByCode{ get; set; }
		public string PredmetCinnosti{get;set;}
		public bool AnonymousOvereniSluzeb{get;set;}
		public bool AnonymousZmenaNaJineRegistracniCislo{get;set;}
		public string AnonymousZmenaNaJineRegistracniCisloText { get; set; }
		public bool AnonymousSouhlasStavajicihoPoradce{get;set;}
		public bool AnonymousSouhlasNavrzenehoPoradce{get;set;}

		public bool AngelTeamClen { get; set; }
		public bool AngelTeamManager { get; set; }
		public bool ManageAnonymousAssign { get; set; }
		public int? AngelTeamManagerTyp { get; set; }

		public bool ZasilaniTiskovin { get; set; }
		public bool ZasilaniNewsletter { get; set; }
		public bool ZasilaniKatalogu { get; set; }

        /// <summary>
        /// accountu Id
        /// </summary>
        public int? RegistrationFromCookiesLinkAccountId { get; set; }

		public bool IsLector
		{
			get
			{
				if (!this.AngelTeamManagerTyp.HasValue) return false;
				if (this.AngelTeamManagerTyp.Value == (int)ATPManagerTyp.Lector1 || this.AngelTeamManagerTyp.Value == (int)ATPManagerTyp.Lector2 || this.AngelTeamManagerTyp.Value == (int)ATPManagerTyp.Lector3 || this.AngelTeamManagerTyp.Value == (int)ATPManagerTyp.ZlatyLector)
					return true;
				else return false;
			}
		}

		public bool IsMaminka
		{
			get
			{
				if (!this.AngelTeamManagerTyp.HasValue) return false;
				if (this.AngelTeamManagerTyp.Value == (int)ATPManagerTyp.Maminka || this.AngelTeamManagerTyp.Value == (int)ATPManagerTyp.MaminkaLector)
					return true;
				else return false;
			}
		}

		/// <summary>
		/// Počet dní do odoslania overovacich udajov
		/// </summary>
		public int LeftToVerification
		{
			get
			{
				TimeSpan ts = (DateTime.Now - this.Created);
				int days = 14 - ts.Days;
				if (days < 0) return 0;

				return days;
			}
		}

		public class ReadByAngelTeam
		{
			public bool? AngelTeamClen { get; set; }
			public bool? AngelTeamManager { get; set; }
			public int? AngelTeamManagerTyp { get; set; }
		}


		public class ReadBy
		{
			public string Code { get; set; }
			public string Name { get; set; }
			public string City { get; set; }
			public string RegionCode { get; set; }
			public bool? Top { get; set; }
			public int? ParentId { get; set; }
			public bool? Verified { get; set; }
			public bool? ZasilaniTiskovin { get; set; }
			public bool? ZasilaniNewsletter { get; set; }
			public bool? ZasilaniKatalogu { get; set; }
		}

		public class ReadByTVDId
		{
			public int TVD_Id { get; set; }
			public int? InstanceId { get; set; }
		}

		public class ReadByCode
		{
			public string Code { get; set; }
		}
		public class ReadTOP { }
		public class ReadTOPForHost
		{
			public string Name { get; set; }
			public string City { get; set; }
			public string RegionCode { get; set; }
		}

		public class ReadByAnonymous
		{
            public string RegionCode { get; set; }
			public bool AnonymousRegistration { get; set; }
			public bool? Assigned { get; set; }
			public bool? AssignedAndConfirmed { get; set; }
			public DateTime? AnonymousCreatedAt { get; set; }
			public DateTime? AnonymousAssignAt { get; set; }
            public DateTime? AnonymousTempAssignAt { get; set; }

			public int? CreatedAtDay { get; set; }
			public int? CreatedAtMonth { get; set; }
			public int? CreatedAtYear { get; set; }

			public int? NotAssignedDays { get; set; }
			public int? NotAssignedHours { get; set; }
			public int? NotAssignedMinutes { get; set; }
		}

		public class ReadAnonymousAssignManager
		{
		}
	}
}

