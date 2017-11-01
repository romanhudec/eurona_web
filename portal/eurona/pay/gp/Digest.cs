using System;
using System.Data;
using System.Configuration;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;


namespace Eurona.pay.gp {
    public class Digest {

        /// <summary>
        /// Ze zpravy vytvori podpis pomoci certifikatu a vysledek zakoduje do Base64.
        /// (Funkce pro vytvoreni Digestu pro podpis pozadavku pro GPE)
        /// </summary>
        /// <param name="message">Podepisovaná zpráva</param>
        /// <param name="privateCertificateFile">Cesta ke keystore ve formátu pfx</param>
        /// <param name="password">Heslo k keystore</param>
        /// <returns>Podpis zakódovaný do Base64</returns>
        public static string SignData(string message, string privateCertificateFile, string password) {
            byte[] msgData = System.Text.Encoding.GetEncoding(1250).GetBytes(message);

            X509Certificate2 cert = new X509Certificate2(privateCertificateFile, password, X509KeyStorageFlags.Exportable);

            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] hashResult = sha.ComputeHash(msgData);

            RSAPKCS1SignatureFormatter RSAFormatter = new RSAPKCS1SignatureFormatter(cert.PrivateKey);
            RSAFormatter.SetHashAlgorithm("SHA1");
            byte[] signedHash = RSAFormatter.CreateSignature(hashResult);

            return Convert.ToBase64String(signedHash);
        }

        public static string SignData(string message, X509Certificate2 cert) {
            byte[] msgData = System.Text.Encoding.GetEncoding(1250).GetBytes(message);

            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] hashResult = sha.ComputeHash(msgData);

            RSAPKCS1SignatureFormatter RSAFormatter = new RSAPKCS1SignatureFormatter(cert.PrivateKey);
            RSAFormatter.SetHashAlgorithm("SHA1");
            byte[] signedHash = RSAFormatter.CreateSignature(hashResult);

            return Convert.ToBase64String(signedHash);

            
        }

        /// <summary>Zkontroluje, zda zaslana odpoved od GPE je prava</summary>
        /// <param name="digest">Podpis zprávy</param>
        /// <param name="message">Zpráva</param>
        /// <param name="publicCertificateFile"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool ValidateDigest(string digest, string message, string publicCertificateFile, string password) {
            bool ret = false;
            try {
                byte[] bDigest = Convert.FromBase64String(digest);

                X509Certificate2 cert = new X509Certificate2(publicCertificateFile, password, X509KeyStorageFlags.Exportable);

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa = (RSACryptoServiceProvider)cert.PublicKey.Key;

                byte[] data = new byte[message.Length];
                data = System.Text.Encoding.GetEncoding(1250).GetBytes(message);

                byte[] hashResult;
                SHA1 sha = new SHA1CryptoServiceProvider();
                hashResult = sha.ComputeHash(data);

                RSAPKCS1SignatureDeformatter rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                rsaDeformatter.SetHashAlgorithm("SHA1");

                if (rsaDeformatter.VerifySignature(hashResult, bDigest)) {
                    ret = true;
                }
            } catch (Exception ex) {
                //throw ex;
            }
            return ret;
        }

        /// <summary>Zkontroluje, zda zaslana odpoved od GPE je prava</summary>
        /// <param name="digest">Podpis zprávy</param>
        /// <param name="message">Zpráva</param>
        /// <param name="publicCertificateFile"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool ValidateDigest(string digest, string message, X509Certificate2 cert) {
            bool ret = false;
            try {
                byte[] bDigest = Convert.FromBase64String(digest);

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa = (RSACryptoServiceProvider)cert.PublicKey.Key;

                byte[] data = new byte[message.Length];
                data = System.Text.Encoding.GetEncoding(1250).GetBytes(message);

                byte[] hashResult;
                SHA1 sha = new SHA1CryptoServiceProvider();
                hashResult = sha.ComputeHash(data);

                RSAPKCS1SignatureDeformatter rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                rsaDeformatter.SetHashAlgorithm("SHA1");

                if (rsaDeformatter.VerifySignature(hashResult, bDigest)) {
                    ret = true;
                }
            } catch (Exception ex) {
                //throw ex;
            }
            return ret;
        }
    }
}
