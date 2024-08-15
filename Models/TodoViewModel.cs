namespace ToDoList.Models;

public class TodoViewModel
{
    public List<ToDo> TodoList { get; set; } = new List<ToDo>();

    public ToDo? ToDo { get; set; }
}
