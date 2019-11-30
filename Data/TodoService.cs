using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Irudd.Todo.Data
{
    public class TodoService
    {
        public static List<TodoCategory> Categories { get; set; }

        public Task<List<TodoCategory>> GetCategories(List<TodoCategory> clientCategories)
        {
            if (Categories == null)
            {
                if (clientCategories == null)
                {
                    Categories = new List<TodoCategory>
                    {
                        new TodoCategory { Text = "Todo", IsFocused = true, IsDefaultCategory = true, Items = new List<TodoItem>() }
                    };
                }
                else
                {
                    Categories = clientCategories;
                }

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

            TriggerOnChange();
        }

        public async Task SetIsDone(NormalizedString categoryText, NormalizedString itemText, bool isDone)
        {
            var i = await GetItem(categoryText, itemText);
            if (i == null)
                return;

            i.IsDone = isDone;

            TriggerOnChange();
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
            await SetIsCategoryFocused(newCategory.Text, true);

            TriggerOnChange();
        }

        public async Task SetIsCategoryFocused(NormalizedString categoryText, bool isFocused)
        {
            Categories.ForEach(x => x.IsFocused = false);

            if (isFocused)
            {
                var category = await GetCategory(categoryText);
                if (category != null)
                {
                    category.IsFocused = true;
                }
            }

            TriggerOnChange();
        }

        public async Task AddItem(NormalizedString itemText, NormalizedString categoryText = null)
        {
            TodoCategory d = null;
            if (categoryText != null)
                d = await GetCategory(categoryText);

            if (d == null)
            {
                d = Categories.Single(x => x.IsDefaultCategory);
            }
            var i = d.Items.FirstOrDefault(x => x.Text == itemText);
            if (i == null)
            {
                i = new TodoItem { Text = itemText };
                d.Items.Add(i);
            }

            TriggerOnChange();
        }

        public async Task RemoveDone(List<Tuple<NormalizedString, NormalizedString>> categoryAndItemTexts)
        {
            if (categoryAndItemTexts == null)
            {
                return;
            }
            var wasChanged = false;
            foreach (var c in categoryAndItemTexts.GroupBy(x => x.Item1))
            {
                var category = await GetCategory(c.Key);
                if (category != null)
                {
                    foreach (var itemText in c)
                    {
                        var item = await GetItem(category.Text, itemText.Item2);
                        if (item != null)
                        {
                            category.Items.Remove(item);
                            wasChanged = true;
                        }
                    }
                }
            }

            if (wasChanged)
            {
                TriggerOnChange();
            }
        }

        private bool Eq(string s1, string s2)
        {
            return (s1 ?? "").ToLowerInvariant().Trim() == (s2 ?? "").ToLowerInvariant().Trim();
        }

        public event Action OnChange;

        public void TriggerOnChange()
        {
            OnChange?.Invoke();
        }
    }
}
