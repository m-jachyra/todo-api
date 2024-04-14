using ToDo.Models;

namespace ToDo.Data;

public class ToDoContext
{
    private static int _idSequence  = 0;
    public List<ToDoItem> ToDoItems { get; } = [];
    public List<CloudEvent> Events { get; set; } = [];
    
    public ToDoItem AddToDoItem(ToDoItemModel model)
    {
        var item = new ToDoItem()
        {
            Id = _idSequence++,
            Name = model.Name
        };
        
        ToDoItems.Add(item);

        return item;
    }
    
    public void AddEvent(ToDoItem toDoItem)
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
        
        Events.Add(newEvent);
        
        var eventsToDelete = Events.Where(x => x.Subject == newEvent.Subject && x.Id != newEvent.Id);
        foreach (var @event in eventsToDelete)
        {
            Events.Remove(@event);
        }
    }
}