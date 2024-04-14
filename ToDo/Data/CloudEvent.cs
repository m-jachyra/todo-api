namespace ToDo.Data;

public class CloudEvent
{
    public string Id { get; set; }
    public string Type { get; set; }
    public Uri Source { get; set; }
    public DateTimeOffset Time { get; set; }
    public string DataContentType { get; set; }
    public int Subject { get; set; }
    public ToDoItem Data { get; set; }
}