using System;
using System.IO;
using System.Reflection;

namespace Mews.Eet.Tests
{
    public class Fixtures
    {
        public static TaxPayerFixture First = new TaxPayerFixture
        {
            TaxId = "CZ8551015704",
            PremisesId = 1,
            CertificatePassword = "aaaa1111",
            CertificateData = File.ReadAllBytes(GetPath("Data/Certificates/Playground/CA_EET-Playground-CZ8551015704.p12"))
        };

        public static TaxPayerFixture Second = new TaxPayerFixture
        {
            TaxId = "CZ00000019",
            PremisesId = 1,
            CertificatePassword = "aaaa1111",
            CertificateData = File.ReadAllBytes(GetPath("Data/Certificates/Playground/CA_EET-Playground-CZ00000019.p12"))
        };

        public static TaxPayerFixture Third = new TaxPayerFixture
        {
            TaxId = "CZ683555118",
            PremisesId = 1,
            CertificatePassword = "aaaa1111",
            CertificateData = File.ReadAllBytes(GetPath("Data/Certificates/Playground/CA_EET-Playground-CZ683555118.p12"))
        };

        private static string GetPath(string relativePath)
        {
            var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
            var dirPath = Path.GetDirectoryName(codeBasePath);
            return Path.Combine(dirPath, relativePath);
        }
    }

    public class TaxPayerFixture
    {
        public string TaxId { get; set; }
        public int PremisesId { get; set; }
        public string CertificatePassword { get; set; }
        public byte[] CertificateData { get; set; }
    }
}
