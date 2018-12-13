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
    public sealed class AngelTeamStorage : MSSQLStorage<AngelTeam> {
        private string entitySelect = "SELECT AngelTeamId, PocetEuronaStarProVstup, PocetEuronaStarProUdrzeni FROM tAngelTeam";
        public AngelTeamStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static AngelTeam GetAngelTeam(DataRow record) {
            AngelTeam entiry = new AngelTeam();
            entiry.Id = Convert.ToInt32(record["AngelTeamId"]);
            entiry.PocetEuronaStarProVstup = Convert.ToInt32(record["PocetEuronaStarProVstup"]);
            entiry.PocetEuronaStarProUdrzeni = Convert.ToInt32(record["PocetEuronaStarProUdrzeni"]);

            return entiry;
        }

        public override List<AngelTeam> Read(object criteria) {
            List<AngelTeam> list = new List<AngelTeam>();

            using (SqlConnection connection = Connect()) {
                DataTable table = null;
                table = Query<DataTable>(connection, entitySelect);
                foreach (DataRow dr in table.Rows)
                    list.Add(GetAngelTeam(dr));

            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        public override void Create(AngelTeam entity) {
            throw new NotImplementedException();
        }

        public override void Update(AngelTeam entity) {
            using (SqlConnection connection = Connect()) {
                Exec(connection, @"UPDATE tAngelTeam SET PocetEuronaStarProVstup=@PocetEuronaStarProVstup, 
										PocetEuronaStarProUdrzeni=@PocetEuronaStarProUdrzeni
										WHERE AngelTeamId=@AngelTeamId",
                        new SqlParameter("@AngelTeamId", entity.Id),
                        new SqlParameter("@PocetEuronaStarProVstup", entity.PocetEuronaStarProVstup),
                        new SqlParameter("@PocetEuronaStarProUdrzeni", entity.PocetEuronaStarProUdrzeni)
                        );
            }
        }

        public override void Delete(AngelTeam entity) {
            throw new NotImplementedException();
        }
    }
}
