using System.Collections.Generic;

namespace Irudd.Todo.Data
{
    public class TodoCategory
    {
        public NormalizedString Text { get; set; }
        public bool IsFocused { get; set; }
        public bool IsDefaultCategory { get; set; }
        public List<TodoItem> Items { get; set; }
    }
}
