using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPP.WebApi.Adapters;
using RPP.WebApi.Models.BindingModels;

namespace RPP.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class WorkersController : ControllerBase
{
    private readonly WorkerAdapter _adapter;

    public WorkersController(WorkerAdapter adapter)
    {
        _adapter = adapter;
    }

    [HttpGet]
    public IActionResult GetAll([FromQuery] bool onlyActive = true)
    {
        return _adapter.GetAll(onlyActive).GetResponse(Request, Response);
    }

    [HttpGet("{data}")]
    public IActionResult GetByData(string data)
    {
        return _adapter.GetByData(data).GetResponse(Request, Response);
    }

    [HttpPost]
    public IActionResult Create([FromBody] WorkerBindingModel model)
    {
        return _adapter.Create(model).GetResponse(Request, Response);
    }

    [HttpPut]
    public IActionResult Update([FromBody] WorkerBindingModel model)
    {
        return _adapter.Update(model).GetResponse(Request, Response);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        return _adapter.Delete(id).GetResponse(Request, Response);
    }
}