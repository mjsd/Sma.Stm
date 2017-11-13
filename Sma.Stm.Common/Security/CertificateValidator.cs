using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Linq;

namespace Sma.Stm.Common.Security
{
    public class CertificateValidator
    {
        public static bool IsCertificateValid(X509Certificate2 cert, string rootCertificateTumbprint, out string errors)
        {
            errors = string.Empty;

            X509Chain chain = new X509Chain();
            chain.ChainPolicy = new X509ChainPolicy()
            {
                RevocationFlag = X509RevocationFlag.EndCertificateOnly,
                RevocationMode = X509RevocationMode.Online,
                UrlRetrievalTimeout = new TimeSpan(0, 0, 10),
                VerificationTime = DateTime.UtcNow
            };

            var chainBuilt = chain.Build(cert);

            if (chainBuilt)
            {
                if (chain.ChainElements
                    .Cast<X509ChainElement>()
                    .Any(x => x.Certificate.Thumbprint.ToUpper().Trim().CompareTo(rootCertificateTumbprint.ToUpper()) == 0))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                foreach (X509ChainStatus chainStatus in chain.ChainStatus)
                {
                    errors += string.Format("Chain error: {0} {1}", chainStatus.Status, chainStatus.StatusInformation);
                }

                return false;
            }
        }
    }
}
