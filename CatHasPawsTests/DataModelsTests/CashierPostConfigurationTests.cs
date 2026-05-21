using RPP.Common.Infrastructure.PostConfigurations;

namespace CatHasPawsTests.DataModelsTests;

[TestFixture]
public class CashierPostConfigurationTests
{
    [Test]
    public void CashierPostConfiguration_InheritsFromPostConfiguration()
    {
        var config = new CashierPostConfiguration();
        Assert.That(config, Is.InstanceOf<PostConfiguration>());
    }

    [Test]
    public void CashierPostConfiguration_HasSalePercent()
    {
        var config = new CashierPostConfiguration { SalePercent = 0.15 };
        Assert.That(config.SalePercent, Is.EqualTo(0.15));
    }

    [Test]
    public void CashierPostConfiguration_HasBonusForExtraSales()
    {
        var config = new CashierPostConfiguration { BonusForExtraSales = 0.05 };
        Assert.That(config.BonusForExtraSales, Is.EqualTo(0.05));
    }

    [Test]
    public void CashierPostConfiguration_Type_ReturnsCashierPostConfiguration()
    {
        var config = new CashierPostConfiguration();
        Assert.That(config.Type, Is.EqualTo("CashierPostConfiguration"));
    }
}