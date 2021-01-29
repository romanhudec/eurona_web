using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Pump;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Drawing;
using EuronaImportFromTVD.DAL;

namespace EuronaImportFromTVD.Tasks {
    public class EuronaAccoutsImport : Synchronize {
        private int instanceId = 0;
        private Odberatel odberatel;
        public EuronaAccoutsImport(int instanceId, MSSQLStorage srcSqlStorage, MSSQLStorage dstSqlStorage, Odberatel odberatel)
            : base(srcSqlStorage, dstSqlStorage) {
            this.instanceId = instanceId;
            this.odberatel = odberatel;
        }

        public int InstanceId { get { return this.instanceId; } }

        public override void Synchronize() {
            int addedItems = 0;
            int updatedItems = 0;
            int errorItems = 0;
            int ignoredItems = 0;

            int rowsCount = 0;
            using (SqlConnection connection = this.DestinationDataStorage.Connect()) {
                try {
                    int rowIndex = 0;
                    try {
                        string sql = string.Empty;
                        //TVD->EURONA
                        DataRow row = EuronaTVDDAL.GetTVDPoradca(this.SourceDataStorage, this.odberatel.IdOdberatele);
                        int accountTVDId = Convert.ToInt32(row["Id_odberatele"]);
                        string kod_odberatele = GetString(row["Kod_odberatele"]);
                        string Nazev_firmy = GetString(row["Nazev_firmy"]);
                        string Ulice = GetString(row["Ulice"]);
                        string Psc = GetString(row["Psc"]);
                        string Misto = GetString(row["Misto"]);
                        string Stat = GetString(row["Stat"]);
                        string Telefon = GetString(row["Telefon"]);
                        string Mobil = GetString(row["Mobil"]);
                        string E_mail = GetString(row["E_mail"]);
                        string Login_www = GetString(row["Login_www"]);
                        string Heslo_www = GetString(row["Heslo_www"]);

                        string Stav_odberatele = GetString(row["Stav_odberatele"]);
                        string Statut = GetString(row["Statut"]);
                        int Stav = Convert.ToInt32(row["Stav"]);

                        bool Zakazat_www = Convert.ToBoolean(row["Zakazat_www"]);

                        string Dor_ulice = GetString(row["Dor_ulice"]);
                        string Dor_psc = GetString(row["Dor_psc"]);
                        string Dor_misto = GetString(row["Dor_misto"]);
                        string Dor_stat = GetString(row["Dor_stat"]);

                        string Banka = GetString(row["Banka"]);
                        string Cislo_uctu = GetString(row["Cislo_uctu"]);

                        if (row["Cislo_nadrizeneho"] == DBNull.Value) {
                            OnError("Chybi Cislo_nadrizeneho!");
                            return;
                        }
                        int Cislo_nadrizeneho = Convert.ToInt32(GetString(row["Cislo_nadrizeneho"]));
                        bool Platce_dph = Convert.ToBoolean(GetString(row["Platce_dph"]));

                        string Ico = GetString(row["Ico"]);
                        string Dic = GetString(row["Dic"]);

                        string Fax = GetString(row["Fax"]);
                        string Icq = GetString(row["Icq"]);
                        string Skype = GetString(row["Skype"]);
                        string Telefon_prace = GetString(row["Telefon_prace"]);
                        string Cislo_op = GetString(row["Cislo_op"]);
                        string P_f = GetString(row["P_f"]);
                        DateTime? Datum_narozeni = ConvertNullable.ToDateTime(row["Datum_narozeni"]);
                        DateTime? Datum_zahajeni = ConvertNullable.ToDateTime(row["Datum_zahajeni"]);
                        string Kod_oblasti = GetString(row["Kod_oblasti"]);

                        int Top_manazer = Convert.ToInt32(row["Top_manazer"]);

                        decimal marze = EuronaTVDDAL.GetTVDPoradcaMarze(this.SourceDataStorage, accountTVDId);
                        int obmedzenyPristup = Convert.ToInt32(row["Ar_jen_1_strana"]);
                        //Id_odberatele ,Kod_odberatele ,Stat_odberatele ,Cislo_nadrizeneho ,Nazev_firmy ,Nazev_firmy_radek ,Ulice ,Psc
                        //Misto ,Stat ,Dor_nazev_firmy ,Dor_nazev_firmy_radek ,Dor_ulice ,Dor_psc ,Dor_misto ,Dor_stat ,Telefon ,Mobil,
                        //E_mail ,Cislo_op ,Ico ,Dic ,P_f ,Banka ,Cislo_uctu ,Skupina ,Login_www ,Heslo_www ,Datum_zahajeni,
                        //Datum_pozastaveni ,Datum_ukonceni ,Stav_odberatele ,Platce_dph ,Statut ,Odpustit_limit ,Spec_symbol,
                        //Kod_oblasti ,Datum_narozeni ,Ar_na_dor_adr ,Leadersky_titul ,Telefon_prace ,Fax ,Icq,
                        //Skype ,Zakazat_www ,Ar_jen_1_strana ,Poznamka ,Stav, Top_manazer

                        //o.Angel_team_clen, o.Angel_team_manager
                        bool Angel_team_clen = Convert.ToBoolean(row["Angel_team_clen"]);
                        bool Angel_team_manager = Convert.ToBoolean(row["Angel_team_manager"]);
                        int Angel_team_manager_typ = Convert.ToInt32(row["Angel_team_typ_managera"]);

                        try {
                            int accountId = EuronaDAL.Account.SyncAccount(this.DestinationDataStorage, accountTVDId, InstanceId, Login_www, this.odberatel.Password, E_mail, !Zakazat_www, "Advisor;RegisteredUser", true, Datum_zahajeni);
                            if (accountId == 0) {
                                OnError("Account sa v datbazi Eurona nepodarilo vytvorit!");
                                return;
                            }
                            EuronaDAL.Account.SyncOrganization(this.DestinationDataStorage, accountId, this.InstanceId, kod_odberatele, E_mail, Nazev_firmy, Ico, Dic, Cislo_nadrizeneho, Platce_dph, Ulice, Misto, Psc, Stat, Telefon, Mobil,
                                    Dor_ulice, Dor_misto, Dor_psc, Dor_stat, Banka, Cislo_uctu, Top_manazer,
                                    Fax, Icq, Skype, Telefon_prace, Cislo_op, P_f, Datum_narozeni, Kod_oblasti, marze, obmedzenyPristup, Statut,
                                    Angel_team_clen, Angel_team_manager, Angel_team_manager_typ);

                            EuronaTVDDAL.UpdateTVDPoradciStav(this.SourceDataStorage, accountTVDId, 2);
                            OnItemProccessed(rowIndex, rowsCount, string.Format("Proccessing account TVD_Id '{0}' : ok", accountTVDId));
                        } catch (Exception ex) {
                            string errorMessage = string.Format("Proccessing account TVD_Id : {0} failed!", accountTVDId);
                            StringBuilder sbMessage = new StringBuilder();
                            sbMessage.Append(errorMessage);
                            sbMessage.AppendLine(ex.Message);
                            if (ex.InnerException != null) sbMessage.AppendLine(ex.InnerException.Message);
                            sbMessage.AppendLine(ex.StackTrace);

                            OnError(errorMessage, ex);
                        }

                    } catch (Exception ex) {
                        string errorMessage = "Proccessing accounts : failed!";
                        StringBuilder sbMessage = new StringBuilder();
                        sbMessage.Append(errorMessage);
                        sbMessage.AppendLine(ex.Message);
                        if (ex.InnerException != null) sbMessage.AppendLine(ex.InnerException.Message);
                        sbMessage.AppendLine(ex.StackTrace);

                        OnError(errorMessage, ex);
                        errorItems++;
                    } finally {
                        rowIndex++;
                    }

                } finally {
                    OnFinish(rowsCount, addedItems, updatedItems, errorItems, ignoredItems);
                }
            }
        }

        private string GetString(object obj) {
            if (obj == null) return null;
            return obj.ToString().Trim();
        }

    }
}
