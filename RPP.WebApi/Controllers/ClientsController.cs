using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPP.WebApi.Adapters;
using RPP.WebApi.Models.BindingModels;

namespace RPP.WebApi.Controllers;

/// <summary>
/// Контроллер для работы с клиентами
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ClientsController : ControllerBase
{
    private readonly ClientAdapter _adapter;

    public ClientsController(ClientAdapter adapter)
    {
        _adapter = adapter;
    }

    /// <summary>
    /// Получить всех клиентов
    /// </summary>
    [HttpGet]
    public IActionResult GetAll()
    {
        return _adapter.GetAll().GetResponse(Request, Response);
    }

    /// <summary>
    /// Получить клиента по ID, телефону или ФИО
    /// </summary>
    [HttpGet("{data}")]
    public IActionResult GetByData(string data)
    {
        return _adapter.GetByData(data).GetResponse(Request, Response);
    }

    /// <summary>
    /// Создать нового клиента
    /// </summary>
    [HttpPost]
    public IActionResult Create([FromBody] ClientBindingModel model)
    {
        return _adapter.Create(model).GetResponse(Request, Response);
    }

    /// <summary>
    /// Обновить данные клиента
    /// </summary>
    [HttpPut]
    public IActionResult Update([FromBody] ClientBindingModel model)
    {
        return _adapter.Update(model).GetResponse(Request, Response);
    }

    /// <summary>
    /// Удалить клиента
    /// </summary>
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        return _adapter.Delete(id).GetResponse(Request, Response);
    }
}