using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;

namespace SHP.Entities {
    [Serializable]
    public class Product : Entity, IUrlAliasEntity {
        public enum SortBy : int {
            Default = 0,
            NameASC = 1,
            PriceASC = 2,
            PriceDESC = 3,
            DarkovySet = 4,
            IdDESC = 5,
            IdASC = 6
        }

        public enum DiscountType : int {
            Percent = 0,
            Price = 1
        }

        public Product() {
            this.Alias = string.Empty;
            this.DiscountTypeId = DiscountType.Percent;
            this.Discount = 0m;
        }

        public class ReadById {
            public int ProductId { get; set; }
        }


        /// <summary>
        /// Načíta všetky produkty, ktoré su v danej kategórii a jej podkategóriach.
        /// </summary>
        public class ReadAllInCategory {
            public int CategoryId { get; set; }
            public ReadByFilter ByFilter { get; set; }
        }

        /// <summary>
        /// Načíta produkty, ktoré sú v danej kategórii.
        /// </summary>
        public class ReadByCategory {
            public int CategoryId { get; set; }
        }

        /// <summary>
        /// Načíta produkty podľa filtra.
        /// </summary>
        public class ReadByFilter {
            public bool? BestSellers { get; set; }
            public string Manufacturer { get; set; }
            public string Expression { get; set; }
            public decimal? PriceFrom { get; set; }
            public decimal? PriceTo { get; set; }
            public SortBy SortBy { get; set; }

            public ReadHighlights Highlights { get; set; }
        }

        /// <summary>
        /// Načíta všetky produkty, ktore maju zvýraznenie.
        /// </summary>
        public class ReadHighlights {
            public int? HighlightId { get; set; }
            public int? MaxCount { get; set; }
        }

        public class IncrementViewCountCommand {
            public int ProductId { get; set; }
        }
        public class IncrementVoteCommand {
            public int AccountId { get; set; }
            public int ProductId { get; set; }
            public int Rating { get; set; }
        }

        public int InstanceId { get; set; }
        public string Manufacturer { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DescriptionLong { get; set; }
        public string Availability { get; set; }
        public int? StorageCount { get; set; }
        public int? VATId { get; set; }
        public decimal VAT { get; set; }
        public decimal Discount { get; set; }
        public DiscountType DiscountTypeId { get; set; }


        /// <summary>
        /// Vráti lokaliyovanu hodnotu typu zlavy
        /// </summary>
        public string DiscountTypeText {
            get {
                switch (this.DiscountTypeId) {
                    case DiscountType.Percent:
                        return Resources.Controls.AdminProductControl_DiscountType_Percent;
                    case DiscountType.Price:
                        return Resources.Controls.AdminProductControl_DiscountType_Price;
                    default:
                        return string.Empty;

                }
            }
        }

        /// <summary>
        /// Cena produktu bez zlavy bez DPH 
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Cena produktu bez zlavy s DPH 
        /// </summary>
        public decimal PriceWVAT {

            get {
                decimal p = this.Price + (this.Price * (this.VAT / 100m));
                return Math.Round(p * (decimal)Math.Pow(10, 2)) / (decimal)Math.Pow(10, 2);
            }
        }

        /// <summary>
        /// Cena celkom zo zlavou bez DPH 
        /// </summary>
        public decimal PriceTotal {
            get {
                if (this.Discount == 0m) return this.Price;
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
        public decimal PriceTotalWVAT {
            get {
                if (this.Discount == 0m) return this.PriceWVAT;
                decimal p = this.PriceTotal + (this.PriceTotal * (this.VAT / 100m));
                return Math.Round(p * (decimal)Math.Pow(10, 2)) / (decimal)Math.Pow(10, 2);
            }
        }

        public string Locale { get; set; }

        //FK ProductCategories
        private List<ProductCategories> productCategories = null;
        public List<ProductCategories> ProductCategories {
            get {
                if (productCategories != null) return productCategories;
                productCategories = Storage<ProductCategories>.Read(new ProductCategories.ReadByProductId { ProductId = this.Id });
                return productCategories;
            }
        }

        //Comments
        public int CommentsCount { get; set; }
        //Vote
        public int ViewCount { get; set; }
        public int Votes { get; set; }
        public int TotalRating { get; set; }
        public double RatingResult { get; set; }
        public static AccountVote.ObjectType AccountVoteType { get { return AccountVote.ObjectType.Product; } }

        #region IUrlAliasEntity Members
        public int? UrlAliasId { get; set; }
        public string Alias { get; set; }
        #endregion
    }
}
