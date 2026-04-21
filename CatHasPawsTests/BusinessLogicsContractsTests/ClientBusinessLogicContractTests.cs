using Microsoft.Extensions.Logging;
using Moq;
using RPP.BusinessLogicsContracts;
using RPP.DataModels;
using RPP.Exceptions;
using RPP.Implementations;
using RPP.StoragesContracts;

namespace CatHasPawsTests.BusinessLogicsContractsTests;

[TestFixture]
public class ClientBusinessLogicContractTests
{
    private Mock<IClientStorageContract> _mockStorage;
    private Mock<ILogger<ClientBusinessLogicContract>> _mockLogger;
    private IClientBusinessLogicContract _businessLogic;

    [SetUp]
    public void SetUp()
    {
        _mockStorage = new Mock<IClientStorageContract>();
        _mockLogger = new Mock<ILogger<ClientBusinessLogicContract>>();
        _businessLogic = new ClientBusinessLogicContract(_mockStorage.Object, _mockLogger.Object);
    }

    [Test]
    public void GetAllClients_StorageReturnsList_ReturnsList()
    {
        // Arrange
        var expectedList = new List<ClientDataModel>
        {
            new ClientDataModel(Guid.NewGuid().ToString(), "Иван Иванов", "ул. Ленина, 1", "+7-999-111-22-33", DateTime.Now.AddDays(-30)),
            new ClientDataModel(Guid.NewGuid().ToString(), "Петр Петров", "ул. Пушкина, 10", "+7-999-222-33-44", DateTime.Now.AddDays(-60))
        };
        _mockStorage.Setup(x => x.GetList()).Returns(expectedList);

        // Act
        var result = _businessLogic.GetAllClients();

        // Assert
        Assert.That(result, Is.EqualTo(expectedList));
        _mockStorage.Verify(x => x.GetList(), Times.Once);
    }

    [Test]
    public void GetAllClients_StorageReturnsEmptyList_ReturnsEmptyList()
    {
        // Arrange
        _mockStorage.Setup(x => x.GetList()).Returns(new List<ClientDataModel>());

        // Act
        var result = _businessLogic.GetAllClients();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
        _mockStorage.Verify(x => x.GetList(), Times.Once);
    }

    [Test]
    public void GetAllClients_StorageReturnsNull_ThrowsNullListException()
    {
        // Arrange
        _mockStorage.Setup(x => x.GetList()).Returns((List<ClientDataModel>)null!);

        // Act & Assert
        Assert.That(() => _businessLogic.GetAllClients(), Throws.TypeOf<NullListException>());
        _mockStorage.Verify(x => x.GetList(), Times.Once);
    }

    [Test]
    public void GetClientByData_EmptyData_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.That(() => _businessLogic.GetClientByData(null!), Throws.ArgumentNullException);
        Assert.That(() => _businessLogic.GetClientByData(string.Empty), Throws.ArgumentNullException);
    }

    [Test]
    public void GetClientByData_DataIsGuid_CallsGetElementById()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var expectedClient = new ClientDataModel(id, "Иван Иванов", "ул. Ленина, 1", "+7-999-111-22-33", DateTime.Now.AddDays(-30));
        _mockStorage.Setup(x => x.GetElementById(id)).Returns(expectedClient);

        // Act
        var result = _businessLogic.GetClientByData(id);

        // Assert
        Assert.That(result, Is.EqualTo(expectedClient));
        _mockStorage.Verify(x => x.GetElementById(id), Times.Once);
    }

    [Test]
    public void GetClientByData_DataIsPhone_CallsGetElementByPhone()
    {
        // Arrange
        var phone = "+7-999-111-22-33";
        var expectedClient = new ClientDataModel(Guid.NewGuid().ToString(), "Иван Иванов", "ул. Ленина, 1", phone, DateTime.Now.AddDays(-30));
        _mockStorage.Setup(x => x.GetElementByPhone(phone)).Returns(expectedClient);

        // Act
        var result = _businessLogic.GetClientByData(phone);

        // Assert
        Assert.That(result, Is.EqualTo(expectedClient));
        _mockStorage.Verify(x => x.GetElementByPhone(phone), Times.Once);
    }

    [Test]
    public void GetClientByData_ElementNotFound_ThrowsElementNotFoundException()
    {
        // Arrange
        var data = "Несуществующий клиент";
        _mockStorage.Setup(x => x.GetElementByName(data)).Returns((ClientDataModel)null!);

        // Act & Assert
        Assert.That(() => _businessLogic.GetClientByData(data), Throws.TypeOf<ElementNotFoundException>());
    }

    [Test]
    public void InsertClient_ValidModel_CallsAddElement()
    {
        // Arrange
        var client = new ClientDataModel(
            Guid.NewGuid().ToString(),
            "Иван Иванов",
            "ул. Ленина, 1",
            "+7-999-111-22-33",
            DateTime.Now.AddDays(-30));

        // Act
        _businessLogic.InsertClient(client);

        // Assert
        _mockStorage.Verify(x => x.AddElement(client), Times.Once);
    }

    [Test]
    public void InsertClient_NullModel_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.That(() => _businessLogic.InsertClient(null!), Throws.ArgumentNullException);
    }

    [Test]
    public void UpdateClient_ValidModel_CallsUpdateElement()
    {
        // Arrange
        var client = new ClientDataModel(
            Guid.NewGuid().ToString(),
            "Иван Иванов",
            "ул. Ленина, 1",
            "+7-999-111-22-33",
            DateTime.Now.AddDays(-30));

        // Act
        _businessLogic.UpdateClient(client);

        // Assert
        _mockStorage.Verify(x => x.UpdateElement(client), Times.Once);
    }

    [Test]
    public void DeleteClient_ValidId_CallsDeleteElement()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();

        // Act
        _businessLogic.DeleteClient(id);

        // Assert
        _mockStorage.Verify(x => x.DeleteElement(id), Times.Once);
    }

    [Test]
    public void DeleteClient_InvalidIdFormat_ThrowsValidationException()
    {
        // Act & Assert
        Assert.That(() => _businessLogic.DeleteClient("invalid-guid"), Throws.TypeOf<ValidationException>());
    }
}