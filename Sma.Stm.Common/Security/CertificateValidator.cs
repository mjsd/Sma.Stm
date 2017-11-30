using System;
using System.Security.Cryptography.X509Certificates;
using System.Linq;

namespace Sma.Stm.Common.Security
{
    public static class CertificateValidator
    {
        public static bool IsCertificateValid(X509Certificate2 cert, string rootCertificateTumbprint, out string errors)
        {
            errors = string.Empty;

            var chain = new X509Chain
            {
                ChainPolicy = new X509ChainPolicy()
                {
                    RevocationFlag = X509RevocationFlag.EndCertificateOnly,
                    RevocationMode = X509RevocationMode.NoCheck,
                    UrlRetrievalTimeout = new TimeSpan(0, 0, 10),
                    VerificationTime = DateTime.UtcNow
                }
            };

            var chainBuilt = chain.Build(cert);

            if (chainBuilt)
            {
                return chain.ChainElements
                    .Cast<X509ChainElement>()
                    .Any(x => string.Compare(x.Certificate.Thumbprint.ToUpper().Trim(), rootCertificateTumbprint.ToUpper(), StringComparison.Ordinal) == 0);
            }

            errors = chain.ChainStatus.Aggregate(errors, (current, chainStatus) =>
                current + string.Format("Chain error: {0} {1}", chainStatus.Status, chainStatus.StatusInformation));

            return false;
        }
    }
}