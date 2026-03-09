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
public class HomeBusinessLogicContractTests
{
    private Mock<IHomeStorageContract> _mockStorage;
    private Mock<ILogger<HomeBusinessLogicContract>> _mockLogger;
    private IHomeBusinessLogicContract _businessLogic;

    [SetUp]
    public void SetUp()
    {
        _mockStorage = new Mock<IHomeStorageContract>();
        _mockLogger = new Mock<ILogger<HomeBusinessLogicContract>>();
        _businessLogic = new HomeBusinessLogicContract(_mockStorage.Object, _mockLogger.Object);
    }

    [Test]
    public void GetAllHomes_StorageReturnsList_ReturnsList()
    {
        // Arrange
        var expectedList = new List<HomeDataModel>
        {
            new HomeDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "ул. Ленина, 1", 75.5, HomeType.Apartment, HomeStatus.Pending),
            new HomeDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "ул. Пушкина, 10", 120.0, HomeType.House, HomeStatus.Completed)
        };
        _mockStorage.Setup(x => x.GetList()).Returns(expectedList);

        // Act
        var result = _businessLogic.GetAllHomes();

        // Assert
        Assert.That(result, Is.EqualTo(expectedList));
        _mockStorage.Verify(x => x.GetList(), Times.Once);
    }

    [Test]
    public void GetAllHomes_StorageReturnsEmptyList_ReturnsEmptyList()
    {
        // Arrange
        _mockStorage.Setup(x => x.GetList()).Returns(new List<HomeDataModel>());

        // Act
        var result = _businessLogic.GetAllHomes();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
        _mockStorage.Verify(x => x.GetList(), Times.Once);
    }

    [Test]
    public void GetAllHomes_StorageReturnsNull_ThrowsNullListException()
    {
        // Arrange
        _mockStorage.Setup(x => x.GetList()).Returns((List<HomeDataModel>)null!);

        // Act & Assert
        Assert.That(() => _businessLogic.GetAllHomes(), Throws.TypeOf<NullListException>());
    }

    [Test]
    public void GetHomesByClient_ValidClientId_CallsStorage()
    {
        // Arrange
        var clientId = Guid.NewGuid().ToString();
        var expectedList = new List<HomeDataModel>
        {
            new HomeDataModel(Guid.NewGuid().ToString(), clientId, "ул. Ленина, 1", 75.5, HomeType.Apartment, HomeStatus.Pending)
        };
        _mockStorage.Setup(x => x.GetListByClient(clientId)).Returns(expectedList);

        // Act
        var result = _businessLogic.GetHomesByClient(clientId);

        // Assert
        Assert.That(result, Is.EqualTo(expectedList));
        _mockStorage.Verify(x => x.GetListByClient(clientId), Times.Once);
    }

    [Test]
    public void GetHomesByClient_EmptyClientId_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.That(() => _businessLogic.GetHomesByClient(null!), Throws.ArgumentNullException);
        Assert.That(() => _businessLogic.GetHomesByClient(string.Empty), Throws.ArgumentNullException);
    }

    [Test]
    public void GetHomesByStatus_ValidStatus_CallsStorage()
    {
        // Arrange
        var status = HomeStatus.Pending;
        var expectedList = new List<HomeDataModel>
        {
            new HomeDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "ул. Ленина, 1", 75.5, HomeType.Apartment, status)
        };
        _mockStorage.Setup(x => x.GetListByStatus(status)).Returns(expectedList);

        // Act
        var result = _businessLogic.GetHomesByStatus(status);

        // Assert
        Assert.That(result, Is.EqualTo(expectedList));
        _mockStorage.Verify(x => x.GetListByStatus(status), Times.Once);
    }

    [Test]
    public void GetHomesByType_ValidType_CallsStorage()
    {
        // Arrange
        var type = HomeType.Apartment;
        var expectedList = new List<HomeDataModel>
        {
            new HomeDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "ул. Ленина, 1", 75.5, type, HomeStatus.Pending)
        };
        _mockStorage.Setup(x => x.GetListByType(type)).Returns(expectedList);

        // Act
        var result = _businessLogic.GetHomesByType(type);

        // Assert
        Assert.That(result, Is.EqualTo(expectedList));
        _mockStorage.Verify(x => x.GetListByType(type), Times.Once);
    }

    [Test]
    public void GetHomeByData_EmptyData_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.That(() => _businessLogic.GetHomeByData(null!), Throws.ArgumentNullException);
        Assert.That(() => _businessLogic.GetHomeByData(string.Empty), Throws.ArgumentNullException);
    }

    [Test]
    public void GetHomeByData_DataIsGuid_CallsGetElementById()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var expectedHome = new HomeDataModel(id, Guid.NewGuid().ToString(), "ул. Ленина, 1", 75.5, HomeType.Apartment, HomeStatus.Pending);
        _mockStorage.Setup(x => x.GetElementById(id)).Returns(expectedHome);

        // Act
        var result = _businessLogic.GetHomeByData(id);

        // Assert
        Assert.That(result, Is.EqualTo(expectedHome));
        _mockStorage.Verify(x => x.GetElementById(id), Times.Once);
    }

    [Test]
    public void GetHomeByData_DataIsAddress_CallsGetElementByAddress()
    {
        // Arrange
        var address = "ул. Ленина, 1";
        var expectedHome = new HomeDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), address, 75.5, HomeType.Apartment, HomeStatus.Pending);
        _mockStorage.Setup(x => x.GetElementByAddress(address)).Returns(expectedHome);

        // Act
        var result = _businessLogic.GetHomeByData(address);

        // Assert
        Assert.That(result, Is.EqualTo(expectedHome));
        _mockStorage.Verify(x => x.GetElementByAddress(address), Times.Once);
    }

    [Test]
    public void InsertHome_NullModel_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.That(() => _businessLogic.InsertHome(null!), Throws.ArgumentNullException);
    }

    [Test]
    public void InsertHome_ValidModel_CallsAddElement()
    {
        // Arrange
        var home = new HomeDataModel(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            "ул. Ленина, 1",
            75.5,
            HomeType.Apartment,
            HomeStatus.Pending);

        // Act
        _businessLogic.InsertHome(home);

        // Assert
        _mockStorage.Verify(x => x.AddElement(home), Times.Once);
    }

    [Test]
    public void UpdateHome_NullModel_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.That(() => _businessLogic.UpdateHome(null!), Throws.ArgumentNullException);
    }

    [Test]
    public void UpdateHome_ValidModel_CallsUpdateElement()
    {
        // Arrange
        var home = new HomeDataModel(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            "ул. Ленина, 1",
            75.5,
            HomeType.Apartment,
            HomeStatus.Pending);

        // Act
        _businessLogic.UpdateHome(home);

        // Assert
        _mockStorage.Verify(x => x.UpdateElement(home), Times.Once);
    }

    [Test]
    public void DeleteHome_ValidId_CallsDeleteElement()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();

        // Act
        _businessLogic.DeleteHome(id);

        // Assert
        _mockStorage.Verify(x => x.DeleteElement(id), Times.Once);
    }

    [Test]
    public void DeleteHome_InvalidIdFormat_ThrowsValidationException()
    {
        // Act & Assert
        Assert.That(() => _businessLogic.DeleteHome("invalid-guid"), Throws.TypeOf<ValidationException>());
    }
}
