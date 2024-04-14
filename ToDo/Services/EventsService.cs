using System.Diagnostics;
using System.Linq.Expressions;
using ToDo.Data;

namespace ToDo.Services;

public class EventsService
{
    private readonly ToDoContext _context;
    
    public EventsService(ToDoContext context)
    {
        _context = context;
    }
    
    public List<CloudEvent> GetFeed(string? lastEventId, int? timeout)
    {
        timeout ??= 5000;
        var timer = new Stopwatch();
        var lastEvent = _context.Events.Find(x => x.Id == lastEventId);
        
        timer.Start();
        var events = new List<CloudEvent>();
        
        while (timer.ElapsedMilliseconds < timeout)
        {
            if (lastEvent == null)
                events = _context.Events;
            else
                events = _context.Events.Where(x => x.Time > lastEvent.Time).ToList();

            if (events.Any())
                return events;

            Task.Delay(50);
        }
        
        return null;
    }
    
    public CloudEvent AddEvent(ToDoItem toDoItem)
    {
        var newEvent = new CloudEvent()
        {
            Id = Guid.NewGuid().ToString(),
            Type = "todoitem",
            Source = new Uri("https://localhost:5432"),
            Time = DateTimeOffset.UtcNow,
            DataContentType = "text/plain",
            Subject = toDoItem.Id,
            Data = toDoItem
        };
        
        _context.Events.Add(newEvent);

        return newEvent;
    }

    public void RunCompaction(CloudEvent newEvent)
    {
        var eventsToDelete = _context.Events.Where(x => x.Subject == newEvent.Subject && x.Id != newEvent.Id);
        
        foreach (var item in eventsToDelete)
        {
            _context.Events.Remove(item);
        }
    }
    
    public void RunCompaction(int subject)
    {
        var eventsToDelete = _context.Events.Where(x => x.Subject == subject);
        
        foreach (var item in eventsToDelete)
        {
            _context.Events.Remove(item);
        }
    }
}