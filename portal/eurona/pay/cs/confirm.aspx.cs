using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OrderEntity = Eurona.DAL.Entities.Order;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Configuration;

namespace Eurona.PAY.CS {
    public partial class ConfirmPage : WebPage {
        protected void Page_Load(object sender, EventArgs e) {
            //Po úspěšném dokončení autorizace transakce odešle ProxyPay, podobně jako u Validation
            //Post, HTTP POST požadavek s parametry definovanými v šabloně (vizte níže) na URL
            //specifikovaný obchodním partnerem (Confirmation Post). Obchodní partner zkontroluje, zda
            //data souhlasí s původně odeslanými údaji. Pokud ano, odpoví HTML řetězcem:
            //<html><head></head><body>[OK]</body></html>

            int orderId = 0;
            if (!Int32.TryParse(Request["merchantvar1"], out orderId)) Failed(0);

            OrderEntity order = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadById { OrderId = orderId });
            Transaction tran = Transaction.CreateTransaction(order, this);

            if (tran.Merchantref != Request["merchantref"]) Failed(orderId);
            if (tran.MerchantId != Request["merchantid"]) Failed(orderId);
            if (tran.Amount != Request["amountcents"]) Failed(orderId);
            if (tran.Currency != Request["currencycode"]) Failed(orderId);
            if (Request["password"] != "apjGPpj451op200muj92jvopPQm") Failed(orderId);

            //Update Order Entity Pay.
            order.PaymentCode = tran.Merchantref;
            order.PaydDate = DateTime.Now;
            Storage<OrderEntity>.Update(order);

            //Zdruzene Objednavky
            OrderEntity.ReadByFilter filter = new OrderEntity.ReadByFilter();
            filter.ParentId = order.Id;
            List<OrderEntity> listZdruzene = Storage<OrderEntity>.Read(filter);
            if (listZdruzene.Count != 0) {
                foreach (OrderEntity orderSdruzena in listZdruzene) {
                    orderSdruzena.PaymentCode = tran.Merchantref;
                    orderSdruzena.PaydDate = DateTime.Now;
                    Storage<OrderEntity>.Update(orderSdruzena);
                }
            }

            // Kontrola OK
            Success(order);
        }

        private bool makeTVPlatbaKartou(OrderEntity order) {
            string connectionString = ConfigurationManager.ConnectionStrings["TVDConnectionString"].ConnectionString;
            CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(connectionString);
            using (SqlConnection connection = tvdStorage.Connect()) {
                string sql = @"SELECT Suma = ([celkem_k_uhrade]-[castka_dobropis])
								FROM www_faktury WHERE id_prepoctu=@id_prepoctu";
                DataTable dt = tvdStorage.Query(connection, sql, new SqlParameter("@id_prepoctu", order.Id));

                decimal suma = order.PriceWVAT;
                if (dt.Rows.Count != 0) {
                    suma = Convert.ToDecimal(dt.Rows[0]["Suma"]);
                    string msg = string.Format("TVD Platba kartou -> Confirm(id_prepoctu:{0}, Suma:{1})", order.Id, suma);
                    CMS.EvenLog.WritoToEventLog(msg, EventLogEntryType.Information);
                } else {
                    string msg = string.Format("TVD Platba kartou -> Confirm(id_prepoctu:{0}, Suma:NULL!)", order.Id);
                    CMS.EvenLog.WritoToEventLog(msg, EventLogEntryType.Warning);
                }


                bool bSuccess = false;
                TVDPlatbaKartou(tvdStorage, connection, order.Id, suma, order.CurrencyCode, out bSuccess);
                return bSuccess;
            }
        }

        private void Success(OrderEntity order) {
            makeTVPlatbaKartou(order);
            //Zdruzene Objednavky
            OrderEntity.ReadByFilter filter = new OrderEntity.ReadByFilter();
            filter.ParentId = order.Id;
            List<OrderEntity> listZdruzene = Storage<OrderEntity>.Read(filter);
            if (listZdruzene.Count != 0) {
                foreach (OrderEntity orderSdruzena in listZdruzene) {
                    makeTVPlatbaKartou(orderSdruzena);
                }
            }

            Session["payment_order"] = order.Id; ;
#if __LOCAL_BANK_PAY
            Response.Redirect(ResolveUrl("~/pay/cs/ok.aspx"));
#else
						Response.Write( "<html><head></head><body>[OK]</body></html>" );
						Response.End();
#endif
        }
        private void Failed(int orderId) {
            Session["payment_order"] = orderId;
#if __LOCAL_BANK_PAY
            Response.Redirect(ResolveUrl("~/pay/cs/nok.aspx"));
#else
						Response.Write( "<html><head></head><body>[FAILED]</body></html>" );
						Response.End();
#endif
        }

        private string TVDPlatbaKartou(CMS.Pump.MSSQLStorage tvdStorage, SqlConnection connection, int orderId, decimal suma, string kodMeny, out bool bSuccess) {
#if __DEBUG_VERSION_WITHOUTTVD
            bSuccess = true;
            return string.Empty;
#endif
            //===============================================================================
            // ZAPIS PLATBy KARTOU
            //===============================================================================
            //@out_Probehlo OUTPUT    -- 0=chyba, 1=OK
            //,@out_Zprava OUTPUT    -- v případě chyby ev. zpráva k chybě, jinak 'OK'

            //@Cislo_objednavky int,
            //@Castka money,
            //@Kod_meny char(3)  

            SqlParameter probehlo = new SqlParameter("@out_Probehlo", false);
            probehlo.Direction = ParameterDirection.Output;

            SqlParameter zprava = new SqlParameter("@out_Zprava", string.Empty);
            zprava.Direction = ParameterDirection.Output;
            zprava.SqlDbType = SqlDbType.VarChar;
            zprava.Size = 255;

            /*
                @Cislo_objednavky int,
                @Castka money,
                @Kod_meny char(3)             
             */

            try {
                tvdStorage.ExecProc(connection, "esp_www_platbakartou",
                        new SqlParameter("Id_prepoctu", orderId),
                        new SqlParameter("Castka", suma),
                        new SqlParameter("Kod_meny", kodMeny),
                        probehlo, zprava);
            } catch (Exception ex) {
                CMS.EvenLog.WritoToEventLog(ex);
                bSuccess = false;
                if (zprava.Value != null) return zprava.Value.ToString();
                return "Eurosap odmítl platbu spracovat!";
            }


            //===============================================================================
            //Vystupne parametra
            //===============================================================================
            //Probehlo	bit	1=úspěch, 0=chyba		
            //Zprava	varchar(255)	text chyby		
            //id_prepoctu	int	prim. klíč		
            bSuccess = Convert.ToBoolean(probehlo.Value);
            if (zprava != null && zprava.Value != null) {
                string msg = string.Format("TVD Platba kartou -> esp_www_platbakartou(Cislo_objednavky:{0}) = {1}", orderId, zprava.Value.ToString());
                CMS.EvenLog.WritoToEventLog(msg, EventLogEntryType.Information);
            }
            return zprava.Value.ToString();
        }
    }
}
