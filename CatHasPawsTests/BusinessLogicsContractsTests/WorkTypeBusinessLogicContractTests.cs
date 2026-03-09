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
public class WorkTypeBusinessLogicContractTests
{
    private Mock<IWorkTypeStorageContract> _mockStorage;
    private Mock<ILogger<WorkTypeBusinessLogicContract>> _mockLogger;
    private IWorkTypeBusinessLogicContract _businessLogic;

    [SetUp]
    public void SetUp()
    {
        _mockStorage = new Mock<IWorkTypeStorageContract>();
        _mockLogger = new Mock<ILogger<WorkTypeBusinessLogicContract>>();
        _businessLogic = new WorkTypeBusinessLogicContract(_mockStorage.Object, _mockLogger.Object);
    }

    [Test]
    public void GetAllWorkTypes_StorageReturnsList_ReturnsList()
    {
        // Arrange
        var expectedList = new List<WorkTypeDataModel>
        {
            new WorkTypeDataModel(Guid.NewGuid().ToString(), "Покраска стен", MeasurementUnit.SquareMeter, 500),
            new WorkTypeDataModel(Guid.NewGuid().ToString(), "Укладка плитки", MeasurementUnit.SquareMeter, 1200),
            new WorkTypeDataModel(Guid.NewGuid().ToString(), "Установка сантехники", MeasurementUnit.Piece, 3000)
        };
        _mockStorage.Setup(x => x.GetList()).Returns(expectedList);

        // Act
        var result = _businessLogic.GetAllWorkTypes();

        // Assert
        Assert.That(result, Is.EqualTo(expectedList));
        _mockStorage.Verify(x => x.GetList(), Times.Once);
    }

    [Test]
    public void GetAllWorkTypes_StorageReturnsEmptyList_ReturnsEmptyList()
    {
        // Arrange
        _mockStorage.Setup(x => x.GetList()).Returns(new List<WorkTypeDataModel>());

        // Act
        var result = _businessLogic.GetAllWorkTypes();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
        _mockStorage.Verify(x => x.GetList(), Times.Once);
    }

    [Test]
    public void GetAllWorkTypes_StorageReturnsNull_ThrowsNullListException()
    {
        // Arrange
        _mockStorage.Setup(x => x.GetList()).Returns((List<WorkTypeDataModel>)null!);

        // Act & Assert
        Assert.That(() => _businessLogic.GetAllWorkTypes(), Throws.TypeOf<NullListException>());
    }

    [Test]
    public void GetWorkTypeByData_EmptyData_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.That(() => _businessLogic.GetWorkTypeByData(null!), Throws.ArgumentNullException);
        Assert.That(() => _businessLogic.GetWorkTypeByData(string.Empty), Throws.ArgumentNullException);
    }

    [Test]
    public void GetWorkTypeByData_DataIsGuid_CallsGetElementById()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var expectedWorkType = new WorkTypeDataModel(id, "Покраска стен", MeasurementUnit.SquareMeter, 500);
        _mockStorage.Setup(x => x.GetElementById(id)).Returns(expectedWorkType);

        // Act
        var result = _businessLogic.GetWorkTypeByData(id);

        // Assert
        Assert.That(result, Is.EqualTo(expectedWorkType));
        _mockStorage.Verify(x => x.GetElementById(id), Times.Once);
    }

    [Test]
    public void GetWorkTypeByData_DataIsName_CallsGetElementByName()
    {
        // Arrange
        var name = "Покраска стен";
        var expectedWorkType = new WorkTypeDataModel(Guid.NewGuid().ToString(), name, MeasurementUnit.SquareMeter, 500);
        _mockStorage.Setup(x => x.GetElementByName(name)).Returns(expectedWorkType);

        // Act
        var result = _businessLogic.GetWorkTypeByData(name);

        // Assert
        Assert.That(result, Is.EqualTo(expectedWorkType));
        _mockStorage.Verify(x => x.GetElementByName(name), Times.Once);
    }

    [Test]
    public void GetPriceHistory_ValidId_CallsStorage()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var expectedHistory = new List<WorkTypeDataModel>
        {
            new WorkTypeDataModel(id, "Покраска стен", MeasurementUnit.SquareMeter, 500, 450, DateTime.Now.AddDays(-30)),
            new WorkTypeDataModel(id, "Покраска стен", MeasurementUnit.SquareMeter, 450, 400, DateTime.Now.AddDays(-60))
        };
        _mockStorage.Setup(x => x.GetPriceHistory(id)).Returns(expectedHistory);

        // Act
        var result = _businessLogic.GetPriceHistory(id);

        // Assert
        Assert.That(result, Is.EqualTo(expectedHistory));
        _mockStorage.Verify(x => x.GetPriceHistory(id), Times.Once);
    }

    [Test]
    public void GetPriceHistory_EmptyId_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.That(() => _businessLogic.GetPriceHistory(null!), Throws.ArgumentNullException);
        Assert.That(() => _businessLogic.GetPriceHistory(string.Empty), Throws.ArgumentNullException);
    }

    [Test]
    public void InsertWorkType_NullModel_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.That(() => _businessLogic.InsertWorkType(null!), Throws.ArgumentNullException);
    }

    [Test]
    public void InsertWorkType_ValidModel_CallsAddElement()
    {
        // Arrange
        var workType = new WorkTypeDataModel(
            Guid.NewGuid().ToString(),
            "Покраска стен",
            MeasurementUnit.SquareMeter,
            500);

        // Act
        _businessLogic.InsertWorkType(workType);

        // Assert
        _mockStorage.Verify(x => x.AddElement(workType), Times.Once);
    }

    [Test]
    public void UpdateWorkType_NullModel_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.That(() => _businessLogic.UpdateWorkType(null!), Throws.ArgumentNullException);
    }

    [Test]
    public void UpdateWorkType_ValidModel_CallsUpdateElement()
    {
        // Arrange
        var workType = new WorkTypeDataModel(
            Guid.NewGuid().ToString(),
            "Покраска стен",
            MeasurementUnit.SquareMeter,
            500);

        // Act
        _businessLogic.UpdateWorkType(workType);

        // Assert
        _mockStorage.Verify(x => x.UpdateElement(workType), Times.Once);
    }

    [Test]
    public void DeleteWorkType_ValidId_CallsDeleteElement()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();

        // Act
        _businessLogic.DeleteWorkType(id);

        // Assert
        _mockStorage.Verify(x => x.DeleteElement(id), Times.Once);
    }

    [Test]
    public void DeleteWorkType_InvalidIdFormat_ThrowsValidationException()
    {
        // Act & Assert
        Assert.That(() => _businessLogic.DeleteWorkType("invalid-guid"), Throws.TypeOf<ValidationException>());
    }
}