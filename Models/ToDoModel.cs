namespace ToDoList.Models;

public class ToDoModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
}

// Два методи в контролері Edit - HttpPOSt HttpGEt