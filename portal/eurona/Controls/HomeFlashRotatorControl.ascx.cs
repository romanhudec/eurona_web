using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;

namespace Eurona.Controls
{
    public partial class HomeFlashRotatorControl : System.Web.UI.UserControl
    {
        //private List<Banner> bannerList = null;
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        /*
        public List<Banner> Baners
        {
            get
            {
                if (this.bannerList != null) return this.bannerList;

                this.bannerList = new List<Banner>();
                string bannerPath = Server.MapPath("~/userfiles/banner/");
                if (Directory.Exists(bannerPath))
                {
                    string[] files = Directory.GetFiles(bannerPath);
                    foreach (string file in files)
                    {
                        string fileName = Path.GetFileName(file);
                        string productCode = Path.GetFileNameWithoutExtension(file);

                        Banner banner = new Banner();
                        banner.FilePath = file;
                        banner.ImageUrl = Page.ResolveUrl(string.Format("~/userfiles/banner/{0}", fileName));
                        Eurona.Common.DAL.Entities.Product product = Storage<Eurona.Common.DAL.Entities.Product>.ReadFirst(new Eurona.Common.DAL.Entities.Product.ReadByCode { Code = productCode });
                        if (product != null) banner.NavigateUrl = Page.ResolveUrl(product.Alias) + "?ReturnUrl=/";
                        this.bannerList.Add(banner);
                    }
                }
                return this.bannerList;

            }
        }
        
        protected string RenderFlash()
        {
            //Rotator databinding
            StringBuilder sbDiv = new StringBuilder();
            int index = 0;
            foreach (Banner banner in this.Baners)
            {
                sbDiv.AppendFormat("<div class='pane pane{0}'><img alt='{2}' src='{1}'/><a href=''></a></div>", index, banner.ImageUrl, "pic_" + index);
                index++;
            }
            return sbDiv.ToString();
        }
        protected string RenderButtons()
        {
            //Rotator databinding
            StringBuilder sbTd = new StringBuilder();
            int index = 0;
            foreach (Banner banner in this.Baners)
            {
                sbTd.AppendFormat("<td align='center' class='dojoxRotatorNumber dojoxRotatorPane{0} dojoxRotatorFirst' dojoattrid='{0}'><a href='#'><span>{1}</span></a></td>", index, index + 1);
                index++;
            }
            return sbTd.ToString();
        }
        public class Banner
        {
            public string FilePath { get; set; }
            public string ImageUrl { get; set; }
            public string NavigateUrl { get; set; }
        }
         * */
    }
}