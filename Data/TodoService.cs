using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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

        public async Task<TodoCategory> GetCategory(NormalizedString categoryText)
        {
            return await Task.FromResult(Categories.FirstOrDefault(x => x.Text.Equals(categoryText)));
        }

        public async Task<TodoItem> GetItem(NormalizedString categoryText, NormalizedString itemText)
        {
            var category = await GetCategory(categoryText);

            return await Task.FromResult(category.Items.FirstOrDefault(x => x.Text.Equals(itemText)));
        }

        public async Task ConvertToItem(NormalizedString categoryText)
        {
            var category = await GetCategory(categoryText);

            if (category.IsDefaultCategory)
            {
                return;
            }

            //Remove the category
            Categories.Remove(category);

            //Add the item to the generic category
            await AddItem(category.Text, null);
        }

        public async Task ConvertItemToCategory(NormalizedString categoryText, NormalizedString itemText)
        {
            var d = await GetCategory(itemText);
            if (d != null)
                return;

            var category = await GetCategory(categoryText);
            var item = await GetItem(categoryText, itemText);
            category.Items.Remove(item);
            var newCategory = new TodoCategory { Text = item.Text, Items = new List<TodoItem>() };
            Categories.Add(newCategory);
            await FocusCategory(newCategory.Text);
        }

        public async Task FocusCategory(NormalizedString categoryText)
        {
            var category = await GetCategory(categoryText);
            Categories.ForEach(x => x.IsFocused = false);
            category.IsFocused = true;
        }

        public async Task<TodoItem> AddItem(NormalizedString itemText, NormalizedString categoryText = null)
        {
            TodoCategory d = null;
            if (categoryText != null)
                d = await GetCategory(categoryText);

            if (d == null)
            {
                d = Categories.Single(x => x.IsDefaultCategory);
            }
            var i = new TodoItem { Text = itemText };
            d.Items.Add(i);
            return i;
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

    public class NormalizedString : IEquatable<NormalizedString>
    {
        private readonly string s;
        public NormalizedString(string s)
        {
            this.s = Normalize(s);
        }

        public static string Normalize(string s)
        {
            return s?.Trim()?.Replace("\r", "").Replace("\n", "").Replace("[", "(").Replace("]", ")");
        }

        public bool Equals([AllowNull] NormalizedString other)
        {
            if (other == null)
                return false;
            return other.s.Equals(s, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return this.s.GetHashCode();
        }

        public override string ToString()
        {
            return this.s;
        }

        public static implicit operator NormalizedString(string s) => new NormalizedString(s);
    }
}
