using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Mews.Eet.Dto;
using Mews.Eet.Dto.Identifiers;
using Newtonsoft.Json;
using Xunit;

namespace Mews.Eet.Tests.IntegrationTests
{
    public class Basics
    {
        [Fact]
        public async Task SendRevenueSimple()
        {
            var certificate = CreateCertificate(Fixtures.Second);
            var record = CreateSimpleRecord(certificate, Fixtures.Second);
            var client = new EetClient(certificate, EetEnvironment.Playground);
            var response = await client.SendRevenueAsync(record);
            Assert.Null(response.Error);
            Assert.NotNull(response.Success);
            Assert.NotNull(response.Success.ConfirmationCode);
            Assert.False(response.Warnings.Any());
        }

        [Fact]
        public async Task TimeoutWorks()
        {

            var certificate = CreateCertificate(Fixtures.Second);
            var record = CreateSimpleRecord(certificate, Fixtures.Second);
            var client = new EetClient(certificate, EetEnvironment.Playground, TimeSpan.FromMilliseconds(1));
            await Assert.ThrowsAsync<TaskCanceledException>(async () => await client.SendRevenueAsync(record));
        }

        [Fact]
        public async Task SendRevenue()
        {
            var fixture = Fixtures.Third;

            var certificate = new Certificate(
                password: fixture.CertificatePassword,
                data: fixture.CertificateData
            );
            var record = new RevenueRecord(
                identification: new Identification(
                    taxPayerIdentifier: new TaxIdentifier(fixture.TaxId),
                    registryIdentifier: new RegistryIdentifier("01"),
                    registrationUnitIdentifier: new RegistrationUnitIdentifier(fixture.RegistrationUnitId),
                    certificate: certificate
                ),
                revenue: new Revenue(
                    gross: new CurrencyValue(1234.00m)
                ),
                billNumber: new BillNumber("2016-123"),
                mandatedByMultipleTaxPayers: true
            );
            var client = new EetClient(certificate, EetEnvironment.Playground);
            var response = await client.SendRevenueAsync(record);
            Assert.Null(response.Error);
            Assert.NotNull(response.Success);
            Assert.NotNull(response.Success.ConfirmationCode);
            Assert.False(response.Warnings.Any());
        }

        [Fact]
        public async Task HandlesError()
        {
            var certificate = CreateCertificate(Fixtures.First);
            var record =  new RevenueRecord(
                    identification: new Identification(
                    taxPayerIdentifier: new TaxIdentifier("CZ111444789"),
                    registryIdentifier: new RegistryIdentifier("01"),
                    registrationUnitIdentifier: new RegistrationUnitIdentifier(Fixtures.First.RegistrationUnitId),
                    certificate: certificate
                ),
                revenue: new Revenue(
                    gross: new CurrencyValue(1234.00m)
                ),
                billNumber: new BillNumber("2016-123")
            );
            var client = new EetClient(certificate, EetEnvironment.Playground);
            var response = await client.SendRevenueAsync(record);
            Assert.NotNull(response.Error);
            Assert.Equal(6, response.Error.Reason.Code);
        }

        [Fact]
        public async Task LoggingIsSerializable()
        {
            var certificate = CreateCertificate(Fixtures.First);
            var record = CreateSimpleRecord(certificate, Fixtures.First);
            var client = new EetClient(
                certificate,
                EetEnvironment.Playground,
                httpTimeout: null,
                logger: new EetLogger((m, d) =>
                {
                    var jsonString = JsonConvert.SerializeObject(d);
                    Assert.StartsWith("{", jsonString);
                })
            );
            var ex = await Record.ExceptionAsync(async () => await client.SendRevenueAsync(record));
            Assert.Null(ex);
        }

        [Fact]
        public async Task ParallelRequestsWork()
        {
            var certificate = CreateCertificate(Fixtures.First);
            var record = CreateSimpleRecord(certificate, Fixtures.First);
            var client = new EetClient(certificate, EetEnvironment.Playground);

            var tasks = Enumerable.Range(0, 10).Select(i => client.SendRevenueAsync(record));
            var ex = await Record.ExceptionAsync(async () => await Task.WhenAll(tasks).ConfigureAwait(continueOnCapturedContext: false));
            Assert.Null(ex);
        }

        [Fact]
        public async Task TimingMeasurementWorks()
        {
            var certificate = CreateCertificate(Fixtures.First);
            var record = CreateSimpleRecord(certificate, Fixtures.First);
            var client = new EetClient(certificate, EetEnvironment.Playground);
            client.HttpRequestFinished += (sender, args) =>
            {
                var duration = args.Duration;
                Assert.InRange(duration, 0, 10000);
            };
            await client.SendRevenueAsync(record);
        }

        [Fact]
        public async Task XmlExtractionWorks()
        {
            var certificate = CreateCertificate(Fixtures.First);
            var record = CreateSimpleRecord(certificate, Fixtures.First);
            var client = new EetClient(certificate, EetEnvironment.Playground);
            client.XmlMessageSerialized += (sender, args) =>
            {
                Assert.NotNull(args.XmlElement);
            };
            await client.SendRevenueAsync(record);
        }

        [Fact]
        public async Task RevenueIsSerializedCorrectly()
        {
            var fixture = Fixtures.First;
            var certificate = CreateCertificate(fixture);
            var record = new RevenueRecord(
                identification: new Identification(
                    taxPayerIdentifier: new TaxIdentifier(fixture.TaxId),
                    registryIdentifier: new RegistryIdentifier("01"),
                    registrationUnitIdentifier: new RegistrationUnitIdentifier(fixture.RegistrationUnitId),
                    certificate: certificate
                ),
                revenue: new Revenue(
                    gross: new CurrencyValue(1234.00m),
                    deposit: new CurrencyValue(432m),
                    usedDeposit: new CurrencyValue(543m)
                ),
                billNumber: new BillNumber("2016-123")
            );
            var client = new EetClient(certificate, EetEnvironment.Playground);
            client.XmlMessageSerialized += (sender, args) =>
            {
                var xmlElement = args.XmlElement;
                Assert.NotNull(xmlElement);

                var namespaceManager = new XmlNamespaceManager(xmlElement.OwnerDocument.NameTable);
                namespaceManager.AddNamespace("eet", "http://fs.gov.cz/eet/schema/v4");
                var dataNode = xmlElement.SelectSingleNode("//eet:Data", namespaceManager);
                var attributes = dataNode.Attributes;
                Assert.Equal("543.00", attributes["cerp_zuct"].Value);
                Assert.Equal("432.00", attributes["urceno_cerp_zuct"].Value);
            };
            await client.SendRevenueAsync(record);
        }



        private Certificate CreateCertificate(TaxPayerFixture fixture)
        {
            return new Certificate(
                password: fixture.CertificatePassword,
                data: fixture.CertificateData
            );
        }

        private RevenueRecord CreateSimpleRecord(Certificate certificate, TaxPayerFixture fixture)
        {
            return new RevenueRecord(
                identification: new Identification(
                    taxPayerIdentifier: new TaxIdentifier(fixture.TaxId),
                    registryIdentifier: new RegistryIdentifier("01"),
                    registrationUnitIdentifier: new RegistrationUnitIdentifier(fixture.RegistrationUnitId),
                    certificate: certificate
                ),
                revenue: new Revenue(
                    gross: new CurrencyValue(1234.00m)
                ),
                billNumber: new BillNumber("2016-123")
            );
        }
    }
}
