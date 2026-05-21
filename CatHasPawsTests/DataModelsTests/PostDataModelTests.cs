using RPP.DataModels;
using RPP.Common.Infrastructure.PostConfigurations;
using RPP.Common.Exceptions;

namespace CatHasPawsTests.DataModelsTests;

[TestFixture]
public class PostDataModelTests
{
    [Test]
    public void Validate_ValidPost_PassesValidation()
    {
        var config = new PostConfiguration { Rate = 50000 };
        var post = new PostDataModel(Guid.NewGuid().ToString(), "Мастер", 1, config);

        Assert.That(() => post.Validate(), Throws.Nothing);
    }

    [Test]
    public void Validate_EmptyId_ThrowsValidationException()
    {
        var config = new PostConfiguration { Rate = 50000 };
        var post = new PostDataModel("", "Мастер", 1, config);

        Assert.That(() => post.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_EmptyName_ThrowsValidationException()
    {
        var config = new PostConfiguration { Rate = 50000 };
        var post = new PostDataModel(Guid.NewGuid().ToString(), "", 1, config);

        Assert.That(() => post.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_ZeroRate_ThrowsValidationException()
    {
        var config = new PostConfiguration { Rate = 0 };
        var post = new PostDataModel(Guid.NewGuid().ToString(), "Мастер", 1, config);

        Assert.That(() => post.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_ConfigurationIsNull_ThrowsValidationException()
    {
        var post = new PostDataModel(Guid.NewGuid().ToString(), "Мастер", 1, (PostConfiguration)null);

        Assert.That(() => post.Validate(), Throws.TypeOf<ValidationException>());
    }
}