using Microsoft.Extensions.Configuration;
using RPP.WebApi.Infrastructure;

namespace CatHasPawsTests.WebApiTests;

[TestFixture]
public class SalaryConfigurationTests
{
    [Test]
    public void ConfigurationSalary_ReadsFromAppSettings()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var salarySettings = config.GetSection("SalarySettings").Get<SalarySettings>();

        Assert.That(salarySettings, Is.Not.Null);
        Assert.That(salarySettings.ExtraSaleSum, Is.GreaterThan(0));
        Assert.That(salarySettings.MaxParallelThreads, Is.GreaterThan(0));
    }
}