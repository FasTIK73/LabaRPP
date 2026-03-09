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
    public void DeleteHome_ValidId_CallsDeleteElement()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();

        // Act
        _businessLogic.DeleteHome(id);

        // Assert
        _mockStorage.Verify(x => x.DeleteElement(id), Times.Once);
    }
}