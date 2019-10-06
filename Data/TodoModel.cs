using System;
using System.Collections.Generic;

namespace Irudd.Todo.Data
{
    public class TodoModel
    {
        public List<TodoItem> Items { get; set; }
    }

    public class TodoItem 
    {
        public string Text { get; set; }
        public bool IsCategory { get; set; }
        public DateTime? DoneDate { get; set; }
        public DateTime? RemovedDate { get; set; }
    }
}
