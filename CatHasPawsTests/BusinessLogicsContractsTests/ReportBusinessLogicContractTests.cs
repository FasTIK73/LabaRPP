using Microsoft.Extensions.Logging;
using Moq;
using RPP.BusinessLogicsContracts;
using RPP.DataModels;
using RPP.Enums;
using RPP.Exceptions;
using RPP.Implementations;
using RPP.StoragesContracts;

namespace CatHasPawsTests.BusinessLogicsContractsTests;

[TestFixture]
public class ReportBusinessLogicContractTests
{
    private Mock<IReportStorageContract> _mockReportStorage;
    private Mock<IWorkerStorageContract> _mockWorkerStorage;
    private Mock<IWorkTypeStorageContract> _mockWorkTypeStorage;
    private Mock<ILogger<ReportBusinessLogicContract>> _mockLogger;
    private IReportBusinessLogicContract _businessLogic;

    [SetUp]
    public void SetUp()
    {
        _mockReportStorage = new Mock<IReportStorageContract>();
        _mockWorkerStorage = new Mock<IWorkerStorageContract>();
        _mockWorkTypeStorage = new Mock<IWorkTypeStorageContract>();
        _mockLogger = new Mock<ILogger<ReportBusinessLogicContract>>();
        _businessLogic = new ReportBusinessLogicContract(
            _mockReportStorage.Object,
            _mockWorkerStorage.Object,
            _mockWorkTypeStorage.Object,
            _mockLogger.Object);
    }

    [Test]
    public void GetAllReports_ValidDates_CallsStorage()
    {
        // Arrange
        var fromDate = DateTime.Now.AddDays(-30);
        var toDate = DateTime.Now;
        var expectedList = new List<ReportDataModel>
        {
            new ReportDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), DateTime.Now.AddDays(-5), 10.5, ReportStatus.Completed, 5250)
        };
        _mockReportStorage.Setup(x => x.GetList(fromDate, toDate)).Returns(expectedList);

        // Act
        var result = _businessLogic.GetAllReports(fromDate, toDate);

        // Assert
        Assert.That(result, Is.EqualTo(expectedList));
        _mockReportStorage.Verify(x => x.GetList(fromDate, toDate), Times.Once);
    }

    [Test]
    public void GetReportsByWorker_ValidWorkerId_CallsStorage()
    {
        // Arrange
        var workerId = Guid.NewGuid().ToString();
        var expectedList = new List<ReportDataModel>
        {
            new ReportDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
                workerId, Guid.NewGuid().ToString(), DateTime.Now.AddDays(-5), 10.5, ReportStatus.Completed, 5250)
        };
        _mockReportStorage.Setup(x => x.GetListByWorker(workerId)).Returns(expectedList);

        // Act
        var result = _businessLogic.GetReportsByWorker(workerId);

        // Assert
        Assert.That(result, Is.EqualTo(expectedList));
        _mockReportStorage.Verify(x => x.GetListByWorker(workerId), Times.Once);
    }

    [Test]
    public void InsertReport_ValidModel_CallsAddElement()
    {
        // Arrange
        var report = new ReportDataModel(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            DateTime.Now.AddDays(-5),
            10.5,
            ReportStatus.Completed,
            5250);

        // Act
        _businessLogic.InsertReport(report);

        // Assert
        _mockReportStorage.Verify(x => x.AddElement(report), Times.Once);
    }

    [Test]
    public void CalculateWorkerSalary_ValidParameters_ReturnsSalary()
    {
        // Arrange
        var workerId = Guid.NewGuid().ToString();
        var fromDate = DateTime.Now.AddDays(-30);
        var toDate = DateTime.Now;

        var worker = new WorkerDataModel(workerId, "Иван Иванов", "+7-999-111-22-33", "ivan@mail.com",
            WorkerPost.Master, DateTime.Now.AddDays(-10), DateTime.Now.AddYears(-25), 50000, false);

        var reports = new List<ReportDataModel>
        {
            new ReportDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
                workerId, Guid.NewGuid().ToString(), DateTime.Now.AddDays(-5), 10.5, ReportStatus.Completed, 5250)
        };

        _mockWorkerStorage.Setup(x => x.GetElementById(workerId)).Returns(worker);
        _mockReportStorage.Setup(x => x.GetListByWorker(workerId)).Returns(reports);

        // Act
        var result = _businessLogic.CalculateWorkerSalary(workerId, fromDate, toDate);

        // Assert
        Assert.That(result, Is.GreaterThan(0));
        _mockWorkerStorage.Verify(x => x.GetElementById(workerId), Times.Once);
        _mockReportStorage.Verify(x => x.GetListByWorker(workerId), Times.Once);
    }
}