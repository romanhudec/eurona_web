using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Eurona.Common.DAL.Entities;
using CMS.MSSQL;

namespace Eurona.Common.DAL.MSSQL {
    [Serializable]
    public sealed class OrganizationStorage : MSSQLStorage<Organization> {
        public OrganizationStorage(int instanceId, CMS.Entities.Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        internal static Organization GetOrganization(DataRow record) {
            Organization org = new Organization();
            org.Id = Convert.ToInt32(record["OrganizationId"]);
            org.InstanceId = Convert.ToInt32(record["InstanceId"]);
            org.AccountId = ConvertNullable.ToInt32(record["AccountId"]);
            org.Created = Convert.ToDateTime(record["Created"]);
            org.Id1 = Convert.ToString(record["Id1"]);
            org.Id2 = Convert.ToString(record["Id2"]);
            org.Id3 = Convert.ToString(record["Id3"]);
            org.Web = Convert.ToString(record["Web"]);
            org.Name = Convert.ToString(record["Name"]);
            org.RegisteredAddressId = ConvertNullable.ToInt32(record["RegisteredAddressId"]);
            org.RegisteredAddressString = Convert.ToString(record["RegisteredAddressString"]);
            org.CorrespondenceAddressId = ConvertNullable.ToInt32(record["CorrespondenceAddressId"]);
            org.CorrespondenceAddressString = Convert.ToString(record["CorrespondenceAddressString"]);
            org.InvoicingAddressId = ConvertNullable.ToInt32(record["InvoicingAddressId"]);
            org.InvoicingAddressString = Convert.ToString(record["InvoicingAddressString"]);
            org.BankContactId = ConvertNullable.ToInt32(record["BankContactId"]);
            org.ContactPersonId = ConvertNullable.ToInt32(record["ContactPersonId"]);
            org.ContactPersonString = Convert.ToString(record["ContactPersonString"]);
            org.ContactEmail = Convert.ToString(record["ContactEmail"]);
            org.ContactPhone = Convert.ToString(record["ContactPhone"]);
            org.ContactMobile = Convert.ToString(record["ContactMobile"]);
            org.Notes = ConvertNullable.ToString(record["Notes"]);

            org.ParentId = ConvertNullable.ToInt32(record["ParentId"]);
            org.Code = ConvertNullable.ToString(record["Code"]);
            org.VATPayment = Convert.ToBoolean(record["VATPayment"]);
            org.TVD_Id = ConvertNullable.ToInt32(record["TVD_Id"]);
            org.TopManager = Convert.ToInt32(record["TopManager"]);

            org.FAX = Convert.ToString(record["FAX"]);
            org.Skype = Convert.ToString(record["Skype"]);
            org.ICQ = Convert.ToString(record["ICQ"]);
            org.ContactBirthDay = ConvertNullable.ToDateTime(record["ContactBirthDay"]);
            org.ContactCardId = Convert.ToString(record["ContactCardId"]);
            org.ContactWorkPhone = Convert.ToString(record["ContactWorkPhone"]);
            org.PF = Convert.ToString(record["PF"]);
            org.RegionCode = Convert.ToString(record["RegionCode"]);
            org.UserMargin = ConvertNullable.ToDecimal(record["UserMargin"]);
            org.RestrictedAccess = Convert.ToInt32(record["RestrictedAccess"]);
            org.Statut = Convert.ToString(record["Statut"]);
            org.SelectedCount = ConvertNullable.ToInt32(record["SelectedCount"]);

            org.AnonymousRegistration = Convert.ToBoolean(record["AnonymousRegistration"] == DBNull.Value ? false : record["AnonymousRegistration"]);
            org.AnonymousCreatedAt = ConvertNullable.ToDateTime(record["AnonymousCreatedAt"]);
            org.AnonymousAssignBy = ConvertNullable.ToInt32(record["AnonymousAssignBy"]);
            org.AnonymousAssignByCode = Convert.ToString(record["AnonymousAssignByCode"]);
            org.AnonymousTempAssignAt = ConvertNullable.ToDateTime(record["AnonymousTempAssignAt"]);
            org.AnonymousAssignAt = ConvertNullable.ToDateTime(record["AnonymousAssignAt"]);
            org.AnonymousAssignToCode = Convert.ToString(record["AnonymousAssignToCode"]);
            org.AnonymousAssignStatus = Convert.ToString(record["AnonymousAssignStatus"]);

            org.PredmetCinnosti = Convert.ToString(record["PredmetCinnosti"]);
            org.AnonymousOvereniSluzeb = Convert.ToBoolean(record["AnonymousOvereniSluzeb"]);
            org.AnonymousZmenaNaJineRegistracniCislo = Convert.ToBoolean(record["AnonymousZmenaNaJineRegistracniCislo"]);
            org.AnonymousZmenaNaJineRegistracniCisloText = Convert.ToString(record["AnonymousZmenaNaJineRegistracniCisloText"]);
            org.AnonymousSouhlasStavajicihoPoradce = Convert.ToBoolean(record["AnonymousSouhlasStavajicihoPoradce"]);
            org.AnonymousSouhlasNavrzenehoPoradce = Convert.ToBoolean(record["AnonymousSouhlasNavrzenehoPoradce"]);

            org.AngelTeamClen = Convert.ToBoolean(record["Angel_team_clen"]);
            org.AngelTeamManager = Convert.ToBoolean(record["Angel_team_manager"]);
            org.AngelTeamManagerTyp = ConvertNullable.ToInt32(record["Angel_team_manager_typ"]);
            org.ManageAnonymousAssign = Convert.ToBoolean(record["ManageAnonymousAssign"] == DBNull.Value ? false : record["ManageAnonymousAssign"]);

            org.ZasilaniTiskovin = Convert.ToBoolean(record["ZasilaniTiskovin"]);
            org.ZasilaniNewsletter = Convert.ToBoolean(record["ZasilaniNewsletter"]);
            org.ZasilaniKatalogu = Convert.ToBoolean(record["ZasilaniKatalogu"]);
            org.RegistrationFromCookiesLinkAccountId = ConvertNullable.ToInt32(record["RegistrationFromCookiesLinkAccountId"]);

            return org;
        }

        public override List<Organization> Read(object criteria) {
            if (criteria is Organization.ReadById) return LoadById(criteria as Organization.ReadById);
            if (criteria is Organization.ReadByAccountId) return LoadByAccountId(criteria as Organization.ReadByAccountId);
            if (criteria is Organization.ReadBy) return LoadBy(criteria as Organization.ReadBy);
            if (criteria is Organization.ReadByTVDId) return LoadByTVDId(criteria as Organization.ReadByTVDId);
            if (criteria is Organization.ReadByCode) return LoadByCode(criteria as Organization.ReadByCode);
            if (criteria is Organization.ReadTOP) return LoadTOP(criteria as Organization.ReadTOP);
            if (criteria is Organization.ReadTOPForHost) return LoadTOPForHost(criteria as Organization.ReadTOPForHost);
            if (criteria is Organization.ReadByAnonymous) return LoadByAnonymous(criteria as Organization.ReadByAnonymous);
            if (criteria is Organization.ReadByAngelTeam) return LoadByAngelTeam(criteria as Organization.ReadByAngelTeam);
            if (criteria is Organization.ReadAnonymousAssignManager) return LoadAnonymousAssignManager();
            List<Organization> orgs = new List<Organization>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
					SELECT	o.AccountId, o.OrganizationId, o.InstanceId,
							o.Id1, o.Id2, o.Id3, o.Name, o.Notes, o.Web,
							o.RegisteredAddressId, o.CorrespondenceAddressId, o.InvoicingAddressId,
							RegisteredAddressString = dbo.fFormatAddress(ra.Street, ra.Zip, ra.City),
							CorrespondenceAddressString = dbo.fFormatAddress(ca.Street, ca.Zip, ca.City),
							InvoicingAddressString = dbo.fFormatAddress(ia.Street, ia.Zip, ia.City),
							BankContactId = o.BankContactId,
							ContactPersonId = o.ContactPersonId, 
							ContactPersonString = dbo.fFormatPerson(cp.FirstName, cp.LastName, ''),
							ContactEMail = o.ContactEMail, ContactPhone = o.ContactPhone, ContactMobile = o.ContactMobile,
							o.ParentId, o.Code, o.VATPayment, o.TVD_Id, o.TopManager,
							o.FAX, o.Skype, o.ICQ, o.ContactBirthDay, o.ContactCardId, o.ContactWorkPhone, o.PF, o.RegionCode, o.UserMargin, o.Statut, o.RestrictedAccess,o.Created, o.SelectedCount,
							o.AnonymousRegistration, o.AnonymousAssignBy, o.AnonymousAssignAt, o.AnonymousTempAssignAt, o.AnonymousAssignToCode, o.AnonymousCreatedAt, o.AnonymousAssignByCode, o.AnonymousAssignStatus, o.ManageAnonymousAssign,
							o.Angel_team_clen, o.Angel_team_manager, o.Angel_team_manager_typ,
							o.PredmetCinnosti, o.AnonymousOvereniSluzeb, o.AnonymousZmenaNaJineRegistracniCislo, o.AnonymousZmenaNaJineRegistracniCisloText, o.AnonymousSouhlasStavajicihoPoradce, o.AnonymousSouhlasNavrzenehoPoradce,
							o.ZasilaniTiskovin, o.ZasilaniNewsletter, o.ZasilaniKatalogu, o.RegistrationFromCookiesLinkAccountId
					FROM vOrganizations o
					LEFT JOIN tPerson cp (NOLOCK) ON o.ContactPersonId = cp.PersonId
					LEFT JOIN tAddress ra (NOLOCK) ON o.RegisteredAddressId = ra.AddressId
					LEFT JOIN tAddress ca (NOLOCK) ON o.CorrespondenceAddressId = ca.AddressId
					LEFT JOIN tAddress ia (NOLOCK) ON o.InvoicingAddressId = ia.AddressId
					WHERE o.InstanceId=@InstanceId
					ORDER BY o.Name ASC";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows) orgs.Add(GetOrganization(dr));
            }
            return orgs;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        private List<Organization> LoadById(Organization.ReadById byId) {
            List<Organization> orgs = new List<Organization>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
					SELECT	o.AccountId, o.OrganizationId, o.InstanceId,
							o.Id1, o.Id2, o.Id3, o.Name, o.Notes, o.Web,
							o.RegisteredAddressId, o.CorrespondenceAddressId, o.InvoicingAddressId,
							RegisteredAddressString = dbo.fFormatAddress(ra.Street, ra.Zip, ra.City),
							CorrespondenceAddressString = dbo.fFormatAddress(ca.Street, ca.Zip, ca.City),
							InvoicingAddressString = dbo.fFormatAddress(ia.Street, ia.Zip, ia.City),
							BankContactId = o.BankContactId,
							ContactPersonId = o.ContactPersonId, 
							ContactPersonString = dbo.fFormatPerson(cp.FirstName, cp.LastName, ''),
							ContactEMail = o.ContactEMail, ContactPhone = o.ContactPhone, ContactMobile = o.ContactMobile,
							o.ParentId, o.Code, o.VATPayment, o.TVD_Id, o.TopManager,
							o.FAX, o.Skype, o.ICQ, o.ContactBirthDay, o.ContactCardId, o.ContactWorkPhone, o.PF, o.RegionCode, o.UserMargin, o.Statut, o.RestrictedAccess,o.Created, o.SelectedCount,
							o.AnonymousRegistration, o.AnonymousAssignBy, o.AnonymousAssignAt, o.AnonymousTempAssignAt, o.AnonymousAssignToCode, o.AnonymousCreatedAt, o.AnonymousAssignByCode, o.AnonymousAssignStatus, o.ManageAnonymousAssign,  
							o.Angel_team_clen, o.Angel_team_manager, o.Angel_team_manager_typ,
							o.PredmetCinnosti, o.AnonymousOvereniSluzeb, o.AnonymousZmenaNaJineRegistracniCislo, o.AnonymousZmenaNaJineRegistracniCisloText, o.AnonymousSouhlasStavajicihoPoradce, o.AnonymousSouhlasNavrzenehoPoradce,
							o.ZasilaniTiskovin, o.ZasilaniNewsletter, o.ZasilaniKatalogu, o.RegistrationFromCookiesLinkAccountId
					FROM vOrganizations o
					LEFT JOIN tPerson cp (NOLOCK) ON o.ContactPersonId = cp.PersonId
					LEFT JOIN tAddress ra (NOLOCK) ON o.RegisteredAddressId = ra.AddressId
					LEFT JOIN tAddress ca (NOLOCK) ON o.CorrespondenceAddressId = ca.AddressId
					LEFT JOIN tAddress ia (NOLOCK) ON o.InvoicingAddressId = ia.AddressId
					WHERE o.OrganizationId = @OrganizationId";
                DataTable table = Query<DataTable>(connection, sql, new SqlParameter("@OrganizationId", byId.OrganizationId));
                foreach (DataRow dr in table.Rows) orgs.Add(GetOrganization(dr));
            }
            return orgs;
        }
        private List<Organization> LoadByAccountId(Organization.ReadByAccountId byAccountId) {
            List<Organization> orgs = new List<Organization>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
					SELECT	o.AccountId, o.OrganizationId, o.InstanceId, 
							o.Id1, o.Id2, o.Id3, o.Name, o.Notes, o.Web,
							o.RegisteredAddressId, o.CorrespondenceAddressId, o.InvoicingAddressId,
							RegisteredAddressString = dbo.fFormatAddress(ra.Street, ra.Zip, ra.City),
							CorrespondenceAddressString = dbo.fFormatAddress(ca.Street, ca.Zip, ca.City),
							InvoicingAddressString = dbo.fFormatAddress(ia.Street, ia.Zip, ia.City),
							BankContactId = o.BankContactId,
							ContactPersonId = o.ContactPersonId, 
							ContactPersonString = dbo.fFormatPerson(cp.FirstName, cp.LastName, ''),
							ContactEMail = o.ContactEMail, ContactPhone = o.ContactPhone, ContactMobile = o.ContactMobile,
							o.ParentId, o.Code, o.VATPayment, o.TVD_Id, o.TopManager,
							o.FAX, o.Skype, o.ICQ, o.ContactBirthDay, o.ContactCardId, o.ContactWorkPhone, o.PF, o.RegionCode, o.UserMargin, o.Statut, o.RestrictedAccess,o.Created, o.SelectedCount,
							o.AnonymousRegistration, o.AnonymousAssignBy, o.AnonymousAssignAt, o.AnonymousTempAssignAt, o.AnonymousAssignToCode, o.AnonymousCreatedAt, o.AnonymousAssignByCode, o.AnonymousAssignStatus, o.ManageAnonymousAssign,  
							o.Angel_team_clen, o.Angel_team_manager, o.Angel_team_manager_typ,
							o.PredmetCinnosti, o.AnonymousOvereniSluzeb, o.AnonymousZmenaNaJineRegistracniCislo, o.AnonymousZmenaNaJineRegistracniCisloText, o.AnonymousSouhlasStavajicihoPoradce, o.AnonymousSouhlasNavrzenehoPoradce,
							o.ZasilaniTiskovin, o.ZasilaniNewsletter, o.ZasilaniKatalogu, o.RegistrationFromCookiesLinkAccountId
					FROM vOrganizations o
					LEFT JOIN tPerson cp (NOLOCK) ON o.ContactPersonId = cp.PersonId
					LEFT JOIN tAddress ra (NOLOCK) ON o.RegisteredAddressId = ra.AddressId
					LEFT JOIN tAddress ca (NOLOCK) ON o.CorrespondenceAddressId = ca.AddressId
					LEFT JOIN tAddress ia (NOLOCK) ON o.InvoicingAddressId = ia.AddressId
					WHERE o.AccountId = @AccountId/* AND o.InstanceId=@InstanceId*/";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@AccountId", byAccountId.AccountId));
                //new SqlParameter( "@InstanceId", InstanceId ) );
                foreach (DataRow dr in table.Rows) orgs.Add(GetOrganization(dr));
            }
            return orgs;
        }
        private List<Organization> LoadByAnonymous(Organization.ReadByAnonymous by) {
            List<Organization> orgs = new List<Organization>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
					SELECT	o.AccountId, o.OrganizationId, o.InstanceId, 
							o.Id1, o.Id2, o.Id3, o.Name, o.Notes, o.Web,
							o.RegisteredAddressId, o.CorrespondenceAddressId, o.InvoicingAddressId,
							RegisteredAddressString = dbo.fFormatAddress(ra.Street, ra.Zip, ra.City),
							CorrespondenceAddressString = dbo.fFormatAddress(ca.Street, ca.Zip, ca.City),
							InvoicingAddressString = dbo.fFormatAddress(ia.Street, ia.Zip, ia.City),
							BankContactId = o.BankContactId,
							ContactPersonId = o.ContactPersonId, 
							ContactPersonString = dbo.fFormatPerson(cp.FirstName, cp.LastName, ''),
							ContactEMail = o.ContactEMail, ContactPhone = o.ContactPhone, ContactMobile = o.ContactMobile,
							o.ParentId, o.Code, o.VATPayment, o.TVD_Id, o.TopManager,
							o.FAX, o.Skype, o.ICQ, o.ContactBirthDay, o.ContactCardId, o.ContactWorkPhone, o.PF, o.RegionCode, o.UserMargin, o.Statut, o.RestrictedAccess,o.Created, o.SelectedCount,
							o.AnonymousRegistration, o.AnonymousAssignBy, o.AnonymousAssignAt, o.AnonymousTempAssignAt, o.AnonymousAssignToCode, o.AnonymousCreatedAt, o.AnonymousAssignByCode, o.AnonymousAssignStatus, o.ManageAnonymousAssign,
							o.Angel_team_clen, o.Angel_team_manager, o.Angel_team_manager_typ,
							o.PredmetCinnosti, o.AnonymousOvereniSluzeb, o.AnonymousZmenaNaJineRegistracniCislo, o.AnonymousZmenaNaJineRegistracniCisloText, o.AnonymousSouhlasStavajicihoPoradce, o.AnonymousSouhlasNavrzenehoPoradce,
							o.ZasilaniTiskovin, o.ZasilaniNewsletter, o.ZasilaniKatalogu, o.RegistrationFromCookiesLinkAccountId
					FROM vOrganizations o
					LEFT JOIN tPerson cp (NOLOCK) ON o.ContactPersonId = cp.PersonId
					LEFT JOIN tAddress ra (NOLOCK) ON o.RegisteredAddressId = ra.AddressId
					LEFT JOIN tAddress ca (NOLOCK) ON o.CorrespondenceAddressId = ca.AddressId
					LEFT JOIN tAddress ia (NOLOCK) ON o.InvoicingAddressId = ia.AddressId
					WHERE o.AnonymousRegistration = @AnonymousRegistration AND
                    ( @RegionCode IS NULL OR o.RegionCode=@RegionCode ) AND
					( @CreatedAtYear IS NULL OR ( YEAR(o.AnonymousCreatedAt)=@CreatedAtYear ) ) AND
					( @CreatedAtMonth IS NULL OR ( MONTH(o.AnonymousCreatedAt)=@CreatedAtMonth ) ) AND
					( @CreatedAtDay IS NULL OR ( DAY(o.AnonymousCreatedAt)=@CreatedAtDay ) ) AND
					( @AnonymousCreatedAt IS NULL OR ( YEAR(o.AnonymousCreatedAt)=YEAR(@AnonymousCreatedAt) AND MONTH(o.AnonymousCreatedAt)=MONTH(@AnonymousCreatedAt) AND DAY(o.AnonymousCreatedAt)=DAY(@AnonymousCreatedAt) ) ) AND
                    ( @AnonymousCreatedFrom IS NULL OR ( YEAR(o.AnonymousCreatedAt)>=YEAR(@AnonymousCreatedFrom) AND MONTH(o.AnonymousCreatedAt)>=MONTH(@AnonymousCreatedFrom) AND DAY(o.AnonymousCreatedAt)>=DAY(@AnonymousCreatedFrom) ) ) AND
                    ( @AnonymousCreatedTo IS NULL OR ( YEAR(o.AnonymousCreatedAt)<=YEAR(@AnonymousCreatedTo) AND MONTH(o.AnonymousCreatedAt)<=MONTH(@AnonymousCreatedTo) AND DAY(o.AnonymousCreatedAt)<=DAY(@AnonymousCreatedTo) ) ) AND
					( @AnonymousAssignAt IS NULL OR ( YEAR(o.AnonymousAssignAt)=YEAR(@AnonymousAssignAt) AND MONTH(o.AnonymousAssignAt)=MONTH(@AnonymousAssignAt) AND DAY(o.AnonymousAssignAt)=DAY(@AnonymousAssignAt) ) ) AND
                    ( @AnonymousTempAssignAt IS NULL OR ( YEAR(o.AnonymousTempAssignAt)=YEAR(@AnonymousTempAssignAt) AND MONTH(o.AnonymousTempAssignAt)=MONTH(@AnonymousTempAssignAt) AND DAY(o.AnonymousTempAssignAt)=DAY(@AnonymousTempAssignAt) ) ) AND
					( @Assigned IS NULL OR (@Assigned = 1 AND o.AnonymousAssignBy IS NOT NULL ) OR (@Assigned = 0 AND o.AnonymousAssignBy IS NULL ) ) AND
					( @AssignedAndConfirmed IS NULL OR (@AssignedAndConfirmed = 1 AND o.AnonymousAssignAt IS NOT NULL ) OR (@AssignedAndConfirmed = 0 AND o.AnonymousAssignAt IS NULL ) ) AND
					( @NotAssignedMinutesTotal IS NULL OR DATEDIFF(MINUTE, o.Created, GETDATE()) < @NotAssignedMinutesTotal )
                    ORDER BY o.AnonymousOvereniSluzeb ASC";

                int? notAssignedMinutesTotal = 0;
                if (by.NotAssignedDays.HasValue)
                    notAssignedMinutesTotal += ((by.NotAssignedDays.Value * 24) * 60);
                if (by.NotAssignedHours.HasValue)
                    notAssignedMinutesTotal += (by.NotAssignedHours.Value * 60);
                if (by.NotAssignedMinutes.HasValue)
                    notAssignedMinutesTotal += by.NotAssignedMinutes.Value;
                if (notAssignedMinutesTotal == 0) notAssignedMinutesTotal = null;

                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@AnonymousRegistration", by.AnonymousRegistration),
                        new SqlParameter("@AnonymousTempAssignAt", Null(by.AnonymousTempAssignAt)),
                        new SqlParameter("@AnonymousAssignAt", Null(by.AnonymousAssignAt)),
                        new SqlParameter("@Assigned", Null(by.Assigned)),
                        new SqlParameter("@AssignedAndConfirmed", Null(by.AssignedAndConfirmed)),
                        new SqlParameter("@AnonymousCreatedAt", Null(by.AnonymousCreatedAt)),
                        new SqlParameter("@CreatedAtDay", Null(by.CreatedAtDay)),
                        new SqlParameter("@CreatedAtMonth", Null(by.CreatedAtMonth)),
                        new SqlParameter("@CreatedAtYear", Null(by.CreatedAtYear)),
                        new SqlParameter("@NotAssignedMinutesTotal", Null(notAssignedMinutesTotal)),
                        new SqlParameter("@RegionCode", Null(by.RegionCode)),
                        new SqlParameter("@AnonymousCreatedFrom", Null(by.AnonymousCreatedFrom)),
                        new SqlParameter("@AnonymousCreatedTo", Null(by.AnonymousCreatedTo))
                        );
                foreach (DataRow dr in table.Rows) orgs.Add(GetOrganization(dr));
            }
            return orgs;
        }
        private List<Organization> LoadByAngelTeam(Organization.ReadByAngelTeam by) {
            List<Organization> orgs = new List<Organization>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
					SELECT	o.AccountId, o.OrganizationId, o.InstanceId, 
							o.Id1, o.Id2, o.Id3, o.Name, o.Notes, o.Web,
							o.RegisteredAddressId, o.CorrespondenceAddressId, o.InvoicingAddressId,
							RegisteredAddressString = dbo.fFormatAddress(ra.Street, ra.Zip, ra.City),
							CorrespondenceAddressString = dbo.fFormatAddress(ca.Street, ca.Zip, ca.City),
							InvoicingAddressString = dbo.fFormatAddress(ia.Street, ia.Zip, ia.City),
							BankContactId = o.BankContactId,
							ContactPersonId = o.ContactPersonId, 
							ContactPersonString = dbo.fFormatPerson(cp.FirstName, cp.LastName, ''),
							ContactEMail = o.ContactEMail, ContactPhone = o.ContactPhone, ContactMobile = o.ContactMobile,
							o.ParentId, o.Code, o.VATPayment, o.TVD_Id, o.TopManager,
							o.FAX, o.Skype, o.ICQ, o.ContactBirthDay, o.ContactCardId, o.ContactWorkPhone, o.PF, o.RegionCode, o.UserMargin, o.Statut, o.RestrictedAccess,o.Created, o.SelectedCount,
							o.AnonymousRegistration, o.AnonymousAssignBy, o.AnonymousAssignAt, o.AnonymousTempAssignAt, o.AnonymousAssignToCode, o.AnonymousCreatedAt, o.AnonymousAssignByCode, o.AnonymousAssignStatus, o.ManageAnonymousAssign,
							o.Angel_team_clen, o.Angel_team_manager, o.Angel_team_manager_typ,
							o.PredmetCinnosti, o.AnonymousOvereniSluzeb, o.AnonymousZmenaNaJineRegistracniCislo, o.AnonymousZmenaNaJineRegistracniCisloText, o.AnonymousSouhlasStavajicihoPoradce, o.AnonymousSouhlasNavrzenehoPoradce,
							o.ZasilaniTiskovin, o.ZasilaniNewsletter, o.ZasilaniKatalogu, o.RegistrationFromCookiesLinkAccountId
					FROM vOrganizations o
					LEFT JOIN tPerson cp (NOLOCK) ON o.ContactPersonId = cp.PersonId
					LEFT JOIN tAddress ra (NOLOCK) ON o.RegisteredAddressId = ra.AddressId
					LEFT JOIN tAddress ca (NOLOCK) ON o.CorrespondenceAddressId = ca.AddressId
					LEFT JOIN tAddress ia (NOLOCK) ON o.InvoicingAddressId = ia.AddressId
					WHERE
					( @AngelTeamClen IS NULL OR o.Angel_team_clen=@AngelTeamClen ) AND
					( @AngelTeamManager IS NULL OR o.Angel_team_manager=@AngelTeamManager ) AND
					( @AngelTeamManagerTyp IS NULL OR o.Angel_team_manager_typ=@AngelTeamManagerTyp )";


                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@AngelTeamClen", Null(by.AngelTeamClen)),
                        new SqlParameter("@AngelTeamManager", Null(by.AngelTeamManager)),
                        new SqlParameter("@AngelTeamManagerTyp", Null(by.AngelTeamManagerTyp))
                        );
                foreach (DataRow dr in table.Rows) orgs.Add(GetOrganization(dr));
            }
            return orgs;
        }
        private List<Organization> LoadAnonymousAssignManager() {
            List<Organization> orgs = new List<Organization>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
					SELECT	o.AccountId, o.OrganizationId, o.InstanceId, 
							o.Id1, o.Id2, o.Id3, o.Name, o.Notes, o.Web,
							o.RegisteredAddressId, o.CorrespondenceAddressId, o.InvoicingAddressId,
							RegisteredAddressString = dbo.fFormatAddress(ra.Street, ra.Zip, ra.City),
							CorrespondenceAddressString = dbo.fFormatAddress(ca.Street, ca.Zip, ca.City),
							InvoicingAddressString = dbo.fFormatAddress(ia.Street, ia.Zip, ia.City),
							BankContactId = o.BankContactId,
							ContactPersonId = o.ContactPersonId, 
							ContactPersonString = dbo.fFormatPerson(cp.FirstName, cp.LastName, ''),
							ContactEMail = o.ContactEMail, ContactPhone = o.ContactPhone, ContactMobile = o.ContactMobile,
							o.ParentId, o.Code, o.VATPayment, o.TVD_Id, o.TopManager,
							o.FAX, o.Skype, o.ICQ, o.ContactBirthDay, o.ContactCardId, o.ContactWorkPhone, o.PF, o.RegionCode, o.UserMargin, o.Statut, o.RestrictedAccess,o.Created, o.SelectedCount,
							o.AnonymousRegistration, o.AnonymousAssignBy, o.AnonymousAssignAt, o.AnonymousTempAssignAt, o.AnonymousAssignToCode, o.AnonymousCreatedAt, o.AnonymousAssignByCode, o.AnonymousAssignStatus, o.ManageAnonymousAssign,
							o.Angel_team_clen, o.Angel_team_manager, o.Angel_team_manager_typ,
							o.PredmetCinnosti, o.AnonymousOvereniSluzeb, o.AnonymousZmenaNaJineRegistracniCislo, o.AnonymousZmenaNaJineRegistracniCisloText, o.AnonymousSouhlasStavajicihoPoradce, o.AnonymousSouhlasNavrzenehoPoradce,
							o.ZasilaniTiskovin, o.ZasilaniNewsletter, o.ZasilaniKatalogu, o.RegistrationFromCookiesLinkAccountId
					FROM vOrganizations o
					LEFT JOIN tPerson cp (NOLOCK) ON o.ContactPersonId = cp.PersonId
					LEFT JOIN tAddress ra (NOLOCK) ON o.RegisteredAddressId = ra.AddressId
					LEFT JOIN tAddress ca (NOLOCK) ON o.CorrespondenceAddressId = ca.AddressId
					LEFT JOIN tAddress ia (NOLOCK) ON o.InvoicingAddressId = ia.AddressId
					WHERE o.ManageAnonymousAssign = 1";

                DataTable table = Query<DataTable>(connection, sql);
                foreach (DataRow dr in table.Rows) orgs.Add(GetOrganization(dr));
            }
            return orgs;
        }

        private List<Organization> LoadBy(Organization.ReadBy by) {
            List<Organization> orgs = new List<Organization>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
					SELECT	o.AccountId, o.OrganizationId, o.InstanceId, 
							o.Id1, o.Id2, o.Id3, o.Name, o.Notes, o.Web,
							o.RegisteredAddressId, o.CorrespondenceAddressId, o.InvoicingAddressId,
							RegisteredAddressString = dbo.fFormatAddress(ra.Street, ra.Zip, ra.City),
							CorrespondenceAddressString = dbo.fFormatAddress(ca.Street, ca.Zip, ca.City),
							InvoicingAddressString = dbo.fFormatAddress(ia.Street, ia.Zip, ia.City),
							BankContactId = o.BankContactId,
							ContactPersonId = o.ContactPersonId, 
							ContactPersonString = dbo.fFormatPerson(cp.FirstName, cp.LastName, ''),
							ContactEMail = o.ContactEMail, ContactPhone = o.ContactPhone, ContactMobile = o.ContactMobile,
							o.ParentId, o.Code, o.VATPayment, o.TVD_Id, o.TopManager,
							o.FAX, o.Skype, o.ICQ, o.ContactBirthDay, o.ContactCardId, o.ContactWorkPhone, o.PF, o.RegionCode, o.UserMargin, o.Statut, o.RestrictedAccess,o.Created, o.SelectedCount,
							o.AnonymousRegistration, o.AnonymousAssignBy, o.AnonymousAssignAt, o.AnonymousTempAssignAt, o.AnonymousAssignToCode, o.AnonymousCreatedAt, o.AnonymousAssignByCode, o.AnonymousAssignStatus, o.ManageAnonymousAssign,
							o.Angel_team_clen, o.Angel_team_manager, o.Angel_team_manager_typ,
							o.PredmetCinnosti, o.AnonymousOvereniSluzeb, o.AnonymousZmenaNaJineRegistracniCislo, o.AnonymousZmenaNaJineRegistracniCisloText, o.AnonymousSouhlasStavajicihoPoradce, o.AnonymousSouhlasNavrzenehoPoradce,
							o.ZasilaniTiskovin, o.ZasilaniNewsletter, o.ZasilaniKatalogu, o.RegistrationFromCookiesLinkAccountId
					FROM vOrganizations o
					LEFT JOIN tPerson cp (NOLOCK) ON o.ContactPersonId = cp.PersonId
					LEFT JOIN tAddress ra (NOLOCK) ON o.RegisteredAddressId = ra.AddressId
					LEFT JOIN tAddress ca (NOLOCK) ON o.CorrespondenceAddressId = ca.AddressId
					LEFT JOIN tAddress ia (NOLOCK) ON o.InvoicingAddressId = ia.AddressId
					WHERE ( @Top IS NULL OR o.TopManager=@Top ) AND (@Code IS NULL OR o.Code=@Code) AND (@Name IS NULL OR o.Name LIKE '%'+ @Name +'%') AND (@City IS NULL OR ra.City LIKE @City +'%') AND 
					( @RegionCode IS NULL OR o.RegionCode=@RegionCode ) AND o.InstanceId=@InstanceId AND
					( @ParentId IS NULL OR o.ParentId=@ParentId ) AND ( @Verified IS NULL OR o.Verified=@Verified ) 

					ORDER BY o.Name ASC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@Code", Null(by.Code)),
                        new SqlParameter("@Name", Null(by.Name)),
                        new SqlParameter("@City", Null(by.City)),
                        new SqlParameter("@RegionCode", Null(by.RegionCode)),
                        new SqlParameter("@Top", Null(by.Top.HasValue ? (int?)(by.Top.Value ? 1 : 0) : null)),
                        new SqlParameter("@ParentId", Null(by.ParentId)),
                        new SqlParameter("@Verified", Null(by.Verified)),
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@ZasilaniTiskovin", Null(by.ZasilaniTiskovin)),
                        new SqlParameter("@ZasilaniNewsletter", Null(by.ZasilaniNewsletter)),
                        new SqlParameter("@ZasilaniKatalogu", Null(by.ZasilaniKatalogu)));
                foreach (DataRow dr in table.Rows) orgs.Add(GetOrganization(dr));
            }
            return orgs;
        }
        private List<Organization> LoadTOP(Organization.ReadTOP by) {
            List<Organization> orgs = new List<Organization>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
					SELECT	o.AccountId, o.OrganizationId, o.InstanceId, 
							o.Id1, o.Id2, o.Id3, o.Name, o.Notes, o.Web,
							o.RegisteredAddressId, o.CorrespondenceAddressId, o.InvoicingAddressId,
							RegisteredAddressString = dbo.fFormatAddress(ra.Street, ra.Zip, ra.City),
							CorrespondenceAddressString = dbo.fFormatAddress(ca.Street, ca.Zip, ca.City),
							InvoicingAddressString = dbo.fFormatAddress(ia.Street, ia.Zip, ia.City),
							BankContactId = o.BankContactId,
							ContactPersonId = o.ContactPersonId, 
							ContactPersonString = dbo.fFormatPerson(cp.FirstName, cp.LastName, ''),
							ContactEMail = o.ContactEMail, ContactPhone = o.ContactPhone, ContactMobile = o.ContactMobile,
							o.ParentId, o.Code, o.VATPayment, o.TVD_Id, o.TopManager,
							o.FAX, o.Skype, o.ICQ, o.ContactBirthDay, o.ContactCardId, o.ContactWorkPhone, o.PF, o.RegionCode, o.UserMargin, o.Statut, o.RestrictedAccess,o.Created, o.SelectedCount,
							o.AnonymousRegistration, o.AnonymousAssignBy, o.AnonymousAssignAt, o.AnonymousTempAssignAt, o.AnonymousAssignToCode, o.AnonymousCreatedAt, o.AnonymousAssignByCode, o.AnonymousAssignStatus, o.ManageAnonymousAssign,
							o.Angel_team_clen, o.Angel_team_manager, o.Angel_team_manager_typ,
							o.PredmetCinnosti, o.AnonymousOvereniSluzeb, o.AnonymousZmenaNaJineRegistracniCislo, o.AnonymousZmenaNaJineRegistracniCisloText, o.AnonymousSouhlasStavajicihoPoradce, o.AnonymousSouhlasNavrzenehoPoradce,
							o.ZasilaniTiskovin, o.ZasilaniNewsletter, o.ZasilaniKatalogu, o.RegistrationFromCookiesLinkAccountId
					FROM vOrganizations o
					LEFT JOIN tPerson cp (NOLOCK) ON o.ContactPersonId = cp.PersonId
					LEFT JOIN tAddress ra (NOLOCK) ON o.RegisteredAddressId = ra.AddressId
					LEFT JOIN tAddress ca (NOLOCK) ON o.CorrespondenceAddressId = ca.AddressId
					LEFT JOIN tAddress ia (NOLOCK) ON o.InvoicingAddressId = ia.AddressId
					WHERE o.TopManager=1 AND o.InstanceId=@InstanceId
					ORDER BY o.Name ASC";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@InstanceId", InstanceId));
                foreach (DataRow dr in table.Rows) orgs.Add(GetOrganization(dr));
            }
            return orgs;
        }
        private List<Organization> LoadTOPForHost(Organization.ReadTOPForHost by) {
            List<Organization> orgs = new List<Organization>();
            using (SqlConnection connection = Connect()) {
                DataTable table = QueryProc<DataTable>(connection, "pEuronaFindAdvisorForHost",
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@City", Null(by.City)),
                        new SqlParameter("@Name", Null(by.Name)),
                        new SqlParameter("@Region", Null(by.RegionCode)));
                foreach (DataRow dr in table.Rows) orgs.Add(GetOrganization(dr));
            }
            return orgs;
        }
        private List<Organization> LoadByTVDId(Organization.ReadByTVDId by) {
            List<Organization> orgs = new List<Organization>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
					SELECT	o.AccountId, o.OrganizationId, o.InstanceId, 
							o.Id1, o.Id2, o.Id3, o.Name, o.Notes, o.Web,
							o.RegisteredAddressId, o.CorrespondenceAddressId, o.InvoicingAddressId,
							RegisteredAddressString = dbo.fFormatAddress(ra.Street, ra.Zip, ra.City),
							CorrespondenceAddressString = dbo.fFormatAddress(ca.Street, ca.Zip, ca.City),
							InvoicingAddressString = dbo.fFormatAddress(ia.Street, ia.Zip, ia.City),
							BankContactId = o.BankContactId,
							ContactPersonId = o.ContactPersonId, 
							ContactPersonString = dbo.fFormatPerson(cp.FirstName, cp.LastName, ''),
							ContactEMail = o.ContactEMail, ContactPhone = o.ContactPhone, ContactMobile = o.ContactMobile,
							o.ParentId, o.Code, o.VATPayment, o.TVD_Id, o.TopManager,
							o.FAX, o.Skype, o.ICQ, o.ContactBirthDay, o.ContactCardId, o.ContactWorkPhone, o.PF, o.RegionCode, o.UserMargin, o.Statut, o.RestrictedAccess,o.Created, o.SelectedCount,
							o.AnonymousRegistration, o.AnonymousAssignBy, o.AnonymousAssignAt, o.AnonymousTempAssignAt, o.AnonymousAssignToCode, o.AnonymousCreatedAt, o.AnonymousAssignByCode, o.AnonymousAssignStatus, o.ManageAnonymousAssign,
							o.Angel_team_clen, o.Angel_team_manager, o.Angel_team_manager_typ,
							o.PredmetCinnosti, o.AnonymousOvereniSluzeb, o.AnonymousZmenaNaJineRegistracniCislo, o.AnonymousZmenaNaJineRegistracniCisloText, o.AnonymousSouhlasStavajicihoPoradce, o.AnonymousSouhlasNavrzenehoPoradce,
							o.ZasilaniTiskovin, o.ZasilaniNewsletter, o.ZasilaniKatalogu, o.RegistrationFromCookiesLinkAccountId
					FROM vOrganizations o
					LEFT JOIN tPerson cp (NOLOCK) ON o.ContactPersonId = cp.PersonId
					LEFT JOIN tAddress ra (NOLOCK) ON o.RegisteredAddressId = ra.AddressId
					LEFT JOIN tAddress ca (NOLOCK) ON o.CorrespondenceAddressId = ca.AddressId
					LEFT JOIN tAddress ia (NOLOCK) ON o.InvoicingAddressId = ia.AddressId
					WHERE o.TVD_Id = @TVD_Id AND (@InstanceId IS NULL OR o.InstanceId = @InstanceId)";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@TVD_Id", by.TVD_Id), new SqlParameter("@InstanceId", Null(by.InstanceId)));
                foreach (DataRow dr in table.Rows) orgs.Add(GetOrganization(dr));
            }
            return orgs;
        }
        private List<Organization> LoadByCode(Organization.ReadByCode by) {
            List<Organization> orgs = new List<Organization>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
					SELECT	o.AccountId, o.OrganizationId, o.InstanceId, 
							o.Id1, o.Id2, o.Id3, o.Name, o.Notes, o.Web,
							o.RegisteredAddressId, o.CorrespondenceAddressId, o.InvoicingAddressId,
							RegisteredAddressString = dbo.fFormatAddress(ra.Street, ra.Zip, ra.City),
							CorrespondenceAddressString = dbo.fFormatAddress(ca.Street, ca.Zip, ca.City),
							InvoicingAddressString = dbo.fFormatAddress(ia.Street, ia.Zip, ia.City),
							BankContactId = o.BankContactId,
							ContactPersonId = o.ContactPersonId, 
							ContactPersonString = dbo.fFormatPerson(cp.FirstName, cp.LastName, ''),
							ContactEMail = o.ContactEMail, ContactPhone = o.ContactPhone, ContactMobile = o.ContactMobile,
							o.ParentId, o.Code, o.VATPayment, o.TVD_Id, o.TopManager,
							o.FAX, o.Skype, o.ICQ, o.ContactBirthDay, o.ContactCardId, o.ContactWorkPhone, o.PF, o.RegionCode, o.UserMargin, o.Statut, o.RestrictedAccess,o.Created, o.SelectedCount,
							o.AnonymousRegistration, o.AnonymousAssignBy, o.AnonymousAssignAt, o.AnonymousTempAssignAt, o.AnonymousAssignToCode, o.AnonymousCreatedAt, o.AnonymousAssignByCode, o.AnonymousAssignStatus, o.ManageAnonymousAssign,
							o.Angel_team_clen, o.Angel_team_manager, o.Angel_team_manager_typ,
							o.PredmetCinnosti, o.AnonymousOvereniSluzeb, o.AnonymousZmenaNaJineRegistracniCislo, o.AnonymousZmenaNaJineRegistracniCisloText, o.AnonymousSouhlasStavajicihoPoradce, o.AnonymousSouhlasNavrzenehoPoradce,
							o.ZasilaniTiskovin, o.ZasilaniNewsletter, o.ZasilaniKatalogu, o.RegistrationFromCookiesLinkAccountId
					FROM vOrganizations o
					LEFT JOIN tPerson cp (NOLOCK) ON o.ContactPersonId = cp.PersonId
					LEFT JOIN tAddress ra (NOLOCK) ON o.RegisteredAddressId = ra.AddressId
					LEFT JOIN tAddress ca (NOLOCK) ON o.CorrespondenceAddressId = ca.AddressId
					LEFT JOIN tAddress ia (NOLOCK) ON o.InvoicingAddressId = ia.AddressId
					WHERE o.Code = @Code";
                DataTable table = Query<DataTable>(connection, sql,
                        new SqlParameter("@Code", by.Code));
                foreach (DataRow dr in table.Rows) orgs.Add(GetOrganization(dr));
            }
            return orgs;
        }
        public override void Create(Organization org) {
            using (SqlConnection connection = Connect()) {
                DataSet ds = QueryProc<DataSet>(connection, "pOrganizationCreate",
                    new SqlParameter("@HistoryAccount", Null(Account != null ? Account.Id : (int?)null)),
                    new SqlParameter("@InstanceId", InstanceId),
                    new SqlParameter("@AccountId", Null(org.AccountId)),
                    new SqlParameter("@Id1", org.Id1),
                    new SqlParameter("@Id2", org.Id2),
                    new SqlParameter("@Id3", org.Id3),
                    new SqlParameter("@Name", org.Name),
                    new SqlParameter("@Notes", org.Notes),
                    new SqlParameter("@Web", org.Web),
                    new SqlParameter("@ContactEmail", org.ContactEmail),
                    new SqlParameter("@ContactPhone", org.ContactPhone),
                    new SqlParameter("@ContactMobile", org.ContactMobile),
                    new SqlParameter("@ParentId", org.ParentId),
                    new SqlParameter("@VATPayment", org.VATPayment),
                    new SqlParameter("@Code", org.Code),
                    new SqlParameter("@TopManager", org.TopManager),

                    new SqlParameter("@FAX", Null(org.FAX)),
                    new SqlParameter("@Skype", Null(org.Skype)),
                    new SqlParameter("@ICQ", Null(org.ICQ)),
                    new SqlParameter("@ContactBirthDay", Null(org.ContactBirthDay)),
                    new SqlParameter("@ContactCardId", Null(org.ContactCardId)),
                    new SqlParameter("@ContactWorkPhone", Null(org.ContactWorkPhone)),
                    new SqlParameter("@PF", Null(org.PF)),
                    new SqlParameter("@RegionCode", Null(org.RegionCode)),
                    new SqlParameter("@UserMargin", Null(org.UserMargin)),
                    new SqlParameter("@Statut", Null(org.Statut)),
                    new SqlParameter("@SelectedCount", Null(org.SelectedCount)),

                    new SqlParameter("@AnonymousRegistration", Null(org.AnonymousRegistration)),
                    new SqlParameter("@AnonymousCreatedAt", Null(org.AnonymousCreatedAt)),
                    new SqlParameter("@AnonymousAssignBy", Null(org.AnonymousAssignBy)),
                    new SqlParameter("@AnonymousAssignAt", Null(org.AnonymousAssignAt)),
                    new SqlParameter("@AnonymousAssignToCode", Null(org.AnonymousAssignToCode)),
                    new SqlParameter("@AnonymousAssignByCode", Null(org.AnonymousAssignByCode)),
                    new SqlParameter("@AnonymousAssignStatus", Null(org.AnonymousAssignStatus)),
                    new SqlParameter("@ManageAnonymousAssign", org.ManageAnonymousAssign),

                    new SqlParameter("@PredmetCinnosti", org.PredmetCinnosti),
                    new SqlParameter("@AnonymousOvereniSluzeb", org.AnonymousOvereniSluzeb),
                    new SqlParameter("@AnonymousZmenaNaJineRegistracniCislo", org.AnonymousZmenaNaJineRegistracniCislo),
                    new SqlParameter("@AnonymousZmenaNaJineRegistracniCisloText", org.AnonymousZmenaNaJineRegistracniCisloText),
                    new SqlParameter("@AnonymousSouhlasStavajicihoPoradce", org.AnonymousSouhlasStavajicihoPoradce),
                    new SqlParameter("@AnonymousSouhlasNavrzenehoPoradce", org.AnonymousSouhlasNavrzenehoPoradce),

                    new SqlParameter("@ZasilaniTiskovin", org.ZasilaniTiskovin),
                    new SqlParameter("@ZasilaniNewsletter", org.ZasilaniNewsletter),
                    new SqlParameter("@ZasilaniKatalogu", org.ZasilaniKatalogu),
                    new SqlParameter("@RegistrationFromCookiesLinkAccountId", org.RegistrationFromCookiesLinkAccountId)
                );
                DataTable dtRegAddres = ds.Tables[0];
                DataTable dtCorAddres = ds.Tables[1];
                DataTable dtInvAddres = ds.Tables[2];
                DataTable dtBankContact = ds.Tables[3];
                DataTable dtPersonHomeAddress = ds.Tables[4];
                DataTable dtPersonTempAddress = ds.Tables[5];
                DataTable dtContactPerson = ds.Tables[6];
                DataTable dtOrg = ds.Tables[7];
                org.RegisteredAddressId = ConvertNullable.ToInt32(dtRegAddres.Rows[0]["AddressId"]);
                org.CorrespondenceAddressId = ConvertNullable.ToInt32(dtCorAddres.Rows[0]["AddressId"]);
                org.InvoicingAddressId = ConvertNullable.ToInt32(dtInvAddres.Rows[0]["AddressId"]);
                org.BankContactId = ConvertNullable.ToInt32(dtBankContact.Rows[0]["BankContactId"]);
                org.ContactPersonId = ConvertNullable.ToInt32(dtContactPerson.Rows[0]["PersonId"]);
                org.Id = Convert.ToInt32(dtOrg.Rows[0]["OrganizationId"]);
            }
        }

        public override void Update(Organization org) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pOrganizationModify",
                    new SqlParameter("@HistoryAccount", Null(Account != null ? Account.Id : (int?)null)),
                    new SqlParameter("@OrganizationId", org.Id),
                    new SqlParameter("@Id1", org.Id1),
                    new SqlParameter("@Id2", org.Id2),
                    new SqlParameter("@Id3", org.Id3),
                    new SqlParameter("@Name", org.Name),
                    new SqlParameter("@Notes", org.Notes),
                    new SqlParameter("@Web", org.Web),
                    new SqlParameter("@ContactEmail", org.ContactEmail),
                    new SqlParameter("@ContactPhone", org.ContactPhone),
                    new SqlParameter("@ContactMobile", org.ContactMobile),
                    new SqlParameter("@ParentId", org.ParentId),
                    new SqlParameter("@VATPayment", org.VATPayment),
                    new SqlParameter("@Code", org.Code),
                    new SqlParameter("@TopManager", org.TopManager),
                    new SqlParameter("@FAX", Null(org.FAX)),
                    new SqlParameter("@Skype", Null(org.Skype)),
                    new SqlParameter("@ICQ", Null(org.ICQ)),
                    new SqlParameter("@ContactBirthDay", Null(org.ContactBirthDay)),
                    new SqlParameter("@ContactCardId", Null(org.ContactCardId)),
                    new SqlParameter("@ContactWorkPhone", Null(org.ContactWorkPhone)),
                    new SqlParameter("@PF", Null(org.PF)),
                    new SqlParameter("@RegionCode", Null(org.RegionCode)),
                    new SqlParameter("@UserMargin", Null(org.UserMargin)),
                    new SqlParameter("@Statut", Null(org.Statut)),
                    new SqlParameter("@SelectedCount", Null(org.SelectedCount)),
                    new SqlParameter("@AnonymousRegistration", Null(org.AnonymousRegistration)),
                    new SqlParameter("@AnonymousCreatedAt", Null(org.AnonymousCreatedAt)),
                    new SqlParameter("@AnonymousAssignBy", Null(org.AnonymousAssignBy)),
                    new SqlParameter("@AnonymousTempAssignAt", Null(org.AnonymousTempAssignAt)),
                    new SqlParameter("@AnonymousAssignAt", Null(org.AnonymousAssignAt)),
                    new SqlParameter("@AnonymousAssignToCode", Null(org.AnonymousAssignToCode)),
                    new SqlParameter("@AnonymousAssignByCode", Null(org.AnonymousAssignByCode)),
                    new SqlParameter("@AnonymousAssignStatus", Null(org.AnonymousAssignStatus)),
                    new SqlParameter("@ManageAnonymousAssign", org.ManageAnonymousAssign),

                    new SqlParameter("@PredmetCinnosti", org.PredmetCinnosti),
                    new SqlParameter("@AnonymousOvereniSluzeb", org.AnonymousOvereniSluzeb),
                    new SqlParameter("@AnonymousZmenaNaJineRegistracniCislo", org.AnonymousZmenaNaJineRegistracniCislo),
                    new SqlParameter("@AnonymousZmenaNaJineRegistracniCisloText", org.AnonymousZmenaNaJineRegistracniCisloText),
                    new SqlParameter("@AnonymousSouhlasStavajicihoPoradce", org.AnonymousSouhlasStavajicihoPoradce),
                    new SqlParameter("@AnonymousSouhlasNavrzenehoPoradce", org.AnonymousSouhlasNavrzenehoPoradce),
                    new SqlParameter("@ZasilaniTiskovin", org.ZasilaniTiskovin),
                    new SqlParameter("@ZasilaniNewsletter", org.ZasilaniNewsletter),
                    new SqlParameter("@ZasilaniKatalogu", org.ZasilaniKatalogu),
                    new SqlParameter("@RegistrationFromCookiesLinkAccountId", org.RegistrationFromCookiesLinkAccountId)
                );
            }
        }

        public override void Delete(Organization org) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pOrganizationDelete",
                    new SqlParameter("@HistoryAccount", Null(Account != null ? AccountId : (int?)null)),
                    new SqlParameter("@OrganizationId", org.Id)
                );
            }
        }
    }
}
