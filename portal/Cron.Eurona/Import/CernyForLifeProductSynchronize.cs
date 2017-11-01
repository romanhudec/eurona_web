using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Pump;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Net;
using System.Drawing;

namespace Cron.Eurona.Import {
    public class CernyForLifeProductSynchronize : Synchronize {
        private string srcTVDImagePath = null;
        private string dstProductImagePath = null;
        private string dstVlastnostiImagePath = null;
        private string dstPiktogramyImagePath = null;
        private string dstZadniEtiketyImagePath = null;

        private int instanceId = 0;
        private int productId = 0;

        public CernyForLifeProductSynchronize(int instanceId, int productId, string srcTVDImagePath, string dstProductImagePath, string dstVlastnostiImagePath, string dstPiktogramyImagePath, string dstZadniEtiketyImagePath,
                MSSQLStorage srcSqlStorage, MSSQLStorage dstSqlStorage)
            : base(srcSqlStorage, dstSqlStorage) {
            this.productId = productId;
            this.instanceId = instanceId;
            this.srcTVDImagePath = srcTVDImagePath;
            this.dstProductImagePath = dstProductImagePath;
            this.dstVlastnostiImagePath = dstVlastnostiImagePath;
            this.dstPiktogramyImagePath = dstPiktogramyImagePath;
            this.dstZadniEtiketyImagePath = dstZadniEtiketyImagePath;
        }

        public int ProductId { get { return this.productId; } }
        public int InstanceId { get { return this.instanceId; } }

        public override void Synchronize() {
            int addedItems = 0;
            int updatedItems = 0;
            int errorItems = 0;
            int ignoredItems = 0;

            using (SqlConnection connection = this.DestinationDataStorage.Connect()) {
                int rowsCount = 0;

                try {
                    int rowIndex = 0;
                    try {
                        string sql = string.Empty;

                        bool isProductValid = CernyForLifeTVDDAL.IsTVDProductLocalized(this.SourceDataStorage, this.ProductId);
                        if (!isProductValid) {
                            OnItemProccessed(rowIndex, rowsCount, string.Format("Proccessing product '{0}' : Not Valid Product - product has no localization", this.ProductId));
                            return;
                        }

                        DataRow drProduct = CernyForLifeTVDDAL.GetTVDProduct(this.SourceDataStorage, this.ProductId);
                        DataTable dtKategorie = CernyForLifeTVDDAL.GetTVDProductKategorie(this.SourceDataStorage, this.ProductId, this.InstanceId);

                        int productInstance = 0;//Urcuje do akej instancie sa naimportuje dany produkt 
                        bool euronaProduct = Convert.ToBoolean(drProduct["Eurona_Produkt"]);
                        bool cernyForLifeProduct = Convert.ToBoolean(drProduct["CL_Produkt"]);
                        productInstance = this.InstanceId;
                        //if ( !euronaProduct || !intensaProduct ) productInstance = this.InstanceId;
                        //Produkt sa importuje bud pre konkretnu instanciu alebo pre obe

                        int top = Convert.ToInt32(drProduct["Top_Produkt"]);
                        bool novinka = GetBool(drProduct["Novinka"]);
                        bool doprodej = GetBool(drProduct["Doprodej"]);
                        bool inovace = GetBool(drProduct["Inovace"]);
                        bool prodejUkoncen = GetBool(drProduct["Prodej_Ukoncen"]);
                        bool vyprodano = GetBool(drProduct["Vyprodano"]);

                        decimal dispozice_HR = Convert.ToDecimal(drProduct["Dispozice_HR"]);

                        //[Megasleva], [Supercena], [CLHit], [Action], [Vyprodej],
                        bool megasleva = GetBool(drProduct["Megasleva"]);
                        bool supercena = GetBool(drProduct["Supercena"]);
                        bool clHit = GetBool(drProduct["CLHit"]);
                        bool action = GetBool(drProduct["Action"]);
                        bool vyprodej = GetBool(drProduct["Vyprodej"]);
                        bool onWeb = GetBool(drProduct["On_web"]);

                        string zadniEtiketa = GetString(drProduct["Zadni_etiketa"]);
                        bool zobrazovat_zadni_etiketu = GetBool(drProduct["Zobrazovat_zadni_etiketu"]);

                        EuronaDAL.Product.SyncProduct(this.DestinationDataStorage, this.ProductId, productInstance, GetString(drProduct["Kod"]), ConvertNullable.ToDecimal(drProduct["Vat"]), ConvertNullable.ToInt32(drProduct["Bod_hodnota"]), ConvertNullable.ToInt32(drProduct["Parfumacia"]),
                                top, novinka, inovace, doprodej, vyprodano, prodejUkoncen, (int)dispozice_HR,
                                megasleva, supercena, clHit, action, vyprodej, onWeb, zadniEtiketa, zobrazovat_zadni_etiketu);
                        foreach (int jazyk in CernyForLifeTVDDAL.TVDJazyky) {
                            DataRow drLocalize = CernyForLifeTVDDAL.GetTVDProductLocalize(this.SourceDataStorage, this.ProductId, jazyk);
                            DataTable dtVlastnosti = CernyForLifeTVDDAL.GetTVDProductVlastnosti(this.SourceDataStorage, this.ProductId, jazyk);
                            DataTable dtPiktogramy = CernyForLifeTVDDAL.GetTVDProductPiktogramy(this.SourceDataStorage, this.ProductId, jazyk);
                            DataTable dtUcinky = CernyForLifeTVDDAL.GetTVDProductUcinky(this.SourceDataStorage, this.ProductId, jazyk);
                            DataRow drCeny = CernyForLifeTVDDAL.GetTVDProductCeny(this.SourceDataStorage, this.ProductId, jazyk);

                            string locale = CernyForLifeTVDDAL.GetLocale(jazyk);

                            if (drLocalize != null)
                                EuronaDAL.Product.SyncProductLocale(this.DestinationDataStorage, this.ProductId, productInstance, locale,
                                        GetString(drProduct["Kod"]),
                                        GetString(drLocalize["Name"]),
                                        GetString(drLocalize["Description"]),
                                        GetString(drLocalize["DescriptionLong"]),
                                        GetString(drLocalize["InstructionsForUse"]),
                                        GetString(drLocalize["AdditionalInformation"]),
                                        CernyForLifeTVDDAL.IsLivienneProduct(this.SourceDataStorage, this.ProductId));


                            #region ProductCeny
                            EuronaDAL.Product.SyncProductCenyCL(this.DestinationDataStorage, this.ProductId, locale,
                                    jazyk,
                                    (drCeny != null ? Convert.ToInt32(drCeny["Body"] == DBNull.Value ? 0m : drCeny["Body"]) : 0),
                                    (drCeny != null ? ConvertNullable.ToDecimal(drCeny["Cena"] == DBNull.Value ? 0m : drCeny["Cena"]) : 0m),
                                    (drCeny != null ? ConvertNullable.ToDecimal(drCeny["Bezna_cena"] == DBNull.Value ? 0m : drCeny["Bezna_cena"]) : 0m),
                                    (drCeny != null ? ConvertNullable.ToBool(drCeny["Dynamicka_sleva"] == DBNull.Value ? 0m : drCeny["Dynamicka_sleva"]) : false),
                                    (drCeny != null ? ConvertNullable.ToDecimal(drCeny["Staticka_sleva"] == DBNull.Value ? 0m : drCeny["Staticka_sleva"]) : 0m),
                                    (drCeny != null ? ConvertNullable.ToDecimal(drCeny["Cena_BK"] == DBNull.Value ? 0m : drCeny["Cena_BK"]) : 0m));
                            #endregion

                            #region Vlastnosti produktu
                            if (dtVlastnosti != null) {
                                EuronaDAL.Product.RemoveVlastnostiProduktu(this.DestinationDataStorage, this.ProductId, locale);
                                foreach (DataRow drVlastnosti in dtVlastnosti.Rows) {
                                    string imageName = GetString(drVlastnosti["ImageUrl"]);
                                    ImportVlastnostiImage(imageName);
                                    EuronaDAL.Product.SyncProductVlastnosti(this.DestinationDataStorage, this.ProductId, locale,
                                            GetString(drVlastnosti["Name"]), GetVlastnostiImageName(imageName));
                                }
                            }
                            #endregion

                            #region Piktogramy produktu
                            if (dtPiktogramy != null) {
                                EuronaDAL.Product.RemovePiktogramyProduktu(this.DestinationDataStorage, this.ProductId, locale);
                                foreach (DataRow drPiktogramy in dtPiktogramy.Rows) {
                                    string imageName = GetString(drPiktogramy["ImageUrl"]);
                                    ImportPiktogramyImage(imageName);
                                    EuronaDAL.Product.SyncProductPiktogramy(this.DestinationDataStorage, this.ProductId, locale,
                                            GetString(drPiktogramy["Name"]), GetPiktogramyImageName(imageName));
                                }
                            }
                            #endregion

                            #region Ucinky produktu
                            if (dtUcinky != null) {
                                EuronaDAL.Product.RemoveUcinkyProduktu(this.DestinationDataStorage, this.ProductId, locale);
                                foreach (DataRow drUcinky in dtUcinky.Rows) {
                                    EuronaDAL.Product.SyncProductUcinky(this.DestinationDataStorage, this.ProductId, locale,
                                            GetString(drUcinky["Name"]), GetUcinkyImageName(drUcinky["Spec_Ucinek_Kod"]));
                                }
                            }
                            #endregion

                            #region ZadniEtikety
                            if (!string.IsNullOrEmpty(zadniEtiketa)) {
                                string imageZadniEtiketaName = GetString(zadniEtiketa);
                                ImportZadniEtiketyImage(imageZadniEtiketaName);
                            }
                            #endregion

                        }


                        #region Instance zavisle entity

                        #region Import images
                        List<String> images = new List<string>();
                        if (drProduct["Obrazek"].ToString() != null && GetString(drProduct["Obrazek"]) != string.Empty) images.Add(drProduct["Obrazek"].ToString().Trim());
                        if (drProduct["Obrazek2"].ToString() != null && GetString(drProduct["Obrazek2"]) != string.Empty) images.Add(drProduct["Obrazek2"].ToString().Trim());
                        if (drProduct["Obrazek3"].ToString() != null && GetString(drProduct["Obrazek3"]) != string.Empty) images.Add(drProduct["Obrazek3"].ToString().Trim());
                        if (drProduct["Obrazek4"].ToString() != null && GetString(drProduct["Obrazek4"]) != string.Empty) images.Add(drProduct["Obrazek4"].ToString().Trim());
                        if (drProduct["Obrazek5"].ToString() != null && GetString(drProduct["Obrazek5"]) != string.Empty) images.Add(drProduct["Obrazek5"].ToString().Trim());
                        if (drProduct["Obrazek6"].ToString() != null && GetString(drProduct["Obrazek6"]) != string.Empty) images.Add(drProduct["Obrazek6"].ToString().Trim());
                        if (drProduct["Obrazek7"].ToString() != null && GetString(drProduct["Obrazek7"]) != string.Empty) images.Add(drProduct["Obrazek7"].ToString().Trim());
                        if (drProduct["Obrazek8"].ToString() != null && GetString(drProduct["Obrazek8"]) != string.Empty) images.Add(drProduct["Obrazek8"].ToString().Trim());
                        if (drProduct["Obrazek9"].ToString() != null && GetString(drProduct["Obrazek9"]) != string.Empty) images.Add(drProduct["Obrazek9"].ToString().Trim());
                        ImportProductImages(this.ProductId, GetString(drProduct["Kod"]).ToString(), images);
                        #endregion

                        #region Product Categories
                        //Odstranenie zvyraznenia produktu
                        EuronaDAL.RemoveProductCategories(this.DestinationDataStorage, this.InstanceId, this.ProductId);

                        foreach (DataRow row in dtKategorie.Rows) {
                            int categoryId = Convert.ToInt32(row["kategorie_id"]);
                            EuronaDAL.InsertProductCategory(this.DestinationDataStorage, this.InstanceId, categoryId, this.ProductId);
                        }
                        #endregion

                        #region Product Highlight
                        ////Odstranenie zvyraznenia produktu
                        //EuronaDAL.RemoveProductHighlights( this.DestinationDataStorage, this.InstanceId, this.ProductId );
                        //foreach ( int jazyk in TVDDAL.TVDJazyky )
                        //{
                        //    string locale = TVDDAL.GetLocale( jazyk );

                        //    bool novinka = Convert.ToBoolean( drProduct["Novinka"] );
                        //    bool doprodej = Convert.ToBoolean( drProduct["Doprodej"] );
                        //    bool inovace = Convert.ToBoolean( drProduct["Inovace"] );
                        //    bool prodejUkoncen = Convert.ToBoolean( drProduct["Prodej_Ukoncen"] );
                        //    bool vyprodano = Convert.ToBoolean( drProduct["Vyprodano"] );

                        //    if ( novinka ) EuronaDAL.InsertProductHighlight( this.DestinationDataStorage, "news", this.InstanceId, this.ProductId, locale );
                        //}
                        #endregion

                        #endregion
                        OnItemProccessed(rowIndex, rowsCount, string.Format("Proccessing product '{0}' : ok", this.ProductId));

                    } catch (Exception ex) {
                        string errorMessage = string.Format("Proccessing product '{0}' : failed!", this.ProductId);
                        StringBuilder sbMessage = new StringBuilder();
                        sbMessage.Append(errorMessage);
                        sbMessage.AppendLine(ex.Message);
                        if (ex.InnerException != null) sbMessage.AppendLine(ex.InnerException.Message);
                        sbMessage.AppendLine(ex.StackTrace);

                        OnError(errorMessage, ex);
#if !_OFFLINE_DEBUG
                        SendEmail(errorMessage, sbMessage.ToString());
#endif
                        errorItems++;
                    } finally {
                        rowIndex++;
                    }

                } finally {
                    OnFinish(rowsCount, addedItems, updatedItems, errorItems, ignoredItems);
                }
            }
        }

        #region Helpers methods
        private string GetVlastnostiImageName(object imagePath) {
            if (imagePath == null) return null;
            string path = GetString(imagePath);
            if (string.IsNullOrEmpty(path)) return null;

            return Path.GetFileName(path);
        }
        private string GetPiktogramyImageName(object imagePath) {
            if (imagePath == null) return null;
            string path = GetString(imagePath);
            if (string.IsNullOrEmpty(path)) return null;

            return Path.GetFileName(path);
        }

        private string GetUcinkyImageName(object code) {
            if (code == null) return null;
            string name = GetString(code).ToLower();
            if (string.IsNullOrEmpty(name)) return null;

            //Ostranenie medzier;
            name = name.Replace(" ", "");

            return string.Format("{0}.jpg", name);
        }

        private string GetString(object obj) {
            if (obj == null) return null;
            return obj.ToString().Trim();
        }

        private bool GetBool(object obj) {
            if (obj == null) return false;
            if (obj == DBNull.Value) return false;
            return Convert.ToBoolean(obj);
        }
        #endregion

        #region Import photos methods
        /// <summary>
        /// Metóda vymaže existujúci obrázok z file systému.
        /// </summary>
        private void RemoveExistingProductImages(string itemImagesPath) {
            //Delete image
            DirectoryInfo di = new DirectoryInfo(itemImagesPath);
            FileInfo[] fileInfos = di.GetFiles("*.*");
            foreach (FileInfo fileInfo in fileInfos)
                fileInfo.Delete();

            //Delete thumbnail
            di = new DirectoryInfo(Path.Combine(itemImagesPath, "_t\\"));
            fileInfos = di.GetFiles("*.*");
            foreach (FileInfo fileInfo in fileInfos)
                fileInfo.Delete();
        }

        /// <summary>
        /// Metóda Uploadne/Nahradí fotografiu auta pre dané auto.
        /// V pripade, že už sa fotografia na tomto poradi nachádza, nahradí sa.
        /// </summary>
        private void ImportProductImages(int itemId, string productKod, List<string> images) {
            string srcDirectory = Path.Combine(this.srcTVDImagePath, "Produkty");

            productKod = productKod.Trim();
            //const int IMAGE_WIDTH = 160;
            //const int IMAGE_HEIGHT = 200;
            const int IMAGE_WIDTH = 100;
            const int IMAGE_HEIGHT = 75;

            string dstItemImagesPath = Path.Combine(this.dstProductImagePath, itemId.ToString());
            string dstItemImagesThumbnailPath = Path.Combine(dstItemImagesPath, "_t");

            if (!Directory.Exists(dstItemImagesPath))
                Directory.CreateDirectory(dstItemImagesPath);

            if (!Directory.Exists(dstItemImagesThumbnailPath))
                Directory.CreateDirectory(dstItemImagesThumbnailPath);

            //Delete existing product photo on position from file system.
            RemoveExistingProductImages(dstItemImagesPath);

            int imageCode = 0;
            if (images.Count == 0 && productKod != null)
                images.Add(Path.Combine(srcDirectory, productKod + ".jpg"));

            string chars = "abcdefghijklmnop";
            foreach (char ch in chars) {
                string imgPath = Path.Combine(srcDirectory, productKod + ch + ".jpg");
                if (File.Exists(imgPath))
                    images.Add(imgPath);
            }

            foreach (string srcFile in images) {
                string srcFilePath = srcFile.Trim();
                if (string.IsNullOrEmpty(srcFilePath)) continue;

                imageCode++;
                string dstFileName = Path.GetFileNameWithoutExtension(srcFilePath) + string.Format("_{0}.jpg", imageCode);
                string dstFilePath = Path.Combine(dstItemImagesPath, dstFileName);
                string dstFilePathThumbnail = Path.Combine(dstItemImagesThumbnailPath, dstFileName);

                //Ak zdrojovy obrazok neexistuje, pokusim sa najs obrazok v lokalnych obrazkoch
                if (!File.Exists(srcFilePath)) {
                    if (productKod == null) continue;
                    srcFilePath = Path.Combine(srcDirectory, productKod + ".jpg");
                    if (!File.Exists(srcFilePath)) continue;
                }

                #region Ulozenie Obrazkou
                if (!Imaging.SaveJpeg(srcFilePath, dstFilePath, 90)) {
                    if (Path.GetExtension(srcFilePath).ToLower() != ".jpg") {
                        Image img = Image.FromFile(srcFilePath);
                        img.Save(dstFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);

                    } else
                        File.Copy(srcFilePath, dstFilePath, true);
                }

                //Write Thumbnail photo
                Bitmap b = (Bitmap)Image.FromFile(srcFilePath);
                int maxWidth = IMAGE_WIDTH * 2;
                int maxHeight = IMAGE_HEIGHT * 2;
                int width = maxWidth;
                int height = maxHeight;
                Imaging.RecalculateImageSize(b.Width, b.Height, maxWidth, maxHeight, ref width, ref height);
                Imaging.ResizeImage(dstFilePath, dstFilePathThumbnail, width, height);
                #endregion
            }
        }
        private void ImportVlastnostiImage(string srcFileName) {
            if (string.IsNullOrEmpty(srcFileName)) return;
            srcFileName = Path.GetFileName(srcFileName);
            string srcDirectory = Path.Combine(this.srcTVDImagePath, "Vlastnosti");
            string srcFilePath = Path.Combine(srcDirectory, srcFileName);

            string dstFilePath = Path.Combine(this.dstVlastnostiImagePath, srcFileName);

            if (!File.Exists(srcFilePath)) return;
            File.Copy(srcFilePath, dstFilePath, true);
        }
        private void ImportPiktogramyImage(string srcFileName) {
            if (string.IsNullOrEmpty(srcFileName)) return;
            srcFileName = Path.GetFileName(srcFileName);
            string srcDirectory = Path.Combine(this.srcTVDImagePath, "Piktogramy");
            string srcFilePath = Path.Combine(srcDirectory, srcFileName);

            string dstFilePath = Path.Combine(this.dstPiktogramyImagePath, srcFileName);

            if (!File.Exists(srcFilePath)) return;
            File.Copy(srcFilePath, dstFilePath, true);
        }
        private void ImportZadniEtiketyImage(string srcFileName) {
            if (string.IsNullOrEmpty(srcFileName)) return;
            srcFileName = Path.GetFileName(srcFileName);
            string srcDirectory = Path.Combine(this.srcTVDImagePath, "ZadniEtikety");
            string srcFilePath = Path.Combine(srcDirectory, srcFileName);

            string dstFilePath = Path.Combine(this.dstZadniEtiketyImagePath, srcFileName);

            if (!File.Exists(srcFilePath)) return;
            File.Copy(srcFilePath, dstFilePath, true);
        }
        #endregion
    }
}
