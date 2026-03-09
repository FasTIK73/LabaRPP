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
    public void GetAllWorkers_StorageReturnsEmptyList_ReturnsEmptyList()
    {
        // Arrange
        _mockStorage.Setup(x => x.GetList(It.IsAny<bool>())).Returns(new List<WorkerDataModel>());

        // Act
        var result = _businessLogic.GetAllWorkers(true);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
        _mockStorage.Verify(x => x.GetList(true), Times.Once);
    }

    [Test]
    public void GetAllWorkers_StorageReturnsNull_ThrowsNullListException()
    {
        // Arrange
        _mockStorage.Setup(x => x.GetList(It.IsAny<bool>())).Returns((List<WorkerDataModel>)null!);

        // Act & Assert
        Assert.That(() => _businessLogic.GetAllWorkers(true), Throws.TypeOf<NullListException>());
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

    // ИСПРАВЛЕНО: вместо null передаем валидное значение, проверяем что метод кидает NotImplementedException
    [Test]
    public void GetWorkersByPost_WhenCalled_ThrowsNotImplementedException()
    {
        // Act & Assert - в красной зоне все методы должны кидать NotImplementedException
        Assert.That(() => _businessLogic.GetWorkersByPost(WorkerPost.Master, true),
            Throws.TypeOf<NotImplementedException>());
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
    public void GetWorkersByHireDate_ValidDates_CallsStorage()
    {
        // Arrange
        var fromDate = DateTime.Now.AddDays(-30);
        var toDate = DateTime.Now.AddDays(-10);
        var expectedList = new List<WorkerDataModel>
        {
            new WorkerDataModel(Guid.NewGuid().ToString(), "Иван Иванов", "+7-999-111-22-33", "ivan@mail.com",
                WorkerPost.Master, DateTime.Now.AddDays(-20), DateTime.Now.AddYears(-25), 50000, false)
        };
        _mockStorage.Setup(x => x.GetListByHireDate(fromDate, toDate, true)).Returns(expectedList);

        // Act
        var result = _businessLogic.GetWorkersByHireDate(fromDate, toDate, true);

        // Assert
        Assert.That(result, Is.EqualTo(expectedList));
        _mockStorage.Verify(x => x.GetListByHireDate(fromDate, toDate, true), Times.Once);
    }

    [Test]
    public void GetWorkerByData_EmptyData_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.That(() => _businessLogic.GetWorkerByData(null!), Throws.ArgumentNullException);
        Assert.That(() => _businessLogic.GetWorkerByData(string.Empty), Throws.ArgumentNullException);
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
    public void GetWorkerByData_DataIsPhone_CallsGetElementByPhone()
    {
        // Arrange
        var phone = "+7-999-111-22-33";
        var expectedWorker = new WorkerDataModel(Guid.NewGuid().ToString(), "Иван Иванов", phone, "ivan@mail.com",
            WorkerPost.Master, DateTime.Now.AddDays(-10), DateTime.Now.AddYears(-25), 50000, false);
        _mockStorage.Setup(x => x.GetElementByPhone(phone)).Returns(expectedWorker);

        // Act
        var result = _businessLogic.GetWorkerByData(phone);

        // Assert
        Assert.That(result, Is.EqualTo(expectedWorker));
        _mockStorage.Verify(x => x.GetElementByPhone(phone), Times.Once);
    }

    [Test]
    public void GetWorkerByData_DataIsEmail_CallsGetElementByEmail()
    {
        // Arrange
        var email = "ivan@mail.com";
        var expectedWorker = new WorkerDataModel(Guid.NewGuid().ToString(), "Иван Иванов", "+7-999-111-22-33", email,
            WorkerPost.Master, DateTime.Now.AddDays(-10), DateTime.Now.AddYears(-25), 50000, false);
        _mockStorage.Setup(x => x.GetElementByEmail(email)).Returns(expectedWorker);

        // Act
        var result = _businessLogic.GetWorkerByData(email);

        // Assert
        Assert.That(result, Is.EqualTo(expectedWorker));
        _mockStorage.Verify(x => x.GetElementByEmail(email), Times.Once);
    }

    [Test]
    public void InsertWorker_NullModel_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.That(() => _businessLogic.InsertWorker(null!), Throws.ArgumentNullException);
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
    public void UpdateWorker_NullModel_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.That(() => _businessLogic.UpdateWorker(null!), Throws.ArgumentNullException);
    }

    [Test]
    public void UpdateWorker_ValidModel_CallsUpdateElement()
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
        _businessLogic.UpdateWorker(worker);

        // Assert
        _mockStorage.Verify(x => x.UpdateElement(worker), Times.Once);
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

    [Test]
    public void DeleteWorker_InvalidIdFormat_ThrowsValidationException()
    {
        // Act & Assert
        Assert.That(() => _businessLogic.DeleteWorker("invalid-guid"), Throws.TypeOf<ValidationException>());
    }
}