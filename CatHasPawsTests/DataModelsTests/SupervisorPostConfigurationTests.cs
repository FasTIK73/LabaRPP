using RPP.Common.Infrastructure.PostConfigurations;

namespace CatHasPawsTests.DataModelsTests;

[TestFixture]
public class SupervisorPostConfigurationTests
{
    [Test]
    public void SupervisorPostConfiguration_InheritsFromPostConfiguration()
    {
        var config = new SupervisorPostConfiguration();
        Assert.That(config, Is.InstanceOf<PostConfiguration>());
    }

    [Test]
    public void SupervisorPostConfiguration_HasPersonalCountTrendPremium()
    {
        var config = new SupervisorPostConfiguration { PersonalCountTrendPremium = 1000 };
        Assert.That(config.PersonalCountTrendPremium, Is.EqualTo(1000));
    }

    [Test]
    public void SupervisorPostConfiguration_Type_ReturnsSupervisorPostConfiguration()
    {
        var config = new SupervisorPostConfiguration();
        Assert.That(config.Type, Is.EqualTo("SupervisorPostConfiguration"));
    }
}