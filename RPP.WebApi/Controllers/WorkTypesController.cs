using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPP.WebApi.Adapters;
using RPP.WebApi.Models.BindingModels;

namespace RPP.WebApi.Controllers;

/// <summary>
/// Контроллер для работы с типами работ
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class WorkTypesController : ControllerBase
{
    private readonly WorkTypeAdapter _adapter;

    public WorkTypesController(WorkTypeAdapter adapter)
    {
        _adapter = adapter;
    }

    /// <summary>
    /// Получить все типы работ
    /// </summary>
    [HttpGet]
    public IActionResult GetAll()
    {
        return _adapter.GetAll().GetResponse(Request, Response);
    }

    /// <summary>
    /// Получить тип работы по ID или названию
    /// </summary>
    [HttpGet("{data}")]
    public IActionResult GetByData(string data)
    {
        return _adapter.GetByData(data).GetResponse(Request, Response);
    }

    /// <summary>
    /// Получить историю изменения цены
    /// </summary>
    [HttpGet("history/{id}")]
    public IActionResult GetPriceHistory(string id)
    {
        return _adapter.GetPriceHistory(id).GetResponse(Request, Response);
    }

    /// <summary>
    /// Создать новый тип работы
    /// </summary>
    [HttpPost]
    public IActionResult Create([FromBody] WorkTypeBindingModel model)
    {
        return _adapter.Create(model).GetResponse(Request, Response);
    }

    /// <summary>
    /// Обновить данные типа работы
    /// </summary>
    [HttpPut]
    public IActionResult Update([FromBody] WorkTypeBindingModel model)
    {
        return _adapter.Update(model).GetResponse(Request, Response);
    }

    /// <summary>
    /// Удалить тип работы
    /// </summary>
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        return _adapter.Delete(id).GetResponse(Request, Response);
    }
}