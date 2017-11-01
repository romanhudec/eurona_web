using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using AccountEntity = CMS.Entities.Account;
using OrderEntity = Eurona.DAL.Entities.Order;
using ShpPaymentEntity = SHP.Entities.Classifiers.Payment;

namespace Eurona.PAY.CS
{
    public class Transaction
    {
        //<form NAME="MERCHANTFORM" method="POST" action="https://3dsecure.csas.cz/transaction">
        //<INPUT TYPE="hidden" NAME="merchantid" VALUE="101">
        //<INPUT TYPE="hidden" NAME="amount" VALUE="500">
        //<INPUT TYPE="hidden" NAME="currency" VALUE="203">
        //<INPUT TYPE="hidden" NAME="brand" VALUE="VISA" >
        //<INPUT TYPE="hidden" NAME="transactiontype" VALUE="sale" >
        //<INPUT TYPE="hidden" NAME="merchantref" VALUE="3453453444" >
        //<INPUT TYPE="hidden" NAME="merchantdesc" VALUE="Your Order 3453453444">
        //<INPUT TYPE="hidden" NAME="extension_recurringfrequency" VALUE="28">
        //<INPUT TYPE="hidden" NAME="extension_recurringenddate" VALUE="20041224">
        //<INPUT TYPE="hidden" NAME="language" VALUE="EN">
        //<INPUT TYPE="hidden" NAME="emailcustomer" VALUE="cardholder@email.cz">
        //<INPUT TYPE="hidden" NAME="merchantvar1" VALUE="ERPFunction1_12">
        //<INPUT TYPE="hidden" NAME="merchantvar2" VALUE="ERPRef1_45">
        //<INPUT TYPE="hidden" NAME="merchantvar3">
        //<INPUT TYPE="hidden" NAME="var1" VALUE="Zbozi: Walkman XYZ">
        //<INPUT TYPE="hidden" NAME="var2">
        //<INPUT TYPE="hidden" NAME="var3">
        //<INPUT TYPE="hidden" NAME="var4">
        //<INPUT TYPE="hidden" NAME="var5">
        //<INPUT TYPE="hidden" NAME="var6">
        //<INPUT TYPE="hidden" NAME="var7" VALUE="Bubenská 1">
        //<INPUT TYPE="hidden" NAME="var8" VALUE="150 00 Praha 5">
        //<INPUT TYPE="hidden" NAME="var9" VALUE="http://www.obchod.cz">
        //<INPUT TYPE=image SRC="button.gif" BORDER=0 VALUE="SSL" Alt="Zaplatit kartou">
        //</form>

        /// <summary>
        /// unikátní identifikační číslo přiřazené obchodnímu partnerovi (95****)
        /// </summary>
        public string MerchantId { get; set; }
        /// <summary>
        /// celková částka objednávky (podrobnosti viz. currency níže). Tento parametr je zobrazován
        /// na platební stránce ČS, kam držitel karty zadává údaje o kartě.
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        /// kód měny dle ISO 4217 (v číselném formátu)
        /// Je třeba věnovat pozornost kombinaci částky a měny – měna s exponentem 0 je
        /// reprezentována v jednotkách, zatímco měna s exponentem 2 v centech.
        /// Povolené měny a jim odpovídající nastavení currency a exponent jsou uvedeny v tabulce:
        /// Měna										currency				exponent
        /// Česká koruna (CZK)			203							2
        /// Euro (EUR)							978							2
        /// Britská libra (GBP)			826							2
        /// Americký dolar (USD)		840							2
        /// Příklad: Při odeslání částky 500 a užití měny CZK,EUR,GBP, USD se jedná o částku 5,00.
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// typ karty, možné hodnoty „VISA“, „MasterCard“, „VisaElectron“, „Maestro“. Je nutné dodržet
        /// přesný formát (case sensitive).
        /// </summary>
        public string Brand { get; set; }
        /// <summary>
        /// typ platby. Pole může nabývat následujících hodnot:
        /// · „authorisation“ – provede se pouze rezervace prostředků na účtu držitele karty. Dalším
        /// krokem musí být ruční dokončení transakce v Merchant Back Office (max. do 7 dnů od
        /// provedení transakce). Zpracování se provádí při expedici zboží (Capture) nebo v případě,
        /// že zboží nelze expedovat (Cancel).
        /// E – Commerce – Integrace do systému obchodníka 10
        /// · „sale“ – direct sale data – dojde k autorizaci a rovnou také k odúčtování částky z účtu
        /// držitele karty. Pro navrácení prostředků držiteli karty je možné provést v Merchant
        /// BackOffice „refundaci“ (max. do 180 dnů od provedení transakce).
        /// </summary>
        public string Transactiontype { get; set; }
        /// <summary>
        /// Referenční číslo transakce. Hodnota merchantRef musí být pro každou transakci
        /// unikátní. V opačném případě bude transakce zamítnuta.
        /// Maximální možná délka tohoto parametru je 10 alfanumerických znaků (tj. [A-Za-z0-9] ).
        /// Tento parametr je vhodné použít pro případné párování transakcí v systému obchodního
        /// partnera, neboť je uváděn mimo jiné v zasílaných avízech.
        /// </summary>
        public string Merchantref { get; set; }
        /// <summary>
        /// Krátký popis objednávky (např. pro další použití v e-mailových notifikacích). Tento
        /// parametr je zobrazován na platební stránce ČS, kam držitel karty zadává údaje o kartě.
        /// Maximální možná délka tohoto parametru je 125 znaků. V případě, že bude pole
        /// merchantDesc obsahovat řetězec delší než 125, bude tento systémem ProxyPay ořezán.
        /// </summary>
        public string Merchantdesc { get; set; }
        /// <summary>
        /// Pravidelné platby - tento parametr definuje počet dní mezi
        /// pravidelnými platbami. Číslo 28 je speciální hodnota, která znamená generování transakce
        /// každý měsíc. V případě, že se jedná o jednorázovou platbu, je nutné odeslat prázdnou
        /// hodnotu nebo pole vypustit úplně.
        /// </summary>
        public string ExtensionRecurringfrequency { get; set; }
        /// <summary>
        /// Pravidelné platby – tento parametr určuje datum, po kterém již
        /// transakce nebudou generovány. Formát je ‘YYYYMMDD’. V případě, že se jedná o
        /// jednorázovou platbu, je nutné odeslat prázdnou hodnotu nebo pole vypustit úplně.
        /// </summary>
        public string ExtensionRecurringenddate { get; set; }
        /// <summary>
        /// jazyk pro volbu šablon. Stránka, která je zobrazena držiteli karty je vygenerována
        /// pomocí šablony v systému ProxyPay. Tyto šablony mohou být ve více jazycích, dle tohoto
        /// pole pak systém vygeneruje stránku v příslušném jazyce.
        /// Pole může obsahovat hodnoty :
        /// „CZ“ – čeština
        /// „EN“ – angličtina
        /// „DE“ – němčina
        /// Je nutné použít velká písmena.
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// E-mailová adresa zákazníka pro zaslání potvrzovací zprávy.
        /// </summary>
        public string EmailCustomer { get; set; }
        /// <summary>
        /// Všechna další pole, která nejsou vyjmenována výše, jsou
        /// obchodním partnerem definovatelná. Mohou obsahovat libovolné hodnoty. Tyto hodnoty jsou
        /// pak odeslány v potvrzovací zprávě obchodnímu partnerovi (confirmation skript) a mohou být
        /// použity pro další zpracování na straně obchodního partnera.
        /// V příkladu výše je použito pole: NAME=“MerchantVar1“ VALUE=“ERPFunction1_12“.
        /// Potvrzovací zpráva bude tedy obsahovat: …&MerchantVar1=ERPFunction1_12…
        /// </summary>
        public string MerchantVar1 { get; set; }
        public string MerchantVar2 { get; set; }
        public string MerchantVar3 { get; set; }

        /// <summary>
        /// var1 – var9: Proměnné definovatelné obchodním partnerem. Mohou být samostatně adresovány
        /// v šablonách. Maximální povolená délka proměnné je 255 znaků.
        /// Zavedeno následující použití:
        /// var1 – var6: Vhodné především pro zanesení adresy pro doručení zboží, názvu zboží,
        /// způsobu dodání atd.)
        /// var7: Adresa obch. partnera 1
        /// var8: Adresa obch. partnera 2
        /// var9: Internetová adresa obchodu
        /// Tyto proměnné jsou dále použity např. v konfirmačních e-mailech zasílaných držitelům karet
        /// (popsáno v kapitole 6 - E-mailové šablony).
        /// </summary>
        public string Var1 { get; set; }
        public string Var2 { get; set; }
        public string Var3 { get; set; }
        public string Var4 { get; set; }
        public string Var5 { get; set; }
        public string Var6 { get; set; }
        /// <summary>
        /// var7: Adresa obch. partnera 1
        /// </summary>
        public string Var7 { get; set; }
        /// <summary>
        /// var8: Adresa obch. partnera 2
        /// </summary>
        public string Var8 { get; set; }
        /// <summary>
        /// var9: Internetová adresa obchodu
        /// </summary>
        public string Var9 { get; set; }

        /// <summary>
        /// Metoda odosle incializacne data transakcie
        /// </summary>
        public string PostTransaction(System.Web.UI.Page page)
        {

#if __LOCAL_BANK_PAY
			string absolute = string.Format("{0}://{1}{2}", page.Request.Url.Scheme, page.Request.Url.Authority, page.ResolveUrl("~/pay/test-post/transaction.aspx"));
			Uri uri = new Uri(absolute);
#else
            Uri uri = new Uri("https://3dsecure.csas.cz/transaction");
#endif
            // Vykonanie POSTu
            WebClient wc = new WebClient();
            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            // Povolit vsetky certifikaty
            ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, error) =>
            {
                return true;
            };

            //Method 1
            /*
            StringBuilder data = new StringBuilder();
            data.AppendFormat("merchantid={0}&", this.MerchantId);
            data.AppendFormat("amount={0}&", this.Amount);
            data.AppendFormat("currency={0}&", this.Currency);
            data.AppendFormat("brand={0}&", this.Brand);
            data.AppendFormat("transactiontype={0}&", this.Transactiontype);
            data.AppendFormat("merchantref={0}&", this.Merchantref);
            data.AppendFormat("merchantdesc={0}&", this.Merchantdesc);
            data.AppendFormat("language={0}&", this.Language);
            data.AppendFormat("emailcustomer={0}&", this.EmailCustomer);
            data.AppendFormat("merchantvar1={0}&", this.MerchantVar1);
            data.AppendFormat("merchantvar2={0}&", this.MerchantVar2);
            data.AppendFormat("merchantvar3={0}&", this.MerchantVar3);
            data.AppendFormat("var1={0}&", this.Var1);
            data.AppendFormat("var2={0}&", this.Var2);
            data.AppendFormat("var3={0}&", this.Var3);
            data.AppendFormat("var4={0}&", this.Var4);
            data.AppendFormat("var5={0}&", this.Var5);
            data.AppendFormat("var6={0}&", this.Var6);
            data.AppendFormat("var7={0}&", this.Var7);
            data.AppendFormat("var8={0}&", this.Var8);
            data.AppendFormat("var9={0}&", this.Var9);
            data.AppendFormat("referrer={0}", "http://www.euronabycerny.com/");
             
			byte[] received = null;
			try
			{
				string htmlData = page.Server.HtmlDecode(data.ToString());
				received = wc.UploadData(uri, "POST", Encoding.UTF8.GetBytes(htmlData));
			}
			catch (WebException wex)
			{
				page.Response.Write(wex.ToString());
				page.Response.Flush();
                page.Response.End();
				return null;
			}
            			return Encoding.UTF8.GetString(received);
             * */

            //Method 2
            StringBuilder htmlForm = new StringBuilder();
            htmlForm.AppendLine("<html>");
            htmlForm.AppendLine(String.Format("<body onload=\"document.forms['{0}'].submit()\">", "EURONA_MERCHANTFORM"));
            htmlForm.AppendLine(String.Format("<form id=\"{0}\" method=\"POST\" action=\"{1}\">", "EURONA_MERCHANTFORM", "https://3dsecure.csas.cz/transaction"));
            htmlForm.AppendLine(GetInputHiddenString("merchantid", this.MerchantId));
            htmlForm.AppendLine(GetInputHiddenString("amount", this.Amount));
            htmlForm.AppendLine(GetInputHiddenString("currency", this.Currency));
            htmlForm.AppendLine(GetInputHiddenString("brand", this.Brand));
            htmlForm.AppendLine(GetInputHiddenString("transactiontype", this.Transactiontype));
            htmlForm.AppendLine(GetInputHiddenString("merchantref", this.Merchantref));
            htmlForm.AppendLine(GetInputHiddenString("merchantdesc", this.Merchantdesc));
            htmlForm.AppendLine(GetInputHiddenString("language", this.Language));
            htmlForm.AppendLine(GetInputHiddenString("emailcustomer", this.EmailCustomer));
            htmlForm.AppendLine(GetInputHiddenString("merchantvar1", this.MerchantVar1));
            htmlForm.AppendLine(GetInputHiddenString("merchantvar2", this.MerchantVar2));
            htmlForm.AppendLine(GetInputHiddenString("merchantvar3", this.MerchantVar3));
            htmlForm.AppendLine(GetInputHiddenString("var1", this.Var1));
            htmlForm.AppendLine(GetInputHiddenString("var2", this.Var2));
            htmlForm.AppendLine(GetInputHiddenString("var3", this.Var3));
            htmlForm.AppendLine(GetInputHiddenString("var4", this.Var4));
            htmlForm.AppendLine(GetInputHiddenString("var5", this.Var5));
            htmlForm.AppendLine(GetInputHiddenString("var6", this.Var6));
            htmlForm.AppendLine(GetInputHiddenString("var7", this.Var7));
            htmlForm.AppendLine(GetInputHiddenString("var8", this.Var8));
            htmlForm.AppendLine(GetInputHiddenString("var9", this.Var9));
            htmlForm.AppendLine(GetInputHiddenString("referrer", "http://www.euronabycerny.com"));
            htmlForm.AppendLine("</form>");
            htmlForm.AppendLine("</body>");
            htmlForm.AppendLine("</html>");

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(htmlForm.ToString());
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            return null;
        }

        private string GetInputHiddenString(String name, string value)
        {
            return String.Format("<input type=\"hidden\" name=\"{0}\" value=\"{1}\" />", name, value);
        }

        /// <summary>
        /// Vrati spravny format retazca pre Merchantref
        /// </summary>
        public static string FormatMerchantref(string str)
        {
            string result = string.Empty;
            foreach (char c in str)
            {
                if (!Char.IsLetterOrDigit(c)) continue;
                result += c;
            }

            return result;
        }

        /// <summary>
        /// Vytorenie objektu Pay transakcie.
        /// </summary>
        public static Transaction CreateTransaction(OrderEntity order, System.Web.UI.Page page)
        {
            AccountEntity account = Storage<AccountEntity>.ReadFirst(new AccountEntity.ReadById { AccountId = order.AccountId });
            decimal priceTotalWithWAT = order.PriceWVAT;
            
            //Zdruzene Objednavky
            OrderEntity.ReadByFilter filter = new OrderEntity.ReadByFilter();
            filter.ParentId = order.Id;
            List<OrderEntity> list = Storage<OrderEntity>.Read(filter);
            if (list.Count != 0) {
                foreach (OrderEntity orderSdruzena in list) {
                    priceTotalWithWAT += orderSdruzena.PriceWVAT;
                }
            }

            /// „CZ“ – čeština
            /// „EN“ – angličtina
            /// „DE“ – němčina
            string language = "CZ";
            string locale = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower();
            if (locale == "cs") language = "CZ";
            if (locale == "en") language = "EN";
            if (locale == "de") language = "DE";

            string currency = "203";/*CZK*/
            if (order.CurrencyCode == "EUR") currency = "978";
            if (order.CurrencyCode == "PLN") currency = "616";

            Transaction tran = new Transaction();
            tran.MerchantId = CMS.Utilities.ConfigUtilities.ConfigValue("SHP:PAY:MerchantID", page);
            tran.Brand = order.PaymentCode;
            tran.Amount = string.Format("{0}", (int)(priceTotalWithWAT * 100));
            tran.Currency = currency;
            tran.Transactiontype = "sale";
            tran.Merchantref = Transaction.FormatMerchantref(order.OrderNumber);
            tran.Merchantdesc = order.OrderNumber;
            tran.Language = language;
            tran.EmailCustomer = account.Email;
            tran.MerchantVar1 = order.Id.ToString();
            tran.Var1 = order.DeliveryAddress.Display.Length > 255 ? order.DeliveryAddress.Display.Substring(0, 254) : order.DeliveryAddress.Display;
            tran.Var2 = order.ShipmentName;
            tran.Var7 = CMS.Utilities.ConfigUtilities.ConfigValue("SHP:PAY:Var7", page);
            tran.Var8 = CMS.Utilities.ConfigUtilities.ConfigValue("SHP:PAY:Var8", page);
            tran.Var9 = "http://www.euronabycerny.com";

            return tran;
        }
    }
}