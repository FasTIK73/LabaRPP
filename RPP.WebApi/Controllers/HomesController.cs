using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPP.WebApi.Adapters;
using RPP.WebApi.Models.BindingModels;

namespace RPP.WebApi.Controllers;

/// <summary>
/// Контроллер для работы с помещениями
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class HomesController : ControllerBase
{
    private readonly HomeAdapter _adapter;

    public HomesController(HomeAdapter adapter)
    {
        _adapter = adapter;
    }

    /// <summary>
    /// Получить все помещения
    /// </summary>
    [HttpGet]
    public IActionResult GetAll()
    {
        return _adapter.GetAll().GetResponse(Request, Response);
    }

    /// <summary>
    /// Получить помещение по ID или адресу
    /// </summary>
    [HttpGet("{data}")]
    public IActionResult GetByData(string data)
    {
        return _adapter.GetByData(data).GetResponse(Request, Response);
    }

    /// <summary>
    /// Создать новое помещение
    /// </summary>
    [HttpPost]
    public IActionResult Create([FromBody] HomeBindingModel model)
    {
        return _adapter.Create(model).GetResponse(Request, Response);
    }

    /// <summary>
    /// Обновить данные помещения
    /// </summary>
    [HttpPut]
    public IActionResult Update([FromBody] HomeBindingModel model)
    {
        return _adapter.Update(model).GetResponse(Request, Response);
    }

    /// <summary>
    /// Удалить помещение
    /// </summary>
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        return _adapter.Delete(id).GetResponse(Request, Response);
    }
}