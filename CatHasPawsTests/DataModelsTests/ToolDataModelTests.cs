using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RPP.DataModels;
using RPP.Exceptions;

namespace CatHasPawsTests.DataModelsTests;

[TestFixture]
public class ToolDataModelTests
{
    [Test]
    public void Validate_IdIsNullOrEmpty_ThrowsValidationException()
    {
        var tool = CreateTool(null, "Перфоратор", "Мощный перфоратор Makita", true);
        Assert.That(() => tool.Validate(), Throws.TypeOf<ValidationException>());

        tool = CreateTool(string.Empty, "Перфоратор", "Мощный перфоратор Makita", true);
        Assert.That(() => tool.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_IdIsNotGuid_ThrowsValidationException()
    {
        var tool = CreateTool("123", "Перфоратор", "Мощный перфоратор Makita", true);
        Assert.That(() => tool.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_ToolNameIsEmpty_ThrowsValidationException()
    {
        var tool = CreateTool(Guid.NewGuid().ToString(), null, "Мощный перфоратор Makita", true);
        Assert.That(() => tool.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_ToolNameIsTooShort_ThrowsValidationException()
    {
        var tool = CreateTool(Guid.NewGuid().ToString(), "П", "Мощный перфоратор Makita", true);
        Assert.That(() => tool.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_WithPreviousName_AllFieldsCorrect_PassesValidation()
    {
        var id = Guid.NewGuid().ToString();
        var toolName = "Перфоратор Makita HR2470";
        var previousName = "Перфоратор Makita";
        var description = "Профессиональный перфоратор";
        var isAvailable = true;

        var tool = new ToolDataModel(id, toolName, description, isAvailable, previousName);

        Assert.That(() => tool.Validate(), Throws.Nothing);
        Assert.Multiple(() =>
        {
            Assert.That(tool.Id, Is.EqualTo(id));
            Assert.That(tool.ToolName, Is.EqualTo(toolName));
            Assert.That(tool.Description, Is.EqualTo(description));
            Assert.That(tool.IsAvailable, Is.EqualTo(isAvailable));
            Assert.That(tool.PreviousToolName, Is.EqualTo(previousName));
        });
    }

    private static ToolDataModel CreateTool(string? id, string? toolName,
        string? description, bool isAvailable)
    {
        return new ToolDataModel(id!, toolName!, description!, isAvailable);
    }
}