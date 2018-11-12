using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Eurona.pay.csob {
    public class PaymentInitRequest : PaymentInitBase {
        /*
            merchantId String ID obchodníka přiřazené platební bránou 
            orderNo String Referenční číslo objednávky využívané pro párování plateb, které bude uvedeno také na výpisu z banky. Numerická hodnota, maximální délka 10 číslic. 
            dttm String Datum a čas odeslání požadavku ve formátu YYYYMMDDHHMMSS 
            payOperation String Typ platební operace. Povolené hodnoty: payment, oneclickPayment  
            payMethod String Typ implicitní platební metody, která bude nabídnuta zákazníkovi. Povolené hodnoty: card. 
            totalAmount Number Celková cena v setinách základní měny. Tato hodnota bude zobrazena na platební bráně jako celková částka k zaplacení 
            currency String Kód měny. Povolené hodnoty: CZK, EUR, USD, GBP, HUF, PLN, HRK, RON, NOK, SEK  
            closePayment Boolean Indikuje, zda má být platba automaticky zahrnuta do uzávěrky a proplacena. Povolené hodnoty: true / false. 
            returnUrl String URL adresa, na kterou bude klient přesměrován zpět do e-shopu po dokončení platby. Maximální délka 300 znaků. Níže je uveden obsah předávaných parametrů v přesměrování  
            returnMethod String Metoda návratu na URL adresu e-shopu. Povolené hodnoty POST, GET. Doporučená metoda je POST. 
            cart Object Seznam položek nákupu, který bude zobrazen na platební bráně. Obsahuje položky Item, popis položky viz níže  
            description String Stručný popis nákupu pro 3DS stránku: V případě ověření klienta na straně vydavatelské banky nelze zobrazit detail košíku jako na platební bráně. Do banky se proto posílá tento stručný popis nákupu. Maximální délka 255 znaků. 
            merchantData String Libovolná pomocná data, která budou vrácena ve zpětném redirectu z platební brány na stránku obchodníka. Mohou sloužit např pro udržení kontinuity procesu na e-shopu, musí být kódována v BASE64. Maximální délka po zakódování 255 znaků. 
            customerId String Jednoznačné ID zákazníka, který přiděluje e-shop. Maximální délka 50 znaků. Používá se při uložení karty a jejím následném použití při další návštěvě tohoto e-shopu 
            language String Preferovaná jazyková mutace, která se zobrazí zákazníkovi na platební bráně. Od verze 1.6 povinný parametr. Povolené hodnoty: CZ, EN, DE, FR, HU, IT, JP, PL, PT, RO, RU, SK, ES, TR, VN, HR, SI. Stejnou jazykovou sadu lze použít i v eAPI v1, v1.5 a V1.6 
            ttlSec Number Nastavení žívotnosti transakce, v sekundách, min. povolená hodnota 300, max. povolená hodnota 1800 (5-30 min) 
            logoVersion Number Verze schváleného loga obchodníka, které se pro danou transakci zobrazí. Pokud se bude jednat o dosud neschválené logo, použije se aktivní logo obchodníka, pokud není, defaultní logo platební brány 
            colorSchemeVersion Number Verze schváleného barevného schématu obchodníka, které se pro danou transakci zobrazí. Pokud se bude jednat o dosud neschválené barevné schéma, zobrazí se aktivní barevné schéma obchodníka, pokud není, zobrazí se defaultní barevné schéma platební brány 
            signature String Podpis požadavku, kódováno v BASE64 
        */

        public string merchantId{get;set;}// String ID obchodníka přiřazené platební bránou 
        public string orderNo{get;set;}// String Referenční číslo objednávky využívané pro párování plateb, které bude uvedeno také na výpisu z banky. Numerická hodnota, maximální délka 10 číslic. 
        public string dttm { get; set; }// String Datum a čas odeslání požadavku ve formátu YYYYMMDDHHMMSS 
        public string payOperation { get; set; }// String Typ platební operace. Povolené hodnoty: payment, oneclickPayment  
        public string payMethod { get; set; }// String Typ implicitní platební metody, která bude nabídnuta zákazníkovi. Povolené hodnoty: card. 
        public int totalAmount { get; set; }// Number Celková cena v setinách základní měny. Tato hodnota bude zobrazena na platební bráně jako celková částka k zaplacení 
        public string currency { get; set; }// String Kód měny. Povolené hodnoty: CZK, EUR, USD, GBP, HUF, PLN, HRK, RON, NOK, SEK  
        public bool closePayment { get; set; }// Boolean Indikuje, zda má být platba automaticky zahrnuta do uzávěrky a proplacena. Povolené hodnoty: true / false. 
        public string returnUrl { get; set; }// String URL adresa, na kterou bude klient přesměrován zpět do e-shopu po dokončení platby. Maximální délka 300 znaků. Níže je uveden obsah předávaných parametrů v přesměrování  
        public string returnMethod { get; set; }// String Metoda návratu na URL adresu e-shopu. Povolené hodnoty POST, GET. Doporučená metoda je POST. 
        public List<PaymentCart> cart { get; set; }// Object Seznam položek nákupu, který bude zobrazen na platební bráně. Obsahuje položky Item, popis položky viz níže  
        public string description { get; set; }// String Stručný popis nákupu pro 3DS stránku: V případě ověření klienta na straně vydavatelské banky nelze zobrazit detail košíku jako na platební bráně. Do banky se proto posílá tento stručný popis nákupu. Maximální délka 255 znaků. 
        public String merchantData { get; set; }// String Libovolná pomocná data, která budou vrácena ve zpětném redirectu z platební brány na stránku obchodníka. Mohou sloužit např pro udržení kontinuity procesu na e-shopu, musí být kódována v BASE64. Maximální délka po zakódování 255 znaků. 
        public String customerId { get; set; }// String Jednoznačné ID zákazníka, který přiděluje e-shop. Maximální délka 50 znaků. Používá se při uložení karty a jejím následném použití při další návštěvě tohoto e-shopu 
        public string language { get; set; }// String Preferovaná jazyková mutace, která se zobrazí zákazníkovi na platební bráně. Od verze 1.6 povinný parametr. Povolené hodnoty: CZ, EN, DE, FR, HU, IT, JP, PL, PT, RO, RU, SK, ES, TR, VN, HR, SI. Stejnou jazykovou sadu lze použít i v eAPI v1, v1.5 a V1.6 
        public Nullable<Int32> ttlSec { get; set; }// Number Nastavení žívotnosti transakce, v sekundách, min. povolená hodnota 300, max. povolená hodnota 1800 (5-30 min) 
        public Nullable<Int32> logoVersion { get; set; }// Number Verze schváleného loga obchodníka, které se pro danou transakci zobrazí. Pokud se bude jednat o dosud neschválené logo, použije se aktivní logo obchodníka, pokud není, defaultní logo platební brány 
        public Nullable<Int32> colorSchemeVersion { get; set; }// Number Verze schváleného barevného schématu obchodníka, které se pro danou transakci zobrazí. Pokud se bude jednat o dosud neschválené barevné schéma, zobrazí se aktivní barevné schéma obchodníka, pokud není, zobrazí se defaultní barevné schéma platební brány 
        public string signature { get; set; }// String Podpis požadavku, kódováno v BASE64 

        public override string getData2Sign() {
            string cart2Sign = "";
            foreach (PaymentCart paymentCart in this.cart) {
                if (!string.IsNullOrEmpty(cart2Sign)) cart2Sign += "|";
                cart2Sign += getStringValue(paymentCart.name) + "|" + getIntValue(paymentCart.quantity) + "|" + getIntValue(paymentCart.amount) + "|" + getStringValue(paymentCart.description);
            }
            string data2Sign = getStringValue(this.merchantId) + "|" + getStringValue(this.orderNo) + "|" + getStringValue(this.dttm) + "|" + getStringValue(this.payOperation) + "|" + getStringValue(this.payMethod) + "|" + getIntValue(this.totalAmount)
                    + "|" + getStringValue(this.currency) + "|" + getBoolValue(this.closePayment) + "|" + getStringValue(this.returnUrl) + "|" + getStringValue(this.returnMethod) + "|" + cart2Sign + "|" + getStringValue(this.description);

            string merchantData = this.merchantData;
            if (!string.IsNullOrEmpty(merchantData)) {
                data2Sign = data2Sign + "|" + getStringValue(merchantData);
            }

            string customerId = this.customerId;
            if (!string.IsNullOrEmpty(customerId) && customerId != "0") {
                data2Sign = data2Sign + "|" + getStringValue(customerId);
            }

            data2Sign = data2Sign + "|" + getStringValue(this.language);

            if (data2Sign[data2Sign.Length - 1] == '|') {
                data2Sign = data2Sign.Substring(0, data2Sign.Length - 1);//substr($data2Sign, 0, strlen($data2Sign) - 1);
            }
            return data2Sign;
        }
    }
}