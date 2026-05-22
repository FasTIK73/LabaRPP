using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPP.WebApi.Adapters;
using RPP.WebApi.Models.BindingModels;
using RPP.WebApi.Services;

namespace RPP.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ReportsController : ControllerBase
{
    private readonly ReportAdapter _adapter;
    private readonly ReportService _reportService;

    public ReportsController(ReportAdapter adapter, ReportService reportService)
    {
        _adapter = adapter;
        _reportService = reportService;
    }

    // ========== СУЩЕСТВУЮЩИЕ МЕТОДЫ ==========

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
    {
        return _adapter.GetAll(fromDate, toDate).GetResponse(Request, Response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        return _adapter.GetById(id).GetResponse(Request, Response);
    }

    [HttpGet("byhome/{homeId}")]
    public async Task<IActionResult> GetByHome(string homeId)
    {
        return _adapter.GetByHome(homeId).GetResponse(Request, Response);
    }

    [HttpGet("byworker/{workerId}")]
    public async Task<IActionResult> GetByWorker(string workerId)
    {
        return _adapter.GetByWorker(workerId).GetResponse(Request, Response);
    }

    [HttpGet("byworktype/{workTypeId}")]
    public async Task<IActionResult> GetByWorkType(string workTypeId)
    {
        return _adapter.GetByWorkType(workTypeId).GetResponse(Request, Response);
    }

    [HttpGet("bytool/{toolId}")]
    public async Task<IActionResult> GetByTool(string toolId)
    {
        return _adapter.GetByTool(toolId).GetResponse(Request, Response);
    }

    [HttpGet("salary/{workerId}")]
    public async Task<IActionResult> CalculateSalary(string workerId, [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
    {
        return _adapter.CalculateSalary(workerId, fromDate, toDate).GetResponse(Request, Response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ReportBindingModel model)
    {
        return _adapter.Create(model).GetResponse(Request, Response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ReportBindingModel model)
    {
        return _adapter.Update(model).GetResponse(Request, Response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Cancel(string id)
    {
        return _adapter.Cancel(id).GetResponse(Request, Response);
    }

    // ========== НОВЫЕ МЕТОДЫ ДЛЯ 6 ЛАБЫ ==========

    [HttpPost("product-report")]
    public async Task<IActionResult> GenerateProductReport([FromBody] ProductReportBindingModel model)
    {
        try
        {
            var reportData = await _reportService.GenerateProductReportAsync(model);
            return File(reportData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"ProductReport_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("sales-report")]
    public async Task<IActionResult> GenerateSalesReport([FromBody] SalesReportBindingModel model)
    {
        try
        {
            var reportData = await _reportService.GenerateSalesReportAsync(model);
            return File(reportData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"SalesReport_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("salary-report")]
    public async Task<IActionResult> GenerateSalaryReport([FromBody] SalaryReportBindingModel model)
    {
        try
        {
            var reportData = await _reportService.GenerateSalaryReportAsync(model);
            return File(reportData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"SalaryReport_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}