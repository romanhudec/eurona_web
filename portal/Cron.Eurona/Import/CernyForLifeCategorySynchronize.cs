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

namespace Cron.Eurona.Import
{
	public class CernyForLifeCategorySynchronize : Synchronize
	{
		private string dstCategoryImagePath = null;
		private string srcCategoryImagePath = null;
		private int instanceId = 0;
		private int categoryId = 0;
		private int? parentId = null;
		public CernyForLifeCategorySynchronize(int instanceId, int categoryId, int? parentId, MSSQLStorage srcSqlStorage, MSSQLStorage dstSqlStorage)
			: base(srcSqlStorage, dstSqlStorage)
		{
			this.categoryId = categoryId;
			this.parentId = parentId;
			this.instanceId = instanceId;
		}

		public int CategoryId { get { return this.categoryId; } }
		public int? ParentId { get { return this.parentId; } }
		public int InstanceId { get { return this.instanceId; } }

		public override void Synchronize()
		{
			int addedItems = 0;
			int updatedItems = 0;
			int errorItems = 0;
			int ignoredItems = 0;

			using (SqlConnection connection = this.DestinationDataStorage.Connect())
			{
				int rowsCount = 0;

				try
				{
					int rowIndex = 0;
					try
					{
						string sql = string.Empty;

						//Synchronizacia kategorie
						EuronaDAL.Classifiers.SyncCategory(this.DestinationDataStorage, this.InstanceId, this.CategoryId, this.ParentId);

						//Synchronizaci lokalizacie kategorie
						foreach (int jazyk in CernyForLifeTVDDAL.TVDJazyky)
						{
							DataRow drCategory = CernyForLifeTVDDAL.GetTVDCategory(this.SourceDataStorage, this.InstanceId, this.CategoryId, jazyk);
							if (drCategory == null) continue;
							string locale = CernyForLifeTVDDAL.GetLocale(jazyk);
							EuronaDAL.Classifiers.SyncCategoryLocale(this.DestinationDataStorage, 
								this.CategoryId, 
								GetString(drCategory["Nazev"]), 
								GetString(drCategory["NazevParent"]), 
								this.instanceId, 
								locale,
								CernyForLifeTVDDAL.IsLivienneCategory(this.SourceDataStorage, this.CategoryId) 
							);
						}

						OnItemProccessed(rowIndex, rowsCount, string.Format("Proccessing category '{0}' : ok", this.CategoryId));

					}
					catch (Exception ex)
					{
						string errorMessage = string.Format("Proccessing category '{0}' : failed!", this.CategoryId);
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
		}

		#region Helpers methods
		private string GetVlastnostiImageURL(object imagePath)
		{
			if (imagePath == null) return null;
			string path = imagePath.ToString().Trim();
			if (string.IsNullOrEmpty(path)) return null;

			return Path.GetFileName(path);
		}
		private string GetPiktogramyImageURL(object imagePath)
		{
			if (imagePath == null) return null;
			string path = imagePath.ToString().Trim();
			if (string.IsNullOrEmpty(path)) return null;

			return Path.GetFileName(path);
		}
		private string GetUcinkyImageURL(object imagePath)
		{
			if (imagePath == null) return null;
			string path = imagePath.ToString().Trim();
			if (string.IsNullOrEmpty(path)) return null;

			return string.Format("{0}.jpg", path);
		}
		private string GetString(object obj)
		{
			if (obj == null) return null;
			return obj.ToString().Trim();
		}
		#endregion

		#region Import photos methods
		/// <summary>
		/// Metóda vymaže existujúci obrázok z file systému.
		/// </summary>
		private void RemoveExistingCategoryPhotos(string itemImagesPath)
		{
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
		private void ImportPhotos(int itemId, string productKod, List<string> images)
		{
			productKod = productKod.Trim();
			const int IMAGE_WIDTH = 100;
			const int IMAGE_HEIGHT = 75;

			string dstItemImagesPath = Path.Combine(this.dstCategoryImagePath, itemId.ToString());
			string dstItemImagesThumbnailPath = Path.Combine(dstItemImagesPath, "_t");

			if (!Directory.Exists(dstItemImagesPath))
				Directory.CreateDirectory(dstItemImagesPath);

			if (!Directory.Exists(dstItemImagesThumbnailPath))
				Directory.CreateDirectory(dstItemImagesThumbnailPath);

			//Delete existing category photo on position from file system.
			RemoveExistingCategoryPhotos(dstItemImagesPath);

			int imageCode = 0;
			if (images.Count == 0 && productKod != null)
				images.Add(Path.Combine(this.srcCategoryImagePath, productKod + ".jpg"));

			foreach (string srcFile in images)
			{
				string srcFilePath = srcFile.Trim();
				if (string.IsNullOrEmpty(srcFilePath)) continue;

				imageCode++;
				string dstFileName = Path.GetFileNameWithoutExtension(srcFilePath) + ".jpg";
				string dstFilePath = Path.Combine(dstItemImagesPath, dstFileName);
				string dstFilePathThumbnail = Path.Combine(dstItemImagesThumbnailPath, dstFileName);

				//Ak zdrojovy obrazok neexistuje, pokusim sa najs obrazok v lokalnych obrazkoch
				if (!File.Exists(srcFilePath))
				{
					if (productKod == null) continue;
					srcFilePath = Path.Combine(this.srcCategoryImagePath, productKod + ".jpg");
					if (!File.Exists(srcFilePath)) continue;
				}

				#region Ulozenie Obrazkou
				//Zmena pripony ak to nie je JPEG
				if (Path.GetExtension(srcFilePath).ToLower() != ".jpg")
				{
					Image img = Image.FromFile(srcFilePath);
					img.Save(dstFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);

				}
				else File.Copy(srcFilePath, dstFilePath, true);

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
		#endregion
	}
}
