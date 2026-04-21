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
public class WorkerBusinessLogicContractTests
{
    private Mock<IWorkerStorageContract> _mockStorage;
    private Mock<ILogger<WorkerBusinessLogicContract>> _mockLogger;
    private IWorkerBusinessLogicContract _businessLogic;

    [SetUp]
    public void SetUp()
    {
        _mockStorage = new Mock<IWorkerStorageContract>();
        _mockLogger = new Mock<ILogger<WorkerBusinessLogicContract>>();
        _businessLogic = new WorkerBusinessLogicContract(_mockStorage.Object, _mockLogger.Object);
    }

    [Test]
    public void GetAllWorkers_StorageReturnsList_ReturnsList()
    {
        // Arrange
        var expectedList = new List<WorkerDataModel>
        {
            new WorkerDataModel(Guid.NewGuid().ToString(), "Иван Иванов", "+7-999-111-22-33", "ivan@mail.com",
                WorkerPost.Master, DateTime.Now.AddDays(-10), DateTime.Now.AddYears(-25), 50000, false),
            new WorkerDataModel(Guid.NewGuid().ToString(), "Петр Петров", "+7-999-222-33-44", "petr@mail.com",
                WorkerPost.Handyman, DateTime.Now.AddDays(-5), DateTime.Now.AddYears(-30), 40000, false)
        };
        _mockStorage.Setup(x => x.GetList(It.IsAny<bool>())).Returns(expectedList);

        // Act
        var result = _businessLogic.GetAllWorkers(true);

        // Assert
        Assert.That(result, Is.EqualTo(expectedList));
        _mockStorage.Verify(x => x.GetList(true), Times.Once);
    }

    [Test]
    public void GetWorkersByPost_ValidPost_CallsStorage()
    {
        // Arrange
        var post = WorkerPost.Master;
        var expectedList = new List<WorkerDataModel>
        {
            new WorkerDataModel(Guid.NewGuid().ToString(), "Иван Иванов", "+7-999-111-22-33", "ivan@mail.com",
                post, DateTime.Now.AddDays(-10), DateTime.Now.AddYears(-25), 50000, false)
        };
        _mockStorage.Setup(x => x.GetListByPost(post, true)).Returns(expectedList);

        // Act
        var result = _businessLogic.GetWorkersByPost(post, true);

        // Assert
        Assert.That(result, Is.EqualTo(expectedList));
        _mockStorage.Verify(x => x.GetListByPost(post, true), Times.Once);
    }

    [Test]
    public void GetWorkersByBirthDate_ValidDates_CallsStorage()
    {
        // Arrange
        var fromDate = DateTime.Now.AddYears(-30);
        var toDate = DateTime.Now.AddYears(-20);
        var expectedList = new List<WorkerDataModel>
        {
            new WorkerDataModel(Guid.NewGuid().ToString(), "Иван Иванов", "+7-999-111-22-33", "ivan@mail.com",
                WorkerPost.Master, DateTime.Now.AddDays(-10), DateTime.Now.AddYears(-25), 50000, false)
        };
        _mockStorage.Setup(x => x.GetListByBirthDate(fromDate, toDate, true)).Returns(expectedList);

        // Act
        var result = _businessLogic.GetWorkersByBirthDate(fromDate, toDate, true);

        // Assert
        Assert.That(result, Is.EqualTo(expectedList));
        _mockStorage.Verify(x => x.GetListByBirthDate(fromDate, toDate, true), Times.Once);
    }

    [Test]
    public void GetWorkersByBirthDate_IncorrectDates_ThrowsIncorrectDatesException()
    {
        // Arrange
        var fromDate = DateTime.Now.AddYears(-20);
        var toDate = DateTime.Now.AddYears(-30);

        // Act & Assert
        Assert.That(() => _businessLogic.GetWorkersByBirthDate(fromDate, toDate, true),
            Throws.TypeOf<IncorrectDatesException>());
    }

    [Test]
    public void GetWorkerByData_DataIsGuid_CallsGetElementById()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var expectedWorker = new WorkerDataModel(id, "Иван Иванов", "+7-999-111-22-33", "ivan@mail.com",
            WorkerPost.Master, DateTime.Now.AddDays(-10), DateTime.Now.AddYears(-25), 50000, false);
        _mockStorage.Setup(x => x.GetElementById(id)).Returns(expectedWorker);

        // Act
        var result = _businessLogic.GetWorkerByData(id);

        // Assert
        Assert.That(result, Is.EqualTo(expectedWorker));
        _mockStorage.Verify(x => x.GetElementById(id), Times.Once);
    }

    [Test]
    public void InsertWorker_ValidModel_CallsAddElement()
    {
        // Arrange
        var worker = new WorkerDataModel(
            Guid.NewGuid().ToString(),
            "Иван Иванов",
            "+7-999-111-22-33",
            "ivan@mail.com",
            WorkerPost.Master,
            DateTime.Now.AddDays(-10),
            DateTime.Now.AddYears(-25),
            50000,
            false);

        // Act
        _businessLogic.InsertWorker(worker);

        // Assert
        _mockStorage.Verify(x => x.AddElement(worker), Times.Once);
    }

    [Test]
    public void DeleteWorker_ValidId_CallsDeleteElement()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();

        // Act
        _businessLogic.DeleteWorker(id);

        // Assert
        _mockStorage.Verify(x => x.DeleteElement(id), Times.Once);
    }
}