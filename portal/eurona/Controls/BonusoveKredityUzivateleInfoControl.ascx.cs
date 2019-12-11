using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Eurona.DAL.Entities;
using Eurona.Common.DAL.Entities;

namespace Eurona.Controls {
    public partial class BonusoveKredityUzivateleInfoControl : System.Web.UI.UserControl {
        private List<BKUSumar> list;
        protected void Page_Load(object sender, EventArgs e) {
            ReloadControlData();
        }

        public void ReloadControlData() {
            //DateTime date = DateTime.Now.AddMonths(-1);
            DateTime date = BonusovyKreditUzivateleHelper.GetCurrentObdobiFromTVD();
            date = date.AddMonths(-1);

            this.lblDosazenoTentoMesic.Text = BonusovyKreditUzivateleHelper.GetBonusoveKredityUzivateleNazbiraneTentoMesicCelkem(/*Security.Account.Id*/this.AccountId).ToString("F0");
            this.lblPlatnychTentoMesic.Text = "0";
            decimal platnychNaTentoMesic = BonusovyKreditUzivateleHelper.GetPlatnyKreditCelkem(this.Account, date.Year, date.Month);
            decimal cerpanoTentoMesic = BonusovyKreditUzivateleHelper.GetCerpaniKredituCelkem(this.Account, date.Year, date.Month);
            this.lblPlatnychTentoMesic.Text = platnychNaTentoMesic.ToString("F0");
            this.lblCerpanoTentoMesicCelkem.Text = cerpanoTentoMesic.ToString("F0");
            this.lblZbyvaCelkemTentoMesic.Text = (platnychNaTentoMesic - cerpanoTentoMesic).ToString("F0");
            /*
            this.list = new List<BKUSumar>();
            List<BonusovyKredit> bkList = Storage<BonusovyKredit>.Read();
            foreach (BonusovyKredit bk in bkList) {
                switch (bk.Typ) {
                    case (int)DAL.Entities.Classifiers.BonusovyKreditTyp.ProduktDetail: {
                            BKUSumar sum = new BKUSumar();
                            sum.Typ = (int)DAL.Entities.Classifiers.BonusovyKreditTyp.ProduktDetail;
                            sum.Id = bk.Id;
                            sum.Nazev = "Zobrazení detailu výrobku";
                            sum.Pocet = bk.Kredit.ToString("F0");
                            sum.Order = 10000;
                            this.list.Add(sum);
                        } break;
                    case (int)DAL.Entities.Classifiers.BonusovyKreditTyp.ProduktHodnoceni: {
                            BKUSumar sum = this.list.FirstOrDefault(x => x.Typ == (int)DAL.Entities.Classifiers.BonusovyKreditTyp.ProduktHodnoceni);
                            if (sum == null) {
                                sum = new BKUSumar();
                                sum.Typ = (int)DAL.Entities.Classifiers.BonusovyKreditTyp.ProduktHodnoceni;
                                sum.Id = bk.Id;
                                sum.Nazev = "Hodnocení produktu uživatelem";
                                sum.Pocet = "0-" + bk.HodnotaDo.Value.ToString("F0");
                                sum.Order = (int)(20000m + bk.HodnotaOd.Value);
                                this.list.Add(sum);
                            } else
                                sum.Pocet = "0-" + bk.HodnotaDo.Value.ToString("F0");
                        } break;
                    case (int)DAL.Entities.Classifiers.BonusovyKreditTyp.OdeslanaObjednavka: {
                            BKUSumar sum = new BKUSumar();
                            sum.Typ = (int)DAL.Entities.Classifiers.BonusovyKreditTyp.OdeslanaObjednavka;
                            sum.Id = bk.Id;
                            if (bk.HodnotaDo.Value < 9999) {
                                string odMena = "";
                                string doMena = "";
                                if (bk.HodnotaDo <= 1999) {
                                    odMena = "(39€/66zl)"; doMena = "(79.99€/332,99zl)";
                                } else if (bk.HodnotaDo > 1999 && bk.HodnotaDo <= 3999) {
                                    odMena = "(80€/333zl)"; doMena = "(159,99€/666,99zl)";
                                }
                                sum.Nazev = string.Format("Objednávka on-line {0} Kč {2}  až {1} Kč {3}",
                                        bk.HodnotaOd.Value.ToString("F0"), bk.HodnotaDo.Value.ToString("F0"), odMena, doMena);
                            } else {
                                sum.Nazev = string.Format("Objednávka on-line nad {0} Kč (160€/667zl)", bk.HodnotaOd.Value.ToString("F0"));
                            }
                            sum.Pocet = bk.Kredit.ToString("F0");
                            sum.Order = (int)(30000m + bk.HodnotaOd.Value);
                            this.list.Add(sum);

                        } break;
                    case (int)DAL.Entities.Classifiers.BonusovyKreditTyp.ProduktEmailPoslatPriteli: {
                            BKUSumar sum = new BKUSumar();
                            sum.Typ = (int)DAL.Entities.Classifiers.BonusovyKreditTyp.ProduktEmailPoslatPriteli;
                            sum.Id = bk.Id;
                            sum.Nazev = "Zaslání produktu příjemci e-mailem";
                            sum.Pocet = bk.Kredit.ToString("F0");
                            sum.Order = 40000;
                            this.list.Add(sum);
                        } break;
                    case (int)DAL.Entities.Classifiers.BonusovyKreditTyp.ProduktEmailFacebook: {
                            BKUSumar sum = new BKUSumar();
                            sum.Typ = (int)DAL.Entities.Classifiers.BonusovyKreditTyp.ProduktEmailFacebook;
                            sum.Id = bk.Id;
                            sum.Nazev = "Zaslání produktu na zeď Facebook";
                            sum.Pocet = bk.Kredit.ToString("F0");
                            sum.Order = 50000;
                            this.list.Add(sum);
                        } break;
                    case (int)DAL.Entities.Classifiers.BonusovyKreditTyp.ShareAkcniNabidkyFacebook: {
                            BKUSumar sum = new BKUSumar();
                            sum.Typ = (int)DAL.Entities.Classifiers.BonusovyKreditTyp.ShareAkcniNabidkyFacebook;
                            sum.Id = bk.Id;
                            sum.Nazev = "Sdílení akčních nabídek na Facebooku";
                            sum.Pocet = bk.Kredit.ToString("F0");
                            sum.Order = 60000;
                            this.list.Add(sum);
                        } break;
                    case (int)DAL.Entities.Classifiers.BonusovyKreditTyp.ShareSpecialniNabidkyFacebook: {
                            BKUSumar sum = new BKUSumar();
                            sum.Typ = (int)DAL.Entities.Classifiers.BonusovyKreditTyp.ShareSpecialniNabidkyFacebook;
                            sum.Id = bk.Id;
                            sum.Nazev = "Sdílení speciálních nabídek na Facebooku";
                            sum.Pocet = bk.Kredit.ToString("F0");
                            sum.Order = 70000;
                            this.list.Add(sum);
                        } break;
                    case (int)DAL.Entities.Classifiers.BonusovyKreditTyp.RucneZadany: {
                            BKUSumar sum = new BKUSumar();
                            sum.Typ = (int)DAL.Entities.Classifiers.BonusovyKreditTyp.RucneZadany;
                            sum.Id = bk.Id;
                            sum.Nazev = "Prémiové bonusové kredity";
                            sum.Pocet = "0-X";
                            sum.Order = 99999;
                            this.list.Add(sum);
                        } break;
                }
            }

            DataTable dt = GetBonusoveKredityUzivateleSumar(DateTime.Now.Year, DateTime.Now.Month);
            foreach (DataRow row in dt.Rows) {
                //bku.BonusovyKreditId, Ks=Count(*),  Celkem = SUM(bku.Hodnota)
                int bkId = Convert.ToInt32(row["BonusovyKreditId"]);
                int bkTyp = Convert.ToInt32(row["Typ"]);
                int ks = Convert.ToInt32(row["Ks"]);
                int celkem = Convert.ToInt32(row["Celkem"]);
                foreach (BKUSumar sum in this.list) {
                    if (bkTyp == (int)DAL.Entities.Classifiers.BonusovyKreditTyp.ProduktHodnoceni &&
                            sum.Typ == (int)DAL.Entities.Classifiers.BonusovyKreditTyp.ProduktHodnoceni) {
                        int ksTmp = sum.Ks;
                        int celkemTmp = sum.Celkem;
                        sum.Ks = (ksTmp + ks);
                        sum.Celkem = (celkemTmp + celkem);
                    } else {
                        if (sum.Id != bkId) continue;
                        sum.Ks = ks;
                        sum.Celkem = celkem;
                    }
                }
            }

            this.list.Sort();
            //Celkem
            BKUSumar sumC = new BKUSumar();
            sumC.Nazev = "CELKEM";
            sumC.Ks = 0;
            foreach (BKUSumar sum in this.list) {
                sumC.Ks = sumC.Ks + sum.Ks;
                sumC.Celkem = sumC.Celkem + sum.Celkem;
            }
            this.list.Add(sumC);
            this.rpBKUzivatele.DataSource = this.list;
            this.rpBKUzivatele.DataBind();
            */
        }

        public int AccountId { get; set; }
        private Account account = null;
        public Account Account {
            get {
                if (account != null) return account;
                account = Storage<Account>.ReadFirst(new Account.ReadById { AccountId = this.AccountId });
                return account;
            }
        }

        /// <summary>
        /// Pocet BK platnych tento mesiac za dany typ BK
        /// </summary>
        /// <param name="typ"></param>
        /// <returns></returns>
        public decimal GetBonusoveKredityUzivatelePlatneTentoMesic(DAL.Entities.Classifiers.BonusovyKreditTyp typ) {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            CMS.Pump.MSSQLStorage storage = new CMS.Pump.MSSQLStorage(connectionString);
            using (SqlConnection connection = storage.Connect()) {
                DateTime now = DateTime.Now.AddMonths(-1);
                string sql = @"SELECT Kredit=ISNULL(SUM(Hodnota),0) FROM vBonusoveKredityUzivatele
								WHERE AccountId = @accountId AND Typ=@typ AND
								((YEAR(PlatnostOd)=@rok AND MONTH(PlatnostOd)=@mesic) OR 
								(YEAR(PlatnostDo)=@rok AND MONTH(PlatnostDo)=@mesic))";
                DataTable dt = storage.Query(connection, sql,
                        new SqlParameter("@mesic", now.Month),
                        new SqlParameter("@rok", now.Year),
                        new SqlParameter("@accountId", this.AccountId),
                        new SqlParameter("@typ", typ));

                if (dt.Rows.Count == 0) return 0;
                return Convert.ToDecimal(dt.Rows[0]["Kredit"]);
            }
        }

        /// <summary>
        /// Celkovy sumar BK
        /// </summary>
        /// <returns></returns>
        private DataTable GetBonusoveKredityUzivateleSumar(int rok, int mesic) {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            CMS.Pump.MSSQLStorage storage = new CMS.Pump.MSSQLStorage(connectionString);
            using (SqlConnection connection = storage.Connect()) {
                string sql = @"SELECT 
					bku.Typ, bku.BonusovyKreditId, Ks=Count(*),  Celkem = SUM(bku.Hodnota) 
					FROM vBonusoveKredityUzivatele bku
					INNER JOIN vBonusoveKredity bk ON bk.BonusovyKreditId = bku.BonusovyKreditId
					WHERE bku.AccountId = @accountId AND
					(YEAR(bku.Datum)=@rok AND MONTH(bku.Datum)=@mesic)
					GROUP BY bku.Typ, bku.BonusovyKreditId";
                DataTable dt = storage.Query(connection, sql,
                        new SqlParameter("@accountId", this.AccountId),
                        new SqlParameter("@mesic", mesic),
                        new SqlParameter("@rok", rok)
                        );

                return dt;
            }
        }

        public class BKUSumar : IComparable<BKUSumar> {
            public BKUSumar() {
                this.Ks = 0;
                this.Celkem = 0;
            }

            public int Id { get; set; }
            public int Typ { get; set; }
            public string Nazev { get; set; }
            public string Pocet { get; set; }
            public int Ks { get; set; }
            public int Celkem { get; set; }
            public int Order { get; set; }

            public int CompareTo(BKUSumar obj) {
                return Order.CompareTo(obj.Order);
            }

        }

    }
}