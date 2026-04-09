using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using RPP.Database;
using RPP.WebApi;
using System.Net.Http.Headers;

namespace CatHasPawsTests.WebApiControllersTests;

public abstract class BaseWebApiControllerTest : IAsyncLifetime
{
    protected HttpClient _client;
    protected WebApplicationFactory<Program> _factory;
    protected IServiceScope _scope;
    protected CatHasPawsDbContext _context;

    public async Task InitializeAsync()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();

        // Получаем токен
        var tokenResponse = await _client.GetAsync("/login");
        var token = await tokenResponse.Content.ReadAsStringAsync();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        _scope = _factory.Services.CreateScope();
        _context = _scope.ServiceProvider.GetRequiredService<CatHasPawsDbContext>();
    }

    public async Task DisposeAsync()
    {
        _scope?.Dispose();
        _client?.Dispose();
        _factory?.Dispose();
        await Task.CompletedTask;
    }
}