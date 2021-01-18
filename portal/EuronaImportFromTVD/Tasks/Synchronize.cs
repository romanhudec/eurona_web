using System;
using CMS.Pump;

namespace EuronaImportFromTVD.Tasks {
    /// <summary>
    /// Summary description for Synchronize.
    /// </summary>
    public abstract class Synchronize : CMS.Pump.DataSynchronize {
        public Synchronize(MSSQLStorage srcSqlStorage, MSSQLStorage dstSqlStorage)
                : base(srcSqlStorage, dstSqlStorage) {
        }


        public new MSSQLStorage SourceDataStorage {
            get { return (MSSQLStorage)base.SourceDataStorage; }
        }

        public new MSSQLStorage DestinationDataStorage {
            get { return (MSSQLStorage)base.DestinationDataStorage; }
        }


        protected object Null(object obj) {
            return Null(obj, DBNull.Value);
        }

        protected object Null(bool condition, object obj) {
            return Null(condition, obj, DBNull.Value);
        }

        protected object Null(object obj, object def) {
            return Null(obj != null, obj, def);
        }

        protected object Null(bool condition, object obj, object def) {
            return condition ? obj : def;
        }

    }
}
