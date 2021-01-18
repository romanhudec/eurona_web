using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using EuronaImportFromTVD.Tasks;
using System.Configuration;
using EuronaImportFromTVD.Diagnostics;

namespace EuronaImportFromTVD {
    /// <summary>
    /// Summary description for Worker.
    /// </summary>
    public class Worker : WorkerBase {
        private List<int> odberateleIds;
        private List<int> objednavkyIds;

        public Worker(List<int> odberateleIds, List<int> objednavkyIds) {
            this.odberateleIds = odberateleIds;
            this.objednavkyIds = objednavkyIds;
        }

        private int getCurrentStep(int index, int total) {
            return (int)((index * 100.00F) / total);
        }
        protected override void DoWork() {
            /*
            Trace.WriteLine("================================================================================================================================", TraceCategory.Information);
            string message = String.Format("Konfigurácia");
            Trace.WriteLine(message, TraceCategory.Information);
            OnProgressMessage(message);
            Trace.WriteLine("================================================================================================================================", TraceCategory.Information);
            */

            int accounts = ImportAccounts(this.odberateleIds);
            int orders = ImportOrders(this.objednavkyIds);

            string finishMessage = string.Format("Koniec importu! Celkovo naimportovaných {0} accounts", accounts);
            Trace.WriteLine(finishMessage);
            finishMessage = string.Format("Koniec importu! Celkovo naimportovaných {0} orders", orders);
            Trace.WriteLine(finishMessage);
            OnCompletted(finishMessage);
        }

        private int ImportAccounts(List<int> odberateleIds) {
            int instanceId = 1;
            CMS.Pump.MSSQLStorage mssqStorageSrc = new CMS.Pump.MSSQLStorage(ConfigurationSettings.AppSettings["connectionStringTVD"].ToString());
            CMS.Pump.MSSQLStorage mssqStorageDst = new CMS.Pump.MSSQLStorage(ConfigurationSettings.AppSettings["connectionStringEurona"].ToString());

            Trace.WriteLine(string.Format("Synchronize Eurona accounts"));
            int itemsTotal = 0;
            int itemsImported = 0;
            int itemsError = 0;
            string errors = string.Empty;

            foreach (int idOdberatele in odberateleIds) {
                EuronaAccoutsImport eas = new EuronaAccoutsImport(instanceId, mssqStorageSrc, mssqStorageDst, idOdberatele);
                eas.Info += (message) => {
                    Trace.WriteLine(message);
                };
                eas.ItemProccessed += (item, totalItems, message) => {
                    itemsTotal++;
                    itemsImported++;
                    OnProgressMessage(message);
                    Trace.WriteLine(message);
                };
                eas.Error += (message, e) => {
                    itemsTotal++;
                    itemsError++;
                    if (errors.Length != 0) errors += "\n";
                    errors += message;
                    OnProgressMessage(message);
                    Trace.WriteLine(message, e);
                };
                eas.Synchronize();
            }
            return itemsImported;
        }


        private int ImportOrders(List<int> objednavkyIds) {
            int instanceId = 1;
            CMS.Pump.MSSQLStorage mssqStorageSrc = new CMS.Pump.MSSQLStorage(ConfigurationSettings.AppSettings["connectionStringTVD"].ToString());
            CMS.Pump.MSSQLStorage mssqStorageDst = new CMS.Pump.MSSQLStorage(ConfigurationSettings.AppSettings["connectionStringEurona"].ToString());

            Trace.WriteLine(string.Format("Synchronize Eurona accounts"));
            int itemsTotal = 0;
            int itemsImported = 0;
            int itemsError = 0;
            string errors = string.Empty;

            foreach (int idObjednavky in objednavkyIds) {
                EuronaOrdersImport eas = new EuronaOrdersImport(instanceId, mssqStorageSrc, mssqStorageDst, idObjednavky);
                eas.Info += (message) => {
                    Trace.WriteLine(message);
                };
                eas.ItemProccessed += (item, totalItems, message) => {
                    itemsTotal++;
                    itemsImported++;
                    OnProgressMessage(message);
                    Trace.WriteLine(message);
                };
                eas.Error += (message, e) => {
                    itemsTotal++;
                    itemsError++;
                    if (errors.Length != 0) errors += "\n";
                    errors += message;
                    Trace.WriteLine(message);
                    Trace.WriteLine(message, e);
                };
                eas.Synchronize();
            }
            return itemsImported;
        }

        /// <summary>
        /// Vrati, zostavajuci cas spracovania.
        /// </summary>
        private string GetLeftTimeMessage(DateTime startProgress, int step, int total) {
            TimeSpan ts = DateTime.Now - startProgress;
            double tpr = (double)ts.Ticks / (double)step;
            TimeSpan tsLeft = new TimeSpan((long)(tpr * (total - step)));

            return string.Format("Zostávajúci èas: {0:00}:{1:00}:{2:00}", tsLeft.Hours, tsLeft.Minutes, tsLeft.Seconds);
        }

        private string GetTimestampString(DateTime startDate, DateTime endDate) {
            TimeSpan ts = endDate - startDate;
            return string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
        }
    }
}
