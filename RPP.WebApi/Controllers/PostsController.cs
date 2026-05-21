using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPP.WebApi.Adapters;
using RPP.WebApi.Models.BindingModels;

namespace RPP.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PostsController : ControllerBase
{
    private readonly PostAdapter _adapter;

    public PostsController(PostAdapter adapter)
    {
        _adapter = adapter;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return _adapter.GetAll().GetResponse(Request, Response);
    }

    [HttpGet("history/{id}")]
    public IActionResult GetHistory(string id)
    {
        return _adapter.GetHistory(id).GetResponse(Request, Response);
    }

    [HttpGet("{data}")]
    public IActionResult GetByData(string data)
    {
        return _adapter.GetByData(data).GetResponse(Request, Response);
    }

    [HttpPost]
    public IActionResult Create([FromBody] PostBindingModel model)
    {
        return _adapter.Create(model).GetResponse(Request, Response);
    }

    [HttpPut]
    public IActionResult Update([FromBody] PostBindingModel model)
    {
        return _adapter.Update(model).GetResponse(Request, Response);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        return _adapter.Delete(id).GetResponse(Request, Response);
    }

    [HttpPatch("restore/{id}")]
    public IActionResult Restore(string id)
    {
        return _adapter.Restore(id).GetResponse(Request, Response);
    }
}