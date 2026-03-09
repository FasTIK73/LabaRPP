using Microsoft.Extensions.Logging;
using Moq;
using RPP.BusinessLogicsContracts;
using RPP.DataModels;
using RPP.Exceptions;
using RPP.Implementations;
using RPP.StoragesContracts;

namespace CatHasPawsTests.BusinessLogicsContractsTests;

[TestFixture]
public class ToolBusinessLogicContractTests
{
    private Mock<IToolStorageContract> _mockStorage;
    private Mock<ILogger<ToolBusinessLogicContract>> _mockLogger;
    private IToolBusinessLogicContract _businessLogic;

    [SetUp]
    public void SetUp()
    {
        _mockStorage = new Mock<IToolStorageContract>();
        _mockLogger = new Mock<ILogger<ToolBusinessLogicContract>>();
        _businessLogic = new ToolBusinessLogicContract(_mockStorage.Object, _mockLogger.Object);
    }

    [Test]
    public void GetAllTools_StorageReturnsList_ReturnsList()
    {
        // Arrange
        var expectedList = new List<ToolDataModel>
        {
            new ToolDataModel(Guid.NewGuid().ToString(), "Перфоратор", "Мощный перфоратор Makita", true),
            new ToolDataModel(Guid.NewGuid().ToString(), "Дрель", "Аккумуляторная дрель Bosch", true)
        };
        _mockStorage.Setup(x => x.GetList(true)).Returns(expectedList);

        // Act
        var result = _businessLogic.GetAllTools(true);

        // Assert
        Assert.That(result, Is.EqualTo(expectedList));
        _mockStorage.Verify(x => x.GetList(true), Times.Once);
    }

    [Test]
    public void GetToolByData_DataIsGuid_CallsGetElementById()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var expectedTool = new ToolDataModel(id, "Перфоратор", "Мощный перфоратор Makita", true);
        _mockStorage.Setup(x => x.GetElementById(id)).Returns(expectedTool);

        // Act
        var result = _businessLogic.GetToolByData(id);

        // Assert
        Assert.That(result, Is.EqualTo(expectedTool));
        _mockStorage.Verify(x => x.GetElementById(id), Times.Once);
    }

    [Test]
    public void InsertTool_ValidModel_CallsAddElement()
    {
        // Arrange
        var tool = new ToolDataModel(
            Guid.NewGuid().ToString(),
            "Перфоратор",
            "Мощный перфоратор Makita",
            true);

        // Act
        _businessLogic.InsertTool(tool);

        // Assert
        _mockStorage.Verify(x => x.AddElement(tool), Times.Once);
    }

    [Test]
    public void DeleteTool_ValidId_CallsDeleteElement()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();

        // Act
        _businessLogic.DeleteTool(id);

        // Assert
        _mockStorage.Verify(x => x.DeleteElement(id), Times.Once);
    }
}