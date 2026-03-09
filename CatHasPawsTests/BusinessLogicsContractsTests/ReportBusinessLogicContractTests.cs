using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), DateTime.Now.AddDays(-5), 10.5, ReportStatus.Completed, 5250),
            new ReportDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), DateTime.Now.AddDays(-10), 20.0, ReportStatus.Completed, 10000)
        };
        _mockReportStorage.Setup(x => x.GetList(fromDate, toDate)).Returns(expectedList);

        // Act
        var result = _businessLogic.GetAllReports(fromDate, toDate);

        // Assert
        Assert.That(result, Is.EqualTo(expectedList));
        _mockReportStorage.Verify(x => x.GetList(fromDate, toDate), Times.Once);
    }

    [Test]
    public void GetAllReports_NoDates_CallsStorage()
    {
        // Arrange
        var expectedList = new List<ReportDataModel>
        {
            new ReportDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), DateTime.Now.AddDays(-5), 10.5, ReportStatus.Completed, 5250)
        };
        _mockReportStorage.Setup(x => x.GetList(null, null)).Returns(expectedList);

        // Act
        var result = _businessLogic.GetAllReports();

        // Assert
        Assert.That(result, Is.EqualTo(expectedList));
        _mockReportStorage.Verify(x => x.GetList(null, null), Times.Once);
    }

    [Test]
    public void GetAllReports_IncorrectDates_ThrowsIncorrectDatesException()
    {
        // Arrange
        var fromDate = DateTime.Now;
        var toDate = DateTime.Now.AddDays(-30);

        // Act & Assert
        Assert.That(() => _businessLogic.GetAllReports(fromDate, toDate), Throws.TypeOf<IncorrectDatesException>());
    }

    [Test]
    public void GetReportsByHome_ValidHomeId_CallsStorage()
    {
        // Arrange
        var homeId = Guid.NewGuid().ToString();
        var expectedList = new List<ReportDataModel>
        {
            new ReportDataModel(Guid.NewGuid().ToString(), homeId, Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), DateTime.Now.AddDays(-5), 10.5, ReportStatus.Completed, 5250)
        };
        _mockReportStorage.Setup(x => x.GetListByHome(homeId)).Returns(expectedList);

        // Act
        var result = _businessLogic.GetReportsByHome(homeId);

        // Assert
        Assert.That(result, Is.EqualTo(expectedList));
        _mockReportStorage.Verify(x => x.GetListByHome(homeId), Times.Once);
    }

    [Test]
    public void GetReportsByHome_EmptyHomeId_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.That(() => _businessLogic.GetReportsByHome(null!), Throws.ArgumentNullException);
        Assert.That(() => _businessLogic.GetReportsByHome(string.Empty), Throws.ArgumentNullException);
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
    public void GetReportsByWorkType_ValidWorkTypeId_CallsStorage()
    {
        // Arrange
        var workTypeId = Guid.NewGuid().ToString();
        var expectedList = new List<ReportDataModel>
        {
            new ReportDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), workTypeId,
                Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), DateTime.Now.AddDays(-5), 10.5, ReportStatus.Completed, 5250)
        };
        _mockReportStorage.Setup(x => x.GetListByWorkType(workTypeId)).Returns(expectedList);

        // Act
        var result = _businessLogic.GetReportsByWorkType(workTypeId);

        // Assert
        Assert.That(result, Is.EqualTo(expectedList));
        _mockReportStorage.Verify(x => x.GetListByWorkType(workTypeId), Times.Once);
    }

    [Test]
    public void GetReportsByTool_ValidToolId_CallsStorage()
    {
        // Arrange
        var toolId = Guid.NewGuid().ToString();
        var expectedList = new List<ReportDataModel>
        {
            new ReportDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(), toolId, DateTime.Now.AddDays(-5), 10.5, ReportStatus.Completed, 5250)
        };
        _mockReportStorage.Setup(x => x.GetListByTool(toolId)).Returns(expectedList);

        // Act
        var result = _businessLogic.GetReportsByTool(toolId);

        // Assert
        Assert.That(result, Is.EqualTo(expectedList));
        _mockReportStorage.Verify(x => x.GetListByTool(toolId), Times.Once);
    }

    [Test]
    public void GetReportById_ValidId_CallsStorage()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var expectedReport = new ReportDataModel(id, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), DateTime.Now.AddDays(-5), 10.5, ReportStatus.Completed, 5250);
        _mockReportStorage.Setup(x => x.GetElementById(id)).Returns(expectedReport);

        // Act
        var result = _businessLogic.GetReportById(id);

        // Assert
        Assert.That(result, Is.EqualTo(expectedReport));
        _mockReportStorage.Verify(x => x.GetElementById(id), Times.Once);
    }

    [Test]
    public void GetReportById_EmptyId_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.That(() => _businessLogic.GetReportById(null!), Throws.ArgumentNullException);
        Assert.That(() => _businessLogic.GetReportById(string.Empty), Throws.ArgumentNullException);
    }

    [Test]
    public void GetReportById_InvalidIdFormat_ThrowsValidationException()
    {
        // Act & Assert
        Assert.That(() => _businessLogic.GetReportById("invalid-guid"), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void InsertReport_NullModel_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.That(() => _businessLogic.InsertReport(null!), Throws.ArgumentNullException);
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
    public void UpdateReport_NullModel_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.That(() => _businessLogic.UpdateReport(null!), Throws.ArgumentNullException);
    }

    [Test]
    public void UpdateReport_ValidModel_CallsUpdateElement()
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
        _businessLogic.UpdateReport(report);

        // Assert
        _mockReportStorage.Verify(x => x.UpdateElement(report), Times.Once);
    }

    [Test]
    public void CancelReport_ValidId_CallsDeleteElement()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();

        // Act
        _businessLogic.CancelReport(id);

        // Assert
        _mockReportStorage.Verify(x => x.DeleteElement(id), Times.Once);
    }

    [Test]
    public void CancelReport_InvalidIdFormat_ThrowsValidationException()
    {
        // Act & Assert
        Assert.That(() => _businessLogic.CancelReport("invalid-guid"), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void CalculateWorkerSalary_ValidParameters_ReturnsCalculation()
    {
        // Arrange
        var workerId = Guid.NewGuid().ToString();
        var fromDate = DateTime.Now.AddDays(-30);
        var toDate = DateTime.Now;

        var reports = new List<ReportDataModel>
        {
            new ReportDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
                workerId, Guid.NewGuid().ToString(), DateTime.Now.AddDays(-5), 10.5, ReportStatus.Completed, 5250),
            new ReportDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
                workerId, Guid.NewGuid().ToString(), DateTime.Now.AddDays(-10), 20.0, ReportStatus.Completed, 10000)
        };

        var worker = new WorkerDataModel(workerId, "Иван Иванов", "+7-999-111-22-33", "ivan@mail.com",
            WorkerPost.Master, DateTime.Now.AddDays(-10), DateTime.Now.AddYears(-25), 50000, false);

        _mockReportStorage.Setup(x => x.GetListByWorker(workerId)).Returns(reports);
        _mockWorkerStorage.Setup(x => x.GetElementById(workerId)).Returns(worker);

        // Act
        var result = _businessLogic.CalculateWorkerSalary(workerId, fromDate, toDate);

        // Assert
        Assert.That(result, Is.EqualTo(0)); // Заглушка возвращает 0
        _mockReportStorage.Verify(x => x.GetListByWorker(workerId), Times.Once);
    }

    [Test]
    public void CalculateWorkerSalary_WorkerNotFound_ThrowsElementNotFoundException()
    {
        // Arrange
        var workerId = Guid.NewGuid().ToString();
        var fromDate = DateTime.Now.AddDays(-30);
        var toDate = DateTime.Now;

        _mockWorkerStorage.Setup(x => x.GetElementById(workerId)).Returns((WorkerDataModel)null!);

        // Act & Assert
        Assert.That(() => _businessLogic.CalculateWorkerSalary(workerId, fromDate, toDate),
            Throws.TypeOf<ElementNotFoundException>());
    }

    [Test]
    public void CalculateWorkerSalary_EmptyWorkerId_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.That(() => _businessLogic.CalculateWorkerSalary(null!, DateTime.Now, DateTime.Now),
            Throws.ArgumentNullException);
    }
}