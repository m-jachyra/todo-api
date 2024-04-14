using Microsoft.AspNetCore.Mvc;
using ToDo.Data;
using ToDo.Models;
using ToDo.Services;

namespace ToDo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ToDoController : ControllerBase
{
    private readonly ToDoService _toDoService;
    private readonly EventsService _eventsService;
    
    public ToDoController(ToDoService toDoService, EventsService eventsService)
    {
        _toDoService = toDoService;
        _eventsService = eventsService;
    }
    
    [HttpGet("feed")]
    public ActionResult<List<ToDoItem>> GetFeed([FromQuery] string? lastEventId, [FromQuery] int? timeout)
    {
        var result = _eventsService.GetFeed(lastEventId, timeout);

        return Ok(result);
    }
    
    [HttpGet]
    public ActionResult<List<ToDoItem>> Get()
    {
        var result = _toDoService.Get();
        return Ok(result);
    }
    
    [HttpPost]
    public ActionResult<List<ToDoItem>> Create([FromBody] ToDoItemModel model)
    {
        var result = _toDoService.Create(model);
        var eventResult = _eventsService.AddEvent(result);
        _eventsService.RunCompaction(eventResult);
        
        return Ok();
    }
    
    [HttpPut("{id:int}")]
    public ActionResult<List<ToDoItem>> Update([FromRoute] int id, [FromBody] ToDoItemModel model)
    {
        var result = _toDoService.Update(id, model);

        if (result == null) 
            return BadRequest();

        var eventResult = _eventsService.AddEvent(result);
        _eventsService.RunCompaction(eventResult);
        
        return Ok();
    }
    
    [HttpDelete("{id:int}")]
    public ActionResult<List<ToDoItem>> Delete([FromRoute] int id)
    {
        var result = _toDoService.Delete(id);
        
        if (!result) 
            return BadRequest();
        
        _eventsService.RunCompaction(id);
        
        return Ok();
    }
}