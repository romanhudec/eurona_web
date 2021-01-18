using System;
using System.ComponentModel;
using System.Threading;

namespace EuronaImportFromTVD {
    public enum ProgressEventType {
        /// <summary>
        /// Background worker is entered. This is a internal infrastructure progress event type
        /// and cannot be used directly.
        /// </summary>
        Enter,

        Start,
        Step,
        Stop,
        Message,

        /// <summary>
        /// Background worker is suspended. This is a internal infrastructure progress event type
        /// and cannot be used directly.
        /// </summary>
        Suspeding,

        /// <summary>
        /// Background worker is suspended. This is a internal infrastructure progress event type
        /// and cannot be used directly.
        /// </summary>
        Suspeded,

        /// <summary>
        /// Background worker is suspended. This is a internal infrastructure progress event type
        /// and cannot be used directly.
        /// </summary>
        Resumed,

        /// <summary>
        /// 
        /// </summary>
        Canceling,

        /// <summary>
        /// Background worker is suspended. This is a internal infrastructure progress event type
        /// and cannot be used directly.
        /// </summary>
        Leave
    }

    public class ProgressEventArgs : EventArgs {
        private ProgressEventType type;
        private int min;
        private int max;
        private int value;
        private string message;
        private object tag;

        internal ProgressEventArgs() {
        }

        internal ProgressEventArgs(ProgressEventType type, int min, int max, int value, object tag, string message) {
            this.type = type;
            this.min = min;
            this.max = max;
            this.value = value;
            this.tag = tag;
            this.message = message;
        }

        public ProgressEventType Type {
            get { return this.type; }
        }

        public int Min {
            get { return this.min; }
        }

        public int Max {
            get { return this.max; }
        }

        public int Value {
            get { return this.value; }
        }

        public string Message {
            get { return this.message; }
        }

        public object Tag {
            get { return this.tag; }
        }
    }

    public delegate void ProgressEventHandler(object sender, ProgressEventArgs args);

    public class RunWorkerCompletedEventArgs : EventArgs {
        private bool canceled = false;
        private System.Exception error = null;
        private object result = null;

        private RunWorkerCompletedEventArgs() {
        }

        internal RunWorkerCompletedEventArgs(bool canceled, System.Exception error, object result) {
            this.canceled = canceled;
            this.error = error;
            this.result = result;
        }

        public bool Canceled {
            get { return this.canceled; }
        }

        public System.Exception Error {
            get { return this.error; }
        }

        public object Result {
            get { return this.result; }
        }

    }

    public delegate void RunWorkerCompletedEventHandler(object sender, RunWorkerCompletedEventArgs args);


    /// <summary>
    /// Summary description for BGWorker.
    /// </summary>
    public class WorkerBase : IDisposable {
        private object syncRoot = new object();

        private ProgressEventHandler eventProgressChanged = null;
        private RunWorkerCompletedEventHandler eventCompletted = null;

        private ManualResetEvent seCancel = null;
        private ManualResetEvent seSuspend = null;
        private ManualResetEvent seResume = null;

        private Thread thread = null;
        private volatile bool canceled = false;
        private volatile bool suspended = false;
        private volatile bool completted = false;

        public WorkerBase() {
            this.seCancel = new ManualResetEvent(false);
            this.seSuspend = new ManualResetEvent(false);
            this.seResume = new ManualResetEvent(false);
        }

        public virtual void Dispose() {
            if (this.seCancel != null) {
                this.seCancel.Close();
                this.seCancel = null;
            }

            if (this.seSuspend != null) {
                this.seSuspend.Close();
                this.seSuspend = null;
            }

            if (this.seResume != null) {
                this.seResume.Close();
                this.seResume = null;
            }

            if (this.eventCompletted != null) {
                this.eventCompletted = MulticastDelegate.RemoveAll(this.eventCompletted, this.eventCompletted) as RunWorkerCompletedEventHandler;
                this.eventCompletted = null;
            }

            if (this.eventProgressChanged != null) {
                this.eventProgressChanged = MulticastDelegate.RemoveAll(this.eventProgressChanged, this.eventProgressChanged) as ProgressEventHandler;
                this.eventProgressChanged = null;
            }
        }

        public void Run() {
            try {
                Enter();
                DoWork();

                if (!this.completted)
                    OnCompletted();
            }
            catch (System.Exception ex) {
                this.canceled = true;
                OnCompletted(null, ex);
            }

            try {
                Leave();
            }
            catch (System.Exception ex) {
                this.canceled = true;
                OnCompletted(null, ex);
            }
            finally {
                lock (this.syncRoot) {
                    this.thread = null;
                }
            }
        }

        public void RunAsync() {
            lock (this.syncRoot) {
                if (this.thread != null)
                    throw new InvalidOperationException();

                this.thread = new Thread(new ThreadStart(Run));
                this.thread.Start();
            }
        }

        private void Enter() {
            lock (this.syncRoot) {
                this.seSuspend.Reset();
                this.seCancel.Reset();
            }

            OnProgressChanged(ProgressEventType.Enter);
            OnEnter();
        }

        /// <summary>
        /// MetÛda je volan· na zaûiatku spracovania, eöte pred volanÌm metÛdy DoWork(). Je urËen·
        /// pre poËiatoËn˙ inicializ·ciu a prÌpravu prostredia pre samotnÈ volanie metÛdy DoWork().
        /// T·to metÛda nesmie byù volan· priamo !!!
        /// </summary>
        protected virtual void OnEnter() {
        }

        private void Leave() {
            if (!this.completted)
                OnCompletted();

            try {
                // Ak v OnLeave() vznikne chyba, v˝nimka sa zaloguje a kÛd beûÌ Ôalej.
                // OnProgressChanged( ProgressEventType.Leave ) sa musÌ zavolaù vûdy !!!
                OnLeave();
            }
            catch (System.Exception ex) {
                this.canceled = true;
                OnCompletted(null, ex);
            }

            OnProgressChanged(ProgressEventType.Leave);
        }

        /// <summary>
        /// MetÛda je volan· na konci spracovania. Pokiaæ spracovanie beûÌ asynchrÛnne, je volan· ako
        /// posledn· metÛda pred ukonËenÌm vl·kna v kontexte ktorÈho beûÌ. Je urËen· pre z·vereËnÈ 
        /// upratovanie. T·to metÛda nesmie byù volan· priamo !!! Na uvolnenie systÈmov˝ch zdrojov je 
        /// urËen· metÛda Dispose() !!!  Metoda OnLeave nesmie hadzat vynimky a vdzy musi prejst !!!
        /// </summary>
        protected virtual void OnLeave() {
        }

        public bool IsBusy {
            get {
                Thread thx = null;

                lock (this.syncRoot) {
                    thx = this.thread;
                }

                return thx != null && thx.IsAlive;
            }
        }

        public void Join() {
            Thread thx = null;

            lock (this.syncRoot) {
                thx = this.thread;
            }

            if (thx != null)
                thx.Join();
        }

        public void Cancel() {
            lock (this.syncRoot) {
                if (this.thread == null)
                    throw new InvalidOperationException();

                if (this.canceled)
                    return;

                OnProgressChanged(ProgressEventType.Canceling);
                this.seCancel.Set();

                if (this.Suspended)
                    Resume();
            }
        }

        public bool Canceled {
            get {
                lock (this.syncRoot) {
                    return this.canceled;
                }
            }
        }

        public bool Canceling {
            get {
                lock (this.syncRoot) {
                    return !this.canceled && this.seCancel.WaitOne(1, false);
                }
            }
        }

        public void Suspend() {
            lock (this.syncRoot) {
                if (this.thread == null)
                    throw new InvalidOperationException();

                if (this.suspended)
                    return;

                OnProgressChanged(ProgressEventType.Suspeding);
                this.seSuspend.Set();
                this.seResume.Reset();
            }
        }

        public bool Suspended {
            get {
                lock (this.syncRoot) {
                    return this.suspended;
                }
            }
        }

        public bool Suspending {
            get {
                lock (this.syncRoot) {
                    return this.seSuspend.WaitOne(1, false) && !this.suspended;
                }
            }
        }

        public void Resume() {
            lock (this.syncRoot) {
                this.seResume.Set();
            }
        }

        protected bool CanContinue() {
            lock (this.syncRoot) {
                if (this.Canceling)
                    return false;

                if (!this.Suspending)
                    return true;

                this.suspended = true;
                this.seResume.Reset();

                OnProgressChanged(ProgressEventType.Suspeded);
            }

            this.seResume.WaitOne();

            lock (this.syncRoot) {
                this.suspended = false;
                this.seSuspend.Reset();
                this.seResume.Reset();

                OnProgressChanged(ProgressEventType.Resumed);
            }

            return !this.Canceling;
        }

        #region Public events

        public event ProgressEventHandler ProgressChanged {
            add {
                lock (this.syncRoot) {
                    this.eventProgressChanged = MulticastDelegate.Combine(this.eventProgressChanged, value) as ProgressEventHandler;
                }
            }
            remove {
                lock (this.syncRoot) {
                    this.eventProgressChanged = MulticastDelegate.Remove(this.eventProgressChanged, value) as ProgressEventHandler;
                }
            }
        }

        public event RunWorkerCompletedEventHandler Completed {
            add {
                lock (this.syncRoot) {
                    this.eventCompletted = MulticastDelegate.Combine(this.eventCompletted, value) as RunWorkerCompletedEventHandler;
                }
            }
            remove {
                lock (this.syncRoot) {
                    this.eventCompletted = MulticastDelegate.Remove(this.eventCompletted, value) as RunWorkerCompletedEventHandler;
                }
            }
        }

        #endregion

        private void OnProgressChanged(ProgressEventType type) {
            OnProgressChanged(type, 0, 0, 0, null, null);
        }

        private void OnProgressChanged(ProgressEventType type, int min, int max, int value, object tag, string message) {
            lock (this.syncRoot) {
                if (this.eventProgressChanged != null) {
                    ProgressEventArgs e = new ProgressEventArgs(type, min, max, value, tag, message);

                    foreach (ProgressEventHandler eventHandler in this.eventProgressChanged.GetInvocationList()) {
                        ISynchronizeInvoke si = eventHandler.Target as ISynchronizeInvoke;

                        if (si != null && si.InvokeRequired)
                            si.BeginInvoke(eventHandler, new object[] { this, e });
                        else
                            eventHandler(this, e);
                    }
                }
            }
        }

        #region OnProgressMessage( ... ) methods

        protected void OnProgressMessage(string message) {
            OnProgressChanged(ProgressEventType.Message, 0, 0, 0, null, message);
        }

        protected void OnProgressMessage(string format, params object[] args) {
            OnProgressChanged(ProgressEventType.Message, 0, 0, 0, null, String.Format(format, args));
        }

        #endregion

        #region OnProgressStart( ... ) methods

        protected void OnProgressStart(int min, int max) {
            OnProgressChanged(ProgressEventType.Start, min, max, 0, null, null);
        }

        protected void OnProgressStart(int min, int max, string messageFormat, params object[] messageArgs) {
            OnProgressChanged(ProgressEventType.Start, min, max, 0, null, String.Format(messageFormat, messageArgs));
        }

        #endregion

        #region OnProgressStep( ... ) methods

        protected void OnProgressStep(int min, int max, int value) {
            OnProgressChanged(ProgressEventType.Step, min, max, value, null, null);
        }

        protected void OnProgressStep(int min, int max, int value, object tag) {
            OnProgressChanged(ProgressEventType.Step, min, max, value, tag, null);
        }

        protected void OnProgressStep(int min, int max, int value, string messageFormat, params object[] messageArgs) {
            OnProgressChanged(ProgressEventType.Step, min, max, value, null, String.Format(messageFormat, messageArgs));
        }

        protected void OnProgressStep(int min, int max, int value, object tag, string messageFormat, params object[] messageArgs) {
            OnProgressChanged(ProgressEventType.Step, min, max, value, tag, String.Format(messageFormat, messageArgs));
        }

        #endregion

        #region OnProgressStop( ... ) methods

        protected void OnProgressStop() {
            OnProgressChanged(ProgressEventType.Stop, 0, 0, 0, null, null);
        }

        protected void OnProgressStop(string messageFormat, params object[] messageArgs) {
            OnProgressChanged(ProgressEventType.Stop, 0, 0, 0, null, String.Format(messageFormat, messageArgs));
        }

        protected void OnProgressStop(int min, int max) {
            OnProgressChanged(ProgressEventType.Stop, min, max, 0, null, null);
        }

        protected void OnProgressStop(int min, int max, int value) {
            OnProgressChanged(ProgressEventType.Stop, min, max, value, null, null);
        }

        protected void OnProgressStop(int min, int max, int value, object tag) {
            OnProgressChanged(ProgressEventType.Stop, min, max, value, tag, null);
        }

        protected void OnProgressStop(int min, int max, int value, string messageFormat, params object[] messageArgs) {
            OnProgressChanged(ProgressEventType.Stop, min, max, value, null, String.Format(messageFormat, messageArgs));
        }

        protected void OnProgressStop(int min, int max, int value, object tag, string messageFormat, params object[] messageArgs) {
            OnProgressChanged(ProgressEventType.Stop, min, max, value, tag, String.Format(messageFormat, messageArgs));
        }

        #endregion

        #region OnCompletted methods

        protected void OnCompletted() {
            OnCompletted(null, null);
        }

        protected void OnCompletted(object result) {
            OnCompletted(result, null);
        }

        protected void OnCompletted(System.Exception error) {
            OnCompletted(null, error);
        }

        protected void OnCompletted(object result, System.Exception error) {
            lock (this.syncRoot) {
                if (this.seCancel.WaitOne(1, false))
                    this.canceled = true;
            }

            if (this.eventCompletted != null) {
                RunWorkerCompletedEventArgs e = new RunWorkerCompletedEventArgs(this.canceled, error, result);

                foreach (RunWorkerCompletedEventHandler eventHandler in this.eventCompletted.GetInvocationList()) {
                    ISynchronizeInvoke si = eventCompletted.Target as ISynchronizeInvoke;

                    if (si != null && si.InvokeRequired)
                        si.BeginInvoke(eventHandler, new object[] { this, e });
                    else
                        eventHandler(this, e);
                }
            }

            this.completted = true;
        }

        #endregion

        protected virtual void DoWork() {
        }
    }
}
