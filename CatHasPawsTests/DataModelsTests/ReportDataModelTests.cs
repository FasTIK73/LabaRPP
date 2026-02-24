using RPP.DataModels;
using RPP.Enums;
using RPP.Exceptions;

namespace CatHasPawsTests.DataModelsTests;

[TestFixture]
public class ReportDataModelTests
{
    [Test]
    public void Validate_IdIsNullOrEmpty_ThrowsValidationException()
    {
        var report = new ReportDataModel(null!, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), DateTime.Now.AddDays(-5),
            10.5, ReportStatus.Completed, 100);
        Assert.That(() => report.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_IdIsNotGuid_ThrowsValidationException()
    {
        var report = new ReportDataModel("123", Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), DateTime.Now.AddDays(-5),
            10.5, ReportStatus.Completed, 100);
        Assert.That(() => report.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_HomeIdIsEmpty_ThrowsValidationException()
    {
        var report = new ReportDataModel(Guid.NewGuid().ToString(), null!, Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), DateTime.Now.AddDays(-5),
            10.5, ReportStatus.Completed, 100);
        Assert.That(() => report.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_HomeIdIsNotGuid_ThrowsValidationException()
    {
        var report = new ReportDataModel(Guid.NewGuid().ToString(), "123", Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), DateTime.Now.AddDays(-5),
            10.5, ReportStatus.Completed, 100);
        Assert.That(() => report.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_WorkTypeIdIsEmpty_ThrowsValidationException()
    {
        var report = new ReportDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), null!,
            Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), DateTime.Now.AddDays(-5),
            10.5, ReportStatus.Completed, 100);
        Assert.That(() => report.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_WorkerIdIsEmpty_ThrowsValidationException()
    {
        var report = new ReportDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
            null!, Guid.NewGuid().ToString(), DateTime.Now.AddDays(-5),
            10.5, ReportStatus.Completed, 100);
        Assert.That(() => report.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_ToolIdIsEmpty_ThrowsValidationException()
    {
        var report = new ReportDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(), null!, DateTime.Now.AddDays(-5),
            10.5, ReportStatus.Completed, 100);
        Assert.That(() => report.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_WorkVolumeIsZero_ThrowsValidationException()
    {
        var report = new ReportDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), DateTime.Now.AddDays(-5),
            0, ReportStatus.Completed, 100);
        Assert.That(() => report.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_WorkVolumeIsNegative_ThrowsValidationException()
    {
        var report = new ReportDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), DateTime.Now.AddDays(-5),
            -10, ReportStatus.Completed, 100);
        Assert.That(() => report.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_StatusIsNone_ThrowsValidationException()
    {
        var report = new ReportDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), DateTime.Now.AddDays(-5),
            10.5, ReportStatus.None, 100);
        Assert.That(() => report.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_TotalCostIsZero_ThrowsValidationException()
    {
        var report = new ReportDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), DateTime.Now.AddDays(-5),
            10.5, ReportStatus.Completed, 0);
        Assert.That(() => report.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_TotalCostIsNegative_ThrowsValidationException()
    {
        var report = new ReportDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), DateTime.Now.AddDays(-5),
            10.5, ReportStatus.Completed, -100);
        Assert.That(() => report.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_WorkDateInFuture_ThrowsValidationException()
    {
        var report = new ReportDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), DateTime.Now.AddDays(5),
            10.5, ReportStatus.Completed, 100);
        Assert.That(() => report.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_AllFieldsCorrect_PassesValidation()
    {
        var id = Guid.NewGuid().ToString();
        var homeId = Guid.NewGuid().ToString();
        var workTypeId = Guid.NewGuid().ToString();
        var workerId = Guid.NewGuid().ToString();
        var toolId = Guid.NewGuid().ToString();
        var workDate = DateTime.Now.AddDays(-5);
        var workVolume = 10.5;
        var status = ReportStatus.Completed;
        var totalCost = 5250.0;

        var report = new ReportDataModel(id, homeId, workTypeId, workerId, toolId,
            workDate, workVolume, status, totalCost);

        Assert.That(() => report.Validate(), Throws.Nothing);
        Assert.Multiple(() =>
        {
            Assert.That(report.Id, Is.EqualTo(id));
            Assert.That(report.HomeId, Is.EqualTo(homeId));
            Assert.That(report.WorkTypeId, Is.EqualTo(workTypeId));
            Assert.That(report.WorkerId, Is.EqualTo(workerId));
            Assert.That(report.ToolId, Is.EqualTo(toolId));
            Assert.That(report.WorkDate, Is.EqualTo(workDate));
            Assert.That(report.WorkVolume, Is.EqualTo(workVolume));
            Assert.That(report.Status, Is.EqualTo(status));
            Assert.That(report.TotalCost, Is.EqualTo(totalCost));
        });
    }
}