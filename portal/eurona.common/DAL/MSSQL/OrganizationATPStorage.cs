
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
    public sealed class OrganizationATPStorage : MSSQLStorage<OrganizationATP> {
        public OrganizationATPStorage(int instanceId, CMS.Entities.Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        internal static OrganizationATP GetOrganization(DataRow record) {
            OrganizationATP org = new OrganizationATP();
            org.Id = Convert.ToInt32(record["OrganizationId"]);
            org.AccountId = Convert.ToInt32(record["AccountId"]);
            org.RegisteredAddressCity = Convert.ToString(record["RegisteredAddressCity"]);
            org.Code = ConvertNullable.ToString(record["Code"]);
            org.BankContactId = ConvertNullable.ToInt32(record["BankContactId"]);
            org.AnonymousCreatedAt = ConvertNullable.ToDateTime(record["AnonymousCreatedAt"]);
            org.AnonymousAssignByCode = Convert.ToString(record["AnonymousAssignByCode"]);
            org.AnonymousAssignToCode = Convert.ToString(record["AnonymousAssignToCode"]);
            org.AnonymousAssignStatus = Convert.ToString(record["AnonymousAssignStatus"]);

            org.AnonymousOvereniSluzeb = Convert.ToBoolean(record["AnonymousOvereniSluzeb"]);
            org.AnonymousZmenaNaJineRegistracniCislo = Convert.ToBoolean(record["AnonymousZmenaNaJineRegistracniCislo"]);
            org.AnonymousZmenaNaJineRegistracniCisloText = Convert.ToString(record["AnonymousZmenaNaJineRegistracniCisloText"]);
            org.AnonymousSouhlasStavajicihoPoradce = Convert.ToBoolean(record["AnonymousSouhlasStavajicihoPoradce"]);
            org.AnonymousSouhlasNavrzenehoPoradce = Convert.ToBoolean(record["AnonymousSouhlasNavrzenehoPoradce"]);

        //            public int BankContactId { get; set; }
        //public string Code{ get; set; }
        //public string AnonymousAssignToCode { get; set; }
        //public DateTime? AnonymousCreatedAt { get; set; }
        //public string AnonymousAssignStatus { get; set; }
        //public string AnonymousAssignByCode { get; set; }
        //public bool AnonymousOvereniSluzeb { get; set; }
        //public bool AnonymousZmenaNaJineRegistracniCislo { get; set; }
        //public string AnonymousZmenaNaJineRegistracniCisloText { get; set; }
        //public bool AnonymousSouhlasStavajicihoPoradce { get; set; }
        //public bool AnonymousSouhlasNavrzenehoPoradce { get; set; }
            return org;
        }

        public override List<OrganizationATP> Read(object criteria) {
            if (criteria is OrganizationATP.ReadByAnonymous) return LoadByAnonymous(criteria as OrganizationATP.ReadByAnonymous);
            List<OrganizationATP> orgs = new List<OrganizationATP>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
                    SELECT o.AccountId, o.OrganizationId, RegisteredAddressCity =  ra.City,
                    o.BankContactId, o.Code, o.AnonymousCreatedAt, o.AnonymousAssignToCode, o.AnonymousAssignByCode, o.AnonymousOvereniSluzeb, o.AnonymousZmenaNaJineRegistracniCislo, 
                    o.AnonymousSouhlasStavajicihoPoradce, o.AnonymousSouhlasNavrzenehoPoradce, o.AnonymousZmenaNaJineRegistracniCisloText,
                    o.AnonymousAssignStatus				
					FROM vOrganizations o
					LEFT JOIN tAddress ra (NOLOCK) ON o.RegisteredAddressId = ra.AddressId
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

        private List<OrganizationATP> LoadByAnonymous(OrganizationATP.ReadByAnonymous by) {
            List<OrganizationATP> orgs = new List<OrganizationATP>();
            using (SqlConnection connection = Connect()) {
                string sql = @"
                    SELECT o.AccountId, o.OrganizationId, RegisteredAddressCity =  ra.City,
                    o.BankContactId, o.Code, o.AnonymousCreatedAt, o.AnonymousAssignToCode, o.AnonymousAssignByCode, o.AnonymousOvereniSluzeb, o.AnonymousZmenaNaJineRegistracniCislo, 
                    o.AnonymousSouhlasStavajicihoPoradce, o.AnonymousSouhlasNavrzenehoPoradce, o.AnonymousZmenaNaJineRegistracniCisloText,
                    o.AnonymousAssignStatus					
					FROM vOrganizations o
					LEFT JOIN tAddress ra (NOLOCK) ON o.RegisteredAddressId = ra.AddressId
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

        public override void Create(OrganizationATP entity) {
            throw new NotImplementedException();
        }

        public override void Update(OrganizationATP entity) {
            throw new NotImplementedException();
        }

        public override void Delete(OrganizationATP entity) {
            throw new NotImplementedException();
        }
       
    }
}
