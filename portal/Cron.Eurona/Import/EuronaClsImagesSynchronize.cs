using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Pump;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Drawing;

namespace Cron.Eurona.Import
{
    /// <summary>
    /// Synchronizacia obrazkou ciselnikou z TVD
    /// </summary>
    public class EuronaClsImagesSynchronize : Synchronize
    {
        private string srcTVDImagePath = null;
        private string dstParfumacieImagePath = null;
        private string dstSpecialniUcinkyImagePath = null;

        public EuronaClsImagesSynchronize(string srcTVDImagePath, string dstParfumacieImagePath, string dstSpecialniUcinkyImagePath, MSSQLStorage srcSqlStorage)
            : base(srcSqlStorage, null)
        {

            this.srcTVDImagePath = srcTVDImagePath;
            this.dstParfumacieImagePath = dstParfumacieImagePath;
            this.dstSpecialniUcinkyImagePath = dstSpecialniUcinkyImagePath;
        }

        public override void Synchronize()
        {
            int addedItems = 0;
            int updatedItems = 0;
            int errorItems = 0;
            int ignoredItems = 0;

            int rowsCount = 0;

            try
            {
                int rowIndex = 0;
                try
                {
                    string sql = string.Empty;
                    DataTable td = EuronaTVDDAL.GetTVDClsSpecialniUcinky(this.SourceDataStorage);
                    foreach (DataRow row in td.Rows)
                    {
                        string kod = GetString(row["Spec_Ucinek_Kod"]);
                        string imageName = GetString(row["Obrazek"].ToString());

                        ImportClsSpecialneUcinkyImage(imageName, kod);
                        OnItemProccessed(rowIndex, rowsCount, string.Format("Proccessing image '{0}' : ok", imageName.Length != 0 ? imageName : kod));
                    }

                    td = EuronaTVDDAL.GetTVDClsParfemace(this.SourceDataStorage);
                    foreach (DataRow row in td.Rows)
                    {
                        string kod = GetString(row["Parfemace_Id"]);
                        string imageName = GetString(row["Obrazek"]);

                        ImportClsParfumacieImage(imageName, kod);
                        OnItemProccessed(rowIndex, rowsCount, string.Format("Proccessing image '{0}' : ok", imageName.Length != 0 ? imageName : kod));
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = "Proccessing image : failed!";
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
                }
                finally
                {
                    rowIndex++;
                }

            }
            finally
            {
                OnFinish(rowsCount, addedItems, updatedItems, errorItems, ignoredItems);
            }
        }

        private void ImportClsSpecialneUcinkyImage(string srcFileName, string kod)
        {
            kod = kod.Trim().ToLower();
            kod = kod.Replace(" ", "");
            if (string.IsNullOrEmpty(srcFileName)) srcFileName = string.Format("{0}.jpg", kod);

            srcFileName = Path.GetFileName(srcFileName);
            string srcDirectory = Path.Combine(this.srcTVDImagePath, "SpecialniUcinky");
            string srcFilePath = Path.Combine(srcDirectory, srcFileName);

            string dstFilePath = Path.Combine(this.dstSpecialniUcinkyImagePath, string.Format("{0}.jpg", kod));

            if (!File.Exists(srcFilePath)) return;
            File.Copy(srcFilePath, dstFilePath, true);

        }

        private void ImportClsParfumacieImage(string srcFileName, string kod)
        {

            kod = kod.Trim().ToLower();
            kod = kod.Replace(" ", "");
            if (string.IsNullOrEmpty(srcFileName)) srcFileName = string.Format("{0}.jpg", kod);

            srcFileName = Path.GetFileName(srcFileName);
            string srcDirectory = Path.Combine(this.srcTVDImagePath, "Parfemace");
            string srcFilePath = Path.Combine(srcDirectory, srcFileName);

            string dstFilePath = Path.Combine(this.dstParfumacieImagePath, string.Format("{0}.jpg", kod));

            if (!File.Exists(srcFilePath)) return;
            File.Copy(srcFilePath, dstFilePath, true);

            ////Write Thumbnail photo
            //string dstFilePath_t = Path.Combine( this.dstParfumacieImagePath, string.Format( "{0}_t.jpg", kod ) );
            //const int MAX_IMAGE_WIDTH = 400;
            //const int MAX_IMAGE_HEIGHT = 400;
            //Bitmap b = (Bitmap)Image.FromFile( srcFilePath );
            //int width = MAX_IMAGE_WIDTH;
            //int height = MAX_IMAGE_HEIGHT;
            //Imaging.RecalculateImageSize( b.Width, b.Height, MAX_IMAGE_WIDTH, MAX_IMAGE_HEIGHT, ref width, ref height );
            //Imaging.ResizeImage( dstFilePath, dstFilePath_t, width, height, true );
        }

        private string GetString(object obj)
        {
            if (obj == null) return null;
            return obj.ToString().Trim();
        }

    }
}
