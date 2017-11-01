using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cron.Eurona.Import
{
		class CronCommandIntensa
		{
//        internal void ImportIntensaProduct( Dictionary<string, string> parameters )
//        {
//            int instanceId = 2;
//            //Check na zastavenia spracovania
//            if ( base.BreakPending() )
//                return;

//            string srcTVDImagePath = parameters["srcTVDImagePath"].ToString();

//            string dstProductImagePath = parameters["dstProductImagePath"].ToString();
//            string dstVlastnostiImagePath = parameters["dstVlastnostiImagePath"].ToString();
//            string dstPiktogramyImagePath = parameters["dstPiktogramyImagePath"].ToString();
//            string dstParfumacieImagePath = parameters["dstParfumacieImagePath"].ToString();
//            string dstSpecialniUcinkyImagePath = parameters["dstSpecialniUcinkyImagePath"].ToString();

//            CMS.Pump.MSSQLStorage mssqStorageSrc = new CMS.Pump.MSSQLStorage( parameters["connectionStringSrc"] );
//            CMS.Pump.MSSQLStorage mssqStorageDst = new CMS.Pump.MSSQLStorage( parameters["connectionStringDst"] );

//            #region Synchronizacia obrazkou ciselnikou
//            #region Log operation info
//            if ( TraceGeneral.TraceInfo ) base.WriteLogLine( CommandLogSeparator, TraceCategory.Information );
//#if _OFFLINE_DEBUG
//            base.WriteLogLine( "OFFLINE DEBUG MODE !!!", TraceCategory.Warning );
//#endif
//            if ( TraceGeneral.TraceInfo ) base.WriteLogLine( string.Format( "Synchronize Intensa images for classifiers... [Parfemace, SpecialniUcinky]" ), TraceCategory.Information );
//            #endregion
//            int itemsTotal = 0;
//            int itemsImported = 0;
//            int itemsError = 0;
//            string errors = string.Empty;
//            Import.EuronaClsImagesSynchronize eis = new Import.EuronaClsImagesSynchronize( srcTVDImagePath, dstParfumacieImagePath, dstSpecialniUcinkyImagePath, mssqStorageSrc );
//            eis.Info += ( message ) =>
//            {
//                if ( TraceGeneral.TraceInfo ) base.WriteLogLine( message, TraceCategory.Information );
//            };
//            eis.ItemProccessed += ( item, totalItems, message ) =>
//            {
//                itemsTotal++;
//                itemsImported++;
//                if ( TraceGeneral.TraceInfo ) base.WriteLogLine( message, TraceCategory.Information );
//            };
//            eis.Error += ( message, e ) =>
//            {
//                itemsTotal++;
//                itemsError++;
//                if ( errors.Length != 0 ) errors += "\n";
//                errors += message;

//                if ( TraceGeneral.TraceWarning ) base.WriteLogLine( message, e );
//            };

//            lock ( SyncObject_Product )
//            {
//                eis.Synchronize();
//            }
//            #endregion

//            #region Synchronizacia kategorii
//            DataTable dtCategories = Cron.Eurona.Import.TVDDAL.GetTVDCategories( mssqStorageSrc );
//            foreach ( DataRow drCategory in dtCategories.Rows )
//            {
//                int categoryId = Convert.ToInt32( drCategory["Kategorie_Id"] );
//                int? parentId = Convert.ToInt32( drCategory["Kategorie_Parent"] );
//                int shop = Convert.ToInt32( drCategory["Shop"] );
//                if ( !parentId.HasValue || parentId.Value == 0 ) parentId = null;
//                //Kategorie eurony
//                if ( shop != 1 ) continue;

//                #region Log operation info
//                if ( TraceGeneral.TraceInfo ) base.WriteLogLine( CommandLogSeparator, TraceCategory.Information );
//#if _OFFLINE_DEBUG
//                base.WriteLogLine( "OFFLINE DEBUG MODE !!!", TraceCategory.Warning );
//#endif
//                if ( TraceGeneral.TraceInfo ) base.WriteLogLine( string.Format( "Synchronize Intensa categories... {0}", categoryId ), TraceCategory.Information );
//                #endregion

//                itemsTotal = 0;
//                itemsImported = 0;
//                itemsError = 0;
//                errors = string.Empty;

//                Import.EuronaCategorySynchronize tcs = new Import.EuronaCategorySynchronize( instanceId, categoryId, parentId, mssqStorageSrc, mssqStorageDst );
//                tcs.Info += ( message ) =>
//                {
//                    if ( TraceGeneral.TraceInfo ) base.WriteLogLine( message, TraceCategory.Information );
//                };
//                tcs.ItemProccessed += ( item, totalItems, message ) =>
//                {
//                    itemsTotal++;
//                    itemsImported++;
//                    if ( TraceGeneral.TraceInfo ) base.WriteLogLine( message, TraceCategory.Information );
//                };
//                tcs.Error += ( message, e ) =>
//                {
//                    itemsTotal++;
//                    itemsError++;
//                    if ( errors.Length != 0 ) errors += "\n";
//                    errors += message;

//                    if ( TraceGeneral.TraceWarning ) base.WriteLogLine( message, e );
//                };

//                lock ( SyncObject_Product )
//                {
//                    tcs.Synchronize();
//                }
//            }
//            #endregion

//            #region Synchronizacia Produktov
//            DataTable dt = Cron.Eurona.Import.TVDDAL.GetTVDProducts( mssqStorageSrc, instanceId );
//            foreach ( DataRow row in dt.Rows )
//            {
//                int productId = Convert.ToInt32( row["C_Zbo"] );

//                #region Log operation info
//                if ( TraceGeneral.TraceInfo ) base.WriteLogLine( CommandLogSeparator, TraceCategory.Information );
//#if _OFFLINE_DEBUG
//                base.WriteLogLine( "OFFLINE DEBUG MODE !!!", TraceCategory.Warning );
//                //subject.Email = "roman.hudec@rhudec.sk";
//#endif
//                if ( TraceGeneral.TraceInfo ) base.WriteLogLine( string.Format( "Synchronize Intensa product... {0}", productId ), TraceCategory.Information );
//                #endregion

//                itemsTotal = 0;
//                itemsImported = 0;
//                itemsError = 0;
//                errors = string.Empty;

//                Import.EuronaProductSynchronize tcs = new Import.EuronaProductSynchronize(
//                    instanceId, productId,
//                    srcTVDImagePath,
//                    dstProductImagePath,
//                    dstVlastnostiImagePath,
//                    dstPiktogramyImagePath,
//                    mssqStorageSrc, mssqStorageDst );
//                tcs.Info += ( message ) =>
//                {
//                    if ( TraceGeneral.TraceInfo ) base.WriteLogLine( message, TraceCategory.Information );
//                };
//                tcs.ItemProccessed += ( item, totalItems, message ) =>
//                {
//                    itemsTotal++;
//                    itemsImported++;
//                    if ( TraceGeneral.TraceInfo ) base.WriteLogLine( message, TraceCategory.Information );
//                };
//                tcs.Error += ( message, e ) =>
//                {
//                    itemsTotal++;
//                    itemsError++;
//                    if ( errors.Length != 0 ) errors += "\n";
//                    errors += message;

//                    if ( TraceGeneral.TraceWarning ) base.WriteLogLine( message, e );
//                };

//                lock ( SyncObject_Product )
//                {
//                    tcs.Synchronize();
//                }
//            }
//            #endregion

//            #region Synchronizacia Alternativnych Produktov
//            foreach ( DataRow row in dt.Rows )
//            {
//                int productId = Convert.ToInt32( row["C_Zbo"] );

//                #region Log operation info
//                if ( TraceGeneral.TraceInfo ) base.WriteLogLine( CommandLogSeparator, TraceCategory.Information );
//#if _OFFLINE_DEBUG
//                base.WriteLogLine( "OFFLINE DEBUG MODE !!!", TraceCategory.Warning );
//                //subject.Email = "roman.hudec@rhudec.sk";
//#endif
//                if ( TraceGeneral.TraceInfo ) base.WriteLogLine( string.Format( "Synchronize Intensa alternate products for product... {0}", productId ), TraceCategory.Information );
//                #endregion

//                #region Alternativni produkty
//                using ( SqlConnection connection = mssqStorageDst.Connect() )
//                {
//                    //Odstranenie zvyraznenia produktu
//                    DataTable dtProduktyAlternativni = Cron.Eurona.Import.TVDDAL.GetTVDProductAlternativni( mssqStorageSrc, productId );
//                    Cron.Eurona.Import.EuronaDAL.Product.RemoveProductRelations( mssqStorageDst, instanceId, productId );
//                    foreach ( DataRow rowP in dtProduktyAlternativni.Rows )
//                    {
//                        int altProductId = Convert.ToInt32( rowP["Alternativni_C_Zbo"] );
//                        Cron.Eurona.Import.EuronaDAL.Product.SyncProductAlternativni( mssqStorageDst, instanceId, productId, altProductId );
//                    }
//                }
//                #endregion
//            }
//            #endregion
//        }

//        internal void ExportIntensaAccount( Dictionary<string, string> parameters )
//        {
//            int instanceId = 2;
//            //Check na zastavenia spracovania
//            if ( base.BreakPending() )
//                return;

//            CMS.Pump.MSSQLStorage mssqStorageSrc = new CMS.Pump.MSSQLStorage( parameters["connectionStringSrc"] );
//            CMS.Pump.MSSQLStorage mssqStorageDst = new CMS.Pump.MSSQLStorage( parameters["connectionStringDst"] );

//            #region Log operation info
//            if ( TraceGeneral.TraceInfo ) base.WriteLogLine( CommandLogSeparator, TraceCategory.Information );
//#if _OFFLINE_DEBUG
//            base.WriteLogLine( "OFFLINE DEBUG MODE !!!", TraceCategory.Warning );
//#endif
//            if ( TraceGeneral.TraceInfo ) base.WriteLogLine( string.Format( "Export Intensa accounts" ), TraceCategory.Information );
//            #endregion
//            int itemsTotal = 0;
//            int itemsImported = 0;
//            int itemsError = 0;
//            string errors = string.Empty;
//            Import.IntensaAccoutsExport ias = new Import.IntensaAccoutsExport( instanceId, mssqStorageSrc, mssqStorageDst );
//            ias.Info += ( message ) =>
//            {
//                if ( TraceGeneral.TraceInfo ) base.WriteLogLine( message, TraceCategory.Information );
//            };
//            ias.ItemProccessed += ( item, totalItems, message ) =>
//            {
//                itemsTotal++;
//                itemsImported++;
//                if ( TraceGeneral.TraceInfo ) base.WriteLogLine( message, TraceCategory.Information );
//            };
//            ias.Error += ( message, e ) =>
//            {
//                itemsTotal++;
//                itemsError++;
//                if ( errors.Length != 0 ) errors += "\n";
//                errors += message;

//                if ( TraceGeneral.TraceWarning ) base.WriteLogLine( message, e );
//            };

//            lock ( SyncObject_Account )
//            {
//                ias.Synchronize();
//            }
//        }

//        /// <summary>
//        /// Synchronizacia objednavok Intensa <-- TVD
//        /// </summary>
//        internal void SyncIntensaOrder( Dictionary<string, string> parameters )
//        {
//            int instanceId = 2;
//            //Check na zastavenia spracovania
//            if ( base.BreakPending() )
//                return;

//            CMS.Pump.MSSQLStorage mssqStorageSrc = new CMS.Pump.MSSQLStorage( parameters["connectionStringSrc"] );
//            CMS.Pump.MSSQLStorage mssqStorageDst = new CMS.Pump.MSSQLStorage( parameters["connectionStringDst"] );

//            #region Log operation info
//            if ( TraceGeneral.TraceInfo ) base.WriteLogLine( CommandLogSeparator, TraceCategory.Information );
//#if _OFFLINE_DEBUG
//            base.WriteLogLine( "OFFLINE DEBUG MODE !!!", TraceCategory.Warning );
//#endif
//            if ( TraceGeneral.TraceInfo ) base.WriteLogLine( string.Format( "Synchronize Intensa orders" ), TraceCategory.Information );
//            #endregion
//            int itemsTotal = 0;
//            int itemsImported = 0;
//            int itemsError = 0;
//            string errors = string.Empty;
//            Import.IntensaOrdersSynchronize iis = new Import.IntensaOrdersSynchronize( instanceId, mssqStorageSrc, mssqStorageDst );
//            iis.Info += ( message ) =>
//            {
//                if ( TraceGeneral.TraceInfo ) base.WriteLogLine( message, TraceCategory.Information );
//            };
//            iis.ItemProccessed += ( item, totalItems, message ) =>
//            {
//                itemsTotal++;
//                itemsImported++;
//                if ( TraceGeneral.TraceInfo ) base.WriteLogLine( message, TraceCategory.Information );
//            };
//            iis.Error += ( message, e ) =>
//            {
//                itemsTotal++;
//                itemsError++;
//                if ( errors.Length != 0 ) errors += "\n";
//                errors += message;

//                if ( TraceGeneral.TraceWarning ) base.WriteLogLine( message, e );
//            };

//            lock ( SyncObject_Account )
//            {
//                iis.Synchronize();
//            }

//        }

//        internal void ExportIntensaOrders( Dictionary<string, string> parameters )
//        {
//            int instanceId = 2;
//            //Check na zastavenia spracovania
//            if ( base.BreakPending() )
//                return;

//            CMS.Pump.MSSQLStorage mssqStorageSrc = new CMS.Pump.MSSQLStorage( parameters["connectionStringSrc"] );
//            CMS.Pump.MSSQLStorage mssqStorageDst = new CMS.Pump.MSSQLStorage( parameters["connectionStringDst"] );

//            #region Log operation info
//            if ( TraceGeneral.TraceInfo ) base.WriteLogLine( CommandLogSeparator, TraceCategory.Information );
//#if _OFFLINE_DEBUG
//            base.WriteLogLine( "OFFLINE DEBUG MODE !!!", TraceCategory.Warning );
//#endif
//            if ( TraceGeneral.TraceInfo ) base.WriteLogLine( string.Format( "Export Intensa orders" ), TraceCategory.Information );
//            #endregion
//            int itemsTotal = 0;
//            int itemsImported = 0;
//            int itemsError = 0;
//            string errors = string.Empty;
//            Import.IntensaOrdersExport ias = new Import.IntensaOrdersExport( instanceId, mssqStorageSrc, mssqStorageDst );
//            ias.Info += ( message ) =>
//            {
//                if ( TraceGeneral.TraceInfo ) base.WriteLogLine( message, TraceCategory.Information );
//            };
//            ias.ItemProccessed += ( item, totalItems, message ) =>
//            {
//                itemsTotal++;
//                itemsImported++;
//                if ( TraceGeneral.TraceInfo ) base.WriteLogLine( message, TraceCategory.Information );
//            };
//            ias.Error += ( message, e ) =>
//            {
//                itemsTotal++;
//                itemsError++;
//                if ( errors.Length != 0 ) errors += "\n";
//                errors += message;

//                if ( TraceGeneral.TraceWarning ) base.WriteLogLine( message, e );
//            };

//            lock ( SyncObject_Account )
//            {
//                ias.Synchronize();
//            }
//        }
		}
}
