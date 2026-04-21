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
            new WorkTypeDataModel(Guid.NewGuid().ToString(), "Укладка плитки", MeasurementUnit.SquareMeter, 1200)
        };
        _mockStorage.Setup(x => x.GetList()).Returns(expectedList);

        // Act
        var result = _businessLogic.GetAllWorkTypes();

        // Assert
        Assert.That(result, Is.EqualTo(expectedList));
        _mockStorage.Verify(x => x.GetList(), Times.Once);
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
    public void DeleteWorkType_ValidId_CallsDeleteElement()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();

        // Act
        _businessLogic.DeleteWorkType(id);

        // Assert
        _mockStorage.Verify(x => x.DeleteElement(id), Times.Once);
    }
}