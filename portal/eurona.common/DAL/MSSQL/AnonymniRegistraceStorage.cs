using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using CMS.MSSQL;
using CMS.Entities;
using Eurona.Common.DAL.Entities;

namespace Eurona.Common.DAL.MSSQL {
    [Serializable]
    public sealed class AnonymniRegistraceStorage : MSSQLStorage<AnonymniRegistrace> {
        private string entitySelect = @"SELECT AnonymniRegistraceId, ZobrazitVSeznamuNeomezene, ZobrazitVSeznamuDni, ZobrazitVSeznamuHodin, ZobrazitVSeznamuMinut, EuronaReistraceProcent, EuronaReistracePocitadlo, MaxPocetPrijetychNovacku, ZobrazitVSeznamuLimit 
            FROM tAnonymniRegistrace";

        public AnonymniRegistraceStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static AnonymniRegistrace GetAnonymniRegistrace(DataRow record) {
            AnonymniRegistrace entiry = new AnonymniRegistrace();
            entiry.Id = Convert.ToInt32(record["AnonymniRegistraceId"]);
            entiry.ZobrazitVSeznamuNeomezene = Convert.ToBoolean(record["ZobrazitVSeznamuNeomezene"]);

            entiry.ZobrazitVSeznamuDni = ConvertNullable.ToInt32(record["ZobrazitVSeznamuDni"]);
            entiry.ZobrazitVSeznamuHodin = ConvertNullable.ToInt32(record["ZobrazitVSeznamuHodin"]);
            entiry.ZobrazitVSeznamuMinut = ConvertNullable.ToInt32(record["ZobrazitVSeznamuMinut"]);

            entiry.EuronaReistraceProcent = ConvertNullable.ToInt32(record["EuronaReistraceProcent"]);
            entiry.EuronaReistracePocitadlo = Convert.ToInt32(record["EuronaReistracePocitadlo"]);
            entiry.MaxPocetPrijetychNovacku = ConvertNullable.ToInt32(record["MaxPocetPrijetychNovacku"]);
            entiry.ZobrazitVSeznamuLimit = Convert.ToString(record["ZobrazitVSeznamuLimit"]);

            return entiry;
        }

        public override List<AnonymniRegistrace> Read(object criteria) {
            List<AnonymniRegistrace> list = new List<AnonymniRegistrace>();

            using (SqlConnection connection = Connect()) {
                DataTable table = null;
                table = Query<DataTable>(connection, entitySelect);
                foreach (DataRow dr in table.Rows)
                    list.Add(GetAnonymniRegistrace(dr));

            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        public override void Create(AnonymniRegistrace entity) {
            throw new NotImplementedException();
        }

        public override void Update(AnonymniRegistrace entity) {
            using (SqlConnection connection = Connect()) {
                Exec(connection, @"UPDATE tAnonymniRegistrace SET ZobrazitVSeznamuNeomezene=@ZobrazitVSeznamuNeomezene, 
                        ZobrazitVSeznamuLimit=@ZobrazitVSeznamuLimit,
					    ZobrazitVSeznamuDni=@ZobrazitVSeznamuDni, 
					    ZobrazitVSeznamuHodin=@ZobrazitVSeznamuHodin, 
					    ZobrazitVSeznamuMinut=@ZobrazitVSeznamuMinut,
					    EuronaReistraceProcent=@EuronaReistraceProcent,
					    EuronaReistracePocitadlo=@EuronaReistracePocitadlo,
                        MaxPocetPrijetychNovacku=@MaxPocetPrijetychNovacku  
					    WHERE AnonymniRegistraceId=@AnonymniRegistraceId",
                    new SqlParameter("@AnonymniRegistraceId", entity.Id),
                    new SqlParameter("@ZobrazitVSeznamuNeomezene", entity.ZobrazitVSeznamuNeomezene),
                    new SqlParameter("@ZobrazitVSeznamuDni", Null(entity.ZobrazitVSeznamuDni)),
                    new SqlParameter("@ZobrazitVSeznamuHodin", Null(entity.ZobrazitVSeznamuHodin)),
                    new SqlParameter("@ZobrazitVSeznamuMinut", Null(entity.ZobrazitVSeznamuMinut)),
                    new SqlParameter("@EuronaReistraceProcent", Null(entity.EuronaReistraceProcent)),
                    new SqlParameter("@EuronaReistracePocitadlo", entity.EuronaReistracePocitadlo),
                    new SqlParameter("@MaxPocetPrijetychNovacku", Null(entity.MaxPocetPrijetychNovacku)),
                    new SqlParameter("@ZobrazitVSeznamuLimit", entity.ZobrazitVSeznamuLimit)
                );
            }
        }

        public override void Delete(AnonymniRegistrace entity) {
            throw new NotImplementedException();
        }
    }
}
