using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace Eurona.pay.gp {
    public class CertificateHelper {
        public static X509Certificate2 ToCertificate(string thumbprint, StoreName name = StoreName.My, StoreLocation loacation = StoreLocation.LocalMachine) {
            X509Store store = new X509Store(name, loacation);
            store.Open(OpenFlags.ReadOnly);
            try {
                var cert = store.Certificates.OfType<X509Certificate2>()
                    .FirstOrDefault(c => c.Thumbprint.Equals(thumbprint, StringComparison.OrdinalIgnoreCase));
                return cert != null ? new X509Certificate2(cert) : null;
            } finally {
                store.Certificates.OfType<X509Certificate2>().ToList().ForEach(c => c.Reset());
                store.Close();
            }
        }
    }
}