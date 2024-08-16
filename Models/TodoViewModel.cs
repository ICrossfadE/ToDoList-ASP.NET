namespace ToDoList.Models;

public class TodoViewModel
{
    public List<ToDo> TodoList { get; set; } = [];

    public ToDo? ToDo { get; set; }
}
