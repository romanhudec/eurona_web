using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace EuronaImportFromTVD.Diagnostics {
    using AppTrace = System.Diagnostics.Trace;

    public sealed class TraceCategory {
        public const string Information = "INFO";
        public const string Error = "CHYBA";
        public const string Warning = "UPOZORNENIE";
    }

    public sealed class Trace {
        //Synchronizacny objekt pre viacvlaknove spracovanie
        private static object syncRoot = new object();

        private Trace() {
        }

        private static string GetDateTimeCategory(string category) {
            DateTime now = DateTime.Now;

            return string.Format("{0:00}.{1:00}.{2:0000} {3:00}:{4:00}:{5:00} {6,-6} ",
                    now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Second, category);
        }

        public static void SetFileListener(string filePath) {
            StreamWriter stream = new StreamWriter(filePath, true, Encoding.UTF8);
            stream.AutoFlush = true;
            AppTrace.Listeners.Add(new TextWriterTraceListener(stream));
        }

        public static bool AutoFlush {
            get { return AppTrace.AutoFlush; }
            set { AppTrace.AutoFlush = value; }
        }

        public static int IndentSize {
            get { return AppTrace.IndentSize; }
            set { AppTrace.IndentSize = value; }
        }

        [Conditional("TRACE")]
        public static void WriteLine(System.Exception ex) {
            lock (syncRoot) {
                WriteLine("!!! Neodchytená výnimka !!!", ex, TraceCategory.Error);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLine(System.Exception ex, string category) {
            lock (syncRoot) {
                WriteLine(null, ex, category);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLine(string message, System.Exception ex) {
            lock (syncRoot) {
                WriteLine(message, ex, TraceCategory.Error);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLine(string message, System.Exception ex, string category) {
            lock (syncRoot) {
                StringBuilder sb = new StringBuilder();
                sb.Append(message);

                if (ex != null) {
                    sb.Append("\n{\n");//{
                    AppTrace.Indent();
                    sb.Append(ex.Message);

                    if (ex.InnerException != null) {
                        sb.AppendLine("Inner Exprtion:");
                        sb.AppendLine(ex.InnerException.Message);
                    }

                    sb.Append("\n\t!!StackTrace!!\n\t{\n");//{
                    sb.Append(ex.StackTrace);
                    sb.Append("\n\t}\n");//{

                    AppTrace.Unindent();
                    sb.Append("\n}\n");//{
                }

                WriteLine(sb.ToString(), category);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLineIf(bool condition, System.Exception ex) {
            lock (syncRoot) {
                if (condition)
                    WriteLine(null, ex, TraceCategory.Error);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLineIf(bool condition, System.Exception ex, string category) {
            lock (syncRoot) {
                if (condition)
                    WriteLine(null, ex, category);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLineIf(bool condition, string message, System.Exception ex) {
            lock (syncRoot) {
                if (condition)
                    WriteLine(message, ex, TraceCategory.Error);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLineIf(bool condition, string message, System.Exception ex, string category) {
            lock (syncRoot) {
                if (condition)
                    WriteLine(message, ex, category);
            }
        }

        #region System.Diagnostics.Trace class supplemental methods

        [Conditional("TRACE")]
        public static void Indent() {
            lock (syncRoot) {
                AppTrace.Indent();
            }
        }

        [Conditional("TRACE")]
        public static void Unindent() {
            lock (syncRoot) {
                AppTrace.Unindent();
            }
        }

        [Conditional("TRACE")]
        public static void Write(object value) {
            lock (syncRoot) {
                AppTrace.Write(value);
            }
        }

        [Conditional("TRACE")]
        public static void Write(string message) {
            lock (syncRoot) {
                AppTrace.Write(message);
            }
        }

        [Conditional("TRACE")]
        public static void Write(object value, string message) {
            lock (syncRoot) {
                AppTrace.Write(value, message);
            }
        }

        [Conditional("TRACE")]
        public static void Write(string message, string category) {
            lock (syncRoot) {
                AppTrace.Write(message, GetDateTimeCategory(category));
            }
        }

        [Conditional("TRACE")]
        public static void WriteIf(bool condition, object value) {
            lock (syncRoot) {
                AppTrace.WriteIf(condition, value);
            }
        }

        [Conditional("TRACE")]
        public static void WriteIf(bool condition, string message) {
            lock (syncRoot) {
                AppTrace.WriteIf(condition, message);
            }
        }

        [Conditional("TRACE")]
        public static void WriteIf(bool condition, object value, string message) {
            lock (syncRoot) {
                AppTrace.WriteIf(condition, value, message);
            }
        }

        [Conditional("TRACE")]
        public static void WriteIf(bool condition, string message, string category) {
            lock (syncRoot) {
                AppTrace.WriteIf(condition, message, GetDateTimeCategory(category));
            }
        }

        [Conditional("TRACE")]
        public static void WriteLine(object value) {
            lock (syncRoot) {
                AppTrace.WriteLine(value);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLine(string message) {
            lock (syncRoot) {
                AppTrace.WriteLine(message);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLine(object value, string message) {
            lock (syncRoot) {
                AppTrace.WriteLine(value, message);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLine(string message, string category) {
            lock (syncRoot) {
                AppTrace.WriteLine(message, GetDateTimeCategory(category));
            }
        }

        [Conditional("TRACE")]
        public static void WriteLineIf(bool condition, object value) {
            lock (syncRoot) {
                AppTrace.WriteLineIf(condition, value);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLineIf(bool condition, string message) {
            lock (syncRoot) {
                AppTrace.WriteLineIf(condition, message);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLineIf(bool condition, object value, string message) {
            lock (syncRoot) {
                AppTrace.WriteLineIf(condition, value, message);
            }
        }

        [Conditional("TRACE")]
        public static void WriteLineIf(bool condition, string message, string category) {
            lock (syncRoot) {
                AppTrace.WriteLineIf(condition, message, GetDateTimeCategory(category));
            }
        }

        #endregion
    }
}
