using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RPP.DataModels;
using RPP.Enums;
using RPP.Exceptions;

namespace CatHasPawsTests.DataModelsTests;

[TestFixture]
public class WorkTypeDataModelTests
{
    [Test]
    public void Validate_IdIsNullOrEmpty_ThrowsValidationException()
    {
        var workType = CreateWorkType(null, "Покраска стен", MeasurementUnit.SquareMeter, 500);
        Assert.That(() => workType.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_IdIsNotGuid_ThrowsValidationException()
    {
        var workType = CreateWorkType("123", "Покраска стен", MeasurementUnit.SquareMeter, 500);
        Assert.That(() => workType.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_WorkNameIsEmpty_ThrowsValidationException()
    {
        var workType = CreateWorkType(Guid.NewGuid().ToString(), null, MeasurementUnit.SquareMeter, 500);
        Assert.That(() => workType.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_UnitIsNone_ThrowsValidationException()
    {
        var workType = CreateWorkType(Guid.NewGuid().ToString(), "Покраска стен", MeasurementUnit.None, 500);
        Assert.That(() => workType.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_PriceIsZero_ThrowsValidationException()
    {
        var workType = CreateWorkType(Guid.NewGuid().ToString(), "Покраска стен", MeasurementUnit.SquareMeter, 0);
        Assert.That(() => workType.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_PriceIsNegative_ThrowsValidationException()
    {
        var workType = CreateWorkType(Guid.NewGuid().ToString(), "Покраска стен", MeasurementUnit.SquareMeter, -100);
        Assert.That(() => workType.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_WithHistory_InvalidPreviousPrice_ThrowsValidationException()
    {
        var workType = new WorkTypeDataModel(
            Guid.NewGuid().ToString(),
            "Покраска стен",
            MeasurementUnit.SquareMeter,
            600,
            -100,  // отрицательная предыдущая цена
            DateTime.Now.AddDays(-30));

        Assert.That(() => workType.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_WithHistory_PriceChangeDateInFuture_ThrowsValidationException()
    {
        var workType = new WorkTypeDataModel(
            Guid.NewGuid().ToString(),
            "Покраска стен",
            MeasurementUnit.SquareMeter,
            600,
            500,
            DateTime.Now.AddDays(30));  // дата в будущем

        Assert.That(() => workType.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_WithHistory_AllFieldsCorrect_PassesValidation()
    {
        var id = Guid.NewGuid().ToString();
        var workName = "Покраска стен";
        var unit = MeasurementUnit.SquareMeter;
        var price = 600.0;
        var previousPrice = 500.0;
        var changeDate = DateTime.Now.AddDays(-30);

        var workType = new WorkTypeDataModel(id, workName, unit, price, previousPrice, changeDate);

        Assert.That(() => workType.Validate(), Throws.Nothing);
        Assert.Multiple(() =>
        {
            Assert.That(workType.Id, Is.EqualTo(id));
            Assert.That(workType.WorkName, Is.EqualTo(workName));
            Assert.That(workType.Unit, Is.EqualTo(unit));
            Assert.That(workType.PricePerUnit, Is.EqualTo(price));
            Assert.That(workType.PreviousPrice, Is.EqualTo(previousPrice));
            Assert.That(workType.PriceChangeDate, Is.EqualTo(changeDate));
        });
    }

    private static WorkTypeDataModel CreateWorkType(string? id, string? workName,
        MeasurementUnit unit, double pricePerUnit)
    {
        return new WorkTypeDataModel(id!, workName!, unit, pricePerUnit);
    }
}