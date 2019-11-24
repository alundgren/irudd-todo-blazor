using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Irudd.Todo.Data
{
    public class TodoService
    {
        public static List<TodoCategory> Categories { get; set; }

        public Task<List<TodoCategory>> GetCategoriesAsync()
        {
            if (Categories == null)
            {
                Categories = new List<TodoCategory>
                {
                    new TodoCategory { Text = "Todo", IsFocused = true, IsDefaultCategory = true, Items = new List<TodoItem>() { new TodoItem { Text = "test" }, new TodoItem { Text = "test2" } } }
                };
            }
            return Task.FromResult(Categories);
        }

        public async Task ConvertToItem(TodoCategory category)
        {
            if (category.IsDefaultCategory)
            {
                return;
            }

            //Remove the category
            Categories.Remove(category);

            //Add the item to the generic category
            await AddItem(category.Text, null);
        }

        public async Task ConvertToCategory(TodoCategory category, TodoItem item)
        {
            var d = Categories.Single(x => x.IsDefaultCategory);
            if (Eq(item.Text, d.Text))
                return;
            category.Items.Remove(item);
            var newCategory = new TodoCategory { Text = item.Text, Items = new List<TodoItem>() };
            Categories.Add(newCategory);
            await FocusCategory(newCategory);
        }

        public async Task FocusCategory(TodoCategory category)
        {
            Categories.ForEach(x => x.IsFocused = false);
            category.IsFocused = true;
        }

        public async Task<TodoItem> AddItem(string text, string category = null)
        {
            var d = Categories.FirstOrDefault(x => Eq(x.Text, category));
            if (d == null)
            {
                d = Categories.Single(x => x.IsDefaultCategory);
            }
            var i = new TodoItem { Text = NormalizeText(text) };
            d.Items.Add(i);
            return i;
        }

        private string NormalizeText(string text)
        {
            //TODO: Remove newlines and meta characters that might cause issues with the textfile
            return text;
        }

        private bool Eq(string s1, string s2)
        {
            return (s1 ?? "").ToLowerInvariant().Trim() == (s2 ?? "").ToLowerInvariant().Trim();
        }

        public event Action RefreshRequested;

        public void CallRequestRefresh()
        {
            RefreshRequested?.Invoke();
        }
    }
}
