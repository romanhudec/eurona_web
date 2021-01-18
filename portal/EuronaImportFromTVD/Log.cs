using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace IISLog2SQL {
    public static class Log {

        public static string SerializeObject<T>(T toSerialize) {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());
            using (StringWriter textWriter = new StringWriter()) {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }

        public static string writeLogFile(string path, string fileName, string data) {
            if (path != null && path.Length != 0) {
                string dstPath = Path.Combine(path, fileName);
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(dstPath, false, Encoding.UTF8)) {
                    file.Write(data);
                }
                return dstPath;
            }
            return null;
        }
    }
}
