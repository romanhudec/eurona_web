using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eurona.pay.csob {
    public class PaymentCart{

        public string name { get; set; }// String Název zboží, maximální délka 20 znaků 
        public int quantity { get; set; }// Number Množství, musí být >=1, celé číslo 
        public int amount { get; set; }// Number Celková cena za uvedené množství položek v setinách měny. Měna bude automaticky převzata z položky currency celého požadavku 
        public String description { get; set; }// String Popis položky košíku, maximální délka 40 znaků 

        public PaymentCart() {
        }

        public PaymentCart(string name, int quantity, int amount, String description) {
            this.name = name;
            this.quantity = quantity;
            this.amount = amount;
            this.description = description;
        }
    }
}