using RPP.Database.Models;
using RPP.WebApi.Models.BindingModels;
using RPP.WebApi.Models.ViewModels;
using System.Net;
using System.Text;
using System.Text.Json;

namespace CatHasPawsTests.WebApiControllersTests;

[TestFixture]
public class ClientsControllerTests : BaseWebApiControllerTest
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private StringContent GetJsonContent(object obj)
    {
        return new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
    }

    [Test]
    public async Task GetAll_WhenHaveRecords_ReturnsOk()
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
        await _context.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync("/api/Clients");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = await response.Content.ReadAsStringAsync();
        var clients = JsonSerializer.Deserialize<List<ClientViewModel>>(content, _jsonOptions);

        Assert.That(clients, Is.Not.Null);
        Assert.That(clients!.Count, Is.EqualTo(1));
        Assert.That(clients[0].Id, Is.EqualTo(client.Id));
    }

    [Test]
    public async Task GetById_WhenExists_ReturnsOk()
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
        await _context.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync($"/api/Clients/{client.Id}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ClientViewModel>(content, _jsonOptions);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(client.Id));
    }

    [Test]
    public async Task GetById_WhenNotExists_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync($"/api/Clients/{Guid.NewGuid()}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task Create_ValidModel_ReturnsNoContent()
    {
        // Arrange
        var model = new ClientBindingModel
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Иван Иванов",
            Address = "ул. Ленина, 1",
            PhoneNumber = "+7-999-111-22-33",
            RegistrationDate = DateTime.Now.AddDays(-30)
        };
        var content = GetJsonContent(model);

        // Act
        var response = await _client.PostAsync("/api/Clients", content);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

        var checkResponse = await _client.GetAsync($"/api/Clients/{model.Id}");
        Assert.That(checkResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task Create_InvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var model = new ClientBindingModel
        {
            Name = "",
            Address = "ул. Ленина, 1",
            PhoneNumber = "+7-999-111-22-33"
        };
        var content = GetJsonContent(model);

        // Act
        var response = await _client.PostAsync("/api/Clients", content);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task Update_ValidModel_ReturnsNoContent()
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
        await _context.SaveChangesAsync();

        var model = new ClientBindingModel
        {
            Id = client.Id,
            Name = "Петр Петров",
            Address = "ул. Пушкина, 10",
            PhoneNumber = "+7-999-222-33-44",
            RegistrationDate = client.RegistrationDate
        };
        var content = GetJsonContent(model);

        // Act
        var response = await _client.PutAsync("/api/Clients", content);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task Delete_WhenExists_ReturnsNoContent()
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
        await _context.SaveChangesAsync();

        // Act
        var response = await _client.DeleteAsync($"/api/Clients/{client.Id}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

        var checkResponse = await _client.GetAsync($"/api/Clients/{client.Id}");
        Assert.That(checkResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task Delete_WhenNotExists_ReturnsBadRequest()
    {
        // Act
        var response = await _client.DeleteAsync($"/api/Clients/{Guid.NewGuid()}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}