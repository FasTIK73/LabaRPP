using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public void GetAllTools_StorageReturnsEmptyList_ReturnsEmptyList()
    {
        // Arrange
        _mockStorage.Setup(x => x.GetList(It.IsAny<bool>())).Returns(new List<ToolDataModel>());

        // Act
        var result = _businessLogic.GetAllTools(true);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
        _mockStorage.Verify(x => x.GetList(true), Times.Once);
    }

    [Test]
    public void GetAllTools_StorageReturnsNull_ThrowsNullListException()
    {
        // Arrange
        _mockStorage.Setup(x => x.GetList(It.IsAny<bool>())).Returns((List<ToolDataModel>)null!);

        // Act & Assert
        Assert.That(() => _businessLogic.GetAllTools(true), Throws.TypeOf<NullListException>());
    }

    [Test]
    public void GetToolByData_EmptyData_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.That(() => _businessLogic.GetToolByData(null!), Throws.ArgumentNullException);
        Assert.That(() => _businessLogic.GetToolByData(string.Empty), Throws.ArgumentNullException);
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
    public void GetToolByData_DataIsName_CallsGetElementByName()
    {
        // Arrange
        var name = "Перфоратор";
        var expectedTool = new ToolDataModel(Guid.NewGuid().ToString(), name, "Мощный перфоратор Makita", true);
        _mockStorage.Setup(x => x.GetElementByName(name)).Returns(expectedTool);

        // Act
        var result = _businessLogic.GetToolByData(name);

        // Assert
        Assert.That(result, Is.EqualTo(expectedTool));
        _mockStorage.Verify(x => x.GetElementByName(name), Times.Once);
    }

    [Test]
    public void GetToolByData_DataIsPreviousName_CallsGetElementByPreviousName()
    {
        // Arrange
        var previousName = "Старое название";
        var expectedTool = new ToolDataModel(Guid.NewGuid().ToString(), "Перфоратор", "Мощный перфоратор Makita", true, previousName);
        _mockStorage.Setup(x => x.GetElementByPreviousName(previousName)).Returns(expectedTool);

        // Act
        var result = _businessLogic.GetToolByData(previousName);

        // Assert
        Assert.That(result, Is.EqualTo(expectedTool));
        _mockStorage.Verify(x => x.GetElementByName(previousName), Times.Once);
        _mockStorage.Verify(x => x.GetElementByPreviousName(previousName), Times.Once);
    }

    [Test]
    public void InsertTool_NullModel_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.That(() => _businessLogic.InsertTool(null!), Throws.ArgumentNullException);
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
    public void UpdateTool_NullModel_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.That(() => _businessLogic.UpdateTool(null!), Throws.ArgumentNullException);
    }

    [Test]
    public void UpdateTool_ValidModel_CallsUpdateElement()
    {
        // Arrange
        var tool = new ToolDataModel(
            Guid.NewGuid().ToString(),
            "Перфоратор",
            "Мощный перфоратор Makita",
            true);

        // Act
        _businessLogic.UpdateTool(tool);

        // Assert
        _mockStorage.Verify(x => x.UpdateElement(tool), Times.Once);
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

    [Test]
    public void DeleteTool_InvalidIdFormat_ThrowsValidationException()
    {
        // Act & Assert
        Assert.That(() => _businessLogic.DeleteTool("invalid-guid"), Throws.TypeOf<ValidationException>());
    }
}