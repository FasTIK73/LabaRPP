using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPP.WebApi.Adapters;
using RPP.WebApi.Models.BindingModels;

namespace RPP.WebApi.Controllers;

/// <summary>
/// Контроллер для работы с инструментами
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ToolsController : ControllerBase
{
    private readonly ToolAdapter _adapter;

    public ToolsController(ToolAdapter adapter)
    {
        _adapter = adapter;
    }

    /// <summary>
    /// Получить все инструменты
    /// </summary>
    [HttpGet]
    public IActionResult GetAll([FromQuery] bool onlyAvailable = true)
    {
        return _adapter.GetAll(onlyAvailable).GetResponse(Request, Response);
    }

    /// <summary>
    /// Получить инструмент по ID или названию
    /// </summary>
    [HttpGet("{data}")]
    public IActionResult GetByData(string data)
    {
        return _adapter.GetByData(data).GetResponse(Request, Response);
    }

    /// <summary>
    /// Создать новый инструмент
    /// </summary>
    [HttpPost]
    public IActionResult Create([FromBody] ToolBindingModel model)
    {
        return _adapter.Create(model).GetResponse(Request, Response);
    }

    /// <summary>
    /// Обновить данные инструмента
    /// </summary>
    [HttpPut]
    public IActionResult Update([FromBody] ToolBindingModel model)
    {
        return _adapter.Update(model).GetResponse(Request, Response);
    }

    /// <summary>
    /// Удалить инструмент
    /// </summary>
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        return _adapter.Delete(id).GetResponse(Request, Response);
    }
}