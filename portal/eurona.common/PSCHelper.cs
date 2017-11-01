using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace Eurona.Common {
    public class PSCHelper {
        public static DataTable GetTVDMestoBy(string kodStatu, string mesto) {
            string connectionStringTVD = ConfigurationManager.ConnectionStrings["TVDConnectionString"].ConnectionString;
            CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(connectionStringTVD);
            using (SqlConnection connection = tvdStorage.Connect()) {
                //string sql = @"SELECT DISTINCT Nazev, Psc FROM k2psc WHERE ( @kodStatu IS NULL OR Kod_statu=@kodStatu ) AND Nazev LIKE @mesto+'%'";
                string sql = @"SELECT DISTINCT pc.postoffice_local AS Nazev, pc.postcode AS Psc FROM Eurona_Postcode pc 
                INNER JOIN Eurona_Country c ON pc.country_id=c.id
                WHERE ( @kodStatu IS NULL OR c.alpha_2=@kodStatu ) AND pc.postoffice_local LIKE @mesto+'%'";
//                string sql = @"SELECT DISTINCT pc.city_local AS Nazev, pc.postcode AS Psc FROM Eurona_Postcode pc 
//                INNER JOIN Eurona_Country c ON pc.country_id=c.id
//                WHERE ( @kodStatu IS NULL OR c.alpha_2=@kodStatu ) AND pc.city_local LIKE @mesto+'%'";

                DataTable dt = tvdStorage.Query(connection, sql, new SqlParameter("@kodStatu", kodStatu), new SqlParameter("@mesto", mesto));
                return dt;
            }
        }

        private static DataTable GetTVDPSCByPSC(string psc) {
            return GetTVDPSCByPSC(psc, null);
        }

        private static DataTable GetTVDPSCByPSC(string psc, string locale) {
            object kodStatu = "xyz";
            if (locale == "cs") kodStatu = "cz";
            else if (locale == "sk") kodStatu = "SK";
            else if (locale == "pl") kodStatu = "PL";
            else kodStatu = DBNull.Value;

            string connectionStringTVD = ConfigurationManager.ConnectionStrings["TVDConnectionString"].ConnectionString;
            CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(connectionStringTVD);
            using (SqlConnection connection = tvdStorage.Connect()) {
                //string sql = @"SELECT DISTINCT * FROM k2psc WHERE Psc=@psc AND ( @kodStatu IS NULL OR Kod_statu=@kodStatu )";

//                string sql = @"SELECT DISTINCT c.alpha_2 as Kod_statu, pc.postcode AS Psc, pc.city_local AS Nazev FROM Eurona_Postcode pc 
//                INNER JOIN Eurona_Country c ON pc.country_id=c.id
//                WHERE pc.postcode=@psc AND ( @kodStatu IS NULL OR c.alpha_2=@kodStatu )";

                string sql = @"SELECT DISTINCT c.alpha_2 as Kod_statu, pc.postcode AS Psc, pc.postoffice_local AS Nazev FROM Eurona_Postcode pc 
                INNER JOIN Eurona_Country c ON pc.country_id=c.id
                WHERE pc.postcode=@psc AND ( @kodStatu IS NULL OR c.alpha_2=@kodStatu )";

                DataTable dt = tvdStorage.Query(connection, sql, new SqlParameter("@psc", psc), new SqlParameter("@kodStatu", kodStatu));
                return dt;
            }
        }

        /*
        private static DataTable GetTVDPSCByMesto(string mesto) {
            return GetTVDPSCByMesto(mesto, null);
        }

        private static DataTable GetTVDPSCByMesto(string mesto, string locale) {
            object kodStatu = "xyz";
            if (locale == "cs") kodStatu = "cz";
            else if (locale == "sk") kodStatu = "SK";
            else if (locale == "pl") kodStatu = "PL";
            else kodStatu = DBNull.Value;

            string connectionStringTVD = ConfigurationManager.ConnectionStrings["TVDConnectionString"].ConnectionString;
            CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(connectionStringTVD);
            using (SqlConnection connection = tvdStorage.Connect()) {
                string sql = @"SELECT DISTINCT * FROM k2psc WHERE Nazev=@mesto AND ( @kodStatu IS NULL OR Kod_statu=@kodStatu )";
                DataTable dt = tvdStorage.Query(connection, sql, new SqlParameter("@mesto", mesto), new SqlParameter("@kodStatu", kodStatu));
                return dt;
            }
        }
         * */

        public static string ValidatePSCByPSC(string psc, string city, string stat) {
            string locale = "zyz";
            if (stat == "CZ") locale = "cs";
            else if (stat == "PL") locale = "pl";
            else if (stat == "SK") locale = "sk";

            //Validate PSČ
            DataTable dtPSC = Eurona.Common.PSCHelper.GetTVDPSCByPSC(psc, locale);
            if (dtPSC == null || dtPSC.Rows.Count == 0)
                return "Vyplňte město! Chybná doručovací adresa.";//"PSČ nebo město je uvedeno špatně. Zkontrolujte zadané údaje a opravte je. V případě nejasností kontaktujte vašeho sponzora nebo operátorku. Děkujeme";
            else {
                bool match = false;
                foreach (DataRow row in dtPSC.Rows) {
                    if (city != row["Nazev"].ToString().Trim()) continue;

                    match = true;
                    break;
                }
                if (match == false)
                    return "Vyplňte město! Chybná doručovací adresa.";//"PSČ nebo město je uvedeno špatně. Zkontrolujte zadané údaje a opravte je. V případě nejasností kontaktujte vašeho sponzora nebo operátorku. Děkujeme";
            }

            return string.Empty;
        }

        public static string ValidateState(string stat) {
            if (stat == "CZ" || stat == "PL" || stat == "SK") return string.Empty;
            return "Stát je uveden špatně. (CZ|SK|PL)!";
        }
    }
}
