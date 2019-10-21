using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eurona.Common;
using System.Data;
using System.Text;
using System.IO;
using UrlAliasEntity = CMS.Entities.UrlAlias;
using ProductsEntity = Eurona.Common.DAL.Entities.Product;
using CategoryEntity = SHP.Entities.Category;
using System.Xml;
using System.Configuration;
using System.Data.SqlClient;

namespace Eurona {
    /// <summary>
    /// Summary description for getPPCFeed
    /// </summary>
    public class getPPCFeed : IHttpHandler {

        public void ProcessRequest(HttpContext context) {

            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode shopNode = doc.CreateElement("SHOP");
            XmlAttribute shopAttribute = doc.CreateAttribute("xmlns");
            shopAttribute.Value = "http://www.zbozi.cz/ns/offer/1.0";
            shopNode.Attributes.Append(shopAttribute);
            doc.AppendChild(shopNode);

            List<ProductsEntity> list = new List<ProductsEntity>();
            list = Storage<ProductsEntity>.Read();
            string connectionString = ConfigurationManager.ConnectionStrings["TVDConnectionString"].ConnectionString;
            foreach (ProductsEntity product in list) {


                //string sql = @"select Name from tShpProductLocalization where ProductId=@ProductId and Locale='en'";
                string sql = @"select TOP 1 Name=Universal_nazev from produkty where Kod=@Kod";
                DataTable dt = null;
                CMS.Pump.MSSQLStorage storage = new CMS.Pump.MSSQLStorage(connectionString);
                using (SqlConnection connection = storage.Connect()) {
                    dt = storage.Query(connection, sql, new SqlParameter("@Kod", product.Code));
                }
                string nameEN = "";
                if (dt != null && dt.Rows.Count != 0) {
                    nameEN = dt.Rows[0]["Name"].ToString();
                }

                string categoryList = "";
                if (product.ProductCategories.Count != 0) {
                    CategoryEntity category = Storage<CategoryEntity>.ReadFirst(new CategoryEntity.ReadById { CategoryId = product.ProductCategories[0].CategoryId });
                    categoryList = category.Name;
                }

                XmlNode shopItemNode = doc.CreateElement("SHOPITEM");
                shopNode.AppendChild(shopItemNode);

                XmlNode idNode = doc.CreateElement("ITEM_ID");
                idNode.AppendChild(doc.CreateTextNode(product.Id.ToString()));
                shopItemNode.AppendChild(idNode);

                XmlNode nameNode = doc.CreateElement("PRODUCTNAME");
                nameNode.AppendChild(doc.CreateTextNode(product.Name));
                shopItemNode.AppendChild(nameNode);

                XmlNode descriptionNode = doc.CreateElement("DESCRIPTION");
                descriptionNode.AppendChild(doc.CreateTextNode(product.Description));
                shopItemNode.AppendChild(descriptionNode);

                XmlNode categoryNode = doc.CreateElement("CATEGORYTEXT");
                categoryNode.AppendChild(doc.CreateTextNode(categoryList));
                shopItemNode.AppendChild(categoryNode);

                XmlNode extraMessageNode = doc.CreateElement("EXTRA_MESSAGE");
                extraMessageNode.AppendChild(doc.CreateTextNode(nameEN));
                shopItemNode.AppendChild(extraMessageNode);

                XmlNode codeNode = doc.CreateElement("PRODUCTNO");
                codeNode.AppendChild(doc.CreateTextNode(product.Code));
                shopItemNode.AppendChild(codeNode);

                XmlNode manufacturerNode = doc.CreateElement("MANUFACTURER");
                manufacturerNode.AppendChild(doc.CreateTextNode("EURONA"));
                shopItemNode.AppendChild(manufacturerNode);

                XmlNode urlNode = doc.CreateElement("URL");
                UrlAliasEntity alias = Storage<UrlAliasEntity>.ReadFirst(new UrlAliasEntity.ReadById { UrlAliasId = product.UrlAliasId.Value });
                urlNode.AppendChild(doc.CreateTextNode(alias.Url.Replace("~", "https://euronabycerny.com")));
                //urlNode.AppendChild(doc.CreateTextNode(product.Alias.Replace("~", "https://euronabycerny.com")));
                shopItemNode.AppendChild(urlNode);

                string storagePath = string.Format("{0}{1}/", CMS.Utilities.ConfigUtilities.ConfigValue("SHP:ImageGallery:Product:StoragePath"), product.Id.ToString());
                string productImagesPath = context.Server.MapPath(storagePath);
                if (Directory.Exists(productImagesPath)) {
                    DirectoryInfo di = new DirectoryInfo(productImagesPath);
                    FileInfo[] fileInfos = di.GetFiles("*.*");
                    if (fileInfos.Length != 0) {
                        //Sort files by name
                        Comparison<FileInfo> comparison = new Comparison<FileInfo>(delegate(FileInfo a, FileInfo b) {
                            return String.Compare(a.Name, b.Name);
                        });
                        Array.Sort(fileInfos, comparison);

                        string urlThumbnail = storagePath + "_t/" + fileInfos[0].Name;


                        XmlNode urlImgNode = doc.CreateElement("IMGURL");
                        urlImgNode.AppendChild(doc.CreateTextNode(urlThumbnail.Replace("~", "https://euronabycerny.com")));
                        shopItemNode.AppendChild(urlImgNode);
                    }
                }
                XmlNode priceNode = doc.CreateElement("PRICE_VAT");
                priceNode.AppendChild(doc.CreateTextNode(product.PriceWVAT.ToString()));
                shopItemNode.AppendChild(priceNode);

                //doc.Save("D:\Products.xml");
                /*
             <SHOP xmlns="http://www.zbozi.cz/ns/offer/1.0">
             <SHOPITEM>
             <ITEM_ID>62448</ITEM_ID>
             <PRODUCTNAME>Solartent MC234CZ/A premium Beige</PRODUCTNAME>
             <DESCRIPTION>Velmi praktické stínítko s lehkou konstrukcí z laminátových prutů.</DESCRIPTION>
             <CATEGORYTEXT>Dům, byt a zahrada | Zahrada | Stínící technika | Zahradní slunečníky</CATEGORYTEXT>
             <EAN>8594061743744</EAN>
             <PRODUCTNO>MC234CZ/A</PRODUCTNO>
             <MANUFACTURER>Solartent</MANUFACTURER>
             <URL>http://example.com/slunecniky/solartent123</URL>
             <DELIVERY_DATE>0</DELIVERY_DATE>
             <EXTRA_MESSAGE>extended_warranty</EXTRA_MESSAGE>
             <EXTRA_MESSAGE>free_delivery</EXTRA_MESSAGE>
             <PARAM>
             <PARAM_NAME>Barva</PARAM_NAME>
             <VAL>Béžová</VAL>
             </PARAM>
             <IMGURL>http://example.com/obrazky/slunecniky/solartent123.jpg</IMGURL>
             <PRICE_VAT>1290</PRICE_VAT>
             </SHOPITEM>
             <SHOPITEM>
             <!-- popis druhé nabídky... -->
             </SHOPITEM>
             <!-- ... -->
             </SHOP>
                 * */
            }

            string feedUrl = "~/feed/ppcfeed.xml";
            doc.Save(context.Server.MapPath(feedUrl));

            //doc.Save(context.Response.Output); 
            //context.Response.ContentType = "text/xml";
            //context.Response.Write(doc);
            //context.Response.End();

            string host = GetFullRootUrl();
            string feedFullUrl = feedUrl.Replace("~", host);
            context.Response.Write("PPC Feed byl vykenerován <a href='" + feedFullUrl + "'>" + feedFullUrl + "</a>");
            context.Response.End();
        }

        /// <summary>
        /// Get request string from stream
        /// </summary>
        private string GetRequestData(HttpRequest request) {
            Stream stream = request.InputStream;
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);
            stream.Close();
            return request.ContentEncoding.GetString(data);
        }
        public static string GetFullRootUrl() {
            HttpRequest request = HttpContext.Current.Request;
            return request.Url.AbsoluteUri.Replace(request.Url.AbsolutePath, String.Empty);
        }
        public bool IsReusable {
            get {
                return false;
            }
        }
    }
}