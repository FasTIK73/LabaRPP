using Microsoft.AspNetCore.Mvc.Testing;
using RPP.WebApi;
using System.Net;
using System.Text;
using System.Text.Json;

namespace CatHasPawsTests.WebApiTests;

[TestFixture]
public class ClientsApiTests : IDisposable
{
    private HttpClient _client;
    private HttpClient _clientWithoutToken;
    private WebApplicationFactory<Program> _factory;
    private string _token;

    [SetUp]
    public async Task SetUp()
    {
        _factory = new WebApplicationFactory<Program>();
        _clientWithoutToken = _factory.CreateClient();
        _client = _factory.CreateClient();

        // Получаем токен
        var tokenResponse = await _client.GetAsync("/api/Auth/login");
        var tokenJson = await tokenResponse.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(tokenJson);
        _token = doc.RootElement.GetProperty("token").GetString();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
    }

    [TearDown]
    public void TearDown()
    {
        _client?.Dispose();
        _clientWithoutToken?.Dispose();
        _factory?.Dispose();
    }

    public void Dispose()
    {
        _client?.Dispose();
        _clientWithoutToken?.Dispose();
        _factory?.Dispose();
    }

    // ========== ТЕСТЫ ДЛЯ LOGIN ==========

    [Test]
    public async Task Login_ReturnsToken()
    {
        // Act
        var response = await _client.GetAsync("/api/Auth/login");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = await response.Content.ReadAsStringAsync();
        Assert.That(content, Does.Contain("token"));
    }

    // ========== ТЕСТЫ ДЛЯ GET /api/Clients ==========

    [Test]
    public async Task GetAll_WithoutToken_ReturnsUnauthorized()
    {
        // Act
        var response = await _clientWithoutToken.GetAsync("/api/Clients");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task GetAll_WithToken_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/Clients");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    // ========== ТЕСТЫ ДЛЯ GET /api/Clients/{id} ==========

    [Test]
    public async Task GetById_WithValidId_ReturnsOk()
    {
        // Arrange - сначала создаем клиента
        var createdId = await CreateTestClient();

        // Act
        var response = await _client.GetAsync($"/api/Clients/{createdId}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = await response.Content.ReadAsStringAsync();
        Assert.That(content, Does.Contain(createdId));
    }

    [Test]
    public async Task GetById_WithInvalidId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync($"/api/Clients/{Guid.NewGuid()}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    // ========== ТЕСТЫ ДЛЯ POST /api/Clients ==========

    [Test]
    public async Task Create_WithValidData_ReturnsNoContent()
    {
        // Arrange
        var client = new
        {
            id = Guid.NewGuid().ToString(),
            name = "Тестовый Клиент",
            address = "ул. Тестовая, 1",
            phoneNumber = "+7-999-111-22-33",
            registrationDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
        };
        var content = new StringContent(JsonSerializer.Serialize(client), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/Clients", content);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task Create_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange - пустое имя
        var client = new
        {
            id = Guid.NewGuid().ToString(),
            name = "",
            address = "ул. Тестовая, 1",
            phoneNumber = "+7-999-111-22-33",
            registrationDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
        };
        var content = new StringContent(JsonSerializer.Serialize(client), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/Clients", content);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    // ========== ТЕСТЫ ДЛЯ PUT /api/Clients ==========

    [Test]
    public async Task Update_WithValidData_ReturnsNoContent()
    {
        // Arrange - сначала создаем клиента
        var createdId = await CreateTestClient();

        var updatedClient = new
        {
            id = createdId,
            name = "Обновленный Клиент",
            address = "ул. Новая, 100",
            phoneNumber = "+7-999-888-77-66",
            registrationDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
        };
        var content = new StringContent(JsonSerializer.Serialize(updatedClient), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync("/api/Clients", content);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    // ========== ТЕСТЫ ДЛЯ DELETE /api/Clients/{id} ==========

    [Test]
    public async Task Delete_WithValidId_ReturnsNoContent()
    {
        // Arrange - сначала создаем клиента
        var createdId = await CreateTestClient();

        // Act
        var response = await _client.DeleteAsync($"/api/Clients/{createdId}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task Delete_WithInvalidId_ReturnsBadRequest()
    {
        // Act
        var response = await _client.DeleteAsync($"/api/Clients/{Guid.NewGuid()}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    // ========== ВСПОМОГАТЕЛЬНЫЙ МЕТОД ==========

    private async Task<string> CreateTestClient()
    {
        var id = Guid.NewGuid().ToString();
        var client = new
        {
            id = id,
            name = "Тестовый Клиент",
            address = "ул. Тестовая, 1",
            phoneNumber = "+7-999-111-22-33",
            registrationDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
        };
        var content = new StringContent(JsonSerializer.Serialize(client), Encoding.UTF8, "application/json");

        await _client.PostAsync("/api/Clients", content);

        return id;
    }
}