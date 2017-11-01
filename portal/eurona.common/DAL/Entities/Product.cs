using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;

namespace Eurona.Common.DAL.Entities {
    public class Product : SHP.Entities.Product {
        public const int INTERNAL_STORAGE_NOT_AVAILABLE = -1;
        /// <summary>
        /// Načíta všetky produkty, ktoré su v danej kategórii a jej podkategóriach.
        /// </summary>
        public new class ReadAllInCategory {
            public int CategoryId { get; set; }
            public ReadByFilter ByFilter { get; set; }
        }
        /// <summary>
        /// Načíta produkty podľa filtra.
        /// </summary>
        public new class ReadByFilter {
            public int? MaxCount { get; set; }
            public bool? TOPProducts { get; set; }
            public bool? DarkoveSety { get; set; }

            public bool? Novinka { get; set; }
            public bool? Inovace { get; set; }
            public bool? Doprodej { get; set; }
            public bool? Vyprodano { get; set; }
            public bool? ProdejUkoncen { get; set; }

            public bool? Megasleva { get; set; }
            public bool? Supercena { get; set; }
            public bool? CLHit { get; set; }
            public bool? Action { get; set; }
            public bool? Vyprodej { get; set; }
            public bool? OnWeb { get; set; }

            public bool? BestSellers { get; set; }
            public string Manufacturer { get; set; }
            public string Expression { get; set; }
            public decimal? PriceFrom { get; set; }
            public decimal? PriceTo { get; set; }
            public SortBy SortBy { get; set; }

            //public ReadHighlights Highlights { get; set; }
        }

        public class ReadByCode {
            public string Code { get; set; }
        }

        public class ReadVamiNejviceNakupovane {
        }

        public class ReadDarkoveSety {
        }

        public class ReadWithBK { }


        public decimal? Body { get; set; }
        public int? CurrencyId { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyCode { get; set; }
        public int? Parfumacia { get; set; }
        public string AdditionalInformation { get; set; }
        public string InstructionsForUse { get; set; }

        public int? Top { get; set; }
        public bool Novinka { get; set; }
        public bool Inovace { get; set; }
        public bool Doprodej { get; set; }
        public bool Vyprodano { get; set; }
        public bool ProdejUkoncen { get; set; }
        public int? MaximalniPocetVBaleni { get; set; }
        public int? MinimalniPocetVBaleni { get; set; }
        public decimal? BonusovyKredit { get; set; }

        public bool Megasleva { get; set; }
        public bool Supercena { get; set; }
        public bool CLHit { get; set; }
        public bool Action { get; set; }
        public bool Vyprodej { get; set; }
        public bool OnWeb { get; set; }
        public int InternalStorageCount { get; set; }
        public DateTime? LimitDate { get; set; }

        /// <summary>
        /// Cena produktu s DPH 
        /// </summary>
        public new decimal Price { get; set; }
        public decimal BeznaCena { get; set; }
        public bool? DynamickaSleva { get; set; }
        public decimal? StatickaSleva { get; set; }

        public int? VamiNejviceNakupovane { get; set; }
        public int? DarkovySet { get; set; }
        /// <summary>
        /// Cena produktu bez zlavy s DPH 
        /// </summary>
        public new decimal PriceWVAT {
            get { return this.Price; }
        }

        /// <summary>
        /// Cena celkom zo zlavou s DPH 
        /// </summary>
        public new decimal PriceTotal {
            get {
                if (this.DiscountTypeId == Product.DiscountType.Percent)
                    return this.Price - (this.Price * (this.Discount / 100m));
                else if (this.DiscountTypeId == Product.DiscountType.Price)
                    return this.Price - this.Discount;
                else throw new NotSupportedException(string.Format("Discount type Id = {0} is not supported!", this.DiscountTypeId));
            }
        }
        /// <summary>
        /// Cena celkom zo zlavou s DPH 
        /// </summary>
        public new decimal PriceTotalWVAT {
            get { return this.PriceTotal; }
        }

        /// <summary>
        /// zda je povoleno odečítání marže z ceny produktu
        /// </summary>
        public bool MarzePovolena { get; set; }
        /// <summary>
        /// v případě povoleného odečítání marže a nároku na marži 25% nebo 30% se uplatní jen 20% (momentálně Eurona nemá u žádného produktu nastaveno)
        /// </summary>
        public bool MarzePovolenaMinimalni { get; set; }
        //FK

        private List<VlastnostiProduktu> vlp = null;
        public List<VlastnostiProduktu> VlastnostiProduktu {
            get {
                if (this.vlp != null) return vlp;
                this.vlp = Storage<VlastnostiProduktu>.Read(new VlastnostiProduktu.ReadByProduct { ProductId = this.Id });
                return this.vlp;
            }
        }

        private List<PiktogramyProduktu> pgp = null;
        public List<PiktogramyProduktu> PiktogramyProduktu {
            get {
                if (this.pgp != null) return pgp;
                this.pgp = Storage<PiktogramyProduktu>.Read(new PiktogramyProduktu.ReadByProduct { ProductId = this.Id });
                return this.pgp;
            }
        }

        private List<UcinkyProduktu> up = null;
        public List<UcinkyProduktu> UcinkyProduktu {
            get {
                if (this.up != null) return up;
                this.up = Storage<UcinkyProduktu>.Read(new UcinkyProduktu.ReadByProduct { ProductId = this.Id });
                return this.up;
            }
        }

        private List<CenyProduktu> cp = null;
        public List<CenyProduktu> CenyProduktu {
            get {
                if (this.cp != null) return cp;
                this.cp = Storage<CenyProduktu>.Read(new CenyProduktu.ReadByProduct { ProductId = this.Id });
                return this.cp;
            }
        }

        public string ZadniEtiketa{ get; set; }
        public bool ZobrazovatZadniEtiketu{ get; set; }
    }
}
