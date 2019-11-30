using System;
using System.Diagnostics.CodeAnalysis;

namespace Irudd.Todo.Data
{
    public class TodoItem : IEquatable<TodoItem>
    {
        public NormalizedString Text { get; set; }
        public bool IsDone { get; set; }

        public bool Equals([AllowNull] TodoItem other)
        {
            return other == null ? false : Text.Equals(other.Text);
        }

        public override int GetHashCode()
        {
            return Text.GetHashCode();
        }
    }
}
