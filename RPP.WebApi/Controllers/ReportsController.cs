using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPP.WebApi.Adapters;
using RPP.WebApi.Models.BindingModels;

namespace RPP.WebApi.Controllers;

/// <summary>
/// Контроллер для работы с отчетами
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ReportsController : ControllerBase
{
    private readonly ReportAdapter _adapter;

    public ReportsController(ReportAdapter adapter)
    {
        _adapter = adapter;
    }

    /// <summary>
    /// Получить все отчеты за период
    /// </summary>
    [HttpGet]
    public IActionResult GetAll([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
    {
        return _adapter.GetAll(fromDate, toDate).GetResponse(Request, Response);
    }

    /// <summary>
    /// Получить отчет по ID
    /// </summary>
    [HttpGet("{id}")]
    public IActionResult GetById(string id)
    {
        return _adapter.GetById(id).GetResponse(Request, Response);
    }

    /// <summary>
    /// Получить отчеты по помещению
    /// </summary>
    [HttpGet("byhome/{homeId}")]
    public IActionResult GetByHome(string homeId)
    {
        return _adapter.GetByHome(homeId).GetResponse(Request, Response);
    }

    /// <summary>
    /// Получить отчеты по работнику
    /// </summary>
    [HttpGet("byworker/{workerId}")]
    public IActionResult GetByWorker(string workerId)
    {
        return _adapter.GetByWorker(workerId).GetResponse(Request, Response);
    }

    /// <summary>
    /// Получить отчеты по типу работы
    /// </summary>
    [HttpGet("byworktype/{workTypeId}")]
    public IActionResult GetByWorkType(string workTypeId)
    {
        return _adapter.GetByWorkType(workTypeId).GetResponse(Request, Response);
    }

    /// <summary>
    /// Получить отчеты по инструменту
    /// </summary>
    [HttpGet("bytool/{toolId}")]
    public IActionResult GetByTool(string toolId)
    {
        return _adapter.GetByTool(toolId).GetResponse(Request, Response);
    }

    /// <summary>
    /// Рассчитать зарплату работника
    /// </summary>
    [HttpGet("salary/{workerId}")]
    public IActionResult CalculateSalary(string workerId, [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
    {
        return _adapter.CalculateSalary(workerId, fromDate, toDate).GetResponse(Request, Response);
    }

    /// <summary>
    /// Создать новый отчет
    /// </summary>
    [HttpPost]
    public IActionResult Create([FromBody] ReportBindingModel model)
    {
        return _adapter.Create(model).GetResponse(Request, Response);
    }

    /// <summary>
    /// Обновить отчет
    /// </summary>
    [HttpPut]
    public IActionResult Update([FromBody] ReportBindingModel model)
    {
        return _adapter.Update(model).GetResponse(Request, Response);
    }

    /// <summary>
    /// Отменить отчет (удалить)
    /// </summary>
    [HttpDelete("{id}")]
    public IActionResult Cancel(string id)
    {
        return _adapter.Cancel(id).GetResponse(Request, Response);
    }
}