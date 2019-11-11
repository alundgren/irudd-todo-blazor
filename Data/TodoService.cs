using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Irudd.Todo.Data
{
    public class TodoService
    {
        public Task<TodoModel> GetTodoModelAsync()
        {
            return Task.FromResult(new TodoModel 
            {
                Items = Enumerable.Range(0, 10).Select(x => new TodoItem 
                {
                    Text = $"Item {x}",
                    IsFocused = x % 5 == 0,
                    IsCategory = x % 5 == 0
                }).ToList()
            });
        }

        public event Action RefreshRequested;
        
        public void CallRequestRefresh()
    {
         RefreshRequested?.Invoke();
    }
    }
}
