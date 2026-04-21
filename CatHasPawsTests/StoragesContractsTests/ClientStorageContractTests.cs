using RPP.DataModels;
using RPP.Database;
using RPP.Database.Models;
using RPP.Database.DatabaseImplementations;
using RPP.Common.Exceptions;
using RPP.DatabaseImplementations;

namespace CatHasPawsTests.StoragesContractsTests;

[TestFixture]
public class ClientStorageContractTests : BaseStorageContractTest
{
    private ClientStorageContract _storage;

    [SetUp]
    public void SetUp()
    {
        BaseSetUp();
        _storage = new ClientStorageContract(_context, _mapper);
    }

    [Test]
    public void GetList_WhenHaveRecords_ReturnsList()
    {
        // Arrange
        var client = new ClientEntity
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Иван Иванов",
            Address = "ул. Ленина, 1",
            PhoneNumber = "+7-999-111-22-33",
            RegistrationDate = DateTime.Now.AddDays(-30)
        };
        _context.Clients.Add(client);
        _context.SaveChanges();

        // Act
        var result = _storage.GetList();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].Id, Is.EqualTo(client.Id));
        Assert.That(result[0].Name, Is.EqualTo(client.Name));
    }

    [Test]
    public void GetList_WhenNoRecords_ReturnsEmptyList()
    {
        // Act
        var result = _storage.GetList();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetElementById_WhenExists_ReturnsElement()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var client = new ClientEntity
        {
            Id = id,
            Name = "Иван Иванов",
            Address = "ул. Ленина, 1",
            PhoneNumber = "+7-999-111-22-33",
            RegistrationDate = DateTime.Now.AddDays(-30)
        };
        _context.Clients.Add(client);
        _context.SaveChanges();

        // Act
        var result = _storage.GetElementById(id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(id));
    }

    [Test]
    public void GetElementById_WhenNotExists_ReturnsNull()
    {
        // Act
        var result = _storage.GetElementById(Guid.NewGuid().ToString());

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetElementByPhone_WhenExists_ReturnsElement()
    {
        // Arrange
        var phone = "+7-999-111-22-33";
        var client = new ClientEntity
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Иван Иванов",
            Address = "ул. Ленина, 1",
            PhoneNumber = phone,
            RegistrationDate = DateTime.Now.AddDays(-30)
        };
        _context.Clients.Add(client);
        _context.SaveChanges();

        // Act
        var result = _storage.GetElementByPhone(phone);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.PhoneNumber, Is.EqualTo(phone));
    }

    [Test]
    public void GetElementByPhone_WhenNotExists_ReturnsNull()
    {
        // Act
        var result = _storage.GetElementByPhone("+7-999-000-00-00");

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetElementByName_WhenExists_ReturnsElement()
    {
        // Arrange
        var name = "Иван Иванов";
        var client = new ClientEntity
        {
            Id = Guid.NewGuid().ToString(),
            Name = name,
            Address = "ул. Ленина, 1",
            PhoneNumber = "+7-999-111-22-33",
            RegistrationDate = DateTime.Now.AddDays(-30)
        };
        _context.Clients.Add(client);
        _context.SaveChanges();

        // Act
        var result = _storage.GetElementByName(name);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Name, Is.EqualTo(name));
    }

    [Test]
    public void GetElementByName_WhenNotExists_ReturnsNull()
    {
        // Act
        var result = _storage.GetElementByName("Несуществующее имя");

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void AddElement_ValidElement_AddsToDatabase()
    {
        // Arrange
        var client = new ClientDataModel(
            Guid.NewGuid().ToString(),
            "Иван Иванов",
            "ул. Ленина, 1",
            "+7-999-111-22-33",
            DateTime.Now.AddDays(-30)
        );

        // Act
        _storage.AddElement(client);
        var result = _storage.GetElementById(client.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(client.Id));
        Assert.That(result.Name, Is.EqualTo(client.Name));
        Assert.That(result.PhoneNumber, Is.EqualTo(client.PhoneNumber));
        Assert.That(result.Address, Is.EqualTo(client.Address));
    }


    [Test]
    public void UpdateElement_ValidElement_UpdatesDatabase()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var client = new ClientEntity
        {
            Id = id,
            Name = "Иван Иванов",
            Address = "ул. Ленина, 1",
            PhoneNumber = "+7-999-111-22-33",
            RegistrationDate = DateTime.Now.AddDays(-30)
        };
        _context.Clients.Add(client);
        _context.SaveChanges();

        var updatedClient = new ClientDataModel(
            id,
            "Петр Петров",
            "ул. Пушкина, 10",
            "+7-999-222-33-44",
            DateTime.Now.AddDays(-30)
        );

        // Act
        _storage.UpdateElement(updatedClient);
        var result = _storage.GetElementById(id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Name, Is.EqualTo("Петр Петров"));
        Assert.That(result.Address, Is.EqualTo("ул. Пушкина, 10"));
        Assert.That(result.PhoneNumber, Is.EqualTo("+7-999-222-33-44"));
    }

    [Test]
    public void UpdateElement_ElementNotFound_ThrowsElementNotFoundException()
    {
        // Arrange
        var client = new ClientDataModel(
            Guid.NewGuid().ToString(),
            "Иван Иванов",
            "ул. Ленина, 1",
            "+7-999-111-22-33",
            DateTime.Now.AddDays(-30)
        );

        // Act & Assert
        Assert.That(() => _storage.UpdateElement(client), Throws.TypeOf<ElementNotFoundException>());
    }

    [Test]
    public void DeleteElement_ExistingElement_RemovesFromDatabase()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var client = new ClientEntity
        {
            Id = id,
            Name = "Иван Иванов",
            Address = "ул. Ленина, 1",
            PhoneNumber = "+7-999-111-22-33",
            RegistrationDate = DateTime.Now.AddDays(-30)
        };
        _context.Clients.Add(client);
        _context.SaveChanges();

        // Act
        _storage.DeleteElement(id);
        var result = _storage.GetElementById(id);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void DeleteElement_ElementNotFound_ThrowsElementNotFoundException()
    {
        // Act & Assert
        Assert.That(() => _storage.DeleteElement(Guid.NewGuid().ToString()), Throws.TypeOf<ElementNotFoundException>());
    }
}