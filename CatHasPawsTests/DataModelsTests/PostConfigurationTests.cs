using RPP.Common.Infrastructure.PostConfigurations;

namespace CatHasPawsTests.DataModelsTests;

[TestFixture]
public class PostConfigurationTests
{
    [Test]
    public void PostConfiguration_HasRateProperty()
    {
        var config = new PostConfiguration { Rate = 50000 };
        Assert.That(config.Rate, Is.EqualTo(50000));
    }

    [Test]
    public void PostConfiguration_Type_ReturnsPostConfiguration()
    {
        var config = new PostConfiguration();
        Assert.That(config.Type, Is.EqualTo("PostConfiguration"));
    }
}