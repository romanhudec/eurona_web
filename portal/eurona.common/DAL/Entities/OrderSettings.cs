using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;

namespace Eurona.Common.DAL.Entities {
    [Serializable]
    public class OrderSettings : CMS.Entities.Entity {
        public OrderSettings() {
            this.Code = string.Empty;
            this.Enabled = true;
            this.Value = 0;
            this.Locale = string.Empty;
        }
        public class ReadById {
            public int Id { get; set; }
        }
        public class ReadByCode {
            public string Code { get; set; }
        }
        public string Code { get; set; }
        public bool Enabled { get; set; }
        public decimal Value { get; set; }
        public string Locale { get; set; }


        public static OrderSettings GetFreePostageSumaSK() {
            OrderSettings entity = Storage<OrderSettings>.ReadFirst(new OrderSettings.ReadByCode { Code = "ESHOP_ORDER_FREEE_POSTAGE_SK" });
            return entity;
        }
        public static OrderSettings GetFreePostageSumaCS() {
            OrderSettings entity = Storage<OrderSettings>.ReadFirst(new OrderSettings.ReadByCode { Code = "ESHOP_ORDER_FREEE_POSTAGE_CS" });
            return entity;
        }
        public static OrderSettings GetFreePostageSumaPL() {
            OrderSettings entity = Storage<OrderSettings>.ReadFirst(new OrderSettings.ReadByCode { Code = "ESHOP_ORDER_FREEE_POSTAGE_PL" });
            return entity;
        }

        public static decimal GetFreePostageSuma(string locale) {
            decimal resultValue = 999999999;
            OrderSettings entity = null;
            if( locale == "sk" ){ 
                entity = Storage<OrderSettings>.ReadFirst(new OrderSettings.ReadByCode { Code = "ESHOP_ORDER_FREEE_POSTAGE_SK" });
            }else if( locale == "cs" ){ 
                entity = Storage<OrderSettings>.ReadFirst(new OrderSettings.ReadByCode { Code = "ESHOP_ORDER_FREEE_POSTAGE_CS" });
            } else if (locale == "pl") {
                entity = Storage<OrderSettings>.ReadFirst(new OrderSettings.ReadByCode { Code = "ESHOP_ORDER_FREEE_POSTAGE_PL" });
            }

            if (entity == null) return resultValue;
            if (entity.Value <= 0) return resultValue;
            resultValue = entity.Value;
            return resultValue;
        }
    }
}
