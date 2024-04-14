using ToDo.Data;
using ToDo.Models;

namespace ToDo.Services;

public class ToDoService
{
    private readonly ToDoContext _context;
    
    public ToDoService(ToDoContext context)
    {
        _context = context;
    }
    
    public List<ToDoItem> Get()
    {
        return _context.ToDoItems;
    }

    public ToDoItem Create(ToDoItemModel model)
    {
        return _context.AddToDoItem(model);
    }

    public ToDoItem Update(int id, ToDoItemModel model)
    {
        var entity = _context.ToDoItems.Find(x => x.Id == id);

        if (entity == null)
            return null;
        
        entity.Name = model.Name;

        return entity;
    }

    public bool Delete(int id)
    {
        var entity = _context.ToDoItems.Find(x => x.Id == id);
        
        if (entity == null)
            return false;
        
        _context.ToDoItems.Remove(entity);
        
        return true;
    }
}