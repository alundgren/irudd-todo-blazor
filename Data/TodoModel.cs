using System;
using System.Collections.Generic;

namespace Irudd.Todo.Data
{
    public class TodoItem
    {
        public string Text { get; set; }
        public bool IsDone { get; set; }
    }

    public class TodoCategory
    {
        public string Text { get; set; }
        public bool IsFocused { get; set; }
        public bool IsDefaultCategory { get; set; }
        public List<TodoItem> Items { get; set; }
    }
}
