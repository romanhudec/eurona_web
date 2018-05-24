using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.User.Advisor
{
    public partial class ReklamniZasilkyPage : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Security.IsLogged(true);
            if (!Security.Account.TVD_Id.HasValue) return;
            LoadData(!IsPostBack);
           
        }

        private void LoadData(bool bind)
        {
            DateTime beforDate = DateTime.Now.AddMonths(-1);
            List<DAL.Entities.ReklamniZasilky> reklamniZasilky = Storage<DAL.Entities.ReklamniZasilky>.Read();
            this.rpReklamniZasilky.DataSource = reklamniZasilky;
            if (bind) this.rpReklamniZasilky.DataBind();
        }

        protected void OnItemDataBound(object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CheckBox cbx = (CheckBox)e.Item.FindControl("cbSouhlas");
                if (cbx != null)
                {
                    if (string.IsNullOrEmpty(cbx.Attributes["CommandArgument"])) return;

                    int idZasilky = Convert.ToInt32(cbx.Attributes["CommandArgument"]);
                    int? idOdberatele = Security.Account.TVD_Id;
                    cbx.Checked = false;
                    DAL.Entities.ReklamniZasilkySouhlas reklamniZasilkaSouhlas = Storage<DAL.Entities.ReklamniZasilkySouhlas>.ReadFirst(new DAL.Entities.ReklamniZasilkySouhlas.ReadByOdberatel { Id_zasilky = idZasilky, Id_odberatele = idOdberatele .Value});
                    if (reklamniZasilkaSouhlas != null)
                        cbx.Checked = reklamniZasilkaSouhlas.Souhlas;
                }
            }
        }

        protected void OnSouhlasCheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbx = (sender as CheckBox);
            if (string.IsNullOrEmpty(cbx.Attributes["CommandArgument"])) return;
            int idZasilky = Convert.ToInt32(cbx.Attributes["CommandArgument"]);
            int? idOdberatele = Security.Account.TVD_Id;

            DAL.Entities.ReklamniZasilkySouhlas reklamniZasilkaSouhlas = Storage<DAL.Entities.ReklamniZasilkySouhlas>.ReadFirst(new DAL.Entities.ReklamniZasilkySouhlas.ReadByOdberatel { Id_zasilky = idZasilky, Id_odberatele = idOdberatele.Value });
            if (reklamniZasilkaSouhlas == null)
            {
                reklamniZasilkaSouhlas = new DAL.Entities.ReklamniZasilkySouhlas();
                reklamniZasilkaSouhlas.Id_odberatele = idOdberatele.Value;
                reklamniZasilkaSouhlas.Id_zasilky = idZasilky;
                reklamniZasilkaSouhlas.Souhlas = cbx.Checked;
                Storage<DAL.Entities.ReklamniZasilkySouhlas>.Create(reklamniZasilkaSouhlas);
            }
            else
            {
                reklamniZasilkaSouhlas.Souhlas = cbx.Checked;
                Storage<DAL.Entities.ReklamniZasilkySouhlas>.Update(reklamniZasilkaSouhlas);
            }
            
            //LoadData(true);
        }
    }
}
